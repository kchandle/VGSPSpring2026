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
            startBattle.StartBattleNow();
        }
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
