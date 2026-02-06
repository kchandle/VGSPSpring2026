using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Also attached to CameraBase

//PlayerCamera controls movement, this only controls zoom.
//The two scripts can be merged, they're only kept apart for neatness.
public class CameraScroll : MonoBehaviour
{
    private Vector2 zoomInput;
    private Camera mainCamera;
    private bool clipping = false;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    //Unity Input System returns positive or negative value based on if the mouse is scrolled up or down
    public void OnZoom(InputValue value)
    {
        zoomInput = value.Get<Vector2>();
        transform.localPosition = new Vector3(0, 0, Mathf.Clamp(transform.localPosition.z + zoomInput.y, -10f, 0f));
    }

    
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) return;
        clipping = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) return;
        clipping = false;
    }

    //make camera zoom in when inside a wall to prevent clipping through it
    void FixedUpdate()
    {
        if(clipping)
        {
            transform.localPosition = new Vector3(0, 0, Mathf.Clamp(transform.localPosition.z + 1f, -10f, 0f));
        }
    }

    
}