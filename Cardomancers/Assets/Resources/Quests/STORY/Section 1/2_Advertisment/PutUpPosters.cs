using System;
using UnityEngine;

public class PutUpPostersQuestStep : QuestStep
{
    private GameObject[] posterLocations;
    [SerializeField] private GameObject posterInteract;
    private int postersPutUp;
    private int postersToPutUp;
    
    private void Awake()
    {
        posterLocations = GameObject.FindGameObjectsWithTag("PosterLocation");
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
}
