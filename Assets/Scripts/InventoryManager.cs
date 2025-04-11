using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Inventory UI")]
    public TMP_Text blockCountText;

    [HideInInspector]
    public int currentBlockCount;

    private readonly List<GameObject> placedBlocks = new List<GameObject>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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
}
