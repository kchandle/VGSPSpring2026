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
        LOADINGSCREEN,
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

}