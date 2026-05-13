using UnityEngine;
using System;

[RequireComponent(typeof(BoxCollider))]
public class Ingredient : MonoBehaviour
{
    private static readonly int Gather = Animator.StringToHash("Gather");
    private BoxCollider boxTrigger;

    private void Awake()
    {
        boxTrigger = GetComponent<BoxCollider>();
        boxTrigger.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            // Disable trigger to prevent this method being called many times from one ingredient
            boxTrigger.enabled = false;
            // Play animation for picking it up
            GetComponent<Animator>().SetTrigger(Gather);
            FindFirstObjectByType<GatherIngredients>().GetComponent<GatherIngredients>().GatherIngredient();
        }
    }
    
    // Used at the end of the animation to destroy the game object.
    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
