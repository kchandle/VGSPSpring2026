using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterControllerMovement : MonoBehaviour
{
    public float characterSpeed = 1f; 

	//Vector3 from another script that gives the direction the character will move in 
	public Vector3 inputDirectionInput;

    private Vector3 _moveDirection;

    // intensity of gravity MUST be 9.8f so it is realistic
    [SerializeField] private float gravity = 9.8f; 
	[SerializeField] private float jumpIntensity = 1f;
	public bool jumping;

    private CharacterController _characterController;

	[SerializeField] AudioClip[] footstepClips;
	[SerializeField] AudioClip[] jumpClips;
	private AudioSource footstepSource;

    private void Awake()
    {
		//Set the character controller reference automatically
		_characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
		//makes a vector3 with the movement input WASD 
		Vector3 planarInput = new Vector3(inputDirectionInput.x, 0f, inputDirectionInput.z);

		//if the character controller is off the ground accelerate the player downward
		if(!_characterController.isGrounded)
		{
            _moveDirection.y -= gravity * Time.deltaTime; 
		}

        //if the jump key was pressed this frame then adds to the Y value of the moveDirection
        if (_characterController.isGrounded && jumping)
		{
			SoundEffectManager.Instance.PlaySoundFXClip(jumpClips, transform, 1f);
            _moveDirection.y = Mathf.Sqrt(jumpIntensity * 2f * gravity); 
		}
		
		//combines the y movement direction with the vector3.up planar input directions normalized and then multiply to the character speed
        Vector3 finalMovement = (new Vector3(0f, _moveDirection.y, 0f) + Vector3.Normalize(planarInput)) * characterSpeed;

		//actaully moves the character controller with the direction set 
		_characterController.Move(finalMovement * Time.deltaTime);

		//if it isnt already playing footsteps and the player is moving and grounded than it plays a footstep 
		if (footstepSource == null && planarInput.magnitude > 0.1f && _characterController.isGrounded)
		{
			footstepSource = SoundEffectManager.Instance.PlaySoundFXClip(footstepClips, transform, 1f);
		}
		//if there is a soundplaying check if the player isnt ground or moving and then delete the sound
		else if (footstepSource != null)
		{
			if (!_characterController.isGrounded || planarInput.magnitude <= 0.1f) Destroy(footstepSource.gameObject);
		}

		//resets the jump bool that is set in the player controller script to true whenever space is pressed
		jumping = false; 
    }
	
}