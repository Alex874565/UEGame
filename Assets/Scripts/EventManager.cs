using UnityEngine;

public class EventManager : MonoBehaviour
{
    public EventsDatabase eventsDatabase;
    public BudgetDatabase budgetDatabase;
    public QuizzesDatabase quizzesDatabase;
    public ElectionsDatabase electionsDatabase;

    [Header("Managers")]
    public TimeManager timeManager;
    public ResourceManager resourceManager;

    private int eventIndex;
    private int budgetIndex;
    private int quizIndex;
    private int electionIndex;

    public void Start()
    {
        eventIndex = 0;
        budgetIndex = 0;
        quizIndex = 0;
        electionIndex = 0;
    }

    public void CheckForEvent(string date)
    {
        if(eventsDatabase.events.Count > eventIndex && date == eventsDatabase.events[eventIndex].eventDate)
        {
            Debug.Log("Event Triggered: " + eventsDatabase.events[eventIndex].title);
            eventIndex++;
        }
        if (budgetDatabase.budgets.Count > budgetIndex && date == budgetDatabase.budgets[budgetIndex].budgetAllocationDate)
        {
            Debug.Log("Budget Triggered: " + budgetDatabase.budgets[budgetIndex].budget);
            budgetIndex++;
        }
        if (quizzesDatabase.quizzes.Count > quizIndex && date == quizzesDatabase.quizzes[quizIndex].quizDate)
        {
            Debug.Log("Quiz Triggered: " + quizzesDatabase.quizzes[quizIndex].quizName);
            quizIndex++;
        }
        if (electionsDatabase.elections.Count > electionIndex && date == electionsDatabase.elections[electionIndex].electionDate)
        {
            Debug.Log("Election Triggered: " + electionsDatabase.elections[electionIndex].electionDate);
            electionIndex++;
        }
    }
}
