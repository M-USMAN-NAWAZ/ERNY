Shader "Generated 1"
{
    Properties
    {
        _BaseMap
        (
            "Texture",
            2D
        ) = "white" {}


        _BaseColor
        (
            "Color",
            Color
        ) = (1,1,1,1)


        // Give the real person a small advantage.
        //
        // 0.02 = approximately 2 cm.
        _OcclusionBias
        (
            "Occlusion Bias (Meters)",
            Range(0.0, 0.10)
        ) = 0.02


        // Controls how much depth difference is blended
        // instead of immediately cutting.
        _DepthFeather
        (
            "Depth Feather (Meters)",
            Range(0.001, 0.10)
        ) = 0.015


        // Distance between neighboring TrueDepth samples.
        //
        // Higher value expands the real-person mask.
        _SampleRadius
        (
            "Edge Expansion",
            Range(0.5, 3.0)
        ) = 1.25


        // Lower value = person mask becomes slightly larger.
        //
        // Higher = tighter edge.
        _EdgeThreshold
        (
            "Edge Threshold",
            Range(0.01, 0.50)
        ) = 0.14


        // Spatial feathering of the silhouette.
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

        }


        Pass
        {
            Name "TrueDepthOcclusion"


            Cull Off

            ZWrite Off

            ZTest LEqual


            Blend
                SrcAlpha
                OneMinusSrcAlpha


            CGPROGRAM


            #pragma vertex Vert
            #pragma fragment Frag


            #include "UnityCG.cginc"


            // ====================================================
            // Vertex data
            // ====================================================

            struct Attributes
            {
                float4 vertex : POSITION;

                float2 uv : TEXCOORD0;
            };


            struct Varyings
            {
                float4 positionCS : SV_POSITION;

                float3 positionWS : TEXCOORD0;

                float2 uv : TEXCOORD1;

                float4 screenPos : TEXCOORD2;
            };


            // ====================================================
            // Textures
            // ====================================================

            sampler2D _BaseMap;

            sampler2D _TrueDepthTexture;


            // ====================================================
            // Material properties
            // ====================================================

            float4 _BaseMap_ST;

            float4 _BaseColor;

            float _OcclusionBias;

            float _DepthFeather;

            float _SampleRadius;

            float _EdgeThreshold;

            float _EdgeFeather;

            float _MaxDepth;


            // ====================================================
            // Values supplied globally by TrueDepthManager.cs
            // ====================================================

            float4x4 _TrueDepthDisplayMatrix;

            float4 _TrueDepthTexelSize;

            float _TrueDepthReady;


            // ====================================================
            // Vertex
            // ====================================================

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


                return output;
            }


            // ====================================================
            // Get amount of occlusion from one TrueDepth sample.
            //
            // 0 = virtual quad is visible
            // 1 = real object is in front
            // ====================================================

            float GetOcclusion(
                float2 depthUV,
                float virtualDepth)
            {
                depthUV =
                    saturate(depthUV);


                float realDepth =
                    tex2D(
                        _TrueDepthTexture,
                        depthUV
                    ).r;


                // -----------------------------------------------
                // Reject invalid / unreasonable depth values
                // -----------------------------------------------

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


                // -----------------------------------------------
                // Difference:
                //
                // negative:
                // real object is in front
                //
                // positive:
                // virtual object is in front
                // -----------------------------------------------

                float depthDifference =
                    realDepth
                    - virtualDepth
                    - _OcclusionBias;


                // -----------------------------------------------
                // Instead of a hard 0/1 cut, softly transition
                // in depth.
                // -----------------------------------------------

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


            // ====================================================
            // Fragment
            // ====================================================

            half4 Frag(
                Varyings input)
                : SV_Target
            {
                // ================================================
                // Normal quad texture
                // ================================================

                half4 color =
                    tex2D(
                        _BaseMap,
                        input.uv
                    )
                    *
                    _BaseColor;


                // Don't waste time on fully transparent pixels.
                clip(
                    color.a -
                    0.001
                );


                // ================================================
                // If TrueDepth isn't available yet,
                // just draw normally.
                // ================================================

                if (_TrueDepthReady < 0.5)
                {
                    return color;
                }


                // ================================================
                // Screen position
                // ================================================

                float2 screenUV =
                    input.screenPos.xy /
                    input.screenPos.w;


                // ================================================
                // Align screen UV with ARFoundation camera
                // display orientation.
                // ================================================

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


                // ================================================
                // Don't apply occlusion outside valid camera UVs.
                // ================================================

                if (
                    depthUV.x < 0.0 ||
                    depthUV.x > 1.0 ||
                    depthUV.y < 0.0 ||
                    depthUV.y > 1.0
                )
                {
                    return color;
                }


                // ================================================
                // Convert virtual object position into camera space.
                // ================================================

                float3 viewPosition =
                    mul(
                        UNITY_MATRIX_V,
                        float4(
                            input.positionWS,
                            1.0
                        )
                    ).xyz;


                // Unity camera looks down negative Z.
                float virtualDepth =
                    -viewPosition.z;


                // ================================================
                // Size of neighboring TrueDepth samples.
                // ================================================

                float2 texel =
                    _TrueDepthTexelSize.xy *
                    _SampleRadius;


                // ================================================
                // 3 x 3 TrueDepth sampling
                //
                //          TL     T     TR
                //
                //          L      C      R
                //
                //          BL     B     BR
                //
                // ================================================


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


                // ================================================
                // Weighted blur.
                //
                // Center = strongest
                // Straight neighbors = medium
                // Diagonal neighbors = weakest
                //
                // Total weight:
                //
                // 4 + (2 * 4) + (1 * 4)
                // = 16
                // ================================================

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


                // ================================================
                // Convert our smoothed neighborhood into the
                // final person mask.
                //
                // This removes the pixelated/jagged boundary.
                // ================================================

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


                // ================================================
                // Fade virtual quad where the real person exists.
                // ================================================

                color.a *=
                    1.0 -
                    realPersonMask;


                // Remove extremely transparent fragments.
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
