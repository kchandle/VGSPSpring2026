using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestManager : MonoBehaviour
{
    private Dictionary<string, Quest> questMap = new Dictionary<string, Quest>();
    
    public Dictionary<string, Quest> QuestMap { get { return questMap; } }
    
    #region Unity Methods

    private void Awake()
    {
        questMap = CreateQuestMap();
    }

    // Subscribes methods to their respective events
    private void OnEnable()
    {
        QuestEvents.OnStartQuest += StartQuest;
        QuestEvents.OnAdvanceQuest += AdvanceQuest;
        QuestEvents.OnFinishQuest += FinishQuest;
    }

    private void OnDisable()
    {
        QuestEvents.OnStartQuest -= StartQuest;
        QuestEvents.OnAdvanceQuest -= AdvanceQuest;
        QuestEvents.OnFinishQuest -= FinishQuest;
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
    
    #region Private Methods

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
            newQuestMap.Add(quest.ID, new Quest(quest));
        }
        Debug.Log(newQuestMap.Count);
        return newQuestMap;
    }

    // Returns the quest associated with the ID passed
    private Quest GetQuestByID(string questID)
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
        bool meetsRequirements = true;

        // Check level
        if (ExpLevels.CurrentLevel < quest.info.levelRequirement)
        {
            meetsRequirements = false;
        }

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
        Inventory.Money += quest.info.moneyReward;
        ExpLevels.CurrentExp += quest.info.expReward;
        foreach (Card_SO card in quest.info.cardRewards)
        {
            Inventory.AddCardToInventory(card);
        }

        foreach (Hack_SO hack in quest.info.hackRewards)
        {
            Inventory.AddHackToInventory(hack);
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

    public void LoadQuest(QuestData questData)
    {
        Quest quest = GetQuestByID(questData.ID);
        
        quest.OverrideQuestData(quest.info, questData.state, questData.questStepIndex, questData.questStepStates);
    }
    

    #endregion
    
    
}
