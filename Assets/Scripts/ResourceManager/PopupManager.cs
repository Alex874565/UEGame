using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;
    public Action seenEvent;

    [SerializeField] private Color positiveRevenueColor = Color.green;
    [SerializeField] private Color negativeRevenueColor = Color.red;
    
    [SerializeField] private GameObject particleSystemGO;

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

        eventTitleText.color = Color.white;
        eventDateText.color = Color.white;
        eventQuestionText.color = Color.white;

        for (int i = 0; i < answerPanels.Length; i++)
        {
            GameObject panel = answerPanels[i];

            var title = panel.transform.Find("Event Choice Title").GetComponent<TextMeshProUGUI>();
            var desc = panel.transform.Find("Event Description").GetComponent<TextMeshProUGUI>();
            var money = panel.transform.Find("Event Money Modifier").GetComponent<TextMeshProUGUI>();
            var button = panel.GetComponentInChildren<Button>();

            title.text = "";
            desc.text = "";
            money.text = "";

            title.color = Color.white;
            desc.color = Color.white;
            money.color = Color.white;

            button.onClick.RemoveAllListeners();
        }
    }

    public void ShowQuizEvent(QuizzesDatabase.Quiz quizData)
    {
        ClearCurrentEventData();
        seenEvent?.Invoke();
        ResetAllTexts();

        currentQuiz = quizData;
        quizPanel.SetActive(true);
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

                button.onClick.AddListener(() =>
                {
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
        seenEvent?.Invoke();
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

            GameObject panel = answerPanels[0];
            panel.SetActive(true);

            var title = panel.transform.Find("Event Choice Title").GetComponent<TextMeshProUGUI>();
            var desc = panel.transform.Find("Event Description").GetComponent<TextMeshProUGUI>();
            var moneyModifier = panel.transform.Find("Event Money Modifier").GetComponent<TextMeshProUGUI>();
            
            title.text = "";
            desc.text = currentEvent.description;
            moneyModifier.text = currentEvent.choices[0].moneyModifier.ToString() + "€";
            moneyModifier.color = 
                currentEvent.choices[0].moneyModifier > 0 ? positiveRevenueColor: negativeRevenueColor;

            var button = panel.GetComponentInChildren<Button>();

            playParticles = false;

            button.onClick.AddListener(() =>
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
                    moneyModifier.text = currentEvent.choices[i].moneyModifier.ToString() + "€";
                    moneyModifier.color =
                        currentEvent.choices[i].moneyModifier > 0 ? positiveRevenueColor : negativeRevenueColor;

                    int answerIndex = i;
                    button.onClick.AddListener(() =>
                    {
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
        seenEvent?.Invoke();
        ResetAllTexts();

        quizPanel.SetActive(true);
        quizPopupAnimator.Play("QuizPopup");

        DisableArrows();

        ResourceManager.Instance.UpdateBudget(budgetData.budget);

        eventTitleText.text = "Budget Allocation";
        eventDateText.text = budgetData.budgetAllocationDate;
        eventQuestionText.text = "Budget Increased";

        GameObject panel = answerPanels[0];
        panel.SetActive(true);

        var title = panel.transform.Find("Event Choice Title").GetComponent<TextMeshProUGUI>();
        var desc = panel.transform.Find("Event Description").GetComponent<TextMeshProUGUI>();
        var moneyModifier = panel.transform.Find("Event Money Modifier").GetComponent<TextMeshProUGUI>();
        var button = panel.GetComponentInChildren<Button>();

        title.text = "";
        moneyModifier.text = "";
        desc.text = $"You received + {budgetData.budget} €. Allocate it wisely.";
        desc.color = positiveRevenueColor;

        button.onClick.AddListener(() =>
        {
            AnswerSelected(-1);
        });
    }

    public void ShowElectionEvent(ElectionsDatabase.Election electionData)
    {
        ClearCurrentEventData();
        seenEvent?.Invoke();
        ResetAllTexts();

        quizPanel.SetActive(true);
        quizPopupAnimator.Play("QuizPopup");

        DisableArrows();

        eventTitleText.text = "Election Results";
        eventDateText.text = DateTime.Now.ToShortDateString();
        eventQuestionText.text = "";

        GameObject panel = answerPanels[0];
        panel.SetActive(true);

        var title = panel.transform.Find("Event Choice Title").GetComponent<TextMeshProUGUI>();
        var desc = panel.transform.Find("Event Description").GetComponent<TextMeshProUGUI>();
        var moneyModifier = panel.transform.Find("Event Money Modifier").GetComponent<TextMeshProUGUI>();
        var button = panel.GetComponentInChildren<Button>();

        moneyModifier.text = "";
        title.text = "New Government Formed";
        desc.text = $"The new ruling party are: ";
        for(int i = 0; i < electionData.parties.Count; i++)
        {
            // If the last party don't add the ','.
            bool isLastParty = i == electionData.parties.Count - 1;
            desc.text += isLastParty ? electionData.parties[i].partyName : electionData.parties[i].partyName + ", ";
        }
        desc.text += ".\nPolicy shifts are expected.";

        button.onClick.AddListener(() =>
        {
            EUStats.Instance.ChangeParty(electionData);
            AnswerSelected(-1);
        });
    }

    public void ShowMemberEvent(MembersDatabase.MemberEvent memberData)
    {
        ClearCurrentEventData();
        seenEvent?.Invoke();
        ResetAllTexts();

        quizPanel.SetActive(true);
        quizPopupAnimator.Play("QuizPopup");

        currentPanelIndex = 0;

        eventTitleText.text = memberData.title;
        eventDateText.text = memberData.date;
        eventQuestionText.text = "";

        GameObject panel = answerPanels[0];
        panel.SetActive(true);

        var title = panel.transform.Find("Event Choice Title").GetComponent<TextMeshProUGUI>();
        var desc = panel.transform.Find("Event Description").GetComponent<TextMeshProUGUI>();
        var button = panel.GetComponentInChildren<Button>();

        title.text = "";
        desc.text = memberData.description;

        button.onClick.AddListener(() => 
        { 
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
        quizPanel.SetActive(false);
        TimeManager.Instance.PlayTime();

        if (index < 0) { return; }

        UpdateResources(index);
    }

    private void UpdateResources(int index)
    {
        quizPanel.SetActive(false);
        var selectedChoice = currentEvent.choices[index];

        ResourceManager.Instance.UpdateForeignAffairs(selectedChoice.foreignAffairsModifier);
        ResourceManager.Instance.UpdateEurosceptisism(selectedChoice.euroscepticismModifier);
        ResourceManager.Instance.UpdateBudget(selectedChoice.moneyModifier);
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
}
