using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

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


    private int lastActivatedButtonIndex;

    private float timeScale;
    private float oldTimeScale;
    private float timer;

    #region Getters

    public int GetDay() => day;
    public int GetMonth() => month;
    public int GetYear() => year;
    public int GetHour() => hour;
    public int GetMinute() => minute;

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
        oldTimeScale = timeScale;
        timeScale = defaultTimeScale;
        timer = 0f;
        UpdateHUDDate();
        PlayTime();
    }

    void Update()
    {
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
            if (month > 12)
            {
                month = 1;
                year++;
            }
        }

        if(year == 2025) {
            gameEndManager.TriggerGameFinish();
            return;
        }
        eventsManager.CheckForEvent(GetDateString());
    }

    public string GetDateString()
    {
        DateTime date = new DateTime(year, month, day);
        return $"{date.ToString("dd/MM/yyyy")}";
    }

    public void UpdateHUDDate()
    {
        HUDDate.text = GetDateString();
    }

    public void PauseTime()
    {
        timeScale = 0f;
        lastActivatedButtonIndex = 2;
        pauseButton.interactable = false;
        playButton.interactable = true;
        fastForwardButton.interactable = true;
        pauseButton.GetComponent<Image>().sprite = pauseActiveSprite;
        playButton.GetComponent<Image>().sprite = playSprite;
        fastForwardButton.GetComponent<Image>().sprite = fastForwardSprite;
    }

    public void PlayTime()
    {
        timeScale = defaultTimeScale;
        lastActivatedButtonIndex = 0;
        pauseButton.interactable = true;
        playButton.interactable = false;
        fastForwardButton.interactable = true;
        pauseButton.GetComponent<Image>().sprite = pauseSprite;
        playButton.GetComponent<Image>().sprite = playActiveSprite;
        fastForwardButton.GetComponent<Image>().sprite = fastForwardSprite;
    }

    public void FastForwardTime()
    {
        timeScale = fastForwardTimeScale;
        lastActivatedButtonIndex = 1;
        pauseButton.interactable = true;
        playButton.interactable = true;
        fastForwardButton.interactable = false;
        pauseButton.GetComponent<Image>().sprite = pauseSprite;
        playButton.GetComponent<Image>().sprite = playSprite;
        fastForwardButton.GetComponent<Image>().sprite = fastForwardActiveSprite;
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

    public void EnableTimeButtons()
    {
        pauseButton.interactable = true;
        fastForwardButton.interactable = true;
        playButton.interactable = true;
    }

    public void ResetButtonStates()
    {
        playButton.interactable = lastActivatedButtonIndex != 0;
        fastForwardButton.interactable = lastActivatedButtonIndex != 1;
        pauseButton.interactable = lastActivatedButtonIndex != 2;
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
