using UnityEditor;
using UnityEngine;

// Ensure the correct namespace or assembly reference for BattleSystem is included  
// Example: using YourNamespace;  

public class StartBattle : MonoBehaviour
{
    public Battle_SO battleToStart;
    public GameObject battleManagerPrefab; // Assign the BattleManager prefab in the inspector
    // The only reason this exists is to test the battle system quickly  
    public void StartBattleNow()
    {
        // Updated to use the recommended method for finding objects
         // Ensure the BattleManager prefab is instantiated in the scene
        var battleSystem = Object.FindFirstObjectByType<BattleManager>();
        if (battleSystem == null)
        {
            Instantiate(battleManagerPrefab);
            battleSystem = Object.FindFirstObjectByType<BattleManager>();
        }
        battleSystem.StartBattle(battleToStart);
    }
}
