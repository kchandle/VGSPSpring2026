using UnityEngine;

public class RandomBattleMusic : MonoBehaviour
{
    public AudioClip[] battleThemes;

    public void PlayWhenBattling()
    {
        int randomInt = Random.Range(0, battleThemes.Length);

        this.ChangeMusicMethod(battleThemes[randomInt]);
    }

    protected private void ChangeMusicMethod(AudioClip clip)
    {
        MusicPlayer musicPlayer = FindFirstObjectByType<MusicPlayer>();
        if (musicPlayer == null) return;
        if (!musicPlayer.Clips.Contains(clip)) musicPlayer.Clips.Add(clip);
        musicPlayer.Pause();

        while (musicPlayer.AudioIndex != musicPlayer.Clips.IndexOf(clip))
        {
            musicPlayer.Next();
        }
        musicPlayer.Play();
    }
}
