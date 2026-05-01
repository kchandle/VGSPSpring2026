using DialogueScripts;
using UnityEngine;

public class FindSnailCat : QuestStep
{
    [SerializeField] private DialogueSO dialogue;

    private void Awake()
    {
        GameObject.FindGameObjectWithTag("SnailCat").SetActive(true);
    }

    private void OnEnable()
    {
        DialogueEvents.OnStartDialogue += TalkToSnailCatPerson;
    }

    private void OnDisable()
    {
        DialogueEvents.OnStartDialogue -= TalkToSnailCatPerson;
    }

    private void TalkToSnailCatPerson(DialogueSO dialogue)
    {
        if (dialogue == this.dialogue)
        {
            this.FinishQuestStep();
        }
    }

    protected override void SetQuestStepState(string state)
    {

    }

    public override string GetQuestStepState()
    {
        return "Look in alleyways or under bridges for the missing snail cat";
    }
}
