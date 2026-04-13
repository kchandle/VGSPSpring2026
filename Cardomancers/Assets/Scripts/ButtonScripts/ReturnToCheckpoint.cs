using UnityEngine;
using System.Collections;

public class ReturnToCheckpoint : MonoBehaviour
{

    public void checkpointReturn()
    {
        if (GameObject.FindWithTag("Player") != null)
        {
            print("I hate");
            
            if (LoadingScreen.Instance.updating) return;
            if (LoadingScreen.Instance.alphaCor != null) StopCoroutine(LoadingScreen.Instance.alphaCor);
            LoadingScreen.Instance.ResetLoadingScreen();
            LoadingScreen.Instance.StartCoroutine(LoadingScreen.Instance.UpdateLoadingBar(1f));

            SaveSystem.Load(GameObject.FindWithTag("Player"));

        }
    }
}
