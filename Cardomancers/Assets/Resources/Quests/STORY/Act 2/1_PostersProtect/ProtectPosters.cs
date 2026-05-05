using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class ProtectPosters : QuestStep
{
    [Tooltip("A list containing the possible enemies who will be attacking the posters.")]
    [SerializeField] private GameObject[] enemies;
    private GameObject[] posterLocations;
    private List<GameObject> postersDefended;
    private Dictionary<GameObject, PosterEnemy> enemiesInstantiated = new Dictionary<GameObject, PosterEnemy>();

    private void Awake()
    {
        posterLocations = GameObject.FindGameObjectsWithTag("PosterLocation");
        
    }
    
    private void Start()
    {
        foreach (GameObject go in posterLocations)
        {
            int i = Random.Range(0, enemies.Length);
            GameObject enemy = Instantiate(enemies[i], go.transform.position, Quaternion.identity);
            enemiesInstantiated.Add(go, enemy.GetComponent<PosterEnemy>());
            if (postersDefended.Contains(go))
            {
                enemy.gameObject.SetActive(false);
            }
        }
    }

    public void DefendPoster(GameObject poster)
    {
        postersDefended.Add(poster);
        enemiesInstantiated[poster].InitiateInteraction();
        SetQuestStepState(JsonUtility.ToJson(postersDefended));
        if (posterLocations.ToList() == postersDefended)
        {
            this.FinishQuestStep();
        }
    }
    
    protected override void SetQuestStepState(string state)
    {
        postersDefended = JsonUtility.FromJson<List<GameObject>>(state);
    }

    public override string GetQuestStepState()
    {
        return $"Posters defended: {postersDefended.Count} / {posterLocations.Length}";
    }
}
