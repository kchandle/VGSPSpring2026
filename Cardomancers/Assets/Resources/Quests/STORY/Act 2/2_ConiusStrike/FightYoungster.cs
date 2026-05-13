using DialogueScripts;
using UnityEngine;

public class FightYoungsterAlley : QuestStep
{
    [SerializeField] private DialogueSO startDialogue;
    [SerializeField] private DialogueSO winDialogue;
    [SerializeField] private DialogueSO loseDialogue;

    private void Awake()
    {
        DialogueManager.instance.StartDialogue(startDialogue);
        BattleManager.instance.OnWin.AddListener(PlayWinDialogue);
        BattleManager.instance.OnLose.AddListener(PlayLoseDialogue);
    }

    private void PlayWinDialogue()
    {
        DialogueManager.instance.StartDialogue(winDialogue);
        BattleManager.instance.OnWin.RemoveListener(PlayWinDialogue);
        FinishQuestStep();
    }

    private void PlayLoseDialogue()
    {
        DialogueManager.instance.StartDialogue(loseDialogue);
        BattleManager.instance.OnLose.RemoveListener(PlayLoseDialogue);
        FinishQuestStep();
    }
    
    protected override void SetQuestStepState(string state)
    {
    }

    public override string GetQuestStepState()
    {
        return "Defeat this youngster";
    }
}
