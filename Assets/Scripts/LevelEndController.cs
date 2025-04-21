using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndController : MonoBehaviour
{
    bool triggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;
        UnlockAndLoadNextLevel();
    }

    void UnlockAndLoadNextLevel()
    {
        // 1) Determine build‐index of current & next scenes
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        // 2) If there *is* a next scene, unlock it and load it
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            // a) Get its name
            string path = SceneUtility.GetScenePathByBuildIndex(nextIndex);
            string nextName = System.IO.Path.GetFileNameWithoutExtension(path);

            // b) Mark unlocked in PlayerPrefs
            PlayerPrefs.SetInt("Unlocked_" + nextName, 1);
            PlayerPrefs.Save();

            // c) Immediately load the next level
            SceneManager.LoadScene(nextName);
        }
    }
}
