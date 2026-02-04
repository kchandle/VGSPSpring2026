using UnityEngine;
using UnityEngine.Events;
public class cardPickup : MonoBehaviour
{
     public InventoryCard card;
     public UnityEvent GetCard = new UnityEvent();
     public Inventory inventory;

    public void Awake()
    {
      inventory = GameObject.Find("Player").GetComponent<Inventory>();
    }

    void getCard()
    {
        inventory.AddCardToDeck(card);
        GetCard.Invoke();
    }

    // Puts card in inventroy and deletes :)
    // getCard is called by playerInteract

}
