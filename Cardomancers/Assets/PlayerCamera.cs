using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    Vector3 rotation;
    public float pitchRate = 90f;
    public float yawRate = 90f;
    public float PitchInput{get;set;}
    public float YawInput{get;set;}


    //Game started or clicked on
    void OnEnable() => Cursor.lockState = CursorLockMode.Locked;
    //Esc pressed
    void OnDisable() => Cursor.lockState = CursorLockMode.None;

    //stuff to make the camera rotate
    void Update()
    {
        float pitchInput = PitchInput * pitchRate * Time.deltaTime;
        float yawInput = YawInput * yawRate * Time.deltaTime;

        rotation.x = Mathf.Clamp(rotation.x - pitchInput, -89f, 89f);
        rotation.y += yawInput;

        //resets so camera isn't constantly spinning
        PitchInput = 0f;
        YawInput = 0f;
    }

    void LateUpdate()
    {
        //transform.rotation = Quaternion.Euler(rotation);

        //make camera position change

        transform.localRotation = Quaternion.Euler(rotation);

    }
}