using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Objective : MonoBehaviour
{
    [SerializeField] private string objectiveText;
    [SerializeField] private string completedText;
    bool completion;
    float timer = 0f;
    public TextMeshProUGUI questUI;
    public TextMeshProUGUI completionUI;
    //Battle only needed if on=bjective requires a battle to continue
    public Battle_SO battle_so;
    // Text for when objective begins/is ended

    public UnityEvent OnCompleteObjective;
    public BattleManager battleManager;
    

    private void OnEnable()
    {
        Debug.Log(objectiveText);
        questUI.text = objectiveText;
        
    }

    public void SetBattleManager(BattleManager battleManagerPass)
    {
        battleManager = battleManagerPass;
        battleManager.OnWin.AddListener(() => CompleteObjective(battleManager.battle));
    }

    public void StartTimer()
    {
        StartCoroutine(SuccessTextTimer());
    }

    public void CompleteObjective()
    {
        //Debug.Log(completedText);
        timer = 5f;
        OnCompleteObjective.Invoke();
        completion = true;
    }

    public void CompleteObjective(Battle_SO battle)
    {
        if (!(battle == battle_so)) return;
        Debug.Log(completedText);
        timer = 5f;
        OnCompleteObjective.Invoke();
        completion = true;
    }
    // OnCompleteObjective makes next part get activated
    IEnumerator SuccessTextTimer()
    {
        completionUI.text = completedText;
        yield return new WaitForSeconds(timer);
        completionUI.text = string.Empty;
        this.gameObject.SetActive(false);
    }
    // coroutine makes the text show up and then disappear the full object

    // Whole script copied from OUR goat, Torben. We will sacrifice 40,189 infant lambs to our holy god/goddess Torben.
}
