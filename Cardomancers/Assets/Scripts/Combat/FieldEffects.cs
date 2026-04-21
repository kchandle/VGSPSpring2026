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
    [Header("Type-Based damage boosting")]
    //The types of damage that will be affected by the boostAmount
    public DamageType[] boostedTypes;
    //Amount the BoostType will be multiplied by. a boostAmout of .75 decreases damage by 25%
    public float boostAmount;


    [Header("Chip Damage dealt per turn")]
    //Card containing the type and amount of chip damage to be dealt per turn to all targets on the field
    public bool dealsChipDamage;
    //public bool chipIsRandom; //not used yet
    public Card_SO chipDamageCard;
    

    public FieldEffects(DamageType[] boostedTypes, int boostAmount, bool dealsChipDamage, Card_SO chipDamageCard)
    {
        //
        this.boostedTypes = boostedTypes;
        this.boostAmount = boostAmount;

        //
        this.dealsChipDamage = dealsChipDamage;
        this.chipDamageCard = chipDamageCard;
    }

    
}
