using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<string, Quest> questMap = new Dictionary<string, Quest>();
    
    #region Unity Methods

    private void Awake()
    {
        questMap = CreateQuestMap();
    }

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

    private void Start()
    {
        foreach (Quest quest in questMap.Values)
        {
            QuestEvents.QuestStateChanged(GetQuestByID(quest.info.ID));
        }
    }

    private void Update()
    {
        foreach (Quest quest in questMap.Values)
        {
            if (GetQuestByID(quest.info.ID).state.Equals(QuestState.REQUIREMENTS_NOT_MET) && CheckRequirementsMet(GetQuestByID(quest.info.ID)))
            {
                ChangeQuestState(quest.info.ID, QuestState.CAN_START);
            }
        }
    }
    #endregion
    
    #region Event Subscribers

    private void StartQuest(string ID)
    {
        Quest quest = GetQuestByID(ID);
        quest.InstantiateCurrentStep(this.transform);
        ChangeQuestState(quest.info.ID, QuestState.IN_PROGRESS);
    }
    
    private void AdvanceQuest(string ID)
    {
        Quest quest = GetQuestByID(ID);

        quest.MoveToNextQuestStep();

        if (quest.CurrentQuestStepExists())
        {
            quest.InstantiateCurrentStep(this.transform);
        }
        else
        {
            ChangeQuestState(quest.info.ID, QuestState.CAN_FINISH);
        }
    }

    private void FinishQuest(string ID)
    {
        Quest quest = GetQuestByID(ID);
        ClaimRewards(quest);
        ChangeQuestState(quest.info.ID, QuestState.FINISHED);
    }
    #endregion
    
    #region Private Methods

    private Dictionary<string, Quest> CreateQuestMap()
    {
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");
        Dictionary<string, Quest> newQuestMap = new Dictionary<string, Quest>();

        foreach (QuestInfoSO quest in allQuests)
        {
            Debug.Log("2");
            if (questMap.ContainsKey(quest.ID))
            {
                Debug.LogWarning("Quest " + quest.ID + " duplicate detected");
            }
            newQuestMap.Add(quest.ID, new Quest(quest));
        }
        Debug.Log(newQuestMap.Count);
        return newQuestMap;
    }

    private Quest GetQuestByID(string questID)
    {
        Quest quest = questMap[questID];
        if (quest == null)
        {
            Debug.LogError("Quest " + questID + " not found");
        }
        return quest;
    }

    private void ChangeQuestState(string questID, QuestState newState)
    {
        Quest quest = GetQuestByID(questID);
        quest.state = newState;
        QuestEvents.QuestStateChanged(quest);
    }

    private bool CheckRequirementsMet(Quest quest)
    {
        bool meetsRequirements = true;

        if (ExpLevels.CurrentLevel < quest.info.levelRequirement)
        {
            meetsRequirements = false;
        }

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
}
