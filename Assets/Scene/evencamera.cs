using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class evencamera : MonoBehaviour
{
    WebCamTexture webcamtexturr;
    public string path;
    public RawImage raw;

    // Start is called before the first frame update
   public void openwebcam()
    {

        webcamtexturr = new WebCamTexture();
        float f = webcamtexturr.width / webcamtexturr.height;
        raw.GetComponent<AspectRatioFitter>().aspectRatio = f;
        raw.texture = webcamtexturr;
        webcamtexturr.Play();


    }


    private void OnDestroy()
    {
        webcamtexturr.Stop();
    }


    public void closwebcam()
    {


        webcamtexturr.Stop();
        SceneManager.LoadScene("");

    }
}
