using UnityEngine;
using UnityEngine.Tilemaps;

public class ResetBlocksController : MonoBehaviour
{
    [Tooltip("The tilemap that contains the placed blocks")]
    public Tilemap targetTilemap;

    private void Awake()
    {
        if (targetTilemap == null)
        {
            targetTilemap = FindObjectOfType<Tilemap>();
            if (targetTilemap == null)
                Debug.LogError("No Tilemap found. Please assign one to ResetBlocksController.");
        }
    }

    /// <summary>
    /// Call this from your UI button to clear all placed blocks.
    /// </summary>
    public void ResetBlocks()
    {
        if (InventoryManager.Instance == null)
        {
            Debug.LogError("InventoryManager.Instance is null in ResetBlocksController");
            return;
        }

        // 1) Destroy all placed‑block GameObjects and clear their tiles
        var placedBlocks = GameObject.FindGameObjectsWithTag("placed");
        foreach (var block in placedBlocks)
        {
            if (targetTilemap != null)
            {
                Vector3Int cellPos = targetTilemap.WorldToCell(block.transform.position);
                targetTilemap.SetTile(cellPos, null);
            }
            Destroy(block);
        }

        // 2) Clear InventoryManager’s list & reset count to zero
        InventoryManager.Instance.ClearPlacedBlocks();
        InventoryManager.Instance.ResetBlockCount(0);

        // 3) Reset all give‑zones to unused state
        var giveZones = FindObjectsOfType<GiveZoneController>();
        foreach (var giveZone in giveZones)
            giveZone.ResetZone();
    }
}
