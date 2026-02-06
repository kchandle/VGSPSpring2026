using System.Collections.Generic;

[System.Serializable]
public class InventoryData
{
    public List<InventoryCard> inventory;
    public List<InventoryCard> deck;
    public int inventoryLength;
    public int deckLength;
}
