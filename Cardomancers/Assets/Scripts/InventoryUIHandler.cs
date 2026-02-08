using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.UI;
public class InventoryUIHandler : MonoBehaviour
{

    public Canvas canvas;
    public Inventory inventory; //set in editor

    public CardDragInput cardDragInput; //set in editor

    public Playspace invPlayspace;  //set in editor
    public Playspace deckPlayspace;  //set in editor

    public Playspace hackPlayspace;  //set in editor

    public Playspace trashPlayspace;  //set in editor
    public Image deleteCardPopup; //set in editor, inactive by default
    
    public GameObject cardPrefab;
    public GameObject hackPrefab;


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void OnEnable()
    {
       cardDragInput.PlayitemMoved += CardDraggedIntoPlayspace;
    }

    void OnDisable()
    {
        cardDragInput.PlayitemMoved -= CardDraggedIntoPlayspace;
    }


// CREATE UI
    public void DisplayUI()
    {
        // Ensure that inventory and deck have no duplicates
        inventory.ValidateInventoryIntegrity(); 
        inventory.ValidateDeckIntegrity();

    print(inventory.Deck);
        // Add deck cards
        foreach(InventoryCard card in inventory.Deck)
        {
            
            GameObject newCard = deckPlayspace.NewPlayItem(cardPrefab, card.cardSO, card);
        }

        List<InventoryCard> notInDeck = inventory.CardInventory.Where(card => 
        !inventory.Deck.Any(deckCard => deckCard.cardID == card.cardID))
        .ToList();

        foreach (InventoryCard card in notInDeck)
        {
            print(card);
            GameObject newCard = invPlayspace.NewPlayItem(cardPrefab, card.cardSO, card);
        }
    }

// Called when the player drags a card into the deckPlayspace
    public bool AttemptAddToDeck(Card card)
    {
  
        bool addedToDeck = inventory.AddCardToDeck(card.inventoryCard);
        print(addedToDeck);
        if (addedToDeck == true) return true;
        else
        {
            cardDragInput.MoveToNewPlayspace(card, invPlayspace, deckPlayspace);
            return false;
        }
    }

    public void AttemptRemoveFromDeck(Card card)
    {
        inventory.RemoveCardFromDeck(card.inventoryCard);
    }

    

    public void CardDraggedIntoPlayspace(PlayItem playItem, Playspace to, Playspace from)
    {
        if(to == invPlayspace) CardDraggedIntoInventory(playItem, from);
        if(to == deckPlayspace) CardDraggedIntoDeck(playItem, from);
        if(to == trashPlayspace) CardDraggedIntoTrash(playItem, from);
    }
    public void CardDraggedIntoInventory(PlayItem playItem, Playspace originPlayspace)
    {
        print("Card dragged into inventory");
        if (originPlayspace == deckPlayspace)
        {
            AttemptRemoveFromDeck((Card)playItem);
        }
    }

    public void CardDraggedIntoDeck(PlayItem playItem, Playspace originPlayspace)
    {
        print("Card dragged into deck");
        if (originPlayspace == invPlayspace)
        {
            AttemptAddToDeck((Card)playItem);
        }
    }

    #region Trash Card
    //Still WIP, trashing and returning cards works

    //Make popup appear when trashing card asking player to confirm. If they confirm, the button will call TrashCard. Else, the card will go back to where it came from with ReturnCard. The popup disappears regardless.
    private PlayItem trashItem; 
    private Playspace returnSpace; 
    public void CardDraggedIntoTrash(PlayItem playItem, Playspace originPlayspace)
    {
        //print("Card dragged into trash");
        if(originPlayspace == invPlayspace || originPlayspace == deckPlayspace)
        {
            //trashItem = playItem;

            //trash playspace should only have at most one item in it, the one actively being deleted
            trashItem = trashPlayspace.playItems[0]; //active version of the playItem passed in
            returnSpace = originPlayspace;

            deleteCardPopup.gameObject.SetActive(true);

            //stop cards from being dragged while trashing one to prevent uninteded results
            cardDragInput.DragDropActive = false;
        }
        //print("method done");
    }

    //Method called by the popup's confirm button On Click event
    //Remove the card to be deleted from inventory, then remove it from the trash playspace, fully discarding it.
    public void TrashCard()
    {
        inventory.RemoveCardFromInventory(((Card)trashItem).inventoryCard);
        trashPlayspace.playItems.Remove(trashItem);
        Destroy(trashItem.gameObject);
        //print("delete");
        
        trashItem = null;
        returnSpace = null;

        //trash process ended, allow cards to be dragged again
        cardDragInput.DragDropActive = true;
    }

    //Method called by the popup's no button On Click event. 
    //Returns the card to the playspace it was dragged from.
    public void ReturnCard()
    {
        cardDragInput.MoveToNewPlayspace(trashItem, returnSpace, trashPlayspace);

        trashItem = null;
        returnSpace = null;

        //trash process ended, allow cards to be dragged again
        cardDragInput.DragDropActive = true;
    }
    #endregion


// IN PROGRESS, DO NOT USE

    // public void DisplayUI(Playspace playspace, List<Hack_SO> list)
    // {
    //     foreach(Hack_SO hack in list)
    //     {
    //         playspace.NewPlayItem(hackPrefab);
    //     }
    // }


    public void DestroyUI(Playspace playspace)
    {
        foreach(PlayItem playitem in playspace.playItems)
        {
            playspace.DestroyPlayItem(playitem);
        }
    }
  
  

}
