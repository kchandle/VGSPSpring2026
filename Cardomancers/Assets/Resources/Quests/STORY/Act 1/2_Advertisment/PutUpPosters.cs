using System;
using UnityEngine;
using UnityEngine.UI;

public class PutUpPostersQuestStep : QuestStep
{
    private GameObject[] posterLocations;
    [SerializeField] private GameObject posterInteract;
    // As of 5/5, I am using any sprites that are available as not enough of the posters are approved
    [SerializeField] private Sprite[] posterImages;
    private int postersPutUp;
    private int postersToPutUp = 5;
    
    private void Awake()
    {
        posterLocations = GameObject.FindGameObjectsWithTag("PosterLocation");
        foreach (GameObject go in posterLocations)
        {
            // Set the poster highlight to active
            go.transform.GetChild(0).gameObject.SetActive(true);
        }
        postersToPutUp = posterLocations.Length;
    }

    private void Start()
    {
        int i = 0;
        foreach (GameObject go in posterLocations)
        {
            // Instantiates a poster interact at each pre-set up poster location
            GameObject posterInteractInstance = Instantiate(posterInteract, go.transform.position, go.transform.rotation);
            posterInteractInstance.name = $"PosterInteract_{i}";
            i++;
        }
    }

    public void PutUpPoster()
    {
        // Play animation for putting up a poster
        postersPutUp++;
        this.ChangeState(postersPutUp.ToString());
        if (postersPutUp >= postersToPutUp)
        {
            FinishQuestStep();
        }
    }
    
    protected override void SetQuestStepState(string state)
    {
        try
        {
            postersPutUp = Int32.Parse(state);
        }
        catch
        {
            postersPutUp = 0;
            Debug.LogWarning("Quest save data for PutUpPosters was not formatted correctly. \n int postersPutUp has been initialized to 0.");
        }
    }
    
    public override string GetQuestStepState()
    {
        return $"{postersToPutUp - postersPutUp} posters left to put up.";
    }

    public Sprite GetPosterSprite()
    {
        return posterImages[postersPutUp];
    }
}
