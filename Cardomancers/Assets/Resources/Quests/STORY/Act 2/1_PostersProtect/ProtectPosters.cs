using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class ProtectPosters : QuestStep
{
    [SerializeField] private GameObject[] enemies;
    
    private void Start()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("PosterLocation"))
        {
            Instantiate(enemies[Random.Range(0, enemies.Length)], go.transform.position, Quaternion.identity);
        }
    }
    
    protected override void SetQuestStepState(string state)
    {
        throw new System.NotImplementedException();
    }

    public override string GetQuestStepState()
    {
        throw new System.NotImplementedException();
    }
}
