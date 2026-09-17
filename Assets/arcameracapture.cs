

namespace NatSuite.Examples
{

    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;
     // using Recorders;
     //using Recorders.Clocks;
     // using Recorders.Inputs;
    //using NatSuite.Sharing;
    using System.IO;
    using NatSuite.Examples;
	using UnityEngine.Android;

	public class arcameracapture : MonoBehaviour
{


        public GameObject cross;
        [Header(@"Recording")]
        public int videoWidth = 1280;
        public int videoHeight = 720;
        public bool recordMicrophone;
      
       // public GameObject rawimage;
        //private IMediaRecorder  recorder;
        //private CameraInput cameraInput;
        //private AudioInput audioInput;
		public AudioSource microphoneSource;
        public Camera RecordingCamera;
       
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
			//microphoneSource.Stop();
			
		}
		public IEnumerator startmic()
		{
			microphoneSource.mute =
				microphoneSource.loop = true;
			microphoneSource.bypassEffects =
			microphoneSource.bypassListenerEffects = false;
			microphoneSource.clip = Microphone.Start(null, true, 1, AudioSettings.outputSampleRate);
			yield return new WaitUntil(() => Microphone.GetPosition(null) > 0);
			microphoneSource.Play();

		}


		public void startmicrophone()
		{
			StartCoroutine(startmic());


		}


        public void stopmicrophone()
		{
			microphoneSource.Stop();
			
		}
		private void OnDestroy()
        {
            // Stop microphone
            microphoneSource.Stop();
          //  Microphone.End(null);
        }
        private void Awake()
        {

            
            nativegalley.imagevalue = 0;
        }

       
        public void StartRecording()
        {
            //cross.SetActive(false);
            // Start recording
           // ShowUiElements(false);
            var frameRate = 30;
            var sampleRate = recordMicrophone ? AudioSettings.outputSampleRate : 0;
            var channelCount = recordMicrophone ? (int)AudioSettings.speakerMode : 0;
            //var clock = new RealtimeClock();
            //recorder = new MP4Recorder(videoWidth, videoHeight, frameRate, sampleRate, channelCount, audioBitRate: 96_000);
            //// Create recording inputs
            //cameraInput = new CameraInput(recorder, clock, RecordingCamera);
            //audioInput = recordMicrophone ? new AudioInput(recorder, clock, microphoneSource, true) : null;
            //// Unmute microphone
            //microphoneSource.mute = audioInput == null;
        }

        public async void StopRecording()
        {
         //  cross.SetActive(true);
            // Mute microphone
            microphoneSource.mute = true;
            // Stop recording
            //audioInput?.Dispose();
            //cameraInput.Dispose();
            //var path = await recorder.FinishWriting();
            // Playback recording
           // Debug.Log($"Saved recording to: {path}");
            //Handheld.PlayFullScreenMovie($"file://{path}");


            //var sharepayload = new SharePayload();
            //sharepayload.AddMedia(path);
            //sharepayload.Commit();
          //  ShowUiElements(true);
        }

        public void TakeScreenShot()
        {
            nativegalley.imagevalue = 0;
            Debug.Log("Taking Shot");
            ShowUiElements(false);
            StartCoroutine(TakeScreenshotAndSave());
        }
        private IEnumerator TakeScreenshotAndSave()
        {
            yield return new WaitForEndOfFrame();
      
            Texture2D ss = new Texture2D(Screen.width, Screen.height , TextureFormat.RGB24, false);
            ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

            ss.Apply();

            //var sharepayload = new SharePayload();
            //sharepayload.AddImage(ss);
           
         //   rawimage.SetActive(true);
            float width = Screen.width;
            float height = Screen.height;
            float aspctratio = width / height;

            //    rawimage.GetComponent<AspectRatioFitter>().aspectRatio = aspctratio;
            //     rawimage.GetComponent<RawImage>().texture = ss;




            //pc
            //     byte[] bytes = ss.EncodeToPNG();
            //      File.WriteAllBytes(Application.dataPath + "/../mytest2.png",bytes);

            //    NativeGallery.SaveImageToGallery(ss, "blingAr", "death.png");



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
            ShowUiElements(true);

        }

        public void approve()
        {
            if (nativegalley.imagevalue == 0)
            {
             //   eventgetter.death = rawimage.GetComponent<RawImage>().texture;

                eventgetter.instance.deathtomyself();
                SceneManager.LoadScene("4. MainScreens");
            }
        }

        public void disapprove()
        {
        //    rawimage.SetActive(false);
        }


        public GameObject[] UIElements,vruielements;
        void ShowUiElements(bool x)
        {
            if (arhandler.selectar == 1)
            {

                for (int i = 0; i < vruielements.Length; i++)
                {
                    if (i == 2 && x == true && apigetter.artexture != null)
                    {
                        vruielements[i].SetActive(x);
                    }
                    else
                    {
                        vruielements[i].SetActive(x);

                    }
                }

            }
            if (arhandler.selectar == 0)
            {
                for (int i = 0; i < UIElements.Length; i++)
                {
                    if (i == 2 && x == true && apigetter.artexture != null)
                    {
                        UIElements[i].SetActive(x);
                    }
                    else
                    {
                        UIElements[i].SetActive(x);
                    }
                }
              

            }
        }
    }
}
