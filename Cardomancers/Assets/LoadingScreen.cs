using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen Instance;

    [HideInInspector] public bool loading = false;

    public GameObject loadingScreenCanvas;
    [SerializeField] Image loadBar;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);

        DontDestroyOnLoad(this);
    }

    [ContextMenu("TestLoad")]
    public void Test()
    {
        loading = true;
        StartCoroutine(UpdateLoadingBar(1f));
    }

    public IEnumerator UpdateLoadingBar(float fillPercent)
    {
        while (loading)
        {
            while(Mathf.Abs(loadBar.fillAmount - fillPercent) > 0.01)
            {
                loadBar.fillAmount = Mathf.Lerp(loadBar.fillAmount, fillPercent, 0.01f);
                yield return null;
            }
            loadBar.fillAmount = fillPercent;
            yield return null;
            if (loadBar.fillAmount == 1f) loading = false;
        }
        //fade out loading screen 
    }
}