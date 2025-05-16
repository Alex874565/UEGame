using System;
using TMPro;
using UnityEngine;

public class VotingLoader : MonoBehaviour
{
    public static VotingLoader Instance;

    [SerializeField] private GameObject loaderAnimationPanel;
    [SerializeField] private float animationDuration = 2f;

    [SerializeField] private TextMeshProUGUI yourChoice;
    [SerializeField] private TextMeshProUGUI EUChoice;
    [SerializeField] private TextMeshProUGUI yourConsequences;
    [SerializeField] private TextMeshProUGUI EUConsequences;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void Show(EventsDatabase.Event eventData, int answerIndex)
    {
        StartCoroutine(PlayLoader(eventData, answerIndex));
    }

    private System.Collections.IEnumerator PlayLoader(EventsDatabase.Event eventData, int answerIndex)
    {
        yourChoice.text = eventData.choices[answerIndex].title;
        yourConsequences.text = eventData.choices[answerIndex].consequences;

        EUChoice.text = eventData.EuChoice;
        EUConsequences.text = eventData.EuChoiceDescription;

        loaderAnimationPanel.SetActive(true);
        yield return new WaitForSeconds(animationDuration);
        loaderAnimationPanel.SetActive(false);

        // HARDCODED
        TimeManager.Instance.SetTimeScale(5f);
        TimeManager.Instance.EnableTimeButtons();
    }
}
