using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	// reference to character controller movement
    [SerializeField] private CharacterControllerMovement _characterControllerMovement; 
	private float baseSpeed;
	private bool sprinting;
   
	//Player Input component should have invoke unity events behavior, then make the unity event call this method
    public void OnWalking(InputValue value) 
    {
	  // assigns the input direction value of the movement script to the actual players input
	  _characterControllerMovement.inputDirection = value.Get<Vector3>(); 
	  baseSpeed  = _characterControllerMovement.characterSpeed;
    }

	public void OnJumping(InputValue value)
	{
		// makes the player jump
		_characterControllerMovement.jumping = true; 
	}



	//Left shift to toggle sprint by either starting to sprint or by returning to base speed
	public void OnSprint(InputValue value)
	{
		if(value.isPressed)
		{
			sprinting = !sprinting;
			if(sprinting) _characterControllerMovement.characterSpeed = baseSpeed * 2;
			else _characterControllerMovement.characterSpeed = baseSpeed;	
		}
	}

}
