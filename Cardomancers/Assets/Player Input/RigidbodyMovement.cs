using UnityEngine;
using UnityEngine.InputSystem;

public class RigidbodyMovement : MonoBehaviour
{
	[SerializeField] private Rigidbody _rigidbody;
	private Vector3 _moveDirection; // The direction the player is moving in

	[SerializeField] private InputActionReference WASDMovement; // reference to the wasd movement action

    // Update is called once per frame
    void Update()
    {
        _moveDirection = WASDMovement.action.ReadValue<Vector3>();
    }
}
