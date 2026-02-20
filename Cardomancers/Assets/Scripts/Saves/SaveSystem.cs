using UnityEngine;
using System.IO;
using System.Text;
using System.Security.Cryptography;

public static class SaveSystem
{
    // The file path data is saved to
    private static string DataPath =>  Path.Combine(Application.persistentDataPath, "save-data.json");
    // key used to encrypt save data
    private static byte[] key = new byte[32];

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

        // Serialize save data into JSON
        string json = JsonUtility.ToJson(data);
        //Convert to bytes so that the save data can be encrypted
        //byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
        //using (Aes aes = Aes.Create())
        //{
        //aes.Key = key;
        //aes.IV = aes.GenerateIV();
        //}

        Debug.Log(DataPath);
        // Creates or overwrites save file with readable file structure
        File.WriteAllText(DataPath, json);
    }

    // Takes in an inventory SO and assigns its data based on the saved data
    public static void Load(Inventory_SO inventory, GameObject player)
    {
        // Ends function if there is no save data
        if (!File.Exists(DataPath)) return;

        // where data is assigned
        SaveData data = JsonUtility.FromJson<SaveData>(File.ReadAllText(DataPath));
        inventory.Inventory = data.inventory;
        inventory.Deck =  data.deck;
        inventory.DeckLength = data.deckLength;
        inventory.InventoryLength = data.inventoryLength;
        player.transform.position = data.position;
        player.transform.rotation = Quaternion.Euler(data.rotation);
        player.GetComponent<Inventory>().ValidateDeckIntegrity();
        player.GetComponent<Inventory>().ValidateInventoryIntegrity();
    }


    public static void Load(GameObject player)
    {
        if (!File.Exists(DataPath)) return;

        SaveData data = JsonUtility.FromJson<SaveData>(File.ReadAllText(DataPath));

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        player.transform.position = data.position;
        Debug.Log(DataPath);

        player.transform.rotation = Quaternion.Euler(data.rotation);

        if (cc != null) cc.enabled = true;
    }
}
