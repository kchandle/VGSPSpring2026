using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScroll : MonoBehaviour
{
    private Vector2 zoomInput;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    /*public void OnZoom(InputAction.CallbackContext context)
    {
        zoomInput = context.ReadValue<Vector2>();

        if(zoomInput.y > 0)
        {
            print("Scroll Up");
            mainCamera.orthographicSize -= zoomInput.y;
        }
        else if(zoomInput.y < 0)
        {
            print("Scroll Down");
            mainCamera.orthographicSize += zoomInput.y;
        }
    }*/

    public void OnZoom(InputValue value)
    {
        zoomInput = value.Get<Vector2>();

        if(zoomInput.y > 0)
        {
            //print("Scroll Up");
            mainCamera.fieldOfView -= zoomInput.y;
            print(mainCamera.fieldOfView);
        }
        else if(zoomInput.y < 0)
        {
            print("Scroll Down");
            mainCamera.fieldOfView += zoomInput.y;
        }
    }

}




/*//print(value);
        //Vector3 temp = new Vector3(0, 0, Mathf.Clamp(transform.position.z - zoomInput, -10f, 0));

        transform.localPosition = new Vector3(0, 0, transform.localPosition.z + zoomInput);
        print(transform.localPosition.z);


        Vector3 pos = transform.localPosition;
        pos.z = Mathf.Clamp(pos.z, -7f, 0f);
        transform.localPosition = pos;


        //transform.position += transform.forward * Mathf.Clamp(zoomInput - 0.5f, -1f, 1f);
        //print(transform.forward.z);
        //transform.position = new Vector3(      Mathf.Clamp(transform.localPosition.z + ( zoomInput - 0.5f), 0f, -6f));*/