using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using DG.Tweening;
using UnityEngine.Serialization;

public class MainMenuIndex : MonoBehaviour
{
    private Button startGameButton;
    private Button quitGameButton;
    private Button creditsButton;   
    private Button settingsButton;
    private VisualElement mainMenu;
    private VisualElement credits;
    private VisualElement settings;
    private VisualElement background;

    [Tooltip("Larger numbers = slower animation.")]
    [SerializeField] private float animationDurationSeconds = 5f;
    private float xPos;
    
    [Header("Scene Indices")]
    [SerializeField] private int mainMenuIndex;
    [SerializeField] private int creditsIndex;
    [SerializeField] private int settingsIndex;

    private void Awake()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        startGameButton = root.Q<Button>("Play");
        quitGameButton = root.Q<Button>("Quit");
        creditsButton = root.Q<Button>("Credits");
        settingsButton = root.Q<Button>("Settings");
        
        background = root.Q<VisualElement>("Background");
        
        
        startGameButton.RegisterCallback<ClickEvent>(OnStartGameClick);
        quitGameButton.RegisterCallback<ClickEvent>(OnQuitGameClick);
        creditsButton.RegisterCallback<ClickEvent>(OnCreditsClick);
        settingsButton.RegisterCallback<ClickEvent>(OnSettingsClick);
    }

    private void Start()
    {
        DOTween.Init();

        xPos = background.worldBound.x;
        
        DOTween.To(() => xPos, x=> xPos = x, Camera.main.pixelWidth, animationDurationSeconds).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    }

    private void Update()
    {
        background.style.backgroundPositionX = new StyleBackgroundPosition(new BackgroundPosition(BackgroundPositionKeyword.Left, xPos));
    }
    

    private void OnStartGameClick(ClickEvent clickEvent)
    {
        SceneManager.LoadScene(1);
        Debug.Log("Start Game");
    }

    private void OnQuitGameClick(ClickEvent clickEvent)
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    private void OnCreditsClick(ClickEvent clickEvent)
    {
        // Load credits scene
        if (SceneManager.GetSceneAt(creditsIndex).IsValid())
        {
            SceneManager.LoadScene(creditsIndex);
        }
        else
        {
            Debug.Log("Credits scene build index is incorrect or the scene is not in scene list.");
        }
        Debug.Log("Credits");
    }

    private void OnSettingsClick(ClickEvent clickEvent)
    {
        // Load settings scene
        if (SceneManager.GetSceneAt(settingsIndex).IsValid())
        {
            SceneManager.LoadScene(settingsIndex);
        }
        else
        {
            Debug.Log("Settings scene build index is incorrect or the scene is not in the scene list.");
        }
        Debug.Log("Settings");
    }
    
}
