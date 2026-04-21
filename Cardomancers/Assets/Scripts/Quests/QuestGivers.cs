using UnityEngine;
using static UnityEngine.ParticleSystem;

public class QuestGivers : MonoBehaviour
{
    [SerializeField]GameObject ExclamationMark;
    void Start()
    {
        if(gameObject.CompareTag("QuestGiver"))
            {
            ExclamationMark.SetActive(true);
        }
    }
}
