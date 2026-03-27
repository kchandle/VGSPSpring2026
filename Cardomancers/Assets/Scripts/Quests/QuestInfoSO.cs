using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "Quests/QuestInfoSO")]
public class QuestInfoSO : ScriptableObject
{
    //TODO: private setter, public getter
    public string ID; 
    //TODO: revert to readonly
    public string displayName;

    [Header("Requirements to start")]
    public int levelRequirement;
    public QuestInfoSO[] prerequisiteQuests;

    [Header("Steps")] 
    [Tooltip("Prefab containing the quest step script for each quest step")]
    public GameObject[] questSteps;

    [Header("Rewards")] 
    public int moneyReward;
    public int expReward;
    public DictionaryOfCardSOandInt cardRewards; 
    public DictionaryOfHackSOandInt hackRewards;

    private void OnValidate()
    {
        ID = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
    }

}
