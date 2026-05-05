using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Enemy : MonoBehaviour
{
    #region Variables
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Enemy_SO enemySO;
    // InventoryCard[] deck: Deck of the enemy. Copy from enemySO on instantiation
    public List<Card_SO> hand = new List<Card_SO>();
    public int maxHealth; //Max health of the enemy.
    public int currentHealth; //  MaxHealth by default
    public int maxTimer = 3;
    public int currentTimer;
    public int currentMana = 5;
    private int currentShield = 0;
    public GameObject shieldPanel;
    public TMP_Text shieldText;
    public TMP_Text healthText;

    public Animator hourglassAnim;

    public Playspace cardToPlayspace;
    public Playspace enemyPlayspace;

    #region Effects & Anims
    public GameObject AntiHeal;
    public GameObject Poison;
    public GameObject Regen;
    public GameObject Frostbite;
    public GameObject OnFire;
    public GameObject Awestruck;
    #endregion


    public int CurrentShield
    {
        get { return currentShield; }
        set
        {
            currentShield = value;
            UpdateShield();
        }
    }

    private void UpdateShield()
    {
        if (currentShield <= 0)
        {
            currentShield = 0;
            shieldPanel.SetActive(false);
        }
        else
        {
            shieldPanel.SetActive(true);
            shieldText.text = "" + CurrentShield;
        }
    }

    public int currentActionAmount;
    public CardType currentActionType;
    
    public List<StatusEffectContainer> statusEffects = new List<StatusEffectContainer>();

    public List<DamageType> resistances;
    public List<DamageType> weaknesses;

    [SerializeField] private Animator animator;   //Animator for the enemy’s sprites.
    public Animator attackAnim;


    //References to UI components
    public List<InventoryCard> deck;
    public Image healthBar; // Reference to the health bar UI element
    public TMP_Text manaText;
    public TMP_Text timerText;
    public GameObject actionTypeATK;
    public GameObject actionTypeDEF;
    public GameObject actionTypeRST;
    public TMP_Text actionAmountText;
    public InventoryCard currentCard;
    public GameObject cardPrefab;

    //
    public InventoryCard nextCard;
    public bool nextCardSet;


    public float DamageMult = 2.0f; // Multiplier for damage if weakness is present
    public float DamageReduct = 0.5f; // Multiplier for damage if resistance is present


    //---Variables to do with status effects
    [Header("Status Effect Variables")]
    public float attackMulti = 1; //Multiplier for damage dealt if the enemy has an attack boost
    public float enduranceMulti = 1; //Multipliter for damage taken if the enemy has an endurance booost

    public bool healable = true; //Whether or not enemy can be healed. 
    public bool isStunned; // if the enemy is stunned, they cannot take actions.

    public bool counterSpellActive = false; //Whether or not the player will counter the next damaging spell
    public bool cSpellTriggered = false; //Whether or not counterSpell had been triggered, used as a signal to disable counterSpellActive

    public bool weatherImmune = false; //Whether or not enemy is immune to weather
    public float fieldAtkBoost = 1f; //current attack multiplier as a result of a Field Effect
    public float fieldEndBoost = 1f; //current endurance multiplier as a result of a Field Effect
    //---

    [Header(" ")]
    public bool isShielded = false; //If the enemy is shielded, they take no damage this turn.

    public Enemy_SO EnemySO { get { return enemySO; } set { enemySO = EnemySO; } }

    public BattleManager battleManager;

    // different sprites needed and change depending on state
    public SpriteRenderer spriteRenderer;
    public Sprite IdleSprite;
    public Sprite DamagedSprite;
    public Sprite AttackedSprite;
    public Sprite StunnedSprite;
    public Sprite DefeatedSprite;

    public Image enemyImage;

    public bool deathCalled = false;

    // variable for enum switch state
    bool currentValue;
    int State = 5;

    //the different enemy states
    public enum EnemyState
    {
        Idle,
        Damaged,
        Attacked,
        Stunned,
        Defeated,
    }
    #endregion

    //Changed Awake to a seperate function in order to set enemySO in the battlemanager
    public void SetUp(Enemy_SO enemy_SO)
    {
        battleManager = transform.parent.parent.gameObject.GetComponent<BattleManager>();
        // sets Max Health from the SO and sets the current health to max health
        enemySO = enemy_SO;
        maxHealth = enemySO.maxHealth;
        currentHealth = maxHealth;
        currentTimer = Random.Range(1, 4);
        UpdateTimer();
        UpdateShield();
        UpdateHealthBar();
        currentMana = 5;
        attackAnim.runtimeAnimatorController = enemy_SO.enemyAttkAnim;
        deck = new List<InventoryCard>(enemySO.deck);
        resistances = new List<DamageType>(enemySO.resistances);
        weaknesses = new List<DamageType>(enemySO.weaknesses);
        enemyImage.sprite = enemySO.enemyImage;
        animator = GetComponent<Animator>();
    }

    public void Death()
    {
        if(!deathCalled)
        {
            battleManager.allDrops.AddRange(enemySO.drops);
            deathCalled = true;
        }
    }
    
    //enemy state enum changes here 
    void EnemyAnimatorState()
    {
        switch(State)
        {
            case 5:
            {
                currentValue = animator.GetBool("Idle");
                animator.SetBool("Idle", true);
                spriteRenderer.sprite = IdleSprite;
                break;
            }
            case 4:
            {
                currentValue = animator.GetBool("Defeated");
                animator.SetBool("Defeated", true);
                spriteRenderer.sprite = DefeatedSprite;
                break;
            }
            case 3:
            {
                currentValue = animator.GetBool("Stunned");
                animator.SetBool("Stunned", true);
                spriteRenderer.sprite = StunnedSprite;
                break;
            }
            case 2:
            {
                currentValue = animator.GetBool("Attack");
                animator.SetBool("Attack", true);
                spriteRenderer.sprite = AttackedSprite;
                break;
            }
            case 1:
            {
                currentValue = animator.GetBool("Damaged");
                animator.SetBool("Damaged", true);
                spriteRenderer.sprite = DamagedSprite;
                break;
            }
        }
    }

    public void ShuffleDeck()
    {
        // If deck has less than or equal to zero cards, shuffle the deck
        if (deck.Count <= 0)
        {
            deck = new List<InventoryCard>(enemySO.deck);
        }
    }

    //Draws a random card from deck then removes it
    public InventoryCard DrawCard()
    {
        // Pick random card from deck then remove from deck
        InventoryCard card = deck[Random.Range(0, deck.Count)];

        //If the enemy's next action has been set by a different card, force that one to be drawn
        if(nextCardSet)
        {
            card = nextCard;
            nextCardSet = false;
        }

        //If the drawn card sets the next card, set the value of the next card to be played.
        foreach(BattleEffect effect in card.cardSO.cardEffects)
        {
            if(effect.setsNextCard)
            {
                nextCard = new InventoryCard(effect.nextCard, new Hack_SO[2], 0);
                nextCardSet = true;
                break;
            }
        }
        
        if(deck.Contains(card))
        {
            deck.Remove(card);
        }
        return card;
    }

    public void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            if(healthBar.fillAmount > (float)currentHealth / maxHealth) attackAnim.SetTrigger("Hurt");
            healthBar.fillAmount = (float)currentHealth / maxHealth;
            healthText.text = currentHealth + "/" + maxHealth;
        }
        if(currentHealth <= 0)
        {
            if(!deathCalled)
            {
                Death();
                deathCalled = true;
            }
            this.gameObject.GetComponentInChildren<Image>().enabled = false;
            this.gameObject.GetComponentInChildren<BoxCollider2D>().enabled = false;
            this.gameObject.SetActive(false);
        }
    }

    public void UpdateActionState()
    {
        currentActionType = currentCard.cardSO.CardType;
        //actionTypeText.text = currentActionType;
        switch (currentActionType)
        {
            case (CardType.DEF):
            {
                actionTypeDEF.SetActive(true);
                actionTypeATK.SetActive(false);
                actionTypeRST.SetActive(false);
                break;
            }
            case (CardType.ATK):
            {
                actionTypeDEF.SetActive(false);
                actionTypeATK.SetActive(true);
                actionTypeRST.SetActive(false);
                break;
            }
            case (CardType.RST):
            {
                actionTypeDEF.SetActive(false);
                actionTypeATK.SetActive(false);
                actionTypeRST.SetActive(true);
                break;
            }
        }
        
        cardToPlayspace.DestroyPlayItem(cardToPlayspace.playItems[0]);
        cardToPlayspace.NewPlayItem(cardPrefab, currentCard.cardSO, currentCard);
        cardToPlayspace.playItems[0].draggable = false;
        //print(currentActionType);

        currentActionAmount = currentCard.cardSO.cardEffects[0].StatusAmount;
        if (currentActionType != CardType.RST)
        {
            actionAmountText.text = "" + currentActionAmount;
        }
        else
        {
            actionAmountText.text = "";
        }
        //print(currentActionAmount);
    }

    public void UpdateTimer()
    {
        timerText.text = "" + currentTimer;
        if (currentTimer == maxTimer) return;
        hourglassAnim.SetTrigger("HourglassRotate");
    }

    #region Status Effects
    public IEnumerator StatusEffects()
    {
        //Any status added to enemy should be added to playercontroller and vice versa

        //---Exceptions that need to be evaluated before other status effects (Cleanses)
        bool cleanseNeg = false;
        isStunned = false;
        attackAnim.SetBool("Stunned", false);
        healable = true;
        for (int i = 0; i < statusEffects.Count; i++)
        {
            StatusEffectContainer status = statusEffects[i];
            //print("Test: " + status.statusType);
            switch(status.statusType)
            {
                case(StatusEffectType.CleanseAll):
                {
                    print("Enemy cleansing ALL status effects");
                    statusEffects.Clear();
                    break;
                }
                case(StatusEffectType.CleanseNegative):
                {
                    cleanseNeg = true;
                    break;
                }
                case(StatusEffectType.Stun):
                {
                    isStunned = true;
                    attackAnim.SetBool("Stunned", true);
                    break;
                }
                case(StatusEffectType.EyeOfTheStorm):
                {
                    fieldAtkBoost = 1f;
                    fieldEndBoost = 1f;
                    break;
                }
                case(StatusEffectType.AntiHeal):
                {
                    AntiHeal.SetActive(statusEffects[i].turnsRemaining <= 1 ? false : true);
                    healable = false;
                    break;
                }
            }
        }

        if(cleanseNeg)
        {
            print("Cleansing NEGATIVE status effects");
            for (int i = 0; i < statusEffects.Count; i++)
            {
                StatusEffectContainer status = statusEffects[i];
                if(status.isNegative)
                {
                    statusEffects.Remove(status);
                    i--;
                    print("Enemy cleansed " + status.statusType);
                }
            }
        }
        //---



        //=====Start Loop=====//

        //
        attackMulti = 1 * fieldAtkBoost;
        enduranceMulti = 1 * fieldEndBoost;
        weatherImmune = false;
        //

        for (int i = 0; i < statusEffects.Count; i++)
        {
            //Apply the status effect to the Enemy
            StatusEffectContainer status = statusEffects[i];
            foreach (ParticleSystem particle in (status.particles))
            {
                Instantiate(particle, transform.position, Quaternion.identity);
            }

            //==Big switch statement to handle EVERY status effect==//
            switch(status.statusType)
            {
                case(StatusEffectType.None):
                {
                    print("No statusEffect. If this is printing, you accidentally triggered isStatusEffect on a card.");
                    break;
                }
                //---Stat boosts
                case(StatusEffectType.AttackBoost):
                {
                    print("Attack Boost statusEffect of " + status.statusAmount + " at index " + i);
                    //Change the attack multiplier accordingly
                    attackMulti *= ((float)status.statusAmount/100);
                    break;
                }
                case(StatusEffectType.EnduranceBoost):
                {
                    print("Endurance Boost statusEffect of " + status.statusAmount + " at index " + i);
                    //Change the endurance multiplier accordingly
                    enduranceMulti *= ((float)status.statusAmount/100);
                    break;
                }
                //---

                //---Cleanses
                case(StatusEffectType.CleanseNegative):
                {
                    print("Cleanse negative statusEffects at index: " + i);
                    //Handled above
                    break;
                }
                case(StatusEffectType.CleanseAll):
                {
                    print("Cleanse all statusEffects at index: " + i);
                    //Handled above
                    break;
                }
                //---

                //---Simple DOTs
                case(StatusEffectType.Regeneration):
                {
                        Regen.SetActive(statusEffects[i].turnsRemaining <= 1 ? false : true);
                        print("Regeneration statusEffect at index: " + i);

                    //Do heal
                    if(healable)
                    {
                        /*if( weaknesses.Contains(status.damageType) ){ currentHealth += Mathf.FloorToInt(status.statusAmount*DamageReduct);  }
                        else if (resistances.Contains(status.damageType)){ currentHealth += Mathf.FloorToInt(status.statusAmount * DamageMult); }
                        else{ currentHealth += status.statusAmount; }*/

                        currentHealth += status.statusAmount; 
                    }
                    break;
                }
                case(StatusEffectType.OnFire):
                {
                    OnFire.SetActive(statusEffects[i].turnsRemaining <= 1 ? false : true);
                    print("OnFire statusEffect at index: " + i);

                    //Do burn damage. Is Super effective if the enemy is weak to the damage type
                    if( weaknesses.Contains(status.damageType) ){ currentHealth -= Mathf.FloorToInt(status.statusAmount*DamageMult);  }
                    else if (resistances.Contains(status.damageType)){ currentHealth -= Mathf.FloorToInt(status.statusAmount * DamageReduct); }
                    else{ currentHealth -= status.statusAmount; }
                    enduranceMulti *= 0.75f;

                    break;
                }
                case(StatusEffectType.Poisoned):
                {
                        Poison.SetActive(statusEffects[i].turnsRemaining <= 1 ? false : true);
                    print("Poisoned statusEffect at index: " + i);

                    //Do poison damage. Is super effective if the enemy is weak to the damage type (poison)
                    if( weaknesses.Contains(status.damageType) ){ currentHealth -= Mathf.FloorToInt(status.statusAmount*DamageMult);  }
                    else if (resistances.Contains(status.damageType)){ currentHealth -= Mathf.FloorToInt(status.statusAmount * DamageReduct); }
                    else{ currentHealth -= status.statusAmount; }

                    break;
                }
                case(StatusEffectType.Frostbite):
                {
                        Frostbite.SetActive(statusEffects[i].turnsRemaining <= 1 ? false : true);
                    print("Frostbite statusEffect at index: " + i);
                    //Do Frostbite damage. Is super effective if the enemy is weak to ice
                    if( weaknesses.Contains(status.damageType) ){ currentHealth -= Mathf.FloorToInt(status.statusAmount * DamageMult);  }
                    else if (resistances.Contains(status.damageType)){ currentHealth -= Mathf.FloorToInt(status.statusAmount * DamageReduct); }
                    else{ currentHealth -= status.statusAmount; }
                    attackMulti *= 0.75f;
                    break;
                }
                case(StatusEffectType.Awestruck):
                {
                        Awestruck.SetActive(statusEffects[i].turnsRemaining <= 1 ? false : true);
                    print("Awestruck statusEffect at index: " + i);

                    //Do Awestruck damage
                    //DOT that only triggers while stunned
                    if(isStunned)
                    {
                        if( weaknesses.Contains(status.damageType) ){ currentHealth -= Mathf.FloorToInt(status.statusAmount * DamageMult);  }
                        else if (resistances.Contains(status.damageType)){ currentHealth -= Mathf.FloorToInt(status.statusAmount * DamageReduct); }
                        else{ currentHealth -= status.statusAmount; }
                    }

                    break;
                }
                //---

                //---More complicated
                case(StatusEffectType.Stun): //done
                {
                    print("Stun statusEffect at index: " + i);

                    //Handled in the exceptions above

                    break;
                }
                case(StatusEffectType.CounterSpell): //*
                {
                    print("CounterSpell statusEffect at index: " + i);

                    //Set counterSpellActive to true, then immedieately remove this status effect.
                    //counterSpellActive will be set to false in Card, after a spell is reflected
                    counterSpellActive = true;
                    cSpellTriggered = false;
                    statusEffects[i].turnsRemaining = -1;

                    break;
                }
                case(StatusEffectType.EyeOfTheStorm):
                {
                    print("EyeOfTheStorm statusEffect at index: " + i);

                    //When field effects act, they'll check if the target is weatherImmune first. See in BattleEffect and BattleManager
                    weatherImmune = true;

                    break;
                }
                case(StatusEffectType.AntiHeal): // done
                {
                    print("AntiHeal statusEffect at index: " + i);

                    //Handled in the exceptions above

                    break;
                }
                case(StatusEffectType.Evisceration): //done
                {
                    print("Evisceration statusEffect at index: " + i);

                    //
                    currentHealth -= 200;

                    break;
                }
                //---

                case(StatusEffectType.Random):
                {
                    print("Random statusEffect at index: " + i);
                    //
                    break;
                }
                default:
                {
                    print("If this is printing, you forgot to add " + status.statusType + " to the Enemy script");
                    break;
                }
            }
            //==End of big switch statement to handle EVERY status effect==//

            // Decrement the turn count for perishable effects
            if (statusEffects[i].DecrementTurn() <= 0)
            {
                // Remove the status effect if it has expired
                statusEffects.Remove(statusEffects[i]);
                Debug.Log("The Status Effect " + status.statusType + " has expired on an Enemy");
                i--;
            }

            UpdateHealthBar();
            yield return new WaitForSeconds(0.1f);

            //print(attackMulti);
        }
        //=====End Loop=====//

        yield return null;
    }
    #endregion

}
