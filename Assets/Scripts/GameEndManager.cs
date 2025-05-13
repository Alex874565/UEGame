using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameEndManager 
{
    public ResourceManager resourceManager;
    public Sprite fullStarSprite;

    [Header("Game Over Screens")]
    public GameObject gameLostScreen;
    public GameObject gameFinishedScreen;

    [Header("Stars")]
    public List<Image> moneyStars;
    public List<Image> quizStars;
    public List<Image> foreignImageStars;
    public List<Image> euroscepticismStars;

    public void GameLost()
    {
        Time.timeScale = 0;
        gameLostScreen.SetActive(true);
    }

    public void TriggerGameFinish()
    {
        Time.timeScale = 0;
        foreach (Image star in moneyStars)
        {
            star.sprite = fullStarSprite;
        }
        foreach (Image star in quizStars)
        {
            star.sprite = fullStarSprite;
        }
        foreach (Image star in foreignImageStars)
        {
            star.sprite = fullStarSprite;
        }
        foreach (Image star in euroscepticismStars)
        {
            star.sprite = fullStarSprite;
        }
        gameFinishedScreen.SetActive(true);
    }
   
}
