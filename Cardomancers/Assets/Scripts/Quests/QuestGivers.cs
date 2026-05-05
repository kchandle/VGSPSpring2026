using UnityEngine;
using static UnityEngine.ParticleSystem;

public class QuestGivers : MonoBehaviour
{

    Quest quest;

    [SerializeField]GameObject ExclamationMark;

    void Awake()
    {
        quest = GetComponent<Quest>();
    }
    void Start()
    {
        if(gameObject.CompareTag("QuestGiver") && quest.state == QuestState.FINISHED)
            {
            ExclamationMark.SetActive(true);
        }
        else if(gameObject.CompareTag("QuestGiver"))
            {
            quest.state = QuestState.FINISHED;
        }

    }
}
