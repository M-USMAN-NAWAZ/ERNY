using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookatscript : MonoBehaviour
    
{
    public float turn_speed;
    public GameObject camer;
    // Start is called before the first frame update
    void Start()
    {
        camer = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetposition = new Vector3(camer.transform.position.x, transform.position.y, camer.transform.position.z);


        //transform.LookAt(camer.transform);





        rotateTowards(targetposition);

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
}
