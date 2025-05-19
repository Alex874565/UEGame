using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VotingLoader : MonoBehaviour
{
    public static VotingLoader Instance;

    [SerializeField] private GameObject iconPopup;

    [SerializeField] private Animator loaderAnimator;
    [SerializeField] private GameObject loaderAnimationPanel;
    [SerializeField] private GameObject consequencePanel;
    [SerializeField] private float animationDuration = 2f;

    [SerializeField] private TextMeshProUGUI yourChoice;
    [SerializeField] private TextMeshProUGUI EUChoice;
    [SerializeField] private TextMeshProUGUI yourConsequences;
    [SerializeField] private TextMeshProUGUI EUConsequences;
    [SerializeField] private Button consequenceBtn;


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
        loaderAnimationPanel.SetActive(true);
        loaderAnimator.Play("QuizPopup");
        yield return new WaitForSeconds(animationDuration);
        loaderAnimationPanel.SetActive(false);

        yourChoice.text = eventData.choices[answerIndex].title;
        yourConsequences.text = eventData.choices[answerIndex].consequences;

        EUChoice.text = eventData.EuChoice;
        EUConsequences.text = eventData.EuChoiceDescription;

        consequencePanel.SetActive(true);

        consequenceBtn.onClick.RemoveAllListeners();
        consequenceBtn.onClick.AddListener(
            () => SpawnPopup(eventData, answerIndex)); 
    }

    private void SpawnPopup(EventsDatabase.Event eventData, int answerIndex)
    {
        GameObject popup = 
            EventsManager.Instance.SpawnPopup(iconPopup, eventData.countryName);
        popup.GetComponent<Image>().sprite = eventData.choices[answerIndex].eventIcon;

        TimeManager.Instance.SetTimeScale(TimeManager.Instance.DefaultTimeScale);
        TimeManager.Instance.ResetButtonStates();

        consequencePanel.SetActive(false);
    }
}
