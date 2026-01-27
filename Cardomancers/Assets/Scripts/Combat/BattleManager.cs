using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static BattleManager;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance; // singleton instance

    #region Cameras
    public Camera battleCamera; // the camera used during battles
    public Camera mainCamera; // the main camera used outside of battles
    #endregion

    #region Public Events
    public UnityEvent OnBattleStart; // event triggered when a battle starts
    public UnityEvent OnLose; // event triggered when the player loses a battle
    public UnityEvent OnWin; // event triggered when the player wins a battle
    public UnityEvent PlayerTurn; // event triggered at the start of the player's turn
    public UnityEvent EnemyTurn; // event triggered at the start of the enemy's turn
    #endregion

    #region UI Elements
    public Canvas battleUI; // the canvas for battle UI elements
    public Canvas winScreen; // the canvas displayed when the player wins
    public Canvas loseScreen; // the canvas displayed when the player loses
    #endregion

    public Battle_SO battle; // current battle SO passed in when battlestart is called
    public BattleState battleState; // current state of the battle

    #region All the player scripts
    public GameObject player; // reference to the player game object
    public PlayerController playerController; // reference to the player controller
    public GameObject playerspacePrefab; // prefab for the player's playspace
    public Inventory playerInventory; // reference to the player's inventory
    public float playerMaxHealth; // reference to the player's max health
    public float playerCurrentHealth; // reference to the player's current health
    #endregion

    public InputActionAsset inputActions; // reference to the input system
    public CardDragInput cardDragInput; // reference to the card drag input script

    public List<InventoryCard> playerDeckCopyI; // copy of the player's deck at start of battle
    public List<InventoryCard> playerDeckCopy; // copy of the player's deck for shuffling and use in battle

    public List<GameObject> currentEnemies; // list of current enemy game objects in the battle


    public bool isBattling = false; // flag to indicate if a battle is currently ongoing

    public enum BattleState
    {
        START,
        PLAYER_TURN,
        ENEMY_TURN,
        WON,
        LOST
    }

    #region Setup
    private void Awake()
    {
        // Check if an instance already exists
        if (instance != null && instance != this)
        {
            // If so, destroy this new object to ensure only one instance remains
            Destroy(this.gameObject);
            return;
        }

        // Otherwise, set the instance to this object
        instance = this;

        // Optional: Keep the object alive when loading new scenes
        DontDestroyOnLoad(this.gameObject);

        battleCamera.GetComponent<Camera>();
        mainCamera.GetComponent<Camera>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerInventory = player.GetComponent<Inventory>();
        playerDeckCopyI = playerInventory.Deck;
        playerDeckCopy = playerInventory.Shuffle(playerDeckCopyI);
        playerMaxHealth = playerController.maxPlayerHealth;
        playerCurrentHealth = playerMaxHealth;

    }

    private void OnEnable()
    {
        OnBattleStart.AddListener(() => Debug.Log("Battle Started!"));
        OnLose.AddListener(() => Debug.Log("You Lose!"));
        OnWin.AddListener(() => Debug.Log("You Win!"));
        PlayerTurn.AddListener(() => Debug.Log("Player's Turn"));
        EnemyTurn.AddListener(() => Debug.Log("Enemy's Turn"));
    }
    #endregion

    //Function called by an outside force to start a battle, must pass in battle_SO
    public void StartBattle(Battle_SO battle)
    {
        // Spawn enemies based on the Battle_SO
        this.battle = battle;

        //Switches Camera to Battle camera
        mainCamera.enabled = false;
        battleCamera.enabled = true;

        //Shuffle and arrange both player and enemy decks

        SetPlayspaces();


        StartCoroutine(TurnManager());
        battleState = BattleState.PLAYER_TURN;
        isBattling = true;
    }

    void SetPlayspaces()
    {
        float canvasWidth = battleUI.GetComponent<RectTransform>().rect.width;
        float canvasHeight = battleUI.GetComponent<RectTransform>().rect.height;
        float enemySpacing = canvasWidth / (battle.enemies.Length);
        int i = 0;

        foreach (Enemy_SO e in battle.enemies)
        {
            GameObject enemyPrefab = e.enemyPrefab;
            enemyPrefab = Instantiate(e.enemyPrefab, new Vector3(0+ (enemySpacing*i), (canvasHeight * 3/4) , 0), Quaternion.identity);
            enemyPrefab.transform.SetParent(battleUI.gameObject.transform , false);
            currentEnemies.Add(enemyPrefab);
            i++;
        }

        playerspacePrefab = Instantiate(playerspacePrefab, new Vector3((canvasWidth / 2), (canvasHeight / 4), 0), Quaternion.identity);
    }


    IEnumerator TurnManager()
    {
        while (battleState != BattleState.WON || battleState != BattleState.LOST) {
            StartPlayerTurn();
            //Wait for player to finish turn
            while (battleState == BattleState.PLAYER_TURN)
            {
                yield return null;
            }
            StartEnemyTurn();
            //Wait for enemy to finish turn
            while (battleState == BattleState.ENEMY_TURN)
            {
                yield return null;
            }
        }
        EndBattle();
        yield return null;
    }


    public IEnumerable StartPlayerTurn()
    {
        OnBattleStart.Invoke();
        PlayerTurn.Invoke();
        //Check if player is out of cards
        if (playerDeckCopy.Count <= 0)
        {
            playerDeckCopy = playerInventory.Shuffle(playerDeckCopyI);
        }
        //Display cards

        // Start Player turn coroutine to handle playing cards 
        yield return cardDragInput.DragDropActive = true;

        //On card raises played event: Start iterating through status effects and apply, leaving space to add an animation as the status's iterate

        // If player or enemy is out of health, change battleState to WON or LOST
        checkEndConditions();

        //Changes battlesstate to start enemy turn
        battleState = BattleState.ENEMY_TURN;
    }

    public void StartEnemyTurn()
    {
        // Disables player drag and drop input if not already disabled
        
        //cardDragInput.DragDropActive = false;

        //Check if enemy is out of cards
        foreach (Enemy_SO enemy in battle.enemies)
        {
            //enemy.GameObject.GetComponent<Enemy>().ShuffleDeck();
        }

        //Display cards

        EnemyTurn.Invoke();

        //Enemy picks card from card list

        // If player or enemy is out of health, change battleState to WON or LOST

        //On card raises played event: battleState = BattleState.PLAYER_TURN;

    }

    public void checkEndConditions()
    {
        //If player health <= 0, battleState = BattleState.LOST
        if (playerCurrentHealth <= 0)
        {
            battleState = BattleState.LOST;
            isBattling = false;
        }
        //If all enemies health <= 0, battleState = BattleState.WON

        //Loops through list of all active enemies to check if their health is <= 0
        foreach (GameObject enemy in currentEnemies)
        {
            //loop through all enemies
            foreach (GameObject e in currentEnemies)
            {
                if (!(e.GetComponent<Enemy>().currentHealth <= 0)) 
                {
                    break;
                }
                battleState = BattleState.WON;
                isBattling = false;
            }
        }
    }

    public void EndBattle()
    {
        // Depending on battleState, invoke win or lose events
        switch (battleState)
        {
            case BattleState.WON:
                OnWin.Invoke();
                // Display win screen
                winScreen.enabled = true;

                //Start win coroutine
                //Get rewards from SO and display

                break;
            case BattleState.LOST:
                OnLose.Invoke();
                // Display lose screen
                loseScreen.enabled = true;

                //start loss coroutine
                //If player lost, return to last checkpoint or main menu

                break;
        }

        //Stops the turn manager coroutine
        StopCoroutine(TurnManager());
        
        //Switches Camera back to Main camera
        battleCamera.enabled = false;
        mainCamera.enabled = true;

        //Hides win/lose screen after battle ends
        if (loseScreen.enabled == true)
        {
            loseScreen.enabled = false;
        } else if (winScreen.enabled == true)
        {
            winScreen.enabled = false;
        }


    }
}
