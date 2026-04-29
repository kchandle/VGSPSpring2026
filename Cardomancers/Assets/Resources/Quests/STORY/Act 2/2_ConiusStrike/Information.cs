using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class TalkToPeople : QuestStep
{
    [Tooltip("The NPCs to be instantiated when the quest starts.")]
    [SerializeField] private GameObject[] people;
    [Tooltip("The amount of NPCs the player must talk to.")]
    [SerializeField] private int NPCsToTalkTo;

    [Tooltip("Possible locations NPCs can spawn")]
    [SerializeField] private List<Transform> NPCLocations;
    private int NPCsTalkedTo;

    private void Awake()
    {
        if (NPCLocations.Count > people.Length)
        {
            Debug.LogError("NPCLocations count is greater than people length. This will cause NPCs to be spawned at the same location. The quest step for this has been destroyed.");
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        List<int> possibleIndices = Enumerable.Range(0, NPCLocations.Count).ToList();
        foreach (GameObject go in people)
        {
            int i = Random.Range(0, possibleIndices.Count);
            Instantiate(go, NPCLocations[i].position, Quaternion.identity, NPCLocations[i]);
            possibleIndices.Remove(i);
        }
    }

    public void TalkToNPC()
    {
        NPCsTalkedTo++;
        this.ChangeState(NPCsTalkedTo.ToString());
        if (NPCsTalkedTo >= NPCsToTalkTo)
        {
            // Play cutscene for player being knocked out
            // Teleport player to an alleyway
            this.FinishQuestStep();
        }
    }
    
    protected override void SetQuestStepState(string state)
    {
        try
        {
            NPCsTalkedTo = Int32.Parse(state);
        }
        catch
        {
            NPCsTalkedTo = 0;
            Debug.LogError("State information for TalkToPeople quest step was damaged. \n State is currently: \n " + state);
        }
    }

    public override string GetQuestStepState()
    {
        return $"People informed about hacking: {NPCsTalkedTo} / {NPCsToTalkTo}";
    }
}
