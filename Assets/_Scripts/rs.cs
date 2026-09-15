/* 
*   NatCorder
*   Copyright (c) 2021 Yusuf Olokoba
*/

namespace NatSuite.Examples
{

    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;

    using UnityEngine.Android;

    //  using Recorders;
    ///  using Recorders.Clocks;
    //  using Recorders.Inputs;
    // using NatSuite.Sharing;
    using SilverTau.NSR;
    using System.IO;
    using SilverTau.NSR.Core;

    public class rs : MonoBehaviour
    {



        public GameObject mainscreen;
        [Header(@"Recording")]
        public int videoWidth = 1280;
        public int videoHeight = 720;
        public bool recordMicrophone;
        public Texture tt;
        public GameObject rawimage, bg, newraw, plain;
        //    private MP4Recorder recorder;
        //   private CameraInput cameraInput;
        //   private AudioInput audioInput;
        private AudioSource microphoneSource;

        /* private IEnumerator Start () {
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
        */
        private void OnDestroy()
        {
            // Stop microphone
            microphoneSource.Stop();
            Microphone.End(null);
        }


        void OpenAppSettings()
        {
            string bundleIdentifier = Application.identifier;
            string settingsUrl = string.Format("app-settings:{0}", bundleIdentifier);
            Application.OpenURL(settingsUrl);
        }
        private void Awake()
        {
            nativegalley.imagevalue = 0;
        }
        public void takepicture(int maxSize)
        {
#if UNITY_IOS
            if (Application.HasUserAuthorization(UserAuthorization.WebCam))
            {

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

                        rawimage.GetComponent<RawImage>().texture = ss;
                        //pc
                        //     byte[] bytes = ss.EncodeToPNG();
                        //      File.WriteAllBytes(Application.dataPath + "/../mytest2.png",bytes);


                        ImageCropper.Instance.Show(ss, (bool result, Texture originalImage, Texture2D croppedImage) =>
                            {
                            // Destroy previously cropped texture (if any) to free memory
                            //  Destroy(iamgeton.GetComponent<RawImage>().texture, 5f);

                            // If screenshot was cropped successfully
                            if (result)
                                {
                              //      rawimage.GetComponent<AspectRatioFitter>().aspectRatio = 1f;
                                // Assign cropped texture to the RawImage
                                rawimage.GetComponent<RawImage>().enabled = true;
                                    rawimage.GetComponent<RawImage>().texture = croppedImage;

                                    Vector2 size = newraw.GetComponent<RawImage>().rectTransform.sizeDelta;
                                    if (croppedImage.height <= croppedImage.width)
                                        size = new Vector2(800f, 800f * (croppedImage.height / (float)croppedImage.width));
                                    else
                                        size = new Vector2(800f * (croppedImage.width / (float)croppedImage.height), 800f);
                                    rawimage.GetComponent<RawImage>().rectTransform.sizeDelta = size;

                                    rawimage.GetComponent<RawImage>().enabled = true;
                                    eventgetter.death = (Texture2D)rawimage.GetComponent<RawImage>().texture;
                                    approve();
                                //croppedImageSize.text = "Image size: " + croppedImage.width + ", " + croppedImage.height;
                                mainscreen.SetActive(false);
                                    plain.SetActive(false);
                                }
                                else
                                {
                                    mainscreen.SetActive(false);
                                    plain.SetActive(false);
                                }

                            // Destroy the screenshot as we no longer need it in this case
                            //Destroy(screenshot);
                        },
                                       settings: new ImageCropper.Settings()
                                       {
                                           markTextureNonReadable = false,
                                           ovalSelection = false,
                                           autoZoomEnabled = false,
                                           imageBackground = Color.clear, // transparent background
                                       selectionMinAspectRatio = 1.25f,
                                           selectionMaxAspectRatio = 1.3f

                                       },
                                       croppedImageResizePolicy: (ref int width, ref int height) =>
                                       {
                                       // uncomment lines below to save cropped image at half resolution
                                       //width /= 2;
                                       //height /= 2;
                                   });

                        //    NativeGallery.SaveImageToGallery(ss, "blingAr", "death.png");
                        //           sharepayload.Commit();

                        //              Destroy(ss);
                        ShowUiElements(true);



                    }
                    else
                    {

                        mainscreen.SetActive(false);
                        plain.SetActive(false);


                    }



                }, maxSize);
            }
            else
            {
           /// OpenAppSettings();
              eventgetter.instance.cameranotallowedpopup();

            }
#endif

#if PLATFORM_ANDROID

            NativeGallery.Permission permissionr = NativeGallery.CheckPermission(NativeGallery.PermissionType.Read, NativeGallery.MediaType.Image);

            NativeGallery.Permission permissionw = NativeGallery.CheckPermission(NativeGallery.PermissionType.Write, NativeGallery.MediaType.Image);

            if (Permission.HasUserAuthorizedPermission(Permission.Camera) && (permissionr == NativeGallery.Permission.Granted && permissionw == NativeGallery.Permission.Granted))
            {

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

                        rawimage.GetComponent<RawImage>().texture = ss;
                        //pc
                        //     byte[] bytes = ss.EncodeToPNG();
                        //      File.WriteAllBytes(Application.dataPath + "/../mytest2.png",bytes);


                        ImageCropper.Instance.Show(ss, (bool result, Texture originalImage, Texture2D croppedImage) =>
                        {
                            // Destroy previously cropped texture (if any) to free memory
                            //  Destroy(iamgeton.GetComponent<RawImage>().texture, 5f);

                            // If screenshot was cropped successfully
                            if (result)
                            {
                                //    rawimage.GetComponent<AspectRatioFitter>().aspectRatio = 1f;
                                // Assign cropped texture to the RawImage
                                rawimage.GetComponent<RawImage>().enabled = true;
                                rawimage.GetComponent<RawImage>().texture = croppedImage;

                                Vector2 size = newraw.GetComponent<RawImage>().rectTransform.sizeDelta;
                                if (croppedImage.height <= croppedImage.width)
                                    size = new Vector2(800f, 800f * (croppedImage.height / (float)croppedImage.width));
                                else
                                    size = new Vector2(800f * (croppedImage.width / (float)croppedImage.height), 800f);
                                rawimage.GetComponent<RawImage>().rectTransform.sizeDelta = size;

                                rawimage.GetComponent<RawImage>().enabled = true;
                                eventgetter.death = (Texture2D)rawimage.GetComponent<RawImage>().texture;
                                approve();
                                //croppedImageSize.text = "Image size: " + croppedImage.width + ", " + croppedImage.height;
                                //    mainscreen.SetActive(false);
                                plain.SetActive(false);
                            }
                            else
                            {
                                //  mainscreen.SetActive(false);
                                plain.SetActive(false);
                            }

                            // Destroy the screenshot as we no longer need it in this case
                            //Destroy(screenshot);
                        },
                                       settings: new ImageCropper.Settings()
                                       {
                                           markTextureNonReadable = false,
                                           ovalSelection = false,
                                           autoZoomEnabled = false,
                                           imageBackground = Color.clear, // transparent background
                                           selectionMinAspectRatio = 1.25f,
                                           selectionMaxAspectRatio = 1.3f

                                       },
                                       croppedImageResizePolicy: (ref int width, ref int height) =>
                                       {
                                           // uncomment lines below to save cropped image at half resolution
                                           //width /= 2;
                                           //height /= 2;
                                       });

                        //    NativeGallery.SaveImageToGallery(ss, "blingAr", "death.png");
                        //           sharepayload.Commit();

                        //              Destroy(ss);
                        ShowUiElements(true);



                    }
                    else
                    {

                        mainscreen.SetActive(false);
                        plain.SetActive(false);


                    }



                }, maxSize);
            }
            else
            {
                //Permission.RequestUserPermission(Permission.Camera);
                eventgetter.instance.cameranotallowedpopup();

            }


#endif




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

            Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

            ss.Apply();

            //var sharepayload = new SharePayload();
            //sharepayload.AddImage(ss);

            

            bg.SetActive(true);
            rawimage.SetActive(true);
            float width = Screen.width;
            float height = Screen.height;
            float aspctratio = width / height;

            //   rawimage.GetComponent<AspectRatioFitter>().aspectRatio = aspctratio;
            rawimage.GetComponent<RawImage>().texture = ss;
            //pc
            //     byte[] bytes = ss.EncodeToPNG();
            //      File.WriteAllBytes(Application.dataPath + "/../mytest2.png",bytes);


            ImageCropper.Instance.Show(ss, (bool result, Texture originalImage, Texture2D croppedImage) =>
            {
                // Destroy previously cropped texture (if any) to free memory
                //  Destroy(iamgeton.GetComponent<RawImage>().texture, 5f);
                Texture2D finalImage = ss;
                // If screenshot was cropped successfully
                if (result)
                {
                    finalImage = croppedImage;
                    //  rawimage.GetComponent<AspectRatioFitter>().aspectRatio = 1f;
                    // Assign cropped texture to the RawImage
                    rawimage.GetComponent<RawImage>().enabled = true;
                    rawimage.GetComponent<RawImage>().texture = croppedImage;

                    Vector2 size = newraw.GetComponent<RawImage>().rectTransform.sizeDelta;
                    if (croppedImage.height <= croppedImage.width)
                        size = new Vector2(800f, 800f * (croppedImage.height / (float)croppedImage.width));
                    else
                        size = new Vector2(800f * (croppedImage.width / (float)croppedImage.height), 800f);
                    rawimage.GetComponent<RawImage>().rectTransform.sizeDelta = size;

                    rawimage.GetComponent<RawImage>().enabled = true;
                    //croppedImageSize.text = "Image size: " + croppedImage.width + ", " + croppedImage.height;
                }
                else
                {
                    mainscreen.SetActive(false);
                    plain.SetActive(false);
                }
                // 3?? Save the final image to a temporary file
                string path = Path.Combine(Application.temporaryCachePath, "screenshot.png");
                File.WriteAllBytes(path, finalImage.EncodeToPNG());

                // 4?? Share using NSR
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
                           markTextureNonReadable = false,
                           ovalSelection = false,
                           autoZoomEnabled = false,
                           imageBackground = Color.clear, // transparent background
                           selectionMinAspectRatio = 1.25f,
                           selectionMaxAspectRatio = 1.3f

                       },
                       croppedImageResizePolicy: (ref int width, ref int height) =>
                       {
                           // uncomment lines below to save cropped image at half resolution
                           //width /= 2;
                           //height /= 2;
                       });

            NativeGallery.SaveImageToGallery(ss, "blingAr", "death.png");
            //           sharepayload.Commit();

            //              Destroy(ss);
            ShowUiElements(true);

        }

        public void approve()
        {



            eventgetter.instance.deathtomyself();
            //   bg.SetActive(false);
            rawimage.SetActive(false);
            eventgetter.instance.fromconvertimage();
            //SceneManager.LoadScene("4. MainScreens");

        }

        public void disapprove()
        {
            // bg.SetActive(false);
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