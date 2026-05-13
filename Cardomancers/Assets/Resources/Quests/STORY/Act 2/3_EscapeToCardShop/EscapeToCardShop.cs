using DialogueScripts;
using UnityEngine;

public class EscapeToCardShop : QuestStep
{
    public DialogueSO dialogue;
    
    public void PlayerEntersCardShop()
    {
        //Play dialogue
        DialogueManager.instance.StartDialogue(dialogue);
        FinishQuestStep();
    }
    
    protected override void SetQuestStepState(string state)
    {
    }

    public override string GetQuestStepState()
    {
        return "Seek shelter in the card shop.";
    }
}
