using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Battle_SO", menuName = "Scriptable Objects/Battle_SO")]
public class Battle_SO : ScriptableObject
{
    public Enemy_SO[] enemies; //The enemies that will be faced in this battle.
    public FieldEffect_SO fieldCondition; //A field condition that will be active when entering the battle
    public bool isTutorial;
    
    public List<BattleDialogue> dialogueSOs;
}

[System.Serializable]
public struct BattleDialogue
{
    public int turnToPlay;
    public DialogueScripts.DialogueSO dialogue;

    public BattleDialogue(int ttp, DialogueScripts.DialogueSO dia)
    {
        turnToPlay = ttp;
        dialogue = dia;
    }

    public int GetTurn()
    {
        return turnToPlay;
    }
}
