using UnityEngine;
using System;
using DialogueScripts;

public static class DialogueEvents
{
    public static event Action<DialogueSO> OnStartDialogue;

    public static void StartDialogue(DialogueSO dialogueSO)
    {
        if(OnStartDialogue != null)
            OnStartDialogue(dialogueSO);
    }
    
    public static event Action<DialogueSO> OnEndDialogue;

    public static void EndDialogue(DialogueSO dialogueSO)
    {
        if (OnEndDialogue != null)
            OnEndDialogue(dialogueSO);
    }
}
