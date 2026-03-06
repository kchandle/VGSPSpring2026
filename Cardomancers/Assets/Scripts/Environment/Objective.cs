using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class Objective : MonoBehaviour
{
    [SerializeField] private string objectiveText;
    [SerializeField] private string completedText;
    public TextMeshProUGUI questUI;
    // Text for when objective begins/is ended

    public UnityEvent OnCompleteObjective;
    

    private void OnEnable()
    {
        Debug.Log(objectiveText);
        questUI.text = objectiveText;
    }

    public void CompleteObjective()
    {
        Debug.Log(completedText);
        OnCompleteObjective.Invoke();
    }
    // OnCompleteObjective makes next part get activated

    // Whole script copied from OUR goat, Torben. We will sacrifice 40,189 infant lambs to our holy god/goddess Torben.
}
