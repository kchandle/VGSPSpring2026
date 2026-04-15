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
	public float maxPlayerHealth = 100f;
    public float currentHealth;
    public Image healthbar;
    public TMP_Text currentHealthText;

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

    public void OnEscape(InputAction.CallbackContext context)
    {
        pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
        if (Time.timeScale == 1f){
                Time.timeScale = 0f;
        } else
            {
              Time.timeScale = 1.0f;  
            }
        switch(GameStateScript.CurrentState)
        {
            case GameStateScript.GameState.WALKING:
                GameStateScript.CurrentState = GameStateScript.GameState.INVENTORY;
                break;
            case GameStateScript.GameState.INVENTORY:
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
        if(pauseMenu.activeSelf)
        {
            return;
        }
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
        questMenu.SetActive(!questMenu.activeSelf);
    }

    public IEnumerator StatusEffects()
    {
        //---Exceptions that need to be evaluated before other status effects (Cleanses)
        bool cleanseNeg = false;
        for (int i = 0; i < statusEffects.Count; i++)
        {
            StatusEffectContainer status = statusEffects[i];
            print("Test: " + status.statusType);
            switch(status.statusType)
            {
                case(StatusEffectType.CleanseAll):
                    print("Player cleansing ALL status effects");
                    statusEffects.Clear();
                    break;
                case(StatusEffectType.CleanseNegative):
                    cleanseNeg = true;
                    break;
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
                    print("No statusEffect. If this is printing, you accidentally triggered isStatusEffect on a card.");
                    break;

                //---Simple DOTs
                case(StatusEffectType.Regeneration):
                    print("Regeneration statusEffect at index: " + i);

                    //Do heal
                    currentHealth += status.statusAmount;
                    print("Regeneration Status Effect healed the Player for " + status.statusAmount + " hp");

                    break;
                case(StatusEffectType.OnFire):
                    print("OnFire statusEffect at index: " + i);

                    //Do burn damage
                    currentHealth -= status.statusAmount;
                    print("OnFire Status Effect did " + status.statusAmount + " damage to the Player");

                    break;
                case(StatusEffectType.Poisoned):
                    print("Poisoned statusEffect at index: " + i);

                    //Do poison damage (exact same as burn)
                    currentHealth -= status.statusAmount; 
                    print("Poisoned Status Effect did " + status.statusAmount + " damage to the Player");

                    break;
                //---

                //---More complicated
                case(StatusEffectType.Frostbite):
                    print("Frostbite statusEffect at index: " + i);
                    //

                    break;
                case(StatusEffectType.Awestruck):
                    print("Awestruck statusEffect at index: " + i);

                    //

                    break;
                case(StatusEffectType.Stun):
                    print("Stun statusEffect at index: " + i);

                    //Handled elsewhere

                    break;
                case(StatusEffectType.CounterSpell):
                    print("CounterSpell statusEffect at index: " + i);

                    //

                    break;
                case(StatusEffectType.EyeOfTheStorm):
                    print("EyeOfTheStorm statusEffect at index: " + i);

                    //

                    break;
                case(StatusEffectType.AntiHeal):
                    print("AntiHeal statusEffect at index: " + i);

                    //

                    break;
                //---

                //---Cleanses
                case(StatusEffectType.CleanseNegative):
                    print("Cleanse negative statusEffects at index: " + i);
                    //Handled above
                    break;
                case(StatusEffectType.CleanseAll):
                    print("Cleanse all statusEffects at index: " + i);
                    //Handled above
                    break;
                //---

                default:
                    print("idk");
                    break;
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
