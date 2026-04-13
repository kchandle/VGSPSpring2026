using UnityEngine;

public class WinLoseMusic : MonoBehaviour
{
    ChangeMusic changeMusic;
    BattleManager battleManager;
    [SerializeField] AudioClip Winmusic;
    [SerializeField] AudioClip loseMusic;

    public void Awake()
    {
        changeMusic = GetComponent<ChangeMusic>(); 
        battleManager = GetComponent<BattleManager>();
    }

   public void PlayMusicAtEndOfBattle()
    {
        if(battleManager.endState == BattleManager.EndState.LOSE)
        {
            changeMusic.ChangeMusicMethod(loseMusic);
        }
        else if(battleManager.endState == BattleManager.EndState.WIN)
        {
            changeMusic.ChangeMusicMethod(Winmusic);
        }
    }
}
