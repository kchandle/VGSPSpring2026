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
    
    private Button firstSelectedButton;

    private void OnEnable()
    {
        QuestEvents.OnQuestStateChanged += QuestStateChange;
    }

    private void OnDisable()
    {
        QuestEvents.OnQuestStateChanged -= QuestStateChange;
    }

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
            return;
        }

        if (firstSelectedButton == null)
        {
            firstSelectedButton = questLogButton.button;
        }
    }

    private void SetQuestLogInfo(Quest quest)
    {
        questDisplayNameText.text = quest.info.displayName;
        
        //TODO - Status
        
        moneyRewardText.text = quest.info.moneyReward.ToString();
        experienceRewardText.text = quest.info.expReward.ToString();

        string cardRewards = "";
        if (quest.info.cardRewards != null)
        {
            cardRewards += "Card Rewards: \n";
            
            foreach (Card_SO cardSO in quest.info.cardRewards.Keys)
            {
                cardRewards += cardSO.displayName + $" ({quest.info.cardRewards[cardSO]})\n";
            }
        }

        if (quest.info.hackRewards != null)
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
