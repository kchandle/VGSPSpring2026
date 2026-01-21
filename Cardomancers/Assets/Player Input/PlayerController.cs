using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	// reference to character controller movement
    [SerializeField] private CharacterControllerMovement _characterControllerMovement; 
   
	//Player Input component should have invoke unity events behavior, then make the unity event call this method
    public void OnWalking(InputAction.CallbackContext context) 
    {
	  // assigns the input direction value of the movement script to the actual players input
	  _characterControllerMovement.inputDirection = context.ReadValue<Vector3>(); 
    }

	public void OnJumping(InputAction.CallbackContext context)
	{
			// makes the player jump
			_characterControllerMovement.jumping = true; 
	}
}
