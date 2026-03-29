using UnityEngine;

public class QuestData
{
    // The current state of the quest on saving
    public QuestState state;
    // The ID of the quest being saved
    public string ID;

    // The index of the current quest step on saving
    public int questStepIndex;
    // The state of every quest step
    public QuestStepState[] questStepStates;

    public QuestData(QuestState state, int questStepIndex, string ID, QuestStepState[] questStepStates)
    {
        this.state = state;
        this.questStepIndex = questStepIndex;
        this.questStepStates = questStepStates;
        this.ID = ID;
    }
}
