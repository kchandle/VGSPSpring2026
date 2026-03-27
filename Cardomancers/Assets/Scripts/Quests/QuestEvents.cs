using System;

public static class QuestEvents
{
    public static event Action<string> OnStartQuest;

    public static void StartQuest(string ID)
    {
        if (OnStartQuest != null)
        {
            OnStartQuest(ID);
        }
    }
    
    
    public static event Action<string> OnAdvanceQuest;

    public static void AdvanceQuest(string ID)
    {
        if (OnAdvanceQuest != null)
        {
            OnAdvanceQuest(ID);
        }
    }
    
    public static event Action<string> OnFinishQuest;

    public static void FinishQuest(string ID)
    {
        if (OnFinishQuest != null)
        {
            OnFinishQuest(ID);
        }
    }

    public static event Action<Quest> OnQuestStateChanged;

    public static void QuestStateChanged(Quest quest)
    {
        if (OnQuestStateChanged != null)
        {
            OnQuestStateChanged(quest);
        }
    }
    
    public static event Action<string, int, QuestStepState> OnQuestStepStateChanged;

    public static void QuestStepStateChanged(string id, int stepIndex, QuestStepState state)
    {
        if (OnQuestStepStateChanged != null)
        {
            OnQuestStepStateChanged(id, stepIndex, state);
        }
    }
}
