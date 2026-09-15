#import <Foundation/Foundation.h>
#import <ARKit/ARKit.h>
#import <AVFoundation/AVFoundation.h>
#import <CoreVideo/CoreVideo.h>

#include <stdlib.h>
#include <string.h>


static ARSession* gSession = nil;

static float* gDepthBuffer = nullptr;

static int gDepthWidth = 0;
static int gDepthHeight = 0;
static int gBufferSize = 0;


extern "C"
{
    void TrueDepth_SetSession(void* session)
    {
        if (session == nullptr)
        {
            gSession = nil;
            return;
        }

        gSession = (__bridge ARSession*)session;
    }


    int TrueDepth_Update()
    {
        if (gSession == nil)
            return 0;


        ARFrame* frame = gSession.currentFrame;

        if (frame == nil)
            return 0;


        AVDepthData* depthData = frame.capturedDepthData;

        if (depthData == nil)
            return 0;


        if (depthData.depthDataType !=
            kCVPixelFormatType_DepthFloat32)
        {
            depthData =
                [depthData
                    depthDataByConvertingToDepthDataType:
                    kCVPixelFormatType_DepthFloat32];
        }


        CVPixelBufferRef depthMap =
            depthData.depthDataMap;


        if (depthMap == nil)
            return 0;


        CVReturn lockResult =
            CVPixelBufferLockBaseAddress(
                depthMap,
                kCVPixelBufferLock_ReadOnly
            );


        if (lockResult != kCVReturnSuccess)
            return 0;


        int width =
            (int)CVPixelBufferGetWidth(depthMap);

        int height =
            (int)CVPixelBufferGetHeight(depthMap);


        size_t bytesPerRow =
            CVPixelBufferGetBytesPerRow(depthMap);


        void* baseAddress =
            CVPixelBufferGetBaseAddress(depthMap);


        if (baseAddress == nullptr ||
            width <= 0 ||
            height <= 0)
        {
            CVPixelBufferUnlockBaseAddress(
                depthMap,
                kCVPixelBufferLock_ReadOnly
            );

            return 0;
        }


        int requiredSize =
            width *
            height *
            (int)sizeof(float);


        if (gDepthBuffer == nullptr ||
            gBufferSize != requiredSize)
        {
            if (gDepthBuffer != nullptr)
            {
                free(gDepthBuffer);

                gDepthBuffer = nullptr;
            }


            gDepthBuffer =
                (float*)malloc(requiredSize);


            if (gDepthBuffer == nullptr)
            {
                gBufferSize = 0;

                CVPixelBufferUnlockBaseAddress(
                    depthMap,
                    kCVPixelBufferLock_ReadOnly
                );

                return 0;
            }


            gBufferSize = requiredSize;
        }


        for (int y = 0; y < height; y++)
        {
            uint8_t* sourceRow =
                (uint8_t*)baseAddress +
                (y * bytesPerRow);


            float* destinationRow =
                gDepthBuffer +
                (y * width);


            memcpy(
                destinationRow,
                sourceRow,
                width * sizeof(float)
            );
        }


        gDepthWidth = width;
        gDepthHeight = height;


        CVPixelBufferUnlockBaseAddress(
            depthMap,
            kCVPixelBufferLock_ReadOnly
        );


        return 1;
    }


    void* TrueDepth_GetBuffer()
    {
        return gDepthBuffer;
    }


    int TrueDepth_GetWidth()
    {
        return gDepthWidth;
    }


    int TrueDepth_GetHeight()
    {
        return gDepthHeight;
    }


    void TrueDepth_Destroy()
    {
        if (gDepthBuffer != nullptr)
        {
            free(gDepthBuffer);

            gDepthBuffer = nullptr;
        }


        gDepthWidth = 0;
        gDepthHeight = 0;
        gBufferSize = 0;

        gSession = nil;
    }
}