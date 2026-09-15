using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using SilverTau.NSR.Core;
using SilverTau.NSR.Recorders.Internal;
using UnityEngine;

using SilverTau.NSR.Core.Android;
using SilverTau.NSR.Core.iOS;
using SilverTau.NSR.Core.MacOS;
using SilverTau.NSR.Core.Windows;

#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

namespace SilverTau.NSR.Recorders.Video
{
    [AddComponentMenu("Silver Tau/NSR/Universal Video Recorder")]
    public class UniversalVideoRecorder : UVRecorder
    {
        public static UniversalVideoRecorder Instance { get; set; }
        
        // Recorder Settings
        public AndroidRecorderSettings androidRecorderSettings;
        public iOSRecorderSettings iOSRecorderSettings;
        public MacOSRecorderSettings macOSRecorderSettings;
        public WindowsRecorderSettings windowsRecorderSettings;
        
        #region MonoBehaviour

        private void Awake()
        {
            Instance = this;
        }

        public void Start() { }

        #endregion

        public void Dispose()
        {
            if(NSRVideoRecorder == null) return;
            NSRVideoRecorder.Dispose();
        }
        
        #region Advanced features

        private void OnApplicationPause(bool paused)
        {
            if (!autoPauseResumeRecorder || NSRVideoRecorder == null) return;

            if (paused)
            {
                if (NSRVideoRecorder.GetRecordStatus == RecorderStatus.Recording)
                {
                    PauseVideoRecorder();
                }
            }
            else
            {
                if (NSRVideoRecorder.GetRecordStatus == RecorderStatus.Paused)
                {
                    ResumeVideoRecorder();
                }
            }
        }
        
        #endregion
        
        private void OnEnable()
        {
            if(NSRVideoRecorder == null) return;
            NSRVideoRecorder.Init();
        }

        private void SetUpRecorder()
        {
            NSRVideoRecorder.verifyingBeforeInitialization = verifyingBeforeInitialization;
            NSRVideoRecorder.inputUILayer = inputUILayer;
            
            var settings = NSRVideoRecorder.settings;
            
            settings.cameras = cameras;
            if (useRenderTexture) settings.targetRenderTexture = targetRenderTexture;

            settings.resolutionSettings = new ResolutionSettings
            {
                videoResolution = videoResolution,
                considerScreenResolution = considerScreenResolution,
                useCustomVideoResolution = useCustomVideoResolution,
                customVideoResolution = customVideoResolution
            };

            settings.frameRate = frameRate;
            
            settings.useFrameSkip = useFrameSkip;
            settings.frameSkip = frameSkip;
            
            settings.HDR = HDR;
            settings.videoBitRate = videoBitRate;
            settings.sampleRate = sampleRate;
            settings.channelCount = channelCount;
            settings.audioBitRate = audioBitRate;
            
            settings.recordSeparateAudioFile = recordSeparateAudioFile;
            settings.separateAudioFileFormat = separateAudioFileFormat;
            
            settings.recordAllAudioSources = recordAllAudioSources;
            settings.audioListener = audioListener;
            settings.recordMicrophone = recordMicrophone;
            settings.recordOnlyOneAudioSource = recordOnlyOneAudioSource;
            settings.targetAudioSource = targetAudioSource;
            
            settings.audioReceiverMixer = audioReceiverMixer;
            settings.audioReceivers = audioReceivers;
            
            settings.frameDescriptorSettings.useFrameDescriptorSrgb = useFrameDescriptorSrgb;
            
            NSRVideoRecorder.SetPlatformRecorderSettings(androidRecorderSettings, iOSRecorderSettings, macOSRecorderSettings, windowsRecorderSettings);
            NSRVideoRecorder.SetupRecorder();
        }

        private void OnDisable()
        {
            if(NSRVideoRecorder == null) return;
            NSRVideoRecorder.Dispose();
        }

        private void Update()
        {
            if(NSRVideoRecorder == null) return;
            NSRVideoRecorder.UpdateProcessor();
        }

        #region Video Recorder
        
        /// <summary>
        /// A method that start recording.
        /// </summary>
        public void StartVideoRecorder(string videoFilePath = null, string videoFileName = null, string audioFilePath = null, string audioFileName = null)
        {
            if(NSRVideoRecorder.IsRecording) return;
            
            SetUpRecorder();
            
            if (recordMicrophone)
            {
                StartCoroutine(PrepareMicrophone(error =>
                {
                    if (!string.IsNullOrEmpty(error))
                    {
                        Debug.Log(error);
                        return;
                    }
                    
                    NSRVideoRecorder.StartRecorder(videoFilePath, videoFileName, audioFilePath, audioFileName);
                }));

                return;
            }
            
            NSRVideoRecorder.StartRecorder(videoFilePath, videoFileName, audioFilePath, audioFileName);
        }

        public void StartVideoRecorder() => StartVideoRecorder(null, null, null, null);
        
        private IEnumerator PrepareMicrophone(Action<string> callback)
        {
            if (!recordMicrophone)
            {
                callback?.Invoke(null);
                yield break;
            }

            var error = string.Empty;
            
            NSRVideoRecorder.MicrophoneSource = gameObject.GetComponent<AudioSource>();

            if (NSRVideoRecorder.MicrophoneSource == null)
            {
                NSRVideoRecorder.MicrophoneSource = gameObject.AddComponent<AudioSource>();
            }
        
            NSRVideoRecorder.MicrophoneSource.mute = NSRVideoRecorder.MicrophoneSource.loop = true;
            NSRVideoRecorder.MicrophoneSource.bypassEffects = NSRVideoRecorder.MicrophoneSource.bypassListenerEffects = false;

#if PLATFORM_ANDROID
                if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
                {
                    Permission.RequestUserPermission(Permission.Microphone);
                    yield return new WaitForEndOfFrame();
                    yield return new WaitForSeconds(0.2f);
                }
#endif

#if !NSR_MICROPHONE_DISABLE && !PLATFORM_WEBGL
            try
            {
                NSRVideoRecorder.MicrophoneSource.clip = Microphone.Start(null, true, 10, AudioSettings.outputSampleRate);
            }
            catch (Exception e)
            {
                error = e.ToString();
                callback?.Invoke(e.ToString());
            }
            
            if (!string.IsNullOrEmpty(error)) yield break;
            
            //_microphoneSource.clip = Microphone.Start(null, true, 10, AudioSettings.outputSampleRate);
            if (Microphone.GetPosition(null) == 0)
            {
                yield return new WaitForSeconds(0.5f);
            }
            callback?.Invoke(null);
#else
            callback?.Invoke("Warning: Microphone use is disabled! NSR_MICROPHONE_DISABLE was found. If you plan to use the microphone, remove this symbol from the list.");
#endif
            
            yield return null;
        }
        
        /// <summary>
        /// Coroutine that check Permissions.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CheckPermissions()
        {
            if (!recordMicrophone) yield break;
            
#if PLATFORM_ANDROID
            if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            {
                Permission.RequestUserPermission(Permission.Microphone);
                yield return new WaitForEndOfFrame();
                yield return new WaitForSeconds(0.2f);
            }
#endif
            yield break;
        }
        
        /// <summary>
        /// A method that stops recording.
        /// </summary>
        public async void StopVideoRecorder()
        {
            if(!NSRVideoRecorder.IsRecording) return;
            NSRVideoRecorder.StopRecorder();
            
#if !NSR_MICROPHONE_DISABLE && !PLATFORM_WEBGL
            if (!recordMicrophone) return;
            
            try
            {
                Microphone.End(null);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
#else
            Debug.Log("Warning: Microphone use is disabled! NSR_MICROPHONE_DISABLE was found. If you plan to use the microphone, remove this symbol from the list.");
#endif
            await Task.Delay(1000);
        }

        public void PauseVideoRecorder()
        {
            if(!NSRVideoRecorder.IsRecording) return;
            NSRVideoRecorder.PauseRecorder();
        }

        public void ResumeVideoRecorder()
        {
            if(NSRVideoRecorder.IsRecording) return;
            NSRVideoRecorder.ResumeRecorder();
        }
        
        #endregion
        
        #region Help Methods
        
        /// <summary>
        /// An additional method that searches for the active main camera.
        /// </summary>
        private Camera FindActiveTargetCamera()
        {
            if(cameras.Count == 0) return null;

            foreach (var cam in cameras)
            {
                if(cam == null) continue;
                if(!cam.gameObject.activeSelf) continue;
                return cam;
            }
            
            return null;
        }
        
        /// <summary>
        /// A method that converts RenderTexture and returns Texture2D.
        /// </summary>
        /// <param name="width">Target width.</param>
        /// <param name="height">Target height.</param>
        /// <param name="callback">Callback action.</param>
        public Texture2D CreatePreviewImage(int width = 0, int height = 0, Action callback = null)
        {
            var mainCamera = FindActiveTargetCamera();
            if (mainCamera == null) return null;
            
            if (width <= 0) width = Screen.width;
            if (height <= 0) height = Screen.height;
            
            var renderTexture = !targetRenderTexture ? new RenderTexture(width, height, 24, HDR ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.ARGB32) : targetRenderTexture;
            var rect = new Rect(0, 0, renderTexture.width, renderTexture.height);

            var isLinear = QualitySettings.activeColorSpace == ColorSpace.Linear;
            var previewImage = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false, isLinear);

            
            if (!targetRenderTexture)
            {
                mainCamera.targetTexture = renderTexture;
                mainCamera.Render();
            }
 
            RenderTexture.active = renderTexture;
            previewImage.ReadPixels(rect, 0, 0);
            previewImage.Apply();
            
            if (!targetRenderTexture)
            {
                mainCamera.targetTexture = null;
                Destroy(renderTexture);
                renderTexture = null;
            }
 
            RenderTexture.active = null;
            
            callback?.Invoke();

            return previewImage;
        }
        
        #endregion
        
        #region Additional Mathods

        /// <summary>
        /// Method that performs the preview function.
        /// </summary>
        public void Preview()
        {
            if(NSRVideoRecorder == null) return;
            
            if (string.IsNullOrEmpty(NSRVideoRecorder.VideoOutputPath))
            {
                Debug.Log("VideoOutputPath is empty!");
                return;
            }
            
            StartCoroutine(PlayStreamingVideo(NSRVideoRecorder.VideoOutputPath));
        }
    
        /// <summary>
        /// Coroutine that performs the preview function.
        /// </summary>
        /// <returns></returns>
        private IEnumerator PlayStreamingVideo(string path)
        {
#if !UNITY_STANDALONE && !UNITY_EDITOR
            Handheld.PlayFullScreenMovie($"file://{path}");
#endif
            
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(0.2f);
        }
        
        #endregion
    }
}