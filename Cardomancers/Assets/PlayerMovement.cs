//start time: 8:46
//end time: 9:25

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Much of the code is borrowed from the fps project
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerMovement : MonoBehaviour
{
    Rigidbody rigidbody;
    CapsuleCollider capsule;


    [SerializeField] float speed;
    [SerializeField] float acceleration = 10f;
    [SerializeField] float jumpForce = 10f;


    public Vector3 moveDirection;
   
    float traceDistance => capsule.height * 0.5f;

    //Physics.SphereCast(position, radius, direction, return var, max distance of cast)
    public bool grounded => Physics.SphereCast(transform.position, capsule.radius, Vector3.down, out var hit, traceDistance);

    Vector3 flatVelocity => new Vector3(rigidbody.linearVelocity.x, 0f, rigidbody.linearVelocity.z);

    //Actual resultant movement
    Vector3 moveOutput => Vector3.ProjectOnPlane(moveDirection, Vector3.up).normalized * (speed) - flatVelocity;



    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        //Debug.Log("thing");
        //speed = entity.GetSpeed() * 8;
    }
    void FixedUpdate()
    {
        //A and D affect moveOutput's x value, W and S affect the z value
        rigidbody.AddForce(moveOutput * acceleration);
    }


    //accessors
    public bool GetGrounded()
    {
        return grounded;
    }

    public float GetJumpForce()
    {
        return jumpForce;
    }
}