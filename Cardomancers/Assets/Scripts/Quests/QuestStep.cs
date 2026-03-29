using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;
    private string ID;
    private int stepIndex;

    public void InitializeQuestStep(string questID, int stepIndex, string questStepState)
    {
        this.ID = questID;
        this.stepIndex = stepIndex;
        if (!string.IsNullOrEmpty(questStepState))
        {
            this.SetQuestStepState(questStepState);
        }
    }

    protected void FinishQuestStep()
    {
        this.isFinished = true;
        QuestEvents.AdvanceQuest(ID);
        Destroy(gameObject);
    }

    protected void ChangeState(string newState)
    {
        QuestEvents.QuestStepStateChanged(ID, stepIndex, new QuestStepState(newState));
    }

    protected abstract void SetQuestStepState(string state);
    
    public abstract string GetQuestStepState();

}
