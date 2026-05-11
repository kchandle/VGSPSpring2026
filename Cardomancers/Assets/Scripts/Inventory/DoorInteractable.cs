using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class DoorInteractable : MonoBehaviour
{
    public Transform teleportPosition;

    public void OpenDoor()
    {
        if (LoadingScreen.Instance.updating) return;
        Transform player = GameObject.FindWithTag("Player").transform;
        CharacterController cc = player.GetComponent<CharacterController>();
        cc.enabled = false;
        player.transform.position = teleportPosition.position;
        cc.enabled = true;
        if (LoadingScreen.Instance.alphaCor != null) StopCoroutine(LoadingScreen.Instance.alphaCor);
        LoadingScreen.Instance.ResetLoadingScreen();
        LoadingScreen.Instance.StartCoroutine(LoadingScreen.Instance.UpdateLoadingBar(1f));
    }
}
