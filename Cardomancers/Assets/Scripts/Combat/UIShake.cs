using UnityEngine;
using System.Collections;

public class UIShake : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector2 originalPos;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPos = rectTransform.anchoredPosition;
    }

    public void Shake(float duration, float magnitude)
    {
        StopCoroutine(ShakeRoutine(duration, magnitude));
        StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            Vector2 offset = Random.insideUnitCircle * magnitude;
            rectTransform.anchoredPosition = originalPos + offset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = originalPos;
    }
}