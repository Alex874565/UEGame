using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tutorial : MonoBehaviour
{
    [Header("Tutorial Steps")]
    [SerializeField] private List<GameObject> tutorialSteps = new();
    [SerializeField] private GameObject clickButton;

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
        clickButton.SetActive(false);

        if (currentStep < tutorialSteps.Count)
        {
            tutorialSteps[currentStep].GetComponent<Animator>().Play("FadeOut");
            //tutorialSteps[currentStep].SetActive(false);
            currentStep++;
        }

        if (currentStep < tutorialSteps.Count)
        {
            tutorialSteps[currentStep].SetActive(true);
            tutorialSteps[currentStep].GetComponent<Animator>().Play("FadeIn");
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
