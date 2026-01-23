using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "Enemy_SO", menuName = "Scriptable Objects/Enemy_SO")]
public class Enemy_SO : ScriptableObject
{
    //  The list of cards that the enemy has in their battle.

    // InventoryCard[] deck;
    
   public int maxHealth; //Max health of the enemy.
   public string displayName; //The name of the enemy.
    public int moneyDrops; //The base amount of money the enemy drops when defeated 
    public float xpDrops;  //The base amount of XP the enemy drops when defeated

    //public Dictionary<CardSO, float> cardDrops; //dictionary of possible cards, the enemy and the probability that the card will be dropped.
    public Dictionary <Hack_SO, float> hackDrops; //dictionary of possible hacks, the enemy and the probability that the card will be dropped.
}
