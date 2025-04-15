using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public AudioSource audioSource;
    public AudioClip menuMusic;
    public AudioClip[] levelMusic; // Index 0 = Level1, 1 = Level2, etc.

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string name = scene.name.ToLower();

        if (name.Contains("menu"))
        {
            PlayMusic(menuMusic);
        }
        else if (name.Contains("level"))
        {
            int levelNumber = GetLevelNumberFromSceneName(name);
            if (levelNumber >= 1 && levelNumber <= levelMusic.Length)
            {
                PlayMusic(levelMusic[levelNumber - 1]);
            }
        }
    }

    int GetLevelNumberFromSceneName(string sceneName)
    {
        // Assumes your scenes are named like "Level1", "Level2", etc.
        string digits = System.Text.RegularExpressions.Regex.Match(sceneName, @"\d+").Value;
        int.TryParse(digits, out int result);
        return result;
    }

    void PlayMusic(AudioClip clip)
    {
        if (audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}
