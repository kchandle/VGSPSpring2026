using UnityEngine;

public class DecisionPopupTest : MonoBehaviour
{
    popUp PopUp;
    popUpActive PopUpActive;
    Objective objective;

    public void Awake()
    {
        PopUp = GameObject.Find("PopUpSystem").GetComponent<popUp>();
        PopUpActive = GetComponent<popUpActive>();
        objective = GetComponent<Objective>();
    }

    public void OnActivate()
    {
        PopUpActive.Activate();
    }
    
    void Update()
    {
        if(!(PopUp.choice == 0))
        {
            if (PopUp.choice == 1)
            {
                Debug.Log("Truth nuke");
                objective.CompleteObjective();
            }
            else if (PopUp.choice == 2)
            {
                Debug.Log("False");
            }
        }
    }
}
