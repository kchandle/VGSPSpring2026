using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Video;



// Ensure the correct namespace or assembly reference for BattleSystem is included  
// Example: using YourNamespace;  



public class StartBattle : MonoBehaviour

{
    public float timer = 2f;
    public GameObject canvas;
    public VideoPlayer videoPlayer;

    public Battle_SO battleToStart;

    public GameObject battleManagerPrefab; // Assign the BattleManager prefab in the inspector

    public static event Action OnPlayVideo;

    // The only reason this exists is to test the battle system quickly  

    public void StartBattleNow()
    {
        StartCoroutine(_Impl_StartBattleNow());
    }

    IEnumerator DisableTransition(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canvas.SetActive(false);
    }

    private IEnumerator _Impl_StartBattleNow()
    {
        GameStateScript.CurrentState = GameStateScript.GameState.BATTLE;
        // Updated to use the recommended method for finding objects
        // Ensure the BattleManager prefab is instantiated in the scene
        var battleSystem = FindFirstObjectByType<BattleManager>();
        if (battleSystem == null)
        {
            Instantiate(battleManagerPrefab);
            battleSystem = FindFirstObjectByType<BattleManager>();
        }
        battleSystem.StartBattle(battleToStart);

        // Prepares video, plays it, and sets the battle transition to unactive
        if (videoPlayer != null)
        {
            while (!videoPlayer.isPrepared)
            {   yield return null;
            }

            videoPlayer.Play(); // Starts playing the video
            canvas.SetActive(true);

            while(videoPlayer.isPlaying)
            {
                yield return null;
            }
            canvas.SetActive(false);
        }

        yield break;
    }


    private void Start()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        //Once the battle starts
        videoPlayer.prepareCompleted += OnVideoPrepared;
        videoPlayer.playOnAwake = false;
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.Prepare();
    }
    private void OnVideoPrepared(VideoPlayer vp) //Debugging
    {
        Debug.Log("Video prepared successfully.");
    }
}


