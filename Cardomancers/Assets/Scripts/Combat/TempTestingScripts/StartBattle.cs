using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;



// Ensure the correct namespace or assembly reference for BattleSystem is included  
// Example: using YourNamespace;  



public class StartBattle : MonoBehaviour

{
    public float timer = 2f;
    public GameObject canvas;

    public Battle_SO battleToStart;
    public GameObject battleManagerPrefab; // Assign the BattleManager prefab in the inspector
    // The only reason this exists is to test the battle system quickly  
    public void StartBattleNow()
    {
        // canvas.SetActive(true);
        StartCoroutine(DisableTransition(timer));

        GameStateScript.CurrentState = GameStateScript.GameState.BATTLE;
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

    IEnumerator DisableTransition(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        // canvas.SetActive(false);
    }

        
    

}
