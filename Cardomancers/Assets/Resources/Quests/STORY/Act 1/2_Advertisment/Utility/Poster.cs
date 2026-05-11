using UnityEngine;
using UnityEngine.UI;

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
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).GetComponentInChildren<Image>().sprite = FindFirstObjectByType<PutUpPostersQuestStep>()
            .GetComponent<PutUpPostersQuestStep>().GetPosterSprite();
        // Makes the poster active
        transform.GetChild(1).gameObject.SetActive(true);
    }
}
