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

    public List<InventoryCard> deck;


    void Awake()
    {
        // Check if the SO exists to avoid NullReferenceExceptions
        if (enemySO != null)
        {
            // sets Max Health from the SO and sets the current health to max health
            maxHealth = enemySO.maxHealth;
            currentHealth = maxHealth;
            deck = new List<InventoryCard>(enemySO.deck);
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
        if (deck.Count <= 0)
        {
            deck = new List<InventoryCard>(enemySO.deck);
        }
    }

    //Draws a random card from deck then removes it
    public InventoryCard DrawCard()
    {
        // Pick random card from deck then remove from deck
        InventoryCard card = deck[Random.Range(0, deck.Count)];
        deck.Remove(card);
        //Temp line to show what card was picked
        Debug.Log(card);
        return card;
    }

}
