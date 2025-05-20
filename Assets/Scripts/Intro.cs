using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    [SerializeField] private GameObject studio;
    [SerializeField] private GameObject disclaimer;
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject disclaimerText;
    [SerializeField] private GameObject presentsText;
    [SerializeField] private GameObject mainMenu;

    private float studioWaitTime;
    private float disclaimerWaitTime;

    private bool skipIntro = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.GetInt("SkipIntro", 0) == 1)
        {
            PlayerPrefs.SetInt("SkipIntro", 0); // reset flag

            skipIntro = true;

            // Immediately skip to main menu
            studio.SetActive(false);
            disclaimer.SetActive(false);
            disclaimerText.SetActive(false);
            presentsText.SetActive(false);
            background.SetActive(false);

            mainMenu.SetActive(true);
            mainMenu.GetComponent<Animator>().Play("FadeInMenu");
            return;
        }

        // Normal intro setup here
        studio.GetComponent<Animator>().Play("FadeIn");
        presentsText.GetComponent<Animator>().Play("FadeIn");
        studioWaitTime = 5f;
    }



    // Update is called once per frame
    void Update()
    {
        if (skipIntro)
            return;

        if (disclaimer.activeSelf == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                studioWaitTime = 0f;
            }
            if (studioWaitTime <= 0)
            {
                studio.GetComponent<Animator>().Play("FadeOut");
                presentsText.GetComponent<Animator>().Play("FadeOut");
                ShowDisclaimer();
            }
            else
            {
                studioWaitTime -= Time.deltaTime;
            }
        }
        else
        {
            Debug.Log(disclaimerWaitTime);
            if (Input.GetMouseButtonDown(0))
            {
                disclaimerWaitTime = 0f;
            }
            if (disclaimerWaitTime <= 0f)
            {
                disclaimer.GetComponent<Animator>().Play("FadeOut");
                disclaimerText.GetComponent<Animator>().Play("FadeOut");
                background.GetComponent<Animator>().Play("FadeOut");
            }
            if (disclaimerWaitTime <= -1f)
            {
                background.SetActive(false);
                disclaimerText.SetActive(false);
                mainMenu.SetActive(true);
                mainMenu.GetComponent<Animator>().Play("FadeInMenu");
            }
            disclaimerWaitTime -= Time.deltaTime;
        }
    }

    private void ShowDisclaimer()
    {
        disclaimer.SetActive(true);
        disclaimerText.SetActive(true);
        disclaimer.GetComponent<Animator>().Play("FadeIn");
        disclaimerText.GetComponent<Animator>().Play("FadeIn");
        disclaimerWaitTime = 20f;
    }
}
