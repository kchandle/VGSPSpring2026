using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class SaveCameraTest : MonoBehaviour
{

    public PlayerCamera targetScript;

    bool cameraMoveEnabled = false;
    float speed;


    // Update is called once per frame



    void cutScene()
    {
        targetScript.enabled = false;


    }
    void Update()
    {

        // this is when the camera returns to its position
        if (cameraMoveEnabled == true)
        {
            targetScript.enabled = true;
        }
    }


    public void MoveCameraBack()
    {
        cameraMoveEnabled = true;
    }



}
