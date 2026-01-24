using UnityEngine;

// Base class for draggable, interacteable objects like cards.

public class PlayItem : MonoBehaviour

{
    
    // How quickly to move this GameObject
    public float moveSpeed = 15f;
    // Desired position to move to
    public Vector3 position;
    // Additional (optional) offset from position
    public Vector3 offset;

    // scaling
    void Awake()
    {
        //transform.localScale = new Vector3(1, 1, 1);
    }
    void Start(){
        position = transform.position;
    }

    // Move the PlayItem towards its target position at all times
    void Update()
    {
        Vector3 targetPosition = position + offset;
        float distance = (targetPosition - transform.position).magnitude;
        float moveDistance = moveSpeed * distance * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveDistance);
    }
}
