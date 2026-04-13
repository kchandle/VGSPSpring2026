using TMPro;
using UnityEngine;
using System.Collections;

public class TextPopup : MonoBehaviour
{
    public GameObject text;
    [SerializeField] private GameObject canvas;
    
    /// <summary>
    /// A method that displays a popup for a limited amount of time
    /// </summary>
    /// <param name="displayText">A string that holds the text the popup will show the player</param>
    /// <param name="position">The position on screen where the popup will be displayed represented as a Vector2</param>
    /// <param name="seconds">The time, in seconds, the popup will be displayed for</param>
    public void DisplayPopup(string displayText, Vector2 position, float seconds)
    {
        text.GetComponent<TextMeshProUGUI>().text = displayText;
        text.GetComponent<RectTransform>().anchoredPosition = position;
        StartCoroutine(TextPopupCoroutine(seconds));
    }

    private IEnumerator TextPopupCoroutine(float seconds)
    {
        GameObject tempText = Instantiate(text, canvas.transform);
        yield return new WaitForSeconds(seconds);
        Destroy(tempText);
    }
}
