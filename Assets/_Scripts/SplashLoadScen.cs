using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SplashLoadScen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadNextSceneAfter());
    }

    IEnumerator LoadNextSceneAfter()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(1);
    }
}
