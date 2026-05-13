using DialogueScripts;
using UnityEngine;

public class HiringQuestStep : QuestStep
{
    private BoxCollider boxTrigger;
    [SerializeField] private DialogueSO dialogue;
    
    protected override void SetQuestStepState(string state)
    {
        
    }
    
    public override string GetQuestStepState()
    {
        return "Go to Mallory's Card Emporium";
    }

    private void Awake()
    {
        boxTrigger = GetComponentInChildren<BoxCollider>();
    }

    public void FinishMe()
    {
        Debug.Log("Player entered"); 
        //DialogueManager.instance.StartDialogue(dialogue);
        FinishQuestStep();
    }
}
