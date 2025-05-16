using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public static EventsManager Instance;

    [SerializeField] private Transform mapHolder;
    [SerializeField] private List<Transform> cityLocations = new();

    [Header("Popup Prefabs")]
    [SerializeField] private GameObject mainEventPopup;
    [SerializeField] private GameObject quizPopup;
    [SerializeField] private GameObject budgetPopup;
    [SerializeField] private GameObject electionPopup;
    [SerializeField] private GameObject membersPopup;

    [Header("Databases")]
    public EventsDatabase eventsDatabase;
    public BudgetDatabase budgetDatabase;
    public QuizzesDatabase quizzesDatabase;
    public ElectionsDatabase electionsDatabase;
    public MembersDatabase membersDatabase;

    [Header("Managers")]
    public TimeManager timeManager;
    public ResourceManager resourceManager;

    private int eventIndex;
    private int budgetIndex;
    private int quizIndex;
    private int electionIndex;
    private int membersIndex;

    #region Getters

    public int EventIndex { get { return eventIndex; } set { eventIndex = value; } }
    public int BudgetIndex { get { return budgetIndex; } set { budgetIndex = value; } }
    public int QuizIndex { get { return quizIndex; } set { quizIndex = value; } }
    public int ElectionIndex { get { return electionIndex; } set { electionIndex = value; } }

    #endregion

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    public void Start()
    {
        eventIndex = 0;
        budgetIndex = 0;
        quizIndex = 0;
        electionIndex = 0;
        membersIndex = 0;
    }

    public void CheckForEvent(string date)
    {
        if (eventsDatabase.events.Count > eventIndex && date == eventsDatabase.events[eventIndex].eventDate)
        {
            Debug.Log("Event Triggered: " + eventsDatabase.events[eventIndex].title);
            timeManager.DisableTimeButtons();
            timeManager.SetTimeScale(0);
            StartMainEvent(eventsDatabase.events[eventIndex].countryName);
        }

        if (budgetDatabase.budgets.Count > budgetIndex && date == budgetDatabase.budgets[budgetIndex].budgetAllocationDate)
        {
            Debug.Log("Budget Triggered: " + budgetDatabase.budgets[budgetIndex].budget);
            timeManager.DisableTimeButtons();
            timeManager.SetTimeScale(0);
            StartBudgetEvent(budgetDatabase.budgets[budgetIndex].countryName);
        }

        if (quizzesDatabase.quizzes.Count > quizIndex && date == quizzesDatabase.quizzes[quizIndex].quizDate)
        {
            Debug.Log("Quiz Triggered: " + quizzesDatabase.quizzes[quizIndex].quizName);
            timeManager.DisableTimeButtons();
            timeManager.SetTimeScale(0);
            StartQuizEvent(quizzesDatabase.quizzes[quizIndex].countryName);
        }

        if (electionsDatabase.elections.Count > electionIndex && date == electionsDatabase.elections[electionIndex].electionDate)
        {
            Debug.Log("Election Triggered: " + electionsDatabase.elections[electionIndex].electionDate);
            timeManager.DisableTimeButtons();
            timeManager.SetTimeScale(0);
            //StartElectionEvent(electionsDatabase.elections[electionIndex].countryName);
        }

        if (membersDatabase.memberEvents.Count > membersIndex && date == membersDatabase.memberEvents[membersIndex].date)
        {
            Debug.Log("Election Triggered: " + membersDatabase.memberEvents[membersIndex].date);
            timeManager.DisableTimeButtons();
            timeManager.SetTimeScale(0);
            StartMemberEvent(membersDatabase.memberEvents[membersIndex].countryName);
        }
    }


    public void StartMainEvent(string country)
    {
        SpawnEvent(country);
        eventIndex++;
    }

    public void StartQuizEvent(string country)
    {
        SpawnQuiz(country);
        quizIndex++;
    }

    public void StartBudgetEvent(string country)
    {
        SpawnBudget(country);
        budgetIndex++;
    }

    public void StartElectionEvent(string country)
    {
        SpawnElection(country);
        electionIndex++;
    }

    public void StartMemberEvent(string country)
    {
        SpawnMemberEvent(country);
        eventIndex++;
    }

    private void SpawnEvent(string countryName)
    {
        Transform targetLocation = GetCityLocation(countryName);

        if (targetLocation != null)
        {
            GameObject GO = 
                Instantiate(mainEventPopup, targetLocation.position, Quaternion.identity, mapHolder);
            MainEventTrigger mainEvent = GO.GetComponent<MainEventTrigger>();
            mainEvent.Initialize(eventsDatabase.events[eventIndex]);

            Debug.Log($"Main Event spawned at {countryName}");
        }
        else
        {
            Debug.LogWarning($"No city location found for {countryName}!");
        }
    }

    private void SpawnQuiz(string countryName)
    {
        Transform targetLocation = GetCityLocation(countryName);

        if (targetLocation != null)
        {
            GameObject GO =
                Instantiate(quizPopup, targetLocation.position, Quaternion.identity, mapHolder);
            QuizEventTrigger mainEvent = GO.GetComponent<QuizEventTrigger>();
            mainEvent.Initialize(quizzesDatabase.quizzes[eventIndex]);

            Debug.Log($"Quiz spawned at {countryName}");
        }
        else
        {
            Debug.LogWarning($"No city location found for {countryName}!");
        }
    }

    private void SpawnBudget(string countryName)
    {
        Transform targetLocation = GetCityLocation(countryName);

        if (targetLocation != null)
        {
            GameObject GO =
                Instantiate(budgetPopup, targetLocation.position, Quaternion.identity, mapHolder);
            BudgetEventTrigger mainEvent = GO.GetComponent<BudgetEventTrigger>();
            mainEvent.Initialize(budgetDatabase.budgets[eventIndex]);

            Debug.Log($"Budget event spawned at {countryName}");
        }
        else
        {
            Debug.LogWarning($"No city location found for {countryName}!");
        }
    }

    private void SpawnElection(string countryName)
    {
        Transform targetLocation = GetCityLocation(countryName);

        if (targetLocation != null)
        {
            GameObject GO =
                Instantiate(electionPopup, targetLocation.position, Quaternion.identity, mapHolder);
            ElectionEventTrigger mainEvent = GO.GetComponent<ElectionEventTrigger>();
            mainEvent.Initialize(electionsDatabase.elections[eventIndex]);

            Debug.Log($"Election event spawned at {countryName}");
        }
        else
        {
            Debug.LogWarning($"No city location found for {countryName}!");
        }
    }

    private void SpawnMemberEvent(string countryName)
    {
        Transform targetLocation = GetCityLocation(countryName);

        if (targetLocation != null)
        {
            GameObject GO =
                Instantiate(membersPopup, targetLocation.position, Quaternion.identity, mapHolder);
            MemberEventTrigger mainEvent = GO.GetComponent<MemberEventTrigger>();
            mainEvent.Initialize(membersDatabase.memberEvents[membersIndex]);

            Debug.Log($"Member event spawned at {countryName}");
        }
        else
        {
            Debug.LogWarning($"No city location found for {countryName}!");
        }
    }

    public Transform GetCityLocation(string countryName)
    {
        return cityLocations.Find(loc => loc.name == countryName);
    }

    public GameObject SpawnPopup(GameObject popup, string countryName)
    {
        Transform targetLocation = GetCityLocation(countryName);

        if (targetLocation != null)
        {
            GameObject GO =
                Instantiate(popup, targetLocation.position, Quaternion.identity, mapHolder);
            return GO;
        }
        else
        {
            Debug.LogWarning($"No city location found for {countryName}!");
            return null;
        }
    }
}
