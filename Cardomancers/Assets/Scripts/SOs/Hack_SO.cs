using UnityEngine;

[CreateAssetMenu(fileName = "Hack_SO", menuName = "Scriptable Objects/Hack_SO")]
public class Hack_SO : ScriptableObject
{
     //The base sell value for this card if the player sells it to a shopkeeper.
   public int sellValue;
    // The base price for this card if the player buys it from the black market.
   public int Price;
   public string displayName; //The name of the Hack as it appears to the player
   public string description; //The flavor text for the description

    public Sprite image; // sprite to be displayed when the hack is instanced
    public enum Layer
    {
      TOP,
      BOTTOM
    }
    public Layer sideOfCard;
    public BattleEffect[] hackEffects; // Needs battle effect to be done first
    public ParticleSystem particleSystem; // Used by damage scripts to play effect upon hit


}
