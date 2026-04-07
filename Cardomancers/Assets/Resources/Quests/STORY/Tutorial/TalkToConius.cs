using DialogueScripts;
using UnityEngine;
using static DialogueEvents;

public class TalkToConius : QuestStep
{
    [SerializeField] private DialogueSO dialogueSO;
    
    private void OnEnable()
    {
        OnStartDialogue += TalkedToConius;
    }

    private void OnDisable()
    {
        OnStartDialogue -= TalkedToConius;
    }

    private void TalkedToConius(DialogueSO dialogueSO)
    {
        if (dialogueSO != this.dialogueSO) return;
        this.FinishQuestStep();
    }

    protected override void SetQuestStepState(string str)
    {
        
    }
    
    public override string GetQuestStepState()
    {
        return "Go talk to Traffic Conius";
    }
}
