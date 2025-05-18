using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadGame()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);

        while(!asyncOperation.isDone)
        {
            // maybe we fill a progress bar here or something
        }
    }
}
