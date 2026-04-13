using System;
using UnityEngine;

[System.Serializable]
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
    
    /// <summary>
    /// Used for loading the quest step state data
    /// </summary>
    protected abstract void SetQuestStepState(string state);
    
    /// <summary>
    /// Used for displaying the current quest step state to the player
    /// </summary>
    /// <returns>A string which is shown to the player in the Quest UI</returns>
    public abstract string GetQuestStepState();

}
