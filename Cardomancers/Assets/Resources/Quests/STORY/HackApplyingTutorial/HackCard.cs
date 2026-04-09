using UnityEngine;

public class HackCard : QuestStep
{
    private void OnEnable()
    {
        InventoryEvents.OnCardHacked += this.FinishQuestStep;
    }

    private void OnDisable()
    {
        InventoryEvents.OnCardHacked -= this.FinishQuestStep;
    }
    
    protected override void SetQuestStepState(string state)
    {
        
    }

    public override string GetQuestStepState()
    {
        return "Open your inventory and hack one of your cards";
    }
}
