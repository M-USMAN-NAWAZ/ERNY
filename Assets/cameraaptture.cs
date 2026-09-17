using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class cameraaptture : MonoBehaviour
{
    WebCamTexture webcamtexturr;
    public string path;
    public RawImage raw;
    
    // Start is called before the first frame update
    void Start()
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


    public void AR()
    {


        webcamtexturr.Stop();
        SceneManager.LoadScene("4. MainScreens");

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
