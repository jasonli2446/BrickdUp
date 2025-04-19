using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CoinController : MonoBehaviour
{
    [SerializeField] private string coinID;
    [Tooltip("ID of the coin, used for persistence")]

    void Start()
    {
        // If this coin was already collected, disable it
        if (InventoryManager.Instance.IsCoinCollected(coinID))
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        InventoryManager.Instance.MarkCoinAsCollected(coinID);
        gameObject.SetActive(false);
    }
}
