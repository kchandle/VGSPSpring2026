using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
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
    public GameObject winScreen; // the canvas displayed when the player wins
    public GameObject loseScreen; // the canvas displayed when the player loses
    #endregion

    [Tooltip("The current battle Scriptable Object, will be set by the object that calls on the battle script, only here for visibility")]
    public Battle_SO battle; // current battle SO passed in when battlestart is called
    public BattleState battleState; // current state of the battle

    #region All the player scripts
    private GameObject player; // reference to the player game object
    private PlayerController playerController; // reference to the player controller
    public GameObject playerspacePrefab; // prefab for the player's playspace
    private Inventory playerInventory; // reference to the player's inventory
    private float playerMaxHealth; // reference to the player's max health
    private float playerCurrentHealth; // reference to the player's current health
    #endregion

    public InputActionAsset inputActions; // reference to the input system
    public CardDragInput cardDragInput; // reference to the card drag input script

    [SerializeField] private List<InventoryCard> playerDeckCopyI; // copy of the player's deck at start of battle
    [SerializeField] private List<InventoryCard> playerDeckCopy; // copy of the player's deck for shuffling and use in battle

    public List<InventoryCard> PlayerDeckCopy
    {
        get { return playerDeckCopy; }
        set { playerDeckCopy = value; }
    }

    public List<GameObject> currentEnemies; // list of current enemy game objects in the battle

    public int turnCount = 0; // counter for the number of turns taken in the battle

    public GameObject cardPrefab; // Generic prefab for the cards used in battle


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

    //Curently Switching Cameras doesn't work, but the code is here for future reference and hopefully implementation
    public void SwitchCam()
    {
        if (mainCamera.enabled)
        {
            battleCamera.enabled = true;
            mainCamera.enabled = false;
        }
        else
        {
            mainCamera.enabled = true;
            battleCamera.enabled = false;
        }
    }
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

        SwitchCam();

        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerInventory = player.GetComponent<Inventory>();
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

        //Get the player set up (not in awake cause it ran before the player Inventory was set
        playerDeckCopyI = new List<InventoryCard>(playerInventory.Deck);

        playerMaxHealth = playerController.maxPlayerHealth;
        playerCurrentHealth = playerController.currentHealth;

        //Get the enemy set up

        SetPlayspaces();

        turnCount = 0;
        battleState = BattleState.PLAYER_TURN;
        isBattling = true;
        OnBattleStart.Invoke();
        StartCoroutine(TurnManager());
        
    }

    void SetPlayspaces()
    {
        float canvasWidth = battleUI.GetComponent<RectTransform>().rect.width;
        float canvasHeight = battleUI.GetComponent<RectTransform>().rect.height;
        float enemySpacing = canvasWidth / (battle.enemies.Length);
        int i = 0;

        playerspacePrefab = Instantiate(playerspacePrefab, new Vector3((canvasWidth / 2), -(canvasHeight* 3/4), 0), Quaternion.identity);
        playerspacePrefab.transform.SetParent(battleUI.gameObject.transform, false);
        cardDragInput.AddActivePlayspace(playerspacePrefab.GetComponent<Playspace>());
        //Put Deck in player playerspace

        foreach (Enemy_SO e in battle.enemies)
        {
            GameObject enemyPrefab = e.enemyPrefab;
            enemyPrefab = Instantiate(e.enemyPrefab, new Vector3(0+ (enemySpacing*(i-1)), (canvasHeight * 1/4) , 0), Quaternion.identity);
            enemyPrefab.transform.SetParent(battleUI.gameObject.transform , false);
            enemyPrefab.GetComponent<Enemy>().SetUp(e);

            //Player playspace allowed donors
            cardDragInput.AddActivePlayspace(enemyPrefab.GetComponentInChildren<Playspace>());
            enemyPrefab.GetComponentInChildren<Playspace>().allowedDonors.Add(playerspacePrefab.GetComponent<Playspace>());
            currentEnemies.Add(enemyPrefab);
            i++;
        }


    }


    IEnumerator TurnManager()
    {
        while (battleState != BattleState.WON && battleState != BattleState.LOST) {
            yield return StartCoroutine(StartPlayerTurn()); //Wait for player to finish turn

            //Testing line to skip enemy turn every other turn
            if (true) {
                yield return StartCoroutine(StartEnemyTurn()); //Wait for enemy to finish turn
            }
            
            
            turnCount++; //Adds the turn to turn count
        }
        EndBattle();
        yield return null;
    }


    public IEnumerator StartPlayerTurn()
    {
        
        PlayerTurn.Invoke();
        //Check if player is out of cards
        if (playerDeckCopy.Count <= 0)
        {
            playerDeckCopy = playerInventory.Shuffle(new List<InventoryCard>(playerInventory.Deck));
            
            //Add NewPlayItem from playsapce for each card in deck copy
            foreach (InventoryCard card in playerDeckCopy)
            {
                UnityEngine.GameObject playerCard = playerspacePrefab.GetComponent<Playspace>().NewPlayItem(cardPrefab, card.cardSO);
                playerCard.GetComponent<Card>().inventoryCard = card;
                playerCard.GetComponent<Card>().hacks = card.hacks;
                playerCard.GetComponent<Card>().CardSO = card.cardSO;
            }
        }
        //Display cards

        // Start Player turn coroutine to handle playing cards 
        yield return StartCoroutine(cardDragInput.DragDrop());

        //Status Effects get activated
        yield return StartCoroutine(playerController.StatusEffects());

        // If player or enemy is out of health, change battleState to WON or LOST
        checkEndConditions();

        //Changes battlesstate to start enemy turn
        battleState = BattleState.ENEMY_TURN;
        yield return null;
    }

    public IEnumerator StartEnemyTurn()
    {
        //Check if enemy is out of cards
        foreach (GameObject enemy in currentEnemies)
        {
            
            if (enemy.GetComponent<Enemy>().deck.Count <= 0)
            {
                enemy.GetComponent<Enemy>().ShuffleDeck();
            }  
        }

        EnemyTurn.Invoke();

        //Enemy picks card from card list
        foreach (GameObject enemy in currentEnemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript.currentHealth <= 0) continue; //Skip turn if enemy is dead
            InventoryCard card = enemyScript.DrawCard();
            
            //Plays Card

            foreach (BattleEffect effect in card.cardSO.cardEffects)
            {
                if (enemy.GetComponent<Enemy>().isStunned) continue;
                effect.TriggerEffect(playerController, player.transform.position);
            }
        }

        //Status Effects get activated, seperate foreach to ensure all enemies get status effects applied after all cards are played
        foreach (GameObject enemy in currentEnemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            //InventoryCard card = enemyScript.DrawCard();
            yield return StartCoroutine(enemyScript.StatusEffects());
        }
        

        // If player or enemy is out of health, change battleState to WON or LOST
        checkEndConditions();
        yield return null;
    }

    public void checkEndConditions()
    {
        //If player health <= 0, battleState = BattleState.LOST
        if (playerController.currentHealth <= 0)
        {
            battleState = BattleState.LOST;
            isBattling = false;
        }
        //If all enemies health <= 0, battleState = BattleState.WON

        //Loops through list of all active enemies to check if their health is <= 0
        //loop through all enemies
        bool allDead = true;
        foreach (GameObject e in currentEnemies)
        {
            //print("Enemy: "+e.GetComponent<Enemy>().currentHealth);
            if (!(e.GetComponent<Enemy>().currentHealth <= 0))
            {
                allDead = false;
            }
        }
        if (allDead)
        {
            battleState = BattleState.WON;
            isBattling = false;
        }


    }

    public void EndBattle()
    {
        // Depending on battleState, invoke win or lose events
        // Potential bug where battlecam gets disabled before win/lose screen shows up
        switch (battleState)
        {
            case BattleState.WON:
                OnWin.Invoke();
                // Display win screen
                winScreen.SetActive(true);

                //Start win coroutine
                //Get rewards from SO and display

                break;
            case BattleState.LOST:
                OnLose.Invoke();
                // Display lose screen
                loseScreen.SetActive(true);

                //start loss coroutine
                //If player lost, return to last checkpoint or main menu

                break;
        }

        //Stops the turn manager coroutine
        StopCoroutine(TurnManager());

        //Sets player back to defualt state
        playerController.statusEffects.Clear();
        playerController.currentHealth = playerMaxHealth;

        //Switches Camera back to Main camera
        SwitchCam();

    }
}
