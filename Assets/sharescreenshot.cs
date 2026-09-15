namespace NatSuite.Examples
{

    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;
    //  using Recorders;
    ///  using Recorders.Clocks;
    //  using Recorders.Inputs;
    //using NatSuite.Sharing;
    using SilverTau.NSR;
    using System.IO;
    using SilverTau.NSR.Core;
	//using NatSuite.Recorders;
	using static System.Net.Mime.MediaTypeNames;

	public class sharescreenshot : MonoBehaviour
	{
		
		public RectTransform textureToCapture;
		public RectTransform textureToCaptureBaduge;
		public int textureWidth = 512; // Texture width
		public int textureHeight = 512;
		public RawImage rawcapture;
		public string savepath = "SavedTexture.png";
		//private IMediaRecorder recorder;
		/*   [Header(@"Recording")]
		   public int videoWidth = 1280;
		   public int videoHeight = 720;
		   public bool recordMicrophone;*/
		public Texture tt;
        public GameObject rawimage;
		public GameObject BadugePanl;
        //    private MP4Recorder recorder;
        //   private CameraInput cameraInput;
        //   private AudioInput audioInput;
        /*private AudioSource microphoneSource;

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

        private void OnDestroy()
        {
            // Stop microphone
            microphoneSource.Stop();
            Microphone.End(null);
        }*/
        private void Awake()
        {
            Debug.Log("charity check");
            nativegalley.imagevalue = 0;
        }

        public GameObject sharebutton,fullscreen;
		public GameObject shareButtonBaduge;
        public void takeshot()
        {   Debug.Log("Taking Shot");
            sharebutton.SetActive(false);
            fullscreen.SetActive(false);

            StartCoroutine(TakeScreenshotAnd());
        }
        private IEnumerator TakeScreenshotAnd()
        {
			yield return new WaitForEndOfFrame();


			Texture2D ss = new Texture2D(Screen.width, Screen.height - 350, TextureFormat.RGB24, false);
			ss.ReadPixels(new Rect(0, 350, Screen.width, Screen.height), 0, 0);

			ss.Apply();

			//var sharepayload = new SharePayload();
			//sharepayload.AddImage(ss);
			// rawimage.SetActive(true);
			float width = Screen.width;
			float height = Screen.height;
			float aspctratio = width / height;

            //    rawimage.GetComponent<AspectRatioFitter>().aspectRatio = aspctratio;
            // rawimage.GetComponent<RawImage>().texture = ss;

            //    rawimage.GetComponent<AspectRatioFitter>().aspectRatio = aspctratio;
            //     rawimage.GetComponent<RawImage>().texture = ss;




            //pc
            //     byte[] bytes = ss.EncodeToPNG();
            //      File.WriteAllBytes(Application.dataPath + "/../mytest2.png",bytes);

            //    NativeGallery.SaveImageToGallery(ss, "blingAr", "death.png");



            //sharepayload.Commit();

            string path = Path.Combine(UnityEngine.Application.temporaryCachePath, "screenshot.png");
            File.WriteAllBytes(path, ss.EncodeToPNG());

            // 4️⃣ Share using NSR
#if PLATFORM_IOS || PLATFORM_ANDROID
            SilverTau.NSR.Utilities.Share.ShareAndroidSetTitle("Share Screenshot");
            SilverTau.NSR.Utilities.Share.ShareItem(path);
#elif PLATFORM_STANDALONE || UNITY_EDITOR
            SilverTau.NSR.OpenInFileBrowser.OpenFileBrowser(path);
#endif

            Destroy(ss);

			sharebutton.SetActive(true);
			//fullscreen.SetActive(true);


		}
		public void TakeScreenShot()
        {
			// nativegalley.imagevalue = 0;
			//  Debug.Log("Taking Shot");
			//  ShowUiElements(false);
		
			StartCoroutine(TakeScreenshotAndSave());
        }
		public void TakeScreenShotBaduge()
		{
			// nativegalley.imagevalue = 0;
			//  Debug.Log("Taking Shot");
			//  ShowUiElements(false);
			sharebutton.SetActive(false);
			if(shareButtonBaduge.gameObject.activeInHierarchy)
            {
				shareButtonBaduge.SetActive(false);
            }
			StartCoroutine(TakeScreenshotAndSaveBaduge());
			
		}
		private IEnumerator TakeScreenshotAndSave()
        {
            yield return new WaitForEndOfFrame();


			//	RenderTexture renderTexture = new RenderTexture(textureToCapture.sizeDelta.x, textureToCapture.texture.height, 0);

			// Set the active RenderTexture
			//	RenderTexture.active = renderTexture;


				Vector3[] corners = new Vector3[4];
				textureToCapture.GetWorldCorners(corners);
				//Remove 100 and you will get error
				int width = ((int)corners[3].x - (int)corners[0].x) ;
				int height = (int)corners[1].y - (int)corners[0].y;
				var startX = corners[0].x;
				var startY = corners[0].y;
				Texture2D ss = new Texture2D(width, height, TextureFormat.RGB24, false);
				ss.ReadPixels(new Rect(startX, startY, width, height), 0, 0);
				


			/*int myInt = (int)textureToCapture.sizeDelta.x;
			int myInth = (int)textureToCapture.sizeDelta.y;


			Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
			ss.ReadPixels(new Rect(myInt, myInth, 0, 0), 0, 0);
			*/
			ss.Apply();
			

			//var sharepayload = new SharePayload();
			//sharepayload.AddImage(ss);
			rawimage.GetComponent<RawImage>().texture = ss;
		
			byte[] textureBytes = ss.EncodeToPNG();

			// Specify the path where you want to save the texture
			string fullPath = Path.Combine(UnityEngine.Application.persistentDataPath, savepath);
			Debug.Log("Texture saved to: " + fullPath);
			File.WriteAllBytes(fullPath, textureBytes);


            //sharepayload.Commit();


            string path = Path.Combine(UnityEngine.Application.temporaryCachePath, "screenshot.png");
            File.WriteAllBytes(path, ss.EncodeToPNG());

            // 4️⃣ Share using NSR
#if PLATFORM_IOS || PLATFORM_ANDROID
            SilverTau.NSR.Utilities.Share.ShareAndroidSetTitle("Share Screenshot");
            SilverTau.NSR.Utilities.Share.ShareItem(path);
#elif PLATFORM_STANDALONE || UNITY_EDITOR
            SilverTau.NSR.OpenInFileBrowser.OpenFileBrowser(path);
#endif


            /*    Texture2D ss = new Texture2D(Screen.width,Screen.height -1100, TextureFormat.RGB24, false);
			   ss.ReadPixels(new Rect(0, 650, Screen.width, Screen.height), 0, 0);
				//Texture2D dd = new Texture2D(ss.width, ss.height - 100, TextureFormat.RGB24, false);
				//	dd.ReadPixels(new Rect(0, -100, ss.width, ss.height), 0, 0);

				ss.Apply();


			   // rawimage.SetActive(true);
				float width = Screen.width;
				float height = Screen.height;
				float aspctratio = width / height;

				//    rawimage.GetComponent<AspectRatioFitter>().aspectRatio = aspctratio;





				//pc
				//     byte[] bytes = ss.EncodeToPNG();
				//      File.WriteAllBytes(Application.dataPath + "/../mytest2.png",bytes);

				// NativeGallery.SaveImageToGallery(ss, "blingAr", "death.png");

				//var path = " D:\\Unity 3D\\Unity Projects\\BlingAR\\";
				//sharepayload.AddMedia(path);

				// Calculate the coordinates for the center rectangle
				//int centerX = Screen.width / 2 - captureSize / 2;
			//	int centerY = Screen.height / 2 - captureSize / 2;
			 
				// Create a texture to hold the captured pixels
				Texture2D texture = new Texture2D(Screen.width, captureSize, TextureFormat.RGB24, false);

				// Read pixels from the center area of the screen
				//texture.ReadPixels(new Rect(0, 550, Screen.width, captureSize), 0, 0);
				//texture.Apply();

				// Do something with the captured pixels
				Color[] pixels = ss.GetPixels();
				Debug.Log("Number of pixels captured: " + pixels.Length);

				var sharepayload = new SharePayload();
				sharepayload.AddImage(ss);
				rawimage.GetComponent<RawImage>().texture = ss;
				byte[] textureBytes = ss.EncodeToPNG();

				// Specify the path where you want to save the texture
				string fullPath = Path.Combine(Application.persistentDataPath, savepath);
				Debug.Log("Texture saved to: " + fullPath);
				File.WriteAllBytes(fullPath, textureBytes);

				sharepayload.Commit();
			   rawimage.SetActive(false);*/
            //              Destroy(ss);
            //    ShowUiElements(true);

        }
        private IEnumerator TakeScreenshotAndSaveBaduge()
        {
            yield return new WaitForEndOfFrame();


			//	RenderTexture renderTexture = new RenderTexture(textureToCapture.sizeDelta.x, textureToCapture.texture.height, 0);

			// Set the active RenderTexture
			//	RenderTexture.active = renderTexture;


				Vector3[] corners = new Vector3[4];
				textureToCaptureBaduge.GetWorldCorners(corners);
				//Remove 100 and you will get error
				int width = ((int)corners[3].x - (int)corners[0].x) ;
				int height = (int)corners[1].y - (int)corners[0].y;
				var startX = corners[0].x;
				var startY = corners[0].y;
				Texture2D ss = new Texture2D(width, height, TextureFormat.RGB24, false);
				ss.ReadPixels(new Rect(startX, startY, width, height), 0, 0);
				


			/*int myInt = (int)textureToCapture.sizeDelta.x;
			int myInth = (int)textureToCapture.sizeDelta.y;


			Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
			ss.ReadPixels(new Rect(myInt, myInth, 0, 0), 0, 0);
			*/
			ss.Apply();
			

			//var sharepayload = new SharePayload();
			//sharepayload.AddImage(ss);
			rawimage.GetComponent<RawImage>().texture = ss;
		
			byte[] textureBytes = ss.EncodeToPNG();

			// Specify the path where you want to save the texture
			string fullPath = Path.Combine(UnityEngine.Application.persistentDataPath, savepath);
			Debug.Log("Texture saved to: " + fullPath);
			File.WriteAllBytes(fullPath, textureBytes);


			//sharepayload.Commit();
			sharebutton.SetActive(true);
			
			if(BadugePanl.activeInHierarchy)
            {
				shareButtonBaduge.SetActive(true);
			}
            string path = Path.Combine(UnityEngine.Application.temporaryCachePath, "screenshot.png");
            File.WriteAllBytes(path, ss.EncodeToPNG());

            // 4️⃣ Share using NSR
#if PLATFORM_IOS || PLATFORM_ANDROID
            SilverTau.NSR.Utilities.Share.ShareAndroidSetTitle("Share Screenshot");
            SilverTau.NSR.Utilities.Share.ShareItem(path);
#elif PLATFORM_STANDALONE || UNITY_EDITOR
            SilverTau.NSR.OpenInFileBrowser.OpenFileBrowser(path);
#endif


            /*    Texture2D ss = new Texture2D(Screen.width,Screen.height -1100, TextureFormat.RGB24, false);
			   ss.ReadPixels(new Rect(0, 650, Screen.width, Screen.height), 0, 0);
				//Texture2D dd = new Texture2D(ss.width, ss.height - 100, TextureFormat.RGB24, false);
				//	dd.ReadPixels(new Rect(0, -100, ss.width, ss.height), 0, 0);

				ss.Apply();


			   // rawimage.SetActive(true);
				float width = Screen.width;
				float height = Screen.height;
				float aspctratio = width / height;

				//    rawimage.GetComponent<AspectRatioFitter>().aspectRatio = aspctratio;





				//pc
				//     byte[] bytes = ss.EncodeToPNG();
				//      File.WriteAllBytes(Application.dataPath + "/../mytest2.png",bytes);

				// NativeGallery.SaveImageToGallery(ss, "blingAr", "death.png");

				//var path = " D:\\Unity 3D\\Unity Projects\\BlingAR\\";
				//sharepayload.AddMedia(path);

				// Calculate the coordinates for the center rectangle
				//int centerX = Screen.width / 2 - captureSize / 2;
			//	int centerY = Screen.height / 2 - captureSize / 2;
			 
				// Create a texture to hold the captured pixels
				Texture2D texture = new Texture2D(Screen.width, captureSize, TextureFormat.RGB24, false);

				// Read pixels from the center area of the screen
				//texture.ReadPixels(new Rect(0, 550, Screen.width, captureSize), 0, 0);
				//texture.Apply();

				// Do something with the captured pixels
				Color[] pixels = ss.GetPixels();
				Debug.Log("Number of pixels captured: " + pixels.Length);

				var sharepayload = new SharePayload();
				sharepayload.AddImage(ss);
				rawimage.GetComponent<RawImage>().texture = ss;
				byte[] textureBytes = ss.EncodeToPNG();

				// Specify the path where you want to save the texture
				string fullPath = Path.Combine(Application.persistentDataPath, savepath);
				Debug.Log("Texture saved to: " + fullPath);
				File.WriteAllBytes(fullPath, textureBytes);

				sharepayload.Commit();
			   rawimage.SetActive(false);*/
            //              Destroy(ss);
            //    ShowUiElements(true);

        }
        public int captureSize = 2080;
		public void approve()
        {
            if (nativegalley.imagevalue == 0)
            {
                eventgetter.death = rawimage.GetComponent<RawImage>().texture;

                eventgetter.instance.deathtomyself();
                SceneManager.LoadScene("4. MainScreens");
            }
        }

        public void disapprove()
        {
            rawimage.SetActive(false);
        }


        public GameObject[] UIElements;
        void ShowUiElements(bool x)
        {
            foreach (GameObject ui in UIElements)
            {
                ui.gameObject.SetActive(x);
            }
        }
    }
}
