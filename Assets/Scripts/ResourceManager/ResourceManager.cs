using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    public Action<string, string> LoseAction;

    [Header("Resource Sliders")]
    [SerializeField] private Slider foreignAffairs;
    [SerializeField] private Slider eurosceptisism;

    [SerializeField] private TextMeshProUGUI budget;
    [SerializeField] private TextMeshProUGUI quizzes;

    [Space(5)]
    [Header("Losing Threshold")]
    [SerializeField] private float minForeign = .2f;
    [SerializeField] private float maxEurosceptisism = .85f;

    [SerializeField] private float minBudget = 1000f;
    [SerializeField] private int maxQuizTries = 10;

    private float currentForeignAffairs = 0.8f;
    private float currentEurosceptisism = 0.65f;

    private long currentBudget = 1000;
    private int currentQuizFails = 0;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
    
    void Start()
    {
        Load();
        UpdateUI();
    }

    private void Load()
    {
        print("Need to load stats from save manager here");
    }

    #region Private Functions

    // Each lose will have a reason which will be displayed
    private void Lose(string header, string description)
    {
        LoseAction?.Invoke(header, description);
    }

    private void UpdateUI()
    {
        foreignAffairs.value = currentForeignAffairs / 1f;
        eurosceptisism.value = currentEurosceptisism / 1f;
        budget.text = currentBudget.ToString();
        quizzes.text = currentQuizFails.ToString() + "/" + maxQuizTries;
    }

    #endregion


    #region Public Functions

    public void UpdateEurosceptisism(float sceptisism)
    {
        currentEurosceptisism += sceptisism;

        UpdateUI();
        if (currentEurosceptisism < maxEurosceptisism)
        {
            Lose("header1", "description1");
        }
    }

    public void UpdateForeignAffairs(float affairs)
    {
        currentForeignAffairs += affairs;

        UpdateUI();
        if (currentForeignAffairs < minForeign)
        {
            Lose("header2", "description2");
        }
    }

    public void UpdateBudget(long budget)
    {
        currentBudget += budget;

        UpdateUI();
        if (currentBudget < minBudget)
        {
            Lose("header3", "description3");
        }
    }

    public void UpdateQuizTries(bool failed)
    {
        if (failed) currentQuizFails++;

        UpdateUI();
        if (currentQuizFails > maxQuizTries)
        {
            Lose("header4", "description4");
        }
    }

    public long GetCurrentBudget()
    {
        return currentBudget;
    }
    public int GetCurrentQuizFails()
    {
        return currentQuizFails;
    }
    public float GetCurrentForeignAffairs()
    {
        return currentForeignAffairs;
    }
    public float GetCurrentEurosceptisism()
    {
        return currentEurosceptisism;
    }

    #endregion
}
