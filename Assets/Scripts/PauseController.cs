using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PauseController : MonoBehaviour
{
    public GameObject pauseMenuUI;
    bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenuUI.SetActive(isPaused);
        // Reset all button animators when menu opens
        if (isPaused)
            ResetButtonAnimations();
        Time.timeScale = isPaused ? 0f : 1f;
    }

    void ResetButtonAnimations()
    {
        // Find every Button under the pause menu
        foreach (var btn in pauseMenuUI.GetComponentsInChildren<Button>(true))
        {
            // Grab its Animator (your animated UIButton_Template)
            var anim = btn.GetComponent<Animator>();
            if (anim == null) continue;

            // Immediately jump to the "Normal" state at time = 0
            anim.Play("UIButton_Normal", 0, 0f);
            // Force the Animator to apply that change right now
            anim.Update(0f);
            // Also clear any lingering trigger
            anim.ResetTrigger("UIButton_Pressed");
        }
    }

    public void ResumeGame() => TogglePause();

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame() => Application.Quit();
}
