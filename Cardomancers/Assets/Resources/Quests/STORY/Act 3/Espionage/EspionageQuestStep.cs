using UnityEngine;

// The plan for this one is for there to be an interactable object in an office area of the Eve L Mart which, when interacted will tell the player that conius is being held in the back room and finish this quest step
public class EspionageQuestStep : QuestStep
{
    protected override void SetQuestStepState(string state)
    {
    }

    public override string GetQuestStepState()
    {
        return "Investigate the Eve L Mart to find information on where they are keeping Conius.";
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
