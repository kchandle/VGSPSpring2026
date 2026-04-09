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

//This script will handle field effects like rain and blizzards
[ System.Serializable ]
public struct FieldEffects
{
    //The types of damage that will be affected by the boostAmount
    public DamageType[] boostedTypes;

    //Amount the BoostType will be multiplied by. a boostAmout of .75 decreases damage by 25%
    public float boostAmount;


    //Amount of chip damage done to all targets on the field per turn
    public int chipDamage;


    public FieldEffects(DamageType[] types, int amount, int chip)
    {
        boostedTypes = types;
        boostAmount = amount;

        chipDamage = chip;


    }

    
}
