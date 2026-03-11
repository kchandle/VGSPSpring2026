using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class SaveCameraTest : MonoBehaviour
{
    public Camera savedCamera;
    public Transform targetedPosition;
    bool cameraMoveEnabled = false;
    float speed;

    bool cutsceneHappened = false;

    // Update is called once per frame




    void Update()
    {


        if(cutsceneHappened == true)
        {
            MoveCamera();
        }



        // this is when the camera returns to its position
        if (cameraMoveEnabled == true)
        {
            savedCamera.transform.position = Vector3.Lerp(savedCamera.transform.position, targetedPosition.position, speed * Time.deltaTime);
            savedCamera.transform.rotation = Quaternion.Lerp(savedCamera.transform.rotation, targetedPosition.rotation, speed * Time.deltaTime);
        }
    }


    public void MoveCamera()
    {
        cameraMoveEnabled = true;
    }


}
