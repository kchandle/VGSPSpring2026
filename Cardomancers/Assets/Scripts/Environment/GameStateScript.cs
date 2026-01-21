using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Events;

public class GameStateScript : MonoBehaviour
{
// possible states
enum GameState
{
WALKING,
BATTLE,
SPEAKING,
MENU,
DEAD,
NULL
}

//able-to-change-in-inspector-for-testing
[SerializeField] private GameState inspectorState;

//state
private static GameState currentState;

//when-changed, changes-state
private void OnValidate() {
currentState = inspectorState;
UpdateGameState(currentState);
}

//event-for-state-change
static event Action<GameState> OnGameStateChanged;

static void UpdateGameState(GameState state)
{
    // Update the current state
    currentState = state;

    switch(currentState)
    {
        case GameState.WALKING:
            // add code for walking state
            Debug.Log("Player is walking.");
            break;
        case GameState.BATTLE:
            // add code for battle state
            Debug.Log("Player is in battle.");
            break;
        case GameState.SPEAKING:
            // add code for speaking state
            Debug.Log("Player is speaking.");
            break;
        case GameState.MENU:
            // add code for menu state
            Debug.Log("Player is in menu.");
            break;
        case GameState.DEAD:
            // add code for dead state
            Debug.Log("Player is dead.");
            break;
        case GameState.NULL:
            // add code for null state
            Debug.Log("Game state is null.");
            break;
    }
    // Notify subscribers about the state change
    OnGameStateChanged?.Invoke(currentState);
}
}