using UnityEngine;

public class ReturnToApartment : QuestStep
{
    private BoxCollider boxTrigger;
    
    private void Awake()
    {
        boxTrigger = GetComponentInChildren<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FinishQuestStep();
        }
    }
    
    protected override void SetQuestStepState(string state)
    {
        
    }

    public override string GetQuestStepState()
    {
        return "Get back to Conius' apartment";
    }
}
