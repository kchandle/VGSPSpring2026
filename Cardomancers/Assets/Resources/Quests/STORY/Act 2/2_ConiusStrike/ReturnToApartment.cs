using UnityEngine;

public class ReturnToApartment : QuestStep
{
    private BoxCollider boxTrigger;
    [SerializeField] private GameObject ApartmentDuringArrest;
    
    private void Awake()
    {
        boxTrigger = GetComponentInChildren<BoxCollider>();
        GameObject OldApartment = GameObject.FindWithTag("Apartment");
        Transform OldTransform = OldApartment.transform;
        Destroy(OldApartment);
        Instantiate(ApartmentDuringArrest, OldTransform.position, OldTransform.rotation);
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
        return "Get back to Conius' apartment";
    }
}
