using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
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
            cardDragInput.AddActivePlayspace(hackPlayspace);
        
            // Ensure that inventory and deck have no duplicates
            inventory.ValidateInventoryIntegrity(); 
            inventory.ValidateDeckIntegrity();

            print(inventory.Deck);
            // Add deck cards
            foreach(InventoryCard card in inventory.Deck)
            {
                print(card);   
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

            foreach (Hack_SO hack in inventory.Hacks)
            {
                print(hack);
                GameObject newHack = hackPlayspace.NewPlayItem(hackPrefab, hack);
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


// destroy all inventory ui
    public void DestroyUI()
    {
        if (uiDisplayed == true) // only run if the UI already exists
        {
            uiDisplayed = false;
           print("destroying inv ui");
        StopCoroutine(cardDragInput.DragDrop());

        
        // clear playItems list of each playspace
        deckPlayspace.playItems.Clear();

        invPlayspace.playItems.Clear();

        hackPlayspace.playItems.Clear();

        // deleting all child objects manually to be sure nothing is leftover
        while (deckPlayspace.gameObject.transform.childCount > 0) {
        DestroyImmediate(deckPlayspace.gameObject.transform.GetChild(0).gameObject); }

        while (invPlayspace.gameObject.transform.childCount > 0) {
        DestroyImmediate(invPlayspace.gameObject.transform.GetChild(0).gameObject); }

        while (hackPlayspace.gameObject.transform.childCount > 0) {
        DestroyImmediate(hackPlayspace.gameObject.transform.GetChild(0).gameObject); }
        

        cardDragInput.RemoveActivePlayspace(invPlayspace);
        cardDragInput.RemoveActivePlayspace(deckPlayspace);
        cardDragInput.RemoveActivePlayspace(hackPlayspace);
        canvas.gameObject.SetActive(false); 
        }
    }
    
        // Battle Exiting
    public void ButtonClick(GameObject button)
    {
        if (button.name == "RetryButton") print("Retry");
       SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
  
    }






  
  


