using UnityEngine;
using System.IO;

public static class SaveSystem
{
    // The file path data is saved to
    private static string dataPath =>  Path.Combine(Application.persistentDataPath, "save-data.json");

    // Takes in an inventory SO and the game object for the player and turns it into a JSON file
    public static void Save(Inventory_SO inventory, GameObject player)
    {
        // Creates an instance of the InventoryData class using the Inventory_SO that was input
        SaveData data = new SaveData
        {
            inventory = inventory.Inventory,
            inventoryLength = inventory.InventoryLength,
            deck = inventory.Deck,
            deckLength = inventory.DeckLength,
            position = player.transform.position,
            rotation = player.transform.eulerAngles
        };

        // Creates or overwrites save file with readable file structure
        File.WriteAllText(dataPath, JsonUtility.ToJson(data, true));
    }

    // Takes in an inventory SO and assigns its data based on the saved data
    public static void Load(Inventory_SO inventory, GameObject player)
    {
        // Ends function if there is no save data
        if (!File.Exists(dataPath)) return;
       
        // where data is assigned
        SaveData data = JsonUtility.FromJson<SaveData>(File.ReadAllText(dataPath));
        inventory.Inventory = data.inventory;
        inventory.Deck =  data.deck;
        inventory.DeckLength = data.deckLength;
        inventory.InventoryLength = data.inventoryLength;
        player.transform.position = data.position;
        player.transform.rotation = Quaternion.Euler(data.rotation);
    }
}
