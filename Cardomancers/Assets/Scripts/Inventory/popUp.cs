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
    // "Decision" if the pop up has a yes or no
    private bool decision = false;
    // choice depends on which button hit
    public bool choice;
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
        choice = true;
        responseMade = true;
    }

    public void OnNoClick()
    {
        choice = false;
        responseMade = true;
    }

    // Set by yes/no buttons, Decision can be used by script that calls and responseMade lets the coroutine pass through

    public void SettingActive()
    {
        canvas.SetActive(true);
        if (decision) 
        {
            yes.SetActive(true);
            no.SetActive(true);
        }
        StartCoroutine(popUpTimer(timer));
    }
    // Sets canvas active and starts coroutine, if "decision" active turn on the yes/no

    IEnumerator popUpTimer(float Timer)
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

        canvas.SetActive(false);
        popUpText.text = string.Empty;
        yes.SetActive(false);
        no.SetActive(false);

        // Sets canvas inactive and empties textbox for next use
    }
}
