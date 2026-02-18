using System.Collections.Generic;
using UnityEngine;

//Contains all the data that Inventory_SO does in a serializable form
[System.Serializable]
public class SaveData
{
    public List<InventoryCard> inventory;
    public List<InventoryCard> deck;
    public int inventoryLength;
    public int deckLength;
    public Vector3 position = Vector3.zero;
    public Vector3 rotation =  Vector3.zero;
}
