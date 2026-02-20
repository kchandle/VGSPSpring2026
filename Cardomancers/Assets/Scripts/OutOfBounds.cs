using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    [SerializeField] GameObject player;

    private void Awake()
    {
        //defines what the player is 
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerExit(Collider other)
    {
        //checks if the gameobject leaving the trigger is the player
        if (other.gameObject == player)
        {
            //loads the players last position without loading the inventory
            SaveSystem.Load(GameObject.FindGameObjectWithTag("PlayerInventory").GetComponent<Inventory>(), player);
        }
    }
}
