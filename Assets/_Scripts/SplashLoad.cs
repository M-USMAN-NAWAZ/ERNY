using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Android;
using UnityEngine.Video;

//using UnityEngine.iOS;

public class SplashLoad : MonoBehaviour
{
	public static int loadlevel;
	public static int microphone;
	public static int gallery;
	public static int camera;
	public VideoPlayer splashVideoPlayer;
	public float fallbackDelay = 5.7f;
	private bool sceneLoadStarted;

	private void Start()
    {

		gallery = PlayerPrefs.GetInt("gallery");
		microphone = PlayerPrefs.GetInt("mic");

		camera = PlayerPrefs.GetInt("camera");
		loadlevel = PlayerPrefs.GetInt("loadlevel");

        Debug.Log("check work : " + loadlevel);

		if (splashVideoPlayer == null)
		{
			splashVideoPlayer = GetComponent<VideoPlayer>();
		}

		if (splashVideoPlayer != null)
		{
			splashVideoPlayer.loopPointReached += OnSplashVideoFinished;
		}
		else
		{
			Invoke("LoadMainLevel", fallbackDelay);
		}

    }
	private void OnDestroy()
	{
		if (splashVideoPlayer != null)
		{
			splashVideoPlayer.loopPointReached -= OnSplashVideoFinished;
		}
	}

	private void OnSplashVideoFinished(VideoPlayer source)
	{
		LoadMainLevel();
	}

    public void LoadMainLevel() 
	{
		if (sceneLoadStarted)
		{
			return;
		}

		sceneLoadStarted = true;

#if UNITY_EDITOR
		Debug.Log("check work : " + loadlevel);
		SceneManager.LoadScene(2);

#elif UNITY_IOS

		
			if (Application.HasUserAuthorization(UserAuthorization.Microphone)&& Application.HasUserAuthorization(UserAuthorization.WebCam))		
		{

			SceneManager.LoadScene(2);


		}
		else if (!Application.HasUserAuthorization(UserAuthorization.Microphone) && Application.HasUserAuthorization(UserAuthorization.WebCam))
		{

			SceneManager.LoadScene(2);


		}

		else if (Application.HasUserAuthorization(UserAuthorization.Microphone) && !Application.HasUserAuthorization(UserAuthorization.WebCam))
		{

			SceneManager.LoadScene(2);


		}

		else if (Application.HasUserAuthorization(UserAuthorization.Microphone) && !Application.HasUserAuthorization(UserAuthorization.WebCam))
		{

			SceneManager.LoadScene(2);


		}
		
		else if (loadlevel == 0)
		{

			SceneManager.LoadScene(1);


		}
		else
		{


			Debug.Log("check work : " + loadlevel);
			SceneManager.LoadScene(2);

		}






#elif UNITY_ANDROID || PLATFORM_ANDROID

		if (Permission.HasUserAuthorizedPermission(Permission.Camera) && Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
			SceneManager.LoadScene(2);
		}
		else if (!Permission.HasUserAuthorizedPermission(Permission.Camera) && Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
			SceneManager.LoadScene(2);
		}
		else if (Permission.HasUserAuthorizedPermission(Permission.Camera) && !Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
			SceneManager.LoadScene(1);
		}
		else if (!Permission.HasUserAuthorizedPermission(Permission.Camera) && !Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
			Debug.Log("Both permissions are not granted.");
            SceneManager.LoadScene(1);
		}
		else if (loadlevel == 0)
		{

			SceneManager.LoadScene(1);
		}
		else
		{
			Debug.Log("check work : " + loadlevel);
			SceneManager.LoadScene(2);
		}

#endif

	}
}
