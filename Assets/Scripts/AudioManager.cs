using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource audioSourcePopupAppear;
    [SerializeField] private AudioSource audioSourcePopupClick;
    [SerializeField] private AudioSource audioSourcePoof;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    public void PlaySound()
    {
        audioSource.Play();
    }
    public void PlaySoundPopupAppear()
    {
        audioSourcePopupAppear.Play();
    }
    public void PlaySoundPopupClick()
    {
        audioSourcePopupClick.Play();
    }
    public void PlaySoundPoof()
    {
        audioSourcePoof.Play();
    }
}
