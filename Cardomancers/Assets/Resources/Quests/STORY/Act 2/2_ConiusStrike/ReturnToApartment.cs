using UnityEngine;

public class ReturnToApartment : QuestStep
{
    protected override void SetQuestStepState(string state)
    {
        
    }

    public override string GetQuestStepState()
    {
        return "Get back to Conius' apartment";
    }
}
