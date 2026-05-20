using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class popUp : MonoBehaviour
{
    public popUp_SO popSO;
    //public Image image;
    public TextMeshProUGUI popUpText;
    // "Canvas" is the TEXT component that gets turned active. Was previously the actual canvas but i made it better and im too lazy to rewrite
    public GameObject canvas;
    public GameObject yes;
    public GameObject no;
    public GameObject background; 
    // "Decision" if the pop up has a yes or no
    private bool decision = false;
    // choice depends on which button hit
    public int choice = 0;
    private float timer;
    private bool responseMade = false;

    public void SetVariables(popUp_SO popSO)
    {
        this.popSO = popSO;
        popUpText.text = popSO.popUpText;
        timer = popSO.timer;
        decision = popSO.decision;
        responseMade = false;
        SettingActive();
    } 
    // Script called by other scripts that incur popups, passing in their popup_SO

    public void OnYesClick()
    {
        choice = 1;
        responseMade = true;
    }

    public void OnNoClick()
    {
        choice = 2;
        responseMade = true;
    }

    public void ChoiceReset()
    {
        choice = 0;
        responseMade = false;
    }
    // Upon choice being used, reset for other scripts that may need it

    // Set by yes/no buttons, Decision can be used by script that calls and responseMade lets the coroutine pass through

    public void SettingActive()
    {
        canvas.SetActive(true);
        if (decision) 
        {
            yes.SetActive(true);
            no.SetActive(true);
            background.SetActive(true);
        }
        StartCoroutine(PopUpTimer(timer));
    }
    // Sets canvas active and starts coroutine, if "decision" active turn on the yes/no

    IEnumerator PopUpTimer(float Timer)
    {
        if (!decision)
        {
            print(Timer);
            Timer = timer;
            yield return new WaitForSeconds(timer); 
            // Counts down time until popup ends. Not ran if decision is active
        }
        
        if(decision)
        {
            yield return new WaitUntil(() => responseMade);
        }

        ChoiceReset();
        canvas.SetActive(false);
        popUpText.text = string.Empty;
        yes.SetActive(false);
        no.SetActive(false);
        background.SetActive(false);

        // Sets canvas inactive and empties textbox for next use
    }
}
