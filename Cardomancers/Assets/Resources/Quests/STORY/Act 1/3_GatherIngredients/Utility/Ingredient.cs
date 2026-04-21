using UnityEngine;
using System;

[RequireComponent(typeof(BoxCollider))]
public class Ingredient : MonoBehaviour
{
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
            FindFirstObjectByType<GatherIngredients>().GetComponent<GatherIngredients>().GatherIngredient();
        }
    }
}
