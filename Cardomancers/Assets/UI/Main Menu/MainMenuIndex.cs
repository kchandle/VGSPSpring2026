using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using DG.Tweening;

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

    private float xPos;

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
        DOTween.To(() => xPos, x=> xPos = x, 1207, 5 ).SetLoops(-1, LoopType.Incremental);
        
    }

    private void Update()
    {
        background.style.position.x = xPos;
    }
    

    private void OnStartGameClick(ClickEvent clickEvent)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
        Debug.Log("Credits");
    }

    private void OnSettingsClick(ClickEvent clickEvent)
    {
        // Load settings scene
        Debug.Log("Settings");
    }
    
}
