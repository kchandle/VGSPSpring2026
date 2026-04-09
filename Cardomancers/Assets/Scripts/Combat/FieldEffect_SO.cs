using UnityEngine;
using System.Collections.Generic;


//What type the field is (weather / something else)
public enum FieldType
{
    WEATHER,
    OTHER,
    NONE
}



//Field Effects such as Rainstorm and Blizzard
[CreateAssetMenu(fileName = "FieldEffect_SO", menuName = "Scriptable Objects/FieldEffect_SO")]
public class FieldEffect_SO : ScriptableObject
{
    //Number of turns the field will last
    public int turnsActive;

    //Number of turns left the field has
    public int turnsRemaining;

    //Name of the field effect
    public string name; 

    //
    public string description;

    //Whether or not the effect is active
    public bool active; 


    //The effects of the field
    public FieldEffects[] effects;

    //The Field's type 
    public FieldType fieldType;

    void Awake()
    {
        turnsRemaining = turnsActive;
    }

    //Visual effect
    //
    
}
