using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectorController : MonoBehaviour
{
  // Loads a level by number (Level1, Level2, etc.)
  public void LoadLevel(int levelNumber)
  {
    string sceneName = "Level" + levelNumber;
    SceneManager.LoadScene(sceneName);
  }

  // Called by your "Back" button
  public void BackToMainMenu()
  {
    SceneManager.LoadScene("MainMenu");
  }
}
