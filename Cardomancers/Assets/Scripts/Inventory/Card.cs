using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Serialization;


// The physical cards that appear when in combat or inventory
public class Card : PlayItem
{

    private Card_SO cardSO;

    public InventoryCard inventoryCard; // reference to its own inventory card
    public GameObject backHack;
    public GameObject frontHack;
    // property for the cardSO. When the cardSO is set, also change the text and images on the card to match the data in the cardSO
    public Card_SO CardSO
    {
        get {return cardSO;}
        set
        {
            cardSO = value;
            CardSprite = cardSO.image;
            cardNameDisplay.text = cardSO.displayName;
        }
    }
    
    public Hack_SO[] hacks; //The array, max length 2, of hacks on the card.

    public int maxHacks = 2; // Int containing the maximum number of hacks that can be applied to this card.

    [Header("UI Components")]
    [SerializeField] public TextMeshProUGUI cardNameDisplay; // displays the name of the card

    [SerializeField] public Image cardImage; // set in editor
    [SerializeField] public Image damageImage; // set in editor

    [SerializeField] public Image battleImage; // set in editor

    #region Sprites
    private Sprite cardSprite; 
    public Sprite CardSprite
    {
        get{return cardSprite;}
        set // when CardSprite is changed, also change it in the UI Image
        {
            cardSprite = value;
            cardImage.sprite = value;
        }
    }

    private Sprite damageTypeSprite; // sprite for displaying the DamageType of the card

    public Sprite DamageTypeSprite
    {
        get{return damageTypeSprite;}
        set // when CardSprite is changed, also change it in the UI Image
        {
            damageTypeSprite = value;
            damageImage.sprite = value;
        }
    }
    private Sprite battleEffectSprite; // sprite for displaying the type of BattleEffect the card is (ex. single hit, DOT)
    public Sprite BattleEffectSprite
    {
        get{return battleEffectSprite;}
        set // when CardSprite is changed, also change it in the UI Image
        {
            battleEffectSprite = value;
            battleImage.sprite = value;
        }
    }
    #endregion



    void Start()
    {
        cardSO = inventoryCard.cardSO;
        if (hacks == null)
        {
            hacks = new Hack_SO[maxHacks];
        }
        position = transform.position;
        CardSprite = cardSO.image;
        cardNameDisplay.text = cardSO.displayName;
        frontHack = transform.GetChild(2).gameObject;
        backHack = transform.GetChild(0).gameObject;
        if (hacks.Length > 0 && hacks[0])
        {
            backHack.GetComponent<RawImage>().texture = hacks[0].image.texture;
            backHack.SetActive(true);
        }

        if (hacks.Length > 1 && hacks[1])
        {
            frontHack.GetComponent<RawImage>().texture = hacks[1].image.texture;
            frontHack.SetActive(true);
        }
    }

    public bool TryPlayCard(Enemy enemy)
    {
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        bool returnVal = false;
        //Try to play the card on the target enemy
        List<BattleEffect> effects = cardSO.cardEffects.ToList();
        foreach (Hack_SO hack in hacks)
        {
            if(hack) effects.AddRange(hack.hackEffects.ToList());
        }

        /*bool reflected = false; //track if the enemy countered a spell
        foreach (BattleEffect effect in effects)
        {
            if(effect.actionType != BattleActionType.ATTACK){if(effect.TriggerEffect(player, player.gameObject.transform.position)) {returnVal = true;continue;}} 
            //Apply each effect to the target

            if(effect.targetingType == TargetingType.SingleTarget)
            {
                if(enemy.counterSpellActive) //If the enemy has counterSpell, you take damage instead
                {
                    if(effect.TriggerEffect(player, player.gameObject.transform.position, null, player.attackMulti)){returnVal = true;}
                    reflected = true;
                }
                else{if(effect.TriggerEffect(enemy, enemy.gameObject.transform.position, cardSO, player.attackMulti)){returnVal = true;}}
            }
        }
        //If it was triggered, disable the enemy's counterspell
        if(reflected){enemy.counterSpellActive = false;}*/


        //If there are no singleTarget / AOE / Self targeting attacking effects, the corresponding methods won't do anything.
        //Ask Joshua if you have any concerns
        if(cardSO.CardType == CardType.ATK)
        {
            BattleManager.instance.PlayerAttackOneEnemy(effects, enemy, cardSO);
            BattleManager.instance.PlayerAttackAllEnemies(effects, cardSO);
            BattleManager.instance.PlayerAttackSelf(effects, cardSO);
            returnVal = true;

            SoundEffectManager.Instance.PlaySoundFXClip(cardSO.cardSound, player.transform, .65f);
        }

        return returnVal;
    }

    public bool TryPlayCard(PlayerController player)
    {
        bool returnVal = false;
        // Try to play the card on the player
        BattleEffect[] effects = cardSO.cardEffects;
        foreach (Hack_SO hack in hacks)
        {
            if(hack) effects.AddRange(hack.hackEffects.ToList());
        }
        foreach (BattleEffect effect in effects)
        {
            if(cardSO.CardType != CardType.ATK)
            {
                if(effect.TriggerEffect(player, player.gameObject.transform.position, cardSO))
                { 
                    returnVal = true;
                    //print("card played on self!");
                    continue;
                }
            } 
            //Apply each effect to the target
            if(effect.TriggerEffect(player, player.gameObject.transform.position, cardSO))
            {
                returnVal = true;
            }
        }

        if (returnVal) SoundEffectManager.Instance.PlaySoundFXClip(cardSO.cardSound, player.transform);

        //print("Playing card on self: " + returnVal);
        return returnVal;
    }


    public void AddHackToCard(Hack_SO hack)
    {
        print(hack.sideOfCard);
        switch (hack.sideOfCard)
        {
            case(Hack_SO.Layer.TOP):
            {
                hacks[1] = hack;
                inventoryCard.hacks[1] = hack;
                print("hack added");
                frontHack.GetComponent<Image>().sprite = hack.image;
                print("Design on top.");
                frontHack.SetActive(true);
                break;
            }
            case(Hack_SO.Layer.BOTTOM):
            {
                hacks[0] = hack;
                inventoryCard.hacks[0] = hack;
                print("hack added");
                print(hack.image.name);
                backHack.GetComponent<Image>().sprite = hack.image;
                print("Design on bottom.");
                backHack.SetActive(true);
                break;
            }
            default:
            {
                break;
            }
        }

        inventoryCard = new InventoryCard(cardSO, hacks, maxHacks);
        //inventoryCard.hacks.Add(hack);
    }
    }



