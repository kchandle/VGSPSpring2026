using UnityEngine;

[CreateAssetMenu(fileName = "Battle_SO", menuName = "Scriptable Objects/Battle_SO")]
public class Battle_SO : ScriptableObject
{
    public Enemy_SO[] enemies; //The enemies that will be faced in this battle.
}
