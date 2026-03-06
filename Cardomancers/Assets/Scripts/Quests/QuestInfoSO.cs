using UnityEngine;

[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "Quests/QuestInfoSO")]
public class QuestInfoSO : ScriptableObject
{
    public string ID { get;  private set; }
    private string displayName;

    [Header("Requirements to start")] 
    private int levelRequirement;
    private QuestInfoSO[] prerequisiteQuests;
    
    [Header("Steps")]
    [Tooltip("Prefab containing the quest step script for each quest step")]
    public GameObject[] questSteps { get; }

    [Header("Rewards")] 
    private int moneyReward;
    private int expReward;
    private Card_SO[] cardRewards;
    private Hack_SO[] hackRewards;

    private void OnValidate()
    {
        ID = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
    }

}
