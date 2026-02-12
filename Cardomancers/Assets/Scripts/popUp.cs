using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class popUp : MonoBehaviour
{
    // public popUp_SO;
    public Image image;
    public TextMeshProUGUI popUpText;
    public GameObject canvas;
    private float timer;
    
    public void onEnabled()
    {
        popUpText.text = popUp_SO.Text;
        canvas.SetActive(true);
        StartCoroutine(popUpTimer(timer));
    }

    IEnumerator popUpTimer(float Timer)
    {
        yield return new WaitForSeconds(Timer); 

        canvas.SetActive(false);
    }
}
