using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static GameStateScript;
//using static UnityEditor.PlayerSettings;
using static UnityEngine.ParticleSystem;

public class PlayerController : MonoBehaviour
{
    #region Variables
    private GameState initialState;
    public Image healthbar;
    public TMP_Text currentHealthText;
    public float maxPlayerHealth = 100f;
    public float currentHealth;

    //---Variables affected by status effects
    [Header("Status Effect Variables")]
    public List<StatusEffectContainer> statusEffects = new List<StatusEffectContainer>();
    

    public float attackMulti = 1f; //Multiplier for outgoing damage if the player has an attack boost
    public float enduranceMulti = 1f; //Multiplier for incoming damage if the player has an endurance boost

    public bool healable = true; //Whether or not player can be healed. 
    public bool isStunned
    {
        get => IsStunned;
        set
        {
            IsStunned = value;
            BattleManager battleManager = FindFirstObjectByType<BattleManager>();
            if (battleManager != null) battleManager.playerStunIcon.SetActive(value);
                
        }
    }//player doesn't have stun handling yet
    private bool IsStunned = false;

    public bool counterSpellActive = false; //Whether or not the player will counter the next damaging spell
    public bool cSpellTriggered = false; //Whether or not counterSpell had been triggered, used as a signal to disable counterSpellActive

    public bool weatherImmune = false; //Whether or not player has the Eye Of The Storm status effect
    public float fieldAtkBoost = 1f; //current attack multiplier as a result of a Field Effect
    public float fieldEndBoost = 1f; //current endurance multiplier as a result of a Field Effect

    public float CurrentHealth
    {//ensure health can't be increased while unhealable. does nothing otherwise.
        get
        {
            return currentHealth;
        }
        set
        {
            if(value > currentHealth && !healable)
            {
                print("Player is currently unhealable");
                return;
            }
            currentHealth = value;
        }   
    }
    //--

    [Header(" ")]
    public bool TestingFastMode = false;

    public GameObject shieldPanel;
    public TMP_Text shieldText;

    public InventoryUIHandler inventoryUIHandler;
    public GameObject QuestLogUI;
    [SerializeField] private int shield = 0;

    public GameObject pauseMenu;

    public int Shield
    {
        get { return shield; }
        set
        {
            if (value <= 0)
            {
                shield = 0;
                UpdateShield();
            }
            else
            {
                shield = value;
                UpdateShield();
            }
        }
    }


    public bool isShielded = false; //If the player is shielded, they take no damage this turn.
    #endregion

    private void OnEnable()
    {
        GameStateScript.OnGameStateChanged += UpdateGameState;
    }

    private void Start()
    {
        GameStateScript.CurrentState = GameState.WALKING;
    }

    private void OnDisable()
    {
        GameStateScript.OnGameStateChanged -= UpdateGameState;
    }

    public void UpdateGameState(GameState newState)
    {
        if (newState == GameState.WALKING)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
        else 
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void Awake()
    {
        currentHealth = maxPlayerHealth;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    [SerializeField] GameObject inventoryUI;

    // reference to character controller movement
    [SerializeField] private CharacterControllerMovement _characterControllerMovement;

    //Player Input component should have invoke unity events behavior, then make the unity event call this method
    public void OnWalking(InputAction.CallbackContext context) 
    {
	    // assigns the input direction value of the movement script to the actual players input
	     _characterControllerMovement.inputDirectionInput = context.ReadValue<Vector3>();
    }

    public void OnJumping(InputAction.CallbackContext context)
    {
        if (GameStateScript.CurrentState != GameStateScript.GameState.WALKING) return;

        //returns if it isnt the frame that it is pressed
        if (!context.started) return;

        // makes the player jump
        _characterControllerMovement.jumpWasPressed = true;
    }

    public void ClosePauseMenu() => OnEscape(new InputAction.CallbackContext());
    
    public void OnEscape(InputAction.CallbackContext context)
    {
        if (!context.started) return; 

        pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
        if (pauseMenu.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (Time.timeScale == 1f){
                Time.timeScale = 0f;
        } else
        {
          Time.timeScale = 1.0f;  
        }
        switch(GameStateScript.CurrentState)
        {
            
            case GameStateScript.GameState.PAUSE:
            {
                GameStateScript.CurrentState = initialState;
                break;
            }
            default:
            {
                initialState = GameStateScript.CurrentState;
                GameStateScript.CurrentState = GameStateScript.GameState.PAUSE;
                break;
            }


        }
    }

    public void OnSprinting(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _characterControllerMovement.sprinting = true;
            return;
        }
        _characterControllerMovement.sprinting = false;
    }

   public void OnToggleInventory(InputAction.CallbackContext context)
    {
        //if(pauseMenu.activeSelf || questMenu.activeSelf) return;
        //can only open the inventory when in free movement and alive
        if (GameStateScript.CurrentState == GameStateScript.GameState.WALKING && inventoryUIHandler.uiDisplayed == false)
        {
            inventoryUIHandler.DisplayUI();
            GameStateScript.CurrentState = GameStateScript.GameState.INVENTORY;
        }
        else if (GameStateScript.CurrentState == GameStateScript.GameState.INVENTORY/* && inventoryUIHandler.uiDisplayed == true*/)
        {
            inventoryUIHandler.DestroyUI();
            GameStateScript.CurrentState = GameStateScript.GameState.WALKING;
        }
        
    }

    public void OnToggleQuest(InputAction.CallbackContext context)
    {
        if (GameStateScript.CurrentState == GameState.WALKING && QuestLogUI.activeSelf == false)
        {
            QuestLogUI.SetActive(true);
            GameStateScript.CurrentState = GameStateScript.GameState.QUESTUI;
        }
        else if (GameStateScript.CurrentState == GameState.QUESTUI)
        {
            QuestLogUI.SetActive(false);
            GameStateScript.CurrentState = GameState.WALKING;
        }
    }


    #region Status Effects
    public IEnumerator StatusEffects()
    {
        if(statusEffects.Count > 0){print("--Player Status Effects--");}

        //---Exceptions that need to be evaluated before other status effects (Ex: Cleanses)
        bool cleanseNeg = false;
        healable = true;
        isStunned = false;
        for (int i = 0; i < statusEffects.Count; i++)
        {
            StatusEffectContainer status = statusEffects[i];
            //print("Test: " + status.statusType);
            switch(status.statusType)
            {
                case(StatusEffectType.CleanseAll):
                {
                    print("Player cleansing ALL status effects");
                    statusEffects.Clear();
                    break;
                }
                case(StatusEffectType.CleanseNegative):
                {
                    cleanseNeg = true;
                    break;
                }
                case(StatusEffectType.Stun):
                {
                    isStunned = true;
                    break;
                }
                case(StatusEffectType.EyeOfTheStorm):
                {
                    fieldAtkBoost = 1f;
                    fieldEndBoost = 1f;
                    break;
                }
                case(StatusEffectType.AntiHeal):
                {
                    healable = false;
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        if(cleanseNeg)
        {
            print("Cleansing NEGATIVE status effects");
            for (int i = 0; i < statusEffects.Count; i++)
            {
                StatusEffectContainer status = statusEffects[i];
                if(status.isNegative)
                {
                    statusEffects.Remove(status);
                    i--;
                    print("Player cleansed " + status.statusType);
                }
            }
        }
        //---


        //=====Start Loop=====//

        //
        attackMulti = 1 * fieldAtkBoost;
        enduranceMulti = 1 * fieldEndBoost;
        weatherImmune = false;
        //

        for(int i = 0; i < statusEffects.Count; i++)
        {
            //Apply the status effect to the Player
            StatusEffectContainer status = statusEffects[i];

            //There are no particle effects, this doesn't do anything
            foreach (ParticleSystem particle in statusEffects[i].particles)
            {
                Instantiate(particle, transform.position, Quaternion.identity);
            }

            //==Big switch statement to handle EVERY status effect==//
            switch(status.statusType)
            {
                case(StatusEffectType.None):
                {
                    print("No statusEffect. If this is printing, you accidentally triggered isStatusEffect on a card.");
                    break;
                }
                //---Stat boosts
                case(StatusEffectType.AttackBoost):
                {
                    print("Attack Boost. Index: " + i + ". Amount: " + status.statusAmount + ". Duration: " + status.turnsRemaining + ". Source: " + status.statusSource);
                    //Change the attack multiplier accordingly
                    attackMulti *= ((float)status.statusAmount/100);
                    break;
                }
                case(StatusEffectType.EnduranceBoost):
                {
                    print("Endurance Boost. Index: " + i + ". Amount: " + status.statusAmount + ". Duration: " + status.turnsRemaining + ". Source: " + status.statusSource);
                    //Change the endurance multiplier accordingly
                    enduranceMulti *= ((float)status.statusAmount/100);
                    break;
                }
                //---

                //---Cleanses
                case(StatusEffectType.CleanseNegative):
                {
                    print("Cleanse Negative. Index: " + i + ". Amount: " + status.statusAmount + ". Duration: " + status.turnsRemaining + ". Source: " + status.statusSource);
                    //Handled above
                    break;
                }
                case(StatusEffectType.CleanseAll):
                {
                    print("Cleanse All. Index: " + i + ". Amount: " + status.statusAmount + ". Duration: " + status.turnsRemaining + ". Source: " + status.statusSource);
                    //Handled above
                    break;
                }
                //---

                //---Simple DOTs
                case(StatusEffectType.Regeneration):
                {
                    print("Regeneration. Index: " + i + ". Amount: " + status.statusAmount + ". Duration: " + status.turnsRemaining + ". Source: " + status.statusSource);
                    //Do heal
                    CurrentHealth += status.statusAmount;
                    print("Regeneration Status Effect healed the Player for " + status.statusAmount + " hp");
                    break;
                }
                case(StatusEffectType.OnFire):
                {
                    print("OnFire. Index: " + i + ". Amount: " + status.statusAmount + ". Duration: " + status.turnsRemaining + ". Source: " + status.statusSource);
                    //Do burn damage
                    currentHealth -= status.statusAmount;
                    print("OnFire Status Effect did " + status.statusAmount + " damage to the Player");
                    enduranceMulti *= 0.75f;
                    break;
                }
                case(StatusEffectType.Poisoned):
                {
                    print("Poisoned. Index: " + i + ". Amount: " + status.statusAmount + ". Duration: " + status.turnsRemaining + ". Source: " + status.statusSource);
                    //Do poison damage
                    currentHealth -= status.statusAmount; 
                    print("Poisoned Status Effect did " + status.statusAmount + " damage to the Player");
                    break;
                }
                case(StatusEffectType.Frostbite):
                {
                    print("Frostbite. Index: " + i + ". Amount: " + status.statusAmount + ". Duration: " + status.turnsRemaining + ". Source: " + status.statusSource);
                    //Do frostbite damage
                    currentHealth -= status.statusAmount;
                    print("Frostbite Status Effect did " + status.statusAmount + " damage to the Player");
                    attackMulti *= 0.75f;
                    break;
                }
                case(StatusEffectType.Awestruck)://*
                {
                    print("Awestruck. Index: " + i + ". Amount: " + status.statusAmount + ". Duration: " + status.turnsRemaining + ". Source: " + status.statusSource);
                    //damage only triggers when stunned
                    if(isStunned)
                    {
                        currentHealth -= status.statusAmount;
                        print("Awakened Status Effect did " + status.statusAmount + " damage to the Player");
                    }
                    break;
                }
                //---

                //---Other types of status effects
                case(StatusEffectType.Stun):
                {
                    print("Stun. Index: " + i + ". Amount: " + status.statusAmount + ". Duration: " + status.turnsRemaining + ". Source: " + status.statusSource);
                    //Handled in the exceptions above
                    break;
                }
                case(StatusEffectType.CounterSpell):
                {
                    print("CounterSpell. Index: " + i + ". Amount: " + status.statusAmount + ". Duration: " + status.turnsRemaining + ". Source: " + status.statusSource);
                    //Set counterSpellActive to true, then immedieately remove this status effect.
                    //counterSpellActive will be set to false in the BattleManager, after a spell is reflected
                    counterSpellActive = true;
                    cSpellTriggered = false;
                    statusEffects[i].turnsRemaining = -1;
                    break;
                }
                case(StatusEffectType.EyeOfTheStorm):
                {
                    print("EyeOfTheStorm. Index: " + i + ". Amount: " + status.statusAmount + ". Duration: " + status.turnsRemaining + ". Source: " + status.statusSource);
                    //When field effects act, they'll check if the target is weatherImmune first. See in BattleEffect and BattleManager
                    weatherImmune = true;
                    break;
                }
                case(StatusEffectType.AntiHeal): //AntiHeal is completely unused
                {
                    print("AntiHeal. Index: " + i + ". Amount: " + status.statusAmount + ". Duration: " + status.turnsRemaining + ". Source: " + status.statusSource);
                    //Handled in the exceptions above
                    break;
                }
                case(StatusEffectType.Evisceration): //done
                {
                    print("Evisceration. Index: " + i + ". Amount: " + status.statusAmount + ". Duration: " + status.turnsRemaining + ". Source: " + status.statusSource);
                    //
                    break;
                }
                //---

                case(StatusEffectType.Random):
                {
                    print("Random. Index: " + i + ". Amount: " + status.statusAmount + ". Duration: " + status.turnsRemaining + ". Source: " + status.statusSource);
                    //
                    break;
                }
                default:
                {
                    print("If this is printing, you forgot to add " + status.statusType + " to the PlayerController");
                    break;
                }
            }
            //==End of big switch statement to handle EVERY status effect==//
            

            UpdateHealthBar();
            if (statusEffects[i].DecrementTurn() <= 0)
            {
                // Remove the status effect if it has expired
                statusEffects.Remove(statusEffects[i]);
                Debug.Log("The Status Effect " + status.statusType + " has expired on the Player");
                i--;
            }

            if(TestingFastMode)
                yield return null;
            else
                yield return new WaitForSeconds(0.1f);
        }
        //=====End Loop=====//

        yield return null;
    }

    //Script to filter new status effects
    public void AddStatusEffect(StatusEffectContainer newStatus)
    {
        //dw about this one
        if(newStatus.statusType == StatusEffectType.Evisceration)
        {
            currentHealth -= 200;
            return;
        }


        


        
        if(newStatus.statusType == StatusEffectType.Stun)
        {
            isStunned = true;
        }

        //Duplicate status handling
        foreach(StatusEffectContainer status in statusEffects)
        {
            //if the new status effect is equal to an old one in every important aspect and comes from the same card, just add duration to the old one and don't add the new one to the list.
            //Ex: using Dagger of Shadow twice in a row will just decrease attack for a long time instead of a super large decrease
            if(newStatus.statusSource == status.statusSource && newStatus.statusType == status.statusType)
            {
                status.turnsRemaining += newStatus.turnsRemaining;
                return;
            }
        }

        //if the status is unique and requires no exceptions, just add it to the list
        statusEffects.Add(newStatus);
        return;
    }
    #endregion


    public void UpdateHealthBar()
    {
        healthbar.fillAmount = currentHealth / maxPlayerHealth;
        currentHealthText.text = currentHealth + "/" + maxPlayerHealth;
    }


    public void UpdateShield()
    {
        if (Shield == 0)
        {
            shieldPanel.SetActive(true);
            shieldText.text = "0";
            shieldPanel.SetActive(false);
        }
        else
        {
            shieldPanel.SetActive(true);
            shieldText.text = "" + Shield;
        }
    }
}
