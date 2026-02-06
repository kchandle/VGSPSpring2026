using UnityEngine;

public struct StatusEffectContainer
{
    public DamageType damageType;
    public int statusAmount;
    public bool isPerishable;
    public int turnsLasting;
    private int turnsRemaining;

    public ParticleSystem[] particles;

    //Decrements the turns remaining by 1 if the status effect is perishable
    public int DecrementTurn()
    {
        if (isPerishable && turnsRemaining > 0)
        {
            turnsRemaining--;
        }
        return turnsRemaining;
    }

    //Setting up the status effect container
    public StatusEffectContainer(DamageType damageType,int statusAmount, bool isPerishable, int turnsLasting, ParticleSystem[] particles)
    {
        this.damageType = damageType;
        this.statusAmount = statusAmount;
        this.isPerishable = isPerishable;
        this.turnsLasting = turnsLasting;
        this.turnsRemaining = turnsLasting;
        this.particles = particles;
    }
}
