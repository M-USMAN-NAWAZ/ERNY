using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class updatenativegallery : MonoBehaviour
{
	public static int updateimagevalue;
	public Texture gg;
	public GameObject panel;
	public GameObject rawimage,bg;
	public void approve()
	{

		
			updataeven.instance.closecamera();

			//     new Texture2D(rawimage.GetComponent<RawImage>().texture.width,
			Debug.Log("native gallery call");							//   rawimage.GetComponent<RawImage>().texture.height, TextureFormat.RGB24, false, false); ;
			updataeven.instance.sendconverttext();
			//bg.SetActive(false);
			rawimage.SetActive(false);
		
	}


	public void disapprove()
	{

	//	bg.SetActive(false);
		rawimage.SetActive(false);



	}

	void Update()
	{/*
		if (Input.GetMouseButtonDown(0))
		{
			if (Input.mousePosition.x < Screen.width / 3)
			{
				// Take a screenshot and save it to Gallery/Photos
				StartCoroutine(TakeScreenshotAndSave());
			}
			else
			{
				// Don't attempt to pick media from Gallery/Photos if
				// another media pick operation is already in progress
				if (NativeGallery.IsMediaPickerBusy())
					return;

				if (Input.mousePosition.x < Screen.width * 2 / 3)
				{
					// Pick a PNG image from Gallery/Photos
					// If the selected image's width and/or height is greater than 512px, down-scale the image
					PickImage(512);
				}
				else
				{
					// Pick a video from Gallery/Photos
					PickVideo();
				}
			}
		}*/
	}

	private IEnumerator TakeScreenshotAndSave()
	{
		yield return new WaitForEndOfFrame();

		Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		ss.Apply();

		// Save the screenshot to Gallery/Photos
		NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(ss, "GalleryTest", "Image.png", (success, path) => Debug.Log("Media save result: " + success + " " + path));

		Debug.Log("Permission result: " + permission);

		// To avoid memory leaks
		Destroy(ss);
	}

	public void PickImage(int maxSize)
	{


		//updataeven.instance.opencamera();
		NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
		{
			Debug.Log("Image path: " + path);


			//			Path ourpath= new Path(path);




			if (path != null)
			{


				Debug.Log("path exists? " + File.Exists(path));



				// Create Texture from selected image
				Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize, false);
				if (texture == null)
				{
					Debug.Log("Couldn't load texture from " + path);
					return;
				}
				else

				{
					//bool dd = true;
					//texture.isReadable =dd ;
				}
				updateimagevalue = 1;

			//	bg.SetActive(true);
			//	rawimage.SetActive(true);
				float width = Screen.width;
				float height = Screen.height;
				float aspctratio = width / height;
				if (texture.width > Screen.width || texture.width == Screen.width)
				{
					float ratio = texture.width / texture.height;
				//	rawimage.GetComponent<AspectRatioFitter>().aspectMode = AspectRatioFitter.AspectMode.FitInParent;

					//rawimage.GetComponent<AspectRatioFitter>().aspectRatio = ratio;
					rawimage.GetComponent<RawImage>().texture = texture;
					if (ratio == 0)
					{
						ratio = 1f;

					//	rawimage.GetComponent<AspectRatioFitter>().aspectRatio = ratio;
					}
					else
					{
						if (ratio == 0)
						{
							ratio = 1f;

							rawimage.GetComponent<AspectRatioFitter>().aspectRatio = ratio;
						}
					//	rawimage.GetComponent<AspectRatioFitter>().aspectRatio = ratio;
					//	rawimage.GetComponent<RawImage>().texture = texture;
						Debug.Log("not death death");
						//rawimage.GetComponent<RawImage>().texture = texture;
						//rawimage.GetComponent<AspectRatioFitter>().aspectRatio = aspctratio;
						//	rawimage.GetComponent<AspectRatioFitter>().aspectRatio = aspctratio;
						//rawimage.GetComponent<RawImage>().texture = texture;
						//		eventgetter.death = texture;
					}
				}
				else
				{

					float ratio = texture.width / texture.height;
				//	rawimage.GetComponent<AspectRatioFitter>().aspectMode = AspectRatioFitter.AspectMode.FitInParent;

				//	rawimage.GetComponent<AspectRatioFitter>().aspectRatio = ratio;
					rawimage.GetComponent<RawImage>().texture = texture;
					if (ratio == 0)
					{
						ratio = 1f;

					//	rawimage.GetComponent<AspectRatioFitter>().aspectRatio = ratio;
					}
					else
					{
						if (ratio == 0)
						{
							ratio = 1f;

						//	rawimage.GetComponent<AspectRatioFitter>().aspectRatio = ratio;
						}
					//	rawimage.GetComponent<AspectRatioFitter>().aspectRatio = ratio;
					//	rawimage.GetComponent<RawImage>().texture = texture;
						Debug.Log("not death death");
						//rawimage.GetComponent<RawImage>().texture = texture;
						//rawimage.GetComponent<AspectRatioFitter>().aspectRatio = aspctratio;
						//	rawimage.GetComponent<AspectRatioFitter>().aspectRatio = aspctratio;
						//rawimage.GetComponent<RawImage>().texture = texture;
						//		eventgetter.death = texture;
					}

				}

				//		eventgetter.death = texture;

				updataeven.instance.imagepath = path;

				//	eventgetter.instance.deathtomyself();
				ImageCropper.Instance.Show(texture, (bool result, Texture originalImage, Texture2D croppedImage) =>
				{
					// Destroy previously cropped texture (if any) to free memory
					//Destroy(rawimage.GetComponent<RawImage>().texture, 5f);

					// If screenshot was cropped successfully
					if (result)
					{
						// Assign cropped texture to the RawImage
					//	rawimage.GetComponent<RawImage>().enabled = true;
						rawimage.GetComponent<RawImage>().texture = croppedImage;

						Vector2 size = rawimage.GetComponent<RawImage>().rectTransform.sizeDelta;
						if (croppedImage.height <= croppedImage.width)
							size = new Vector2(1024f, 1024f * (croppedImage.height / (float)croppedImage.width));
						else
							size = new Vector2(1024f * (croppedImage.width / (float)croppedImage.height), 1024f);
						//rawimage.GetComponent<RawImage>().rectTransform.sizeDelta = size;
						duplicateTexture(texture);
						updataeven.eventimgetexture = (Texture2D)rawimage.GetComponent<RawImage>().texture;
					//	rawimage.GetComponent<RawImage>().enabled = true;
						panel.SetActive(false);
						approve();


						//croppedImageSize.text = "Image size: " + croppedImage.width + ", " + croppedImage.height;
					}
					else
					{
						panel.SetActive(false);
						updataeven.instance.closecamera();
						//	rawimage.GetComponent<RawImage>().enabled = false;
						//	rawimage.GetComponent<RawImage>().enabled = false;
					}

					// Destroy the screenshot as we no longer need it in this case
					//Destroy(screenshot);
				},
			settings: new ImageCropper.Settings()
			{
			//	markTextureNonReadable = false,
				ovalSelection = true,
				autoZoomEnabled = false,
				imageBackground = Color.clear, // transparent background
				selectionMinAspectRatio = 1,
				selectionMaxAspectRatio = 1

			},
			croppedImageResizePolicy: (ref int width, ref int height) =>
			{
				// uncomment lines below to save cropped image at half resolution
				//width /= 2;
				//height /= 2;
			});


				// Assign texture to a temporary quad and destroy it after 5 seconds
				/*	GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
					quad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
					quad.transform.forward = Camera.main.transform.forward;
					quad.transform.localScale = new Vector3(1f, texture.height / (float)texture.width, 1f);

					Material material = quad.GetComponent<Renderer>().material;
					if (!material.shader.isSupported) // happens when Standard shader is not included in the build
						material.shader = Shader.Find("Legacy Shaders/Diffuse");

					material.mainTexture = texture;
					duplicateTexture(texture);

					Destroy(quad, 5f);

					// If a procedural texture is not destroyed manually, 
					// it will only be freed after a scene change
					Destroy(texture, 5f);*/
			}
			else
			{
				updataeven.instance.closecamera();
				panel.SetActive(false);
			}
		});

		Debug.Log("Permission result: " + permission);
	}
	Texture2D duplicateTexture(Texture2D source)
	{
		RenderTexture renderTex = RenderTexture.GetTemporary(
					source.width,
					source.height,
					0,
					RenderTextureFormat.Default,
					RenderTextureReadWrite.Linear);

		Graphics.Blit(source, renderTex);
		RenderTexture previous = RenderTexture.active;
		RenderTexture.active = renderTex;
		Texture2D readableText = new Texture2D(source.width, source.height);
		readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
		readableText.Apply();
		RenderTexture.active = previous;
		RenderTexture.ReleaseTemporary(renderTex);
		//updataeven.eventimgetexture = readableText;
		//eventgetter.death = readableText;

		//updataeven.instance.sendconverttext();
		return readableText;
	}
	public void PickVideo()
	{
		NativeGallery.Permission permission = NativeGallery.GetVideoFromGallery((path) =>
		{
			Debug.Log("Video path: " + path);
			if (path != null)
			{
				// Play the selected video
				Handheld.PlayFullScreenMovie("file://" + path);
			}
		}, "Select a video");

		Debug.Log("Permission result: " + permission);
	}

	// Example code doesn't use this function but it is here for reference
	private void PickImageOrVideo()
	{
		if (NativeGallery.CanSelectMultipleMediaTypesFromGallery())
		{
			NativeGallery.Permission permission = NativeGallery.GetMixedMediaFromGallery((path) =>
			{
				Debug.Log("Media path: " + path);
				if (path != null)
				{
					// Determine if user has picked an image, video or neither of these
					switch (NativeGallery.GetMediaTypeOfFile(path))
					{
						case NativeGallery.MediaType.Image: Debug.Log("Picked image"); break;
						case NativeGallery.MediaType.Video: Debug.Log("Picked video"); break;
						default: Debug.Log("Probably picked something else"); break;
					}
				}
			}, NativeGallery.MediaType.Image | NativeGallery.MediaType.Video, "Select an image or video");

			Debug.Log("Permission result: " + permission);
		}
	}
}
