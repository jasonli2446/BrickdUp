using UnityEngine;
using UnityEngine.SceneManagement;
using Gamekit2D;

public class GameOverController : MonoBehaviour
{
    public GameObject gameOverUI;
    private bool triggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (other.GetComponent<PlayerCharacter>() == null) return;

        triggered = true;
        ShowGameOver();
    }

    void ShowGameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level1");
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
