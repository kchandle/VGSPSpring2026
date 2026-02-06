using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Events;

public class GameStateScript
    {
    // possible states
    public enum GameState
    {
        WALKING,
        BATTLE,
        SPEAKING,
        MENU,
        INVENTORY,
        DEAD,
        NULL
    }

    //state
    private static GameState currentState;
    public static GameState CurrentState
    {
        get => currentState;
        set
        {
            if (value == currentState) return;
            currentState = value;
            OnGameStateChanged?.Invoke(currentState);
        }
    }

    //event-for-state-change
    public static event Action<GameState> OnGameStateChanged;

    //public static void UpdateGameState(GameState state)
    //{
    //    // Update the current state
    //    currentState = state;

    //    switch(currentState)
    //    {
    //        case GameState.WALKING:
    //            // add code for walking state
    //            Debug.Log("Player is walking.");
    //            break;
    //        case GameState.BATTLE:
    //            // add code for battle state
    //            Debug.Log("Player is in battle.");
    //            break;
    //        case GameState.SPEAKING:
    //            // add code for speaking state
    //            Debug.Log("Player is speaking.");
    //            break;
    //        case GameState.MENU:
    //            // add code for menu state
    //            Debug.Log("Player is in menu.");
    //            break;
    //        case GameState.DEAD:
    //            // add code for dead state
    //            Debug.Log("Player is dead.");
    //            break;
    //        case GameState.NULL:
    //            // add code for null state
    //            Debug.Log("Game state is null.");
    //            break;
    //    }
    //    // Notify subscribers about the state change
    //    OnGameStateChanged?.Invoke(currentState);
    //}
}