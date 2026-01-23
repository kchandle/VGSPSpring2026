using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    CapsuleCollider capsule;
    public int range = 5;

    void Awake()
    {
        capsule = GetComponent<CapsuleCollider>();
    }

    /*void FixedUpdate()
    {
        RaycastHit hit;

        //Radius in which objects can be interacted with
        if (Physics.SphereCast(transform.position, capsule.radius * range, transform.forward, out hit))
        {
            //If object is interactable, do stuff
            if(hit.transform.gameObject.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                interactable.Interacted();
            }
        }

    }*/

}
