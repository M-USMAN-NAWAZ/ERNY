using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class sceneload : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void mainmenu()
    {
        SceneManager.LoadScene("4. MainScreens");
    }


    public void AR()
    {

        SceneManager.LoadScene("5.AR");

    }







    // Update is called once per frame
    void Update()
    {
        
    }
}
