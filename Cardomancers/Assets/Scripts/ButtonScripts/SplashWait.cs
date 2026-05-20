using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashWait : MonoBehaviour
{
    public float waitTime = 7f;
    public string sceneName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
         yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene(sceneName);
    }
}
