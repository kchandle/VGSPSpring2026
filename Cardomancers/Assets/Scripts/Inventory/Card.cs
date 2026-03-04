using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;


// The physical cards that appear when in combat or inventory
public class Card : PlayItem
{


    private Card_SO cardSO;

    public InventoryCard inventoryCard; // reference to it's own inventory card
    public GameObject red;
    public GameObject blue;
    public Dictionary<DamageType, Sprite> cardElementImageDictionary;
    public Dictionary<damageType, Sprite> cardAttackTypeDictionary;

    public void Awake()
    {
        cardElementImageDictionary = GameObject.Find("CardImageDictionary").GetComponent<CardImageDictionary>().cardElementImageDictionary;
        cardAttackTypeDictionary = GameObject.Find("CardImageDictionary").GetComponent<CardImageDictionary>().cardAttackTypeDictionary;
    }
    // property for the cardSO. When the cardSO is set, also change the text and images on the card to match the data in the cardSO
    public Card_SO CardSO
    {
        get { return cardSO; }
        set
        {
            cardSO = value;
            CardSprite = cardSO.image;
            cardNameDisplay.text = cardSO.displayName;
            foreach (BattleEffect battleEf in cardSO.cardEffects)
            {
                damageTypeImage.sprite = cardElementImageDictionary[battleEf.damageType];
            }
            attackTypeImage.sprite = cardAttackTypeDictionary[cardSO.damageType];
        }
    }


    public Hack_SO[] hacks; //TO BE REMOVED IN A LATER ITERATION OF THE GAME

    public int maxHacks; // Int containing the maximum number of hacks that can be applied to this card.

    [Header("UI Components")]
    [SerializeField] public TextMeshProUGUI cardNameDisplay; // displays the name of the card

    [SerializeField] public Image cardImage; // set in editor
    [SerializeField] public Image damageImage; // set in editor

    [SerializeField] public Image battleImage; // set in editor
    [SerializeField] public Image damageTypeImage;
    [SerializeField] public Image attackTypeImage;

    #region Sprites
    private Sprite cardSprite;
    public Sprite CardSprite
    {
        get { return cardSprite; }
        set // when CardSprite is changed, also change it in the UI Image
        {
            cardSprite = value;
            cardImage.sprite = value;
        }
    }

    private Sprite damageTypeSprite; // sprite for displaying the DamageType of the card

    public Sprite DamageTypeSprite
    {
        get { return damageTypeSprite; }
        set // when CardSprite is changed, also change it in the UI Image
        {
            damageTypeSprite = value;
            damageImage.sprite = value;
        }
    }
    private Sprite battleEffectSprite; // sprite for displaying the type of BattleEffect the card is (ex. single hit, DOT)
    public Sprite BattleEffectSprite
    {
        get { return battleEffectSprite; }
        set // when CardSprite is changed, also change it in the UI Image
        {
            battleEffectSprite = value;
            battleImage.sprite = value;
        }
    }
    #endregion



    void Start()
    {

        position = transform.position;
        CardSprite = cardSO.image;
        cardNameDisplay.text = cardSO.displayName;
    }

    public bool TryPlayCard(Enemy target)
    {
        bool returnVal = false;
        //Try to play the card on the target enemy
        BattleEffect[] effects = cardSO.cardEffects;
        foreach (BattleEffect effect in effects)
        {
            //Apply each effect to the target
            if (effect.TriggerEffect(target, target.gameObject.transform.position, cardSO)) returnVal = true;
        }
        return returnVal;
    }

    public bool TryPlayCard(PlayerController player)
    {
        bool returnVal = false;
        // Try to play the card on the player
        if (cardSO.type == "DEF")
        {
            print(cardSO.cardEffects[0].StatusAmount);
            player.Shield += cardSO.cardEffects[0].StatusAmount;
            returnVal = true;
        }
        else
        {
            BattleEffect[] effects = cardSO.cardEffects;
            foreach (BattleEffect effect in effects)
            {
                //Apply each effect to the target
                if (effect.TriggerEffect(player, player.gameObject.transform.position, cardSO)) returnVal = true;
            }
        }
        return returnVal;
    }


    public void AddHackToCard(Hack_SO hack)
    {
        print(hack.sideOfCard);
        switch (hack.sideOfCard)
        {
            case (Hack_SO.Layer.TOP):
                {
                    hacks[1] = hack;
                    inventoryCard.hacks[1] = hack;
                    print("hack added");
                    blue.GetComponent<RawImage>().texture = hack.image.texture;
                    print("Design on top.");
                    blue.SetActive(true);
                    break;
                }
            case (Hack_SO.Layer.BOTTOM):
                {
                    hacks[0] = hack;
                    inventoryCard.hacks[0] = hack;
                    print("hack added");
                    print(hack.image.name);
                    red.GetComponent<RawImage>().texture = hack.image.texture;
                    print("Design on bottom.");
                    red.SetActive(true);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}



