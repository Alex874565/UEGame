using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VotingLoader : MonoBehaviour
{
    public static VotingLoader Instance;

    [SerializeField] private GameObject iconPopup;

    [Header("Animation")]
    [SerializeField] private Animator loaderAnimator;
    [SerializeField] private GameObject loaderAnimationPanel;
    [SerializeField] private float animationDuration = 2f;

    [Header("UI")]
    [SerializeField] private GameObject consequencePanel1;
    [SerializeField] private GameObject consequencePanel2;

    [SerializeField] private TextMeshProUGUI yourChoice;
    [SerializeField] private TextMeshProUGUI EUChoice1;
    [SerializeField] private TextMeshProUGUI EUChoice2;
    [SerializeField] private TextMeshProUGUI yourConsequences;
    [SerializeField] private TextMeshProUGUI EUConsequences1;
    [SerializeField] private TextMeshProUGUI EUConsequences2;
    [SerializeField] private Button consequenceBtn1;
    [SerializeField] private Button consequenceBtn2;
    [SerializeField] private GameObject resourcesCanvas;


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
        resourcesCanvas.SetActive(false);
        loaderAnimationPanel.SetActive(true);
        loaderAnimator.Play("Consequence");
        yield return new WaitForSeconds(animationDuration);
        loaderAnimationPanel.SetActive(false);

        if (eventData.choices.Count == 1)
        {
            EUChoice2.text = eventData.EuChoice;
            EUConsequences2.text = eventData.EuChoiceDescription;

            consequencePanel2.SetActive(true);
            consequenceBtn2.onClick.RemoveAllListeners();
            consequenceBtn2.onClick.AddListener(() => 
            { 
                resourcesCanvas.SetActive(true); 
                SpawnPopup(eventData, answerIndex);
                EventsManager.Instance.DestroyedEvent();
            });
        }
        else
        {
            yourChoice.text = eventData.choices[answerIndex].title;
            yourConsequences.text = eventData.choices[answerIndex].consequences;

            EUChoice1.text = eventData.EuChoice;
            EUConsequences1.text = eventData.EuChoiceDescription;

            consequencePanel1.SetActive(true);

            Debug.Log("Removing listeners from " + consequenceBtn1.name);
            consequenceBtn1.onClick.RemoveAllListeners();
            consequenceBtn1.onClick.AddListener(
                () => 
                { 
                    resourcesCanvas.SetActive(true); 
                    SpawnPopup(eventData, answerIndex);
                    EventsManager.Instance.DestroyedEvent();
                });
        }
    }

    private void SpawnPopup(EventsDatabase.Event eventData, int answerIndex)
    {
        GameObject popup = 
            EventsManager.Instance.SpawnPopup(iconPopup, eventData.countryName);
        popup.GetComponent<Image>().sprite = eventData.choices[answerIndex].eventIcon;

        TimeManager.Instance.PlayTime();

        consequencePanel1.SetActive(false);
        consequencePanel2.SetActive(false);
    }
}
