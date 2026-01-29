using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;


// The physical cards that appear when in combat or inventory
public class Card : PlayItem
{

    private Card_SO cardSO;

    public InventoryCard inventoryCard; // reference to it's own inventory card
    public Card_SO CardSO
    {
        get {return cardSO;}
        set
        {
            cardSO = value;
            CardSprite = cardSO.sprite;
            cardNameDisplay.text = cardSO.displayName;
        }
    }

    
    public List<Hack_SO> hacks;

    public int maxHacks; //Int containing the maximum number of hacks that can be applied to this card.

    [SerializeField] public TextMeshProUGUI cardNameDisplay; // displays the name of the card

    [SerializeField] public SpriteRenderer cardSpriteRenderer; // set in editor
    [SerializeField] public SpriteRenderer damageSpriteRenderer; // set in editor

    [SerializeField] public SpriteRenderer battleEffectSpriteRenderer; // set in editor

    #region Sprites
    private Sprite cardSprite; 
    public Sprite CardSprite
    {
        get{return cardSprite;}
        set // when CardSprite is changed, also change it in the spriteRenderer
        {
            cardSprite = value;
            cardSpriteRenderer.sprite = value;
        }
    }

    private Sprite damageTypeSprite; // sprite for displaying the DamageType of the card

    public Sprite DamageTypeSprite
    {
        get{return damageTypeSprite;}
        set // when CardSprite is changed, also change it in the spriteRenderer
        {
            damageTypeSprite = value;
            damageSpriteRenderer.sprite = value;
        }
    }
    private Sprite battleEffectSprite; // sprite for displaying the type of BattleEffect the card is (ex. single hit, DOT)
    public Sprite BattleEffectSprite
    {
        get{return battleEffectSprite;}
        set // when CardSprite is changed, also change it in the spriteRenderer
        {
            battleEffectSprite = value;
            battleEffectSpriteRenderer.sprite = value;
        }
    }
    #endregion

    public Animator animator; // set in editor

    // Once we have the animated cards, we can use the Animator field to start it's animation

    void Start()
    {
        position = transform.position;
        CardSprite = cardSO.sprite;
        cardNameDisplay.text = cardSO.displayName;
    }

    public void TryPlayCard(Enemy target)
    {
        //Try to play the card on the target enemy
        BattleEffect[] effects = cardSO.cardEffects;
        foreach (BattleEffect effect in effects)
        {
            //Apply each effect to the target
            effect.TriggerEffect(target.gameObject, target.gameObject.transform.position);
        }
    }


}
