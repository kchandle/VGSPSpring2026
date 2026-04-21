using UnityEngine;
using System.Collections.Generic;

public class GetCardsOnEvent : MonoBehaviour
{
    public List<Card_SO> cardsToGet;
    public GameObject player;

    public void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    public void GiveCards()
    {

        foreach (Card_SO card in cardsToGet)
        {
            Inventory.AddCardToInventory(card);
        }

        cardsToGet.Clear();
        player.GetComponent<PlayerInteract>().interacting = false;
    }
}