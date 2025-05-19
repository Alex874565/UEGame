using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tutorial : MonoBehaviour
{
    [Header("Tutorial Steps")]
    [SerializeField] private List<GameObject> tutorialSteps = new();

    [Header("End of Tutorial Event")]
    public UnityEvent onTutorialComplete;

    private int currentStep = 0;

    void Start()
    {
        DeactivateAll();
        if (tutorialSteps.Count > 0)
        {
            tutorialSteps[0].SetActive(true);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AdvanceTutorial();
        }
    }

    void AdvanceTutorial()
    {
        if (currentStep < tutorialSteps.Count)
        {
            tutorialSteps[currentStep].SetActive(false);
            currentStep++;
        }

        if (currentStep < tutorialSteps.Count)
        {
            tutorialSteps[currentStep].SetActive(true);
        }
        else
        {
            SaveManager.Instance.currentData.finishedTutorial = true;
            onTutorialComplete?.Invoke();
        }
    }

    void DeactivateAll()
    {
        foreach (GameObject step in tutorialSteps)
        {
            if (step != null)
                step.SetActive(false);
        }
    }
}
