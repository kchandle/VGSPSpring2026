using UnityEngine;
using System.Collections.Generic;

public class EmptyDeck : MonoBehaviour
{
    private Inventory inventory;
    
    public void Awake()
    {
      inventory = GameObject.FindWithTag("PlayerInventory").GetComponent<Inventory>();
    }
    
    // puts all cards in deck into inventory
    public void emptyDeck()
    {
        List<InventoryCard> tempCardList = new List<InventoryCard>();
        foreach (InventoryCard card in inventory.Deck)
        {
            tempCardList.Add(card);
        }
        foreach (InventoryCard card in tempCardList)
        {
            inventory.RemoveCardFromDeck(card);
        }
    }
}
