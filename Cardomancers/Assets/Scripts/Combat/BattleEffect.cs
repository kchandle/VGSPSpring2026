using Unity.VisualScripting;
using UnityEngine;

public enum DamageType
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

}

[ System.Serializable ]
public struct BattleEffect 
{
    // The amount of damage/heal/stun/whatever the BattleEffect inflicts on the target
    public int StatusAmount;
    //The damage type of the BattleEffect (used for determining weakness/resistance in enemies/player)
    public DamageType damageType;

    // Status effect related variables, only used if isStatusEffect is true
    public bool isStatusEffect; // Whether this BattleEffect is a status effect
    public bool isPerishable; //If status effect is perishable
    public int turnsActive; //Amount of turns active at start of effect    

    //A list of particle effects to happen when the BattleEffect is played
    ParticleSystem[] particles;

    //public void PlayParticles(Vector3 pos)
    //{
    //    foreach (ParticleSystem particle in particles)
    //    {
    //        Instantiate(particle, pos, Quaternion.identity);
    //    }
    //}

    public BattleEffect(int statusAmount, DamageType damageType, bool isStatusEffect, bool isPerishable, int turnsActive, ParticleSystem[] particles)
    {
        this.StatusAmount = statusAmount;
        this.damageType = damageType;
        this.isStatusEffect = isStatusEffect;
        this.isPerishable = isPerishable;
        this.turnsActive = turnsActive;
        this.particles = particles;
    }


    //The function that is called when the card is played: Change into Overload function for player and enemy respectively
    public void TriggerEffect(GameObject target, Vector3 pos)
    {
        bool isEnemy = target.CompareTag("Enemy");
        bool isPlayer = target.CompareTag("Player");

        int DamageDealt = StatusAmount;

        if (isEnemy)
        {
            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy.isShielded) return;

            if (isStatusEffect)
            {
                enemy.statusEffects.Add(new StatusEffectContainer(damageType,StatusAmount , isPerishable,turnsActive,particles));
                return;
            }

            foreach (DamageType resistance in enemy.resistances)
            {
                if (resistance == damageType)
                {
                    DamageDealt = Mathf.RoundToInt(StatusAmount * enemy.DamageReduct);
                    break;
                }
            }
            foreach (DamageType weakness in enemy.weaknesses)
            {
                if (weakness == damageType)
                {
                    DamageDealt = Mathf.RoundToInt(StatusAmount * enemy.DamageMult);
                    break;
                }
            }
            //PlayParticles(pos);
            enemy.currentHealth -= DamageDealt;
        }
        else if (isPlayer)
        {
            PlayerController player = target.GetComponent<PlayerController>();
            if (player.isShielded) return;
            if (isStatusEffect)
            {
                player.statusEffects.Add(new StatusEffectContainer(damageType, StatusAmount, isPerishable, turnsActive, particles));
                return;
            }
            //PlayParticles(pos);
            player.currentHealth -= StatusAmount;
        }
    }
}