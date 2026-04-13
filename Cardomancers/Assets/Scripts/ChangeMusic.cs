using UnityEngine;

public class ChangeMusic : MonoBehaviour
{
    public void ChangeMusicMethod(AudioClip clip)
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