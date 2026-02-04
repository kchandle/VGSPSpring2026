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

   // public BattleEffect[] hackEffects; //The list of effects that occur when this hack is played

}
