using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Collider2D))]
public class ZoneInventoryController : MonoBehaviour
{
  [Tooltip("How many blocks the player gets when entering this zone")]
  public int allowedBlockCount = 3;

  [Tooltip("The tilemap that contains the placed blocks")]
  public Tilemap targetTilemap;

  private void Awake()
  {
    // Find tilemap if not assigned
    if (targetTilemap == null)
    {
      targetTilemap = FindObjectOfType<Tilemap>();
      if (targetTilemap == null)
      {
        Debug.LogError("No Tilemap found in scene. Please assign one to ZoneInventoryController.");
      }
    }
  }

  private void ResetZone()
  {
    if (InventoryManager.Instance != null)
    {
      // Find all GameObjects with "placed" tag
      GameObject[] placedBlocks = GameObject.FindGameObjectsWithTag("placed");

      // Clear both the tile in the tilemap and the GameObject
      foreach (GameObject block in placedBlocks)
      {
        // Convert the block's world position to a cell position in the tilemap
        if (targetTilemap != null)
        {
          Vector3Int cellPosition = targetTilemap.WorldToCell(block.transform.position);
          targetTilemap.SetTile(cellPosition, null); // Clear the tile
        }

        // Destroy the tracker GameObject
        Destroy(block);
      }

      // Original inventory manager cleanup (keep this too)
      InventoryManager.Instance.ClearPlacedBlocks();
      InventoryManager.Instance.ResetBlockCount(allowedBlockCount);
    }
    else
    {
      Debug.LogError("InventoryManager.Instance is null in ZoneInventoryController");
    }
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Player"))
    {
      Debug.Log("Player entered zone - resetting");
      ResetZone();
    }
  }
}
