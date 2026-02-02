using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonHandler : MonoBehaviour
{
    [SerializeField] private GameObject confirmQuitPanel;

    public void OnPlayButtonClick(string scene)
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    public void OnQuitButtonClick(string name)
    {
        switch(name)
        {
            case "Quit":
            {
                confirmQuitPanel.SetActive(true);
                break;
            }
            case "Cancel":
            {
                confirmQuitPanel.SetActive(false);
                break;
            }
            case "Confirm":
            {
                Application.Quit();
                break;
            }
            default:
            {
                break;
            }
        }
    }
}
