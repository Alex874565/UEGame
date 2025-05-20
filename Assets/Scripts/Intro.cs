using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    [SerializeField] private GameObject studio;
    [SerializeField] private GameObject disclaimer;
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject disclaimerText;
    [SerializeField] private GameObject presentsText;

    private float studioWaitTime;
    private float disclaimerWaitTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        studio.GetComponent<Animator>().Play("FadeIn");
        presentsText.GetComponent<Animator>().Play("FadeIn");
        studioWaitTime = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(disclaimer.activeSelf);
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
            }
            if (disclaimerWaitTime <= -2f)
            {
                background.GetComponent<Animator>().Play("FadeOut");
                loadingScreen.SetActive(true);
                SceneManager.LoadScene(1);
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
