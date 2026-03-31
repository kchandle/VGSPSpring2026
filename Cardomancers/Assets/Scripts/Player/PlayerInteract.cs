using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    public bool interacting = false;
    // the range of the area player can interact with things in:
     public int range = 5;

    private void Update()
    {
        
    }


    //if the interactkey is set to being interacted or whatever, basically if u press the key:
    public void OnInteract(InputAction.CallbackContext obj)
    {
        if (!obj.started) return;
        if(interacting) return;
        // Checking CurrentState to make sure you can't interact while in battle
        if (GameStateScript.CurrentState == GameStateScript.GameState.INVENTORY) return;
        if (GameStateScript.CurrentState == GameStateScript.GameState.BATTLE) return;
        if (GameStateScript.CurrentState == GameStateScript.GameState.SPEAKING) return;

        // sends an array thing to get all objects:
        Collider[] col = Physics.OverlapSphere(transform.position, range);
        {
            float minRange = 1000f;
            //If object is interactable, so basically if it has the interactable object script, do what it needs to do:
            foreach (Collider c in col)
            {
                InteractableObject interactable;
                if (c.TryGetComponent(out InteractableObject inter))
                {
                    float range = (inter.transform.position - transform.position).magnitude;
                    if (range < minRange)
                    {
                        interactable = inter;
                        minRange = range;
                    }
                }
                interacting = true;
                inter.interactable.Invoke();
            }  
        }
    }



}
