using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Lean.Touch;

public class ardata : MonoBehaviour
{
    public Material defaultt, medalar;
    public static ardata instance;
    public GameObject close, away;
    public TextMeshPro name2, name, distance, time;
    public TextMeshPro victry;
    public static string ename, edistance, etime, victoryphrase, edistenaceunit, artheme;
    public MeshRenderer mesh, mesh3;
    string[] textSplit;
    public GameObject camer;
    public float turn_speed;
    public static string mname, mdistance, mtime, munit;
    public static int checks;
    public GameObject faceoff;
    public GameObject AR;
    int check;
    public GameObject p1, p2, parent;
    private Texture lastAppliedArTexture;
    private bool profileImageVisible = true;
    private bool cameraRelativeTransformLocked;

    private void Awake()
    {
        instance = this;

        if (camer == null)
        {
            camer = GameObject.FindGameObjectWithTag("MainCamera");
        }

        checks = 0;
    }

    IEnumerator death()
    {
        yield return new WaitForSeconds(8);
        if (checks == 1)
        {
            close.SetActive(true);
        }
        else
        {
            away.SetActive(true);
        }
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(8);

        if (p1 != null && profileImageVisible)
        {
            p1.SetActive(true);
        }
    }

    public void checkdata()
    {
        StartCoroutine(delay());

        ApplyVictoryText();
        name2.text = ename;
        name.text = ename;

        /*   textSplit = edistance.Split(char.Parse("."));

           if (textSplit[0] == "00")
           {
               StartCoroutine(death());
               checks = 1;
               if (textSplit[1].Length != 2)
               {
                   if (textSplit[2].Length != 2)
                   {
                       distance.text = textSplit[1]  + "." + textSplit[2] + "0" + " ";
                   }
                   else
                   {
                       distance.text = textSplit[1]  + "." + textSplit[2] + " ";

                   }
               }
               else
               {
                   if (textSplit[2].Length != 2)
                   {
                       distance.text = textSplit[1] + "." + textSplit[2] + "0" + " ";
                   }
                   else
                   {
                       distance.text = textSplit[1] + "." + textSplit[2] + " ";

                   }
               }


           }
           else
           {
               StartCoroutine(death());
               if (textSplit[0].Length != 2)
               {



                   if (textSplit[1].Length != 2)
                   {
                       if (textSplit[2].Length != 2)
                       {
                           distance.text = textSplit[0]  + "." + textSplit[1]  + "." + textSplit[2] + "0" + " ";
                       }
                   }
                   else
                   {
                       distance.text = textSplit[0] + "." + textSplit[1] + "." + textSplit[2] + " ";

                   }
               }
               else
               {
                   if (textSplit[1].Length != 2)
                   {
                       if (textSplit[2].Length != 2)
                       {
                           distance.text = textSplit[0] + "." + textSplit[1]  + "." + textSplit[2] + "0" + " ";
                       }
                   }
                   else
                   {
                       if (textSplit[2].Length != 2)
                       {
                           distance.text = textSplit[0] + "." + textSplit[1] + "." + textSplit[2] + "0" + " ";
                       }
                       else
                       {
                           distance.text = textSplit[0] + "." + textSplit[1] + "." + textSplit[2] + " ";
                       }
                   }
               }
           }

           textSplit = etime.Split(char.Parse(":"));


           if (textSplit[0] == "00")
           {
               if (textSplit[1] != "00")
               {

                   time.text = textSplit[1] + ":" + textSplit[2];
               }
               else
               {
                   time.text = textSplit[1] + ":" + textSplit[2];

               }
           }
           else
           {
               time.text = textSplit[0] + ":" + textSplit[1] + ":" + textSplit[2];

           }



           time.text = etime;

           munit = edistenaceunit;

           mname = name.text;
           mdistance = distance.text;
           mtime = time.text;*/
        if (apigetter.artexture != null)
        {
            ApplyArTextureToRenderers();
        }
        else
        {
            HideArTextureRenderers();
        }
    }

    void Start()
    {
        StartCoroutine(delay());

        ApplyVictoryText();
        name2.text = ename;
        name.text = ename;

        /*   textSplit = edistance.Split(char.Parse("."));

           if (textSplit[0] == "00")
           {
               StartCoroutine(death());
               checks = 1;
               if (textSplit[1].Length != 2)
               {
                   if (textSplit[2].Length != 2)
                   {
                       distance.text = textSplit[1]  + "." + textSplit[2] + "0" + " ";
                   }
                   else
                   {
                       distance.text = textSplit[1]  + "." + textSplit[2] + " ";

                   }
               }
               else
               {
                   if (textSplit[2].Length != 2)
                   {
                       distance.text = textSplit[1] + "." + textSplit[2] + "0" + " ";
                   }
                   else
                   {
                       distance.text = textSplit[1] + "." + textSplit[2] + " ";

                   }
               }


           }
           else
           {
               StartCoroutine(death());
               if (textSplit[0].Length != 2)
               {



                   if (textSplit[1].Length != 2)
                   {
                       if (textSplit[2].Length != 2)
                       {
                           distance.text = textSplit[0]  + "." + textSplit[1]  + "." + textSplit[2] + "0" + " ";
                       }
                   }
                   else
                   {
                       distance.text = textSplit[0] + "." + textSplit[1] + "." + textSplit[2] + " ";

                   }
               }
               else
               {
                   if (textSplit[1].Length != 2)
                   {
                       if (textSplit[2].Length != 2)
                       {
                           distance.text = textSplit[0] + "." + textSplit[1]  + "." + textSplit[2] + "0" + " ";
                       }
                   }
                   else
                   {
                       if (textSplit[2].Length != 2)
                       {
                           distance.text = textSplit[0] + "." + textSplit[1] + "." + textSplit[2] + "0" + " ";
                       }
                       else
                       {
                           distance.text = textSplit[0] + "." + textSplit[1] + "." + textSplit[2] + " ";
                       }
                   }
               }
           }

           textSplit = etime.Split(char.Parse(":"));


           if (textSplit[0] == "00")
           {
               if (textSplit[1] != "00")
               {

                   time.text = textSplit[1] + ":" + textSplit[2];
               }
               else
               {
                   time.text = textSplit[1] + ":" + textSplit[2];

               }
           }
           else
           {
               time.text = textSplit[0] + ":" + textSplit[1] + ":" + textSplit[2];

           }



           time.text = etime;

           munit = edistenaceunit;

           mname = name.text;
           mdistance = distance.text;
           mtime = time.text;*/
        if (apigetter.artexture != null)
        {
            ApplyArTextureToRenderers();
        }
        else
        {
            HideArTextureRenderers();
        }
    }

    void FixedUpdate()
    {
        if (!cameraRelativeTransformLocked && arhandler.selectar == 0)
        {
            Vector3 targetposition = new Vector3(camer.transform.position.x, transform.position.y, camer.transform.position.z);
            rotateTowards(targetposition);
        }

        if (check == 1)
        {
            faceoff = GameObject.FindGameObjectWithTag("faceoff");
            if (faceoff != null)
            {
                faceoff.SetActive(false);
            }
        }
        //  transform.LookAt(Vector3(alvo.x, transform.position.y, alvo.z));
        /*
        var adjusted = camer.transform;
        adjusted.x = 0;
        adjusted.z = 0;
        transform.LookAt(adjusted);
    */
    }

    protected void rotateTowards(Vector3 to)
    {
        Quaternion _lookRotation =
            Quaternion.LookRotation((to - transform.position).normalized);

        //over time
        transform.rotation =
            Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * turn_speed);

        //instant
        transform.rotation = _lookRotation;
    }

    private void Update()
    {
        AR = GameObject.Find("AR");
        if (AR != null)
        {
            //Debug.LogError("check error: " + AR.name);
            if (!AR.activeInHierarchy || !AR.activeSelf)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void LateUpdate()
    {
        ApplyVictoryText();

        if (!profileImageVisible)
        {
            SetProfileObjectsActive(false);
            return;
        }

        if (apigetter.artexture == null)
        {
            HideArTextureRenderers();
            return;
        }

        bool parentHidden = parent != null && !parent.activeSelf;
        bool meshHidden = mesh != null && !mesh.enabled;
        bool mesh3Hidden = mesh3 != null && !mesh3.enabled;

        if (lastAppliedArTexture != apigetter.artexture || parentHidden || meshHidden || mesh3Hidden)
        {
            ApplyArTextureToRenderers();
        }

        if (!cameraRelativeTransformLocked)
        {
            FaceProfileObjectsToCamera();
        }
    }

    public bool IsProfileImageVisible()
    {
        return profileImageVisible;
    }

    public void SetProfileImageVisible(bool visible)
    {
        profileImageVisible = visible;

        if (!visible)
        {
            SetProfileObjectsActive(false);
            return;
        }

        if (apigetter.artexture != null)
        {
            ApplyArTextureToRenderers();
        }
    }

    private void ApplyArTextureToRenderers()
    {
        check = 0;
        lastAppliedArTexture = apigetter.artexture;

        if (parent != null)
        {
            parent.SetActive(true);
            EnableChildRenderers(parent);
        }

        ApplyArTextureToRenderer(mesh);
        ApplyArTextureToRenderer(mesh3);
        if (!cameraRelativeTransformLocked)
        {
            FaceProfileObjectsToCamera();
        }
    }

    public void SetCameraRelativeTransformLocked(bool locked)
    {
        cameraRelativeTransformLocked = locked;
    }

    private void HideArTextureRenderers()
    {
        check = 1;
        lastAppliedArTexture = null;

        if (mesh != null)
        {
            mesh.material = defaultt;
        }

        if (mesh3 != null)
        {
            //mesh3.material = defaultt;
        }

        if (parent != null)
        {
            parent.SetActive(false);
        }
    }

    private void SetProfileObjectsActive(bool active)
    {
        if (p1 != null)
        {
            p1.SetActive(active);
        }

        if (p2 != null)
        {
            p2.SetActive(active);
        }

        if (parent != null)
        {
            parent.SetActive(active);
        }
    }

    private void EnableChildRenderers(GameObject root)
    {
        Renderer[] childRenderers = root.GetComponentsInChildren<Renderer>(true);

        for (int i = 0; i < childRenderers.Length; i++)
        {
            childRenderers[i].gameObject.SetActive(true);
            childRenderers[i].enabled = true;
        }
    }

    private void ApplyArTextureToRenderer(MeshRenderer targetRenderer)
    {
        if (targetRenderer == null)
        {
            return;
        }

        targetRenderer.enabled = true;
        //targetRenderer.material = medalar;
        targetRenderer.material.mainTexture = apigetter.artexture;
        targetRenderer.material.SetTexture("_EmissionMap", apigetter.artexture);
    }

    private void FaceProfileObjectsToCamera()
    {
        Transform cameraTransform = GetCameraTransform();

        if (cameraTransform == null)
        {
            return;
        }

        if (parent != null && parent.activeInHierarchy)
        {
            FaceTransformToCamera(parent.transform, cameraTransform);
            return;
        }

        if (mesh != null && mesh.gameObject.activeInHierarchy)
        {
            FaceTransformToCamera(mesh.transform, cameraTransform);
        }

        if (mesh3 != null && mesh3.gameObject.activeInHierarchy)
        {
            FaceTransformToCamera(mesh3.transform, cameraTransform);
        }
    }

    private Transform GetCameraTransform()
    {
        if (camer == null)
        {
            camer = GameObject.FindGameObjectWithTag("MainCamera");
        }

        if (camer != null)
        {
            return camer.transform;
        }

        return Camera.main != null ? Camera.main.transform : null;
    }

    private void FaceTransformToCamera(Transform target, Transform cameraTransform)
    {
        Vector3 direction = cameraTransform.position - target.position;

        if (direction.sqrMagnitude > 0.001f)
        {
            target.rotation = Quaternion.LookRotation(direction.normalized, cameraTransform.up);
        }
    }

    private void ApplyVictoryText()
    {
        if (victry == null)
        {
            return;
        }

        if (artheme == "baloon")
        {
            victry.gameObject.SetActive(true);
            return;
        }

        victry.text = victoryphrase;
    }
}
