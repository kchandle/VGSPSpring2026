using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Enemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Enemy_SO enemySO;
    // InventoryCard[] deck: Deck of the enemy. Copy from enemySO on instantiation
    public List<Card_SO> hand = new List<Card_SO>();
    public int maxHealth; //Max health of the enemy.
    public int currentHealth; //  MaxHealth by default
    public bool isStunned; // f the enemy is stunned, they cannot take actions.
    public int currentTimer;
    public int currentMana = 5;
    private int currentShield = 0;
    public GameObject shieldPanel;
    public TMP_Text shieldText;

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

    public float DamageMult = 2.0f; // Multiplier for damage if weakness is present
    public float DamageReduct = 0.5f; // Multiplier for damage if resistance is present

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
        currentMana = 5;
        deck = new List<InventoryCard>(enemySO.deck);
        resistances = new List<DamageType>(enemySO.resistances);
        weaknesses = new List<DamageType>(enemySO.weaknesses);

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
        deck.Remove(card);
        return card;
    }

    public void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = (float)currentHealth / maxHealth;
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
        print(currentActionType);

        currentActionAmount = currentCard.cardSO.cardEffects[0].StatusAmount;
        if (currentActionType != CardType.RST)
        {
            actionAmountText.text = "" + currentActionAmount;
        }
        else
        {
            actionAmountText.text = "";
        }
        print(currentActionAmount);
    }

    public void UpdateTimer()
    {
        timerText.text = "" + currentTimer;
        //float rotateTime = 2f;  // This should eventually rotate the timer on update.
        //float amount = -180f; // 
        //float currentTime = 0f;
        //float amountPerMillis = amount/rotateTime;
        //while (currentTime < rotateTime)
        //{
        //    print("rotating, current angle: " + timerText.transform.parent.GetChild(1).rotation.z);
        //    timerText.transform.parent.GetChild(1).Rotate(0, 0, ((amount/rotateTime) * currentTime));
        //    currentTime += Time.deltaTime;
        //}
        //timerText.transform.parent.GetChild(1).Rotate(0, 0, 180);
    }

    public IEnumerator StatusEffects()
    {
        for (int i = 0; i < statusEffects.Count; i++)
        {
            // Apply the status effect to the player
            foreach (ParticleSystem particle in (statusEffects[i].particles))
            {
                Instantiate(particle, transform.position, Quaternion.identity);
            }

            if (weaknesses.Contains(statusEffects[i].damageType) )
            {
                currentHealth -= Mathf.FloorToInt(statusEffects[i].statusAmount*DamageMult);  
            }
            else if (resistances.Contains(statusEffects[i].damageType))
            {
                currentHealth -= Mathf.FloorToInt(statusEffects[i].statusAmount * DamageReduct);
            }
            // Decrement the turn count for perishable effects
            if (statusEffects[i].DecrementTurn() <= 0)
            {
                // Remove the status effect if it has expired
                if (statusEffects[i].damageType == DamageType.Stun) isStunned = false;
                statusEffects.Remove(statusEffects[i]);
                i++;
                Debug.Log("A status effect has expired.");
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

}
