using System;

public static class QuestEvents
{
    /// <summary>
    /// Raised whenever a quest is started.
    /// </summary>
    public static event Action<string> OnStartQuest;

    public static void StartQuest(string ID)
    {
        if (OnStartQuest != null)
        {
            OnStartQuest(ID);
        }
    }
    
    /// <summary>
    /// Raised whenever a quest goes from one step to the next.
    /// </summary>
    public static event Action<string> OnAdvanceQuest;

    public static void AdvanceQuest(string ID)
    {
        if (OnAdvanceQuest != null)
        {
            OnAdvanceQuest(ID);
        }
    }
    
    /// <summary>
    /// Raised whenever the player finsishes a quest.
    /// </summary>
    public static event Action<string> OnFinishQuest;

    public static void FinishQuest(string ID)
    {
        if (OnFinishQuest != null)
        {
            OnFinishQuest(ID);
        }
    }

    /// <summary>
    /// Raised whenever the state of a quest changes, passes the actual quest instead of its ID.
    /// </summary>
    public static event Action<Quest> OnQuestStateChanged;

    public static void QuestStateChanged(Quest quest)
    {
        if (OnQuestStateChanged != null)
        {
            OnQuestStateChanged(quest);
        }
    }
    
    /// <summary>
    /// Raised whenever the state of an individual step is changed.
    /// </summary>
    public static event Action<string, int, QuestStepState> OnQuestStepStateChanged;

    public static void QuestStepStateChanged(string id, int stepIndex, QuestStepState state)
    {
        if (OnQuestStepStateChanged != null)
        {
            OnQuestStepStateChanged(id, stepIndex, state);
        }
    }
}
