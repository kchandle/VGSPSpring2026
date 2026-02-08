using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;
using static UnityEngine.ParticleSystem;

public class PlayerController : MonoBehaviour
{
	public float maxPlayerHealth = 100f;
    public float currentHealth;
    public Image healthbar;

    public List<StatusEffectContainer> statusEffects = new List<StatusEffectContainer>();

    public bool isShielded = false; //If the player is shielded, they take no damage this turn.

    public void Awake()
    {
        currentHealth = maxPlayerHealth;
    }

    [SerializeField] InventoryUIHandler inventoryUIHandler;

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
        //returns if it isnt the frame that it is pressed
        if (!context.started) return;
		// makes the player jump
	    _characterControllerMovement.jumping = true; 
	}

    public void OnToggleInventory(InputAction.CallbackContext context)
    {
        //can only open the inventory when in free movement and alive
        if (GameStateScript.CurrentState == GameStateScript.GameState.WALKING && inventoryUIHandler.uiDisplayed == false)
        {
            inventoryUIHandler.DisplayUI();
            GameStateScript.CurrentState = GameStateScript.GameState.INVENTORY;
        }
        else if (GameStateScript.CurrentState == GameStateScript.GameState.INVENTORY && inventoryUIHandler.uiDisplayed == true)
        {
            inventoryUIHandler.DestroyUI();
            GameStateScript.CurrentState = GameStateScript.GameState.WALKING;
        }
    }

    public IEnumerator StatusEffects()
    {
        foreach(StatusEffectContainer statusEffect in statusEffects)
        {
            // Apply the status effect to the player
            foreach (ParticleSystem particle in (statusEffect.particles))
            {
                Instantiate(particle, transform.position, Quaternion.identity);
            }
            currentHealth -= statusEffect.statusAmount;

            // Decrement the turn count for perishable effects
            if (statusEffect.DecrementTurn() <= 0)
            {
                // Remove the status effect if it has expired
                statusEffects.Remove(statusEffect);
                Debug.Log("A status effect has expired.");
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    public void UpdateHealthbar()
    {
        healthbar.fillAmount = currentHealth / maxPlayerHealth;
    }


}
