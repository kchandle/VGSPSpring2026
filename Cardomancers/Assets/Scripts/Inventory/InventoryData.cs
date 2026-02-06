using System.Collections.Generic;

//Contains all the data that Inventory_SO does in a serializable form
[System.Serializable]
public class InventoryData
{
    public List<InventoryCard> inventory;
    public List<InventoryCard> deck;
    public int inventoryLength;
    public int deckLength;
}
