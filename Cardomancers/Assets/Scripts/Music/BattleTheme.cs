using UnityEngine;


public class BattleTheme : MonoBehaviour
{
   public AudioClip battleTheme;
    public AudioClip battleTheme2;
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
        audioSource.volume = 0.067f;
        audioSource.clip = wandDistrictTheme;
        audioSource.volume = 0.05f;
        this.audioSource.Play();
    }
    void OnStateChange(GameStateScript.GameState state)
    {
        int randomInt = Random.Range(1, 3);

        if (state == soundState) return;
        if (state == GameStateScript.GameState.SPEAKING || state == GameStateScript.GameState.INVENTORY) return;
        soundState = state;

        if (state == GameStateScript.GameState.BATTLE)
        {
            

            if(randomInt == 1) 
            {
                audioSource.clip = battleTheme;
                audioSource.volume = 0.425f;
            }
            else
            {
                audioSource.clip = battleTheme2;
                audioSource.volume = 0.175f;
            }

            this.audioSource.Play();
        }
        else if(state == GameStateScript.GameState.WALKING)
        {
            audioSource.volume = 0.067f;
            audioSource.clip = wandDistrictTheme;
            this.audioSource.Play();
        }
    }
}
