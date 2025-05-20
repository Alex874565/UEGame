using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadGame()
    {
        loadingScreen.SetActive(true);
        SceneManager.LoadScene(2);
    }
}
