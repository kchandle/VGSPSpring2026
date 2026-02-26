using UnityEngine;

public class QuestManager : MonoBehaviour
{
    QuestStepsSO queststepsSO;
    QuestSO questSO;

     void Awake()
    {
        queststepsSO = GetComponent<QuestStepsSO>();
        questSO = GetComponent<QuestSO>();


    }
}
