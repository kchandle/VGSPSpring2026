using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory_SO", menuName = "Inventory_SO")]
public class Inventory_SO : ScriptableObject
{
    [SerializeField] private List<InventoryCard> inventory;
	[SerializeField] private List<InventoryCard> deck;
	[SerializeField] private int deckLength;
	[SerializeField] private int inventoryLength;

	// ALL cards currently in the player's inventory, including those in the Deck
	public List<InventoryCard> Inventory
	{
			get => inventory;
			set
			{
			    inventory = value;
			}
	}

	// Cards inside of the player's deck only
	public List<InventoryCard> Deck
	{
			get => deck;
			set
			{
			    deck = value;
			}
	}

	// Max amount of cards you can have in your dck
	public int DeckLength
	{
			get => deckLength;
			set 
			{
				deckLength = value;
			}
	}
	// Max amount of cards you can have in your inventory
	public int InventoryLength
	{
			get => inventoryLength;
			set
			{
					inventoryLength = value;
			}
	}

}
