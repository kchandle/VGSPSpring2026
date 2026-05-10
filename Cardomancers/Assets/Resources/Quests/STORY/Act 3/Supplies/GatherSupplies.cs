using UnityEngine;

// There will be an interactable object in conius's apartment that will call the finish quest step method
public class GatherSupplies : QuestStep
{
    private void Start()
    {
        //Change the teleport to conius's apartment to teleport to one with an enemy at the entrance and the interactable object
    }
    
    protected override void SetQuestStepState(string state)
    {
    }

    public override string GetQuestStepState()
    {
        return "Return to Traffic Conius's apartment to find his hidden stash of hacked cards.";
    }
}
