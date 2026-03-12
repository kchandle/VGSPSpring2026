using UnityEngine;
using UnityEngine.UI;

public enum cardType
{
    ATK,
    DEF,
    RST
}

[SerializeField]
public enum damageType
{
    damageOverTime,
    damageInstant,
    healOverTime,
    healInstant,
    
}

[CreateAssetMenu(fileName = "Card_SO", menuName = "Scriptable Objects/Card_SO")]
public class Card_SO : ScriptableObject
{
    public int sellValue; // Price to SELL (Lower than price)
    public int price; // Price to BUY (Higher than sell value)

    public int energyCost; // amount of energy it takes from the user

    [Tooltip("The type of action. Currently can be ATK, DEF, or RST.")]
    public string type; // TODO: In scripts that reference this, change the reference to this to a reference to the cardType enum
    public Sprite image; // sprite to be displayed when the card is instanced
    public BattleEffect[] cardEffects; // Needs battle effect to be done first
    public ParticleSystem particleSystem; // Used by damage scripts to play effect upon hit
    
    public string displayName; // Card's name
    public string description; // Description of what the card does
    public string tagLine; // short tag line
    public cardType cardType;
    public damageType damageType;
}
