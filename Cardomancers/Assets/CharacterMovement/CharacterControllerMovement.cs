using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterControllerMovement : MonoBehaviour
{
    public float characterSpeed = 1f; 
    public Vector3 InputDirection; //Vector3 from another script that gives the direction the character will move in 
    [SerializeField] private float gravity = 9.8f; // intensity of gravity
    private CharacterController _characterController;

    private void Start()
    {
	  _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
	  Vector3 gravityVector = Vector3.zero;
	  if(!_characterController.isGrounded)
	  {
		gravityVector = Vector3.down * gravity; //move the player down if they are not on the ground
	  }

	  Vector3 finalMovement = ((InputDirection * characterSpeed) + gravityVector); // combines all factors

	  _characterController.Move(finalMovement * Time.deltaTime); // moves the player
    }
	  
}
