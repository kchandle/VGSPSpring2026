using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class popUp : MonoBehaviour
{
    public popUp_SO popSO;
    public Image image;
    public TextMeshProUGUI popUpText;
    public GameObject canvas;
    public GameObject yesorno;
    // "Decision" if the pop up has a yes or no
    public bool decision;
    private float timer;

    public void SetVariables(popUp_SO popSO)
    {
        this.popSO = popSO;
        timer = popSO.timer;
        decision = popSO.decision;
        SettingActive();
    } 
    // Script called by other scripts that incur popups, passing in their popup_SO


    public void SettingActive()
    {
        canvas.SetActive(true);
        popUpText.text = popSO.popUpText;
        if (decision) 
        {
            yesorno.SetActive(true);
        }
        StartCoroutine(popUpTimer(timer));
    }
    // Sets canvas active and starts coroutine, if "decision" active turn on the yes/no

    IEnumerator popUpTimer(float Timer)
    {
        print(Timer);
        Timer = timer;
        yield return new WaitForSeconds(Timer); 
        // Counts down time until popup ends

        canvas.SetActive(false);
        popUpText.text = string.Empty;
        // Sets canvas inactive and empties textbox for next use
    }
}
