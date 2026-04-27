using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuIndex : MonoBehaviour
{
    private Button startGameButton;
    private Button quitGameButton;
    private Button creditsButton;   
    private Button settingsButton;
    private VisualElement mainMenu;
    private VisualElement credits;
    private VisualElement settings;

    private void Awake()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        startGameButton = root.Q<Button>("Play");
        quitGameButton = root.Q<Button>("Quit");
        creditsButton = root.Q<Button>("Credits");
        settingsButton = root.Q<Button>("Settings");
        
        startGameButton.RegisterCallback<ClickEvent>(OnStartGameClick);
        quitGameButton.RegisterCallback<ClickEvent>(OnQuitGameClick);
        creditsButton.RegisterCallback<ClickEvent>(OnCreditsClick);
        settingsButton.RegisterCallback<ClickEvent>(OnSettingsClick);
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
