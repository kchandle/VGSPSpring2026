using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Enemy_SO enemySO;
    // InventoryCard[] deck: Deck of the enemy. Copy from enemySO on instantiation
    public List<Card_SO> hand = new List<Card_SO>();
    public int maxHealth; //Max health of the enemy.
    public int currentHealth; //  MaxHealth by default
    bool isStunned; // f the enemy is stunned, they cannot take actions.
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
    
    public void ShuffleDeck()
    {
        // If deck has less than or equal to zero cards, shuffle the deck
        //if (deck.Count <= 0)
        //{
        //  deck = new List<InventoryCard>(enemySO.deck);
        //  hand.Clear();
        //  hand = DrawCards(5); // Draw 5 cards
        //}
    }

    public void DrawCards()
    {
        // Draw cards from the deck to the hand
        // Display card in playspace
    }

}
