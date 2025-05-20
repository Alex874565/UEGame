using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;

    [SerializeField] private GameObject loadingScreen;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject quitPanel;

    private bool isPaused = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            HandlePause();
        }
    }

    public void HandlePause()
    {
        if(isPaused)
        {
            Unpause();
        }
        else
        {
            Pause();
        }
        isPaused = !isPaused;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        Cursor.visible = true;
        loadingScreen.SetActive(true);
        SceneManager.LoadScene(0);
    }

    private void Unpause()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        quitPanel.SetActive(false);
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }
}
