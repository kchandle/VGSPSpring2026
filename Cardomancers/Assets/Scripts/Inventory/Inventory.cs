using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	// the amount of money the player has
	[SerializeField] private int money;
	// canvas for the inventory ui
	[SerializeField] private Canvas canvas;
	// prefab of the card
	[SerializeField] private GameObject inventoryCardPrefab;

	// all the cards the player has
	[SerializeField] private List<InventoryCard> inventory;
	// cards that can be used when in battle
	[SerializeField] private List<InventoryCard> deck;
	// total size of inventory
	[SerializeField] private int inventoryLength;
	// amount of cards the player is alowed to have in their deck at one time
	[SerializeField] private int deckLength;

	//inventory_so reference
	[SerializeField] private Inventory_SO inventorySO;

    public List<InventoryCard> Deck
    {
        get { return deck; }
    }


	//Use inventory_so variables for the variables in here
	private void Awake()
	{
		inventory = inventorySO.Inventory;
		deck = inventorySO.Deck;
		inventoryLength = inventorySO.InventoryLength;
		deckLength = inventorySO.DeckLength;
	}

	// all add card to x methods return true if card is successfully added to inventory otherwise returns false

	public bool AddCardToInventory(Card card)
	{
		// stops the method and returns false if the inventory is full
		if (inventory.Count >= inventoryLength) return false;
		// very temporary
		InventoryCard newInventoryCard = new InventoryCard(card);
		// add new card to deck
		inventory.Add(newInventoryCard);
		// automatically add to deck if possible
		if (deck.Count >= deckLength) AddCardToDeck(newInventoryCard);
		// sync inventory with the SO
		inventorySO.Inventory = inventory;
		return true;
	}

	public bool AddCardToDeck(InventoryCard card)
	{
		//don't add a card to the deck if the deck is full
		if (deck.Count >= deckLength) return false;
		// stop if the inventory doesn't contain the card
		if (!inventory.Contains(card)) return false;
		deck.Add(card);
		// sync deck with the SO
		inventorySO.Deck = deck;
		return true;
	}

	public void RemoveCardFromInventory(InventoryCard card)
	{
		deck.Remove(card);
		// sync with SO
		inventorySO.Inventory = inventory;
	}

	public void RemoveCardFromDeck(InventoryCard card)
	{
		deck.Remove(card);
		// sync with the SO
		inventorySO.Deck = deck;
	}

	public List<InventoryCard> Shuffle(List<InventoryCard> deck)
	{
		for (int i = 0; i < deck.Count; i++)
		{
			InventoryCard temp = deck[i];
			int randomIndex = Random.Range(i, deck.Count);
			deck[i] = deck[randomIndex];
			deck[randomIndex] = temp;
		}
        return deck;
    }

	//Currently a function that will not be called, but is here in a case that would require the player to pick a random card
	public InventoryCard DrawCards()
    {
        // Pick random card from deck then remove from deck
        InventoryCard card = deck[Random.Range(0, deck.Count)];
        deck.Remove(card);
        //Temp line to show what card was picked
        Debug.Log(card);
        return card;
    }
}
