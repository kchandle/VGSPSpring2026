using UnityEngine;
using UnityEngine.Events;
public class cardPickup : MonoBehaviour
{
     public InventoryCard card;
     public UnityEvent GetCard = new UnityEvent();
     private Inventory inventory;

    public void Awake()
    {
      inventory = GameObject.Find("Player").GetComponent<Inventory>();
    }
    // Gets playerInventory.

    public void getCard()
    {
        inventory.AddCardToDeck(card);
        // Adds card to deck
        GetCard.Invoke();
        // Deletes the object because you only get the card ONCE!!!!!!!
        Debug.Log ("Card Got!");
    }

    // Puts card in inventroy and deletes :)
    // getCard is called by playerInteract

}
