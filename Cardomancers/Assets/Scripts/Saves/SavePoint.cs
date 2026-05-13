using UnityEngine;

// automatically adds a sphere collider to the game object the script is attached to
[RequireComponent(typeof(SphereCollider))]
public class SavePoint : MonoBehaviour
{
    [SerializeField] private SphereCollider trigger;
    [SerializeField] private QuestManager questManager;

    //automatically assign sphere collider component and make it a trigger
    private void Awake()
    {
        trigger = GetComponent<SphereCollider>();
        trigger.isTrigger = true;
        
        questManager = FindFirstObjectByType<QuestManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //only save if the player is entering the save point
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Saving");
            SaveSystem.Save(other.gameObject, questManager);
        }
    }
}
