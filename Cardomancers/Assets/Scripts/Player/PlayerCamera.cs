using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using UnityEditor.ShaderGraph;

public class PlayerCamera : MonoBehaviour
{
    [Tooltip("How fast the zoom speed is?")]
    public float ZoomSpeed = 15f;
    [Tooltip("How fast we should change the zoom speed?")]
    public float ZoomLerpSpeed = 5f;
    [Tooltip("Zoom minimum distance from player (radius)")]
    public float MinDistance = 3f;
    [Tooltip("Zoom maximum distance from player (radius)")]
    public float MaxDistance = 15f;

    // Camera attached to player
    private CinemachineCamera cam;
    // Orbital sphere object magical thing from player, see radius for stuff.
    private CinemachineOrbitalFollow orbital;
    // Decollider component on the Cinemachine camera, controls smoothing time
    private CinemachineDecollider decollider;
    private CinemachineInputAxisController input;
    // Scroll delta is just lerping holder.
    private float scrollDelta = 0f;
    // Scroll position, self explanatory.
    private Vector2 scrollPosition;
    // Player, self explanatory.
    private GameObject player;
    // Target Zoom that we want.
    private float targetZoom;
    // Current zoom, being lerped...
    private float currentZoom;

    private CinemachineBrain brain;

    // setup the variables.
    void Start()
    {
        // get the cinemachine attached.
        //*set the decollider smoothing time to 2 in the inspector
        cam = GetComponent<CinemachineCamera>();
        // get the orbital attached this is like the sphere around the player that looks at them in third person..
        orbital = cam.GetComponent<CinemachineOrbitalFollow>();

        //get the decollider attached
        decollider = cam.GetComponent<CinemachineDecollider>();
        input = cam.GetComponent<CinemachineInputAxisController>();
        brain = FindFirstObjectByType<CinemachineBrain>();

        //Optimal settings to reduce motion sickness
        ZoomLerpSpeed = 5f;

        // This is just the player.
        player = GameObject.FindGameObjectWithTag("Player");

        // Get the target zoom we are using.
        targetZoom = orbital.Radius;
        // Set the current zoom, pretty obvious???.
        currentZoom = targetZoom;
    }

    private void HandleMouseScroll()
    {
        // gets the scroll input.
        scrollDelta += Input.GetAxisRaw("Mouse ScrollWheel");
    }
    
    // update camera position...
    void Update()
    {
        // Contact Group-1 team lead for this they added it, and I dont know what it does.
        input.enabled = GameStateScript.CurrentState == GameStateScript.GameState.WALKING || GameStateScript.CurrentState == GameStateScript.GameState.LOADINGSCREEN ? true : false;
        brain.enabled = GameStateScript.CurrentState == GameStateScript.GameState.WALKING || GameStateScript.CurrentState == GameStateScript.GameState.LOADINGSCREEN ? true : false;
        if (GameStateScript.CurrentState == GameStateScript.GameState.WALKING || GameStateScript.CurrentState == GameStateScript.GameState.LOADINGSCREEN) RotatePlayerModel();
    }

    public void RotatePlayerModel()
    {
        // Handles the scroll delta which is the just mouse scroll wheel input.
        HandleMouseScroll();

        // if we dont have a orbital well this doesnt work.
        if (orbital != null)
        {
            // Get the future zoom position that the player wants.
            // This is mosly just to lerp the camera...
            // Basically this just smooths the camera zoom in.
            targetZoom = Mathf.Clamp(orbital.Radius - (scrollDelta * ZoomSpeed), MinDistance, MaxDistance);
        }

        // Lerps the zoom, see above.
        currentZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * ZoomLerpSpeed);
        // Lerps the scroll, see above,
        scrollDelta = Mathf.Lerp(scrollDelta, 0, Time.deltaTime * ZoomLerpSpeed);

        // This just sets the magical orbital object that is just a sphere around the player looking at them, to the radius.
        // The radius is basically the distance from the center of the player
        orbital.Radius = currentZoom;

        // I didnt do this, contact the programming director for this, I dont know what it does.
        player.transform.rotation = Quaternion.Euler(0, cam.transform.rotation.eulerAngles.y, 0);
    }
}