using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;


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
    //Stun,
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

#region Status Effect Types
public enum StatusEffectType
{
    None,

    //---Implemented---//
    //-Stat boosts
    AttackBoost, // outgoing damage * (AttackMulti/100). Ex: an Attackboost with a statusAmount of 125 gives a 1.25x attack boost
    EnduranceBoost, // incoming damage/(EunduranceMulti/100)
    //-


    //-Statuses that cleanse other statuses. For a one time cleanse, just use a turn count of 0 or 1.
    CleanseNegative,
    CleanseAll,
    //-

    //-DOTs
    Regeneration,
    OnFire,
    Poisoned,
    Frostbite,
    Awestruck,
    //-
    //--- ---//


    //---Not Yet Implemented---//
    Stun, //done
    CounterSpell, //done
    EyeOfTheStorm, //done
    AntiHeal, //done
    Evisceration, //ignore
    //--- ---//

    
    Random, //For the aura of minor chaos hack
    Other
}
#endregion

//Not used just yettttttt
public enum TargetingType
{
    SingleTarget,
    AOETarget,
    SelfTarget, //Only to be used with attacking cards that damage the caster. Use the HEAL or DEF action type for cards with positive effects on the user



    //AOE_And_SelfTarget

    //BlastTarget, //Main and adjacent enemies with lower output
}


[ System.Serializable ]
public struct BattleEffect 
{
    #region VARIABLES
    [Header("Basic Card Info")]
    // The amount of damage/heal/damage per turn/whatever the BattleEffect inflicts on the target
    public int StatusAmount;
    //The damage type of the BattleEffect (used for determining weakness/resistance in enemies/player)
    public DamageType damageType;
    public BattleActionType actionType;
    public TargetingType targetingType;


    [Header("Status Effects")]
    // Status effect related variables, only used if isStatusEffect is true
    public bool isStatusEffect; // Whether this BattleEffect is a status effect
    public bool isPerishable; //If status effect is perishable
    public bool isNegative; //If the status effect is negative, and can thus be cleansed
    public int turnsActive; //Amount of turns active at start of effect   
    public float probability; //The chance of inflicting the status effect (0-1)
    public StatusEffectType statusType;


    [Header("Enemy Summoning (Note: For enemies only)")] 
    //Will have no effect if used by player

    //Summoning variables, only used if summonsEnemies is true
    public bool summonsEnemies; //Whether or not this card summons enemies
    public Enemy_SO[] summonableEnemies; //The possible enemy types that can be summoned


    [Header("Set-Order (Note: different effects for enemy and player)")] 
    //If a set-order card is used by an enemy in any context, the enemy's next action will be nextCard
    //If a set-order card is used by a player on an enemy, it will force the enemy's next action to be nextCard.

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


    public BattleEffect(int statusAmount, DamageType damageType, TargetingType targetingType, bool isStatusEffect, bool isPerishable, bool isNegative, int turnsActive, float probability, StatusEffectType statusType, bool summonsEnemies, Enemy_SO[] summonableEnemies, bool setsNextCard, Card_SO nextCard, bool setsFieldCondition, FieldEffect_SO fieldCondition, ParticleSystem[] particles, BattleActionType actionType)
    {
        //Basic attributes. Applies to cards that just do damage and cards with status effects
        this.StatusAmount = statusAmount;
        this.damageType = damageType;
        this.targetingType = targetingType; //doesn't do anything yet

        //Status Effect attributes. turnsActive is also used to define how long field conditions last
        this.isStatusEffect = isStatusEffect;
        this.isPerishable = isPerishable;
        this.isNegative = isNegative;
        this.turnsActive = turnsActive;
        this.probability = probability;
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



    #region TriggerEffects - Player
    //The function that is called when the card is played: Change into Overload function for player and enemy respectively
    public bool TriggerEffect(PlayerController target, Vector3 pos, Card_SO card = null, float incomingAttackBoost = 1f)
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

        //---Applying the Random status effect chooses a random status effect from the enum to hit the opponent with.
        if(isStatusEffect && statusType == StatusEffectType.Random)
        {
            StatusEffectType value = (StatusEffectType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(StatusEffectType)).Length);
            statusType = value;
            //Debug.Log(statusType);
        }
        //---


        switch (actionType)
        {
            case (BattleActionType.ATTACK):
            {
                float dmgToDo = (float)StatusAmount;
                int shieldAmount = player.Shield;

                //---Account for player stat boosts
                //Debug.Log("Player dmgToDo: " + dmgToDo);
                dmgToDo *= incomingAttackBoost;
                //Debug.Log("dmgToDo after the enemy's attack boost: " + dmgToDo);
                dmgToDo /= player.enduranceMulti;
                //Debug.Log("dmgToDo after player endurance: " + dmgToDo);
                //---


                //---Account for Field Effects on damage and application of Field Effects
                //If there is a field condition active (such as rain), it affects damage, and the target is not immune to weather
                FieldEffect_SO condition = BattleManager.instance.fieldCondition;

                if(condition && condition.active && condition.boostsTypeDamage && !player.weatherImmune)
                {
                    //Debug.Log("Evaluating " + condition.name + " effects on damage to player");
                    foreach(FieldEffects effect in condition.effects)
                    {
                        //decreases / increases damage if the type of damage is affected by the field condition
                        if(System.Array.IndexOf(effect.boostedTypes, damageType) != -1)
                        {
                           // Debug.Log("Initial Damage to deal on player: " + dmgToDo);
                            dmgToDo = (dmgToDo * effect.boostAmount); 
                            //Debug.Log("Weather-boosted Damage to deal on player: " + dmgToDo);
                            //Debug.Log(condition.name + " has affected damage dealt");      
                        }
                    }
                }

                //If the played card sets a new field condition.
                if(setsFieldCondition)
                {
                    fieldCondition.active = true;
                    fieldCondition.turnsActive = turnsActive;
                    fieldCondition.turnsRemaining = turnsActive;
                    BattleManager.instance.fieldCondition = fieldCondition;
                    //Debug.Log("set field condition to: " + fieldCondition.name);       
                } 
                //---


                //---Account for Application of status Effects
                if (isStatusEffect)
                {
                    float statusRoll = UnityEngine.Random.Range(0, 1);
                    //Debug.Log(statusRoll);

                    if(statusRoll <= probability)
                    {
                        player.statusEffects.Add(new StatusEffectContainer(damageType, Mathf.RoundToInt(dmgToDo), isPerishable, isNegative, turnsActive, particles, actionType, statusType));
                        if (statusType == StatusEffectType.Stun) 
                        {
                            //Trigger stun immediately, since status effects are normally evaluated at the end of the turn
                            target.isStunned = true;
                        }



                        return true;
                    }
                    else
                    {
                        Debug.Log("Status missed the roll to trigger on player");
                    }
                }
                //---


                //---Actually do damage to player
                int intDmgToDo = Mathf.RoundToInt(dmgToDo);

                if (shieldAmount > 0)
                {
                    player.Shield -= intDmgToDo;
                    intDmgToDo -= shieldAmount;
                }
                if(intDmgToDo > 0)
                    player.currentHealth -= intDmgToDo;
                //PlayParticles(pos);
                player.UpdateHealthbar();
                return true;
                //---
            }


            case (BattleActionType.DEFEND):
            {
                //For friendly status effects to be applied on self
                if (isStatusEffect)
                {
                    float statusRoll = UnityEngine.Random.Range(0, 1);
                    //Debug.Log(statusRoll);

                    if(statusRoll <= probability)
                    {
                        player.statusEffects.Add(new StatusEffectContainer(damageType, StatusAmount, isPerishable, isNegative, turnsActive, particles, actionType, statusType));
                        return true;
                    }
                    else
                    {
                        Debug.Log("Status missed the roll to trigger on player");
                    }
                }

                player.Shield += StatusAmount;
                return true;
                break;
            }


            case (BattleActionType.HEAL):
            {
                //For friendly status effects to be applied on self
                if (isStatusEffect)
                {
                    float statusRoll = UnityEngine.Random.Range(0, 1);
                    //Debug.Log(statusRoll);

                    if(statusRoll <= probability)
                    {
                        player.statusEffects.Add(new StatusEffectContainer(damageType, StatusAmount, isPerishable, isNegative, turnsActive, particles, actionType, statusType));
                        return true;
                    }
                    else
                    {
                        Debug.Log("Status missed the roll to trigger on player");
                    }
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
    #endregion



    #region TriggerEffects - Enemy
    public bool TriggerEffect(Enemy target, Vector3 pos, Card_SO card = null, float incomingAttackBoost = 1f)
    {
        if(card)
        {
            if(card.CardType == CardType.DEF)
            {
                Debug.Log("a card with only def played on enemy");
                return false;
            }
        }
        
        //---Applying random status effects
        if(isStatusEffect && statusType == StatusEffectType.Random)
        {
            StatusEffectType value = (StatusEffectType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(StatusEffectType)).Length);
            statusType = value;
            //Debug.Log(statusType);
        }
        //---

        
        Enemy enemy = target.GetComponent<Enemy>();
        switch (actionType)
        {
            case (BattleActionType.ATTACK):
            {
                //---If the incoming attack sets the enemy's next card
                if(setsNextCard && nextCard)
                {
                    enemy.nextCardSet = true;
                    enemy.nextCard = new InventoryCard(nextCard, new Hack_SO[2], 0);
                }
                //---



                //---Account for player stat boosts
                float DamageDealt = (float)StatusAmount;
                //Debug.Log("Enemy damage to receive: " + DamageDealt);
                DamageDealt *= incomingAttackBoost;
                //Debug.Log("dmg to recieve after the player's attack boost: " + DamageDealt);
                DamageDealt /= enemy.enduranceMulti;
                //Debug.Log("dmg to recieve after enemy endurance: " + DamageDealt);
                //---


                
                //---Account for Field Effects on damage and application of Field Effects
                //If there is a field condition active (such as rain), it affects damage, and the target isn't immune to weather effects
                FieldEffect_SO condition = BattleManager.instance.fieldCondition;

                if(condition && condition.active && condition.boostsTypeDamage && !enemy.weatherImmune)
                {
                    //Debug.Log("Evaluating " + condition.name + " effects on damage to enemy");
                    foreach(FieldEffects effect in condition.effects)
                    {
                        //decreases / increases damage if the type of damage is affected by the field condition
                        if(System.Array.IndexOf(effect.boostedTypes, damageType) != -1)
                        {
                            //Debug.Log("Initial damage to be dealt on enemy: " + DamageDealt);
                            DamageDealt = (DamageDealt * effect.boostAmount); 
                            //Debug.Log("Weather-Boosted damage to be dealt enemy: " + DamageDealt);
                            //Debug.Log(condition.name + " has affected damage dealt");      
                        }
                    }
                }

                //If the played card sets a new field condition.
                if(setsFieldCondition)
                {
                    fieldCondition.active = true;
                    fieldCondition.turnsActive = turnsActive;
                    fieldCondition.turnsRemaining = turnsActive;
                    BattleManager.instance.fieldCondition = fieldCondition;
                    //Debug.Log("set field condition to: " + fieldCondition.name);       
                } 
                //---



                //---Add Status Effects
                if (isStatusEffect)
                {
                    float statusRoll = UnityEngine.Random.Range(0f, 1f);
                    //Debug.Log(statusRoll);

                    if(statusRoll <= probability)
                    {
                        enemy.statusEffects.Add(new StatusEffectContainer(damageType, Mathf.RoundToInt(DamageDealt), isPerishable, isNegative, turnsActive, particles, actionType, statusType));
                        if (statusType == StatusEffectType.Stun) 
                        {
                            //Trigger stun immediately, since status effects are normally evaluated at the end of the turn
                            target.isStunned = true;
                            target.attackAnim.SetBool("Stunned", true);
                        }
                        enemy.UpdateHealthBar();
                    }
                    else
                    {
                        Debug.Log("Unlucky womp womp");
                    }
                    return true;
                    
                }
                //---



                //---Actually damage the thing
                float dmgToDeal = DamageDealt;
                int shieldAmount = enemy.CurrentShield;
    
                if (shieldAmount > 0)
                {
                    enemy.CurrentShield -= Mathf.RoundToInt(dmgToDeal);
                    DamageDealt -= shieldAmount;
                    dmgToDeal -= shieldAmount;
                }
                foreach (DamageType resistance in enemy.resistances)
                {
                    if (resistance == damageType)
                    {
                        DamageDealt = (dmgToDeal * enemy.DamageReduct);
                        break;
                    }
                }
                foreach (DamageType weakness in enemy.weaknesses)
                {
                    if (weakness == damageType)
                    {
                        DamageDealt = (dmgToDeal * enemy.DamageMult);
                        break;
                    }
                }
                //PlayParticles(pos);
                if (DamageDealt > 0)
                    enemy.currentHealth -= Mathf.RoundToInt(DamageDealt);
                enemy.UpdateHealthBar();
                break;
                //---
            }


            case (BattleActionType.HEAL):
            {
                //For friendly status effects to be applied on self
                if (isStatusEffect)
                {
                    float statusRoll = UnityEngine.Random.Range(0, 1);
                    //Debug.Log(statusRoll);

                    if(statusRoll <= probability)
                    {
                        enemy.statusEffects.Add(new StatusEffectContainer(damageType, StatusAmount, isPerishable, isNegative, turnsActive, particles, actionType, statusType));
                    }
                    else
                    {
                        Debug.Log("Status missed the roll to trigger on Enemy");
                    }
                    return true;
                }

                enemy.currentHealth += StatusAmount;
                enemy.UpdateHealthBar();
                break;
            }


            case (BattleActionType.DEFEND):
            {
                //For friendly status effects to be applied on self
                if (isStatusEffect)
                {
                    float statusRoll = UnityEngine.Random.Range(0, 1);
                    //Debug.Log(statusRoll);

                    if(statusRoll <= probability)
                    {
                        //Debug.Log(statusType + " WHY WHY WHY WHY");
                        enemy.statusEffects.Add(new StatusEffectContainer(damageType, StatusAmount, isPerishable, isNegative, turnsActive, particles, actionType, statusType));
                    }
                    else
                    {
                        Debug.Log("Status missed the roll to trigger on Enemy");
                    }
                    return true;
                }

                enemy.CurrentShield += StatusAmount;
                break;
            }
        }

        //---Enemy death
        if (enemy.currentHealth <= 0)
        {
            //Stops the player from interacting with the enemy once dead
            enemy.gameObject.GetComponentInChildren<Image>().enabled = false;
            enemy.gameObject.GetComponentInChildren<BoxCollider2D>().enabled = false;
            enemy.Death();
            enemy.gameObject.SetActive(false);
        }
        return true;
        //---
    }
    #endregion

}