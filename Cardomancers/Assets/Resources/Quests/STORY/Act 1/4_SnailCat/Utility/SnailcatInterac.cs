using UnityEngine;
using DialogueScripts;

public class SnailcatInteract : MonoBehaviour
{
    [SerializeField] private DialogueSO dialogue;

    public void Interact()
    {
        DialogueManager.instance.StartDialogue(dialogue);
        FindFirstObjectByType<PlayerInteract>().GetComponent<PlayerInteract>().interacting = false;
    }
}
