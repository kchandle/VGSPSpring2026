using UnityEngine;
using System.Collections.Generic;


//The type of attack boosted by the field
/*public enum BoostType
{
    None,
    Ice,
    Fire,
    Water,
    Earth,
    Wind,
    Light,
    Lightning,
    Poison,
    Dark,
    DamageBlock,
    Psychic,
    ALL_TYPES
}*/

//***

//This script will handle field effects like rain and blizzards in conjunction with FieldEffect_SO
[ System.Serializable ]
public struct FieldEffects
{
    [Header("Universal Attack/Defense Changes")] //directly multiplies the player and enemies' attack and defense.
    //Whether or not the Field Effect boosts attack and defense. Ensures the default values of zero don't cause issues. 
    public bool statChanges;

    //the boosts. Ex: attackBoost of 2 doubles damage
    public float attackBoost;
    public float enduranceBoost;


    [Header("Type-Based damage boosting")]
    //The types of damage that will be affected by the boostAmount
    public DamageType[] boostedTypes;

    //Amount the BoostType will be multiplied by. a boostAmout of .75 decreases damage by 25%
    public float boostAmount;


    [Header("Chip Damage dealt per turn")]
    //Whether or not the field deals chip damage (acid rain, thunderstorm)
    public bool dealsChipDamage;

    //Whether or not the chip damage only strikes one random target (thunderstorm)
    public bool chipIsRandom; 

    //Card containing the type and amount of chip damage to be dealt per turn to all targets on the field
    public Card_SO chipDamageCard;
    

    public FieldEffects(bool statChanges, float attackBoost, float enduranceBoost, DamageType[] boostedTypes, int boostAmount, bool dealsChipDamage, bool chipIsRandom, Card_SO chipDamageCard)
    {
        //
        this.statChanges = statChanges;
        this.attackBoost = attackBoost;
        this.enduranceBoost = enduranceBoost;

        //
        this.boostedTypes = boostedTypes;
        this.boostAmount = boostAmount;

        //
        this.dealsChipDamage = dealsChipDamage;
        this.chipIsRandom = chipIsRandom;
        this.chipDamageCard = chipDamageCard;
    }

    
}
