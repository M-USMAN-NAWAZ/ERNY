#ifndef CORRECTED_TRUE_DEPTH_COMMON_INCLUDED
#define CORRECTED_TRUE_DEPTH_COMMON_INCLUDED


sampler2D _TrueDepthMaskTexture;

float4x4 _TrueDepthDisplayMatrix;

float _TrueDepthMaskReady;

float _TrueDepthMatrixReady;


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


    if (
        _TrueDepthMatrixReady > 0.5
    )
    {
        maskUV =
            mul(
                float4(
                    screenUV.x,
                    screenUV.y,
                    1.0,
                    0.0
                ),
                _TrueDepthDisplayMatrix
            ).xy;
    }
    else
    {
        maskUV =
            screenUV;
    }


    if (
        maskUV.x < 0.0 ||
        maskUV.x > 1.0 ||
        maskUV.y < 0.0 ||
        maskUV.y > 1.0
    )
    {
        return 0.0;
    }


    float mask =
        tex2D(
            _TrueDepthMaskTexture,
            maskUV
        ).r;


    // Additional gentle edge smoothing.
    return smoothstep(
        0.20,
        0.80,
        mask
    );
}


#endif