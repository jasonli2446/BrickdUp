using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Called by "Start Game" button
    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    // Called by "Level Select" button
    public void OpenLevelSelector()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void ResetProgress()
    {
        // Clear PlayerPrefs data
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // Reset the InventoryManager's in-memory state
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.ResetAllCollectedCoins();
            InventoryManager.Instance.ResetBlockCount(0);
        }

        SceneManager.LoadScene("MainMenu");
    }

    // Called by "Quit Game" button
    public void QuitGame()
    {
        Application.Quit();
    }
}
