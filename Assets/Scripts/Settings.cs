using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider mainVolumeSlider;
    [SerializeField] private Slider SFXVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;

    [Header("Audio Settings")]
    [SerializeField] private AudioMixer mainMixer;

    [SerializeField] private float defaultMainVolume = .1f;
    [SerializeField] private float defaultSFXVolume = .4f;
    [SerializeField] private float defaultMusicVolume = .5f;

    [SerializeField] private string mainMixerKey = "MainVolume";
    [SerializeField] private string sfxMixerKey = "SFXVolume";
    [SerializeField] private string musicMixerKey = "MusicVolume";

    private void Awake()
    {
        ApplySettings();
    }

    private void ApplySettings()
    {
        float mainVol = PlayerPrefs.GetFloat(mainMixerKey, defaultMainVolume);
        float sfxVol = PlayerPrefs.GetFloat(sfxMixerKey, defaultSFXVolume);
        float musicVol = PlayerPrefs.GetFloat(musicMixerKey, defaultMusicVolume);

        mainVolumeSlider.value = mainVol;
        SFXVolumeSlider.value = sfxVol;
        musicVolumeSlider.value = musicVol;

        mainMixer.SetFloat(mainMixerKey, ToDecibel(mainVol));
        mainMixer.SetFloat(sfxMixerKey, ToDecibel(sfxVol));
        mainMixer.SetFloat(musicMixerKey, ToDecibel(musicVol));
    }

    public void SetMainMixerVolume(float value)
    {
        PlayerPrefs.SetFloat(mainMixerKey, value);
        mainMixer.SetFloat(mainMixerKey, ToDecibel(value));
    }

    public void SetSFXMixerVolume(float value)
    {
        PlayerPrefs.SetFloat(sfxMixerKey, value);
        mainMixer.SetFloat(sfxMixerKey, ToDecibel(value));
    }

    public void SetMusicMixerVolume(float value)
    {
        PlayerPrefs.SetFloat(musicMixerKey, value);
        mainMixer.SetFloat(musicMixerKey, ToDecibel(value));
    }

    private float ToDecibel(float value)
    {
        return Mathf.Approximately(value, 0f) ? -80f : Mathf.Log10(value) * 20f;
    }
}
