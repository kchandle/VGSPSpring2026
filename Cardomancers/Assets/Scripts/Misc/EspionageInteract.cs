using DialogueScripts;
using UnityEngine;

public class EspionageInteract : MonoBehaviour
{
    [SerializeField] private DialogueSO dialogue;
    
    public void Interact()
    {
        DialogueManager.instance.StartDialogue(dialogue);
        FindFirstObjectByType<EspionageQuestStep>().GetComponent<EspionageQuestStep>().FinishMe();
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
