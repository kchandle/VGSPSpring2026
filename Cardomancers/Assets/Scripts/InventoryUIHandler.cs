using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
public class InventoryUIHandler : MonoBehaviour
{

    public Canvas canvas;
    public Inventory inventory; //set in editor

    public CardDragInput cardDragInput; //set in editor

    public Playspace invPlayspace;  //set in editor
    public Playspace deckPlayspace;  //set in editor

    public Playspace hackPlayspace;  //set in editor

    public GameObject cardPrefab;
    public GameObject hackPrefab;

    public bool uiDisplayed; // is the ui currently on screen?


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
        if(uiDisplayed == false)
        {
            uiDisplayed = true;
            canvas.gameObject.SetActive(true);
            
            cardDragInput.AddActivePlayspace(invPlayspace);
            cardDragInput.AddActivePlayspace(deckPlayspace);
        
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

            StartCoroutine(cardDragInput.DragDrop());
        }

    }

// Called when the player drags a card into the deckPlayspace
    public bool AttemptAddToDeck(Card card)
    {
  
        bool addedToDeck = inventory.AddCardToDeck(card.inventoryCard);
        if (addedToDeck == true) return true;
        else
        {
            print("failed to add card to deck");
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
        print("Card dragged into playspace. To playspace: " + to);
        if(to == invPlayspace) CardDraggedIntoInventory(playItem, from);
        if(to == deckPlayspace) CardDraggedIntoDeck(playItem, from);
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

// IN PROGRESS, DO NOT USE

    // public void DisplayUI(Playspace playspace, List<Hack_SO> list)
    // {
    //     foreach(Hack_SO hack in list)
    //     {
    //         playspace.NewPlayItem(hackPrefab);
    //     }
    // }


// destroy all inventory ui
    public void DestroyUI()
    {
        if (uiDisplayed == true)
        {
            uiDisplayed = false;
           print("destroying inv ui");
        StopCoroutine(cardDragInput.DragDrop());
        // loop through each playspace and destroy all playItems
        // you must use for loops b/c foreach loops will error when deleting items from the collection

        // deleting all child objects manually to be sure nothing is leftover

        deckPlayspace.playItems.Clear();
    
        for( int i = deckPlayspace.gameObject.transform.childCount-1 ;  i > 0 ; i-- )
        {
            GameObject.Destroy(deckPlayspace.gameObject.transform.GetChild(i).gameObject);
        }

        invPlayspace.playItems.Clear();

        for( int i = invPlayspace.gameObject.transform.childCount-1 ;  i > 0 ; i-- )
        {
            GameObject.Destroy(invPlayspace.gameObject.transform.GetChild(i).gameObject);
        }

        hackPlayspace.playItems.Clear();

        //try this code later
        
        //while (transform.childCount > 0) {
       // DestroyImmediate(transform.GetChild(0).gameObject);
}

        for( int i = hackPlayspace.gameObject.transform.childCount-1 ;  i > 0 ; i-- )
        {
            GameObject.Destroy(hackPlayspace.gameObject.transform.GetChild(i).gameObject);
        }



        cardDragInput.RemoveActivePlayspace(invPlayspace);
        cardDragInput.RemoveActivePlayspace(deckPlayspace);
        canvas.gameObject.SetActive(false); 
        }
        
    }
  
  

}
