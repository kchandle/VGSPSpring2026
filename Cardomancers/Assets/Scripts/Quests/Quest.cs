using System.Linq;
using UnityEngine;

public class Quest
{
    // Scriptable object that contains the data for the quest
    public QuestInfoSO info { get; private set; }
    // Enum containing the current state of the quest
    public QuestState state  { get; internal set; }
    // The index of the current step
    public int currentStepIndex { get; private set; }
    // An array, containing the state of each quest step
    private readonly QuestStepState[] questStepStates;

    public Quest(QuestInfoSO info)
    {
        this.info = info;
        this.state = QuestState.REQUIREMENTS_NOT_MET;
        this.currentStepIndex = 0;
        this.questStepStates = new QuestStepState[info.questSteps.Length];
        for (int i = 0; i < info.questSteps.Length; i++)
        {
            questStepStates[i] = new QuestStepState();
        }
    }

    public Quest(QuestInfoSO info, QuestData data)
    {
        this.info = info;
        this.state = data.state;
        this.currentStepIndex = data.questStepIndex;
        this.questStepStates = data.questStepStates;
        //Debug.Log(data.state);
        //Debug.Log(data.questStepIndex);
        //Debug.Log(data.questStepStates);
        if (this.questStepStates == null)
        {
            this.questStepStates = new QuestStepState[info.questSteps.Length];
            for (int i = 0; i < info.questSteps.Length; i++)
            {
                this.questStepStates[i] = new QuestStepState();
            }
        }

        if (this.questStepStates.Length != info.questSteps.Length)
        {
            Debug.LogWarning("Quest Step Prefabs and Quest Step States are of different length. This indicates something happened with the Quest info and the saved data");
        }
    }

    // Increments the current step index by 1
    public void MoveToNextQuestStep()
    {
        currentStepIndex++;
    }

    // Checks if attempting to access the current quest step will cause an error.
    public bool CurrentQuestStepExists()
    {
        if (currentStepIndex < info.questSteps.Length)
        {
            return true;
        }
        return false;
    }

    // Instantiates the current step prefab as a child of the input transform, then initializes the step.
    public void InstantiateCurrentStep(Transform parentTransform)
    {
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();
        if (questStepPrefab != null)
        { 
            GameObject questStep = Object.Instantiate<GameObject>(questStepPrefab, parentTransform);
            questStep.GetComponent<QuestStep>().InitializeQuestStep(info.ID, currentStepIndex, questStepStates[currentStepIndex].state);
        }
    }

    // Returns the prefab associated with the current quest step.
    private GameObject GetCurrentQuestStepPrefab()
    {
        GameObject currentQuestStepPrefab = null;
        if (CurrentQuestStepExists())
        {
            currentQuestStepPrefab = info.questSteps[currentStepIndex];
        }
        else
        {
            Debug.LogWarning($"Attempted to get current quest step prefab, but step index was out of bounds, indicating that there's no current quest step. \n QuestID: {this.info.ID} \n StepIndex: {this.currentStepIndex}");
        }
        return currentQuestStepPrefab;
    }

    // Stores the state data for the player's progress on the current step
    public void StoreQuestStepState(QuestStepState state, int index)
    {
        if (index < questStepStates.Length)
        {
            questStepStates[index].state = state.state;
        }
        else
        {
            Debug.LogWarning("Tried to access quest step data, but stepIndex was out of range: " + "Quest Id = " + info.ID +", StepIndex = " + index);
        }
    }

    // Returns a quest data object based on the current state of the quest
    public QuestData GetQuestData()
    {
        return new QuestData(state, currentStepIndex, this.info.ID, this.questStepStates);
    }
}