
namespace NatSuite.Examples
{
    using System.Collections;
    using UnityEngine;
    //  using Recorders;
    // using Recorders.Clocks;
    //  using Recorders.Inputs;
    //using NatSuite.Sharing;
    using UnityEngine.UI;

    using UnityEngine.Android;
    using System.IO;

    public class eventcamera : MonoBehaviour
    {
        public GameObject panel;
        string ssd;
        public GameObject rawimage, bg;
        [Header(@"Recording")]
        public int videoWidth = 1280;
        public int videoHeight = 720;
        public bool recordMicrophone;
        public GameObject iamgeton;
        public Texture tt;
        //    private MP4Recorder recorder;
        //   private CameraInput cameraInput;
        //   private AudioInput audioInput;
        private AudioSource microphoneSource;

        /*   private IEnumerator Start()
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
           }*/


        void OpenAppSettings()
        {
            string bundleIdentifier = Application.identifier;
            string settingsUrl = string.Format("app-settings:{0}", bundleIdentifier);
            Application.OpenURL(settingsUrl);
        }





        public void TakePicture(int maxSize)
        {
#if UNITY_IOS



            if (Application.HasUserAuthorization(UserAuthorization.WebCam))
            {
                eventgetter.instance.opencamera();
                NativeCamera.Permission permission = NativeCamera.TakePicture((path) =>
                {
                    Debug.Log("Image path: " + path);
                    if (path != null)
                    {
                    // Create a Texture2D from the captured image
                    Texture2D ss = NativeCamera.LoadImageAtPath(path, maxSize);
                        if (ss == null)
                        {
                            eventgetter.instance.closecamera();
                            Debug.Log("Couldn't load texture from " + path);
                            return;
                        }

                        ImageCropper.Instance.Show(ss, (bool result, Texture originalImage, Texture2D croppedImage) =>
                        {
                        // Destroy previously cropped texture (if any) to free memory
                        //  Destroy(iamgeton.GetComponent<RawImage>().texture, 5f);

                        // If screenshot was cropped successfully
                        if (result)
                            {
                              //  iamgeton.GetComponent<AspectRatioFitter>().aspectRatio = 1;
                            // Assign cropped texture to the RawImage
                            iamgeton.GetComponent<RawImage>().enabled = true;
                                iamgeton.GetComponent<RawImage>().texture = croppedImage;

                                Vector2 size = iamgeton.GetComponent<RawImage>().rectTransform.sizeDelta;
                                if (croppedImage.height <= croppedImage.width)
                                    size = new Vector2(400f, 400f * (croppedImage.height / (float)croppedImage.width));
                                else
                                    size = new Vector2(400f * (croppedImage.width / (float)croppedImage.height), 400f);
                                iamgeton.GetComponent<RawImage>().rectTransform.sizeDelta = size;

                                iamgeton.GetComponent<RawImage>().enabled = true;
                                eventgetter.profiletexture = (Texture2D)iamgeton.GetComponent<RawImage>().texture;
                                eventgetter.eventimgetexture = (Texture2D)iamgeton.GetComponent<RawImage>().texture;
                                good();
                                panel.SetActive(false);
                            //croppedImageSize.text = "Image size: " + croppedImage.width + ", " + croppedImage.height;
                        }
                            else
                            {

                                panel.SetActive(false);
                                eventgetter.instance.closecamera();
                            //   bg.SetActive(false);
                            //   iamgeton.SetActive(false);
                        }

                        // Destroy the screenshot as we no longer need it in this case
                        //Destroy(screenshot);
                    },
                               settings: new ImageCropper.Settings()
                               {
                                   ovalSelection = true,
                                   autoZoomEnabled = true,
                                   imageBackground = Color.clear,
                               // transparent background
                               selectionMinAspectRatio = 1,
                                   selectionMaxAspectRatio = 1

                               },
                               croppedImageResizePolicy: (ref int width, ref int height) =>
                               {
                               // uncomment lines below to save cropped image at half resolution
                               //width /= 2;
                               //height /= 2;
                           });
                    //  NativeGallery.SaveImageToGallery(ss, "blingAr", "death.png",ssd);

                //    bg.SetActive(true);
                        iamgeton.SetActive(true);


                        float width = Screen.width;
                        float height = Screen.height;
                        float aspctratio = width / height;

                    //    iamgeton.GetComponent<AspectRatioFitter>().aspectRatio = aspctratio;
                        iamgeton.GetComponent<RawImage>().texture = ss;
                        newnativegallery.imagevalue = 0;
                    //eventgetter.instance.opencamera();

                    //  NativeGallery.SaveImageToGallery(ss, "Bling", "Image.png", (success, path) => ssd = path);//))Debug.Log("Media save result: " + success + " " + path));


                        eventgetter.instance.imagepath = ssd;
                        Debug.Log("death: " + ssd);

                    // eventgetter.instance.deathtomyself();
                    // sharepayload.Commit();

                    //  ShowUiElements(true);

                    // If a procedural texture is not destroyed manually, 
                    // it will only be freed after a scene change
                    ///Destroy(texture, 5f);
                }
                    else
                    {
                        eventgetter.instance.closecamera();
                        panel.SetActive(false);
                    }
                }, maxSize);
            }else
            {
               // OpenAppSettings();

                eventgetter.instance.cameranotallowedpopup();
            }
#endif

#if PLATFORM_ANDROID


            NativeGallery.Permission permissionr = NativeGallery.CheckPermission(NativeGallery.PermissionType.Read, NativeGallery.MediaType.Image);

            NativeGallery.Permission permissionw = NativeGallery.CheckPermission(NativeGallery.PermissionType.Write, NativeGallery.MediaType.Image);
           
            if (Permission.HasUserAuthorizedPermission(Permission.Camera) && (permissionr == NativeGallery.Permission.Granted && permissionw == NativeGallery.Permission.Granted))
            {
                eventgetter.instance.opencamera();
                NativeCamera.Permission permission = NativeCamera.TakePicture((path) =>
                {
                    Debug.Log("Image path: " + path);
                    if (path != null)
                    {
                                // Create a Texture2D from the captured image
                            Texture2D ss = NativeCamera.LoadImageAtPath(path, maxSize);
                        if (ss == null)
                        {
                            eventgetter.instance.closecamera();
                            Debug.Log("Couldn't load texture from " + path);
                            return;
                        }

                        ImageCropper.Instance.Show(ss, (bool result, Texture originalImage, Texture2D croppedImage) =>
                        {
                            // Destroy previously cropped texture (if any) to free memory
                            //  Destroy(iamgeton.GetComponent<RawImage>().texture, 5f);

                            // If screenshot was cropped successfully
                            if (result)
                            {
                                    //      iamgeton.GetComponent<AspectRatioFitter>().aspectRatio = 1;
                                    // Assign cropped texture to the RawImage
                                iamgeton.GetComponent<RawImage>().enabled = true;
                                iamgeton.GetComponent<RawImage>().texture = croppedImage;

                                Vector2 size = iamgeton.GetComponent<RawImage>().rectTransform.sizeDelta;
                                if (croppedImage.height <= croppedImage.width)
                                size = new Vector2(400f, 400f * (croppedImage.height / (float)croppedImage.width));
                                else
                                size = new Vector2(400f * (croppedImage.width / (float)croppedImage.height), 400f);
                                    //     iamgeton.GetComponent<RawImage>().rectTransform.sizeDelta = size;
                                    //
                                    //     iamgeton.GetComponent<RawImage>().enabled = true;
                                eventgetter.profiletexture = (Texture2D)iamgeton.GetComponent<RawImage>().texture;
                                eventgetter.eventimgetexture = (Texture2D)iamgeton.GetComponent<RawImage>().texture;
                                good();
                                panel.SetActive(false);
                                        //croppedImageSize.text = "Image size: " + croppedImage.width + ", " + croppedImage.height;
                            }
                            else
                            {

                                panel.SetActive(false);
                                eventgetter.instance.closecamera();
                                    //   bg.SetActive(false);
                                    //   iamgeton.SetActive(false);
                            }

                                // Destroy the screenshot as we no longer need it in this case
                                //Destroy(screenshot);
                        },
                        settings: new ImageCropper.Settings()
                        {
                            ovalSelection = true,
                            autoZoomEnabled = true,
                            imageBackground = Color.clear,
                                // transparent background
                            selectionMinAspectRatio = 1,
                            selectionMaxAspectRatio = 1

                        },
                        croppedImageResizePolicy: (ref int width, ref int height) =>
                        {
                            // uncomment lines below to save cropped image at half resolution
                            //width /= 2;
                            //height /= 2;
                        });
                                //  NativeGallery.SaveImageToGallery(ss, "blingAr", "death.png",ssd);

                                //    bg.SetActive(true);
                                //      iamgeton.SetActive(true);


                            float width = Screen.width;
                            float height = Screen.height;
                            float aspctratio = width / height;

                                //  iamgeton.GetComponent<AspectRatioFitter>().aspectRatio = aspctratio;
                                //  iamgeton.GetComponent<RawImage>().texture = ss;
                         newnativegallery.imagevalue = 0;
                                //eventgetter.instance.opencamera();

                                //  NativeGallery.SaveImageToGallery(ss, "Bling", "Image.png", (success, path) => ssd = path);//))Debug.Log("Media save result: " + success + " " + path));


                            eventgetter.instance.imagepath = ssd;
                            Debug.Log("death: " + ssd);

                            // eventgetter.instance.deathtomyself();
                            // sharepayload.Commit();

                            //  ShowUiElements(true);

                            // If a procedural texture is not destroyed manually, 
                            // it will only be freed after a scene change
                            ///Destroy(texture, 5f);
                }
                else
                {
                    eventgetter.instance.closecamera();
                    panel.SetActive(false);
                }
                }, maxSize);
            }
            else
            {

                eventgetter.instance.cameranotallowedpopup();
                //    Permission.RequestUserPermission(Permission.Camera);
                //   OpenAppSettings();
                //     eventgetter.instance.cameranotallowedpopup();
            }



#endif


        }

        private void OnDestroy()
        {
            // Stop microphone
            microphoneSource.Stop();
            Microphone.End(null);
        }




        public void TakeScreenShott()
        {
            newnativegallery.imagevalue = 0;
            Debug.Log("Taking Shot");
            ShowUiElements(false);
            StartCoroutine(TakeScreenshotAndSave());
        }
        private IEnumerator TakeScreenshotAndSave()
        {
            yield return new WaitForEndOfFrame();

            Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            ss.Apply();

            //var sharepayload = new SharePayload();
            //sharepayload.AddImage(ss);


            bg.SetActive(true);
            iamgeton.SetActive(true);


            float width = Screen.width;
            float height = Screen.height;
            float aspctratio = width / height;

            iamgeton.GetComponent<AspectRatioFitter>().aspectRatio = aspctratio;
            iamgeton.GetComponent<RawImage>().texture = ss;
            newnativegallery.imagevalue = 0;
            ImageCropper.Instance.Show(ss, (bool result, Texture originalImage, Texture2D croppedImage) =>
            {
                Texture2D finalImage = ss;
                
                // Destroy previously cropped texture (if any) to free memory
                //  Destroy(iamgeton.GetComponent<RawImage>().texture, 5f);

                // If screenshot was cropped successfully
                if (result)
                {
                    finalImage = croppedImage;
                    iamgeton.GetComponent<AspectRatioFitter>().aspectRatio = 1;
                    // Assign cropped texture to the RawImage
                    iamgeton.GetComponent<RawImage>().enabled = true;
                    iamgeton.GetComponent<RawImage>().texture = croppedImage;

                    Vector2 size = iamgeton.GetComponent<RawImage>().rectTransform.sizeDelta;
                    if (croppedImage.height <= croppedImage.width)
                        size = new Vector2(400f, 400f * (croppedImage.height / (float)croppedImage.width));
                    else
                        size = new Vector2(400f * (croppedImage.width / (float)croppedImage.height), 400f);
                    iamgeton.GetComponent<RawImage>().rectTransform.sizeDelta = size;

                    iamgeton.GetComponent<RawImage>().enabled = true;
                    //croppedImageSize.text = "Image size: " + croppedImage.width + ", " + croppedImage.height;
                    eventgetter.profiletexture = ss;
                    eventgetter.eventimgetexture = ss;
                    good();
                    panel.SetActive(false);
                }
                else
                {

                    panel.SetActive(false);
                    iamgeton.SetActive(false);
                }
                bg.SetActive(false);

                string path = Path.Combine(Application.temporaryCachePath, "screenshot.png");
                File.WriteAllBytes(path, finalImage.EncodeToPNG());

                // 4️⃣ Share using NSR
#if PLATFORM_IOS || PLATFORM_ANDROID
                SilverTau.NSR.Utilities.Share.ShareAndroidSetTitle("Share Screenshot");
                SilverTau.NSR.Utilities.Share.ShareItem(path);
#elif PLATFORM_STANDALONE || UNITY_EDITOR
            SilverTau.NSR.OpenInFileBrowser.OpenFileBrowser(path);
#endif
                // Destroy the screenshot as we no longer need it in this case
                //Destroy(screenshot);
            },
            settings: new ImageCropper.Settings()
            {
                ovalSelection = true,
                autoZoomEnabled = true,
                imageBackground = Color.clear,
                // transparent background
                selectionMinAspectRatio = 1,
                selectionMaxAspectRatio = 1

            },
            croppedImageResizePolicy: (ref int width, ref int height) =>
            {
                // uncomment lines below to save cropped image at half resolution
                //width /= 2;
                //height /= 2;
            });
            //  NativeGallery.SaveImageToGallery(ss, "blingAr", "death.png",ssd);

            NativeGallery.SaveImageToGallery(ss, "GalleryTest", "Image.png", (success, path) => ssd = path);//))Debug.Log("Media save result: " + success + " " + path));


            eventgetter.instance.imagepath = ssd;
            Debug.Log("death: " + ssd);

            // eventgetter.instance.deathtomyself();
            // sharepayload.Commit();

            ShowUiElements(true);

        }




        public void good()
        {






            //  bg.SetActive(false);
            rawimage.SetActive(false);
            eventgetter.instance.Toconvertprofileimage();

            eventgetter.instance.sendcameratext();
            eventgetter.instance.closecamera();





        }
        public void disapprove()
        {
            iamgeton.SetActive(false);
            //    bg.SetActive(false);
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