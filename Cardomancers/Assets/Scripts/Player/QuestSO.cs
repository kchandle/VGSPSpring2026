using System;
using UnityEngine;
using System.Collections;


[CreateAssetMenu(fileName = "QuestSO", menuName = "Scriptable Objects/QuestInfoSO", order = 1)]
public class QuestSO : ScriptableObject
{
    [field: SerializeField] public string Id { get; private set; }

    //quest naming
    public String displayname;

    //required before starting
    public int playerRequirement; //Level player needs to be at
    public QuestSO[] questPrerrequesites;

    public ScriptableObject[] questSteps;

    // rewards
    public int moneyReward;
    public int ExpReward;

   

}
