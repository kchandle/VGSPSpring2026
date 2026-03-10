using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;
    private string ID;

    public void InitializeQuestStep(string questID)
    {
        this.ID = questID;
    }

    protected void FinishQuestStep()
    {
        this.isFinished = true;
        QuestEvents.AdvanceQuest(ID);
        Destroy(gameObject);
    }
}
