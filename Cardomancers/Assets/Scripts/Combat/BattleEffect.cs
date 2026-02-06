using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;
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
Stun,

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
    public void TriggerEffect(PlayerController target, Vector3 pos)
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

    public void TriggerEffect(Enemy target, Vector3 pos)
    {
        int DamageDealt = StatusAmount;
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy.isShielded) return;
        

        if (isStatusEffect)
        {
            enemy.statusEffects.Add(new StatusEffectContainer(damageType, StatusAmount, isPerishable, turnsActive, particles));

            // Healing
            if (damageType == DamageType.Light)
            {
                StatusAmount += target.currentHealth;
            }

            // Anti-Heal
            if (damageType == DamageType.Light && target.hasAntiHeal)
            {
                // Once anti-heal is applied to the target it will last for a certain amount of turns and, if they try to use a spell that heals, it will heal 0 health
                target.hasAntiHeal = true;
                turnsActive = 5;
                turnsActive--;

                if (turnsActive >= 0) StatusAmount = 0;
                if (turnsActive <=0) target.hasAntiHeal = false;
            }

            // Causes stun to happen
            if (damageType == DamageType.Stun) 
            {
                target.isStunned = true;
            }

            // Shield, Defense, whatever you want to call it
            if (damageType == DamageType.DamageBlock)
            {
                // Applied target has ALL damage blocked for 1 turn, then the status effect perishes
                target.isShielded = true;
                turnsActive = 1;
                turnsActive--;

                if (turnsActive >= 0) StatusAmount = 0;
                if (turnsActive <= 0) target.isShielded = false;
            }
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
        if (enemy.currentHealth <= 0)
        {
            //Stops the player from interacting with the enemy once dead
            enemy.gameObject.GetComponentInChildren<Image>().enabled = false;
            enemy.gameObject.GetComponentInChildren<BoxCollider2D>().enabled = false;
        }

    }
}