using UnityEngine;

// Ensure the correct namespace or assembly reference for BattleSystem is included  
// Example: using YourNamespace;  

public class StartBattle : MonoBehaviour
{
    public Battle_SO battleToStart;
    // The only reason this exists is to test the battle system quickly  
    public void StartBattleNow()
    {
        // Updated to use the recommended method for finding objects  
        var battleSystem = Object.FindFirstObjectByType<BattleManager>();
        if (battleSystem != null)
        {
            battleSystem.StartBattle(battleToStart);
        }
        else
        {
            Debug.LogError("BattleSystem not found in the scene.");
        }
    }
}
