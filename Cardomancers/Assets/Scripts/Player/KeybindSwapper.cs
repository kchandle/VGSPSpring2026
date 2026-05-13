using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class KeybindSwapper : MonoBehaviour
{
    public GameObject questUI;
    public GameObject moneyUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameStateScript.GameState state = GameStateScript.CurrentState;
        GameStateScript.OnGameStateChanged += SwitchActiveBindTutorial;
    }
    // Reference to game state script

    void SwitchActiveBindTutorial(GameStateScript.GameState state)
    {
        if (state == GameStateScript.GameState.WALKING)
        {
            questUI.SetActive(true);
            moneyUI.SetActive(true);
        }
        // If other bind UI is added SET IT FALSE EXCEPT FOR WHATS SUPPOSED TO BE TRUE

        if (state == GameStateScript.GameState.INVENTORY)
        {
            questUI.SetActive(false);
            moneyUI.SetActive(false);

        }

        if (state == GameStateScript.GameState.BATTLE)
        {
            questUI.SetActive(false);
            moneyUI.SetActive(false);
        }
       // For all of these, when a specific gamestate is opened, close other keybind UI and open the necessary keybind UI
       
    }
}
