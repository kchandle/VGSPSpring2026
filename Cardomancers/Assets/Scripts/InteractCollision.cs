using UnityEngine;
using UnityEngine.Events;

public class InteractCollision : MonoBehaviour
{
     public UnityEvent interactable;


    void OnTriggerEnter(Collider other)
    {
        //check the state, if speaking, inventory, or battle, return, if not it invokes

        if (other.tag == "Player")
        { 
            if (GameStateScript.CurrentState == GameStateScript.GameState.INVENTORY) return;
            if (GameStateScript.CurrentState == GameStateScript.GameState.BATTLE) return;
            if (GameStateScript.CurrentState == GameStateScript.GameState.SPEAKING) return;
        
            interactable.Invoke();
        }
    }
}
