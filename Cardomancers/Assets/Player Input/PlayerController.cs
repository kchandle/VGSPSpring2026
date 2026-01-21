using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterControllerMovement _characterControllerMovement; // reference to character controller movement
    
    public void OnWalking(InputAction.CallbackContext context) //Player Input component should have invoke unity events behavior, then make the unity event call this method
    {
	  _characterControllerMovement.inputDirection = context.ReadValue<Vector3>(); // assigns the input direction value of the movement script to the actual players input
    }

	public void OnJumping(InputAction.CallbackContext context)
	{
			_characterControllerMovement.jumping = true; // makes the player jump
	}
}
