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
    public float videoTimer = 1f;
    public GameObject canvas;
    public VideoPlayer videoPlayer;

    public Battle_SO battleToStart;

    public GameObject battleManagerPrefab; // Assign the BattleManager prefab in the inspector

    public static event Action OnPlayVideo;

    public bool battleStarted = false;

    public DialogueScripts.DialogueSO altText;
    

    // The only reason this exists is to test the battle system quickly  

    public void StartBattleNow()
    {
        if (Inventory.Deck.Count <= 1)
        {
            //play alt text
            GameObject.Find("DialogueScreen").GetComponent<DialogueScripts.DialogueManager>().StartDialogue(altText);
            //print("Help");
            return ;
        } 

        if (Inventory.Deck.Count == 0 && Inventory.InventoryList.Count > 0)
        {
            //print("Add some cards to your deck and come back.");
            FindFirstObjectByType<PlayerInteract>().interacting = false;
            return;
        }

        if (Inventory.InventoryList.Count == 0)
        {
            //print("Pick up some cards and add them to your deck. Then come back.");
            return;
        }

        StartCoroutine(_Impl_StartBattleNow());
    }

    public  void SetObjectives(BattleManager battleManager)
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Objective"))
        {
            o.GetComponent<Objective>().SetBattleManager(battleManager);
            print("done");
        }
    }

    IEnumerator DisableTransition(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canvas.SetActive(false);
    }

    private IEnumerator _Impl_StartBattleNow()
    {        
         // Prepares video, plays it, and sets the battle transition to unactive
        if (videoPlayer != null)
        {
            while (!videoPlayer.isPrepared)
            {   
                yield return null;
            }

            videoPlayer.Play(); // Starts playing the video
            canvas.SetActive(true);

      
        
            while(videoPlayer.isPlaying)
            {
                yield return new WaitForSeconds(videoTimer);
                if(!battleStarted){
                
                GameStateScript.CurrentState = GameStateScript.GameState.BATTLE;
                // Updated to use the recommended method for finding objects
                // Ensure the BattleManager prefab is instantiated in the scene
                BattleManager battleSystem = FindFirstObjectByType<BattleManager>();
                if (battleSystem == null)
                {
                    Instantiate(battleManagerPrefab);

                    battleSystem = FindFirstObjectByType<BattleManager>();
                    battleSystem.startBattle = this;
                    SetObjectives(battleSystem);
                }
                battleSystem.StartBattle(battleToStart);
                battleStarted = true;
        

                    

                }
              
                yield return null;
            }

            if (canvas == null)
            {
                canvas = GameObject.FindWithTag("BattleManager").transform.GetChild(2).gameObject;
            }

            GameObject.FindWithTag("Player").gameObject.GetComponent<PlayerInteract>().interacting = false;
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
        // Debug.Log("Video prepared successfully.");
    }
}


