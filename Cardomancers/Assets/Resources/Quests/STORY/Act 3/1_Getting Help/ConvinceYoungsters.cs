using System;
using UnityEngine;
using System.Collections.Generic;

public class ConvinceYoungsters : QuestStep
{
    private SerializeableDictionaryOfGameObjectAndBool youngsters;

    private void Awake()
    {
        GameObject[] youngstersKeys = GameObject.FindGameObjectsWithTag("Youngster");
        foreach (GameObject go in youngstersKeys)
        {
            youngsters.Add(go, false);
        }
    }
    
    public void DefeatYoungster(GameObject youngster)
    {
        if (!youngsters.ContainsKey(youngster))
        {
            Debug.LogError("Attempted to fight gameobject that was not a youngster");
            return;
        }
        youngsters[youngster] = true;
        ChangeState(JsonUtility.ToJson(youngsters));
    }

    // Should be called when the player goes back to the card shop and talks to Mallory while having convinced at least one
    public void FinishMe()
    {
        // Store the number of youngsters the player defeated somewhere, so it can be checked later
        FinishQuestStep();
    }
    
    protected override void SetQuestStepState(string state)
    {
        youngsters = JsonUtility.FromJson<SerializeableDictionaryOfGameObjectAndBool>(state);
    }

    public override string GetQuestStepState()
    {
        int i = 0;
        foreach (GameObject go in youngsters.Keys)
        {
            if (youngsters[go] == true)
            {
                i++;
            }
        }
        return $"Youngsters convinced: {i}";
    }
}

[Serializable]
public class SerializeableDictionaryOfGameObjectAndBool : SerializableDictionary<GameObject, bool>
{
    
}