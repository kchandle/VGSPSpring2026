using UnityEngine;


public class InteractCollision : MonoBehaviour
{   
    InteractableObject interaction;

    void OnTriggerEnter(Collider other)
    {
        if(other.collider.tag == "Player")
        {
            interaction
        }
    }
}
