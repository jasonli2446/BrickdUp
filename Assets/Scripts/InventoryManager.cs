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

    // keep track of placed blocks so we can clear them
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

    // Resets the block count to the given amount and updates the UI.
    public void ResetBlockCount(int count)
    {
        currentBlockCount = count;

        // If you have UI elements to update, do that here
        UpdateUI();
    }

    // Call this whenever you successfully place a block.
    public void OnBlockPlaced(GameObject block)
    {
        placedBlocks.Add(block);
        currentBlockCount--;
        UpdateUI();
        Debug.Log("Block placed. Current block count: " + currentBlockCount);
    }

    // Destroys all placed blocks and clears the list.
    public void ClearPlacedBlocks()
    {
        foreach (var block in placedBlocks)
            if (block != null)
                Destroy(block);
        placedBlocks.Clear();
    }

    private void UpdateUI()
    {
        if (blockCountText != null)
            blockCountText.text = currentBlockCount.ToString();
    }
}
