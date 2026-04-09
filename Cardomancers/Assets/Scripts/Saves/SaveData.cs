using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

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
    
    #region Quests
    public List<QuestData> questData = new List<QuestData>();
    #endregion
    
    public  SaveData(GameObject player)
    {
       inventory = Inventory.InventoryList;
       deck = Inventory.Deck;
       inventoryLength = Inventory.InventorySize;
       deckLength = Inventory.DeckSize;
       
       position = player.transform.position;
       rotation = player.transform.eulerAngles;

       currentLevel = ExpLevels.CurrentLevel;
       expToNextLevel = ExpLevels.CurrentLevel * 50;
       currentExp = ExpLevels.CurrentExp;
       skillPoints = ExpLevels.CurrentLevel * 5;

       QuestManager questManager = Object.FindFirstObjectByType<QuestManager>();
       if (questManager != null)
       {
           foreach (Quest quest in questManager.QuestMap.Values)
           {
               questData.Add(quest.GetQuestData());
           }
       }
       else
       {
           throw new NullReferenceException();
       }
    }
}
