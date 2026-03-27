using UnityEngine;
using System.IO;


public static class SaveSystem
{
    // The file path data is saved to
    private static string DataPath =>  Path.Combine(Application.persistentDataPath, "save-data.json");

    private static readonly string key = "cardomancers";
    private static readonly EncryptionService encryption = new EncryptionService(key);

    // Takes in an inventory SO and the game object for the player and turns it into a JSON file
    public static void Save(GameObject player)
    {
        Debug.Log("Saving");
        // Creates an instance of the InventoryData class using the input
        SaveData data = new SaveData(player);

        // Serialize save data into JSON
        string json = JsonUtility.ToJson(data);

        string encryptedJson = encryption.Encrypt(json);
        
        Debug.Log(DataPath);
        // Creates or overwrites save file with readable file structure
        File.WriteAllText(DataPath, encryptedJson);
    }

    // Takes in an inventory SO and assigns its data based on the saved data
    public static void Load(GameObject player)
    {
        Debug.Log(File.Exists(DataPath));
        // Ends function if there is no save data
        if (!File.Exists(DataPath)) return;
        Debug.Log(DataPath);

        // where data is assigned
        SaveData data = JsonUtility.FromJson<SaveData>(encryption.Decrypt(File.ReadAllText(DataPath)));
        Inventory.InventoryList = data.inventory;
        Inventory.Deck =  data.deck;
        Inventory.DeckSize = data.deckLength;
        Inventory.InventorySize = data.inventoryLength;

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        player.transform.position = data.position;
        player.transform.rotation = Quaternion.Euler(data.rotation);

        if (cc != null) cc.enabled = true;
        
        // Update exp data based on save file
        ExpLevels.UpdateExpData(data.currentLevel, data.expToNextLevel, data.currentExp, data.skillPoints);

        //GameObject.FindGameObjectWithTag("PlayerInventory").GetComponent<Inventory>().ValidateDeckIntegrity();
        //GameObject.FindGameObjectWithTag("PlayerInventory").GetComponent<Inventory>().ValidateInventoryIntegrity();

        QuestManager questManager = player.GetComponent<QuestManager>();
        foreach(QuestData questData in data.questData)
        {
            questManager.LoadQuest(questData);
        }
    }
}
