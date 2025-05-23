using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI playText;
    [SerializeField] private Button deleteSaveButton;
    [SerializeField] private Button playButton;

    [SerializeField] private Slider mainVolumeSlider;
    [SerializeField] private Slider SFXVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;

    [Header("Volume Icons")]
    [SerializeField] private Image mainVolumeIcon;
    [SerializeField] private Image sfxVolumeIcon;
    [SerializeField] private Image musicVolumeIcon;

    [SerializeField] private Sprite muteIcon;
    [SerializeField] private Sprite lowIcon;
    [SerializeField] private Sprite midIcon;
    [SerializeField] private Sprite highIcon;

    [Header("Audio Settings")]
    [SerializeField] private AudioMixer mainMixer;

    [SerializeField] private float defaultMainVolume = .1f;
    [SerializeField] private float defaultSFXVolume = .4f;
    [SerializeField] private float defaultMusicVolume = .5f;

    [SerializeField] private string mainMixerKey = "MainVolume";
    [SerializeField] private string sfxMixerKey = "SFXVolume";
    [SerializeField] private string musicMixerKey = "MusicVolume";

    private string SavePath;

    private void Awake()
    {
        ApplySettings();
        SavePath = Path.Combine(Application.persistentDataPath, "save.dat");

        if(playText != null)
            playText.text = File.Exists(SavePath) ? "Continue" : "New Game";

        if(playButton != null)
        {
            if (File.Exists(SavePath))
            {
                playButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 170);
            }
            else
            {
                playButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 20);
            }
        }

        if(deleteSaveButton != null)
            deleteSaveButton.gameObject.SetActive(File.Exists(SavePath));
    }

    public void DeleteSave()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("Save file deleted.");
            playText.text = "New Game";
            playButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 20);
            deleteSaveButton.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("No save file to delete.");
        }
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

        UpdateVolumeIcon(mainVolumeIcon, mainVol);
        UpdateVolumeIcon(sfxVolumeIcon, sfxVol);
        UpdateVolumeIcon(musicVolumeIcon, musicVol);
    }

    public void SetMainMixerVolume(float value)
    {
        PlayerPrefs.SetFloat(mainMixerKey, value);
        mainMixer.SetFloat(mainMixerKey, ToDecibel(value));
        UpdateVolumeIcon(mainVolumeIcon, value);
    }

    public void SetSFXMixerVolume(float value)
    {
        PlayerPrefs.SetFloat(sfxMixerKey, value);
        mainMixer.SetFloat(sfxMixerKey, ToDecibel(value));
        UpdateVolumeIcon(sfxVolumeIcon, value);
    }

    public void SetMusicMixerVolume(float value)
    {
        PlayerPrefs.SetFloat(musicMixerKey, value);
        mainMixer.SetFloat(musicMixerKey, ToDecibel(value));
        UpdateVolumeIcon(musicVolumeIcon, value);
    }

    private float ToDecibel(float value)
    {
        return Mathf.Approximately(value, 0f) ? -80f : Mathf.Log10(value) * 20f;
    }

    private void UpdateVolumeIcon(Image icon, float value)
    {
        if (value <= 0.01f)
        {
            icon.sprite = muteIcon;
        }
        else if (value <= 0.3f)
        {
            icon.sprite = lowIcon;
        }
        else if (value <= 0.7f)
        {
            icon.sprite = midIcon;
        }
        else
        {
            icon.sprite = highIcon;
        }
    }
}
