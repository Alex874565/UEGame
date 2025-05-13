using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameEndManager 
{
    public ResourceManager resourceManager;
    
    public GameObject gameLostScreen;
    public GameObject gameFinishedScreen;

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
        gameFinishedScreen.SetActive(true);
    }
   
}
