using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
//using NUnit.Framework;
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
    public StartBattle startBattle;
    public Battle_SO battle; // current battle SO passed in when battlestart is called
    public FieldEffect_SO fieldCondition; //***The current active field condition
    public BattleState battleState; // current state of the battle
    public EndState endState;
    #region All the player scripts
    [SerializeField]private GameObject player; // reference to the player game object
    [SerializeField]private PlayerController playerController; // reference to the player controller
    [SerializeField]private PlayerInteract playerInteract; // reference to player interact
    public GameObject playerspacePrefab; // prefab for the player's playspace
    public GameObject playerspacePlayOnSelf;
    [SerializeField]private float playerMaxHealth; // reference to the player's max health
    [SerializeField]private float playerCurrentHealth; // reference to the player's current health
    [SerializeField]private float attackAnimDelay = 0.5f; // How long the enemy moves down
    [SerializeField] private float attackOffset = 0.25f;
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

    #region utility References
    public bool tutorial = false;
    public DialogueScripts.DialogueManager dialogueManager;
    public int turnCount = 0;
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
        playerController = player.GetComponent<PlayerController>();
        // player.GetComponent<PlayerInteract>().interacting = true;

        //Assign Variables for Cameras and UI
        mainCamera = Camera.main;
        dialogueManager = GameObject.Find("DialogueScreen").GetComponent<DialogueScripts.DialogueManager>();
    }

    private void OnEnable()
    {
        // OnBattleStart.AddListener(() => Debug.Log("Battle Started!")); //Occurs on start
        // OnLose.AddListener(() => Debug.Log("You Lose!")); //Occurs on Lose
        OnWin.AddListener(() => {Debug.Log("You Win!");}); //Occurs on Win
        // PlayerTurn.AddListener(() => Debug.Log("Player's Turn")); //Occurs on Player Turn
        // EnemyTurn.AddListener(() => Debug.Log("Enemy's Turn")); //Occurs on Enemies Turn
        OnEnd.AddListener(() => {Debug.Log("Battle Over"); playerInteract.battleManager = null;}); //Occurs on Battle End
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
                    ExpLevels.CurrentExp += drop.quantity;
                }
                else if(drop.dropType == Drop.DropType.MONEY)
                {
                    Inventory.Money += drop.quantity;
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
                        Inventory.AddCardToInventory((Card_SO)drop.item);
                        break;
                    }
                    case(Drop.DropType.HACK):
                    {
                        Inventory.AddHackToInventory((Hack_SO)drop.item);
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
        print(battle.name);
        // Spawn enemies based on the Battle_SO
        this.battle = battle;

        //Enter battle with weather active
        if (battle.fieldCondition)
        {
            this.fieldCondition = battle.fieldCondition;
            fieldCondition.active = true;
            fieldCondition.turnsRemaining = fieldCondition.turnsActive;
            StartCoroutine(TurnBasedFieldEffects());
            print("Set start field condition");
        }

        if (battle.isTutorial) tutorial = true;

        //Switches Camera to Battle camera
        mainCamera.enabled = false;
        battleCamera.enabled = true;
        battleUI.gameObject.SetActive(true);
        //Get the player set up (not in awake cause it ran before the player Inventory was set
        playerDeckCopyInitial = new List<InventoryCard>(Inventory.Deck);

        playerMaxHealth = playerController.maxPlayerHealth;
        playerCurrentHealth = playerController.currentHealth;
        playerInteract = FindAnyObjectByType<PlayerInteract>();
        playerInteract.battleManager = this;

        //Get the enemy set up

        SetupPlayspaces();

        battleState = BattleState.START; //So that it finishes setup correctly.
        isBattling = true;
        OnBattleStart.Invoke();
        StartCoroutine(BattleStateManager());
        // print("BattleStateManager has run.");
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
        playerController.healthbar = playerspacePrefab.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        playerController.currentHealthText = playerspacePrefab.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        playerController.shieldText = playerspacePrefab.transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>();
        playerController.shieldPanel = playerspacePrefab.transform.GetChild(1).gameObject;
        playerController.Shield = 0;

        foreach (Enemy_SO e in battle.enemies)
        {
            GameObject enemyPrefab = e.enemyPrefab;
            enemyPrefab = Instantiate(e.enemyPrefab, new Vector3(0 + (enemySpacing * (i - 1)), (canvasHeight * 1 / 4), 0), Quaternion.identity);
            enemyPrefab.transform.SetParent(battleUI.gameObject.transform, false);
            enemyPrefab.GetComponent<Enemy>().SetUp(e);

            //Player playspace allowed donors
            

            cardDragInput.AddActivePlayspace(enemyPrefab.GetComponent<Enemy>().cardToPlayspace);
            cardDragInput.AddActivePlayspace(enemyPrefab.GetComponent<Enemy>().enemyPlayspace);
            enemyPrefab.GetComponentInChildren<Playspace>().allowedDonors.Add(playerspacePrefab.GetComponent<Playspace>());
            currentEnemies.Add(enemyPrefab);
            i++;
            
        }
        ResetEnemyPositions();
    }
    #endregion 
    //Player based defense needs to be fixed.

    #region Battle Flow
    public void PlayDialogue()
    {
        if (tutorial)
        {
            for  (int i = 0; i < battle.dialogueSOs.Count; i++)
            {
                if (turnCount == battle.dialogueSOs[i].GetTurn())
                {
                    dialogueManager.StartDialogue(battle.dialogueSOs[i].dialogue);
                }
            }
        }
    }


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
                    PlayDialogue();
                    turnCount++;
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
            playerDeckCopyActive = Inventory.Shuffle(Inventory.Deck);
            //playerDeckCopyActive = Inventory.Deck;

            //print("TESTING: " + playerDeckCopyActive.Count);

            //Add NewPlayItem from playspace for each card in deck copy
            foreach (InventoryCard card in playerDeckCopyActive)
            {
                GameObject playerCard = playerspacePrefab.GetComponent<Playspace>().NewPlayItem(cardPrefab, card.cardSO);
                playerCard.GetComponent<Card>().inventoryCard = card;
                playerCard.GetComponent<Card>().hacks = card.hacks;
                playerCard.GetComponent<Card>().CardSO = card.cardSO;
            }
            //print("TESTING2: " + playerDeckCopyActive.Count);
        }
        //Display cards

        // Start Player turn coroutine to handle playing cards 
        if(playerController.isStunned)
        {
            print("Player is stunned");
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            yield return StartCoroutine(cardDragInput.DragDrop());    
        }
         

        //Status Effects get activated
        yield return StartCoroutine(playerController.StatusEffects());

        //Evaluate Field Conditions for the turn
        StartCoroutine(TurnBasedFieldEffects());


        yield return new WaitForSeconds(1f);
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

        /*//Status Effects get activated, seperate foreach to ensure all enemies get status effects applied after all cards are played
        foreach (GameObject enemy in currentEnemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            //InventoryCard card = enemyScript.DrawCard();
            yield return StartCoroutine(enemyScript.StatusEffects());
        }*/

        EnemyTurn.Invoke();

        //Enemy picks card from card list
        //foreach (GameObject enemy in currentEnemies)
        int count = currentEnemies.Count;
        for(int i = 0; i < count; i++) //Changed from foreach loop to account for summoning enemies
        {
            GameObject enemy = currentEnemies[i];
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript.currentHealth <= 0) continue; //Skip turn if enemy is dead
            InventoryCard card = enemyScript.currentCard;
            
            // Image nextCardDisplay = card.image;
            //Plays Card

            enemyScript.currentTimer--;
            enemyScript.UpdateTimer();

            bool reflected = false; //track if player counterspell has been triggered

            foreach (BattleEffect effect in card.cardSO.cardEffects)
            {
                if (enemy.GetComponent<Enemy>().isStunned) continue;
                if (enemyScript.currentTimer > 0) continue;

                if(effect.summonsEnemies)
                {
                    TrySummonEnemy(effect);
                }

                switch(effect.actionType)
                {
                    case(BattleActionType.ATTACK):
                    {
                        //print("Enemy attacking opponent. Attack multi: " + enemyScript.attackMulti);

                        //If the player has counterSpell, launch the attack on the enemy instead
                        if(playerController.counterSpellActive)
                        {
                            effect.TriggerEffect(enemyScript, enemyScript.transform.position, card.cardSO, enemyScript.attackMulti);
                            print("Spell countered");
                            reflected = true;
                        }
                        else
                        {
                            effect.TriggerEffect(playerController, player.transform.position, card.cardSO, enemyScript.attackMulti);
                        }
                        break;
                    }
                    case(BattleActionType.DEFEND):
                    {
                        print("Enemy defending themelves");
                        effect.TriggerEffect(enemyScript, enemyScript.transform.position, card.cardSO);
                        break;
                    }
                    case(BattleActionType.HEAL):
                    {
                        print("Enemy healing themselves");
                        effect.TriggerEffect(enemyScript, enemyScript.transform.position, card.cardSO);
                        break;
                    }
                    default:
                    {
                        print("Enemy doing some other option");
                        break;
                    }
                }

            }

            if(reflected) //disable player counterSpell
            {
                playerController.counterSpellActive = false;
            }

            if (enemyScript.currentTimer <= 0)
            {  
                if(enemyScript.currentActionType == CardType.ATK) enemyScript.attackAnim.SetTrigger("Attack");
                
                #region attackAnim
                // float xOffset = 0;
                // float yOffset = 0;
                // float slope = 0;
                //
                // GameObject ps = playerspacePrefab.transform.GetChild(0).gameObject;
                //
                // //print("ps y: " + ps.transform.position.y);
                // //print("enemy y: " + enemy.transform.position.y);
                // xOffset = -(enemy.transform.position.x - ps.transform.position.x);
                // yOffset = enemy.transform.position.y + ps.transform.position.y;
                //
                // slope = yOffset/xOffset;
                // if (float.IsInfinity(slope)) slope = yOffset;
                //
                // //print("yOffset: " + yOffset);
                // //print("XOffset: " + xOffset);
                // //print("slope: " + slope);
                // if (xOffset == 0) xOffset = 1;
                // Vector3 moveAnim = new Vector3(attackOffset*xOffset, -slope*attackOffset*xOffset, 0);
                //
                // enemyScript.enemyImage.transform.position += moveAnim;
                // yield return new WaitForSeconds(attackAnimDelay);
                // enemyScript.enemyImage.transform.position -= moveAnim;
                #endregion
                
                EnemiesChooseCards(currentEnemies.IndexOf(enemy));
                enemyScript.currentTimer = 3;
                enemyScript.UpdateTimer();
            }

            yield return new WaitForSeconds(1f);
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
        ResetEnemyPositions();
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
        player.GetComponent<PlayerInteract>().interacting = false;
        mainCamera.enabled = true;
        battleCamera.enabled = false;
        startBattle.battleStarted = false;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void Retry()
    {
        player.GetComponent<PlayerInteract>().interacting = false;
        mainCamera.enabled = true;
        battleCamera.enabled = false;
        startBattle.battleStarted = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void Continue()
    {
        player.GetComponent<PlayerInteract>().interacting = false;
        mainCamera.enabled = true;
        battleCamera.enabled = false;
        startBattle.battleStarted = false;
        Destroy(this.gameObject);
    }

    public void Flee()
    {
        player.GetComponent<PlayerInteract>().interacting = false;
        OnFlee.Invoke();
        mainCamera.enabled = true;
        battleCamera.enabled = false;
        startBattle.battleStarted = false;
        Destroy(this.gameObject);
    }

    #endregion




    #region Enemy Manipulation
    //Repositions enemies
    void ResetEnemyPositions()
    {
        //Count number of alive enemies
        int alive = 0;
        foreach(GameObject e in currentEnemies)
        {
            if(e.GetComponent<Enemy>().currentHealth > 0)
            {
                alive++;
            }
        }

        //Re-positions playspaces based on the number of alive enemies in the battle
        float canvasWidth = battleUI.GetComponent<RectTransform>().rect.width;
        float canvasHeight = battleUI.GetComponent<RectTransform>().rect.height;
        float enemySpacing = canvasWidth/2 / (alive);

        Vector3 position;
        int i = 0;
        foreach(GameObject e in currentEnemies)
        {
            //perform operation only on alive enemies
            if(e.GetComponent<Enemy>().currentHealth > 0)
            {
                i++;
                position = new Vector3(0, (canvasHeight * 1 / 4), 0);
                if(alive % 2 == 1) //for odd number battles, enemies are positioned as: (side  mid  side)
                {
                    if(i % 2 == 1)
                    {
                        float off =  2 * ((canvasWidth/2) / alive) * (int)(i/2); 
                        e.transform.localPosition = position + Vector3.left * off;
                    }
                    else if(i % 2 == 0)
                    {
                        float off =  2 * ((canvasWidth/2) / alive) * (int)(i/2);
                        e.transform.localPosition = position + Vector3.right * off;
                    }
                }
                else if(alive % 2 == 0) //for even number battles, enemies are positioned as: (side  mid  mid  side)
                {
                    if(i % 2 == 1)
                    {
                        float off =  2 * ((canvasWidth/2) / alive) * (i/2f);
                        e.transform.localPosition = position + Vector3.left * off;
                    }
                    else if(i % 2 == 0)
                    {
                        float off =  2 * ((canvasWidth/2) / alive) * ((i-1)/2f);
                        e.transform.localPosition = position + Vector3.right * off;
                    }
                }
               
            }
        }
    }


    //Summons enemies
    private void TrySummonEnemy(BattleEffect effect)
    {
        if(!effect.summonsEnemies || effect.summonableEnemies.Length == 0)
        {
            return;
        }
        //print("Evaluating Enemy Card Battle Effects");
        //print("Summons enemies: " + effect.summonsEnemies);
        //print("Card Name: " + card.cardSO.displayName);
        print("Attempting to summon enemy");
        //Summoning enemy logic

        //Get number of enemies alive
        int alive = 0;
        foreach (GameObject enemy in currentEnemies)
        {
            if (enemy.GetComponent<Enemy>().currentHealth > 0)
            {
                alive++;
            }
        }

        //Summon fails if six or more enemies are on the field, just so they don't start overlapping
        if(alive >= 6)
        {
            print("Max enemies, summon failed");
        }
        else
        {
            print("Summoned enemy");
            //Selects random enemy from list of possible options set in the card's battle effect
            Enemy_SO newEnemy = effect.summonableEnemies[UnityEngine.Random.Range(0, effect.summonableEnemies.Length)];

            //Same code for creating an enemy object used in SetUpPlayspaces
            GameObject enemyPrefab = newEnemy.enemyPrefab;
            enemyPrefab = Instantiate(newEnemy.enemyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            enemyPrefab.transform.SetParent(battleUI.gameObject.transform, false);
            enemyPrefab.GetComponent<Enemy>().SetUp(newEnemy);

            cardDragInput.AddActivePlayspace(enemyPrefab.GetComponentInChildren<Playspace>());
            enemyPrefab.GetComponentInChildren<Playspace>().allowedDonors.Add(playerspacePrefab.GetComponent<Playspace>());
            currentEnemies.Add(enemyPrefab);

            EnemiesChooseCards(currentEnemies.IndexOf(enemyPrefab));
            ResetEnemyPositions();
        }
    }
    #endregion




    #region Player Attack Targeting
    //Methods for the player to use to attack in accordance with an effect's targeting type.
    //These methods are called in the Card script's TryPlayCard(Enemy enemy){}
    //These only handle effects with the ATTACK action type. For positive statusEffects, just use the DEFEND or HEAL action types


    //Method for the player to attack one enemy. Done just to centralize the system and make universal changes easier
    public void PlayerAttackOneEnemy(List<BattleEffect> effects, Enemy enemyScript, Card_SO card)
    {
        foreach(BattleEffect effect in effects)
        {
            if(effect.targetingType != TargetingType.SingleTarget){continue;}


            switch(effect.actionType)
            {
                case(BattleActionType.ATTACK):
                {
                    //If enemy has counterSpell, hit the player with the effect. Else, hit the enemy as usual
                    if(enemyScript.counterSpellActive)
                    {
                        effect.TriggerEffect(playerController, playerController.transform.position, card, playerController.attackMulti);

                        enemyScript.cSpellTriggered = true;
                        print("counterspell triggered");
                    }
                    else
                    {
                        effect.TriggerEffect(enemyScript, enemyScript.transform.position, card, playerController.attackMulti);
                    }
                    break;
                }
                case(BattleActionType.DEFEND):
                {
                    break;
                }
                case(BattleActionType.HEAL):
                {
                    break;
                }
                default:
                {
                    break;
                }
            }

        }

        //Disable counterspell if the enemy had it triggered
        if(enemyScript.cSpellTriggered)
        {
            enemyScript.counterSpellActive = false;
            enemyScript.cSpellTriggered = false;
        }

    }

    //Method for specifically the player to affect ALL enemies with a card and its hacks
    public void PlayerAttackAllEnemies(List<BattleEffect> effects, Card_SO card)
    {
        foreach(BattleEffect effect in effects)
        {
            if(effect.targetingType != TargetingType.AOETarget){continue;}

            switch(effect.actionType)
            {
                case(BattleActionType.ATTACK):
                {
                    //If enemy has counterSpell, hit the player with the effect. Else, hit the enemy as usual
                    foreach(GameObject e in currentEnemies)
                    {
                        Enemy enemyScript = e.GetComponent<Enemy>();

                        if(enemyScript.counterSpellActive)
                        {
                            effect.TriggerEffect(playerController, playerController.transform.position, card, playerController.attackMulti);

                            enemyScript.cSpellTriggered = true;
                            print("counterspell triggered");
                        }
                        else
                        {
                            effect.TriggerEffect(enemyScript, enemyScript.transform.position, card, playerController.attackMulti);
                        }
                    }
                    break;
                }
                case(BattleActionType.DEFEND):
                {
                    break;
                }
                case(BattleActionType.HEAL):
                {
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        //Disable counterspell for any enemy that had it triggered
        foreach(GameObject e in currentEnemies)
        {
            Enemy enemyScript = e.GetComponent<Enemy>();

            if(enemyScript.cSpellTriggered)
            {
                enemyScript.counterSpellActive = false;
                enemyScript.cSpellTriggered = false;
            }
        }

    }

    //Method for the player to attack themselves
    public void PlayerAttackSelf(List<BattleEffect> effects, Card_SO card)
    {
        foreach(BattleEffect effect in effects)
        {
            if(effect.targetingType != TargetingType.SelfTarget){continue;}

            //print(effect.StatusAmount);

            switch(effect.actionType)
            {
                case(BattleActionType.ATTACK):
                {
                    
                    effect.TriggerEffect(playerController, playerController.transform.position, card, playerController.attackMulti);
                    break;
                }
                case(BattleActionType.DEFEND):
                {
                    break;
                }
                case(BattleActionType.HEAL):
                {
                    break;
                }
                default:
                {
                    break;
                }
            }

        }

    }
    #endregion









    
    #region Field Effects
    //For turn-based Field effects
    private IEnumerator TurnBasedFieldEffects()
    {
        if(!fieldCondition)
        {
            //print("No field condition exists");
            yield break;
        }
        if(!fieldCondition.active)
        {
            //print("No field condition is active");
            yield break;
        }
        if(fieldCondition.turnsRemaining < 0)
        {
            //print("Field Condition has expired");
            fieldCondition.active = false;
            yield break;
        }
        print(fieldCondition.name + "field condition Is active! " + fieldCondition.turnsRemaining + " turns remaining." );



        //=====Universal stat changes=====//
        if(fieldCondition.hasStatChanges)
        {
            
            foreach(FieldEffects effect in fieldCondition.effects)
            {
                //If the type of field is meant to boost damage and this effect does so
                if(effect.statChanges)
                {

                    if(!playerController.weatherImmune) //eye of the storm status
                    {
                        playerController.fieldAtkBoost = effect.attackBoost;
                        playerController.fieldEndBoost = effect.enduranceBoost;
                    }

                    //Do the same for all enemies
                    foreach(GameObject e in currentEnemies)
                    {
                        if(!e.GetComponent<Enemy>().weatherImmune)
                        {
                            e.GetComponent<Enemy>().fieldAtkBoost = effect.attackBoost;
                            e.GetComponent<Enemy>().fieldEndBoost = effect.enduranceBoost;
                        }
                    }
                }

            }
            

            //make stat changes happen on the first turn
            if(fieldCondition.turnsRemaining == fieldCondition.turnsActive)
            {
                foreach(FieldEffects effect in fieldCondition.effects)
                {
                    //If the type of field is meant to boost damage and this effect does so
                    if(effect.statChanges)
                    {
                        if(!playerController.weatherImmune) //eye of the storm status
                        {
                            playerController.attackMulti *= effect.attackBoost;
                            playerController.enduranceMulti *= effect.enduranceBoost;
                        }
                        foreach(GameObject e in currentEnemies)
                        {
                            if(!e.GetComponent<Enemy>().weatherImmune)
                            {
                                e.GetComponent<Enemy>().attackMulti *= effect.attackBoost;
                                e.GetComponent<Enemy>().enduranceMulti *= effect.enduranceBoost;
                            }
                        }
                    }
                }

            }


        }
        else
        {
            playerController.fieldAtkBoost = 1f;
            playerController.fieldEndBoost = 1f;

            foreach(GameObject e in currentEnemies)
            {
                e.GetComponent<Enemy>().fieldAtkBoost = 1f;
                e.GetComponent<Enemy>().fieldEndBoost = 1f;
            }
        }
        //=====End of Universal stat changes=====//



        //=====Chip damage=====//
        //If an active field condition deals chip damage (acid rain and thunderstorm)
        if(fieldCondition.chipDamage && fieldCondition.turnsRemaining < fieldCondition.turnsActive) //Don't do chip on the first turn
        {
            //Field chip damage works by playing a damaging card on the targets
            //Evaluate field effects, similarrly to how cards evaluate battle effects
            foreach(FieldEffects effect in fieldCondition.effects)
            {
                if(effect.dealsChipDamage && effect.chipDamageCard)
                {
                    //Strike one target at random (thunderstorm)
                    if(effect.chipIsRandom)
                    {
                        int target = (int)UnityEngine.Random.Range(0, currentEnemies.Count + 1);

                        if(target >= currentEnemies.Count && !playerController.weatherImmune) //trigger on player
                        {
                            foreach(BattleEffect bEffect in effect.chipDamageCard.cardEffects)
                            {
                                bEffect.TriggerEffect(playerController, player.transform.position);
                            }
                        }
                        else
                        {
                            foreach(BattleEffect bEffect in effect.chipDamageCard.cardEffects)
                            {
                                if(!currentEnemies[target].GetComponent<Enemy>().weatherImmune)
                                {
                                    bEffect.TriggerEffect(currentEnemies[target].GetComponent<Enemy>(), currentEnemies[target].transform.position, currentEnemies[target].GetComponent<Enemy>().currentCard.cardSO);
                                }
                            }
                        }
                        
                    }
                    else //Hit all enemies and the player (acid rain)
                    {
                        //Play the damaging card on each enemy and the player
                        foreach(BattleEffect bEffect in effect.chipDamageCard.cardEffects)
                        {
                            if(!playerController.weatherImmune)
                            {
                                bEffect.TriggerEffect(playerController, player.transform.position);    
                            }
                            
                            //print("Damaging player with acid rain");
                            foreach(GameObject e in currentEnemies)
                            {
                                if(!e.GetComponent<Enemy>().weatherImmune)
                                {
                                    bEffect.TriggerEffect(e.GetComponent<Enemy>(), e.transform.position, e.GetComponent<Enemy>().currentCard.cardSO);   
                                }
                                //print("Damaging enemies with acid rain");
                            }

                        }
                    }

                }
            }

        }
        //=====End of Chip damage handling=====//




        //---Decrement turn
        fieldCondition.turnsRemaining--;
        //print(fieldCondition.name + " turns remaining: " + fieldCondition.turnsRemaining);

        //If the field's duration is up, deactivate it and reset everything as needed
        if(fieldCondition.turnsRemaining < 0)
        {
            fieldCondition.active = false;

            playerController.fieldAtkBoost = 1f;
            playerController.fieldEndBoost = 1f;

            foreach(GameObject e in currentEnemies)
            {
                e.GetComponent<Enemy>().fieldAtkBoost = 1f;
                e.GetComponent<Enemy>().fieldEndBoost = 1f;
            }
        }
        //---

        yield return null;

    }
    #endregion


}
