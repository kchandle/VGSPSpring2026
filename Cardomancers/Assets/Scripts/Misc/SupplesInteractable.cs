using UnityEngine;
using DialogueScripts;

public class SupplesInteractable : MonoBehaviour
{
    [SerializeField] private DialogueSO dialogue;

    public void Interact()
    {
        DialogueManager.instance.StartDialogue(dialogue);
        FindObjectOfType<GatherSupplies>().GetComponent<GatherSupplies>().FinishMe();
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
