using UnityEngine;

public class ExclamationMark : MonoBehaviour
{
    [Tooltip("How far up and down the exclamation mark should hover.")]
    public float OffsetRangeY = 0.25f;
    [Tooltip("How fast the exclamation mark should hover.")]
    public float HoverSpeed = 1f;
    [Tooltip("How fast the exclamation mark should rotate.")]
    public float RotationSpeed = 1f;
    [Tooltip("Whether the exclamation mark should rotate clockwise.")]
    public bool RotateClockwise = true;
    [Tooltip("The color the exclamation mark should change to when the player is interacting with the npc.")]
    public Color InteractingColor = Color.grey;
    [Tooltip("The speed at which the exclamation mark should change color when the player is interacting with the npc.")]
    public float ColorChangeSpeed = 1f;
    [Tooltip("The color the exclamation mark should change to when the player is in range to interact with the npc.")]
    public Color InRangeColor = Color.yellow;
    [Tooltip("The speed at which the exclamation mark should change color when the player is in range to interact with the npc.")]
    public float InRangeColorChangeSpeed = 1f;


    private GameObject player;
    private GameObject parent;
    private InteractableObject parentInteractable;
    private PlayerInteract playerInteract;
    private Canvas canvas;
    private float initialOffset;
    private bool isRising = true;
    private RectTransform rectTransform = null;
    private Color originalColor;
    private SpriteRenderer image;

    void Start()
    {
        canvas = gameObject.GetComponentInChildren<Canvas>();

        Debug.Assert(canvas != null, "Canvas not assigned to ExclamationMark script.");

        rectTransform = canvas.GetComponent<RectTransform>();

        Debug.Assert(rectTransform != null, "RectTransform component not found on the assigned Canvas.");

        image = canvas.GetComponentInChildren<SpriteRenderer>();

        Debug.Assert(image != null, "SpriteRenderer component not found on the assigned Canvas.");

        originalColor = image.color;
        player = GameObject.FindWithTag("Player");

        Debug.Assert(player != null, "Player GameObject with tag 'Player' not found in the scene.");

        playerInteract = player.GetComponent<PlayerInteract>();

        Debug.Assert(playerInteract != null, "PlayerInteract component not found on the Player GameObject.");

        parent = transform?.parent?.gameObject;
        parentInteractable = parent?.GetComponent<InteractableObject>();

        if(parentInteractable == null || parent == null)
        {   Debug.LogWarning("Parent of ExclamationMark does not have an InteractableObject Component. ExclamationMark will not be able to detect interactions.");
        }

        if(parent && parent.GetComponent<Collider>() == null)
        {   Debug.LogWarning("Parent of ExclamationMark does not have a Collider Component. Please add a Collider to the parent so that the ExclamationMark can detect interactions.");
        }

        initialOffset = rectTransform.anchoredPosition.y;
    }

    bool IsBetween(float value, float min, float max)
    {   return value >= min && value <= max;
    }

    void Update()
    {
        if(canvas == null || rectTransform == null || player == null || playerInteract == null)
        {   return;
        }

        Vector2 currentPosition = rectTransform.anchoredPosition;
        Vector2 targetPosition = new Vector2(rectTransform.anchoredPosition.x, initialOffset);
        Vector2 targetOffset = new Vector2(0f, OffsetRangeY);

        float RotationDirection = RotateClockwise ? -1f : 1f;

        // constants to make the speed settings more intuitive, these can be tweaked if needed.

        // Full rotation per second at RotationSpeed = 1
        float ROTATION_SPEED_MULTIPLIER = 360f; 
        // Full hover cycle per second at HoverSpeed = 1
        float HOVER_SPEED_MULTIPLIER = 2f;  // 2 times since one cycle is up and down.

        if(!isRising)
        {   targetOffset = -targetOffset;
        }

        targetPosition += targetOffset;

        if(playerInteract.interacting)
        {   
            float distance = Vector3.Distance(transform.position, player.transform.position);
            float interactRange = playerInteract.range;

            if(parentInteractable != null || playerInteract.currentHighlight != null)
            {
                if(parentInteractable == playerInteract.currentHighlight)
                {   
                    image.color = Color.Lerp(image.color, InteractingColor, ColorChangeSpeed * Time.deltaTime);
                }
            }
            else if(distance <= interactRange)
            {   
                image.color = Color.Lerp(image.color, InteractingColor, ColorChangeSpeed * Time.deltaTime);
            }
        }
        else
        {
            if(parentInteractable != null && playerInteract.currentHighlight == parentInteractable)
            {   
                image.color = Color.Lerp(image.color, InRangeColor, InRangeColorChangeSpeed * Time.deltaTime);
            }
            else if(parentInteractable == null && playerInteract.inRange)
            {   
                image.color = Color.Lerp(image.color, InRangeColor, InRangeColorChangeSpeed * Time.deltaTime);
            }
            else
            {   
                image.color = Color.Lerp(image.color, originalColor, ColorChangeSpeed * Time.deltaTime);
            }
        }

        rectTransform.anchoredPosition = Vector2.MoveTowards(currentPosition, targetPosition, HoverSpeed * Time.deltaTime * HOVER_SPEED_MULTIPLIER);
        rectTransform.Rotate(0, RotationSpeed * RotationDirection * Time.deltaTime * ROTATION_SPEED_MULTIPLIER, 0);

        // floats can be tricky so we just estimate.
        if(IsBetween(currentPosition.y, targetPosition.y - 0.1f, targetPosition.y + 0.1f))
        {   isRising = !isRising;
        }
    }
}
