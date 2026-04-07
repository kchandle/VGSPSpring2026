using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

[ System.Serializable ]
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

public enum BattleActionType
{
    ATTACK,
    DEFEND,
    HEAL,
    REST_ENEMY_ONLY,
    OTHER
}
public enum damageType
{
    damageOverTime,
    damageInstant,
    healOverTime,
    healInstant,
    
}

[ System.Serializable ]
public struct BattleEffect 
{
    #region VARIABLES
    // The amount of damage/heal/stun/whatever the BattleEffect inflicts on the target
    public int StatusAmount;
    //The damage type of the BattleEffect (used for determining weakness/resistance in enemies/player)
    public DamageType damageType;
    public BattleActionType actionType;

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

    public BattleEffect(int statusAmount, DamageType damageType, bool isStatusEffect, bool isPerishable, int turnsActive, ParticleSystem[] particles, BattleActionType actionType)
    {
        this.StatusAmount = statusAmount;
        this.damageType = damageType;
        this.isStatusEffect = isStatusEffect;
        this.isPerishable = isPerishable;
        this.turnsActive = turnsActive;
        this.particles = particles;
        this.actionType = actionType;
    }
    #endregion

    #region TriggerEffects
    //The function that is called when the card is played: Change into Overload function for player and enemy respectively
    public bool TriggerEffect(PlayerController target, Vector3 pos, Card_SO card = null)
    {
        PlayerController player = target.GetComponent<PlayerController>();
        if(card != null)
        {
            if(card.CardType != CardType.DEF)
            {
                return false;
            }
        }

        switch (actionType)
        {
            case (BattleActionType.ATTACK):
            {
                int dmgToDo = StatusAmount;
                int shieldAmount = player.Shield;
                if (isStatusEffect)
                {
                    player.statusEffects.Add(new StatusEffectContainer(damageType, StatusAmount, isPerishable, turnsActive, particles, actionType));
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
            case (BattleActionType.DEFEND):
            {
                player.Shield += StatusAmount;
                return true;
                break;
            }
            case (BattleActionType.HEAL):
            {
                player.currentHealth += StatusAmount;
                return true;
            }
            default:
            {
                return false;
            }
        }
        return false;
    }

    public bool TriggerEffect(Enemy target, Vector3 pos, Card_SO card = null)
    {
        if(card)
        {
            if(card.CardType == CardType.DEF)
            {
                Debug.Log("a card with only def played on enemy");
                return false;
            }
        }
        
        Enemy enemy = target.GetComponent<Enemy>();
        switch (actionType)
        {
            case (BattleActionType.ATTACK):
            {
                int DamageDealt = StatusAmount;
                if (isStatusEffect)
                {
                    enemy.statusEffects.Add(new StatusEffectContainer(damageType, StatusAmount, isPerishable, turnsActive, particles, actionType));
            
                    if (damageType == DamageType.Stun) 
                    {

                        target.isStunned = true;
                    }
                    // Causes stun to happen
                    enemy.UpdateHealthBar();
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
                break;
            }
            case (BattleActionType.HEAL):
            {
                enemy.currentHealth += StatusAmount;
                enemy.UpdateHealthBar();
                break;
            }
            case (BattleActionType.DEFEND):
            {
                enemy.CurrentShield += StatusAmount;
                break;
            }
        }
        if (enemy.currentHealth <= 0)
        {
            //Stops the player from interacting with the enemy once dead
            enemy.gameObject.GetComponentInChildren<Image>().enabled = false;
            enemy.gameObject.GetComponentInChildren<BoxCollider2D>().enabled = false;
            enemy.Death();
            enemy.gameObject.SetActive(false);
        }
        return true;
    }
    #endregion

}