Shader "Hidden/TrueDepth/MaskFilter"
{
    Properties
    {
        _MainTex(
            "Raw TrueDepth",
            2D
        ) = "black" {}
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


            // ====================================================
            // TRUEDEPTH COVERAGE BORDER
            // ====================================================

            float _EdgeGuard;

            float _EdgeFade;


            // ====================================================
            // DEPTH SAMPLE
            // ====================================================

            float PersonAt(
                float2 uv
            )
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


                // ------------------------------------------------
                // NaN-safe depth test.
                //
                // Invalid sky / invalid depth -> background.
                // ------------------------------------------------

                if (
                    depth > _MinDepth &&
                    depth < _MaxDepth
                )
                {
                    return 1.0;
                }


                return 0.0;
            }


            // ====================================================
            // DISTANCE FROM DEPTH TEXTURE BORDER
            // ====================================================

            float DistanceFromEdge(
                float2 uv
            )
            {
                float left =
                    uv.x;


                float right =
                    1.0 -
                    uv.x;


                float bottom =
                    uv.y;


                float top =
                    1.0 -
                    uv.y;


                return min(
                    min(
                        left,
                        right
                    ),

                    min(
                        bottom,
                        top
                    )
                );
            }


            // ====================================================
            // DEPTH COVERAGE
            //
            // 0 = ignore this area
            // 1 = fully trust this area
            // ====================================================

            float DepthCoverage(
                float2 uv
            )
            {
                float distanceFromEdge =
                    DistanceFromEdge(
                        uv
                    );


                return smoothstep(
                    _EdgeGuard,
                    _EdgeGuard +
                    _EdgeFade,
                    distanceFromEdge
                );
            }


            // ====================================================
            // FRAGMENT
            // ====================================================

            fixed4 frag(
                v2f_img i
            ) : SV_Target
            {
                // ------------------------------------------------
                // Calculate TrueDepth coverage first.
                // ------------------------------------------------

                float coverage =
                    DepthCoverage(
                        i.uv
                    );


                // Completely reject the outer TrueDepth boundary.
                if (
                    coverage <= 0.001
                )
                {
                    return fixed4(
                        0,
                        0,
                        0,
                        1
                    );
                }


                float2 texel =
                    _DepthTexelSize.xy *
                    _SpatialRadius;


                float weightedMask =
                    0.0;


                float totalWeight =
                    0.0;


                float strongestMask =
                    0.0;


                // =================================================
                // 5 x 5 FILTER
                // =================================================

                [unroll]
                for (
                    int y = -2;
                    y <= 2;
                    y++
                )
                {
                    [unroll]
                    for (
                        int x = -2;
                        x <= 2;
                        x++
                    )
                    {
                        float2 sampleUV =
                            i.uv +
                            float2(
                                x,
                                y
                            )
                            *
                            texel;


                        float sampleCoverage =
                            DepthCoverage(
                                sampleUV
                            );


                        float sampleValue =
                            PersonAt(
                                sampleUV
                            )
                            *
                            sampleCoverage;


                        float distanceFromCenter =
                            abs(
                                (float)x
                            )
                            +
                            abs(
                                (float)y
                            );


                        float weight;


                        if (
                            distanceFromCenter < 0.5
                        )
                        {
                            weight =
                                4.0;
                        }
                        else if (
                            distanceFromCenter < 1.5
                        )
                        {
                            weight =
                                3.0;
                        }
                        else if (
                            distanceFromCenter < 2.5
                        )
                        {
                            weight =
                                2.0;
                        }
                        else
                        {
                            weight =
                                1.0;
                        }


                        weightedMask +=
                            sampleValue *
                            weight;


                        totalWeight +=
                            weight;


                        strongestMask =
                            max(
                                strongestMask,
                                sampleValue
                            );
                    }
                }


                weightedMask /=
                    max(
                        totalWeight,
                        0.001
                    );


                // =================================================
                // SPATIAL HOLE FILLING
                // =================================================

                float spatialMask =
                    max(
                        weightedMask,
                        strongestMask *
                        0.32
                    );


                spatialMask =
                    smoothstep(
                        0.10,
                        0.68,
                        spatialMask
                    );


                // ------------------------------------------------
                // IMPORTANT:
                //
                // Force mask to disappear before reaching the
                // rectangular TrueDepth boundary.
                // ------------------------------------------------

                spatialMask *=
                    coverage;


                // =================================================
                // TEMPORAL SMOOTHING
                // =================================================

                float previousMask =
                    tex2D(
                        _PreviousMask,
                        i.uv
                    ).r;


                float temporalSpeed;


                if (
                    spatialMask >
                    previousMask
                )
                {
                    temporalSpeed =
                        _Attack;
                }
                else
                {
                    temporalSpeed =
                        _Release;
                }


                float finalMask =
                    lerp(
                        previousMask,
                        spatialMask,
                        temporalSpeed
                    );


                // ------------------------------------------------
                // IMPORTANT:
                //
                // Temporal history is ALSO prevented from leaking
                // into the depth-texture border.
                // ------------------------------------------------

                finalMask *=
                    coverage;


                finalMask =
                    saturate(
                        finalMask
                    );


                return fixed4(
                    finalMask,
                    finalMask,
                    finalMask,
                    1.0
                );
            }


            ENDCG
        }
    }


    FallBack Off
}