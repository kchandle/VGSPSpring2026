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
	public float maxPlayerHealth = 100f;
    public float currentHealth;
    public Image healthbar;
    public TMP_Text currentHealthText;


    //---Variables affected by status effects
    [Header("Status Effect Variables")]
    public float attackMulti = 1f; //Multiplier for outgoing damage if the player has an attack boost
    public float enduranceMulti = 1f; //Multiplier for incoming damage if the player has an endurance boost
    public bool healable = true; //Whether or not player can be healed. 

    public bool isStunned = false; //player doesn't have stun handling yet
    public bool counterSpellActive = false; //Whether or not the player will counter the next damaging spell

    public bool weatherImmune = false; //Whether or not player has the Eye Of The Storm status effect
    public float fieldAtkBoost = 1f; //current attack multiplier as a result of a Field Effect
    public float fieldEndBoost = 1f; //current endurance multiplier as a result of a Field Effect
    //--

    [Header(" ")]
    public bool TestingFastMode = false;

    public GameObject shieldPanel;
    public TMP_Text shieldText;

    public InventoryUIHandler inventoryUIHandler;
    [SerializeField] private int shield = 0;

    public GameObject pauseMenu;
    public GameObject questMenu;

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

    public List<StatusEffectContainer> statusEffects = new List<StatusEffectContainer>();

    public bool isShielded = false; //If the player is shielded, they take no damage this turn.
    #endregion

    private void OnEnable()
    {
        GameStateScript.OnGameStateChanged += UpdateGameState;
    }

    private void OnDisable()
    {
        GameStateScript.OnGameStateChanged += UpdateGameState;
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
        if (CurrentState == GameState.INVENTORY || questMenu.activeSelf) return;
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
            case GameStateScript.GameState.WALKING:
                GameStateScript.CurrentState = GameStateScript.GameState.PAUSE;
                break;
            case GameStateScript.GameState.PAUSE:
                GameStateScript.CurrentState = GameStateScript.GameState.WALKING;
                break;



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

    public void OnQuest(InputAction.CallbackContext context)
    {
        if (pauseMenu.activeSelf || CurrentState == GameState.INVENTORY) return;
        questMenu.SetActive(!questMenu.activeSelf);
        if (questMenu.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    #region Status Effects
    public IEnumerator StatusEffects()
    {
        //---Exceptions that need to be evaluated before other status effects (Cleanses)
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
                    print("Attack Boost statusEffect of " + status.statusAmount + " at index " + i);

                    //Change the attack multiplier accordingly
                    attackMulti *= ((float)status.statusAmount/100);

                    break;
                }
                case(StatusEffectType.EnduranceBoost):
                {
                    print("Endurance Boost statusEffect of " + status.statusAmount + " at index " + i);

                    //Change the endurance multiplier accordingly
                    enduranceMulti *= ((float)status.statusAmount/100);

                    break;
                }
                //---

                //---Cleanses
                case(StatusEffectType.CleanseNegative):
                {
                    print("Cleanse negative statusEffects at index: " + i);
                    //Handled above
                    break;
                }
                case(StatusEffectType.CleanseAll):
                {
                    print("Cleanse all statusEffects at index: " + i);
                    //Handled above
                    break;
                }
                //---

                //---Simple DOTs
                case(StatusEffectType.Regeneration):
                {
                    print("Regeneration statusEffect at index: " + i);

                    //Do heal
                    currentHealth += status.statusAmount;
                    print("Regeneration Status Effect healed the Player for " + status.statusAmount + " hp");

                    break;
                }
                case(StatusEffectType.OnFire):
                {
                    print("OnFire statusEffect at index: " + i);

                    //Do burn damage
                    currentHealth -= status.statusAmount;
                    print("OnFire Status Effect did " + status.statusAmount + " damage to the Player");
                    enduranceMulti *= 0.75f;

                    break;
                }
                case(StatusEffectType.Poisoned):
                {
                    print("Poisoned statusEffect at index: " + i);

                    //Do poison damage
                    currentHealth -= status.statusAmount; 
                    print("Poisoned Status Effect did " + status.statusAmount + " damage to the Player");

                    break;
                }
                case(StatusEffectType.Frostbite):
                {
                    print("Frostbite statusEffect at index: " + i);
                    
                    //Do frostbite damage
                    currentHealth -= status.statusAmount;
                    print("Frostbite Status Effect did " + status.statusAmount + " damage to the Player");
                    attackMulti *= 0.75f;

                    break;
                }
                case(StatusEffectType.Awestruck)://*
                {
                    print("Awestruck statusEffect at index: " + i);

                    //damage only triggers when stunned
                    if(isStunned)
                    {
                        currentHealth -= status.statusAmount;
                        print("Awakened Status Effect did " + status.statusAmount + " damage to the Player");
                    }

                    break;
                }
                //---

                //---Not done yet
                case(StatusEffectType.Stun): //*
                {
                    print("Stun statusEffect at index: " + i);

                    //Handled in the exceptions above

                    break;
                }
                case(StatusEffectType.CounterSpell): //done
                {
                    print("CounterSpell statusEffect at index: " + i);

                    //Set counterSpellActive to true, then immedieately remove this status effect.
                    //counterSpellActive will be set to false in the BattleManager, after a spell is reflected
                    counterSpellActive = true;
                    statusEffects[i].turnsRemaining = -1;

                    break;
                }
                case(StatusEffectType.EyeOfTheStorm):
                {
                    print("EyeOfTheStorm statusEffect at index: " + i);

                    //When field effects act, they'll check if the target is weatherImmune first. See in BattleEffect and BattleManager
                    weatherImmune = true;

                    break;
                }
                case(StatusEffectType.AntiHeal): //done
                {
                    print("AntiHeal statusEffect at index: " + i);

                    //Handled in the exceptions above

                    break;
                }
                case(StatusEffectType.Evisceration): //done
                {
                    print("Evisceration statusEffect at index: " + i);

                    //
                    currentHealth -= 200;

                    break;
                }
                //---

                case(StatusEffectType.Random):
                {
                    print("Random statusEffect at index: " + i);
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
            

            UpdateHealthbar();
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
    #endregion

    public void UpdateHealthbar()
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
