Shader "Custom/Corrected depth particle shader"
{
    Properties
    {
        _MainTex(
            "Particle Texture",
            2D
        ) = "white" {}


        _Color(
            "Material Tint",
            Color
        ) = (1,1,1,1)


        [Header(TrueDepth)]

        _MaskStrength(
            "Person Mask Strength",
            Range(0.5,2.0)
        ) = 1.0
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
            Name "SmoothTrueDepthParticles"


            Cull Off

            ZWrite Off

            ZTest LEqual


            Blend
                SrcAlpha
                OneMinusSrcAlpha


            CGPROGRAM


            #pragma vertex Vert
            #pragma fragment Frag
            #pragma target 3.0
            #pragma multi_compile_particles


            #include "UnityCG.cginc"

            #include "CorrectedDepthCommon.cginc"


            struct Attributes
            {
                float4 vertex :
                    POSITION;


                float2 uv :
                    TEXCOORD0;


                fixed4 color :
                    COLOR;
            };


            struct Varyings
            {
                float4 positionCS :
                    SV_POSITION;


                float3 positionWS :
                    TEXCOORD0;


                float2 uv :
                    TEXCOORD1;


                float4 screenPos :
                    TEXCOORD2;


                fixed4 color :
                    COLOR;
            };


            sampler2D _MainTex;

            float4 _MainTex_ST;

            fixed4 _Color;

            float _MaskStrength;


            Varyings Vert(
                Attributes input
            )
            {
                Varyings output;


                output.positionCS =
                    UnityObjectToClipPos(
                        input.vertex
                    );


                output.positionWS =
                    mul(
                        unity_ObjectToWorld,
                        input.vertex
                    ).xyz;


                output.screenPos =
                    ComputeScreenPos(
                        output.positionCS
                    );


                output.uv =
                    TRANSFORM_TEX(
                        input.uv,
                        _MainTex
                    );


                output.color =
                    input.color;


                return output;
            }


            fixed4 Frag(
                Varyings input
            ) : SV_Target
            {
                fixed4 color =
                    tex2D(
                        _MainTex,
                        input.uv
                    )
                    *
                    input.color
                    *
                    _Color;


                clip(
                    color.a -
                    0.001
                );


                float personMask =
                    CorrectedDepthPersonMask(
                        input.screenPos,
                        input.positionWS
                    );


                personMask =
                    saturate(
                        personMask *
                        _MaskStrength
                    );


                // Keep original RGB.
                // Only visibility is affected.
                color.a *=
                    1.0 -
                    personMask;


                clip(
                    color.a -
                    0.001
                );


                return color;
            }


            ENDCG
        }
    }


    FallBack Off
}