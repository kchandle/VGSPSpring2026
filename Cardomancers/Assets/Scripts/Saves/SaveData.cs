using System.Collections.Generic;
using UnityEngine;

//Contains all the data in a serializable form
[System.Serializable]
public class SaveData
{
    #region Inventory/Deck
    public List<InventoryCard> inventory;
    public List<InventoryCard> deck;
    public int inventoryLength;
    public int deckLength;
    #endregion
   
    #region Transform
    public Vector3 position = Vector3.zero;
    public Vector3 rotation =  Vector3.zero;
    #endregion

    #region Exp/Levels
    public int currentLevel;
    public int expToNextLevel;
    public int currentExp;
    public int skillPoints;
    #endregion
    
    public  SaveData(Inventory_SO inventorySO, GameObject player)
    {
       inventory = inventorySO.Inventory;
       deck = inventorySO.Deck;
       inventoryLength = inventorySO.InventoryLength;
       deckLength = inventorySO.DeckLength;
       
       position = player.transform.position;
       rotation = player.transform.eulerAngles;

       currentLevel = ExpLevels.CurrentLevel;
       expToNextLevel = ExpLevels.CurrentLevel * 50;
       currentExp = ExpLevels.CurrentExp;
       skillPoints = ExpLevels.CurrentLevel * 5;
    }
}
