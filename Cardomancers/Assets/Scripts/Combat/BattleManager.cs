using System;
using UnityEngine;
using UnityEngine.Events;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance; // singleton instance

    #region Cameras
    public Camera battleCamera; // the camera used during battles
    public Camera mainCamera; // the main camera used outside of battles
    #endregion

    #region Public Events
    public UnityEvent OnBattleStart; // event triggered when a battle starts
    public UnityEvent OnLose; // event triggered when the player loses a battle
    public UnityEvent OnWin; // event triggered when the player wins a battle
    public UnityEvent PlayerTurn; // event triggered at the start of the player's turn
    public UnityEvent EnemyTurn; // event triggered at the start of the enemy's turn
    #endregion

    //public Battle_SO battle; // current battle SO passed in when battlestart is called


    private void Awake()
    {
        battleCamera.GetComponent<Camera>();
        mainCamera.GetComponent<Camera>();

        // Check if an instance already exists
        if (instance != null && instance != this)
        {
            // If so, destroy this new object to ensure only one instance remains
            Destroy(this.gameObject);
            return;
        }

        // Otherwise, set the instance to this object
        instance = this;

        // Optional: Keep the object alive when loading new scenes
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        OnBattleStart.AddListener(() => Debug.Log("Battle Started!"));
        OnLose.AddListener(() => Debug.Log("You Lose!"));
        OnWin.AddListener(() => Debug.Log("You Win!"));
        PlayerTurn.AddListener(() => Debug.Log("Player's Turn"));
        EnemyTurn.AddListener(() => Debug.Log("Enemy's Turn"));
    }


    //public void StartBattle(Battle_SO battle)
    //{

    //}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
