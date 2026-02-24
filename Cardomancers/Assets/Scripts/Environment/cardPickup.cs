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
      print("Get card function call");
      // Adds extra value that makes it get added to deck if inventory priority isnt true
        if (inventory.inventoryPriority){
          added = inventory.AddCardToInventory(card);
        } 
        else 
        {
          added = inventory.AddCardToInventory(card, true);
        }

        // Deletes the object because you only get the card ONCE!!!!!!!
        Debug.Log("Card Got!");
        if (added) GetCard.Invoke();
    }

    // Puts card in inventroy and deletes :)
    // getCard is called by playerInteract

}
