using UnityEngine;
using UnityEngine.Events;
public class cardPickup : MonoBehaviour
{
     public InventoryCard card;
     public UnityEvent GetCard = new UnityEvent();
     private Inventory inventory;
     public bool added; 

    public void Awake()
    {
      inventory = GameObject.FindWithTag("PlayerInventory").GetComponent<Inventory>();
    }
    // Gets playerInventory.

    public void getCard()
    {
        if (inventory.inventoryPriority) added = inventory.AddCardToInventory(card);
        else added = inventory.AddCardToInventory(card, true);
        // Adds card to deck
        if (added) GetCard.Invoke();
        // Deletes the object because you only get the card ONCE!!!!!!!
        Debug.Log ("Card Got!");
    }

    // Puts card in inventroy and deletes :)
    // getCard is called by playerInteract

}
