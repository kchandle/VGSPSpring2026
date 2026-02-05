/*using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using UnityEngine.UI;

public class TrashCard : Playspace
{
    //reference to the popup UI element that gives the choice to delete the card
    [SerializeField] Image popup;

    //References the card being placed in the trash area. The trash space will only ever have one card at a time.
    private InventoryCard card;


    //Upon "addition" to the trash space, show popup asking if card should be deleted
    public GameObject NewPlayItem(GameObject prefab, Card_SO cardSO, InventoryCard inventoryCard)
    {

        //print("Spawning this Card: " + cardSO.name);
        GameObject newPlayItem = Instantiate(prefab);
        newPlayItem.transform.SetParent(transform);

        playItems.Add(newPlayItem.GetComponent<PlayItem>());
        newPlayItem.GetComponent<Card>().CardSO = cardSO;
        newPlayItem.GetComponent<Card>().inventoryCard = inventoryCard;


        //popup that asks if the card should be deleted and presents yes and no buttons
        //both buttons disable the popup afterwards
        popup.gameObject.SetActive(true);
        

        return newPlayItem;
    }


    //Pressing Yes on the popup destroys the card and removes it from the inventory.
    public void YesTrashCard()
    {
        playItems.Get(0);
    }


    //Pressing No will move the card to the "Not in Deck" space.
    public void RemoveCardFromSpace()
    {
        return;
    }
    
}*/
