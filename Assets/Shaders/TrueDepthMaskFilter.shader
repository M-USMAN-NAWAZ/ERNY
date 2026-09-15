Shader "Hidden/TrueDepth/MaskFilter"
{
    Properties
    {
        _MainTex ("Depth", 2D) = "black" {}
    }

    SubShader
    {
        Cull Off
        ZWrite Off
        ZTest Always

        Pass
        {
            CGPROGRAM

            #pragma vertex vert_img
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _PreviousMask;

            float4 _DepthTexelSize;

            float _MinDepth;
            float _MaxDepth;

            float _SpatialRadius;

            float _Attack;
            float _Release;


            float PersonAt(float2 uv)
            {
                if (
                    uv.x < 0.0 ||
                    uv.x > 1.0 ||
                    uv.y < 0.0 ||
                    uv.y > 1.0
                )
                {
                    return 0.0;
                }

                float depth =
                    tex2D(
                        _MainTex,
                        uv
                    ).r;

                if (
                    depth <= _MinDepth ||
                    depth >= _MaxDepth
                )
                {
                    return 0.0;
                }

                return 1.0;
            }


            fixed4 frag(v2f_img i) : SV_Target
            {
                float2 t =
                    _DepthTexelSize.xy *
                    _SpatialRadius;


                // ==============================================
                // 5 x 5 FOREGROUND MASK
                // ==============================================

                float mask = 0.0;


                // CENTER
                mask = max(
                    mask,
                    PersonAt(i.uv)
                );


                // ==============================================
                // RING 1
                // ==============================================

                float ring1 = 0.92;


                mask = max(
                    mask,
                    PersonAt(
                        i.uv + float2(-t.x, -t.y)
                    ) * ring1
                );

                mask = max(
                    mask,
                    PersonAt(
                        i.uv + float2(0, -t.y)
                    ) * ring1
                );

                mask = max(
                    mask,
                    PersonAt(
                        i.uv + float2(t.x, -t.y)
                    ) * ring1
                );


                mask = max(
                    mask,
                    PersonAt(
                        i.uv + float2(-t.x, 0)
                    ) * ring1
                );

                mask = max(
                    mask,
                    PersonAt(
                        i.uv + float2(t.x, 0)
                    ) * ring1
                );


                mask = max(
                    mask,
                    PersonAt(
                        i.uv + float2(-t.x, t.y)
                    ) * ring1
                );

                mask = max(
                    mask,
                    PersonAt(
                        i.uv + float2(0, t.y)
                    ) * ring1
                );

                mask = max(
                    mask,
                    PersonAt(
                        i.uv + float2(t.x, t.y)
                    ) * ring1
                );


                // ==============================================
                // RING 2
                // ==============================================

                float ring2 = 0.72;

                float2 t2 =
                    t * 2.0;


                mask = max(
                    mask,
                    PersonAt(
                        i.uv + float2(-t2.x, -t2.y)
                    ) * ring2
                );

                mask = max(
                    mask,
                    PersonAt(
                        i.uv + float2(-t.x, -t2.y)
                    ) * ring2
                );

                mask = max(
                    mask,
                    PersonAt(
                        i.uv + float2(0, -t2.y)
                    ) * ring2
                );

                mask = max(
                    mask,
                    PersonAt(
                        i.uv + float2(t.x, -t2.y)
                    ) * ring2
                );

                mask = max(
                    mask,
                    PersonAt(
                        i.uv + float2(t2.x, -t2.y)
                    ) * ring2
                );


                mask = max(
                    mask,
                    PersonAt(
                        i.uv + float2(-t2.x, -t.y)
                    ) * ring2
                );

                mask = max(
                    mask,
                    PersonAt(
                        i.uv + float2(t2.x, -t.y)
                    ) * ring2
                );


                mask = max(
                    mask,
                    PersonAt(
                        i.uv + float2(-t2.x, 0)
                    ) * ring2
                );

                mask = max(
                    mask,
                    PersonAt(
                        i.uv + float2(t2.x, 0)
                    ) * ring2
                );


                mask = max(
                    mask,
                    PersonAt(
                        i.uv + float2(-t2.x, t.y)
                    ) * ring2
                );

                mask = max(
                    mask,
                    PersonAt(
                        i.uv + float2(t2.x, t.y)
                    ) * ring2
                );


                mask = max(
                    mask,
                    PersonAt(
                        i.uv + float2(-t2.x, t2.y)
                    ) * ring2
                );

                mask = max(
                    mask,
                    PersonAt(
                        i.uv + float2(-t.x, t2.y)
                    ) * ring2
                );

                mask = max(
                    mask,
                    PersonAt(
                        i.uv + float2(0, t2.y)
                    ) * ring2
                );

                mask = max(
                    mask,
                    PersonAt(
                        i.uv + float2(t.x, t2.y)
                    ) * ring2
                );

                mask = max(
                    mask,
                    PersonAt(
                        i.uv + float2(t2.x, t2.y)
                    ) * ring2
                );


                // ==============================================
                // TEMPORAL SMOOTHING
                // ==============================================

                float previous =
                    tex2D(
                        _PreviousMask,
                        i.uv
                    ).r;


                // Foreground appears quickly.
                // Foreground disappears more slowly.
                float smoothing =
                    mask > previous
                    ? _Attack
                    : _Release;


                float filtered =
                    lerp(
                        previous,
                        mask,
                        smoothing
                    );


                return fixed4(
                    filtered,
                    filtered,
                    filtered,
                    1.0
                );
            }

            ENDCG
        }
    }

    FallBack Off
}