using UnityEngine;

[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "Quests/QuestInfoSO")]
public class QuestInfoSO : ScriptableObject
{
    public string ID { get;  private set; }
    private string displayName;

    [Header("Requirements to start")] 
    public int levelRequirement { get; }
    public QuestInfoSO[] prerequisiteQuests { get; }
    
    [Header("Steps")]
    [Tooltip("Prefab containing the quest step script for each quest step")]
    public GameObject[] questSteps { get; }

    [Header("Rewards")] 
    public int moneyReward { get; }
    public int expReward { get; }
    public Card_SO[] cardRewards { get; }
    public Hack_SO[] hackRewards { get; }

    private void OnValidate()
    {
        ID = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
    }

}
