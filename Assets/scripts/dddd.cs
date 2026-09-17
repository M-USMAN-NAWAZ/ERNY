/* 
*   NatCorder
*   Copyright (c) 2020 Yusuf Olokoba
*/

namespace NatSuite.Examples
{

    using UnityEngine;
    using System.Collections;
    //using Recorders;
    //using Recorders.Clocks;
    //using Recorders.Inputs;
    //using NatSuite.Sharing;
    using SilverTau.NSR;
    using System.IO;
    using SilverTau.NSR.Core;

    public class dddd : MonoBehaviour
    {
        [Header(@"Recording")]
        public int videoWidth = 1280;
        public int videoHeight = 720;
        public bool recordMicrophone;
        public GameObject one;
        //private MP4Recorder recorder;
        //private CameraInput cameraInput;
        //private AudioInput audioInput;
        private AudioSource microphoneSource;


        private IEnumerator Start()
        {
            // Start microphone
            microphoneSource = gameObject.AddComponent<AudioSource>();
            microphoneSource.mute =
            microphoneSource.loop = true;
            microphoneSource.bypassEffects =
            microphoneSource.bypassListenerEffects = false;
            microphoneSource.clip = Microphone.Start(null, true, 1, AudioSettings.outputSampleRate);
            yield return new WaitUntil(() => Microphone.GetPosition(null) > 0);
            microphoneSource.Play();
        }

        public void StartRecording()
        {
            one.SetActive(false);
            //// Start recording
            //// Start microphone
            //var frameRate = 30;
            //var sampleRate = recordMicrophone ? AudioSettings.outputSampleRate : 0;
            //var channelCount = recordMicrophone ? (int)AudioSettings.speakerMode : 0;
            //var clock = new RealtimeClock();
            //recorder = new MP4Recorder(videoWidth, videoHeight, frameRate, sampleRate, channelCount, audioBitRate: 96_000);
            //// Create recording inputs
            //cameraInput = new CameraInput(recorder, clock, Camera.main);
            //audioInput = recordMicrophone ? new AudioInput(recorder, clock, microphoneSource, true) : null;
            //// Unmute microphone
            //microphoneSource.mute = audioInput == null;
        }
        public async void StopRecording()
        {

            // Mute microphone
            //microphoneSource.mute = true;
            // Stop recording
            //audioInput?.Dispose();
         //   microphoneSource.mute = true;
         //   // Stop recording
         //   audioInput?.Dispose();
         //   cameraInput.Dispose();
         //   var path = await recorder.FinishWriting();
         //   // Playback recording
         //   Debug.Log($"Saved recording to: {path}");
         ////   Handheld.PlayFullScreenMovie($"file://{path}");
         //   //cameraInput.Dispose();
         //   //var path = await recorder.FinishWriting();


         //   var sharePayload = new SharePayload();
         //   sharePayload.AddMedia(path);
         //   sharePayload.Commit();
         //   one.SetActive(false);
        }



        public void TakeScreenShot()
        {
            Debug.Log("Screen Shot Taken");
       //     canvas.SetActive(false);
            StartCoroutine(TakeScreenshotAndSave());
        }
        private IEnumerator TakeScreenshotAndSave()
        {
            yield return new WaitForEndOfFrame();

            Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            ss.Apply();

            // var sharepayload = new SharePayload();
            // sharepayload.AddImage(ss);
            //sharepayload.Commit();

            string path = Path.Combine(Application.temporaryCachePath, "screenshot.png");
            File.WriteAllBytes(path, ss.EncodeToPNG());

            // 4️⃣ Share using NSR
#if PLATFORM_IOS || PLATFORM_ANDROID
            SilverTau.NSR.Utilities.Share.ShareAndroidSetTitle("Share Screenshot");
            SilverTau.NSR.Utilities.Share.ShareItem(path);
#elif PLATFORM_STANDALONE || UNITY_EDITOR
            SilverTau.NSR.OpenInFileBrowser.OpenFileBrowser(path);
#endif

            Destroy(ss);

           /// canvas.SetActive(true);
        }


    }
}