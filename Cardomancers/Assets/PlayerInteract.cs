using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR.Haptics;

public class PlayerInteract : MonoBehaviour
{

    //gets interact key reference from the input system:
    public InputActionReference Interact;
    public bool interacting = false;
    // the range of the area player can interact with things in:
     public int range = 5;

    //if the interactkey is set to being interacted or whatever, basically if u press the key:
    public void OnInteract(InputAction.CallbackContext obj)
    {
        print("pressed interact keybind");
        if(interacting) return;
        print("interacting now");
        // Checking CurrentState to make sure you can't interact while in battle
        if (GameStateScript.CurrentState == GameStateScript.GameState.INVENTORY) return;
        print("still interacting now");
        if (GameStateScript.CurrentState == GameStateScript.GameState.BATTLE) return;
        print("still interacting now x2");
        if (GameStateScript.CurrentState == GameStateScript.GameState.SPEAKING) return;
        print("still interacting now x3");

        // sends an array thing to get all objects:
        Collider[] col = Physics.OverlapSphere(transform.position, range);
        {
            //If object is interactable, so basically if it has the interactable object script, do what it needs to do:
            foreach (Collider c in col)
            {
                if (c.TryGetComponent(out InteractableObject inter))
                {
                    print(inter);
                    interacting = true;
                    inter.interactable.Invoke();
                }
            }  
        }
        
 
    }

}
