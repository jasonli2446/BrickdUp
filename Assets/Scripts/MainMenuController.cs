using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Called by "Start Game" button
    public void StartGame()
    {
        SceneManager.LoadScene("Demo");
    }

    // Called by "Level Select" button
    public void OpenLevelSelector()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    // Called by "Quit Game" button
    public void QuitGame()
    {
        Application.Quit();
    }
}
