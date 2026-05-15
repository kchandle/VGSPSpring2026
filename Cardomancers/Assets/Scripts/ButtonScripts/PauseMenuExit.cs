using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class PauseMenuExit : MonoBehaviour
{
    popUp PopUp;
    popUpActive PopUpActive;
    Objective objective;
    GraphicRaycaster graphicRaycaster;
    

    public void Awake()
    {
      PopUp = GameObject.Find("PopUpSystem").GetComponent<popUp>();
      PopUpActive = GetComponent<popUpActive>();
      objective = GetComponent<Objective>();
      graphicRaycaster = transform.parent.parent.gameObject.GetComponent<GraphicRaycaster>();
    }

    public void OnActivate()
    {
        SceneManager.LoadScene(0);
        //PopUpActive.Activate();
    }

    void Update()
    {
        if(!(PopUp.choice == 0))
        {
            if (PopUp.choice == 1)
            {
                Debug.Log("Exit");
                SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            }
            else if (PopUp.choice == 2)
            {
                Debug.Log("or stay for eternity");
                graphicRaycaster.enabled = true;
            }
        }
    }
}
