using UnityEngine;

public class BattleTheme : MonoBehaviour
{
   [SerializeField] AudioClip battleTheme;
    // Update is called once per frame
    void Update()
    {
        if (GameStateScript.CurrentState == GameStateScript.GameState.BATTLE)
        {
           
        }
    }
    
}
