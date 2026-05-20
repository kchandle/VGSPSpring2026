using UnityEngine;
using DialogueScripts;

public class QuestPoint : MonoBehaviour
{
    private string questID;
    private QuestState currentQuestState;
    [SerializeField] private QuestInfoSO questInfo;

    [Header("Config")] 
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool endPoint = true;
    [SerializeField] private GameObject[] setActiveOnFinish;
    [SerializeField] private bool shopkeeper = false;
    [SerializeField] private bool startsBattleOnEnd = false;
    [SerializeField] private bool playDialogueOnFinishOnly;
    [SerializeField] private DialogueSO altDialogue;
    [SerializeField] private DialogueSO defaultDialogue;

    private void Awake()
    {
        questID = questInfo.ID;
    }

    private void OnEnable()
    {
        QuestEvents.OnQuestStateChanged += QuestStateChange;
        currentQuestState = FindFirstObjectByType<QuestManager>().GetQuestByID(questID).state;
    }

    private void OnDisable()
    {
        QuestEvents.OnQuestStateChanged -= QuestStateChange;
    }
    
    public void StartOrFinishQuest()
    {
        Debug.Log(currentQuestState);
        // If you can start the quest and this is where you start it, start the quest
        if (currentQuestState == QuestState.CAN_START && startPoint)
        {
            QuestEvents.StartQuest(questID);
            Debug.Log("Starting Quest: " +  questID);
            if (defaultDialogue && !playDialogueOnFinishOnly)
            {
                Debug.Log(defaultDialogue.name);
                DialogueManager.instance.StartDialogue(defaultDialogue);
            }
        }
        // If you can finish the quest and theis is where you end it, end the quest
        else if (currentQuestState == QuestState.CAN_FINISH && endPoint)
        {
            Debug.Log("Ending Quest: " +  questID);
            QuestEvents.FinishQuest(questID);
            foreach (GameObject setActive in setActiveOnFinish)
            {
                setActive.SetActive(true);
            }
            gameObject.SetActive(false);
            DialogueManager.instance.StartDialogue(defaultDialogue);
            if (startsBattleOnEnd)
            {
                if (GetComponent<DialogueOnBattleEnd>() != null)
                {
                    GetComponent<DialogueOnBattleEnd>().enabled = true;
                }
                GetComponent<StartBattle>().StartBattleNow();
            }
        }
        else if (shopkeeper)
        {
            Debug.Log("Shopkeeper");
            GetComponent<ShopkeeperInteract>().OnInteract();
        }
        else if (altDialogue)
        {
            Debug.Log("AltDialogue");
            DialogueManager.instance.StartDialogue(altDialogue);
        }

        FindFirstObjectByType<PlayerInteract>().GetComponent<PlayerInteract>().interacting = false;
    }

    // Keeps the state synced with the quest state
    private void QuestStateChange(Quest quest)
    {
        if (quest.info.ID == questInfo.ID)
        {
            currentQuestState = quest.state;
        }
    }
}
