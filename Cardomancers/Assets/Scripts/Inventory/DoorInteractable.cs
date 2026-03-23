using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class DoorInteractable : MonoBehaviour
{
    [SerializeField] string sceneToLoad;
    SceneLoaderAsync async;
    bool opened = false;

    public void OpenDoor()
    {
        if (opened)
        {   return;
        }

        LoadingScreen.Instance.LoadNewScene(sceneToLoad);

        opened = true;
    }
}
