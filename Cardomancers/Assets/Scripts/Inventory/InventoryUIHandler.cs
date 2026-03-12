using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
//using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine.UI;

public class InventoryUIHandler : MonoBehaviour
{

    public Canvas canvas;

    public CardDragInput cardDragInput; //set in editor

    public Playspace invPlayspace;  //set in editor
    public Playspace deckPlayspace;  //set in editor
    
    public Playspace hackCombinePlayspace;
    public Playspace cardCombinePlayspace;

    
    public Playspace hackPlayspace;  //set in editor
    public Playspace trashPlayspace;
    public Image deleteCardPopup;

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
            cardDragInput.AddActivePlayspace(trashPlayspace);
            cardDragInput.AddActivePlayspace(hackCombinePlayspace);
            cardDragInput.AddActivePlayspace(cardCombinePlayspace);
        
            // Ensure that inventory and deck have no duplicates
            //inventory.ValidateInventoryIntegrity(); 
            //inventory.ValidateDeckIntegrity();

            // Add deck cards
            foreach(InventoryCard card in Inventory.Deck)
            {
                print(card);   
                GameObject newCard = deckPlayspace.NewPlayItem(cardPrefab, card.cardSO, card);
            }

            List<InventoryCard> notInDeck = Inventory.InventoryList.Where(card => 
            !Inventory.Deck.Any(deckCard => deckCard.cardID == card.cardID))
            .ToList();
 
            foreach (InventoryCard card in notInDeck)
            {
                print(card);
                GameObject newCard = invPlayspace.NewPlayItem(cardPrefab, card.cardSO, card);
            } 

            foreach (Hack_SO hack in Inventory.HackInventory)
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
  
        bool addedToDeck = Inventory.AddCardToDeck(card.inventoryCard);
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
        Inventory.RemoveCardFromDeck(card.inventoryCard);
    }

    public void CardDraggedIntoPlayspace(PlayItem playItem, Playspace to, Playspace from)
    {
        print("Card dragged into playspace. To playspace: " + to);
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
    //Make popup appear when trashing card asking player to confirm. If they confirm, the button will call TrashCard. Else, the card will go back to where it came from with ReturnCard. The popup disappears regardless.
    private PlayItem trashItem;
    private Playspace returnSpace;
    public void CardDraggedIntoTrash(PlayItem playItem, Playspace originPlayspace)
    {
        //trashItem = playItem;
        //trash playspace will only have at most one item in it, the one actively being deleted

        trashItem = trashPlayspace.playItems[0]; //active version of the playItem passed in
        returnSpace = originPlayspace;
        print(trashItem);

        deleteCardPopup.gameObject.SetActive(true);

        cardDragInput.DragDropActive = false;
    }

    //Method called by the popup's confirm button On Click event
    //Remove the card to be deleted from inventory, then remove it from the trash playspace, fully discarding it.
    public void TrashCard()
    {
        Inventory.RemoveCardFromInventory(((Card)trashItem).inventoryCard);
        trashPlayspace.playItems.Remove(trashItem);
        Destroy(trashItem.gameObject);
       
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
        switch (button.name)
        {
            case("RetryButton"):
            {
                print("Retry");
                SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
                break;
            }
            case ("ApplyHack"):
            {
                Inventory.CardSlot = ((Card)cardCombinePlayspace.playItems[0]);
                Inventory.HackSlot = (hackCombinePlayspace.playItems[0]);
                Inventory.HackCard();
                break;
            }
        }
    }
    
    
  
    }






  
  


