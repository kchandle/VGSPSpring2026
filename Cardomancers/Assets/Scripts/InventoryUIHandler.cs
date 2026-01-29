using UnityEngine;
using System.Collections.Generic;
public class InventoryUIHandler : MonoBehaviour
{

    Canvas canvas;
    Inventory inventory; //set in editor

    CardDragInput cardDragInput; //set in editor

    Playspace invPlayspace;  //set in editor
    Playspace deckPlayspace;  //set in editor

    Playspace hackPlayspace;  //set in editor

    GameObject cardPrefab;
    GameObject hackPrefab;


    // Start is called once before the first execution of Update after the MonoBehaviour is created


// CREATE UI
    public void DisplayUI(Playspace playspace, List<InventoryCard> list)
    {
        foreach(InventoryCard card in list)
        {
            playspace.NewPlayItem(cardPrefab, card.cardSO);
        }
    }

// Called when the player drags a card into the deckPlayspace
    public bool AttemptAddToDeck(Card card)
    {
  
        bool addedToDeck = inventory.AddCardToDeck(card.inventoryCard);
        if (addedToDeck == true) return true;
        else
        {
            cardDragInput.MoveToNewPlayspace(card, deckPlayspace, invPlayspace);
            return false;
        }
    }


// IN PROGRESS, DO NOT USE
    public void DisplayUI(Playspace playspace, List<Hack_SO> list)
    {
        foreach(Hack_SO hack in list)
        {
            playspace.NewPlayItem(hackPrefab);
        }
    }


    public void DestroyUI(Playspace playspace)
    {
        foreach(PlayItem playitem in playspace.playItems)
        {
            playspace.DestroyPlayItem(playitem);
        }
    }
  
  

}
