using UnityEngine;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.Animations;
#endif

[CreateAssetMenu(fileName = "Enemy_SO", menuName = "Scriptable Objects/Enemy_SO")]
public class Enemy_SO : ScriptableObject
{
    //  The list of cards that the enemy has in their battle.

    public InventoryCard[] deck;
    
    public int maxHealth; //Max health of the enemy.
    public string displayName; //The name of the enemy.
    // public int moneyDrops; //The base amount of money the enemy drops when defeated 
    // public float xpDrops;  //The base amount of XP the enemy drops when defeated
    public int timer; // turns until enemy acts
    public int energy; //energy it can use on cards
    public Sprite enemyImage;
    #if UNITY_EDITOR
    public AnimatorController enemyAttkAnim;
    #endif
    public GameObject enemyPrefab; //Prefab of the enemy to be spawned in battle.

    public List<DamageType> resistances; //List of damage types the enemy is resistant to.
    public List<DamageType> weaknesses; //List of damage types the enemy is weak to.

    public List<Drop> drops; //List of all possible drops
    
}


[Serializable]
public struct Drop
{
    [Tooltip("Set greater than or equal to 1, or if guaranteed, -1")]
    public float weight;
    [Tooltip("Set to the Scriptable object given, None if Money or EXP")]
    public ScriptableObject item;
    [Tooltip("Amount of Money or EXP")]
    public int quantity;

    public enum DropType
    {
        CARD,
        HACK,
        MONEY,
        EXP,
        MISC
    }
    [Tooltip("The type of drop")]
    public DropType dropType;
}
