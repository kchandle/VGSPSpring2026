using UnityEngine;

// Base class for draggable, interactable objects like cards.

[RequireComponent(typeof(BoxCollider2D))]
public class PlayItem : MonoBehaviour

{
    
    // How quickly to move this GameObject
    public float moveSpeed = 15f;
    // Desired position to move to
    public Vector3 position;
    // Additional (optional) offset from position
    public Vector3 offset;
    [SerializeField] private BoxCollider2D boxCollider;

    // scaling
    void Awake()
    {
        //transform.localScale = new Vector3(1, 1, 1);
        //transform.position = new Vector3(0,0,0);
        boxCollider = GetComponent<BoxCollider2D>();
        
        boxCollider.isTrigger = true;
    }
    void Start(){
        position = transform.position;
    } 

    // Move the PlayItem towards its target position at all times
    void Update()
    {
        Vector3 targetPosition = position + offset;
        //targetPosition.z = 0f;
        float distance = (targetPosition - transform.position).magnitude;
        float moveDistance = moveSpeed * distance * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveDistance);
    }
}
