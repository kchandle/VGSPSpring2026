using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    // the range of the area player can interact with things in:
     public int range = 5;

    //if the interactkey is set to being interacted or whatever, basically if u press the key:
    public void OnInteract(InputAction.CallbackContext obj)
    {
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
                    inter.interactable.Invoke();
                }
            }  
        }
    }
}