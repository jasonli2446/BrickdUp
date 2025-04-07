using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ZoneInventoryController : MonoBehaviour
{
  [Tooltip("How many blocks the player gets when entering this zone")]
  public int allowedBlockCount = 3;

  private void ResetZone()
  {
    if (InventoryManager.Instance != null)
    {
      InventoryManager.Instance.ClearPlacedBlocks();
      InventoryManager.Instance.ResetBlockCount(allowedBlockCount);
    }
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Player"))
    {
      ResetZone();
    }
  }
}
