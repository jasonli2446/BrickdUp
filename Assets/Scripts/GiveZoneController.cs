using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GiveZoneController : MonoBehaviour
{
  [Tooltip("How many blocks to give when player enters")]
  public int giveAmount = 3;

  private bool used = false;

  public void ResetZone()
  {
    // Reset the used state so the zone can give blocks again
    used = false;
    Debug.Log($"Give zone {gameObject.name} reset to unused state");
    Animator animator = GetComponentInChildren<Animator>();
    animator.SetBool("open", false);
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (used || !other.CompareTag("Player")) return;

    InventoryManager.Instance.ResetBlockCount(giveAmount);
    used = true;

    Animator animator = GetComponentInChildren<Animator>();
    animator.SetBool("open", true);
    animator.SetTrigger("entered");
  }
}
