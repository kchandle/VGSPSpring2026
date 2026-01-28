using UnityEngine;

public class StatusEffect : BattleEffect
{
    public bool isPerishable;
    public int turnsActive; //Amount of turns active at start of effect
    public int turnsRemaining; //Amount of turns remaining of active effect

    //The damage type of the BattleEffect (used for determining weakness/resistance in enemies/player)
    public DamageType damageType;

    private void Awake()
    {
        turnsRemaining = turnsActive;
    }

    public bool IsActive()
    {
        turnsRemaining--;
        if (turnsRemaining <= 0 && isPerishable)
        {
            return true; // Effect has expired
        }
        return false; // Effect is still active
    }
}
