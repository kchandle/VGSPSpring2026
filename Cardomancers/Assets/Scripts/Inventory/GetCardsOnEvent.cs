using UnityEngine;
using System.Collections.Generic;

public class GetCardsOnEvent : MonoBehaviour
{
    public List<Card_SO> cardsToGet;
    public List<Hack_SO> hacksToGet;
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

	foreach (Hack_SO hack in hacksToGet)
        {
            Inventory.AddHackToInventory(hack);
        }

        cardsToGet.Clear();
	hacksToGet.Clear();
        player.GetComponent<PlayerInteract>().interacting = false;
    }
}