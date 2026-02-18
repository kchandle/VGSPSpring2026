using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterControllerMovement : MonoBehaviour
{
    public float walkSpeed = 15f;
	public float sprintSpeed = 30f;
	private float currentSpeed;

	//the bool changed in playercontroller for whether the player is sprinting or not 
    public bool sprinting;

    //Vector3 from another script that gives the direction the character will move in 
    public Vector3 inputDirectionInput;

    private Vector3 _moveDirection;

	public Animator animator;

    // intensity of gravity MUST be 9.8f so it is realistic
    [SerializeField] private float gravity = 9.8f; 
	[SerializeField] private float jumpIntensity = 4f;
	[SerializeField] private float maxFallSpeed = -30f;
	public bool jumpWasPressed;
	private bool _jumping;
	[HideInInspector] public float jumpMultiplier = 15f;

	//reference to the character controller component
    private CharacterController _characterController;

	//different audioclips for different actions
	[SerializeField] AudioClip[] footstepClips;
	[SerializeField] AudioClip[] jumpClips;
	[SerializeField] AudioClip[] jumpLandClips;
	
	//keeps track of if there is already a footstep sound so it doesnt overlap
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

		if(planarInput.x != 0 || planarInput.z != 0){
			// triggers Run animator
			animator.SetTrigger("Run");


		}

		//if the character controller is off the ground accelerate the player downward and cap the downward velocity
		if (!_characterController.isGrounded && _characterController.velocity.y > maxFallSpeed)
		{
			_moveDirection.y -= gravity * Time.deltaTime;
		}

		//if the jump key was pressed this frame then adds to the Y value of the moveDirection
		if (_characterController.isGrounded && jumpWasPressed)
		{
			_jumping = true;
			SoundEffectManager.Instance.PlaySoundFXClip(jumpClips, transform, 0.25f);
            _moveDirection.y = Mathf.Sqrt(jumpIntensity); 

			// triggers jump animation
			animator.SetTrigger("Jump");
		}

		//if the player was jumping and they became grounded on a frame where the jumpkey wasnt pressed then it plays a sound for the player landing
		if (_jumping == true && _characterController.isGrounded == true && !jumpWasPressed)
		{
			_jumping = false;
			SoundEffectManager.Instance.PlaySoundFXClip(jumpLandClips, transform, 0.05f);
		}

		//changes the current speed to the speed of either sprinting or walking depending on if youre sprinting or not
		currentSpeed = sprinting ? sprintSpeed : walkSpeed;

		//combines the y movement direction with the vector3.up planar input directions normalized and then multiply to the character speed
        Vector3 moveDirection = new Vector3(0f, _moveDirection.y * jumpMultiplier, 0f) + Vector3.Normalize(planarInput) * currentSpeed;

        //Movement based on the intended movement direction and the rotation of the player so that the movement is always in the direction the player is facing
        Vector3 finalMovement = transform.TransformDirection(moveDirection);

        //actaully moves the character controller with the direction set 
        _characterController.Move(finalMovement * Time.deltaTime);

		//if it isnt already playing footsteps and the player is moving and grounded than it plays a footstep 
		if (footstepSource == null && planarInput.magnitude > 0.1f && _characterController.isGrounded)
		{
			footstepSource = SoundEffectManager.Instance.PlaySoundFXClip(footstepClips, transform, 0.5f, 1.5f);
		}
		////if there is a soundplaying check if the player isnt ground or moving and then delete the sound
		else if (footstepSource != null)
		{
			if (!_characterController.isGrounded || planarInput.magnitude <= 0.1f) Destroy(footstepSource.gameObject);
		}

        //resets the jump bool that is set in the player controller script to true whenever space is pressed
        jumpWasPressed = false; 
    }
	
}