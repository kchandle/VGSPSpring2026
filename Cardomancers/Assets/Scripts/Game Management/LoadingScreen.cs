using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen Instance;

    public bool updating = false;

    public GameObject loadingScreenCanvas;
    private CanvasGroup canvasGroup;
    [SerializeField] Image loadBar;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);

        DontDestroyOnLoad(this);

        canvasGroup = GetComponent<CanvasGroup>();
    }

    public IEnumerator UpdateLoadingBar(float fillPercent)
    {
        updating = true;

        while(Mathf.Abs(loadBar.fillAmount - fillPercent) > 0.01)
        {
            loadBar.fillAmount = Mathf.Lerp(loadBar.fillAmount, fillPercent, 0.01f);
            yield return null;
        }
        loadBar.fillAmount = fillPercent;
        yield return null;
        if (loadBar.fillAmount == 1f) StartCoroutine(ChangeAlpha(0f));
        updating = false;
    }

    public IEnumerator ChangeAlpha(float alpha)
    {
        while (Mathf.Abs(canvasGroup.alpha - alpha) > 0.01)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, alpha, 0.1f);
            yield return null;
        }
        canvasGroup.alpha = alpha;
    }

    public void ResetLoadingScreen()
    {
        canvasGroup.alpha = 1f;
        loadBar.fillAmount = 0f;
    }
}