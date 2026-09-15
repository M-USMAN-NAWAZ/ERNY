using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class sceneloading : MonoBehaviour
{
    public GameObject prbell;
    public static int prcheck;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void mainmenu()
    {
        SceneManager.LoadScene("4. MainScreens");
    }





    // Update is called once per frame
    void Update()
    {
        
        if(prcheck==0)
        {
            prbell.SetActive(false);
        }
        else
        {
            prbell.SetActive(true);

        }
    }
}
