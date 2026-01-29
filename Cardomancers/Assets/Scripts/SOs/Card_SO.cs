using UnityEngine;

[CreateAssetMenu(fileName = "Card_SO", menuName = "Scriptable Objects/Card_SO")]
public class Card_SO : ScriptableObject
{
    public int sellValue; // Price to SELL (Lower than price)
    public int price; // Price to BUY (Higher than sell value)
    public string displayName; // Card's name
    public string description; // Flavor text used for descriping the card
    
    public Sprite sprite; // sprite to be displayed when the card is instanced
    public BattleEffect[] cardEffects; // Needs battle effect to be done first
    public ParticleSystem particleSystem; // Used by damage scripts to play effect upon hit
}
