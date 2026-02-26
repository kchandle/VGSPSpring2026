using System;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestStepsSO", menuName = "Scriptable Objects/QuestStepsSO")]
public class QuestStepsSO : ScriptableObject
{
    [field: SerializeField] public string Id { get; private set; }

    public String Questname;

    public int MoneyCollectionRequired;
    public bool DefeatEnemy;
    public int HowManyDefeatEnemy;
    public bool reginalDefeat;
    public int JanitorDefeat;
    public int CashierDefeat;
    public int HandymanDefeat;

}
