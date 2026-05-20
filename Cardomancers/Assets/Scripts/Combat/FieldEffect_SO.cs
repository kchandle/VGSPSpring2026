using UnityEngine;
using System.Collections.Generic;


//What type the field is (weather / something else)
public enum FieldType
{
    WEATHER,
    OTHER,
    NONE
}

//***

//Field Effects such as Rainstorm and Blizzard
//NOTE: cards that set field conditions have to have the attack type
[CreateAssetMenu(fileName = "FieldEffect_SO", menuName = "Scriptable Objects/FieldEffect_SO")]
public class FieldEffect_SO : ScriptableObject
{
    [Header("Basic Field Info")]
    //---Both will be overwritten and replaced by the turnsRemaining quantity of the card that summons the field effect
    //Number of turns the field will last. 
    public int turnsActive;
    //Number of turns left the field has
    public int turnsRemaining;
    //---

    //Name of the field effect
    public string name; 

    //This isn't really required
    public string description;

    //Whether or not the effect is active
    public bool active; 

    //Effect the field will play at the end of each turn, just uses the same vfx as the weather cards for now
    public GameObject vfxPrefab;



    [Header("Check the following according to what the field does.")] //This makes checking in the code faster
    //Whether or not the field boosts / decreases the attack or defense of targets on the field
    public bool hasStatChanges;

    //Whether or not the field boosts / decreases any damage types
    public bool boostsTypeDamage;

    //Whether or not the field deals chip damage
    public bool chipDamage;

    

    [Header("The Field's Effects")]
    //The effects of the field
    public FieldEffects[] effects;



    [Header("The Category of the Field")]
    //The Field's type 
    public FieldType fieldType;

    void Awake()
    {
        turnsRemaining = turnsActive;
    }

    //Visual effect
    //
    
}
