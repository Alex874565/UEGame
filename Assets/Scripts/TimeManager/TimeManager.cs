using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [SerializeField] private EventsManager eventsManager;
    [SerializeField] private GameEndManager gameEndManager;

    [Header("Time Settings")]
    [SerializeField] private int day = 1;
    [SerializeField] private int month = 1;
    [SerializeField] private int year = 1000;
    [SerializeField] private int hour = 6;
    [SerializeField] private int minute = 0;
    [SerializeField] private int endYear = 2025;

    [SerializeField] private long monthlyNegativeAllowance = -10000000000;
    [SerializeField] private float defaultTimeScale;
    [SerializeField] private float fastForwardTimeScale;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text HUDDate;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button fastForwardButton;

    [Header("Button Sprites")]
    [SerializeField] private Sprite pauseSprite;
    [SerializeField] private Sprite playSprite;
    [SerializeField] private Sprite fastForwardSprite;
    [SerializeField] private Sprite fastForwardActiveSprite;
    [SerializeField] private Sprite playActiveSprite;
    [SerializeField] private Sprite pauseActiveSprite;

    private float timeScale;
    private float oldTimeScale;
    private float timer;

    #region Getters

    public int GetDay() => day;
    public int GetMonth() => month;
    public int GetYear() => year;
    public int GetHour() => hour;
    public int GetMinute() => minute;

    public float DefaultTimeScale { get { return defaultTimeScale;  } }

    public float Timer { get { return timer; } }
    public float TimeScale { get { return timeScale; } }

    public float OldTimeScale { get { return oldTimeScale; } }

    #endregion

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        SaveManager.Instance.currentData.finishedTutorial = true;

        oldTimeScale = timeScale;
        timeScale = defaultTimeScale;
        timer = 0f;
        UpdateHUDDate();
        PauseTime();
    }

    void Update()
    {
        if(EventsManager.Instance.NrOfSpawnedEvents > 0) { return; }

        timer += Time.deltaTime * 3600 * 24 * timeScale;

        while (timer >= 60f)
        {
            timer -= 60f;
            AddMinute();
        }
    }

    void AddMinute()
    {
        minute++;
        if (minute >= 60)
        {
            minute = 0;
            hour++;
            if (hour >= 24)
            {
                hour = 0;
                AddDay();
            }
        }
        UpdateHUDDate();
    }

    void AddDay()
    {
        day++;
        int daysInMonth = DateTime.DaysInMonth(year, month);

        if (day > daysInMonth)
        {
            day = 1;
            month++;

            ResourceManager.Instance.UpdateBudget(monthlyNegativeAllowance);

            if (month > 12)
            {
                month = 1;
                year++;
            }
        }

        if(year == endYear) {
            gameEndManager.TriggerGameFinish();
            return;
        }
        eventsManager.CheckForEvent(GetDateString());
    }

    public string GetDateString()
    {
        DateTime date = new DateTime(year, month, day);
        return $"{date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}";
    }

    public void UpdateHUDDate()
    {
        HUDDate.text = GetDateString();
    }

    public void PauseTime()
    {
        timeScale = 0f;
        pauseButton.interactable = false;
        playButton.interactable = true;
        fastForwardButton.interactable = true;
        pauseButton.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
        playButton.transform.localScale = new Vector3(1, 1, 1);
        fastForwardButton.transform.localScale = new Vector3(-1, 1, 1);
    }

    public void PlayTime()
    {
        timeScale = defaultTimeScale;
        pauseButton.interactable = true;
        playButton.interactable = false;
        fastForwardButton.interactable = true;
        pauseButton.transform.localScale = new Vector3(1, 1, 1);
        playButton.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
        fastForwardButton.transform.localScale = new Vector3(-1, 1, 1);
    }

    public void FastForwardTime()
    {
        timeScale = fastForwardTimeScale;
        pauseButton.interactable = true;
        playButton.interactable = true;
        fastForwardButton.interactable = false;
        pauseButton.transform.localScale = new Vector3(1, 1, 1);
        playButton.transform.localScale = new Vector3(1, 1, 1);
        fastForwardButton.transform.localScale = new Vector3(-1.25f, 1.25f, 1.25f);
    }

    public void SetTimeScale(float newTimeScale)
    {
        oldTimeScale = timeScale;
        timeScale = newTimeScale;
    }

    public void DisableTimeButtons()
    {
        pauseButton.interactable = false;
        fastForwardButton.interactable = false;
        playButton.interactable = false;
    }

    public void Load(SaveData currentData)
    {
        day = currentData.day;
        month = currentData.month;
        year = currentData.year;
        hour = currentData.hour;
        minute = currentData.minute;
        UpdateHUDDate();
    }

}
