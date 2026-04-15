using UnityEngine;

public class Poster : MonoBehaviour
{
    private PutUpPostersQuestStep parentQuestStep;

    private void Awake()
    {
        parentQuestStep = transform.parent.GetComponent<QuestStep>() as PutUpPostersQuestStep;
    }
    
    public void Interact()
    {
        parentQuestStep.PutUpPoster();
    }
}
