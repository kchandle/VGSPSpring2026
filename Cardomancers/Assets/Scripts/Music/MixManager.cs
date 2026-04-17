
/* Author: DerjenigeUberMensch
 *
 * Contact Group 1 For help or questions relating to this script.
 */
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine.Audio;

public static class MixManager
{
    private static AudioMixer mixer = null;             // The main audio mixer
    private static AudioMixerGroup musicGroup = null;   // music group mixer.
    private static AudioMixerGroup sfxGroup = null;     // sfx group mixer.

    // Intialize music mixer.
    static MixManager()
    {
        if(mixer != null)
        {   return;
        }

        mixer = Resources.Load<AudioMixer>("Audio/Mixers/MainMixer");

        Debug.Assert(mixer != null, "MainMixer not found in Resources/Audio/Mixers. Please ensure it is placed there.");

        musicGroup = mixer.FindMatchingGroups("Music")[0];
        sfxGroup = mixer.FindMatchingGroups("SFX")[0];

        /* make things happen */
        Application.runInBackground = true;
    }

    // Convert linear volume 0 -> 1 to decibels for mixer to understand.
    private static float ToDb(float volume)
    {   
        // smooth out to actual human hearing
        float linear = Mathf.Pow(volume, 2.0f);

        return Mathf.Log10(Mathf.Max(linear, 0.000001f)) * 20f;
    }

    // Set the master volume for the mixer.
    public static void SetMasterVolume(float volume)
    {
        mixer.SetFloat("MasterVolume", ToDb(volume));
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    // Set the music volume for the mixer.
    public static void SetMusicVolume(float volume)
    {
        mixer.SetFloat("MusicVolume", ToDb(volume));
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    // Set the sfx volume for the mixer.
    public static void SetSFXVolume(float volume)
    {
        mixer.SetFloat("SFXVolume", ToDb(volume));
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    // Assign an AudioSource to a mixer group by name.
    public static void AudioSourceAssignMixerGroup(AudioSource source, string groupName)
    {
        AudioMixerGroup[] groups = mixer.FindMatchingGroups(groupName);

        if (groups.Length > 0)
        {   source.outputAudioMixerGroup = groups[0];
        }
        else
        {   Debug.LogWarning($"AudioMixerGroup '{groupName}' not found in MainMixer.");
        }
    }
}