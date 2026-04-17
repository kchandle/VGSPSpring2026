
/* Author: DerjenigeUberMensch
 *
 * Contact Group 1 For help or questions relating to this script.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Events;

public class PauseMenu : MonoBehaviour
{
    public GameObject musicSlider;
    public GameObject sfxSlider;

    void Awake()
    {
        Debug.Assert(musicSlider != null, "Music Slider is not assigned in the inspector.");
        Debug.Assert(sfxSlider != null, "SFX Slider is not assigned in the inspector.");
    }

    void Start()
    {
        musicSlider.GetComponent<UnityEngine.UI.Slider>().onValueChanged.AddListener(MusicSliderUpdate);
        sfxSlider.GetComponent<UnityEngine.UI.Slider>().onValueChanged.AddListener(SFXSliderUpdate);
    }

    void MusicSliderUpdate(float value)
    {
        float volume = value / 100f; // Convert from 0-100 to 0-1

        MixManager.SetMusicVolume(volume);
    }

    void SFXSliderUpdate(float value)
    {
        float volume = value / 100f; // Convert from 0-100 to 0-1

        MixManager.SetSFXVolume(volume);
    }

    public void OnEnable()
    {
        /* intialize settings */

        UnityEngine.UI.Slider music = musicSlider.GetComponent<UnityEngine.UI.Slider>();
        UnityEngine.UI.Slider sfx = sfxSlider.GetComponent<UnityEngine.UI.Slider>();

        music.onValueChanged.RemoveListener(MusicSliderUpdate);
        sfx.onValueChanged.RemoveListener(SFXSliderUpdate);

        music.value = PlayerPrefs.GetFloat("MusicVolume", 1f) * 100f;
        sfx.value = PlayerPrefs.GetFloat("SFXVolume", 1f) * 100f;

        music.onValueChanged.AddListener(MusicSliderUpdate);
        sfx.onValueChanged.AddListener(SFXSliderUpdate);
    }

}