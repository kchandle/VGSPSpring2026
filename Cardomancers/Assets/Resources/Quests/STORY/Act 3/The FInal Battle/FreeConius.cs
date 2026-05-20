using UnityEngine;
using UnityEngine.SceneManagement;

public class FreeConius : MonoBehaviour
{
    private int numberOfYoungsters;
    [SerializeField] private int loseWith1YoungsterSceneIndex;
    [SerializeField] private int loseWith2YoungsterSceneIndex;
    [SerializeField] private int loseWith3YoungsterSceneIndex;
    [SerializeField] private int winWith1YoungsterSceneIndex;
    [SerializeField] private int winWith2YoungsterSceneIndex;
    [SerializeField] private int winWith3YoungsterSceneIndex;

    private void Awake()
    {
        numberOfYoungsters = Inventory.youngstersConvinced;
    }

    private void OnEnable()
    {
        switch (numberOfYoungsters)
        {
            case 1:
                BattleManager.instance.OnWin.AddListener(EndingOne);
                BattleManager.instance.OnLose.AddListener(EndingZero);
                break;
            case 2:
                BattleManager.instance.OnWin.AddListener(EndingTwo);
                BattleManager.instance.OnLose.AddListener(EndingFour);
                break;
            case 3:
                BattleManager.instance.OnWin.AddListener(EndingThree);
                BattleManager.instance.OnLose.AddListener(EndingFive);
                break;
        }
    }

    private void OnDisable()
    {
        switch (numberOfYoungsters)
        {
            case 1:
                BattleManager.instance.OnWin.RemoveListener(EndingOne);
                BattleManager.instance.OnLose.RemoveListener(EndingZero);
                break;
            case 2:
                BattleManager.instance.OnWin.RemoveListener(EndingTwo);
                BattleManager.instance.OnLose.RemoveListener(EndingFour);
                break;
            case 3:
                BattleManager.instance.OnWin.RemoveListener(EndingThree);
                BattleManager.instance.OnLose.RemoveListener(EndingFive);
                break;
        }  
    }

    // If the player loses the battle against reginald with only one youngster
    private void EndingZero()
    {
        SceneManager.LoadScene(loseWith1YoungsterSceneIndex);
    }

    // If the player defeats reginald with only one youngster
    private void EndingOne()
    {
        SceneManager.LoadScene(winWith1YoungsterSceneIndex);
    }

    // If the player defeats reginald with two youngsters
    private void EndingTwo()
    {
        SceneManager.LoadScene(winWith2YoungsterSceneIndex);
    }

    // Player defeats reginald with all three youngsters
    private void EndingThree()
    {
        SceneManager.LoadScene(winWith3YoungsterSceneIndex);
    }

    // Player loses to reginald with two youngsters
    private void EndingFour()
    {
        SceneManager.LoadScene(loseWith2YoungsterSceneIndex);
    }
    
    // Player loses to reginald with three youngsters
    private void EndingFive()
    {
        SceneManager.LoadScene(loseWith3YoungsterSceneIndex);
    }
}
