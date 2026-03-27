using UnityEngine;

public class QuestData
{
    public QuestState state;
    public string ID;

    public int questStepIndex;
    public QuestStepState[] questStepStates;

    public QuestData(QuestState state, int questStepIndex, string ID, QuestStepState[] questStepStates)
    {
        this.state = state;
        this.questStepIndex = questStepIndex;
        this.questStepStates = questStepStates;
        this.ID = ID;
    }
}
