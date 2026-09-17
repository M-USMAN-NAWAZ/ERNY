using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.XR;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class spawnmangr : MonoBehaviour
{
    public GameObject[] turnactiveobj;
    public bool checkcircle;
    public GameObject facetoggle;
    public Material greenindicator,whiteindicator;
    public GameObject placementindicator,test;
    public XROrigin arOrigin;
    public ARSession arses;
    public GameObject panel,tutorial;
    public GameObject shutterbutton,poistionimage;
    private Pose placementpose;
    public bool placementposeisvalid;
    [SerializeField]
    ARRaycastManager m_raycastmanager;
    public AddressableHandler address;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();
    [SerializeField]
    GameObject[] spawnableobject;
  public  Camera arcam;
    public bool parentSpawnedObjectsToCamera = true;
    public bool bypassPlaneDetectionForSpawn = true;
    [Range(0f, 1f)] [SerializeField] private float addressableProfileViewportHeight = 0.62f;
	GameObject spawnedobject;
    private ARPlaneManager arsession;
    private ARPointCloudManager arcloud;
    private Vector2 touchPosition;
    int check;
    public GameObject micorphoneobject;
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    public GameObject confetti;

    public bool objectSpawned = false;
    private bool waitingForFreshTouchRelease = true;
    // Start is called before the first frame update
    void Start()
    {
        objectSpawned = false;
        checkcircle = false;
        m_raycastmanager = GetComponent<ARRaycastManager>();
           arcloud = GetComponent<ARPointCloudManager>();
        arsession = GetComponent<ARPlaneManager>();
        check = 0;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        spawnedobject = null;
        arcam = GameObject.Find("AR Camera").GetComponent<Camera>();
        
    }

    private void OnEnable()
    {
        waitingForFreshTouchRelease = true;
        ClearSpawnedObject(true);
        ClearExistingArItems();
        confetti = null;
    }

    private bool IsPrimaryTouchPressed()
    {
        return Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed;
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (IsPrimaryTouchPressed())
        {
            touchPosition = Touchscreen.current.primaryTouch.position.ReadValue() ;
            return true;
        }

        touchPosition = default;
        return false;
    }

    private bool IsPointerOverUi()
    {
        if (EventSystem.current == null)
        {
            return false;
        }

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            int touchId = Touchscreen.current.primaryTouch.touchId.ReadValue();

            if (EventSystem.current.IsPointerOverGameObject(touchId))
            {
                return true;
            }
        }

        return EventSystem.current.IsPointerOverGameObject();
    }


    public void tunroffarobjects()
    {
        for(int i=0;i<turnactiveobj.Length;i++)
        {

            turnactiveobj[i].SetActive(false);

        }

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
        checkcircle = true;
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
        checkcircle = true;
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
        //#if !UNITY_EDITOR
        var camera = Camera.main;

        var screencenter = Camera.current.ViewportToScreenPoint(new Vector3 (0.5f,0.5f));
      
        m_raycastmanager.Raycast(screencenter, hits, TrackableType.Planes);

        Ray ray = camera.ScreenPointToRay(screencenter);

        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red);

        placementposeisvalid = hits.Count > 0;
        if(placementposeisvalid)
        {
            Debug.DrawRay(placementpose.position, Vector3.up * 0.2f, Color.green);
            placementpose = hits[0].pose;
        }

//#endif
    }




    public void changeecolor()
    {


        test.GetComponentInChildren<MeshRenderer>().material = greenindicator;

    }

    

    public void indicator()
    {
        if (arhandler.selectar == 0)
        {
            Ray ray = arcam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            if (placementposeisvalid && !checkcircle)
            {
                RaycastHit checkhit;
                if (Physics.Raycast(ray, out checkhit, 100))
                {
                    if (checkhit.collider != null)
                    {
                        Debug.Log("KAKAAKAKKAAKAKAKAKAKAKKAKAKKKAKAKAKAKAKAKAKKAKAKAKAAKAKAKA");
                        placementindicator.SetActive(true);
                        micorphoneobject.SetActive(true);

						placementindicator.transform.SetPositionAndRotation(placementpose.position, placementpose.rotation);
                    }
                    else
                    {
                        
                        placementindicator.SetActive(false);

                    }
                }
                else
                {
                    Debug.Log("UAUAUAUAUAUAAUAUAUAUAUAUAUUAAUUAUAUAUAUAUAAUAUAUAUAUAUAUAU");

                    placementindicator.SetActive(false);

                    placementindicator.transform.SetPositionAndRotation(placementpose.position, placementpose.rotation);



                }
            }
            else
            {
                Debug.Log("UAUAUAUAUAUAAUAUAUAUAUAUAUUAAUUAUAUAUAUAUAAUAUAUAUAUAUAUAU");
                placementindicator.SetActive(false);


            }
        }
        else
        {
            Debug.Log("UAUAUAUAUAUAAUAUAUAUAUAUAUUAAUUAUAUAUAUAUAAUAUAUAUAUAUAUAU");
            placementindicator.SetActive(false);
        }
    }
    Transform pp;
	private List<ARRaycastHit> hit;
    public Text distance;


    bool IsPlaneTracked()
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        return m_raycastmanager.Raycast(
            new Vector2(Screen.width / 2, Screen.height / 2),
            hits,
            TrackableType.PlaneWithinPolygon
        );
    }

    private bool ShouldSkipPlaneDetection(GameObject prefab)
    {
        CelebrationJsonSpawnable celebrationSpawnable = prefab != null ? prefab.GetComponentInChildren<CelebrationJsonSpawnable>(true) : null;
        return celebrationSpawnable != null && celebrationSpawnable.skipPlaneDetection;
    }

    private bool IsModelDownloaded(int modelIndex)
    {
        return AddressableHandler.Instance != null &&
               AddressableHandler.Instance.isModelDownloaded != null &&
               modelIndex >= 0 &&
               modelIndex < AddressableHandler.Instance.isModelDownloaded.Length &&
               AddressableHandler.Instance.isModelDownloaded[modelIndex];
    }

    private bool ShouldUseDefaultSpawnable()
    {
        if (ardata.artheme == "baloon")
        {
            return true;
        }

        if (ardata.artheme == "clover")
        {
            return !IsModelDownloaded(1);
        }

        if (ardata.artheme == "winter")
        {
            return !IsModelDownloaded(2);
        }

        if (ardata.artheme == "fire")
        {
            return !IsModelDownloaded(3);
        }

        if (ardata.artheme == "patrik")
        {
            return !IsModelDownloaded(4);
        }

        if (ardata.artheme == "usa")
        {
            return !IsModelDownloaded(5);
        }

        if (ardata.artheme == "halloween")
        {
            return !IsModelDownloaded(6);
        }

        if (ardata.artheme == "thanksgiving")
        {
            return !IsModelDownloaded(7);
        }

        if (ardata.artheme == "christmas")
        {
            return !IsModelDownloaded(9);
        }

        if (ardata.artheme == "valentine")
        {
            return !IsModelDownloaded(10);
        }

        if (ardata.artheme == "balloon")
        {
            return !IsModelDownloaded(11);
        }

        return true;
    }

    private Vector3 GetCameraSpawnPosition(GameObject prefab)
    {
        CelebrationJsonSpawnable celebrationSpawnable = prefab != null ? prefab.GetComponentInChildren<CelebrationJsonSpawnable>(true) : null;
        float distance = celebrationSpawnable != null ? celebrationSpawnable.cameraFollowDistance : 1.5f;
        Vector3 offset = celebrationSpawnable != null ? celebrationSpawnable.cameraFollowOffset : Vector3.zero;

        return arcam.transform.position +
               arcam.transform.forward * Mathf.Max(0.01f, distance) +
               arcam.transform.TransformVector(offset);
    }

    private Canvas GetSpawnCanvas()
    {
        Canvas canvas = panel != null ? panel.GetComponentInParent<Canvas>() : null;
        if (canvas != null)
        {
            return canvas;
        }

        return FindObjectOfType<Canvas>();
    }

    private bool ShouldSpawnOnCanvas(GameObject prefab)
    {
        CelebrationJsonSpawnable celebrationSpawnable = prefab != null ? prefab.GetComponentInChildren<CelebrationJsonSpawnable>(true) : null;
        return celebrationSpawnable != null && celebrationSpawnable.renderOnCanvas;
    }

    public void ClearSpawnedObject(bool destroyExisting)
    {
        if (destroyExisting && spawnedobject != null)
        {
            Destroy(spawnedobject);
        }

        spawnedobject = null;
        objectSpawned = false;
        checkcircle = false;
    }

    private void ClearExistingArItems()
    {
        GameObject[] arItems = GameObject.FindGameObjectsWithTag("ARItem");

        for (int i = 0; i < arItems.Length; i++)
        {
            Destroy(arItems[i]);
        }
    }

    private void ActivateSpawnUi(int turnActiveIndex)
    {
        if (tutorial != null)
        {
            tutorial.GetComponent<Animator>().Play("fade out");
        }

        if (turnactiveobj != null && turnActiveIndex >= 0 && turnActiveIndex < turnactiveobj.Length)
        {
            turnactiveobj[turnActiveIndex].SetActive(true);
        }
    }

    private GameObject GetCurrentSpawnPrefab()
    {
        if (ardata.artheme == "clover" && IsModelDownloaded(1))
        {
            ActivateSpawnUi(1);
            return address.currentModels[1];
        }

        if (ardata.artheme == "winter" && IsModelDownloaded(2))
        {
            ActivateSpawnUi(2);
            return address.currentModels[2];
        }

        if (ardata.artheme == "fire" && IsModelDownloaded(3))
        {
            ActivateSpawnUi(3);
            return address.currentModels[3];
        }

        if (ardata.artheme == "patrik" && IsModelDownloaded(4))
        {
            ActivateSpawnUi(4);
            return address.currentModels[4];
        }

        if (ardata.artheme == "usa" && IsModelDownloaded(5))
        {
            ActivateSpawnUi(5);
            return address.currentModels[5];
        }

        if (ardata.artheme == "halloween" && IsModelDownloaded(6))
        {
            ActivateSpawnUi(6);
            return address.currentModels[6];
        }

        if (ardata.artheme == "thanksgiving" && IsModelDownloaded(7))
        {
            ActivateSpawnUi(7);
            return address.currentModels[7];
        }

        if (ardata.artheme == "christmas" && IsModelDownloaded(9))
        {
            ActivateSpawnUi(8);
            return address.currentModels[9];
        }

        if (ardata.artheme == "valentine" && IsModelDownloaded(10))
        {
            ActivateSpawnUi(9);
            return address.currentModels[10];
        }

        if (ardata.artheme == "balloon" && IsModelDownloaded(11))
        {
            ActivateSpawnUi(0);
            return address.currentModels[11];
        }

        ActivateSpawnUi(0);
        return spawnableobject != null && spawnableobject.Length > 0 ? spawnableobject[0] : null;
    }

    private GameObject InstantiateArObject(GameObject prefab, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        GameObject instance = Instantiate(prefab, spawnPosition, spawnRotation);
        confetti = instance;
        ParentSpawnedObjectToCamera(instance, prefab);
        LockAddressableToCamera(instance, prefab);
        ConfigureSpawnedArObject(instance);
        return instance;
    }

    private bool IsAddressablePrefab(GameObject prefab)
    {
        if (prefab == null ||
            prefab.GetComponentInChildren<CelebrationJsonSpawnable>(true) != null ||
            address == null ||
            address.currentModels == null)
        {
            return false;
        }

        for (int i = 0; i < address.currentModels.Length; i++)
        {
            if (address.currentModels[i] == prefab)
            {
                return true;
            }
        }

        return false;
    }

    private void LockAddressableToCamera(GameObject instance, GameObject prefab)
    {
        if (instance == null || !IsAddressablePrefab(prefab))
        {
            return;
        }

        ardata[] arDataComponents = instance.GetComponentsInChildren<ardata>(true);
        for (int i = 0; i < arDataComponents.Length; i++)
        {
            arDataComponents[i].SetCameraRelativeTransformLocked(true);
        }
    }

    private void ParentSpawnedObjectToCamera(GameObject instance, GameObject prefab)
    {
        bool isAddressablePrefab = IsAddressablePrefab(prefab);
        if ((!parentSpawnedObjectsToCamera && !isAddressablePrefab) || instance == null || arcam == null || ShouldSpawnOnCanvas(prefab))
        {
            return;
        }

        CelebrationJsonSpawnable celebrationSpawnable = prefab != null ? prefab.GetComponentInChildren<CelebrationJsonSpawnable>(true) : null;
        float cameraDistance = celebrationSpawnable != null ? celebrationSpawnable.cameraFollowDistance : 1.5f;
        Vector3 cameraOffset = celebrationSpawnable != null ? celebrationSpawnable.cameraFollowOffset : Vector3.zero;
        CelebrationJsonSpawnable spawnedCelebration = instance.GetComponentInChildren<CelebrationJsonSpawnable>(true);
        if (spawnedCelebration != null)
        {
            spawnedCelebration.parentToCamera = true;
        }

        // instance.transform.SetParent(arcam.transform, false);
        // instance.transform.localPosition = Vector3.forward * Mathf.Max(0.01f, cameraDistance) + cameraOffset;
        // instance.transform.localRotation = Quaternion.LookRotation(Vector3.back, Vector3.up);

        instance.transform.SetParent(arcam.transform, false);
        instance.transform.localPosition = new Vector3(0f, 0f, cameraDistance + 0.35f);
        instance.transform.localRotation = Quaternion.LookRotation(Vector3.back, Vector3.up);

        if (isAddressablePrefab)
        {
            //CenterAddressableProfileOnCamera(instance);
        }
    }

    private void CenterAddressableProfileOnCamera(GameObject instance)
    {
        ardata profileData = instance.GetComponentInChildren<ardata>(true);
        if (profileData == null)
        {
            return;
        }

        bool hasBounds = false;
        Bounds profileBounds = default;
        EncapsulateRendererBounds(profileData.mesh, ref profileBounds, ref hasBounds);
        EncapsulateRendererBounds(profileData.mesh3, ref profileBounds, ref hasBounds);

        if (!hasBounds)
        {
            return;
        }

        Vector3 centerInCameraSpace = arcam.transform.InverseTransformPoint(profileBounds.center);
        Vector3 targetWorldPosition = arcam.ViewportToWorldPoint(
            new Vector3(0.5f, addressableProfileViewportHeight, centerInCameraSpace.z));
        Vector3 cameraLocalOffset = arcam.transform.InverseTransformVector(targetWorldPosition - profileBounds.center);
        instance.transform.localPosition += new Vector3(cameraLocalOffset.x, cameraLocalOffset.y, 0f);
    }

    private static void EncapsulateRendererBounds(Renderer targetRenderer, ref Bounds bounds, ref bool hasBounds)
    {
        if (targetRenderer == null)
        {
            return;
        }

        if (!hasBounds)
        {
            bounds = targetRenderer.bounds;
            hasBounds = true;
            return;
        }

        bounds.Encapsulate(targetRenderer.bounds);
    }

    private void SpawnWithoutPlaneDetection(GameObject prefab)
    {
        Debug.Log("Spawn");
        if (spawnedobject != null || prefab == null)
        {
            return;
        }

        if (ShouldSpawnOnCanvas(prefab))
        {
            Canvas canvas = GetSpawnCanvas();
            if (canvas != null)
            {
                spawnedobject = Instantiate(prefab, canvas.transform);
                confetti = spawnedobject;
                spawnedobject.transform.localPosition = Vector3.zero;
                spawnedobject.transform.localRotation = Quaternion.identity;
                spawnedobject.transform.localScale = Vector3.one;
                ConfigureSpawnedArObject(spawnedobject);
                Debug.Log("if Spawn");
            }
            else
            {
                spawnedobject = InstantiateArObject(prefab, GetCameraSpawnPosition(prefab), Quaternion.identity);
            }
        }
        else
        {
            spawnedobject = InstantiateArObject(prefab, GetCameraSpawnPosition(prefab), Quaternion.identity);
        }

        checkcircle = true;
        objectSpawned = true;

        setallplaneinactive();
        panel.SetActive(false);
        StartCoroutine(turnofftutorial());
    }

    private void ConfigureSpawnedArObject(GameObject instance)
    {
        if (instance == null)
        {
            return;
        }

        instance.tag = "ARItem";
    }

    private void Update()
    {
        if (bypassPlaneDetectionForSpawn)
        {
            placementindicator.SetActive(false);
            distance.text = "dist: " + distance;

            if (waitingForFreshTouchRelease)
            {
                if (!IsPrimaryTouchPressed())
                {
                    waitingForFreshTouchRelease = false;
                }

                return;
            }

            if (!TryGetTouchPosition(out Vector2 bypassTouchPosition))
            {
                return;
            }

            if (IsPointerOverUi())
            {
                return;
            }

            if (eventgetter.archeck == 1 && spawnedobject == null)
            {
                Handheld.Vibrate();
                SpawnWithoutPlaneDetection(GetCurrentSpawnPrefab());
            }

            return;
        }

        bool skipPlaneDetectionForCurrentPrefab = spawnableobject != null &&
                                                  spawnableobject.Length > 0 &&
                                                  ShouldUseDefaultSpawnable() &&
                                                  ShouldSkipPlaneDetection(spawnableobject[0]);

        if (!skipPlaneDetectionForCurrentPrefab)
        {
            indicator();
            updateplacementpose();

            float dist = Vector3.Distance(placementindicator.transform.position, arcam.transform.position);
            if (dist > 1.89f && dist < 2.2f)
            {

                placementindicator.GetComponentInChildren<MeshRenderer>().material = greenindicator;

            }
            else
            {

                placementindicator.GetComponentInChildren<MeshRenderer>().material = whiteindicator;
            }
        }
        else
        {
            placementindicator.SetActive(false);
        }

        distance.text = "dist: " + distance;


        if (waitingForFreshTouchRelease)
        {
            if (!IsPrimaryTouchPressed())
            {
                waitingForFreshTouchRelease = false;
            }

            return;
        }

        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;

        if (IsPointerOverUi())
        {
            return;
        }

        if (skipPlaneDetectionForCurrentPrefab)
        {
            if (eventgetter.archeck == 1)
            {
                tutorial.GetComponent<Animator>().Play("fade out");
                turnactiveobj[0].SetActive(true);
                SpawnWithoutPlaneDetection(spawnableobject[0]);
            }

            return;
        }

        if (m_raycastmanager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            if (eventgetter.archeck == 1)
            {
                var hitPose = s_Hits[0].pose;
                gameObject.GetComponent<ARPlaneManager>().enabled = false;

                foreach (ARPlane plane in gameObject.GetComponent<ARPlaneManager>().trackables)
                {
                    plane.gameObject.SetActive(false);
                }

                if (spawnedobject == null && placementindicator)
                {
                    Handheld.Vibrate();
                    if (ardata.artheme == "baloon")
                    {
                        tutorial.GetComponent<Animator>().Play("fade out");
                        turnactiveobj[0].SetActive(true);

                        spawnedobject = InstantiateArObject(spawnableobject[0], placementindicator.transform.position, Quaternion.identity);


                        //if (IsPlaneTracked())
                        //{
                        //    spawnedobject = Instantiate(
                        //        spawnableobject[0],
                        //        placementindicator.transform.position,
                        //        Quaternion.identity
                        //    );
                        //    Debug.Log("are theme" + ardata.artheme);
                        //}
                        //else /*if (Input.touchCount > 0 && Input.GetTouch(0).phase == UnityEngine.TouchPhase.Began)*/
                        //{
                        //    Vector3 spawnPos = arcam.transform.position +
                        //                       arcam.transform.forward * 1.5f;

                        //    spawnedobject = Instantiate(
                        //        spawnableobject[0],
                        //        spawnPos,
                        //        Quaternion.identity
                        //    );
                        //    Debug.Log("Instantiated without planetracking");
                        //}
                    }
                    else if (ardata.artheme == "clover")
                    {
                        if (AddressableHandler.Instance.isModelDownloaded[1])
                        {
                            tutorial.GetComponent<Animator>().Play("fade out");
                            turnactiveobj[1].SetActive(true);



                            spawnedobject = InstantiateArObject(address.currentModels[1], placementindicator.transform.position, Quaternion.identity);

                         
                        }
                        else
                        {

                            tutorial.GetComponent<Animator>().Play("fade out");
                            turnactiveobj[0].SetActive(true);

                            spawnedobject = InstantiateArObject(spawnableobject[0], placementindicator.transform.position, Quaternion.identity);






                        }



                    }
                    else if (ardata.artheme == "winter")
                    {
                        if (AddressableHandler.Instance.isModelDownloaded[2])
                        {
                            tutorial.GetComponent<Animator>().Play("fade out");
                            turnactiveobj[2].SetActive(true);



                            spawnedobject = InstantiateArObject(address.currentModels[2], placementindicator.transform.position, Quaternion.identity);


                        }
                        else
                        {

                            tutorial.GetComponent<Animator>().Play("fade out");
                            turnactiveobj[0].SetActive(true);

                            spawnedobject = InstantiateArObject(spawnableobject[0], placementindicator.transform.position, Quaternion.identity);






                        }


                    }
                    else if (ardata.artheme == "fire")
                    {/*
                        tutorial.GetComponent<Animator>().Play("fade out");
                        turnactiveobj[3].SetActive(true);
                        spawnedobject = Instantiate(spawnableobject[3], placementindicator.transform.position, Quaternion.identity);
                        */
                        if (AddressableHandler.Instance.isModelDownloaded[3])
                        {
                            tutorial.GetComponent<Animator>().Play("fade out");
                            turnactiveobj[3].SetActive(true);



                            spawnedobject = InstantiateArObject(address.currentModels[3], placementindicator.transform.position, Quaternion.identity);


                        }
                        else
                        {

                            tutorial.GetComponent<Animator>().Play("fade out");
                            turnactiveobj[0].SetActive(true);

                            spawnedobject = InstantiateArObject(spawnableobject[0], placementindicator.transform.position, Quaternion.identity);






                        }

                    }
                    else if (ardata.artheme == "patrik")
                    {
                        if (AddressableHandler.Instance.isModelDownloaded[4])
                        {
                            tutorial.GetComponent<Animator>().Play("fade out");
                            turnactiveobj[4].SetActive(true);



                            spawnedobject = InstantiateArObject(address.currentModels[4], placementindicator.transform.position, Quaternion.identity);


                        }
                        else
                        {

                            tutorial.GetComponent<Animator>().Play("fade out");
                            turnactiveobj[0].SetActive(true);

                            spawnedobject = InstantiateArObject(spawnableobject[0], placementindicator.transform.position, Quaternion.identity);






                        }


                    }
                    else if (ardata.artheme == "usa")
                    {
                        if (AddressableHandler.Instance.isModelDownloaded[5])
                        {
                            tutorial.GetComponent<Animator>().Play("fade out");
                            turnactiveobj[5].SetActive(true);



                            spawnedobject = InstantiateArObject(address.currentModels[5], placementindicator.transform.position, Quaternion.identity);


                        }
                        else
                        {

                            tutorial.GetComponent<Animator>().Play("fade out");
                            turnactiveobj[0].SetActive(true);

                            spawnedobject = InstantiateArObject(spawnableobject[0], placementindicator.transform.position, Quaternion.identity);






                        }


                    }
                    else if (ardata.artheme == "halloween")
                    {
                        if (AddressableHandler.Instance.isModelDownloaded[6])
                        {
                            tutorial.GetComponent<Animator>().Play("fade out");
                            turnactiveobj[6].SetActive(true);



                            spawnedobject = InstantiateArObject(address.currentModels[6], placementindicator.transform.position, Quaternion.identity);


                        }
                        else
                        {

                            tutorial.GetComponent<Animator>().Play("fade out");
                            turnactiveobj[0].SetActive(true);

                            spawnedobject = InstantiateArObject(spawnableobject[0], placementindicator.transform.position, Quaternion.identity);






                        }


                    }
                    else if (ardata.artheme == "thanksgiving")
                    {
                        if (AddressableHandler.Instance.isModelDownloaded[7])
                        {
                            tutorial.GetComponent<Animator>().Play("fade out");
                            turnactiveobj[7].SetActive(true);



                            spawnedobject = InstantiateArObject(address.currentModels[7], placementindicator.transform.position, Quaternion.identity);


                        }
                        else
                        {

                            tutorial.GetComponent<Animator>().Play("fade out");
                            turnactiveobj[0].SetActive(true);

                            spawnedobject = InstantiateArObject(spawnableobject[0], placementindicator.transform.position, Quaternion.identity);






                        }


                    }
                    else if (ardata.artheme == "christmas")
                    {
                        if (AddressableHandler.Instance.isModelDownloaded[9])
                        {
                            tutorial.GetComponent<Animator>().Play("fade out");
                            turnactiveobj[8].SetActive(true);



                            spawnedobject = InstantiateArObject(address.currentModels[9], placementindicator.transform.position, Quaternion.identity);


                        }
                        else
                        {

                            tutorial.GetComponent<Animator>().Play("fade out");
                            turnactiveobj[0].SetActive(true);

                            spawnedobject = InstantiateArObject(spawnableobject[0], placementindicator.transform.position, Quaternion.identity);






                        }


					}
					else if (ardata.artheme == "valentine")
					{
						if (AddressableHandler.Instance.isModelDownloaded[10])
						{
							tutorial.GetComponent<Animator>().Play("fade out");
							turnactiveobj[9].SetActive(true);



							spawnedobject = InstantiateArObject(address.currentModels[10], placementindicator.transform.position, Quaternion.identity);


						}
						else
						{

							tutorial.GetComponent<Animator>().Play("fade out");
							turnactiveobj[0].SetActive(true);

							spawnedobject = InstantiateArObject(spawnableobject[0], placementindicator.transform.position, Quaternion.identity);






						}


					}
					else
                    {
                        tutorial.GetComponent<Animator>().Play("fade out");

                        spawnedobject = InstantiateArObject(spawnableobject[0], placementindicator.transform.position, Quaternion.identity);


                    }

                    setallplaneinactive();
                    panel.SetActive(false);
                    StartCoroutine(turnofftutorial());


                }



            }

        }
        else if (!objectSpawned)
        {
            GameObject currentPrefab = GetCurrentSpawnPrefab();
            Vector3 spawnPos = GetCameraSpawnPosition(currentPrefab);

            spawnedobject = InstantiateArObject(currentPrefab, spawnPos, Quaternion.identity);
            
            checkcircle = true;
            objectSpawned = true;

            Debug.Log("Instantiated without planetracking");
            setallplaneinactive();
            panel.SetActive(false);
            StartCoroutine(turnofftutorial());
        }


    }


    IEnumerator turnofftutorial()
    {
        tutorial.SetActive(false);
        yield return new WaitForSeconds(1);
        
        poistionimage.SetActive(false);
        shutterbutton.SetActive(true);
        facetoggle.SetActive(true);
    }



	




}
