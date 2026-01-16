using UnityEngine;

public class GameStateScript : MonoBehaviour
{
   enum GameState
    {
        WALKING,
        BATTLE,
        SPEAKING,
        MENU,
        DEAD,
        NULL

    }

    GameState currentState = GameState.NULL;

   void FixedUpdate(){

switch(currentState)
    {
        case GameState.WALKING:
            Debug.Log("Player is walking.");
            break;
        case GameState.BATTLE:
            Debug.Log("Player is in battle.");
            break;
        case GameState.SPEAKING:
            Debug.Log("Player is speaking.");
            break;
        case GameState.MENU:
            Debug.Log("Player is in menu.");
            break;
        case GameState.DEAD:
            Debug.Log("Player is dead.");
            break;
        case GameState.NULL:
            Debug.Log("Game state is null.");
            break;
    }


    }
}
