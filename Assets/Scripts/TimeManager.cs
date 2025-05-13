using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    [Header("Time Settings")]
    public int day = 1;
    public int month = 1;
    public int year = 1000;
    public int hour = 6;
    public int minute = 0;

    public float timeScale = 60f;

    [Header("UI Elements")]
    public TMP_Text HUDDate;
    public Button pauseButton;
    public Button playButton;
    public Button fastForwardButton;

    [Header("Button Sprites")]
    public Sprite pauseSprite;
    public Sprite playSprite;
    public Sprite fastForwardSprite;
    public Sprite fastForwardActiveSprite;
    public Sprite playActiveSprite;
    public Sprite pauseActiveSprite;

    [Header("Event Manager")]
    public EventManager eventManager;


    private float timer;

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

         eventManager.CheckForEvent(GetDateString());
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
        pauseButton.interactable = false;
        playButton.interactable = true;
        fastForwardButton.interactable = true;
        pauseButton.GetComponent<Image>().sprite = pauseActiveSprite;
        playButton.GetComponent<Image>().sprite = playSprite;
        fastForwardButton.GetComponent<Image>().sprite = fastForwardSprite;
    }

    public void PlayTime()
    {
        timeScale = 1f;
        pauseButton.interactable = true;
        playButton.interactable = false;
        fastForwardButton.interactable = true;
        pauseButton.GetComponent<Image>().sprite = pauseSprite;
        playButton.GetComponent<Image>().sprite = playActiveSprite;
        fastForwardButton.GetComponent<Image>().sprite = fastForwardSprite;
    }

    public void FastForwardTime()
    {
        timeScale = 10f;
        pauseButton.interactable = true;
        playButton.interactable = true;
        fastForwardButton.interactable = false;
        pauseButton.GetComponent<Image>().sprite = pauseSprite;
        playButton.GetComponent<Image>().sprite = playSprite;
        fastForwardButton.GetComponent<Image>().sprite = fastForwardActiveSprite;
    }
}
