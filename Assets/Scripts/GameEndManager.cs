using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;

public class GameEndManager : MonoBehaviour
{
    public static GameEndManager Instance;

    public ResourceManager resourceManager;
    public TimeManager timeManager;
    public Sprite fullStarSprite;

    [Header("Lose Popup")]
    [SerializeField] private Animator losePopupAnimator;
    [SerializeField] private TextMeshProUGUI losePopupTextHeader;
    [SerializeField] private TextMeshProUGUI losePopupTextDescription;
    private int popupAnimHash = Animator.StringToHash("Popup1");

    [Header("Game Over Screens")]
    public GameObject gameLostScreen;
    public GameObject gameFinishedScreen;

    [Header("Stars")]
    public List<Image> moneyStars;
    public List<Image> quizzesStars;
    public List<Image> foreignImageStars;
    public List<Image> euroscepticismStars;

    [Header("Values")]
    [SerializeField] private TMP_Text moneyValue;
    [SerializeField] private TMP_Text quizzesValue;
    [SerializeField] private TMP_Text foreignImageValue;
    [SerializeField] private TMP_Text euroscepticismValue;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    public void GameLost(string loseReason, string loseDescription)
    {
        SaveManager.Instance.LostGame();

        TimeManager.Instance.SetTimeScale(0f);
        TimeManager.Instance.enabled = false;
        EventsManager.Instance.enabled = false;

        SaveManager.Instance.DeleteSave();
        SaveManager.Instance.enabled = false;

        losePopupTextDescription.text = loseDescription;
        gameLostScreen.SetActive(true);

        losePopupAnimator.Play(popupAnimHash);
    }

    public void RestartGame()
    {
        PauseMenu.Instance.MainMenu();
    }

    public void TriggerGameFinish()
    {
        SaveManager.Instance.DeleteSave();
        timeManager.SetTimeScale(0);

        int moneyStarsLevel = 0;
        int quizStarsLevel = 0;
        int foreignStarsLevel = 0;
        int euroscepticismStarsLevel = 0;
        
        long money = resourceManager.GetCurrentBudget();
        int quizFails = resourceManager.GetCurrentQuizFails();
        int foreignAffairs = (int)(resourceManager.GetCurrentForeignAffairs() * 100);
        int euroscepticism = (int)(resourceManager.GetCurrentEurosceptisism() * 100);

        money = 100000; // For testing purposes

        // Money Stars
        if (money >= 1000000000000)
        {
            moneyStarsLevel = 3;
        }
        else if (money >= 500000000000)
        {
            moneyStarsLevel = 2;
        }
        else if (money >= 0)
        {
            moneyStarsLevel = 1;
        }

        // Quiz Stars
        if (quizFails == 0)
        {
            quizStarsLevel = 3;
        }
        else if (quizFails <= 5)
        {
            quizStarsLevel = 2;
        }
        else
        {
            quizStarsLevel = 1;
        }

        // Foreign Affairs Stars
        if (foreignAffairs >= 80)
        {
            foreignStarsLevel = 3;
        }
        else if (foreignAffairs >= 50)
        {
            foreignStarsLevel = 2;
        }
        else if (foreignAffairs >= 20)
        {
            foreignStarsLevel = 1;
        }

        // Euroscepticism Stars
        if (euroscepticism <= 20)
        {
            euroscepticismStarsLevel = 3;
        }
        else if (euroscepticism <= 50)
        {
            euroscepticismStarsLevel = 2;
        }
        else if (euroscepticism <= 80)
        {
            euroscepticismStarsLevel = 1;
        }

        // Set Stars
        for (int i = 0; i < moneyStarsLevel; i++)
        {
            moneyStars[i].sprite = fullStarSprite;
        }
        for (int i = 0; i < quizStarsLevel; i++)
        {
            quizzesStars[i].sprite = fullStarSprite;
        }
        for (int i = 0; i < foreignStarsLevel; i++)
        {
            foreignImageStars[i].sprite = fullStarSprite;
        }
        for (int i = 0; i < euroscepticismStarsLevel; i++)
        {
            euroscepticismStars[i].sprite = fullStarSprite;
        }

        // Set Values
        moneyValue.text = money.ToString();
        quizzesValue.text = quizFails.ToString();
        foreignImageValue.text = foreignAffairs.ToString();
        euroscepticismValue.text = euroscepticism.ToString();

        // Show Game Finished Screen
        gameFinishedScreen.SetActive(true);
    }
}
