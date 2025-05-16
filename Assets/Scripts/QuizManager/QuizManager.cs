using UnityEngine;
using System.Collections.Generic;

public class QuizManager : MonoBehaviour
{
    public static QuizManager Instance;

    [Header("Databases")]
    [SerializeField] private EventsDatabase eventsDatabase;
    [SerializeField] private QuizzesDatabase quizzesDatabase;
    [SerializeField] private ElectionsDatabase electionsDatabase;
    [SerializeField] private BudgetDatabase budgetDatabase;

    [Header("External Handlers")]
    [SerializeField] private ResourceManager resourceManager; // For budget rewards
    [SerializeField] private EUStats partyManager;       // For elections (you will provide this)

    private EventsDatabase.Event currentMainEvent;

    private bool isMainEvent = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PopupManager.Instance.OnQuestionAnswered = OnQuestionAnswered;
    }

    public void TriggerBudgetEvent(int eventIndex)
    {
        PopupManager.Instance.ShowBudgetEvent(budgetDatabase.budgets[eventIndex]);
    }

    public void TriggerElection(int eventIndex)
    {
        PopupManager.Instance.ShowElectionEvent(electionsDatabase.elections[eventIndex]);
    }

    private void OnQuestionAnswered()
    {
        if (isMainEvent)
        {
            VotingLoader.Instance.Show();
        }
        else
        {
            // HARDCODED
            TimeManager.Instance.SetTimeScale(5f);
            TimeManager.Instance.EnableTimeButtons();
        }
    }
}
