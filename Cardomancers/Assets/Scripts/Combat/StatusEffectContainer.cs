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
    public int turnsLasting;
    public int turnsRemaining;
    public BattleActionType actionType;

    public ParticleSystem[] particles;

    //Decrements the turns remaining by 1 if the status effect is perishable
    public int DecrementTurn()
    {
        if (isPerishable && turnsRemaining > 0)
        {
            turnsRemaining--;
            Debug.Log("turnsRemainingDecremented: " + turnsRemaining);
        }
        else if (turnsRemaining <= 0 && isPerishable)
        {
            statusAmount = 0;
        }
        return turnsRemaining;
    }

    //Setting up the status effect container
    public StatusEffectContainer(DamageType damageType,int statusAmount, bool isPerishable, int turnsLasting, ParticleSystem[] particles, BattleActionType actionType)
    {
        this.damageType = damageType;
        this.statusAmount = statusAmount;
        this.isPerishable = isPerishable;
        this.turnsLasting = turnsLasting;
        this.turnsRemaining = turnsLasting;
        this.particles = particles;
        this.actionType = actionType;
    }
}
