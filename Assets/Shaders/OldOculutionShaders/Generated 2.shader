Shader "Generated 2"
{
    Properties
    {
        _BaseMap
        (
            "Particle Texture",
            2D
        ) = "white" {}


        _BaseColor
        (
            "Material Tint",
            Color
        ) = (1,1,1,1)


        _OcclusionBias
        (
            "Occlusion Bias (Meters)",
            Range(0.0, 0.10)
        ) = 0.02


        _DepthFeather
        (
            "Depth Feather",
            Range(0.001, 0.10)
        ) = 0.015


        _SampleRadius
        (
            "Edge Expansion",
            Range(0.5, 3.0)
        ) = 1.25


        _EdgeThreshold
        (
            "Edge Threshold",
            Range(0.01, 0.50)
        ) = 0.14


        _EdgeFeather
        (
            "Edge Softness",
            Range(0.01, 0.40)
        ) = 0.12


        _MaxDepth
        (
            "Maximum Real Depth",
            Range(0.5, 5.0)
        ) = 3.0
    }


    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"

            "Queue" = "Transparent"

            "IgnoreProjector" = "True"

        }


        Pass
        {
            Name "TrueDepthParticles"


            Cull Off

            ZWrite Off

            ZTest LEqual


            Blend
                SrcAlpha
                OneMinusSrcAlpha


            CGPROGRAM


            #pragma vertex Vert
            #pragma fragment Frag

            #pragma multi_compile_particles


            #include "UnityCG.cginc"


            // ===================================================
            // Particle vertex
            // ===================================================

            struct Attributes
            {
                float4 vertex : POSITION;

                float2 uv : TEXCOORD0;

                // THIS IS IMPORTANT.
                //
                // Unity sends:
                // Start Color
                // Color Over Lifetime
                // particle alpha
                //
                // through this value.
                half4 color : COLOR;
            };


            struct Varyings
            {
                float4 positionCS : SV_POSITION;

                float3 positionWS : TEXCOORD0;

                float2 uv : TEXCOORD1;

                float4 screenPos : TEXCOORD2;

                half4 color : COLOR;
            };


            sampler2D _BaseMap;

            sampler2D _TrueDepthTexture;


            float4 _BaseMap_ST;

            float4 _BaseColor;

            float _OcclusionBias;

            float _DepthFeather;

            float _SampleRadius;

            float _EdgeThreshold;

            float _EdgeFeather;

            float _MaxDepth;


            float4x4 _TrueDepthDisplayMatrix;

            float4 _TrueDepthTexelSize;

            float _TrueDepthReady;


            // ===================================================
            // Vertex Shader
            // ===================================================

            Varyings Vert(
                Attributes input)
            {
                Varyings output;


                float3 worldPosition =
                    mul(
                        unity_ObjectToWorld,
                        input.vertex
                    ).xyz;


                output.positionWS =
                    worldPosition;


                output.positionCS =
                    UnityObjectToClipPos(
                        input.vertex
                    );


                output.screenPos =
                    ComputeScreenPos(
                        output.positionCS
                    );


                output.uv =
                    input.uv *
                    _BaseMap_ST.xy +
                    _BaseMap_ST.zw;


                // Preserve Particle System color.
                output.color =
                    input.color;


                return output;
            }


            // ===================================================
            // TrueDepth sample
            //
            // Returns:
            //
            // 0 = virtual particle is visible
            //
            // 1 = real person/object is in front
            // ===================================================

            float GetOcclusion(
                float2 depthUV,
                float virtualDepth)
            {
                depthUV =
                    saturate(
                        depthUV
                    );


                float realDepth =
                    tex2D(
                        _TrueDepthTexture,
                        depthUV
                    ).r;


                // Valid TrueDepth pixel?
                float validNear =
                    step(
                        0.05,
                        realDepth
                    );


                float validFar =
                    step(
                        realDepth,
                        _MaxDepth
                    );


                float validDepth =
                    validNear *
                    validFar;


                float depthDifference =
                    realDepth
                    - virtualDepth
                    - _OcclusionBias;


                // Smooth transition rather than hard depth cut.
                float visibility =
                    smoothstep(
                        -_DepthFeather,
                        _DepthFeather,
                        depthDifference
                    );


                float occlusion =
                    1.0 -
                    visibility;


                return
                    occlusion *
                    validDepth;
            }


            // ===================================================
            // Fragment Shader
            // ===================================================

            half4 Frag(
                Varyings input)
                : SV_Target
            {
                // ===============================================
                // NORMAL PARTICLE RENDERING
                // ===============================================

                half4 textureColor =
                    tex2D(
                        _BaseMap,
                        input.uv
                    );


                // -----------------------------------------------
                // Final particle color:
                //
                // Particle Texture
                //     ×
                // Particle System Color
                //     ×
                // Material Tint
                //
                // This preserves colorful particles.
                // -----------------------------------------------

                half4 color =
                    textureColor *
                    input.color *
                    _BaseColor;


                // Remove invisible texture regions.
                clip(
                    color.a -
                    0.001
                );


                // ===============================================
                // NO TRUEDEPTH YET
                // ===============================================

                if (_TrueDepthReady < 0.5)
                {
                    return color;
                }


                // ===============================================
                // CURRENT PARTICLE PIXEL ON SCREEN
                // ===============================================

                float2 screenUV =
                    input.screenPos.xy /
                    input.screenPos.w;


                // ===============================================
                // ALIGN WITH AR CAMERA
                // ===============================================

                float4 transformedUV =
                    mul(
                        float4(
                            screenUV.x,
                            screenUV.y,
                            1.0,
                            0.0
                        ),
                        _TrueDepthDisplayMatrix
                    );


                float2 depthUV =
                    transformedUV.xy;


                if (
                    depthUV.x < 0.0 ||
                    depthUV.x > 1.0 ||
                    depthUV.y < 0.0 ||
                    depthUV.y > 1.0
                )
                {
                    return color;
                }


                // ===============================================
                // PARTICLE DEPTH FROM CAMERA
                // ===============================================

                float3 viewPosition =
                    mul(
                        UNITY_MATRIX_V,
                        float4(
                            input.positionWS,
                            1.0
                        )
                    ).xyz;


                float virtualDepth =
                    -viewPosition.z;


                // Don't process something behind the camera.
                if (virtualDepth <= 0.0)
                {
                    return color;
                }


                // ===============================================
                // DEPTH NEIGHBOR SIZE
                // ===============================================

                float2 texel =
                    _TrueDepthTexelSize.xy *
                    _SampleRadius;


                // ===============================================
                // 3 x 3 TRUEDEPTH FILTER
                //
                // TL  T  TR
                //
                // L   C   R
                //
                // BL  B  BR
                // ===============================================


                float occC =
                    GetOcclusion(
                        depthUV,
                        virtualDepth
                    );


                float occL =
                    GetOcclusion(
                        depthUV +
                        float2(
                            -texel.x,
                            0
                        ),
                        virtualDepth
                    );


                float occR =
                    GetOcclusion(
                        depthUV +
                        float2(
                            texel.x,
                            0
                        ),
                        virtualDepth
                    );


                float occT =
                    GetOcclusion(
                        depthUV +
                        float2(
                            0,
                            texel.y
                        ),
                        virtualDepth
                    );


                float occB =
                    GetOcclusion(
                        depthUV +
                        float2(
                            0,
                            -texel.y
                        ),
                        virtualDepth
                    );


                float occTL =
                    GetOcclusion(
                        depthUV +
                        float2(
                            -texel.x,
                            texel.y
                        ),
                        virtualDepth
                    );


                float occTR =
                    GetOcclusion(
                        depthUV +
                        float2(
                            texel.x,
                            texel.y
                        ),
                        virtualDepth
                    );


                float occBL =
                    GetOcclusion(
                        depthUV +
                        float2(
                            -texel.x,
                            -texel.y
                        ),
                        virtualDepth
                    );


                float occBR =
                    GetOcclusion(
                        depthUV +
                        float2(
                            texel.x,
                            -texel.y
                        ),
                        virtualDepth
                    );


                // ===============================================
                // WEIGHTED FILTER
                //
                // Center has strongest weight.
                // ===============================================

                float weightedOcclusion =
                (
                    occC * 4.0

                    +

                    (
                        occL +
                        occR +
                        occT +
                        occB
                    ) * 2.0

                    +

                    (
                        occTL +
                        occTR +
                        occBL +
                        occBR
                    )
                )
                /
                16.0;


                // ===============================================
                // CLEAN / FEATHER THE PERSON EDGE
                // ===============================================

                float edgeMinimum =
                    max(
                        0.0,
                        _EdgeThreshold -
                        _EdgeFeather
                    );


                float edgeMaximum =
                    min(
                        1.0,
                        _EdgeThreshold +
                        _EdgeFeather
                    );


                float realPersonMask =
                    smoothstep(
                        edgeMinimum,
                        edgeMaximum,
                        weightedOcclusion
                    );


                // ===============================================
                // HIDE ONLY THE PARTICLE ALPHA.
                //
                // RGB COLOR IS NOT MODIFIED.
                // ===============================================

                color.a *=
                    1.0 -
                    realPersonMask;


                clip(
                    color.a -
                    0.003
                );


                return color;
            }


            ENDCG
        }
    }
}
