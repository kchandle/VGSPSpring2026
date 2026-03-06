using UnityEngine;

public class QuestPoint : MonoBehaviour
{
    private bool playerInRange;
    private string questID;
    private QuestState currentQuestState;
    private QuestInfoSO questInfo;

    [Header("Config")] 
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool endPoint = true;

    private void Awake()
    {
        questID = questInfo.ID;
    }

    private void OnEnable()
    {
        QuestEvents.OnQuestStateChanged += QuestStateChange;
    }

    private void OnDisable()
    {
        QuestEvents.OnQuestStateChanged -= QuestStateChange;
    }

    public void StartOrFinishQuest()
    {
        if (!playerInRange) return;

        if (currentQuestState == QuestState.CAN_START && startPoint)
        {
            QuestEvents.StartQuest(questID);
        }
        else if (currentQuestState == QuestState.CAN_FINISH && endPoint)
        {
            QuestEvents.FinishQuest(questID);
        }
    }

    private void QuestStateChange(Quest quest)
    {
        if (quest.info.ID == questInfo.ID)
        {
            currentQuestState = quest.state;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
