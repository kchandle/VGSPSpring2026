using UnityEngine;
using static GameStateScript;

public class switchState : MonoBehaviour
{
    

    public void OnExit()
    {
        switch(GameStateScript.CurrentState)
        {
            case GameStateScript.GameState.WALKING:
                GameStateScript.CurrentState = GameStateScript.GameState.INVENTORY;
                break;
            case GameStateScript.GameState.INVENTORY:
                GameStateScript.CurrentState = GameStateScript.GameState.WALKING;
                break;

        }
    }
    
}
