using UnityEngine;

[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "Quests/QuestInfoSO")]
public class QuestInfoSO : ScriptableObject
{
    public string ID; 
    private readonly string displayName;

    [Header("Requirements to start")]
    public int levelRequirement;
    public QuestInfoSO[] prerequisiteQuests;

    [Header("Steps")] 
    [Tooltip("Prefab containing the quest step script for each quest step")]
    public GameObject[] questSteps;

    [Header("Rewards")] 
    public int moneyReward;
    public int expReward;
    public Card_SO[] cardRewards; 
    public Hack_SO[] hackRewards;

    private void OnValidate()
    {
        ID = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
    }

}
