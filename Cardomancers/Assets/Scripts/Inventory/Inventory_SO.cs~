using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory_SO", menuName = "Inventory_SO")]
public class Inventory_SO : ScriptableObject
{
    [SerializeField] private List<InventoryCard> inventory;
	[SerializeField] private List<InventoryCard> deck;
	[SerializeField] private int deckLength;
	[SerializeField] private int inventoryLength;

	public List<InventoryCard> Inventory
	{
			get => inventory;
	}

	public List<InventoryCard> Deck;
	{
			get => deck;
	}

	public int DeckLength
	{
			get => deckLength;
			set 
			{
				deckLength = value;
			}
	}

	public int InventoryLength
	{
			get => inventoryLength;
			set
			{
					inventoryLength = value;
			}
	}

}
