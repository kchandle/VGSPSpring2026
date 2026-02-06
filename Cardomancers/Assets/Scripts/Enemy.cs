using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Enemy_SO enemySO;
    // InventoryCard[] deck: Deck of the enemy. Copy from enemySO on instantiation
    public List<Card_SO> hand = new List<Card_SO>();
    public int maxHealth; //Max health of the enemy.
    public int currentHealth; //  MaxHealth by default
    public bool isStunned; // f the enemy is stunned, they cannot take actions.

    public List<StatusEffectContainer> statusEffects = new List<StatusEffectContainer>();

    public List<DamageType> resistances;
    public List<DamageType> weaknesses;

    [SerializeField] private Animator animator;   //Animator for the enemy’s sprites.

    public List<InventoryCard> deck;
    public Image healthBar; // Reference to the health bar UI element

    public float DamageMult = 2.0f; // Multiplier for damage if weakness is present
    public float DamageReduct = 0.5f; // Multiplier for damage if resistance is present

    public bool isShielded = false; //If the enemy is shielded, they take no damage this turn.

    public Enemy_SO EnemySO { get { return enemySO; } set { enemySO = EnemySO; } }

    
    // different sprites needed and change depending on state
    public Image UIimage;
    public Sprite IdleSprite;
    public Sprite DamagedSprite;
    public Sprite AttackedSprite;
    public Sprite StunnedSprite;
    public Sprite DefeatedSprite;

//the different enemy states
    public enum EnemyState
    {
        Idle = 1,
        Damaged = 2,
        Attack = 3,
        Stunned = 4,
        Defeated = 5,
    }

// variable for enum switch state
    bool currentValue;
    public EnemyState State = EnemyState.Idle;


    //Changed Awake to a seperate function in order to set enemySO in the battlemanager
    public void SetUp(Enemy_SO enemy_SO)
    {
        // sets Max Health from the SO and sets the current health to max health
        enemySO = enemy_SO;
        maxHealth = enemySO.maxHealth;
        currentHealth = maxHealth;
        deck = new List<InventoryCard>(enemySO.deck);
        resistances = new List<DamageType>(enemySO.resistances);
        weaknesses = new List<DamageType>(enemySO.weaknesses);

        animator = GetComponent<Animator>();
    }
    
    //enemy state enum changes here 
   public void EnemyAnimatorState()
    {
        switch (State)
        {
            case EnemyState.Idle:
               // animator.SetBool("Idle", true);
                UIimage.sprite = IdleSprite;
                break;

            case EnemyState.Defeated:
                //animator.SetBool("Defeated", true);
                UIimage.sprite = DefeatedSprite;
                break;

            case EnemyState.Stunned:
                //animator.SetBool("Stunned", true);
                UIimage.sprite = StunnedSprite;
                break;

            case EnemyState.Attack:
                //animator.SetBool("Attack", true);
                Debug.Log("Changing sprite to AttackedSprite");
                UIimage.sprite = AttackedSprite;
                break;

            case EnemyState.Damaged:
               // animator.SetBool("Damaged", true);
                UIimage.sprite = DamagedSprite;
                break;
        }
    }

    void Awake()
    {
        UIimage = transform.GetChild(1).GetComponent<Image>();

        if (UIimage == null)
        {
            Debug.LogError("UIimage is missing! Please attach an Image component.");
        }
    }

    // void Start()
    // {
    //     // spriteRenderer = GetComponent<SpriteRenderer>();
    //     // UIimage = GetComponent<Image>();
    //     State = EnemyState.Attack;
    //     EnemyAnimatorState();
    // }

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
    }

    public IEnumerator StatusEffects()
    {
        foreach (StatusEffectContainer statusEffect in statusEffects)
        {
            // Apply the status effect to the player
            foreach (ParticleSystem particle in (statusEffect.particles))
            {
                Instantiate(particle, transform.position, Quaternion.identity);
            }

            if (weaknesses.Contains(statusEffect.damageType) )
            {
                currentHealth -= Mathf.FloorToInt(statusEffect.statusAmount*DamageMult);  
            }
            else if (resistances.Contains(statusEffect.damageType))
            {
                currentHealth -= Mathf.FloorToInt(statusEffect.statusAmount * DamageReduct);
            }

            UpdateHealthBar();

            // Decrement the turn count for perishable effects
            if (statusEffect.DecrementTurn() <= 0)
            {
                // Remove the status effect if it has expired
                if (statusEffect.damageType == DamageType.Stun) isStunned = false;
                statusEffects.Remove(statusEffect);
                Debug.Log("A status effect has expired.");
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

}
