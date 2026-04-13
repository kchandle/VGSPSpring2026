using UnityEngine;
using DialogueScripts;

public class FightYoungster : QuestStep
{
    [SerializeField] private DialogueSO dialogue;
    
    private void OnEnable()
    {
        BattleManager.instance.OnEnd.AddListener(this.FinishQuestStep);
    }

    private void OnDisable()
    {
        BattleManager.instance.OnEnd.RemoveListener(this.FinishQuestStep);
    }

    private void Start()
    {
        DialogueManager.instance.StartDialogue(dialogue);
    }
    
    protected override void SetQuestStepState(string str)
    {
        
    }

    public override string GetQuestStepState()
    {
        return "Defeat this youngster!";
    }
}
