using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using static BattleManager;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance; // singleton instance

    #region Cameras
    public Camera battleCamera; // the camera used during battles
    public Camera mainCamera; // the main camera used outside of battles
    #endregion

    #region Drops
    public List<Drop> allDrops;
    public List<Drop> chanceDrops;
    #endregion

    #region Public Events 
    //May be used, will implement later?
    public UnityEvent OnBattleStart; // event triggered when a battle starts
    public UnityEvent OnLose; // event triggered when the player loses a battle
    public UnityEvent OnWin; // event triggered when the player wins a battle
    public UnityEvent PlayerTurn; // event triggered at the start of the player's turn
    public UnityEvent EnemyTurn; // event triggered at the start of the enemy's turn
    public UnityEvent OnEnd; // event triggered at the end of the battle
    public UnityEvent OnFlee; // event triggered if player clicks flee
    #endregion

    #region UI Elements
    public Canvas battleUI; // the canvas for battle UI elements
    public GameObject winScreen; // the canvas displayed when the player wins
    public GameObject loseScreen; // the canvas displayed when the player loses
    #endregion

    [Tooltip("The current battle Scriptable Object, will be set by the object that calls on the battle script, only here for visibility")]
    public Battle_SO battle; // current battle SO passed in when battlestart is called
    public BattleState battleState; // current state of the battle
    public EndState endState;
    #region All the player scripts
    private GameObject player; // reference to the player game object
    private PlayerController playerController; // reference to the player controller
    public GameObject playerspacePrefab; // prefab for the player's playspace
    public GameObject playerspacePlayOnSelf;
    private Inventory playerInventory; // reference to the player's inventory
    private float playerMaxHealth; // reference to the player's max health
    private float playerCurrentHealth; // reference to the player's current health
    #endregion

    #region Input Actions
    public InputActionAsset inputActions; // reference to the input system
    public CardDragInput cardDragInput; // reference to the card drag input script
    #endregion


    #region Card Lists
    [SerializeField] private List<InventoryCard> playerDeckCopyInitial; // copy of the player's deck at start of battle
    [SerializeField] private List<InventoryCard> playerDeckCopyActive; // copy of the player's deck for shuffling and use in battle

    public List<InventoryCard> PlayerDeckCopyActive
    {
        get { return playerDeckCopyActive; }
        set { playerDeckCopyActive = value; }
    }

    public InventoryCard restCard;
    #endregion

    public List<GameObject> currentEnemies; // list of current enemy game objects in the battle

    public GameObject cardPrefab; // Generic prefab for the cards used in battle


    public bool isBattling = false; // flag to indicate if a battle is currently ongoing

    public enum BattleState //Indicates State of Gameplay. Can be START, END, PLAYER_TURN, ENEMIES_TURN, CHECK_PLAYER_HP, CHECK_ENEMIES_HP
    {
        START,
        END,
        PLAYER_TURN,
        ENEMIES_TURN,
        CHECK_PLAYER_HP,
        CHECK_ENEMIES_HP
    }

    public enum EndState //Indicates State at BattleState.END. Can be WIN or LOSE, if not BattleState.END, then NA
    {
        NA,
        WIN,
        LOSE
    }


    #region Setup
    private void Awake()
    {
        // Check if an instance already exists
        if (instance != null && instance != this)
        {
            Destroy(instance.gameObject);
        }

        // Otherwise, set the instance to this object
        instance = this;
        allDrops = new List<Drop>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerInventory = GameObject.FindGameObjectWithTag("PlayerInventory").GetComponent<Inventory>();
        playerController = player.GetComponent<PlayerController>();
        // player.GetComponent<PlayerInteract>().interacting = true;

        //Assign Variables for Cameras and UI
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        OnBattleStart.AddListener(() => Debug.Log("Battle Started!")); //Occurs on start
        OnLose.AddListener(() => Debug.Log("You Lose!")); //Occurs on Lose
        OnWin.AddListener(() => {Debug.Log("You Win!"); Win();}); //Occurs on Win
        PlayerTurn.AddListener(() => Debug.Log("Player's Turn")); //Occurs on Player Turn
        EnemyTurn.AddListener(() => Debug.Log("Enemy's Turn")); //Occurs on Enemies Turn
        OnEnd.AddListener(() => Debug.Log("Battle Over")); //Occurs on Battle End
    }

    private void OnDestroy() //Swap camera back to main at end of battle.
    {
        battleCamera.enabled = false;
        mainCamera.enabled = true;
        GameStateScript.CurrentState = GameStateScript.GameState.WALKING;
        player.GetComponent<PlayerInteract>().interacting = false;

        
    }
    #endregion

    public void Win()
    {
        float totalWeights = 0;
        foreach(Drop drop in allDrops)
        {
            if(drop.weight <= 0)
            {
                if(drop.dropType == Drop.DropType.EXP)
                {
                    //playerInventory.xp += drop.quantity;
                }
                else if(drop.dropType == Drop.DropType.MONEY)
                {
                    playerInventory.Money += drop.quantity;
                }
            }
            else
            {
                totalWeights += drop.weight;
                chanceDrops.Add(drop);
            }
        }
        float randVal = UnityEngine.Random.Range(0, totalWeights);
        foreach (Drop drop in chanceDrops)
        {
            if(randVal < drop.weight)
            {
                switch(drop.dropType)
                {
                    case(Drop.DropType.CARD):
                    {
                        playerInventory.AddCardToInventory((Card_SO)drop.item, 1, true);
                        break;
                    }
                    case(Drop.DropType.HACK):
                    {
                        playerInventory.AddHackToInventory((Hack_SO)drop.item);
                        break;
                    }
                    case(Drop.DropType.MISC):
                    {
                        Debug.Log("Add this type of functionality.");
                        break;
                    }
                    default:
                    {
                        Debug.Log("This should never print. (BattleManager.Win)");
                        break;
                    }
                }
            }
            randVal -= drop.weight;
        }
        
    }

    #region Startup
    //Function called by an outside force to start a battle, must pass in battle_SO
    public void StartBattle(Battle_SO battle)
    {
        // Spawn enemies based on the Battle_SO
        this.battle = battle;

        //Switches Camera to Battle camera
        mainCamera.enabled = false;
        battleCamera.enabled = true;
        battleUI.gameObject.SetActive(true);
        //Get the player set up (not in awake cause it ran before the player Inventory was set
        playerDeckCopyInitial = new List<InventoryCard>(playerInventory.Deck);

        playerMaxHealth = playerController.maxPlayerHealth;
        playerCurrentHealth = playerController.currentHealth;

        //Get the enemy set up

        SetupPlayspaces();

        battleState = BattleState.START; //So that it finishes setup correctly.
        isBattling = true;
        OnBattleStart.Invoke();
        StartCoroutine(BattleStateManager());
        print("BattleStateManager has run.");
    }


    void SetupPlayspaces()
    {
        float canvasWidth = battleUI.GetComponent<RectTransform>().rect.width;
        float canvasHeight = battleUI.GetComponent<RectTransform>().rect.height;
        float enemySpacing = canvasWidth / (battle.enemies.Length);
        int i = 0;


        //Sets Playerspace to be in bottom center
        playerspacePrefab = Instantiate(playerspacePrefab, new Vector3((canvasWidth / 2), -(canvasHeight * 3 / 4), 0), Quaternion.identity);
        playerspacePrefab.transform.SetParent(battleUI.gameObject.transform, false);
        playerspacePlayOnSelf = playerspacePrefab.transform.GetChild(2).gameObject;
        cardDragInput.AddActivePlayspace(playerspacePrefab.GetComponent<Playspace>());

        cardDragInput.AddActivePlayspace(playerspacePlayOnSelf.GetComponent<Playspace>());
        playerspacePlayOnSelf.GetComponent<Playspace>().allowedDonors.Add(playerspacePrefab.GetComponent<Playspace>());
        
        //Shows player HP and Mana
        playerController.healthbar = playerspacePrefab.transform.GetChild(0).GetComponent<Image>();
        playerController.shieldText = playerspacePrefab.transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>();
        playerController.shieldPanel = playerspacePrefab.transform.GetChild(1).gameObject;
        playerController.UpdateShield();

        foreach (Enemy_SO e in battle.enemies)
        {
            GameObject enemyPrefab = e.enemyPrefab;
            enemyPrefab = Instantiate(e.enemyPrefab, new Vector3(0 + (enemySpacing * (i - 1)), (canvasHeight * 1 / 4), 0), Quaternion.identity);
            enemyPrefab.transform.SetParent(battleUI.gameObject.transform, false);
            enemyPrefab.GetComponent<Enemy>().SetUp(e);

            //Player playspace allowed donors
            

            cardDragInput.AddActivePlayspace(enemyPrefab.GetComponentInChildren<Playspace>());
            enemyPrefab.GetComponentInChildren<Playspace>().allowedDonors.Add(playerspacePrefab.GetComponent<Playspace>());
            currentEnemies.Add(enemyPrefab);
            i++;
        }
        


    }
    #endregion 
    //Player based defense needs to be fixed.

    #region Battle Flow

    IEnumerator BattleStateManager()
    {
        while(isBattling)
        {
            switch (battleState)
            {
                case BattleState.START:
                {
                    EnemiesChooseCards();
                    //print("EnemiesChooseCards has run.");
                    battleState = BattleState.PLAYER_TURN;
                    playerController.currentHealth = playerController.maxPlayerHealth;
                    break;
                }
                case BattleState.ENEMIES_TURN:
                {
                    EnemyTurn.Invoke();
                    yield return StartCoroutine(StartEnemyTurn());
                    battleState = BattleState.CHECK_PLAYER_HP;
                    break;
                }
                case BattleState.PLAYER_TURN:
                {
                    PlayerTurn.Invoke();
                    yield return StartCoroutine(StartPlayerTurn());
                    battleState = BattleState.CHECK_ENEMIES_HP;
                        break;
                }
                case BattleState.CHECK_PLAYER_HP:
                {
                    battleState = BattleState.PLAYER_TURN;
                    yield return StartCoroutine(checkEndConditions());
                    break;
                }
                case BattleState.CHECK_ENEMIES_HP:
                {
                    battleState = BattleState.ENEMIES_TURN;
                    yield return StartCoroutine(checkEndConditions());
                    break;
                }
                case BattleState.END:
                {
                    print("ended");
                    isBattling = false;
                    playerController.statusEffects.Clear();
                    playerController.currentHealth = playerMaxHealth;
                    OnEnd.Invoke();
                    break;
                }
            }

            //turnCount++;
            //if (turnCount > 30)
            //    battleState = BattleState.END;
        }

        yield return null;
    }

    private IEnumerator EndStateManager()
    {
        switch (endState)
        {
            case (EndState.WIN):
            {
                winScreen.SetActive(true);
                OnWin.Invoke();
                break;
            }
            case (EndState.LOSE):
            {
                loseScreen.SetActive(true);
                OnLose.Invoke();
                break;
            }
        }

        yield return null;
    }

    private void EnemiesChooseCards(int enemyIndex = -1)
    {
        if (enemyIndex == -1)
        {
            foreach (var enemy in currentEnemies)
            {
                var enemyScript = enemy.GetComponent<Enemy>();
                enemyScript.ShuffleDeck();
                var cardToPlay = enemyScript.DrawCard();
                //print("Mana: " + enemyScript.currentMana + ", Cost: " + cardToPlay.cardSO.energyCost);
                if (enemyScript.currentMana > cardToPlay.cardSO.energyCost)
                {
                    enemyScript.currentCard = cardToPlay;
                    enemyScript.currentMana -= cardToPlay.cardSO.energyCost;
                }
                else
                {
                    enemyScript.currentCard = restCard;
                    enemyScript.currentMana -= enemyScript.currentCard.cardSO.energyCost;
                }
                enemyScript.UpdateActionState();
                //print("enemy created and updated.");
            }
        }
        else
        {
            var enemy = currentEnemies[enemyIndex];
            var enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.ShuffleDeck();
            var cardToPlay = enemyScript.DrawCard();
            //print("Mana: " + enemyScript.currentMana + ", Cost: " + cardToPlay.cardSO.energyCost);
            if (enemyScript.currentMana > cardToPlay.cardSO.energyCost)
            {
                enemyScript.currentCard = cardToPlay;
                enemyScript.currentMana -= cardToPlay.cardSO.energyCost;
            }
            else
            {
                enemyScript.currentCard = restCard;
                enemyScript.currentMana -= enemyScript.currentCard.cardSO.energyCost;
            }
            enemyScript.UpdateActionState();
            //print("enemy created and updated.");
        }
    }
    #endregion

    #region Turns
    public IEnumerator StartPlayerTurn()
    {

        PlayerTurn.Invoke();
        //Check if player is out of cards
        if (playerDeckCopyActive.Count <= 0)
        {
            playerDeckCopyActive = playerInventory.Shuffle(new List<InventoryCard>(playerInventory.Deck));

            //Add NewPlayItem from playsapce for each card in deck copy
            foreach (InventoryCard card in playerDeckCopyActive)
            {
                GameObject playerCard = playerspacePrefab.GetComponent<Playspace>().NewPlayItem(cardPrefab, card.cardSO);
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
            InventoryCard card = enemyScript.currentCard;

            //Plays Card

            enemyScript.currentTimer--;
            enemyScript.UpdateTimer();

            foreach (BattleEffect effect in card.cardSO.cardEffects)
            {
                if (enemy.GetComponent<Enemy>().isStunned) continue;
                if (enemyScript.currentTimer > 0) continue;

                switch (enemyScript.currentActionType) //Chooses to attack or defend based on the current action type of the enemy.
                {
                    case ("ATK"):
                    {
                        effect.TriggerEffect(playerController, player.transform.position);
                        break;
                    }
                    case ("DEF"):
                    {
                        enemyScript.CurrentShield += effect.StatusAmount;
                        break;
                    }
                }
            }

            if (enemyScript.currentTimer <= 0)
            {
                EnemiesChooseCards(currentEnemies.IndexOf(enemy));
                enemyScript.currentTimer = 3;
                enemyScript.UpdateTimer();
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

    public IEnumerator checkEndConditions()
    {
        //If player health <= 0, battleState = BattleState.LOST
        if (playerController.currentHealth <= 0)
        {
            battleState = BattleState.END;
            endState = EndState.LOSE;
            isBattling = false;
            yield return StartCoroutine(EndStateManager());
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
                break;
            }
        }
        if (allDead)
        {
            battleState = BattleState.END;
            endState = EndState.WIN;
            isBattling = false;
            yield return StartCoroutine(EndStateManager());
        }

        yield return null;
        


    }


    #endregion

    #region EndGameButtons

    public void MainMenu()
    {
        mainCamera.enabled = true;
        battleCamera.enabled = false;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void Retry()
    {
        mainCamera.enabled = true;
        battleCamera.enabled = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void Continue()
    {
        player.GetComponent<PlayerInteract>().interacting = false;
        mainCamera.enabled = true;
        battleCamera.enabled = false;
        Destroy(this.gameObject);
    }

    public void Flee()
    {
        player.GetComponent<PlayerInteract>().interacting = false;
        OnFlee.Invoke();
        mainCamera.enabled = true;
        battleCamera.enabled = false;
        Destroy(this.gameObject);
    }

    #endregion



}
