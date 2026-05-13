using UnityEngine;


public class BattleTheme : MonoBehaviour
{
    //public AudioClip battleTheme;
    //public AudioClip battleTheme2;
    public AudioClip wandDistrictTheme;
    //public AudioClip wandDistrictThemeV2;
    //public AudioClip cardShopTheme;

    AudioSource audioSource;

    GameStateScript.GameState soundState;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        MixManager.AudioSourceAssignMixerGroup(this.audioSource, "Music");
        //GameStateScript.OnGameStateChanged += OnStateChange;
    }



    private void Start()
    {
        audioSource.volume = 0.067f;
        audioSource.clip = wandDistrictTheme;
        this.audioSource.Play();


    }
    //    void OnStateChange(GameStateScript.GameState state)
    //    {
    //        int randomInt = Random.Range(1, 3);


    //        if (state == soundState) return;
    //        if (state == GameStateScript.GameState.SPEAKING || state == GameStateScript.GameState.INVENTORY) return;
    //        soundState = state;

    //        if (state == GameStateScript.GameState.BATTLE)
    //        {


    //            if(randomInt == 1) 
    //            {
    //                audioSource.clip = battleTheme;
    //                audioSource.volume = 0.425f;
    //            }
    //            else
    //            {
    //                audioSource.clip = battleTheme2;
    //                audioSource.volume = 0.175f;
    //            }

    //            this.audioSource.Play();
    //        }
    //        else if(state == GameStateScript.GameState.WALKING)
    //        {
    //            if (randomInt == 1) {
    //                audioSource.volume = 0.067f;
    //                audioSource.clip = wandDistrictTheme;
    //                this.audioSource.Play();
    //            }
    //            else
    //            {
    //                audioSource.volume = 0.067f;
    //                audioSource.clip = wandDistrictThemeV2;
    //                this.audioSource.Play();
    //            }
    //        }
    //  }
}
