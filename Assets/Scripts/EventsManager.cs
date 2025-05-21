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
    [SerializeField] private EventsDatabase eventsDatabase;
    [SerializeField] private BudgetDatabase budgetDatabase;
    [SerializeField] private QuizzesDatabase quizzesDatabase;
    [SerializeField] private ElectionsDatabase electionsDatabase;
    [SerializeField] private MembersDatabase membersDatabase;

    [Header("Managers")]
    public TimeManager timeManager;
    public ResourceManager resourceManager;

    private int eventIndex;
    private int budgetIndex;
    private int quizIndex;
    private int electionIndex;
    private int membersIndex;
    private int nrOfSpawnedEvents;

    #region Getters

    public int EventIndex { get { return eventIndex; } set { eventIndex = value; } }
    public int BudgetIndex { get { return budgetIndex; } set { budgetIndex = value; } }
    public int QuizIndex { get { return quizIndex; } set { quizIndex = value; } }
    public int ElectionIndex { get { return electionIndex; } set { electionIndex = value; } }
    public int MembersIndex { get { return membersIndex; } set { electionIndex = value; } }
    public int NrOfSpawnedEvents { get { return nrOfSpawnedEvents; } set { nrOfSpawnedEvents = value; } }
    
    public ElectionsDatabase ElectionsDatabase { get { return electionsDatabase; } }

    #endregion

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    public void Start()
    {
        PopupManager.Instance.seenEvent = DestroyedEvent;
        eventIndex = 0;
        budgetIndex = 0;
        quizIndex = 0;
        electionIndex = 0;
        membersIndex = 0;
    }

    public void Load(SaveData saveData)
    {
        SaveData data = SaveManager.Instance.currentData;

        this.eventIndex = data.eventIndex;
        this.budgetIndex = data.budgetIndex;
        this.quizIndex = data.quizIndex;
        this.electionIndex = data.electionIndex;
        this.membersIndex = data.membersIndex;

        // We know which map we have by the members index.
        EUStats.Instance.UpdateMap(membersDatabase.memberEvents[membersIndex].newMap);
    }

    public void CheckForEvent(string date)
    {
        if (eventsDatabase.events.Count > eventIndex && date == eventsDatabase.events[eventIndex].eventDate)
        {
            timeManager.DisableTimeButtons();
            timeManager.SetTimeScale(0);
            StartMainEvent(eventsDatabase.events[eventIndex].countryName);
        }

        if (budgetDatabase.budgets.Count > budgetIndex && date == budgetDatabase.budgets[budgetIndex].budgetAllocationDate)
        {
            timeManager.DisableTimeButtons();
            timeManager.SetTimeScale(0);
            StartBudgetEvent(budgetDatabase.budgets[budgetIndex].countryName);
        }

        if (quizzesDatabase.quizzes.Count > quizIndex && date == quizzesDatabase.quizzes[quizIndex].quizDate)
        {
            timeManager.DisableTimeButtons();
            timeManager.SetTimeScale(0);
            StartQuizEvent(quizzesDatabase.quizzes[quizIndex].countryName);
        }

        if (electionsDatabase.elections.Count > electionIndex && date == electionsDatabase.elections[electionIndex].electionDate)
        {
            timeManager.DisableTimeButtons();
            timeManager.SetTimeScale(0);
            StartElectionEvent(electionsDatabase.elections[electionIndex].countryName);
        }

        if (membersDatabase.memberEvents.Count > membersIndex && date == membersDatabase.memberEvents[membersIndex].date)
        {
            timeManager.DisableTimeButtons();
            timeManager.SetTimeScale(0);
            StartMemberEvent(membersDatabase.memberEvents[membersIndex].countryName);
        }
    }

    #region Start Event Functions

    public void StartMainEvent(string country)
    {
        AudioManager.Instance.PlaySoundPopupAppear();
        nrOfSpawnedEvents++;
        SpawnEvent(country);
        eventIndex++;
    }

    public void StartQuizEvent(string country)
    {
        AudioManager.Instance.PlaySoundPopupAppear();
        nrOfSpawnedEvents++;
        SpawnQuiz(country);
        quizIndex++;
    }

    public void StartBudgetEvent(string country)
    {
        AudioManager.Instance.PlaySoundPopupAppear();
        nrOfSpawnedEvents++;
        SpawnBudget(country);
        budgetIndex++;
    }

    public void StartElectionEvent(string country)
    {
        AudioManager.Instance.PlaySoundPopupAppear();
        nrOfSpawnedEvents++;
        SpawnElection(country);
        electionIndex++;
    }

    public void StartMemberEvent(string country)
    {
        AudioManager.Instance.PlaySoundPopupAppear();
        nrOfSpawnedEvents++;
        SpawnMemberEvent(country);
        membersIndex++;
    }

    #endregion

    #region Spawn Event Region

    private void SpawnEvent(string countryName)
    {
        Transform targetLocation = GetCityLocation(countryName);

        if (targetLocation != null)
        {
            GameObject GO = 
                Instantiate(mainEventPopup, targetLocation.position, Quaternion.identity, mapHolder);
            MainEventTrigger mainEvent = GO.GetComponentInChildren<MainEventTrigger>();
            mainEvent.Initialize(eventsDatabase.events[eventIndex]);
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
            QuizEventTrigger mainEvent = GO.GetComponentInChildren<QuizEventTrigger>();
            mainEvent.Initialize(quizzesDatabase.quizzes[quizIndex]);
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
            BudgetEventTrigger mainEvent = GO.GetComponentInChildren<BudgetEventTrigger>();
            mainEvent.Initialize(budgetDatabase.budgets[budgetIndex]);
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
            ElectionEventTrigger mainEvent = GO.GetComponentInChildren<ElectionEventTrigger>();
            mainEvent.Initialize(electionsDatabase.elections[electionIndex]);
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
            MemberEventTrigger mainEvent = GO.GetComponentInChildren<MemberEventTrigger>();
            mainEvent.Initialize(membersDatabase.memberEvents[membersIndex]);

            Debug.Log($"Member event spawned at {countryName}");
        }
        else
        {
            Debug.LogWarning($"No city location found for {countryName}!");
        }
    }

    #endregion

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
            GO.GetComponent<Animator>().Update(0f);
            GO.transform.position = targetLocation.position;

            return GO;
        }
        else
        {
            Debug.LogWarning($"No city location found for {countryName}!");
            return null;
        }
    }

    public void DestroyedEvent()
    {
        nrOfSpawnedEvents--;
    }
}
