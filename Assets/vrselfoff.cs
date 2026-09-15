using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vrselfoff : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(tuenoff());
    }
    IEnumerator tuenoff()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
