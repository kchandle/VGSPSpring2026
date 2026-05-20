using UnityEngine;
using DialogueScripts;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(StartBattle))]
public class StartBattleOnTrigger : MonoBehaviour
{
    private BoxCollider boxCollider;
    private StartBattle startBattle;
    [SerializeField] private DialogueSO dialogue;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        startBattle = GetComponent<StartBattle>();
        boxCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            DialogueManager.instance.StartDialogue(dialogue);
            DialogueEvents.OnEndDialogue += StartBattleAfterDialogue;
        }
    }

    private void StartBattleAfterDialogue(DialogueSO dialogue)
    {
        if (dialogue == this.dialogue)
        {
            startBattle.StartBattleNow();
            DialogueEvents.OnEndDialogue -=  StartBattleAfterDialogue;
        }
    }
}
