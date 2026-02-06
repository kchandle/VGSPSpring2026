using UnityEngine;
using System.IO;
using System.Text;

public static class InventorySaveSystem
{
    private static string dataPath =>  Path.Combine(Application.persistentDataPath, "inventory.json");

    public static void Save(Inventory_SO inventory)
    {
        Debug.Log(dataPath);
        Debug.Log(JsonUtility.ToJson(inventory, true));
        InventoryData data = new InventoryData
        {
            inventory = inventory.Inventory,
            inventoryLength = inventory.InventoryLength,
            deck = inventory.Deck,
            deckLength = inventory.DeckLength
        };

        File.WriteAllText(dataPath, JsonUtility.ToJson(data, true));
    }

    public static void Load(Inventory_SO inventory)
    {
        if (!File.Exists(dataPath)) return;
        
        InventoryData data = JsonUtility.FromJson<InventoryData>(File.ReadAllText(dataPath));
        inventory.Inventory = data.inventory;
        inventory.Deck =  data.deck;
        inventory.DeckLength = data.deckLength;
        inventory.InventoryLength = data.inventoryLength;
    }
}
