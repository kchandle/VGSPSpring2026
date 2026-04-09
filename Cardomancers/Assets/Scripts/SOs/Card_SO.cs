using UnityEngine;
using UnityEngine.UI;

public enum CardType
{
    ATK,
    DEF,
    RST,
    HEAL,
    NULL
}

[SerializeField]


[CreateAssetMenu(fileName = "Card_SO", menuName = "Scriptable Objects/Card_SO")]
public class Card_SO : ScriptableObject
{
    public int sellValue; // Price to SELL (Lower than price)
    public int price; // Price to BUY (Higher than sell value)

    public int energyCost; // amount of energy it takes from the user (enemies only)

    [Tooltip("The type of action. Currently can be ATK, DEF, or RST.")]
    public Sprite image; // sprite to be displayed when the card is instanced
    public BattleEffect[] cardEffects; // Needs battle effect to be done first
    public ParticleSystem particleSystem; // Used by damage scripts to play effect upon hit
    
    public string displayName; // Card's name
    public string description; // Description of what the card does
    public string tagLine; // short tag line
    [SerializeField] private CardType cardType =  CardType.NULL;

    public CardType CardType
    {
        get
        {
            if (cardType == CardType.NULL)
            {
                foreach (BattleEffect effect in cardEffects)
                {
                    if (effect.actionType == BattleActionType.ATTACK) 
                    {
                        cardType = CardType.ATK;
                        return cardType;
                    }
                }
                foreach (BattleEffect effect in cardEffects)
                {
                    if (effect.actionType == BattleActionType.HEAL) 
                    {
                        cardType = CardType.HEAL;
                        return cardType;
                    }
                }
                foreach (BattleEffect effect in cardEffects)
                {
                    if (effect.actionType == BattleActionType.DEFEND) 
                    {
                        cardType = CardType.DEF;
                        return cardType;
                    }
                }
                foreach (BattleEffect effect in cardEffects)
                {
                    if (effect.actionType == BattleActionType.REST_ENEMY_ONLY) 
                    {
                        cardType = CardType.RST;
                        return cardType;
                    }
                }
            }
            return cardType;
        }
        set { cardType = value; }
    }
    public damageType damageType;
    //***public DamageType damageType;
}
