using UnityEngine;
using System.Collections.Generic;

public class GetCardsOnEvent : MonoBehaviour
{
    public List<Card_SO> cardsToGet;
    
    public void GiveCards()
    {

        foreach (Card_SO card in cardsToGet)
        {
            Inventory.AddCardToInventory(card);
        }

        cardsToGet.Clear();
    }
}