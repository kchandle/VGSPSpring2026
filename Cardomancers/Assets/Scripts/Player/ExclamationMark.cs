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

    private Canvas canvas;
    private float initialOffset;
    private bool isRising = true;
    private RectTransform rectTransform = null;

    void Start()
    {
        canvas = gameObject.GetComponentInChildren<Canvas>();

        Debug.Assert(canvas != null, "Canvas not assigned to ExclamationMark script.");

        rectTransform = canvas.GetComponent<RectTransform>();

        Debug.Assert(rectTransform != null, "RectTransform component not found on the assigned Canvas.");

        initialOffset = rectTransform.anchoredPosition.y;
    }

    bool IsBetween(float value, float min, float max)
    {   return value >= min && value <= max;
    }

    void Update()
    {
        if(canvas == null || rectTransform == null)
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

        //rectTransform.anchoredPosition = Vector2.Lerp(currentPosition, targetPosition, HoverSpeed * Time.deltaTime * HOVER_SPEED_MULTIPLIER);
        rectTransform.anchoredPosition = Vector2.MoveTowards(currentPosition, targetPosition, HoverSpeed * Time.deltaTime * HOVER_SPEED_MULTIPLIER);
        rectTransform.Rotate(0, RotationSpeed * RotationDirection * Time.deltaTime * ROTATION_SPEED_MULTIPLIER, 0);

        // floats can be tricky so we just estimate.
        if(IsBetween(currentPosition.y, targetPosition.y - 0.1f, targetPosition.y + 0.1f))
        {   isRising = !isRising;
        }
    }
}
