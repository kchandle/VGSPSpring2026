using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "Quests/QuestInfoSO")]
public class QuestInfoSO : ScriptableObject
{
    // The ID of the quest, matches the name of the file
    public string ID { get; private set; }
    // The name displayed to the player
    public string displayName;

    [Header("Requirements to start")]
    // The level the player must have to start
    public int levelRequirement;
    // All the quests the player must have completed to start
    public QuestInfoSO[] prerequisiteQuests;

    [Header("Steps")] 
    [Tooltip("Prefab containing the quest step script for each quest step")]
    public GameObject[] questSteps;

    [Header("Rewards")] 
    // How much money the player gets when they complete the quest
    public int moneyReward;
    // How much experience the player gets when they complete the quest
    public int expReward;
    [Tooltip("Keys list contains the Card_SO to be rewarded. The matching element in the Values list is the number of that card.")]
    public DictionaryOfCardSOandInt cardRewards; 
    [Tooltip("Keys list contains the Hack_SO to be rewarded. The matching element in the Values list is the number of that hack.")]
    public DictionaryOfHackSOandInt hackRewards;

    private void OnValidate()
    {
        ID = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
    }

}
