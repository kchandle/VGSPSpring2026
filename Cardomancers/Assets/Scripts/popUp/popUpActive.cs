    using UnityEngine;

public class popUpActive : MonoBehaviour
{
    popUp popup;
    public popUp_SO popSO;
    public void Awake()
    {
      popup = GameObject.Find("PopUpSystem").GetComponent<popUp>();
    }
    // Finds and sets the pop up

    public void Activate()
    {
        popup.SetVariables(popSO);
    }
    // Activates the pop up
}
