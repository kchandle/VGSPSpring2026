using UnityEngine;

public class Quest
{
    public QuestInfoSO info { get; private set; }
    private QuestState state;
    private int currentStepIndex;

    public Quest(QuestInfoSO info)
    {
        this.info = info;
        this.state = QuestState.REQUIREMENTS_NOT_MET;
        this.currentStepIndex = 0;
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
            //QuestStep.InitializeQuestStep(info.ID);
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
}
