using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class limitscale : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale =  new Vector3 (Mathf.Clamp(transform.localScale.x, 0.5f, 1.2f), Mathf.Clamp(transform.localScale.y, 0.5f, 1.2f), Mathf.Clamp(transform.localScale.z, 0.5f, 1.2f));
        
    }
}
