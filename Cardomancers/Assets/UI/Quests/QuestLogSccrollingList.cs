using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class QuestLogScrollingList : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private GameObject contentParent;

    [Header("Quest Log Button")] 
    [SerializeField] private GameObject questLogButtonPrefab;
    
    private Dictionary<string, QuestLogButton> idToButtonMap = new Dictionary<string, QuestLogButton>();

    /* Test code, no longer needed
    private void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            QuestInfoSO questInfoTest = ScriptableObject.CreateInstance<QuestInfoSO>();
            questInfoTest.ID = "test " + i;
            GameObject.FindWithTag("Player").GetComponent<QuestManager>().GetQuestByID(questInfoTest.ID);
            questInfoTest.displayName = "Test " + i;
            questInfoTest.questSteps = new GameObject[0];
            Quest quest = new Quest(questInfoTest);

            QuestLogButton questLogButton = CreateButtonIfNotExists(quest, () =>
            {
                Debug.Log("SELECTED: " + quest.info.displayName);
            });

            if (i == 0)
            {
                questLogButton.button.Select();
            }
            
        }
    }*/

    /// <summary>
    /// Creates a new quest log button if one does not already exist, and returns the quest log button associated with quest passed
    /// </summary>
    public QuestLogButton CreateButtonIfNotExists(Quest quest, UnityAction selectAction)
    {
        QuestLogButton questLogButton = null;
        if (!idToButtonMap.ContainsKey(quest.info.ID))
        {
            questLogButton = InstantiateQuestLogButton(quest, selectAction);
        }
        else
        {
            questLogButton = idToButtonMap[quest.info.ID];
        }
        return questLogButton;
    }
    
    /// <summary>
    /// Instantiates a new quest log button
    /// </summary>
    /// <param name="quest">The quest object for the button</param>
    /// <param name="selectAction"></param>
    /// <returns>The quest log button that was created</returns>
    private QuestLogButton InstantiateQuestLogButton(Quest quest, UnityAction selectAction)
    {
        QuestLogButton questLogButton = Instantiate(
            questLogButtonPrefab,
            contentParent.transform).GetComponent<QuestLogButton>();
        questLogButton.gameObject.name = quest.info.ID + "_button";
        questLogButton.Initialize(quest.info.displayName, selectAction);
        idToButtonMap[quest.info.ID] = questLogButton;
        return questLogButton;
    }
}
