#ifndef CORRECTED_TRUE_DEPTH_COMMON_INCLUDED
#define CORRECTED_TRUE_DEPTH_COMMON_INCLUDED


sampler2D _TrueDepthMaskTexture;


float4x4 _TrueDepthDisplayMatrix;


float _TrueDepthMaskReady;

float _TrueDepthMatrixReady;


float _TrueDepthEdgeGuard;

float _TrueDepthEdgeFade;


// ================================================================
// MASK UV BORDER CHECK
// ================================================================

float TrueDepthDistanceFromEdge(
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


// ================================================================
// PERSON MASK
// ================================================================

float CorrectedDepthPersonMask(
    float4 screenPos,
    float3 positionWS
)
{
    if (
        _TrueDepthMaskReady < 0.5
    )
    {
        return 0.0;
    }


    float2 screenUV =
        screenPos.xy /
        screenPos.w;


    float2 maskUV;


    // ------------------------------------------------------------
    // Match the TrueDepth texture to the AR selfie camera.
    // ------------------------------------------------------------

    if (
        _TrueDepthMatrixReady > 0.5
    )
    {
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


        maskUV =
            transformedUV.xy;
    }
    else
    {
        maskUV =
            screenUV;
    }


    // ------------------------------------------------------------
    // Outside actual depth texture.
    // ------------------------------------------------------------

    if (
        maskUV.x < 0.0 ||
        maskUV.x > 1.0 ||
        maskUV.y < 0.0 ||
        maskUV.y > 1.0
    )
    {
        return 0.0;
    }


    // ============================================================
    // SECOND BORDER SAFETY CHECK
    //
    // This is intentionally done again here.
    //
    // Even if bilinear filtering produces a tiny mask value at
    // the border, this removes it.
    // ============================================================

    float edgeDistance =
        TrueDepthDistanceFromEdge(
            maskUV
        );


    if (
        edgeDistance <=
        _TrueDepthEdgeGuard
    )
    {
        return 0.0;
    }


    float edgeCoverage =
        smoothstep(
            _TrueDepthEdgeGuard,
            _TrueDepthEdgeGuard +
            _TrueDepthEdgeFade,
            edgeDistance
        );


    float mask =
        tex2D(
            _TrueDepthMaskTexture,
            maskUV
        ).r;


    mask *=
        edgeCoverage;


    // Gentle final smoothing.
    mask =
        smoothstep(
            0.08,
            0.85,
            mask
        );


    return saturate(
        mask
    );
}


#endif