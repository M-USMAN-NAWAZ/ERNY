using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.Experimental.XR;
public class fakrarspawner : MonoBehaviour
{
    
    public bool death;
    public GameObject placementindicator,_Confirm;
    public ARSessionOrigin arOrigin;
    public ARSession arses;
    public GameObject panel;
    private Pose placementpose;
    public bool placementposeisvalid;
    [SerializeField]
    ARRaycastManager m_raycastmanager;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();
    [SerializeField]
    GameObject[] spawnableobject;
    public Camera arcam;

    GameObject spawnedobject;
    private ARPlaneManager arsession;
    private ARPointCloudManager arcloud;
    private Vector2 touchPosition;
    int check;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    // Start is called before the first frame update
    void Start()
    {
        death = false;
        m_raycastmanager = GetComponent<ARRaycastManager>();
        arcloud = GetComponent<ARPointCloudManager>();
        arsession = GetComponent<ARPlaneManager>();
        check = 0;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        spawnedobject = null;
     //   arcam = GameObject.Find("AR Camera").GetComponent<Camera>();

    }
    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }
    // Update is called once per frame
    /*  void Update()
      {
          if (Input.touchCount == 0)
              return;

          RaycastHit hit;
          Ray ray = arcam.ScreenPointToRay(Input.GetTouch(0).position);
          if(m_raycastmanager.Raycast(Input.GetTouch(0).position,m_hits))
          {
              if(Input.GetTouch(0).phase== TouchPhase.Began&&spawnedobject==null)
              {


                  if(Physics.Raycast(ray,out hit))
                  {
                      if (hit.collider.gameObject.tag == "spawnable")
                      { 
                          spawnedobject = hit.collider.gameObject;
                      }

                      else
                      {
                          if (check == 0)
                          {
                              check += 1;
                              Spawnprefab(m_hits[0].pose.position);

                              panel.SetActive(false);
                          }
                      }
                  }
              }
              else if(Input.GetTouch(0).phase==TouchPhase.Moved&&spawnedobject!=null)
              {
                  spawnedobject.transform.position = m_hits[0].pose.position;
              }
              if(Input.GetTouch(0).phase== TouchPhase.Ended)
              {
                  spawnedobject = null;
              }
          }


      }
      */


    public void setallplaneactive()
    {
        arses.Reset();
        death = true;
        for (int i = 0; i < hits.Count; i++)
        {
            hits.RemoveAt(i);
        }
        hits.Clear();
        placementposeisvalid = false;
        placementindicator.SetActive(false);
        arsession.GetComponent<ARPlaneManager>().enabled = true;
        //  arcloud.enabled = true;
        check = 0;
        foreach (var plane in arsession.trackables)
        {
            plane.gameObject.SetActive(true);
        }
        placementindicator.SetActive(false);
    }
    private void setallplaneinactive()
    {
        //arcloud.enabled = false;
        death = true;
        foreach (var plane in arsession.trackables)
        {
            plane.gameObject.SetActive(false);
        }
        placementindicator.SetActive(false);
    }
    private void Spawnprefab(Vector3 spawnposition)
    {
        //  spawnedobject = Instantiate(spawnableobject, spawnposition, Quaternion.identity);
        setallplaneinactive();
        arsession.GetComponent<ARPlaneManager>().enabled = false;
    }




    public void updateplacementpose()
    {
        var screencenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));

        m_raycastmanager.Raycast(screencenter, hits, TrackableType.Planes);



        placementposeisvalid = hits.Count > 0;
        if (placementposeisvalid)
        {

            placementpose = hits[0].pose;
        }
    }

    public void indicator()
    {
        if (placementposeisvalid && !death)
        {

            placementindicator.SetActive(true);

            placementindicator.transform.SetPositionAndRotation(placementpose.position, placementpose.rotation);
        }
        else
        { placementindicator.SetActive(false); }

    }
    Transform pp;
    private List<ARRaycastHit> hit;

    private void Update()
    {
        indicator();
        updateplacementpose();
        Debug.Log("are theme" + ardata.artheme);
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;

        if (m_raycastmanager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.

            var hitPose = s_Hits[0].pose;
            gameObject.GetComponent<ARPlaneManager>().enabled = false;

            foreach (ARPlane plane in gameObject.GetComponent<ARPlaneManager>().trackables)
            {
                plane.gameObject.SetActive(false);
            }

            if (spawnedobject == null)
            {
               // spawnedobject = Instantiate(spawnableobject[0], placementindicator.transform.position, Quaternion.identity);
                Handheld.Vibrate();
                _Confirm.SetActive(true);
                setallplaneinactive();
                panel.SetActive(false);
            }



        }

    }










    // Start is called before the first frame update
    
}
