using System;
using UnityEngine;
using DialogueScripts;
using Unity.VisualScripting;

[RequireComponent(typeof(Collider))]
public class PosterEnemy : MonoBehaviour
{
    [SerializeField] private Battle_SO battleSO;
    [Header("Dialogue")]
    [Tooltip("The dialogue that plays when the player confronts this enemy")]
    [SerializeField] private DialogueSO startDialogue;
    [Tooltip("The dialogue that plays when the PLAYER wins")]
    [SerializeField] private DialogueSO victoryDialogue;
    [Tooltip("The dialogue that plays when the ENEMY wins")]
    [SerializeField] private DialogueSO defeatDialogue;
    [NonSerialized] public bool playerWon;
    
    Collider triggerCollider;

    private void Awake()
    {
        triggerCollider = GetComponent<Collider>();
        triggerCollider.isTrigger = true;
    }

    public void InitiateInteraction()
    {
        DialogueManager.instance.StartDialogue(startDialogue);
        BattleManager.instance.OnWin.AddListener(PlayVictoryDialogue);
        BattleManager.instance.OnWin.AddListener(() => playerWon = true);
        BattleManager.instance.OnLose.AddListener(PlayDefeatDialogue);
        DialogueEvents.OnEndDialogue += CheckIfDialogueFinished;
    }

    private void CheckIfDialogueFinished(DialogueSO dialogueSO)
    {
        if (dialogueSO == victoryDialogue || dialogueSO == defeatDialogue)
        {
            BattleManager.instance.OnWin.RemoveListener(PlayVictoryDialogue);
            BattleManager.instance.OnLose.RemoveListener(PlayDefeatDialogue);
            DialogueEvents.OnEndDialogue -= CheckIfDialogueFinished;
        }
    }

    private void PlayVictoryDialogue()
    {
        DialogueManager.instance.StartDialogue(victoryDialogue);
    }

    private void PlayDefeatDialogue()
    {
        DialogueManager.instance.StartDialogue(defeatDialogue);
    }
}
