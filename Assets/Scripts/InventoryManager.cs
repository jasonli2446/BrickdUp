using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Inventory UI")]
    public TMP_Text blockCountText;

    [HideInInspector]
    public int currentBlockCount;

    public int currentCoinCount;

    private readonly List<GameObject> placedBlocks = new List<GameObject>();

    private HashSet<string> collectedCoinIDs = new HashSet<string>();


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scenes
        }
        else
            Destroy(gameObject);

        UpdateUI();
    }

    void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadCollectedCoins();
        blockCountText = GameObject.Find("BlockCountText").GetComponent<TMP_Text>();
        Debug.Log($"Loaded coins for scene {scene.name}: {collectedCoinIDs.Count} collected");
    }

    void Start()
    {
        UpdateUI();
    }

    public void ResetBlockCount(int newCount)
    {
        currentBlockCount = newCount;
        UpdateUI();
    }

    public void OnBlockPlaced(GameObject block)
    {
        placedBlocks.Add(block);
        currentBlockCount--;
        UpdateUI();
    }

    public void ClearPlacedBlocks()
    {
        foreach (var b in placedBlocks)
            if (b != null) Destroy(b);
        placedBlocks.Clear();
    }

    private void UpdateUI()
    {
        if (blockCountText != null)
            blockCountText.text = currentBlockCount.ToString();
    }

    // Add to InventoryManager.cs
    public void SaveCollectedCoins()
    {
        // Save current coin count
        PlayerPrefs.SetInt("TotalCoins", currentCoinCount);

        // Save which coins were collected in current level
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        string collectedCoinsKey = $"CollectedCoins_{currentScene}";
        PlayerPrefs.SetString(collectedCoinsKey, string.Join(",", collectedCoinIDs));
        PlayerPrefs.Save();
    }

    public void LoadCollectedCoins()
    {
        currentCoinCount = PlayerPrefs.GetInt("TotalCoins", 0);

        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        string collectedCoinsKey = $"CollectedCoins_{currentScene}";
        string savedCoins = PlayerPrefs.GetString(collectedCoinsKey, "");

        if (!string.IsNullOrEmpty(savedCoins))
            collectedCoinIDs = new HashSet<string>(savedCoins.Split(','));
    }

    public bool IsCoinCollected(string coinID)
    {
        return collectedCoinIDs.Contains(coinID);
    }

    public void MarkCoinAsCollected(string coinID)
    {
        currentCoinCount++;
        collectedCoinIDs.Add(coinID);
        SaveCollectedCoins();
        Debug.Log($"Coin {coinID} collected! Total coins: {currentCoinCount}");
    }

}
