using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class GetToConius : QuestStep
{
    BoxCollider boxCollider;
    
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        boxCollider.center = GameObject.FindWithTag("StorageRoom").transform.position;
        boxCollider.size = new Vector3(30, 30, 30);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FinishQuestStep();
        }
    }
    
    protected override void SetQuestStepState(string state)
    {
    }

    public override string GetQuestStepState()
    {
        return "Go to the back room where Conius is held and free him";
    }
}
