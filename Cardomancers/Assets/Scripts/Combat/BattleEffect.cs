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
    public bool TriggerEffect(PlayerController target, Vector3 pos, Card_SO card = null)
    {
        PlayerController player = target.GetComponent<PlayerController>();
        if(card)
        {
            if(card.type == "DEF")
            {
                player.Shield += StatusAmount;
                return true;
            }
            return false;
        }


        int dmgToDo = StatusAmount;
        int shieldAmount = player.Shield;
        if (isStatusEffect)
        {
            player.statusEffects.Add(new StatusEffectContainer(damageType, StatusAmount, isPerishable, turnsActive, particles));
            return true;
        }
        if (shieldAmount > 0)
        {
            player.Shield -= dmgToDo;
            dmgToDo -= shieldAmount;
        }
        if(dmgToDo > 0)
            player.currentHealth -= dmgToDo;
        //PlayParticles(pos);
        player.UpdateHealthbar();
        return true;
    }

    public bool TriggerEffect(Enemy target, Vector3 pos, Card_SO card = null)
    {
        if(card)
        {
            if(card.type == "DEF")
            {
                Debug.Log("def played on enemy");
                return false;
            }
        }
        int DamageDealt = StatusAmount;
        Enemy enemy = target.GetComponent<Enemy>();
        
        

        if (isStatusEffect)
        {
            enemy.statusEffects.Add(new StatusEffectContainer(damageType, StatusAmount, isPerishable, turnsActive, particles));
            
            if (damageType == DamageType.Stun) 
            {

                target.isStunned = true;
            }
            // Causes stun to happen

            return true;
        }

        int dmgToDeal = DamageDealt;
        int shieldAmount = enemy.CurrentShield;
        if (shieldAmount > 0)
        {
            enemy.CurrentShield -= dmgToDeal;
            DamageDealt -= shieldAmount;
            dmgToDeal -= shieldAmount;
        }
        foreach (DamageType resistance in enemy.resistances)
        {
            if (resistance == damageType)
            {
                DamageDealt = Mathf.RoundToInt(dmgToDeal * enemy.DamageReduct);
                break;
            }
        }
        foreach (DamageType weakness in enemy.weaknesses)
        {
            if (weakness == damageType)
            {
                DamageDealt = Mathf.RoundToInt(dmgToDeal * enemy.DamageMult);
                break;
            }
        }
        //PlayParticles(pos);
        if (DamageDealt > 0)
            enemy.currentHealth -= DamageDealt;

        enemy.UpdateHealthBar();
        if (enemy.currentHealth <= 0)
        {
            //Stops the player from interacting with the enemy once dead
            enemy.gameObject.GetComponentInChildren<Image>().enabled = false;
            enemy.gameObject.GetComponentInChildren<BoxCollider2D>().enabled = false;
            enemy.gameObject.SetActive(false);
        }
        return true;

    }
}