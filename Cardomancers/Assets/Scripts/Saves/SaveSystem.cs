using UnityEngine;
using System.IO;


public static class SaveSystem
{
    // The file path data is saved to
    private static string DataPath =>  Path.Combine(Application.persistentDataPath, "save-data.json");

    private static readonly string key = "cardomancers";
    private static readonly EncryptionService encryption = new EncryptionService(key);

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

        string encryptedJson = encryption.Encrypt(json);
        
        Debug.Log(DataPath);
        // Creates or overwrites save file with readable file structure
        File.WriteAllText(DataPath, encryptedJson);
    }

    // Takes in an inventory SO and assigns its data based on the saved data
    public static void Load(Inventory inventory, GameObject player)
    {
        Debug.Log(File.Exists(DataPath));
        // Ends function if there is no save data
        if (!File.Exists(DataPath)) return;

        // where data is assigned
        SaveData data = JsonUtility.FromJson<SaveData>(encryption.Decrypt(File.ReadAllText(DataPath)));
        inventory.InventorySO.Inventory = data.inventory;
        inventory.InventorySO.Deck =  data.deck;
        inventory.InventorySO.DeckLength = data.deckLength;
        inventory.InventorySO.InventoryLength = data.inventoryLength;
        
        inventory.CardInventory = inventory.InventorySO.Inventory;
        inventory.Deck = inventory.InventorySO.Deck;
        inventory.InventoryLength = inventory.InventorySO.InventoryLength;
        inventory.DeckLength = inventory.InventorySO.DeckLength;

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        player.transform.position = data.position;
        player.transform.rotation = Quaternion.Euler(data.rotation);

        if (cc != null) cc.enabled = true;

        GameObject.FindGameObjectWithTag("PlayerInventory").GetComponent<Inventory>().ValidateDeckIntegrity();
        GameObject.FindGameObjectWithTag("PlayerInventory").GetComponent<Inventory>().ValidateInventoryIntegrity();
    }


    public static void Load(GameObject player)
    {
        if (!File.Exists(DataPath)) return;

        SaveData data = JsonUtility.FromJson<SaveData>(File.ReadAllText(DataPath));

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        player.transform.position = data.position;

        player.transform.rotation = Quaternion.Euler(data.rotation);

        if (cc != null) cc.enabled = true;
    }
}
