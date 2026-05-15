using DialogueScripts;
using UnityEngine;
using System.Collections;

public class DialogueOnBattleEnd : MonoBehaviour
{
    [SerializeField] private DialogueSO winDialogue;
    [SerializeField] private DialogueSO loseDialogue;

    private void PlayWinDialogue()
    {
        DialogueManager.instance.StartDialogue(winDialogue);
        // Remove this script from existence after the dialogue is played
        Destroy(this);
    }

    private void PlayLoseDialogue()
    {
        DialogueManager.instance.StartDialogue(loseDialogue);
        Destroy(this);
    }

    private void OnEnable()
    {
        BattleManager.instance.OnWin.AddListener(PlayWinDialogue);
        BattleManager.instance.OnLose.AddListener(PlayLoseDialogue);
    }

    private void OnDisable()
    {
        BattleManager.instance.OnWin.RemoveListener(PlayWinDialogue);
        BattleManager.instance.OnLose.RemoveListener(PlayLoseDialogue);
    }
}
