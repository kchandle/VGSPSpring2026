using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR.Haptics;

public class PlayerInteract : MonoBehaviour
{
    public bool interacting = false;
    // the range of the area player can interact with things in:
    public int range = 5;

    //The prompt that appears to indicate an object is interactable
    [SerializeField] public GameObject interactPrompt; //set in inspector
    private bool interactableInRange;

    void Awake()
    {
        interactPrompt.SetActive(false);
        interactableInRange = false;
    }

    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);

        interactableInRange = false;
        foreach(Collider collider in colliders)
        {
            if(!collider.gameObject.TryGetComponent(out InteractableObject interactable))
            {
                continue;
            }
            else
            {
                interactableInRange = true;
            }
        }

        //If there is an interactable object in range, the prompt isn't already active, and we're in the overworld
        if(interactableInRange && !interactPrompt.active && !interacting && GameStateScript.CurrentState == GameStateScript.GameState.WALKING)
        {  
            interactPrompt.SetActive(true);
            //print("set prompt active");
        }  //If there isn't an interactable object in range but the prompt is active. The prompt is also disabled when mid interaction
        else if(!interactableInRange && interactPrompt.active || interacting)
        {
            interactPrompt.SetActive(false);
            //print("set prompt inactive");
        }
        
    }


    //if the interactkey is set to being interacted or whatever, basically if u press the key:
    public void OnInteract(InputAction.CallbackContext obj)
    {
        if (!obj.started) return;
        if(interacting) 
        {
            interactPrompt.SetActive(false);
            print("Already interacting with an object");
            return;
        }
        // Checking CurrentState to make sure you can't interact while in battle
        if (GameStateScript.CurrentState == GameStateScript.GameState.INVENTORY) return;
        if (GameStateScript.CurrentState == GameStateScript.GameState.BATTLE) return;
        if (GameStateScript.CurrentState == GameStateScript.GameState.SPEAKING) return;

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
                    print("INTERACTING    " + c.gameObject.name);
                }
            }  
        }
        
 
    }

    //Just for testing, the DialogueManager will set interacting to true later
    public void ReEnableInteract()
    {
        interacting = false;
    }

}
