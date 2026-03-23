using UnityEngine;
using UnityEngine.Events;
public class cardPickup : MonoBehaviour
{
     public InventoryCard card;
     public UnityEvent GetCard = new UnityEvent();
     public bool added; 

    public void GetCardNow()
    {
        added = Inventory.AddCardToInventory(card);
        

        // Deletes the object because you only get the card ONCE!!!!!!!
        Debug.Log("Card Got!");
        if (added) GetCard.Invoke();
    }
    

    // Puts card in inventroy and deletes :)
    // GetCardNow is called by playerInteract

}
