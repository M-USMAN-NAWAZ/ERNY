using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screentoshow : MonoBehaviour
{
    public GameObject android, ioscreen;





    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void change()
    {
#if UNITY_IOS
        ioscreen.SetActive(true);
#endif
#if UNITY_ANDROID || UNITY_EDITOR
   android.SetActive(true);
#endif


    }

    // Update is called once per frame
    void Update()
    {
        
    }







}
