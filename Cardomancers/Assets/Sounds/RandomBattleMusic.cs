using UnityEngine;

public class RandomBattleMusic : MonoBehaviour
{
    public AudioClip OneBattleTheme;
    public AudioClip TwoBattleTheme;

    ChangeMusic changeMusic;
    public void Awake()
    {
        changeMusic = new ChangeMusic();
    }

    public void PlayWhenBattling()
    {
        int randomInt = Random.Range(1, 3);

        if (randomInt == 1)
        {
            changeMusic.ChangeMusicMethod(OneBattleTheme);
        }
        else
        {
            changeMusic.ChangeMusicMethod(TwoBattleTheme);
        }
    }
}
