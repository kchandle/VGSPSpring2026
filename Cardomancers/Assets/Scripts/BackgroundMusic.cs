// Debugging 
#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusic : MonoBehaviour
{
    [Tooltip("The Background Music Audio Clip To Play.")]
    [SerializeField] public AudioClip audioClip = null;
    [Tooltip("How Loud You Want The Background Music Audio Clip Be.")]
    [Range(0f, 1f)]
    [SerializeField] public float Volume = 1.0f;

    // Allow other scripts to interface with this if they need too.
    [NonSerialized] public AudioSource audioSource;

    void Start()
    {
        // No audio clip to play so throw error msg.
        if(!this.audioClip)
        {   Debug.LogError("No audio clip found in Background Music Script.");
        }
        else
        {
            // This sets the settings for the AudioSource.

            // Even though this line is not necessary, we might have audio preloading in the future so its left here for compatibility reasons.
            this.audioClip.LoadAudioData();

            this.audioSource = GetComponent<AudioSource>();
            this.audioSource.volume = Volume;
            this.audioSource.clip = this.audioClip;
            this.audioSource.loop = true;
        }

        this.audioSource.Play();
    }

    void Update()
    {
        // If during testing we want to make it sound different this handles that.
        #if UNITY_EDITOR
            this.audioSource.volume = Volume;
        #endif
    }

    void OnDestroy()
    {   
        if(this.audioClip != null)
        {
            // stop playing on destroy
            this.audioClip.UnloadAudioData();
        }
        this.audioSource.Stop();
    }
}
