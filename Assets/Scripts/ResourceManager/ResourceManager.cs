using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    public Action<string, string> LoseAction;

    [Header("Foreign Affairs")]
    [SerializeField] private Animator foreignAnimator;
    [SerializeField] private Slider foreignAffairs;

    [Header("Eurosceptisism")]
    [SerializeField] private Animator euroAnimator;
    [SerializeField] private Slider eurosceptisism;

    [Header("Budget")]
    [SerializeField] private Animator budgetAnimator;
    [SerializeField] private TextMeshProUGUI budget;

    [Header("Quiz")]
    [SerializeField] private Animator quizAnimator;
    [SerializeField] private TextMeshProUGUI quizzes;

    [Space(5)]
    [Header("Losing Threshold")]
    [SerializeField] private float minForeign = .2f;
    [SerializeField] private float maxEurosceptisism = .85f;

    [SerializeField] private float minBudget = 1000f;
    [SerializeField] private int maxQuizTries = 21;

    private float currentForeignAffairs = 0.8f;
    private float currentEurosceptisism = 0.65f;

    private long currentBudget = 1000;
    private int currentQuizFails = 0;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        UpdateUI();
    }

    public void Load(SaveData saveData)
    {
        SaveData data = SaveManager.Instance.currentData;

        currentForeignAffairs = data.foreignAffair;
        currentEurosceptisism = data.euroscepticism;
        currentBudget = data.budget;
        currentQuizFails = data.quizzesFailed;

        Debug.Log("ResourceManager: Data loaded from SaveManager.");
        UpdateUI();
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

        budget.text = FormatLargeNumber(currentBudget);
        quizzes.text = currentQuizFails.ToString() + "/" + maxQuizTries;
    }

    private string FormatLargeNumber(float number)
    {
        if (number >= 1_000_000_000)
            return (number / 1_000_000_000f).ToString("0.#") + " Bil";
        else if (number >= 1_000_000)
            return (number / 1_000_000f).ToString("0.#") + " Mil";
        else if (number >= 1_000)
            return (number / 1_000f).ToString("0.#") + "K";
        else
            return number.ToString("0");
    }


    #endregion


    #region Public Functions

    public void UpdateEurosceptisism(float sceptisism)
    {
        currentEurosceptisism += sceptisism;

        euroAnimator.Play(sceptisism < 0 ? "GreenFlashBar" : "RedFlashBar");

        UpdateUI();
        if (currentEurosceptisism > maxEurosceptisism)
        {
            Lose("header1", "description1");
        }
    }

    public void UpdateForeignAffairs(float affairs)
    {
        currentForeignAffairs += affairs;

        foreignAnimator.Play(affairs > 0 ? "GreenFlashBar" : "RedFlashBar");

        UpdateUI();
        if (currentForeignAffairs < minForeign)
        {
            Lose("header2", "description2");
        }
    }

    public void UpdateBudget(long budget)
    {
        currentBudget += budget;

        budgetAnimator.Play(budget > 0 ? "GreenFlash" : "RedFlash");
        
        UpdateUI();
        if (currentBudget < minBudget)
        {
            Lose("header3", "description3");
        }
    }

    public void UpdateQuizTries(bool failed)
    {
        if (failed) currentQuizFails++;

        quizAnimator.Play(failed == false ? "GreenFlash" : "RedFlash");

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
