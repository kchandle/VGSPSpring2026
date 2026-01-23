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
	[SerializeField] private InventoryCard[] inventory;
	// cards that can be used when in battle
	[SerializeField] private InventoryCard[] deck;
	// total size of inventory
	[SerializeField] private int inventorySize;
	// amount of cards the player is alowed to have in their deck at one time
	[SerializeField] private int deckSize;

	//returns true if card is successfully added to inventory otherwise returns false
	public bool AddCardToInventory(Card card)
	{
		// stops the method and retuurns false
		if(inventory.length + 1 > deckSize) return false;
		InventoryCard[] temp = new InventoryCard[inventory.length + 1];
	}
}
