using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestLogButton : MonoBehaviour, ISelectHandler
{
    public Button button { get; private set; }
    private UnityAction onSelectAction;
    private TextMeshProUGUI buttonText;
    private string questID;

    public string QuestID
    {
        get { return questID; }
        set { if (string.IsNullOrEmpty(questID)) questID = value; }
    }

    public void Initialize(string displayName, UnityAction selectAction)
    {
        this.button = this.GetComponent<Button>();
        this.buttonText = this.GetComponentInChildren<TextMeshProUGUI>();
        
        this.buttonText.text = displayName;
        this.onSelectAction = selectAction;
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        onSelectAction();
    }
}
