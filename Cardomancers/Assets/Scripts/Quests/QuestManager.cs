using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// A MonoBehaviour which manages the quests.
/// </summary>
public class QuestManager : MonoBehaviour
{
    private Dictionary<string, Quest> questMap = new Dictionary<string, Quest>();
    
    public Dictionary<string, Quest> QuestMap { get { return questMap; } }
    
    #region Unity Methods

    private void Awake()
    {
        questMap = CreateQuestMap();
        DontDestroyOnLoad(this);
    }

    // Subscribes methods to their respective events
    private void OnEnable()
    {
        QuestEvents.OnStartQuest += StartQuest;
        QuestEvents.OnAdvanceQuest += AdvanceQuest;
        QuestEvents.OnFinishQuest += FinishQuest;
        QuestEvents.OnQuestStepStateChanged += QuestStepStateChange;
    }

    private void OnDisable()
    {
        QuestEvents.OnStartQuest -= StartQuest;
        QuestEvents.OnAdvanceQuest -= AdvanceQuest;
        QuestEvents.OnFinishQuest -= FinishQuest;
        QuestEvents.OnQuestStepStateChanged -= QuestStepStateChange;
    }

    // Update the state of every quest on start
    private void Start()
    {
        foreach (Quest quest in questMap.Values)
        {
            if (quest.state == QuestState.IN_PROGRESS)
            {
                quest.InstantiateCurrentStep(this.transform);
            }
            QuestEvents.QuestStateChanged(GetQuestByID(quest.info.ID));
        }
    }

    private void Update()
    {
        foreach (Quest quest in questMap.Values)
        {
            // Constantly check every quest if it can be started
            if (GetQuestByID(quest.info.ID).state.Equals(QuestState.REQUIREMENTS_NOT_MET) && CheckRequirementsMet(GetQuestByID(quest.info.ID)))
            {
                ChangeQuestState(quest.info.ID, QuestState.CAN_START);
            }
        }
    }
    #endregion
    
    #region Event Subscribers

    // Instantiate first quest step prefab and assign the correct quest state
    private void StartQuest(string ID)
    {
        Quest quest = GetQuestByID(ID);
        quest.InstantiateCurrentStep(this.transform);
        ChangeQuestState(quest.info.ID, QuestState.IN_PROGRESS);
    }
    
    private void AdvanceQuest(string ID)
    {
        Quest quest = GetQuestByID(ID);

        // Increment quest step
        quest.MoveToNextQuestStep();

        // Check if there is another quest step
        if (quest.CurrentQuestStepExists())
        {
            // if there is, instantiate the step prefab
            quest.InstantiateCurrentStep(this.transform);
        }
        else
        {
            // else make it so the quest can be finished
            ChangeQuestState(quest.info.ID, QuestState.CAN_FINISH);
        }
    }

    // Claims the rewards of the quest and changes the state to finished
    private void FinishQuest(string ID)
    {
        Quest quest = GetQuestByID(ID);
        ClaimRewards(quest);
        ChangeQuestState(quest.info.ID, QuestState.FINISHED);
    }
    #endregion
    
    #region Utility Methods

    // Call once on awake so all the quests are loaded
    private Dictionary<string, Quest> CreateQuestMap()
    {
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");
        Dictionary<string, Quest> newQuestMap = new Dictionary<string, Quest>();

        foreach (QuestInfoSO quest in allQuests)
        {
            if (questMap.ContainsKey(quest.ID))
            {
                Debug.LogWarning("Quest " + quest.ID + " duplicate detected");
            }
            newQuestMap.Add(quest.ID, LoadQuest(quest));
        }
        Debug.Log(newQuestMap.Count);
        return newQuestMap;
    }

    // Returns the quest associated with the ID passed
    public Quest GetQuestByID(string questID)
    {
        Quest quest = questMap[questID];
        if (quest == null)
        {
            Debug.LogError("Quest " + questID + " not found");
        }
        return quest;
    }

    // Changes the state of the quest, and raises the quest state changed event
    private void ChangeQuestState(string questID, QuestState newState)
    {
        Quest quest = GetQuestByID(questID);
        quest.state = newState;
        QuestEvents.QuestStateChanged(quest);
    }
    
    private bool CheckRequirementsMet(Quest quest)
    {
        // Check level requirements
        bool meetsRequirements = !(ExpLevels.CurrentLevel < quest.info.levelRequirement);

        // Check prerequisite quests
        foreach (QuestInfoSO info in quest.info.prerequisiteQuests)
        {
            if (GetQuestByID(info.ID).state != QuestState.FINISHED)
            {
                meetsRequirements = false;
            }
        }
        return meetsRequirements;
    }

    private void ClaimRewards(Quest quest)
    {
        // Increase the money by the money reward
        Inventory.Money += quest.info.moneyReward;
        // Adds the exp reward to the player's exp
        ExpLevels.CurrentExp += quest.info.expReward;
        foreach (Card_SO card in quest.info.cardRewards.Keys)
        {
            // Add the amount of cards based on each card so
            Inventory.AddCardToInventory(card, quest.info.cardRewards[card]);
        }

        foreach (Hack_SO hack in quest.info.hackRewards.Keys)
        {
            // Add the amount of hacks based on each hack so
            Inventory.AddHackToInventory(hack, quest.info.hackRewards[hack]);
        }
    }
    
    #endregion

    #region Saving
    
    private void QuestStepStateChange(string questID, int stepIndex, QuestStepState newState)
    {
        Quest quest = GetQuestByID(questID);
        quest.StoreQuestStepState(newState, stepIndex);
        ChangeQuestState(quest.info.ID, quest.state);
    }

    private static Quest LoadQuest(QuestInfoSO info)
    {
        Quest quest = null;
        try
        {
            if (SaveSystem.QuestDataExists(info.ID))
            {
                quest = new Quest(info, SaveSystem.LoadQuestData(info.ID));
            }
            else
            {
                quest = new Quest(info);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to load quest " + info.ID + ": " + e.Message);
            throw;
        }

        return quest;
    }

    public string SaveQuest(Quest quest)
    {
        try
        {
            QuestData questData = quest.GetQuestData();
            string serializedData = JsonUtility.ToJson(questData);
            Debug.Log(serializedData);
            return serializedData;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    #endregion
    
    
}
