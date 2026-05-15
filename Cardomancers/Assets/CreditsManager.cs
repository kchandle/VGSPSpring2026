using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CreditsManager : MonoBehaviour
{
    public float scrollSpeed = 1f;
    public GameObject creditsMoveParent;
    public CanvasGroup logoAlpha;
    public CanvasGroup backgroundAlpha;

    public bool moveCredits = false;

    private void Start()
    {
        StartCoroutine(OnEnterCredits());
    }

    IEnumerator OnEnterCredits()
    {
        while (Mathf.Abs(1f - logoAlpha.alpha) > 0.1f)
        {
            logoAlpha.alpha += 0.15f * Time.deltaTime;
            yield return null;
        }

        logoAlpha.alpha = 1f;
        moveCredits = true;

        while (Mathf.Abs(1f - backgroundAlpha.alpha) > 0.025)
        {
            backgroundAlpha.alpha = Mathf.Lerp(backgroundAlpha.alpha, 1f, Time.deltaTime * 0.1f);
            yield return null;
        }

        backgroundAlpha.alpha = 1f;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("NewMainMenu");
        }

        if (!moveCredits) return;

        scrollSpeed = Input.GetKey(KeyCode.Space) ? 5f : 1f;

        if (creditsMoveParent.transform.position.y < 12900)
            creditsMoveParent.transform.position = new Vector3(creditsMoveParent.transform.position.x, creditsMoveParent.transform.position.y + 100f * scrollSpeed * Time.deltaTime, creditsMoveParent.transform.position.z);
        else
        {
            print(creditsMoveParent.transform.position.y);
            StartCoroutine(WaitSecondsToTitleScreen(3f));
        }
    }

    public IEnumerator WaitSecondsToTitleScreen(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("NewMainMenu");
    }
}
