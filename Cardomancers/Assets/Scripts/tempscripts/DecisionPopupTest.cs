using UnityEngine;

public class DecisionPopupTest : MonoBehaviour
{
    popUp PopUp;
    popUpActive PopUpActive;
    public bool returned = false;

    public void Awake()
    {
      PopUp = GameObject.Find("PopUpSystem").GetComponent<popUp>();
      PopUpActive = GetComponent<popUpActive>();
    }

    public void OnActivate()
    {
        PopUpActive.Activate();
    }
    
    void Update()
    {
        if(!returned)
        {
            if (PopUp.choice)
            {
                Debug.Log("True");
                returned = true;
            }
        }
    }
}
