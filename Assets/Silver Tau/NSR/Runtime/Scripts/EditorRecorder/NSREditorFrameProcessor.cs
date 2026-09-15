using System;
using UnityEngine;
using UnityEngine.Events;
using Unity.Collections;
using UnityEngine.Rendering;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Media;
#endif

namespace SilverTau.NSR.Core.Editor
{
    public class NSREditorFrameProcessor 
    {
#if UNITY_EDITOR
        public int TargetWidth { get; private set; }
        public int TargetHeight { get; private set; }
        
        private Texture2D _tempFrameTexture2D;
        private bool _initialized = false;
        
        public bool IsRecording { get; set; }
        public bool IsPaused { get; set; }
        
#if UNITY_2021_2_OR_NEWER
        public VideoTrackEncoderAttributes VideoAttributes { get; private set; }
#else
		public VideoTrackAttributes VideoAttributes { get; private set; }
#endif

        public void Initialize(int width, int height)
        {
            TargetWidth = width;
            TargetHeight = height;

            if (_tempFrameTexture2D == null)
            {
                _tempFrameTexture2D = new Texture2D(TargetWidth, TargetHeight, TextureFormat.RGBA32, false);
            }
            
            _initialized = true;
        }

        
#if UNITY_2021_2_OR_NEWER
        public void InitEncoder(int frameRate, EditorEncoder encoder, VideoBitrateMode bitrateMode, VideoEncodingProfile videoH264EncodingProfile, bool includeAlpha, out string fileFormat)
#else
        public void InitEncoder(int frameRate, EditorEncoder encoder, VideoBitrateMode bitrateMode, bool includeAlpha, out string fileFormat)
#endif
        {
            fileFormat = ".mp4";
			
#if UNITY_2021_2_OR_NEWER
            switch (encoder)
            {
                case EditorEncoder.H264:
                    var h264Attr = new H264EncoderAttributes
                    {
                        gopSize = 25,
                        numConsecutiveBFrames = 2,
                        profile = videoH264EncodingProfile
                    };
					
                    SetupVideoAttributes(new VideoTrackEncoderAttributes(h264Attr)
                    {
                        frameRate = new MediaRational(frameRate),
                        width = (uint)TargetWidth,
                        height = (uint)TargetHeight,
                        includeAlpha = includeAlpha,
                        bitRateMode = bitrateMode
                    });
					
                    fileFormat = ".mp4";
                    break;
                case EditorEncoder.VP8:
                    var vp8Attr = new VP8EncoderAttributes
                    {
                        keyframeDistance = 25
                    };
					
                    SetupVideoAttributes(new VideoTrackEncoderAttributes(vp8Attr)
                    {
                        frameRate = new MediaRational(frameRate),
                        width = (uint)TargetWidth,
                        height = (uint)TargetHeight,
                        includeAlpha = includeAlpha,
                        bitRateMode = bitrateMode
                    });
                    fileFormat = ".webm";
                    break;
                default:
                    break;
            }
#else
			SetupVideoAttributes(new VideoTrackAttributes
            {
                frameRate = new MediaRational(frameRate),
                width = (uint)TargetWidth,
                height = (uint)TargetHeight,
#if UNITY_2018_1_OR_NEWER
                includeAlpha = includeAlpha,
                bitRateMode = bitrateMode
#endif
            });

			fileFormat = ".mp4";
#endif
        }
        
#if UNITY_2021_2_OR_NEWER
        public void SetupVideoAttributes(VideoTrackEncoderAttributes attributes)
        {
            VideoAttributes = attributes;
        }
#else
        public void SetupVideoAttributes(VideoTrackAttributes attributes)
        {
            VideoAttributes = attributes;
        }
#endif
        
        public void CaptureFrame(MediaEncoder mediaEncoder, IntPtr dataPtr, int sizeBytes, UnityAction<Texture> onFrameRender)
        {
            if (!_initialized) return;
            if (!IsRecording) return;
            if (IsPaused) return;

            if (_tempFrameTexture2D == null)
            {
                _tempFrameTexture2D = new Texture2D(TargetWidth, TargetHeight, TextureFormat.RGBA32, false);
            }

            _tempFrameTexture2D.LoadRawTextureData(dataPtr, sizeBytes);
            _tempFrameTexture2D.Apply();

            onFrameRender?.Invoke(_tempFrameTexture2D);
            mediaEncoder?.AddFrame(_tempFrameTexture2D);
        }

        public void Dispose()
        {
            if (_tempFrameTexture2D != null)
            {
                UnityEngine.Object.DestroyImmediate(_tempFrameTexture2D);
            }

            _initialized = false;
            IsRecording = false;
            IsPaused = false;
        }
#endif
    }
}