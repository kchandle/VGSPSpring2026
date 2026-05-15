using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    public float scrollSpeed = 1f;

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("NewMainMenu");
        }
    }
}
