using UnityEditor.Search;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager Instance;

    //an empty gameobject with the audio source component to instantiate to play audioclips
    [SerializeField] private AudioSource soundFXEmpty;

    //setup the single static instance
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
    }

    //plays a sound effect clip with a spawn position and volume from 0f to 1f
    public void PlaySoundFXClip(AudioClip audioClip, Transform spawn, float volume)
    {
        //instances the audiosource prefab empty and gets the audio source from it 
        AudioSource audioSource = Instantiate(soundFXEmpty, spawn.position, Quaternion.identity);

        //sets the audio clip in the source and volume and then plays it 
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        //gets the time length of the clip in seconds and destorys it in that amount of time
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    //overload for the method above but uses a random clip from an array instead
    public void PlaySoundFXClip(AudioClip[] audioClips, Transform spawn, float volume)
    {
        //instances the audiosource prefab empty and gets the audio source from it 
        AudioSource audioSource = Instantiate(soundFXEmpty, spawn.position, Quaternion.identity);

        //gets a random int between 0 and the amount of clips in the array
        int randomIndex = Random.Range(0, audioClips.Length);

        //sets the audio clip to the random index in the audiosource and sets volume and then plays it 
        audioSource.clip = audioClips[randomIndex];
        audioSource.volume = volume;
        audioSource.Play();

        //gets the time length of the clip in seconds and destorys it in that amount of time
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }
}