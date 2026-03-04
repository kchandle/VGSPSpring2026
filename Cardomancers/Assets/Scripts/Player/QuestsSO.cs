using System;
using System.Collections.Generic;
using UnityEngine;
using static QuestsSO;

[CreateAssetMenu(fileName = "QuestsSO", menuName = "Scriptable Objects/QuestsSO")]
public class QuestsSO : ScriptableObject
{

    public string questID;
    public string questName;
    public string description;
    public int requiredLevel;
    public int expReward;
    public int moneyReward;

    //private Inventory inventory;

    public QuestObjectives questObjectives = new();


    public List<QuestObjectives> objective;

    private void OnValidate()
    {
        //basically if there isn’t a quest ID, it gets one randomly, so there isn’t multiple quests with the same ID
        if (string.IsNullOrEmpty(questID))
        {
            questID = questName + Guid.NewGuid().ToString();
        }
    }

    
    //void Awake()
    //{
    //    inventory = GameObject.Find("PlayerInventory").GetComponent<Inventory>();
    //}

    [System.Serializable]
    public class QuestObjectives
    {
        //things that are needed to know a quest/complete one
        public string description;
        public string objectiveID;
        public ObjectiveType type;
        public int requiredAmount;
        public int currentAmount;

        public bool IsCompleted => currentAmount >= requiredAmount;
    }
    //different things you can make quests for
    public enum ObjectiveType { CollectMoney, CollectCards, DefeatEnemy, }

    //checks the quest’s progress
    [System.Serializable]
    public class questProgress
    {
        public QuestsSO quest;
        public List<QuestObjectives> objectives;

        public questProgress(QuestsSO quest)
        {
            this.quest = quest;
            objectives = new List<QuestObjectives>();

            foreach (var obj in quest.objective)
            {
                objectives.Add(new QuestObjectives
                {
                    objectiveID = obj.objectiveID,
                    description = obj.description,
                    type = obj.type,
                    requiredAmount = obj.requiredAmount,
                    currentAmount = 0,
                });
            }

           


        }
        public bool IsCompleted => objectives.TrueForAll(o => o.IsCompleted);
    

        public string QuestID => quest.questID;
    }

}
