using System;
using UnityEngine;

public class TalkToPeople : QuestStep
{
    [Tooltip("The NPCs to be instantiated when the quest starts.")]
    [SerializeField] private GameObject[] people;
    [Tooltip("The amount of NPCs the player must talk to.")]
    [SerializeField] private int NPCsToTalkTo;
    private int NPCsTalkedTo;

    private void Start()
    {
        foreach (GameObject go in people)
        {
            //Temporary. This currently instantiates them at the quest manager position
            Instantiate(go, transform.position, transform.rotation);
        }
    }

    public void TalkToNPC()
    {
        NPCsTalkedTo++;
        this.ChangeState(NPCsTalkedTo.ToString());
        if (NPCsTalkedTo >= NPCsToTalkTo)
        {
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
