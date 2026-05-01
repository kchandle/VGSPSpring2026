using System;
using UnityEngine;

public class GatherIngredients : QuestStep
{
    private int ingredientsToGather = 10;
    private int ingredientsGathered = 0;

    private void Start()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Ingredient"))
        {
            go.SetActive(true);
        }
    }

    public void GatherIngredient()
    {
        ingredientsGathered++;
        SetQuestStepState(ingredientsGathered.ToString());
        if(ingredientsGathered >= ingredientsToGather)
        {
            this.FinishQuestStep();
        }
    }

    protected override void SetQuestStepState(string state)
    {
        try
        {
            ingredientsGathered = Int32.Parse(state);
        }
        catch
        {
            Debug.LogWarning("Error when parsing state data for GatherIngredients quest step.");
            ingredientsGathered = 0;
        }
    }

    public override string GetQuestStepState()
    {
        return $"{ingredientsGathered} / {ingredientsToGather} ingredients gathered from the park";
    }
}
