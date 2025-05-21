using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;
    public Action seenEvent;

    [Header("Revenue Colors")]
    [SerializeField] private Color positiveRevenueColor = new Color(0f, 0.25f, 0f);
    [SerializeField] private Color negativeRevenueColor = new Color(0.25f, 0f, 0f);

    [Header("Particle System")]
    [SerializeField] private GameObject particleSystemGO;

    [Header("Quiz UI")]
    [SerializeField] private Animator quizPopupAnimator;
    [SerializeField] private GameObject quizPanel;
    [SerializeField] private GameObject eventPanel;
    [SerializeField] private GameObject questionPanel;

    [SerializeField] private TextMeshProUGUI eventTitleText;
    [SerializeField] private TextMeshProUGUI eventDateText;
    [SerializeField] private TextMeshProUGUI eventQuestionText;
    [SerializeField] private TextMeshProUGUI eventDescriptionText;

    [Header("Answer Panels")]
    [SerializeField] private GameObject[] answerPanels; // each panel contains title, desc, and button
    [SerializeField] private Button leftArrow;
    [SerializeField] private Button rightArrow;
    

    private EventsDatabase.Event currentEvent;
    private QuizzesDatabase.Quiz currentQuiz;
    private int currentPanelIndex = 0;
    private bool playParticles = true;

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
        GameEndManager.Instance.GameLost(headerText, description);
    }

    #endregion

    private void ResetAllTexts()
    {
        eventTitleText.text = "";
        eventDateText.text = "";
        eventQuestionText.text = "";
        eventDescriptionText.text = "";

         eventTitleText.color = new Color(0.1176471f, 0.145098f, 0.3686275f);
         eventDateText.color = new Color(0.1176471f, 0.145098f, 0.3686275f);
         eventQuestionText.color = new Color(0.1176471f, 0.145098f, 0.3686275f);
         eventDescriptionText.color = new Color(0.1176471f, 0.145098f, 0.3686275f);
        var eventButton = eventPanel.transform.Find("Event Button").GetComponent<Button>();
        eventButton.onClick.RemoveAllListeners();

        for (int i = 0; i < answerPanels.Length; i++)
        {
            GameObject panel = answerPanels[i];
            panel.SetActive(false); // <-- This is the missing key line

            var title = panel.transform.Find("Event Choice Title").GetComponent<TextMeshProUGUI>();
            var desc = panel.transform.Find("Event Description").GetComponent<TextMeshProUGUI>();
            var money = panel.transform.Find("Event Money Modifier").GetComponent<TextMeshProUGUI>();
            var button = panel.GetComponentInChildren<Button>();

            title.text = "";
            desc.text = "";
            money.text = "";

            title.color = new Color(0.1176471f, 0.145098f, 0.3686275f);
            desc.color = new Color(0.1176471f, 0.145098f, 0.3686275f);
            money.color = new Color(0.1176471f, 0.145098f, 0.3686275f);

            Debug.Log("Removing listeners from " + button.name);
            button.onClick.RemoveAllListeners();
        }
    }


    public void ShowQuizEvent(QuizzesDatabase.Quiz quizData)
    {
        ClearCurrentEventData();
        ResetAllTexts();

        currentQuiz = quizData;
        quizPanel.SetActive(true);
        eventPanel.SetActive(false);
        questionPanel.SetActive(true);
        quizPopupAnimator.Play("QuizPopup");

        currentPanelIndex = 0;

        eventTitleText.text = currentQuiz.quizName;
        eventDateText.text = currentQuiz.quizDate;
        eventQuestionText.text = currentQuiz.question;

        for (int i = 0; i < answerPanels.Length; i++)
        {
            GameObject panel = answerPanels[i];
            bool isActive = i == currentPanelIndex && i < currentQuiz.answers.Count;
            panel.SetActive(isActive);

            if (i < currentQuiz.answers.Count)
            {
                var answer = currentQuiz.answers[i];

                var title = panel.transform.Find("Event Choice Title").GetComponent<TextMeshProUGUI>();
                var desc = panel.transform.Find("Event Description").GetComponent<TextMeshProUGUI>();
                var button = panel.GetComponentInChildren<Button>();

                title.text = $"Answer {i + 1}";
                desc.text = answer.answerText;

                SetButtonListener(button, () =>
                {
                    seenEvent?.Invoke();
                    ResourceManager.Instance.UpdateQuizTries(!answer.isCorrect);
                    AnswerSelected(-1);
                });
            }
        }

        UpdateArrowVisibilityCurrentEvent();
    }

    public void ShowMainEvent(EventsDatabase.Event eventData)
    {
        ClearCurrentEventData();
        ResetAllTexts();
        currentEvent = eventData;
        currentPanelIndex = 0;
        // Cannot event with nothing to do
        if (currentEvent.choices.Count == 1)
        {
            DisableArrows();

            quizPanel.SetActive(true);
            quizPopupAnimator.Play("QuizPopup");

            eventTitleText.text = currentEvent.title;
            eventDateText.text = currentEvent.eventDate;
            eventQuestionText.text = "";
            eventDescriptionText.text = currentEvent.description;

            GameObject panel = answerPanels[0];
            panel.SetActive(true);

            var title = panel.transform.Find("Event Choice Title").GetComponent<TextMeshProUGUI>();
            var desc = panel.transform.Find("Event Description").GetComponent<TextMeshProUGUI>();
            var moneyModifier = panel.transform.Find("Event Money Modifier").GetComponent<TextMeshProUGUI>();
            
            title.text = "";
            desc.text = currentEvent.description;
            moneyModifier.text = "€" + FormatLargeNumber(currentEvent.choices[0].moneyModifier);

            moneyModifier.color = 
                currentEvent.choices[0].moneyModifier > 0 ? positiveRevenueColor: negativeRevenueColor;

            var eventButton = eventPanel.transform.Find("Event Button").GetComponent<Button>();

            playParticles = false;

            SetButtonListener(eventButton, () =>
            {
                VotingLoader.Instance.Show(currentEvent, 0);
                UpdateResources(0);
            });
        }
        else
        {
            quizPanel.SetActive(true);
            quizPopupAnimator.Play("QuizPopup");

            eventTitleText.text = currentEvent.title;
            eventDateText.text = currentEvent.eventDate;
            eventQuestionText.text = currentEvent.question;
            eventDescriptionText.text = currentEvent.description;

            var eventButton = eventPanel.transform.Find("Event Button").GetComponent<Button>();
            SetButtonListener(eventButton, () =>
            {
                eventPanel.SetActive(false);
                questionPanel.SetActive(true);
            });

            for (int i = 0; i < answerPanels.Length; i++)
            {
                GameObject panel = answerPanels[i];
                panel.SetActive(i == currentPanelIndex && i < currentEvent.choices.Count);

                if (i < currentEvent.choices.Count)
                {
                    var choice = currentEvent.choices[i];
                    var title = panel.transform.Find("Event Choice Title").GetComponent<TextMeshProUGUI>();
                    var desc = panel.transform.Find("Event Description").GetComponent<TextMeshProUGUI>();
                    var moneyModifier = panel.transform.Find("Event Money Modifier").GetComponent<TextMeshProUGUI>();
                    var button = panel.GetComponentInChildren<Button>();

                    title.text = choice.title;
                    desc.text = choice.description;
                    moneyModifier.text = "€" + FormatLargeNumber(currentEvent.choices[i].moneyModifier);
                    moneyModifier.color =
                        currentEvent.choices[i].moneyModifier > 0 ? positiveRevenueColor : negativeRevenueColor;

                    int answerIndex = i;
                    Debug.Log($"[PopupManager] Setting listener for button: {button.name}, panel active: {panel.activeInHierarchy}");
                    SetButtonListener(button, () =>
                    {
                        Debug.Log($"[PopupManager] Button clicked — index: {answerIndex}");
                        VotingLoader.Instance.Show(currentEvent, answerIndex);
                        UpdateResources(answerIndex);
                    });
                }
            }

            UpdateArrowVisibilityCurrentEvent();
        }
    }

    public void ShowBudgetEvent(BudgetDatabase.Budget budgetData)
    {
        ClearCurrentEventData();
        ResetAllTexts();

        quizPanel.SetActive(true);
        quizPopupAnimator.Play("QuizPopup");

        DisableArrows();

        ResourceManager.Instance.UpdateBudget(budgetData.budget);

        eventTitleText.text = "Budget Allocation";
        eventDateText.text = budgetData.budgetAllocationDate;
        eventQuestionText.text = "Budget Increased";
        eventDescriptionText.text = $"You received €{FormatLargeNumber(budgetData.budget)}. Allocate it wisely.";

        GameObject panel = answerPanels[0];
        panel.SetActive(true);

        var title = panel.transform.Find("Event Choice Title").GetComponent<TextMeshProUGUI>();
        var desc = panel.transform.Find("Event Description").GetComponent<TextMeshProUGUI>();
        var moneyModifier = panel.transform.Find("Event Money Modifier").GetComponent<TextMeshProUGUI>();
        var eventButton = eventPanel.transform.Find("Event Button").GetComponent<Button>();

        title.text = "";
        moneyModifier.text = "";
        desc.text = $"You received €{FormatLargeNumber(budgetData.budget)}. Allocate it wisely.";
        desc.color = positiveRevenueColor;

        SetButtonListener(eventButton, () =>
        {
            seenEvent?.Invoke();
            AnswerSelected(-1);
        });
    }

    public void ShowElectionEvent(ElectionsDatabase.Election electionData)
    {
        ClearCurrentEventData();
        ResetAllTexts();

        quizPanel.SetActive(true);
        quizPopupAnimator.Play("QuizPopup");

        DisableArrows();

        eventTitleText.text = "Election Results";
        eventDateText.text = DateTime.Now.ToShortDateString();
        eventQuestionText.text = "";
        eventDescriptionText.text = electionData.description;

        GameObject panel = answerPanels[0];
        panel.SetActive(true);

        var title = panel.transform.Find("Event Choice Title").GetComponent<TextMeshProUGUI>();
        var desc = panel.transform.Find("Event Description").GetComponent<TextMeshProUGUI>();
        var moneyModifier = panel.transform.Find("Event Money Modifier").GetComponent<TextMeshProUGUI>();
        var eventButton = eventPanel.transform.Find("Event Button").GetComponent<Button>();

        moneyModifier.text = "";
        title.text = electionData.title;
        desc.text = electionData.description;
        
        SetButtonListener(eventButton, () =>
        {
            seenEvent?.Invoke();
            EUStats.Instance.ChangeParty(electionData);
            AnswerSelected(-1);
        });
    }

    public void ShowMemberEvent(MembersDatabase.MemberEvent memberData)
    {
        ClearCurrentEventData();
        ResetAllTexts();

        quizPanel.SetActive(true);
        quizPopupAnimator.Play("QuizPopup");

        currentPanelIndex = 0;

        eventTitleText.text = memberData.title;
        eventDateText.text = memberData.date;
        eventQuestionText.text = "";
        eventDescriptionText.text = memberData.description;

        GameObject panel = answerPanels[0];
        panel.SetActive(true);

        var title = panel.transform.Find("Event Choice Title").GetComponent<TextMeshProUGUI>();
        var desc = panel.transform.Find("Event Description").GetComponent<TextMeshProUGUI>();
        var eventButton = eventPanel.transform.Find("Event Button").GetComponent<Button>();

        title.text = "";
        desc.text = memberData.description;

        SetButtonListener(eventButton, () =>
        {
            seenEvent?.Invoke();
            EUStats.Instance.UpdateMap(memberData.newMap);
            AnswerSelected(-1);
        });
        
        DisableArrows();
    }

    private void ShowNextPanel()
    {
        if (currentQuiz != null && currentPanelIndex < currentQuiz.answers.Count - 1)
        {
            answerPanels[currentPanelIndex].SetActive(false);
            currentPanelIndex++;
            answerPanels[currentPanelIndex].SetActive(true);
            UpdateArrowVisibilityCurrentQuiz();
        }
        else if (currentEvent != null && currentPanelIndex < currentEvent.choices.Count - 1)
        {
            answerPanels[currentPanelIndex].SetActive(false);
            currentPanelIndex++;
            answerPanels[currentPanelIndex].SetActive(true);
            UpdateArrowVisibilityCurrentEvent();
        }
    }

    private void ShowPreviousPanel()
    {
        if (currentPanelIndex > 0)
        {
            answerPanels[currentPanelIndex].SetActive(false);
            currentPanelIndex--;

            answerPanels[currentPanelIndex].SetActive(true);

            if (currentQuiz != null)
                UpdateArrowVisibilityCurrentQuiz();
            else if (currentEvent != null)
                UpdateArrowVisibilityCurrentEvent();
        }
    }

    private void UpdateArrowVisibilityCurrentQuiz()
    {
        leftArrow.gameObject.SetActive(currentPanelIndex > 0);
        rightArrow.gameObject.SetActive(currentPanelIndex < currentQuiz.answers.Count - 1);
    }


    private void UpdateArrowVisibilityCurrentEvent()
    {
        if(currentEvent == null) { UpdateArrowVisibilityCurrentQuiz(); return; }

        leftArrow.gameObject.SetActive(currentPanelIndex > 0);
        rightArrow.gameObject.SetActive(currentPanelIndex < currentEvent.choices.Count - 1);
    }

    private void DisableArrows()
    {
        leftArrow.gameObject.SetActive(false);
        rightArrow.gameObject.SetActive(false);
    }

    private void AnswerSelected(int index)
    {
        Debug.Log("anserSelected");
        quizPanel.SetActive(false);
        TimeManager.Instance.PlayTime();

        if (index < 0) { SaveManager.Instance.SaveGame(); return; }

        UpdateResources(index);
    }

    private void UpdateResources(int index)
    {
        quizPanel.SetActive(false);
        var selectedChoice = currentEvent.choices[index];

        ResourceManager.Instance.UpdateForeignAffairs(selectedChoice.foreignAffairsModifier);
        ResourceManager.Instance.UpdateEurosceptisism(selectedChoice.euroscepticismModifier);
        ResourceManager.Instance.UpdateBudget(selectedChoice.moneyModifier);
        SaveManager.Instance.SaveGame();
    }

    private void ClearCurrentEventData()
    {
        currentQuiz = null;
        currentEvent = null;
    }


    public void PlayParticles()
    {
        if (!playParticles)
        {
            playParticles = true;
            return;
        }

        if (particleSystemGO == null) return;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        worldPos.z = 0f; 

        particleSystemGO.transform.position = worldPos;

        var ps = particleSystemGO.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
        }
    }
    private string FormatLargeNumber(float number)
    {
        float absNumber = Math.Abs(number);
        string sign = number < 0 ? "-" : "";

        if (absNumber >= 1_000_000_000)
            return sign + (absNumber / 1_000_000_000f).ToString("0.#") + " Bil";
        else if (absNumber >= 1_000_000)
            return sign + (absNumber / 1_000_000f).ToString("0.#") + " Mil";
        else if (absNumber >= 1_000)
            return sign + (absNumber / 1_000f).ToString("0.#") + "K";
        else
            return number.ToString("0");
    }
    private void SetButtonListener(Button button, Action action)
    {
        if (button == null)
        {
            Debug.LogError("Trying to assign listener to a null button.");
            return;
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            Debug.Log($"[PopupManager] Button {button.name} clicked.");
            try
            {
                action();
            }
            catch (Exception e)
            {
                Debug.LogError($"[PopupManager] Button action error: {e}");
            }
        });
    }
}
