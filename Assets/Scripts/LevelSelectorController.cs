using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelSelectorController : MonoBehaviour
{
  [Tooltip("Drag your level buttons here in ascending order")]
  public Button[] levelButtons;
  [Tooltip("Type the exact scene names in the same order")]
  public string[] levelSceneNames;

  void Start()
  {
    for (int i = 0; i < levelButtons.Length; i++)
    {
      Button btn = levelButtons[i];
      TMP_Text txt = btn.GetComponentInChildren<TMP_Text>();
      bool unlocked = PlayerPrefs.GetInt("Unlocked_" + levelSceneNames[i], i == 0 ? 1 : 0) == 1;

      // Clear any existing listeners
      btn.onClick.RemoveAllListeners();

      if (unlocked)
      {
        // Show level name and enable button
        if (txt != null)
        {
          // Format the level name with a space between "Level" and the number
          string levelName = levelSceneNames[i];
          if (levelName.StartsWith("Level"))
          {
            string levelNumber = levelName.Substring(5);
            txt.text = "Level " + levelNumber;
          }
          else
          {
            txt.text = levelName; // Fallback to original name if it doesn't follow pattern
          }
          txt.color = Color.white;
        }
        btn.interactable = true;

        // Capture index for the click handler
        int idx = i;
        btn.onClick.AddListener(() => LoadLevel(idx));
      }
      else
      {
        // Show "Locked!" and disable button
        if (txt != null)
        {
          txt.text = "Locked!";
          txt.color = Color.gray;
        }
        btn.interactable = false;
      }
    }
  }

  public void LoadLevel(int index)
  {
    SceneManager.LoadScene(levelSceneNames[index]);
  }

  public void BackToMainMenu()
  {
    SceneManager.LoadScene("MainMenu");
  }
}
