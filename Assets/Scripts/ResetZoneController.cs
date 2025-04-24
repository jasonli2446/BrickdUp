using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Collider2D))]
public class ResetZoneController : MonoBehaviour
{
    [Tooltip("The tilemap that contains the placed blocks")]
    public Tilemap targetTilemap;

    private void Awake()
    {
        // Auto‑assign the Tilemap if you forgot
        if (targetTilemap == null)
        {
            targetTilemap = FindObjectOfType<Tilemap>();
            if (targetTilemap == null)
                Debug.LogError("No Tilemap found. Please assign one to ResetZoneController.");
        }
    }

    private void ResetZone()
    {
        
        if (InventoryManager.Instance == null)
        {
            Debug.LogError("InventoryManager.Instance is null in ResetZoneController");
            return;
        }

        // 1) Destroy all placed‑block GameObjects and clear their tiles
        var placedBlocks = GameObject.FindGameObjectsWithTag("placed");
        foreach (var block in placedBlocks)
        {
            if (targetTilemap != null)
            {
                // Convert world pos to cell pos and clear the tile
                Vector3Int cellPos = targetTilemap.WorldToCell(block.transform.position);
                targetTilemap.SetTile(cellPos, null);
            }
            Destroy(block);
        }

        // 2) Also clear InventoryManager’s own list & reset count to zero
        InventoryManager.Instance.ClearPlacedBlocks();
        InventoryManager.Instance.ResetBlockCount(0);

        // 3) Reset all give zones to unused state
        GiveZoneController[] giveZones = FindObjectsOfType<GiveZoneController>();
        foreach (GiveZoneController giveZone in giveZones)
        {
            giveZone.ResetZone();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            ResetZone();
    }
}
