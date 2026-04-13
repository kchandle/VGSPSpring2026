using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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
    rest
}

//***
public enum StatusEffectType
{
    None,

    OnFire,
    Poisoned,
    Stun,
    Regeneration,
    CleanseNegative,
    CleanseAll,


    Other
}


[ System.Serializable ]
public struct BattleEffect 
{
    #region VARIABLES
    [Header("Basic Damage Info")]
    // The amount of damage/heal/stun/whatever the BattleEffect inflicts on the target
    public int StatusAmount;
    //The damage type of the BattleEffect (used for determining weakness/resistance in enemies/player)
    public DamageType damageType;
    public BattleActionType actionType;

    [Header("Status Effects")]
    // Status effect related variables, only used if isStatusEffect is true
    public bool isStatusEffect; // Whether this BattleEffect is a status effect
    public bool isPerishable; //If status effect is perishable
    public bool isNegative; //If the status effect is negative, and can thus be cleansed
    public int turnsActive; //Amount of turns active at start of effect   
    public StatusEffectType statusType; //***

    [Header("Enemy Summoning")]
    //Summoning variables, only used if summonsEnemies is true
    public bool summonsEnemies; //Whether or not this card summons enemies
    public Enemy_SO[] summonableEnemies; //The possible enemy types that can be summoned

    [Header("Set-Order")]
    //Variables for set-order cards
    public bool setsNextCard;
    public Card_SO nextCard;

    [Header("Field Conditions")]
    //Variables for Field Effects
    public bool setsFieldCondition;
    public FieldEffect_SO fieldCondition;

    [Header("Particle Effects")]
    //A list of particle effects to happen when the BattleEffect is played
    ParticleSystem[] particles;

    //public void PlayParticles(Vector3 pos)
    //{
    //    foreach (ParticleSystem particle in particles)
    //    {
    //        Instantiate(particle, pos, Quaternion.identity);
    //    }
    //}


    public BattleEffect(int statusAmount, DamageType damageType, bool isStatusEffect, bool isPerishable, bool isNegative, int turnsActive, StatusEffectType statusType, bool summonsEnemies, Enemy_SO[] summonableEnemies, bool setsNextCard, Card_SO nextCard, bool setsFieldCondition, FieldEffect_SO fieldCondition, ParticleSystem[] particles, BattleActionType actionType)
    {
        //Basic attributes. Applies to cards that just do damage and cards with status effects
        this.StatusAmount = statusAmount;
        this.damageType = damageType;

        //Status Effect attributes. turnsActive is also used to define how long field conditions last
        this.isStatusEffect = isStatusEffect;
        this.isPerishable = isPerishable;
        this.isNegative = isNegative;
        this.turnsActive = turnsActive;
        this.statusType = statusType;

        //These effects apply to the enemies only. Players using cards with these effects will have nothing happen.
        this.summonsEnemies = summonsEnemies;
        this.summonableEnemies = summonableEnemies;
        this.setsNextCard = setsNextCard;
        this.nextCard = nextCard;

        //Field effects / conditions (weather)
        this.setsFieldCondition = setsFieldCondition;
        this.fieldCondition = fieldCondition;

        //other
        this.particles = particles;
        this.actionType = actionType;
    }
    #endregion

    #region TriggerEffects
    //The function that is called when the card is played: Change into Overload function for player and enemy respectively
    public bool TriggerEffect(PlayerController target, Vector3 pos, Card_SO card = null)
    {
        //Debug.Log("test helooooooooo test");
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

                //***If there is a field condition active (such as rain), evaluate the effects it may have on damage.
                FieldEffect_SO condition = BattleManager.instance.fieldCondition;
                //Debug.Log("Field Condition Active: " + condition + ", " + condition.active);
                if(condition && condition.active && condition.boostsDamage)
                {
                    Debug.Log("Evaluating " + condition.name + " effects on damage to player");
                    foreach(FieldEffects effect in condition.effects)
                    {
                        //decreases / increases damage if the type of damage is affected by the field condition
                        if(System.Array.IndexOf(effect.boostedTypes, damageType) != -1)
                        {
                           // Debug.Log("Initial Damage to deal on player: " + dmgToDo);
                            dmgToDo = (int)(dmgToDo * effect.boostAmount); 
                            //Debug.Log("Weather-boosted Damage to deal on player: " + dmgToDo);
                            //Debug.Log(condition.name + " has affected damage dealt");      
                        }
                    }
                }

                //***If the played card sets a new field condition.
                if(setsFieldCondition)
                {
                    fieldCondition.active = true;
                    fieldCondition.turnsActive = turnsActive;
                    fieldCondition.turnsRemaining = turnsActive;
                    BattleManager.instance.fieldCondition = fieldCondition;
                    //Debug.Log("set field condition to: " + fieldCondition.name);       
                } 

                if (isStatusEffect)
                {
                    player.statusEffects.Add(new StatusEffectContainer(damageType, StatusAmount, isPerishable, isNegative, turnsActive, particles, actionType, statusType));
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
                if (isStatusEffect)
                {
                    player.statusEffects.Add(new StatusEffectContainer(damageType, StatusAmount, isPerishable, isNegative, turnsActive, particles, actionType, statusType));
                    return true;
                }

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
                    enemy.statusEffects.Add(new StatusEffectContainer(damageType, StatusAmount, isPerishable, isNegative, turnsActive, particles, actionType, statusType));
            
                    if (damageType == DamageType.Stun) 
                    {

                        target.isStunned = true;
                    }
                    // Causes stun to happen
                    enemy.UpdateHealthBar();
                    return true;
                }

                

                //***If there is a field condition active (such as rain), evaluate the effects it may have on damage.
                FieldEffect_SO condition = BattleManager.instance.fieldCondition;
                //Debug.Log("Field Condition Active: " + condition + ", " + condition.active);
                if(condition && condition.active)
                {
                    Debug.Log("Evaluating " + condition.name + " effects damage to enemy");
                    foreach(FieldEffects effect in condition.effects)
                    {
                        //decreases / increases damage if the type of damage is affected by the field condition
                        if(System.Array.IndexOf(effect.boostedTypes, damageType) != -1)
                        {
                            //Debug.Log("Initial damage to be dealt on enemy: " + DamageDealt);
                            DamageDealt = (int)(DamageDealt * effect.boostAmount); 
                            //Debug.Log("Weather-Boosted damage to be dealt enemy: " + DamageDealt);
                            //Debug.Log(condition.name + " has affected damage dealt");      
                        }
                    }
                }
                //Debug.Log("last check: " + DamageDealt);

                //***If the played card sets a new field condition.
                if(setsFieldCondition)
                {
                    fieldCondition.active = true;
                    fieldCondition.turnsActive = turnsActive;
                    fieldCondition.turnsRemaining = turnsActive;
                    BattleManager.instance.fieldCondition = fieldCondition;
                    //Debug.Log("set field condition to: " + fieldCondition.name);       
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