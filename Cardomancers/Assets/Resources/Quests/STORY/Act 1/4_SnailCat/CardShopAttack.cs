using UnityEngine;

public class CardShopAttack : QuestStep
{
    private void Awake()
    {
        // make it so the card show door teleports the player to a different card shop where the reginald intimidation scene happens
        // temporarily remove any battles the player might be able to enter into in the world
        BattleManager.instance.OnEnd.AddListener(this.FinishQuestStep);
    }
    
    protected override void SetQuestStepState(string state)
    {
        throw new System.NotImplementedException();
    }

    public override string GetQuestStepState()
    {
        return "Return to the Card Shop with Thelma";
    }
}
