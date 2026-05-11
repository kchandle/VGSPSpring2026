using System;
using UnityEngine;
using DialogueScripts;
using System.Collections.Generic;

public class ConvinceYoungsters : QuestStep
{
    private SerializeableDictionaryOfBattleSOAndBool youngsters;
    [SerializeField] private DialogueSO finishDialogue;

    private void Awake()
    {
        GameObject[] youngstersKeys = GameObject.FindGameObjectsWithTag("Youngster");
        foreach (GameObject go in youngstersKeys)
        {
            youngsters.Add(go.GetComponent<StartBattle>().battleToStart, false);
        }
    }

    private void OnEnable()
    {
        BattleManager.instance.OnWin.AddListener(CheckIfPlayerDefeatedYoungster);
        DialogueEvents.OnEndDialogue += FinishMe;
    }

    private void OnDisable()
    {
        BattleManager.instance.OnWin.RemoveListener(CheckIfPlayerDefeatedYoungster);
        DialogueEvents.OnEndDialogue -= FinishMe;
    }

    private void CheckIfPlayerDefeatedYoungster()
    {
        DefeatYoungster(BattleManager.instance.battle);
    }

    private void DefeatYoungster(Battle_SO youngster)
    {
        if (!youngsters.ContainsKey(youngster))
        {
            return;
        }
        youngsters[youngster] = true;
        ChangeState(JsonUtility.ToJson(youngsters));
    }

    // Should be called when the player goes back to the card shop and talks to Mallory while having convinced at least one
    public void FinishMe(DialogueSO dialogue)
    {
        if (dialogue != finishDialogue) return;
        FinishQuestStep();
    }
    
    protected override void SetQuestStepState(string state)
    {
        youngsters = JsonUtility.FromJson<SerializeableDictionaryOfBattleSOAndBool>(state);
    }

    public override string GetQuestStepState()
    {
        int i = 0;
        foreach (Battle_SO SO in youngsters.Keys)
        {
            if (youngsters[SO] == true)
            {
                i++;
            }
        }
        return $"Youngsters convinced: {i}";
    }
}

[Serializable]
public class SerializeableDictionaryOfBattleSOAndBool : SerializableDictionary<Battle_SO, bool>
{
    
}