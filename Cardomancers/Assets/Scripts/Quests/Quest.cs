using UnityEngine;

public class Quest
{
    public QuestInfoSO info { get; private set; }
    //TODO internal set
    public QuestState state  { get; set; }
    private int currentStepIndex;
    private QuestStepState[] questStepStates;

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

    public void OverrideQuestData(QuestInfoSO info, QuestState state, int currentStepIndex, QuestStepState[] questStepStates)
    {
        this.info = info;
        this.state = state;
        this.currentStepIndex = currentStepIndex;
        this.questStepStates = questStepStates;

        if (this.questStepStates.Length != info.questSteps.Length)
        {
            Debug.LogWarning("Quest Step Prefabs and Quest Step States are of different length. . This indicates something hanged with the Quest info and the saved data");
        }
    }

    public void MoveToNextQuestStep()
    {
        currentStepIndex++;
    }

    public bool CurrentQuestStepExists()
    {
        if (currentStepIndex < info.questSteps.Length)
        {
            return true;
        }
        return false;
    }

    public void InstantiateCurrentStep(Transform parentTransform)
    {
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();
        if (questStepPrefab != null)
        { 
            GameObject questStep = Object.Instantiate<GameObject>(questStepPrefab, parentTransform);
            questStep.GetComponent<QuestStep>().InitializeQuestStep(info.ID, currentStepIndex, questStepStates[currentStepIndex].state);
        }
    }

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

    public QuestData GetQuestData()
    {
        return new QuestData(state, currentStepIndex, this.info.ID, questStepStates);
    }
}
