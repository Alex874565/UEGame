using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource audioSource;
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    public void PlaySound()
    {
        audioSource.Play();
    }
}
