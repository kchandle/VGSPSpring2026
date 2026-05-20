using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadCredits : MonoBehaviour
{
    [SerializeField] private int creditsIndex;
    
    public void LoadCreditsScene()
    {
        SceneManager.LoadScene(creditsIndex);
    }
}
