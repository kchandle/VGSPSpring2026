using UnityEngine;
using System.IO;

public static class InventorySaveSystem
{
    // The file path data is saved to
    private static string dataPath =>  Path.Combine(Application.persistentDataPath, "inventory.json");

    // Takes in an inventory SO and turns it into a JSON file
    public static void Save(Inventory_SO inventory)
    {
        // Creates an instance of the InventoryData class using the Inventory_SO that was input
        InventoryData data = new InventoryData
        {
            inventory = inventory.Inventory,
            inventoryLength = inventory.InventoryLength,
            deck = inventory.Deck,
            deckLength = inventory.DeckLength
        };

        // Creates or overwrites save file with readable file structure
        File.WriteAllText(dataPath, JsonUtility.ToJson(data, true));
    }

    // Takes in an inventory SO and assigns its data based on the saved data
    public static void Load(Inventory_SO inventory)
    {
        // Ends function if there is no save data
        if (!File.Exists(dataPath)) return;
       
        // where data is assigned
        InventoryData data = JsonUtility.FromJson<InventoryData>(File.ReadAllText(dataPath));
        inventory.Inventory = data.inventory;
        inventory.Deck =  data.deck;
        inventory.DeckLength = data.deckLength;
        inventory.InventoryLength = data.inventoryLength;
    }
}
