using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    public void loadLevelNow(int x) 
    {
       SplashLoad.loadlevel = 1;
        PlayerPrefs.SetInt("loadlevel",SplashLoad.loadlevel);
        PlayerPrefs.Save();
        SceneManager.LoadScene(x);
    }
}
