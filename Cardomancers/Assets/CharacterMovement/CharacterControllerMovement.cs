using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class CharacterControllerMovement : MonoBehaviour
{
    public float characterSpeed = 1f; 
	//Vector3 from another script that gives the direction the character will move in 
    public Vector3 inputDirection; 
    // intensity of gravity MUST be 9.8f so it is realistic
    [SerializeField] private float gravity = 9.8f; 
	[SerializeField] private float jumpIntensity = 5f;
	public bool jumping;
    private CharacterController _characterController;

    private void Awake()
    {
	  //Set the character controller reference automatically
	  _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
	  if(!_characterController.isGrounded)
	  {
		//accelerate the player downward when they are not on the ground;
		inputDirection.y -= gravity * Time.deltaTime; 
	  }
	  if(_characterController.isGrounded && jumping)
	  {
		//sets the velocity such that it goes up as high as the jump intensity is set
		inputDirection.y = Mathf.Sqrt(jumpIntensity * 2f * gravity); 
	  }


	  // combines all factors
	  Vector3 finalMovement = (inputDirection * characterSpeed);

	  // moves the player
	  _characterController.Move(finalMovement * Time.deltaTime); 

      //don't fly away
	  jumping = false; 
    }
	  
}
