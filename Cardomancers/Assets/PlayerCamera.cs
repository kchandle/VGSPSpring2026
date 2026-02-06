using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public float ZoomSpeed = 15f;
    public float ZoomLerpSpeed = 5f;
    public float MinDistance = 3f;
    public float MaxDistance = 15f;

    private CinemachineCamera cam;
    private CinemachineOrbitalFollow orbital;
    private float scrollDelta = 0f;
    private Vector2 scrollPosition;

    private float targetZoom;
    private float currentZoom;

    void Start()
    {
        cam = GetComponent<CinemachineCamera>();
        orbital = cam.GetComponent<CinemachineOrbitalFollow>();

        targetZoom = orbital.Radius;
        currentZoom = targetZoom;
    }

    private void HandleMouseScroll()
    {
        scrollDelta += Input.GetAxisRaw("Mouse ScrollWheel");
    }

    void Update()
    {
        HandleMouseScroll();

        if(orbital != null)
        {   targetZoom = Mathf.Clamp(orbital.Radius - (scrollDelta * ZoomSpeed), MinDistance, MaxDistance);
        }

        currentZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * ZoomLerpSpeed);
        scrollDelta = Mathf.Lerp(scrollDelta, 0, Time.deltaTime * ZoomLerpSpeed);

        orbital.Radius = currentZoom;

        Debug.Log(currentZoom);
    }
}