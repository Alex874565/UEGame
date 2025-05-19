using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

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

    public void ShowQuizEvent(QuizzesDatabase.Quiz quizData)
    {
        quizPanel.SetActive(true);
        quizPopupAnimator.Play("QuizPopup");

        currentPanelIndex = 0;

        eventTitleText.text = quizData.quizName;
        eventDateText.text = quizData.quizDate;
        eventQuestionText.text = quizData.question;

        for (int i = 0; i < answerPanels.Length; i++)
        {
            GameObject panel = answerPanels[i];
            bool isActive = i == currentPanelIndex && i < quizData.answers.Count;
            panel.SetActive(isActive);

            if (i < quizData.answers.Count)
            {
                var answer = quizData.answers[i];

                var title = panel.transform.Find("Event Choice Title").GetComponent<TextMeshProUGUI>();
                var desc = panel.transform.Find("Event Description").GetComponent<TextMeshProUGUI>();
                var button = panel.GetComponentInChildren<Button>();

                title.text = $"Answer {i + 1}";
                desc.text = answer.answerText;

                int index = i;
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                    quizPanel.SetActive(false);

                    if (answer.isCorrect)
                    {
                        Debug.Log("Correct answer selected!");
                        // Optional: Reward player or trigger effects here
                    }
                    else
                    {
                        Debug.Log("Incorrect answer selected.");
                        ResourceManager.Instance.UpdateQuizTries(true);
                    }
                });
            }
        }

        UpdateArrowVisibility(quizData);
    }

    public void ShowMainEvent(EventsDatabase.Event eventData)
    {
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

            button.onClick.RemoveAllListeners();
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

                    button.onClick.RemoveAllListeners();
                    int answerIndex = i;
                    button.onClick.AddListener(() =>
                    {
                        VotingLoader.Instance.Show(currentEvent, answerIndex); 
                        AnswerSelected(answerIndex);
                    });
                }
            }

            UpdateArrowVisibility();
        }
    }

    public void ShowBudgetEvent(BudgetDatabase.Budget budgetData)
    {
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

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            AnswerSelected(-1);
        });
    }

    public void ShowElectionEvent(ElectionsDatabase.Election electionData)
    {
        quizPanel.SetActive(true);
        quizPopupAnimator.Play("QuizPopup");

        DisableArrows();

        eventTitleText.text = "Election Results";
        eventDateText.text = System.DateTime.Now.ToShortDateString();
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
        EUStats.Instance.ChangeParty(electionData);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            AnswerSelected(-1);
        });
    }

    public void ShowMemberEvent(MembersDatabase.MemberEvent memberData)
    {
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

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => 
        { 
            EUStats.Instance.UpdateMap(memberData.newMap);
            AnswerSelected(-1); 
        });
        
        DisableArrows();
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

    private void UpdateArrowVisibility(QuizzesDatabase.Quiz quizData)
    {
        leftArrow.gameObject.SetActive(currentPanelIndex > 0);
        rightArrow.gameObject.SetActive(currentPanelIndex < quizData.answers.Count - 1);
    }

    private void DisableArrows()
    {
        leftArrow.gameObject.SetActive(false);
        rightArrow.gameObject.SetActive(false);
    }

    private void AnswerSelected(int index)
    {
        quizPanel.SetActive(false);
        TimeManager.Instance.SetTimeScale(TimeManager.Instance.DefaultTimeScale);
        TimeManager.Instance.ResetButtonStates();

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
        ResourceManager.Instance.UpdateQuizTries(!selectedChoice.Equals(true));
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
