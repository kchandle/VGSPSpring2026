using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuButtonHandler : MonoBehaviour
{
    [SerializeField] private GameObject confirmQuitPanel;
    Button startGameButton;
    Button quitButton;
    [SerializeField] UIDocument uiDocument;

    private void Awake()
    {
        startGameButton = uiDocument.rootVisualElement.Q<Button>("StartGameButton");
        quitButton = uiDocument.rootVisualElement.Q<Button>("QuitGameButton");
        
        if (startGameButton == null)
            Debug.LogError("StartGameButton not found!");

        if (quitButton == null)
            Debug.LogError("QuitButton not found!"); 
    }

    private void OnEnable()
    {
        startGameButton.clicked += OnPlayButtonClick;
        quitButton.clicked += OnQuitButtonClick;
    }

    private void OnDisable()
    {
        startGameButton.clicked -= OnPlayButtonClick;
        quitButton.clicked -= OnQuitButtonClick;
        
    }

    private void OnPlayButtonClick(/*string scene*/)
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    private void OnQuitButtonClick()
    {
        Application.Quit();
    }
}
