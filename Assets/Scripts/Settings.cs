using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider mainVolumeSlider;
    [SerializeField] private Slider SFXVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;

    #region Audio

    [Header("Audio")]
    [Space(5)]
    [Header("Audio Settings")]
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private AudioMixerGroup SFXMixer;
    [SerializeField] private AudioMixerGroup musicMixer;


    [SerializeField] private string mainMixerKey = "MainMixer";
    [SerializeField] private string SFXMixerKey = "SFXMixer";
    [SerializeField] private string musicMixerKey = "MusicMixer";

    private const string mainMixerParameterName = "MainVolume";
    private const string SFXMixerParameterName = "SFXVolume";
    private const string musicMixerParameterName = "MusicVolume";


    private float MainVolume
    {
        get
        {
            return PlayerPrefs.HasKey(mainMixerKey) ? PlayerPrefs.GetFloat(mainMixerKey) == 0.001f
                ? -80f : PlayerPrefs.GetFloat(mainMixerKey) : 1f;
        }
        set { PlayerPrefs.SetFloat(mainMixerKey, value); }
    }

    private float SFXVolume
    {
        get
        {
            return PlayerPrefs.HasKey(SFXMixerKey) ? PlayerPrefs.GetFloat(SFXMixerKey) == 0.001f
                ? -80f : PlayerPrefs.GetFloat(SFXMixerKey) : 1f;
        }
        set { PlayerPrefs.SetFloat(SFXMixerKey, value); }
    }

    private float MusicVolume
    {
        get
        {
            return PlayerPrefs.HasKey(musicMixerKey) ? PlayerPrefs.GetFloat(musicMixerKey) == 0.001f
                ? -80f : PlayerPrefs.GetFloat(musicMixerKey) : 1f;
        }
        set { PlayerPrefs.SetFloat(musicMixerKey, value); }
    }

    #endregion


    private void Awake()
    {
        ApplySettings();
    }

    private void ApplySettings()
    {
        mainVolumeSlider.value = MainVolume;
        SFXVolumeSlider.value = SFXVolume;
        musicVolumeSlider.value = MusicVolume;

        mainMixer.SetFloat(mainMixerParameterName, Mathf.Log10(PlayerPrefs.GetFloat(mainMixerParameterName)) * 20);
        mainMixer.SetFloat(SFXMixerParameterName, Mathf.Log10(PlayerPrefs.GetFloat(SFXMixerParameterName)) * 20);
        mainMixer.SetFloat(musicMixerParameterName, Mathf.Log10(PlayerPrefs.GetFloat(musicMixerParameterName)) * 20);
    }

    #region Sliders

    public void SetMainMixerVolume(float volumeValue)
    {
        MainVolume = volumeValue;
        mainMixer.SetFloat(mainMixerParameterName, Mathf.Log10(volumeValue) * 20);
    }

    public void SetSFXMixerVolume(float volumeValue)
    {
        SFXVolume = volumeValue;
        mainMixer.SetFloat(mainMixerParameterName, Mathf.Log10(volumeValue) * 20);
    }

    public void SetMusicMixerVolume(float volumeValue)
    {
        MusicVolume = volumeValue;
        mainMixer.SetFloat(mainMixerParameterName, Mathf.Log10(volumeValue) * 20);
    }

    # endregion
}