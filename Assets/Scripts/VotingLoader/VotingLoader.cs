using System;
using UnityEngine;

public class VotingLoader : MonoBehaviour
{
    public static VotingLoader Instance;

    [SerializeField] private GameObject loaderAnimationPanel;
    [SerializeField] private float animationDuration = 2f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void Show(Action onComplete)
    {
        StartCoroutine(PlayLoader(onComplete));
    }

    private System.Collections.IEnumerator PlayLoader(Action onComplete)
    {
        loaderAnimationPanel.SetActive(true);
        yield return new WaitForSeconds(animationDuration);
        loaderAnimationPanel.SetActive(false);

        // Restart time manager ticking

        onComplete?.Invoke();
    }
}
