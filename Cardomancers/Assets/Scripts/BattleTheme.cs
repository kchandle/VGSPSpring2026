using UnityEngine;


public class BattleTheme : MonoBehaviour
{
   public AudioClip battleTheme;
    public AudioClip wandDistrictTheme;
    AudioSource audioSource;

    GameStateScript.GameState soundState;
     void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        GameStateScript.OnGameStateChanged += OnStateChange;
    }

    private void Start()
    {
        audioSource.clip = wandDistrictTheme;
        this.audioSource.Play();
    }
    void OnStateChange(GameStateScript.GameState state)
    {
        if (state == soundState) return;
        if (state == GameStateScript.GameState.SPEAKING || state == GameStateScript.GameState.INVENTORY) return;
        soundState = state;

        if (state == GameStateScript.GameState.BATTLE)
        {
            audioSource.clip = battleTheme;
            this.audioSource.Play();
        }
        else if(state == GameStateScript.GameState.WALKING)
        {
            audioSource.clip = wandDistrictTheme;
            this.audioSource.Play();
        }
    }
}
