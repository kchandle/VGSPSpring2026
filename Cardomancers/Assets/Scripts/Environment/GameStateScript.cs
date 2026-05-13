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
        PAUSE,
        INVENTORY,
        DEAD,
        SHOPPING,
        LOADINGSCREEN,
        NULL
    }

    //state
    private static GameState currentState = GameState.WALKING;

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

}