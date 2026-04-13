using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Battle_SO", menuName = "Scriptable Objects/Battle_SO")]
public class Battle_SO : ScriptableObject
{
    public Enemy_SO[] enemies; //The enemies that will be faced in this battle.
    public FieldEffect_SO fieldCondition; //*****  
    public bool isTutorial;
    
    public list<DialogueScripts.DialogueSO> dialogueSO;
}
