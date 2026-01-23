using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//This script is attached to an Empty GameObject called CameraBase which is a child of the player.
//The Main Camera is a child of the CameraBase placed at x: 0 y: 0 z: -0.6

public class PlayerCamera : MonoBehaviour
{   //Controls the rotation of the Camerabase
    Vector3 rotation;

    //Values are changeable in editor, not during gameplay
    public float pitchRate = 90f;
    public float yawRate = 90f;

    //Getter/setters that are separate from the above two variables. These are changed by mouse input.
    public float PitchInput{get;set;}
    public float YawInput{get;set;}

    //Mouse Input from Unity Input System (Default Map: Player)
    Vector2 lookInput;




    //Game started or clicked on
    void OnEnable() => Cursor.lockState = CursorLockMode.Locked;
    //Esc pressed
    void OnDisable() => Cursor.lockState = CursorLockMode.None;



    //Calculations to make the camera rotate
    void Update()
    {
        //Changes the camera's x and y rotation
        float pitchInput = PitchInput * pitchRate * Time.deltaTime;
        float yawInput = YawInput * yawRate * Time.deltaTime;

        rotation.x = Mathf.Clamp(rotation.x - pitchInput, -89f, 89f);
        rotation.y += yawInput;

        //Resets so camera isn't spinning when mouse isn't moving
        PitchInput = 0f;
        YawInput = 0f;
    }



    void LateUpdate()
    {
        //Make camera position change relative to CameraBase
        transform.localRotation = Quaternion.Euler(rotation);
    }

    //Moves camera with mouse input
    void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();

        PitchInput = lookInput.y;
        YawInput = lookInput.x;
    }

    
}