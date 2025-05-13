using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    [Header("Lose Popup")]
    [SerializeField] private Animator losePopupAnimator;
    [SerializeField] private TextMeshProUGUI losePopupTextHeader;
    [SerializeField] private TextMeshProUGUI losePopupTextDescription;
    private int popup1AnimHash = Animator.StringToHash("Popup");

    [Header("Quiz UI")]
    [SerializeField] private Animator quizPopupAnimator;
    [SerializeField] private GameObject quizPanel;

    [SerializeField] private TextMeshProUGUI eventTitleText;
    [SerializeField] private TextMeshProUGUI eventDateText;
    [SerializeField] private TextMeshProUGUI eventQuestionText;

    [Header("Answer Panels")]
    [SerializeField] private GameObject[] answerPanels; // each panel contains title, desc, and button
    [SerializeField] private Button leftArrow;
    [SerializeField] private Button rightArrow;

    private EventsDatabase.Event currentEvent;
    private int currentPanelIndex = 0;

    public System.Action OnQuestionAnswered;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        ResourceManager.Instance.LoseAction += LosePopup;

        leftArrow.onClick.AddListener(ShowPreviousPanel);
        rightArrow.onClick.AddListener(ShowNextPanel);
    }

    #region Losing UI

    public void LosePopup(string headerText, string description)
    {
        losePopupTextHeader.text = headerText;
        losePopupTextDescription.text = description;
        losePopupAnimator.Play(popup1AnimHash);
    }

    #endregion

    public void ShowEvent(EventsDatabase.Event eventData)
    {
        currentEvent = eventData;
        quizPanel.SetActive(true);
        quizPopupAnimator.Play("QuizPopup");

        currentPanelIndex = 0;

        eventTitleText.text = currentEvent.title;
        eventDateText.text = currentEvent.eventDate;
        eventQuestionText.text = currentEvent.question;

        for (int i = 0; i < answerPanels.Length; i++)
        {
            GameObject panel = answerPanels[i];
            panel.SetActive(i == currentPanelIndex && i < currentEvent.choices.Count);

            if (i < currentEvent.choices.Count)
            {
                var choice = currentEvent.choices[i];
                var title = panel.transform.Find("Event Choice Title").GetComponent<TextMeshProUGUI>();
                var desc = panel.transform.Find("Event Description").GetComponent<TextMeshProUGUI>();
                var button = panel.GetComponentInChildren<Button>();

                title.text = choice.title;
                desc.text = choice.description;

                int answerIndex = i;
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => AnswerSelected(answerIndex));
            }
        }

        UpdateArrowVisibility();
    }

    private void ShowNextPanel()
    {
        if (currentPanelIndex < currentEvent.choices.Count - 1)
        {
            answerPanels[currentPanelIndex].SetActive(false);
            currentPanelIndex++;
            answerPanels[currentPanelIndex].SetActive(true);
            UpdateArrowVisibility();
        }
    }

    private void ShowPreviousPanel()
    {
        if (currentPanelIndex > 0)
        {
            answerPanels[currentPanelIndex].SetActive(false);
            currentPanelIndex--;
            answerPanels[currentPanelIndex].SetActive(true);
            UpdateArrowVisibility();
        }
    }

    private void UpdateArrowVisibility()
    {
        leftArrow.gameObject.SetActive(currentPanelIndex > 0);
        rightArrow.gameObject.SetActive(currentPanelIndex < currentEvent.choices.Count - 1);
    }

    private void AnswerSelected(int index)
    {
        quizPanel.SetActive(false);

        var selectedChoice = currentEvent.choices[index];

        ResourceManager.Instance.UpdateForeignAffairs(selectedChoice.foreignAffairsModifier);
        ResourceManager.Instance.UpdateEurosceptisism(selectedChoice.euroscepticismModifier);
        ResourceManager.Instance.UpdateBudget(selectedChoice.moneyModifier);

        OnQuestionAnswered?.Invoke();
    }
}
