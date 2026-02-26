using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class KeybindSwapper : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameStateScript.GameState state = GameStateScript.CurrentState;
        GameStateScript.OnGameStateChanged += SwitchActiveBindTutorial;
    }

    void SwitchActiveBindTutorial(GameStateScript.GameState state)
    {
       
    }
}
