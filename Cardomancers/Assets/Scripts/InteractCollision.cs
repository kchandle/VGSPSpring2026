using UnityEngine;


public class InteractCollision : MonoBehaviour
{   
    InteractableObject interaction;

    //if in radius, player cannot interact, speaking state

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Collider>().tag == "Player")
        {
            GameStateScript.CurrentState = GameStateScript.GameState.SPEAKING;
        }
    }

    //when player leaves radius, state is walking, can interact
    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Collider>().tag == "Player")
        {
            GameStateScript.CurrentState = GameStateScript.GameState.WALKING;
        }   
    }
}
