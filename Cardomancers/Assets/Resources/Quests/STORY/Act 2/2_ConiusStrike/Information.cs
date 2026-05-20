using System;
using UnityEngine;
using DialogueScripts;

public class TalkToPeople : QuestStep
{
    [SerializeField] private int NPCsToTalkTo;
    [SerializeField] private DialogueSO dialogue;

    private int NPCsTalkedTo;

    private void OnEnable()
    {
        BattleManager.instance.OnWin.AddListener(TalkToNPC);
    }

    private void OnDisable()
    {
        BattleManager.instance.OnWin.RemoveListener(TalkToNPC);
    }

    public void TalkToNPC()
    {
        NPCsTalkedTo++;
        DialogueManager.instance.StartDialogue(dialogue);
        this.ChangeState(NPCsTalkedTo.ToString());
        if (NPCsTalkedTo >= NPCsToTalkTo)
        {
            // Play cutscene for player being knocked out
            // Teleport player to an alleyway
            this.FinishQuestStep();
        }
    }
    
    protected override void SetQuestStepState(string state)
    {
        try
        {
            NPCsTalkedTo = Int32.Parse(state);
        }
        catch
        {
            NPCsTalkedTo = 0;
            Debug.LogError("State information for TalkToPeople quest step was damaged. \n State is currently: \n " + state);
        }
    }

    public override string GetQuestStepState()
    {
        return $"People informed about hacking: {NPCsTalkedTo} / {NPCsToTalkTo}";
    }
}
