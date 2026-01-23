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
	[SerializeField] private int inventorySize;
	// amount of cards the player is alowed to have in their deck at one time
	[SerializeField] private int deckSize;

	// all add card to x methods return true if card is successfully added to inventory otherwise returns false

	public bool AddCardToInventory(Card card)
	{
		// stops the method and returns false if the inventory is full
		if(inventory.Count >= deckSize) return false;
		// very temporary
		newInventoryCard = new InventoryCard(card);
		// add new card to deck
		inventory.Add(newInventoryCard);
		// automatically add to deck if possible
		if(deck.Count >= deckSize) AddCardToDeck(newInventoryCard);
		return true;
	}

	public bool AddCardToDeck(InventoryCard card)
	{
		//don't add a card to the deck if the deck is full
		if(deck.Count >= deckSize) return false;
		// stop if the inventory doesn't contain the card
		if(!inventory.Contains(card)) return false;
		deck.Add(card);
		return true;
	}

}
