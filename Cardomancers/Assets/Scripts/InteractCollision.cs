using UnityEngine;


public class InteractCollision : MonoBehaviour
{   
    InteractableObject interaction;

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Collider>().tag == "Player")
        {
            GameStateScript.CurrentState = GameStateScript.GameState.SPEAKING;
        }
    }
    void OnTriggerExit(Collider other)
    {
        GameStateScript.CurrentState = GameStateScript.GameState.WALKING;
    }
}
