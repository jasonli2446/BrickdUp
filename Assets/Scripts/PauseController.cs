using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [Tooltip("Drag your PauseMenuUI root object here")]
    public GameObject pauseMenuUI;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            // Show menu, then reset button anims, then pause time
            pauseMenuUI.SetActive(true);
            ResetButtonAnimations();
            Time.timeScale = 0f;
        }
        else
        {
            // Before hiding, also reset (so next open is clean)
            ResetButtonAnimations();
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    private void ResetButtonAnimations()
    {
        // Find every Button (even inactive) under the pause menu
        var buttons = pauseMenuUI.GetComponentsInChildren<Button>(true);
        foreach (var btn in buttons)
        {
            var anim = btn.GetComponent<Animator>();
            if (anim == null)
                continue;

            // Jump to the "Normal" state at time=0
            anim.Play("UIButton_Normal", 0, 0f);
            // Force the Animator to update immediately
            anim.Update(0f);
            // Clear any lingering "Pressed" trigger
            anim.ResetTrigger("Pressed");
        }
    }

    // Hooked to your Resume button
    public void ResumeGame() => TogglePause();

    // Hooked to your Main Menu button
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // Hooked to your Quit button
    public void QuitGame() => Application.Quit();
}
