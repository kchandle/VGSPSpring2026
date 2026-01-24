using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
        [SerializeField] private Enemy_SO enemySO;
        // InventoryCard[] deck: Deck of the enemy. Copy from enemySO on instantiation
       public int maxHealth; //Max health of the enemy.
       public int currentHealth; //  MaxHealth by default
       bool isStunned; // Whether or not the enemy is stunned. If the enemy is stunned, they cannot take actions.
        [SerializeField] private Animator animator;   //Animator for the enemy’s sprites.


void Awake()
    {
        // Check if the SO exists to avoid NullReferenceExceptions
        if (enemySO != null)
        {
            // sets Max Health from the SO and sets the current health to max health
            maxHealth = enemySO.maxHealth;
            currentHealth = maxHealth;
        }
        else
        {
            Debug.LogError("Enemy_SO is missing on " + gameObject.name);
        }

        animator = GetComponent<Animator>();
    }
    void Start()
    {      
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
