/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerMovement movement;
    PlayerCamera camera;

    Vector2 lookInput;

    //Keybinds from new unity input system
    Vector2 moveInput;


    public Vector3 moveInputV3 => new Vector3(moveInput.x, 0, moveInput.y);



    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        camera = GetComponentInChildren<PlayerCamera>();
    }

    //Moves camera with mouse
    void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();

        camera.PitchInput = lookInput.y;
        camera.YawInput = lookInput.x;

    }

    //Space to jump
    public void OnJump(InputValue value)
    {
        if(value.isPressed && movement.grounded)
        {
            //Debug.Log("jumping");
            //reset vertical velocity for better jumps
            movement.GetComponent<Rigidbody>().linearVelocity = new Vector3(movement.GetComponent<Rigidbody>().linearVelocity.x, 0, movement.GetComponent<Rigidbody>().linearVelocity.z);

            //jump force
            movement.GetComponent<Rigidbody>().AddForce(Vector3.up * (movement.GetGrounded() ? movement.GetJumpForce() : 0f), ForceMode.Impulse);
        }
    } 



    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        //Debug.Log("moving");
    }



    void Update()
    {
        //Camera and player rotation with mouse
        Vector3 forward = Camera.main.transform.forward * moveInput.y; 
        Vector3 right = Camera.main.transform.right * moveInput.x; 
        movement.moveDirection = forward + right;
    }
}*/