using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Android;
public class turnoffon : MonoBehaviour
{
    public CanvasGroup canvas;
    public GameObject loading;
    public GameObject ardata, arobject, camera;
    public spawnmangr sp;
    // Start is called before the first frame update
    void Start()
    {
        
    }


   IEnumerator turningon()
    {


        yield return new WaitForSeconds(1);

        camera.SetActive(false);
        if (sp != null)
        {
            sp.ClearSpawnedObject(true);
        }

        ardata.SetActive(true);
        canvas.alpha = 0;
        canvas.interactable = false;
        canvas.blocksRaycasts = false;
        loading.SetActive(false);
    }


    public void turnaron()
    {
#if UNITY_IOS



        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            loading.SetActive(true);
            StartCoroutine(turningon());
        }
        else
        {
            eventgetter.instance.ARcameranotallowed();

        }
#endif
#if PLATFORM_ANDROID

if (Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            loading.SetActive(true);
            StartCoroutine(turningon());
        }
        else
        {

            eventgetter.instance.ARcameranotallowed();
            // eventgetter.instance.cameranotallowedpopup();

        }


#endif


    }

    IEnumerator death()

    {

        yield return new WaitForSeconds(0.5f);
        if (arobject != null)
        {
         //   Destroy(arobject);
            Debug.LogError("checking " + arobject.name);
        }
    }

    public void circel()
    {
        StartCoroutine(checkcircle());

    }

    IEnumerator checkcircle()

    {
        yield return new WaitForSeconds(2.5f);
       sp.checkcircle = false;
        Debug.Log("chcekinglog");
    }

    public void gameprofileon_off()
    {
        GameObject[] aritems = GameObject.FindGameObjectsWithTag("ARItem");
        foreach (GameObject x in aritems)
        {
            ardata itemData = x.GetComponent<ardata>();

            if (itemData == null)
            {
                continue;
            }

            if (itemData.IsProfileImageVisible())
            {
                Debug.Log("this is working");
                itemData.SetProfileImageVisible(false);
            }
            else
            {
                itemData.SetProfileImageVisible(true);
            }
        }
    }







    public void turnaroffvr()
    {
        loading.SetActive(false);
        ardata.SetActive(false);

        //arobject = GameObject.FindGameObjectWithTag("bro");
        //Debug.LogError("test " + arobject.name);
        //Destroy(arobject);
        //arobject = GameObject.FindGameObjectWithTag("bro");
        //Destroy(arobject);
        //Debug.LogError("checking " + arobject.name);

        GameObject[] aritems = GameObject.FindGameObjectsWithTag("ARItem");
        foreach (GameObject x in aritems)
        {
            Debug.LogError("Deleting item" + x.name);
            Destroy(x.gameObject);
        }

        if (sp != null)
        {
            sp.ClearSpawnedObject(true);
        }

        canvas.alpha = 1;
        canvas.interactable = true;
        canvas.blocksRaycasts = true;
        camera.SetActive(true);

        StartCoroutine(death());
    }

    public void turnaroff()
    {
        loading.SetActive(false);
        ardata.SetActive(false);

        //arobject = GameObject.FindGameObjectWithTag("bro");
        //Debug.LogError("test " + arobject.name);
        //Destroy(arobject);
        //arobject = GameObject.FindGameObjectWithTag("bro");
        //Destroy(arobject);
        //Debug.LogError("checking " + arobject.name);

        GameObject[] aritems = GameObject.FindGameObjectsWithTag("ARItem");
        foreach (GameObject x in aritems) 
        {
            Debug.LogError("Deleting item" + x.name);
            Destroy(x.gameObject);
        }

        if (sp != null)
        {
            sp.ClearSpawnedObject(true);
        }

        
        canvas.alpha = 1;
        canvas.interactable = true;
        canvas.blocksRaycasts = true;
        camera.SetActive(true);

        StartCoroutine(death());
    }
    // Update is called once per frame
    
}
