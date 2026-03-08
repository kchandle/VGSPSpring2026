using UnityEngine;
using System.Collections.Generic;

public class EmptyDeck : MonoBehaviour
{
    // puts all cards in deck into inventory
    public void emptyDeck()
    {
        List<InventoryCard> tempCardList = new List<InventoryCard>();
        foreach (InventoryCard card in Inventory.Deck)
        {
            tempCardList.Add(card);
        }
        foreach (InventoryCard card in tempCardList)
        {
            Inventory.RemoveCardFromDeck(card);
        }
    }
}
