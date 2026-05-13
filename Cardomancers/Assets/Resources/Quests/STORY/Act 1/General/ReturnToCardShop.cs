using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ReturnToCardShop : QuestStep
{
    private BoxCollider boxTrigger;

    private void Awake()
    {
        boxTrigger = GetComponentInChildren<BoxCollider>();
    }

    protected override void SetQuestStepState(string state)
    {
        throw new System.NotImplementedException();
    }

    public override string GetQuestStepState()
    {
        return "Return to the Card Shop";
    }

    public void FinishMe()
    {
        FinishQuestStep();
    }
}
