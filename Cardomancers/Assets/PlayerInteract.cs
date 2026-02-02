using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{

    //gets interact key reference from the input system:
    public InputActionReference UIInteract;

  

    // the range of the area player can interact with things in:
     public int range = 5;

   
    //get the interact referance and set it to another variable, if the player presses:
     void OnEnable()
    {
        UIInteract.action.started += InteractKey;
    }

    //if the interactkey is set to being interacted or whatever, basically if u press the key:
     void InteractKey(InputAction.CallbackContext obj)
    {
        // sends an array thing to get all objects:
        Collider[] col = Physics.OverlapSphere(transform.position, range);
        {
            //If object is interactable, so basically if it has the interactable object script, do what it needs to do:
            foreach (Collider c in col)
            {
                if (c.TryGetComponent(out InteractableObject inter))
                {
                    inter.interactable.Invoke();
                }
            }  
        }   


    }

}
