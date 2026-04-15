using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
        if (Instance == null)
        {   Instance = this;
        }
        else
        {   Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this);

        canvasGroup = GetComponent<CanvasGroup>();
    }

    private IEnumerator WaitForSceneToFinishLoading(SceneLoaderAsync async)
    {
        bool isready = false;
        do
        {
            StartCoroutine(LoadingScreen.Instance.UpdateLoadingBar(async.LoadProgress / 100f));

            while (LoadingScreen.Instance.updating)
            {   
                Debug.Log(async.LoadProgress);
                yield return null;
            }

            if(async.IsReady())
            {
                if (!isready)
                {
                    FindFirstObjectByType<PlayerInput>().gameObject.SetActive(false);
                    SceneLoader.Load(async);
                    isready = true;
                }
            }

        } while(!async.IsLoaded());

        while(LoadingScreen.Instance.updating)
        {   yield return null;
        }

        StartCoroutine(LoadingScreen.Instance.UpdateLoadingBar(1f));
    }

    public Coroutine alphaCor;
    public IEnumerator UpdateLoadingBar(float fillPercent)
    {   

            float fillMult = 6.7f;
        GameStateScript.CurrentState = GameStateScript.GameState.LOADINGSCREEN;

        updating = true;

        while(Mathf.Abs(loadBar.fillAmount - fillPercent) > 0.01)
        {
            loadBar.fillAmount = Mathf.Lerp(loadBar.fillAmount, fillPercent, Time.deltaTime * fillMult);
            yield return null;
        }

        loadBar.fillAmount = fillPercent;

        yield return null;

        if (loadBar.fillAmount == 1f)
        {
            alphaCor = StartCoroutine(ChangeAlpha(0f));
        }

        updating = false;
        GameStateScript.CurrentState = GameStateScript.GameState.WALKING;
    }

    public IEnumerator ChangeAlpha(float alpha)
    {
        float fillMult = 6.7f;

        while (Mathf.Abs(canvasGroup.alpha - alpha) > 0.01)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, alpha, Time.deltaTime * fillMult);
            yield return null;
        }
        canvasGroup.alpha = alpha;
    }

    public void ResetLoadingScreen()
    {
        canvasGroup.alpha = 1f;
        loadBar.fillAmount = 0f;
    }

    public void LoadNewScene(string scene)
    {
        LoadingScreen.Instance.ResetLoadingScreen();
        StartCoroutine(WaitForSceneToFinishLoading(SceneLoader.PreLoad(scene)));
    }
}