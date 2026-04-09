using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestLogUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private QuestLogScrollingList scrollingList;

    [SerializeField] private TextMeshProUGUI questDisplayNameText;
    [SerializeField] private TextMeshProUGUI questDescriptionText;
    [SerializeField] private TextMeshProUGUI moneyRewardText;
    [SerializeField] private TextMeshProUGUI experienceRewardText;
    [SerializeField] private TextMeshProUGUI cardRewardText;
    
    [Header("Outside Reference")]
    [SerializeField] private QuestManager questManager;
    
    private Button firstSelectedButton;

    private void OnEnable()
    {
        QuestEvents.OnQuestStateChanged += QuestStateChange;
        // Update buttons based on state changes when the game object is inactive
        foreach (QuestLogButton questLogButton in scrollingList.IDToButtonMap.Values)
        {
            switch (questManager.GetQuestByID(questLogButton.QuestID).state)
            {
                case QuestState.CAN_START:
                    questLogButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.yellowNice;
                    break;
                case QuestState.IN_PROGRESS:
                    questLogButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.blue;
                    break;
                case QuestState.CAN_FINISH:
                    questLogButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.chartreuse;
                    break;
                default:
                    // if the state isn't one of the above ones, the button should be destroyed
                    Destroy(questLogButton.gameObject);
                    break;
            }
        }
    }

    private void OnDisable()
    {
        QuestEvents.OnQuestStateChanged -= QuestStateChange;
    }

    // Whenever a quest's state changes, if it has the right state, create a button and change the color based on the state
    private void QuestStateChange(Quest quest)
    {
        QuestLogButton questLogButton;
        
        if (quest.state == QuestState.IN_PROGRESS || quest.state == QuestState.CAN_START ||
            quest.state == QuestState.CAN_FINISH) questLogButton = scrollingList.CreateButtonIfNotExists(quest, () =>
        {
            SetQuestLogInfo(quest);
        });
        else
        {
            questLogButton = scrollingList.GetQuestLogButton(quest);
            if(questLogButton == null) return;
        }

        switch (quest.state)
        {
            case QuestState.CAN_START:
                questLogButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.yellowNice;
                break;
            case QuestState.IN_PROGRESS:
                questLogButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.blue;
                break;
            case QuestState.CAN_FINISH:
                questLogButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.chartreuse;
                break;
            default:
                Destroy(questLogButton.gameObject);
                break;
        }

        if (firstSelectedButton == null)
        {
            firstSelectedButton = questLogButton.button;
        }
    }

    private void SetQuestLogInfo(Quest quest)
    {
        // Name
        questDisplayNameText.text = quest.info.displayName;
        
        //Status
        questDescriptionText.text = quest.info.questSteps[quest.currentStepIndex].GetComponent<QuestStep>().GetQuestStepState();
        
        // Rewards
        moneyRewardText.text = "Wizard Money: " + (quest.info.moneyReward > 0 ? quest.info.moneyReward : "None");
        experienceRewardText.text = "Experience: " + (quest.info.expReward > 0 ? quest.info.expReward : "None");

        string cardRewards = "";
        if (quest.info.cardRewards.Keys.Count > 0)
        {
            cardRewards += "Card Rewards: \n";
            
            foreach (Card_SO cardSO in quest.info.cardRewards.Keys)
            {
                cardRewards += cardSO.displayName + $" ({quest.info.cardRewards[cardSO]})\n";
            }
        }

        if (quest.info.hackRewards.Keys.Count > 0)
        {
            cardRewards += "Hack Rewards: \n";

            foreach (Hack_SO hackSO in quest.info.hackRewards.Keys)
            {
                cardRewards += hackSO.displayName + $" ({quest.info.hackRewards[hackSO]})\n";
            }
        }

        cardRewardText.text = cardRewards;
    }
}
