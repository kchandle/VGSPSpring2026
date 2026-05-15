using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class StatusEffectContainer
{
    public DamageType damageType;
    public int statusAmount;
    public bool isPerishable;
    public bool isNegative;
    public int turnsLasting;
    public int turnsRemaining;
    public BattleActionType actionType;
    public StatusEffectType statusType;
    
    //Source of the status effect. When two status effects of the same source are applied, it'll lengthen the turn count instead of applying an entirely seperate effect
    //This is only internal, the player doesn't need to know about it. 
    public string statusSource;
    

    public ParticleSystem[] particles;

    //Decrements the turns remaining by 1 if the status effect is perishable
    public int DecrementTurn()
    {
        if (isPerishable && turnsRemaining > 0)
        {
            turnsRemaining--;
            //Debug.Log("turnsRemainingDecremented: " + turnsRemaining);
        }
        else if (turnsRemaining <= 0 && isPerishable)
        {
            statusAmount = 0;
        }
        return turnsRemaining;
    }

    //Setting up the status effect container
    public StatusEffectContainer(DamageType damageType,int statusAmount, bool isPerishable, bool isNegative, int turnsLasting, ParticleSystem[] particles, BattleActionType actionType, StatusEffectType statusType, string statusSource)
    {
        this.damageType = damageType;
        this.statusAmount = statusAmount;
        this.isPerishable = isPerishable;
        this.isNegative = isNegative;
        this.turnsLasting = turnsLasting;
        this.turnsRemaining = turnsLasting;
        this.particles = particles;
        this.actionType = actionType;
        this.statusType = statusType;
        this.statusSource = statusSource;
    }
}
