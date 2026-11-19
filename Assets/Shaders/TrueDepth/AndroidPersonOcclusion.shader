Shader "Hidden/Erny/AndroidPersonOcclusion"
{
    SubShader
    {
        Tags { "Queue" = "Background+1" "ForceNoShadowCasting" = "True" }

        Pass
        {
            Cull Off
            ZWrite On
            ZTest Always
            ColorMask 0

            CGPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma target 3.0
            #include "UnityCG.cginc"

            sampler2D _AndroidPersonMask;
            float4x4 _AndroidPersonDisplayTransform;
            float2 _AndroidPersonCropScale;
            int _AndroidPersonMaskRotation;
            float _AndroidPersonOccluderDepth;

            struct Varyings
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings Vert(float4 vertex : POSITION)
            {
                Varyings output;
                float2 screenUV = vertex.xy;
                float4 position = mul(UNITY_MATRIX_P, float4(0, 0, -_AndroidPersonOccluderDepth, 1));
                position.xy = (screenUV * 2 - 1) * float2(1, _ProjectionParams.x) * position.w;
                output.position = position;

                // Use the row-vector convention of the ARCore background shader, including its front-camera mirror.
                float2 sensorUV = mul(float4(screenUV, 1, 0), _AndroidPersonDisplayTransform).xy;
                sensorUV = (sensorUV - 0.5) * _AndroidPersonCropScale + 0.5;

                // The Java bitmap is rotated clockwise into the model's upright input coordinates.
                if (_AndroidPersonMaskRotation == 90)
                    sensorUV = float2(1 - sensorUV.y, sensorUV.x);
                else if (_AndroidPersonMaskRotation == 180)
                    sensorUV = 1 - sensorUV;
                else if (_AndroidPersonMaskRotation == 270)
                    sensorUV = float2(sensorUV.y, 1 - sensorUV.x);

                output.uv = sensorUV;
                return output;
            }

            fixed4 Frag(Varyings input) : SV_Target
            {
                float2 edge = min(input.uv, 1 - input.uv);
                clip(min(edge.x, edge.y));
                clip(tex2D(_AndroidPersonMask, input.uv).r - 0.55);
                return 0;
            }
            ENDCG
        }
    }
    Fallback Off
}
