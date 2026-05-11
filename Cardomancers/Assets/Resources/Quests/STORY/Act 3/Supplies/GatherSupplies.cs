using UnityEngine;

// There will be an interactable object in conius's apartment that will call the finish quest step method
public class GatherSupplies : QuestStep
{
    [SerializeField] private GameObject newApartment;
    
    private void Start()
    {
        GameObject apartment =  GameObject.FindGameObjectWithTag("Apartment");
        Transform apartmentLocation = apartment.transform;
        Destroy(apartment.gameObject);
        Instantiate(newApartment, apartmentLocation.position, apartmentLocation.rotation);
    }

    public void FinishMe()
    {
        FinishQuestStep();
    }
    
    protected override void SetQuestStepState(string state)
    {
    }

    public override string GetQuestStepState()
    {
        return "Return to Traffic Conius's apartment to find his hidden stash of hacked cards.";
    }
}
