using UnityEngine;
using System.Collections.Generic;

public class QuizManager : MonoBehaviour
{
    public static QuizManager Instance;

    [SerializeField] private EventsDatabase eventsDatabase;

    private int currentRegularEventIndex = 0;
    private EventsDatabase.Event currentMainEvent;

    private int currentMainEventIndex = 0;
    private bool isMainEvent = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PopupManager.Instance.OnQuestionAnswered = OnQuestionAnswered;
    }

    public void StartNextRegularEvent()
    {
        isMainEvent = false;

        if (currentRegularEventIndex >= eventsDatabase.events.Count)
        {
            Debug.Log("All events completed.");
            return;
        }

        var ev = eventsDatabase.events[currentRegularEventIndex];
        currentRegularEventIndex++;

        PopupManager.Instance.ShowEvent(ev);
    }

    public void StartMainEvent()
    {
        isMainEvent = true;
        currentMainEvent = eventsDatabase.events[currentMainEventIndex];

        PopupManager.Instance.ShowEvent(currentMainEvent);
    }

    private void OnQuestionAnswered()
    {
        if (isMainEvent)
        {
            VotingLoader.Instance.Show(() =>
            {
                Debug.Log("wow, such an animation");
            });
        }
        else
        {
            StartNextRegularEvent();
        }
    }
}
