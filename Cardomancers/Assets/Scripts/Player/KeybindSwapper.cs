using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class KeybindSwapper : MonoBehaviour
{
    public GameObject bindsWalking;
    public GameObject bindsBattling;
    public GameObject bindsInventory;
    public GameObject questUI;
    public GameObject moneyUI;

    void Awake()
    {
        bindsWalking.SetActive(true);
    }
    // Band aid fix, if the walking binds weren't active in editor they wouldnt show up on start, this makes it show up

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
            bindsWalking.SetActive(true);
            questUI.SetActive(true);
            bindsBattling.SetActive(false);
            bindsInventory.SetActive(false);
            moneyUI.SetActive(true);
        }
        // If other bind UI is added SET IT FALSE EXCEPT FOR WHATS SUPPOSED TO BE TRUE

        if (state == GameStateScript.GameState.INVENTORY)
        {
            bindsWalking.SetActive(false);
            questUI.SetActive(false);
            bindsBattling.SetActive(false);
            bindsInventory.SetActive(true);
            moneyUI.SetActive(false);

        }

        if (state == GameStateScript.GameState.BATTLE)
        {
            bindsWalking.SetActive(false);
            questUI.SetActive(false);
            bindsBattling.SetActive(true);
            bindsInventory.SetActive(false);
            moneyUI.SetActive(false);
        }
       // For all of these, when a specific gamestate is opened, close other keybind UI and open the necessary keybind UI
       
    }
}
