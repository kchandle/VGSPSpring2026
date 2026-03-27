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

    public bool TestingFastMode = false;

    public GameObject shieldPanel;
    public TMP_Text shieldText;

    public InventoryUIHandler inventoryUIHandler;
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
        print("pressed escape");
        pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
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
    public IEnumerator StatusEffects()
    {
        for(int i = 0; i < statusEffects.Count; i++)
        {
            // Apply the status effect to the player
            foreach (ParticleSystem particle in statusEffects[i].particles)
            {
                Instantiate(particle, transform.position, Quaternion.identity);
            }
            currentHealth -= statusEffects[i].statusAmount;
            print("Status Effects did " + statusEffects[i].statusAmount + " damage to the player");
            UpdateHealthbar();
            // statusEffects[i].turnsRemaining--;
            // Decrement the turn count for perishable effects
            if (statusEffects[i].DecrementTurn() <= 0)
            {
                // Remove the status effect if it has expired
                statusEffects.Remove(statusEffects[i]);
                Debug.Log("A status effect has expired.");
                i++;
            }
            if(TestingFastMode)
                yield return null;
            else
                yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    public void UpdateHealthbar()
    {
        healthbar.fillAmount = currentHealth / maxPlayerHealth;
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
