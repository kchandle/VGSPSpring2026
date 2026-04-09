using UnityEngine;
using UnityEngine.Rendering.UI;

public class ObtainHack : QuestStep
{
    // Update is called once per frame
    void Update()
    {
        if (HasHack())
        {
            this.FinishQuestStep();
        }
    }

    private bool HasHack()
    {
        return Inventory.HackInventory.Count > 0;
    }

    protected override void SetQuestStepState(string state)
    {
        
    }

    public override string GetQuestStepState()
    {
        return "Obtain a hack";
    }
}
