using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class DoorInteractable : MonoBehaviour
{
    [SerializeField] string sceneToLoad;
    bool opened = false;

    public void OpenDoor()
    {
        if (opened) return;
        LoadingScreen.Instance.ResetLoadingScreen();
        SceneLoader.Instance.Load(sceneToLoad);
        StartCoroutine(WaitForSceneToFinishLoading());
        opened = true;
    }

    IEnumerator WaitForSceneToFinishLoading()
    {
        StartCoroutine(LoadingScreen.Instance.UpdateLoadingBar(0.9f));
        while (LoadingScreen.Instance.updating == true) yield return null;
        while(SceneLoader.Instance.IsSceneLoading(sceneToLoad) == false)
        {
            Debug.LogWarning("WATING FOR SCENE TO FINSIH LOADING");
            yield return null;
        }
        StartCoroutine(LoadingScreen.Instance.UpdateLoadingBar(1f));
        while (LoadingScreen.Instance.updating == true) yield return null;
        SceneLoader.Instance.SwitchScene(sceneToLoad);
    }
}