using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class TilePainter : MonoBehaviour
{
    [Header("Tile Painting Settings")]
    [SerializeField] private Tilemap targetTilemap;
    [SerializeField] private Tilemap previewTilemap;
    [SerializeField] private TileBase tileToPaint;
    [SerializeField] private TileBase previewTile;
    [SerializeField] private TileBase previewTileOutOfRange;
    [SerializeField] private float maxPaintDistance = 3f;
    [SerializeField] private Transform playerTransform;

    [Header("Preview Feedback")]
    [SerializeField] private float flickerDuration = 0.1f;
    [SerializeField] private float previewOpacity = 0.3f;

    [Header("Old Style Settings")]
    [SerializeField] private Color validColor = new Color(0, 1, 0, 0.7f);
    [SerializeField] private Color invalidColor = new Color(1, 0, 0, 0.7f);
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip placeBlockSound;

    private Collider2D playerCollider;
    private Vector3 mouseWorldPos;
    private Vector3Int tileCellPos;
    private bool tryPlaceBlock = false;
    private bool placedThisFrame = false;
    private bool useOldStyle = false;
    private GameObject cursorIndicator;
    
    // Track clicks for debugging
    private int clickCount = 0;
    // Add frame protection to prevent multiple calls in the same frame
    private int lastPlacementFrame = -1;

    void Start()
    {
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            playerTransform = playerObj != null ? playerObj.transform : transform;
        }

        playerCollider = playerTransform.GetComponent<Collider2D>();
        if (!playerCollider)
        {
            Debug.LogError("TilePainter: Player is missing a Collider2D component!");
        }

        if (targetTilemap == null)
        {
            targetTilemap = FindObjectOfType<Tilemap>();
        }

        if (tileToPaint == null || previewTile == null || previewTileOutOfRange == null)
        {
            Debug.LogError("TilePainter: Missing required tiles!");
        }

        if (previewTilemap == null)
        {
            Debug.LogWarning("TilePainter: No previewTilemap, preview disabled in new style.");
        }

        CreateCursorIndicator();
    }

    void CreateCursorIndicator()
    {
        cursorIndicator = new GameObject("CursorIndicator");
        var renderer = cursorIndicator.AddComponent<SpriteRenderer>();
        renderer.sprite = Sprite.Create(TextureCircle(16), new Rect(0, 0, 16, 16), new Vector2(0.5f, 0.5f));
        renderer.sortingOrder = 100;
        cursorIndicator.transform.localScale = Vector3.one * 2f;
        cursorIndicator.SetActive(false);
    }

    Texture2D TextureCircle(int size)
    {
        Texture2D texture = new Texture2D(size, size);
        Color[] pixels = new Color[size * size];
        float radius = size / 2f;
        for (int i = 0; i < pixels.Length; i++)
        {
            int x = i % size;
            int y = i / size;
            float dx = x - radius + 0.5f;
            float dy = y - radius + 0.5f;
            pixels[i] = (dx * dx + dy * dy <= radius * radius) ? Color.white : Color.clear;
        }
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        if (Input.GetMouseButtonDown(0)) 
        {
            tryPlaceBlock = true;
            clickCount++;
            Debug.Log($"Mouse clicked (Count: {clickCount})");
        }
        
        if (Input.GetMouseButtonDown(1)) useOldStyle = !useOldStyle;
    }

    void LateUpdate()
    {
        // Reset placement flag at start of frame
        placedThisFrame = false;
        
        // ✅ Immediately reset to prevent double execution
        bool doPlaceBlock = tryPlaceBlock;
        tryPlaceBlock = false;

        Camera cam = Camera.main;
        if (!cam || !playerTransform || !tileToPaint || !targetTilemap) return;

        Vector3 screen = Input.mousePosition;
        screen.z = -cam.transform.position.z;
        mouseWorldPos = cam.ScreenToWorldPoint(screen);
        mouseWorldPos.z = 0f;

        tileCellPos = targetTilemap.WorldToCell(mouseWorldPos);
        Vector3 tileWorldCenter = targetTilemap.GetCellCenterWorld(tileCellPos);

        float distance = Vector2.Distance(playerTransform.position, tileWorldCenter);
        bool inRange = distance <= (maxPaintDistance + 0.4f);

        bool alreadyOccupied = targetTilemap.HasTile(tileCellPos);
        bool overlapsPlayer = playerCollider != null && playerCollider.bounds.Intersects(new Bounds(tileWorldCenter, targetTilemap.cellSize));
        bool hasBlocks = InventoryManager.Instance.currentBlockCount > 0;

        bool canPlace = inRange && !alreadyOccupied && !overlapsPlayer && hasBlocks;

        if (useOldStyle)
        {
            if (cursorIndicator != null)
            {
                cursorIndicator.transform.position = tileWorldCenter;
                cursorIndicator.SetActive(true);
                cursorIndicator.GetComponent<SpriteRenderer>().color = canPlace ? validColor : invalidColor;
            }

            previewTilemap?.ClearAllTiles();

            if (doPlaceBlock && canPlace)
            {
                PlaceTile(tileCellPos, tileWorldCenter, clickCount);
            }
        }
        else
        {
            if (cursorIndicator != null) cursorIndicator.SetActive(false);

            // Only update preview if we haven't placed a tile this frame
            if (previewTilemap != null && !placedThisFrame)
            {
                previewTilemap.color = new Color(1f, 1f, 1f, previewOpacity);
                previewTilemap.ClearAllTiles();
                previewTilemap.SetTile(tileCellPos, canPlace ? previewTile : previewTileOutOfRange);
            }

            if (doPlaceBlock && canPlace)
            {
                PlaceTile(tileCellPos, tileWorldCenter, clickCount);
                if (!placedThisFrame) // Make sure we call StartCoroutine only once
                {
                    StartCoroutine(FlickerPreview());
                }
            }
        }
    }

    void PlaceTile(Vector3Int cellPos, Vector3 tileWorldCenter, int clickNum)
    {
        // Check if we're trying to place in the same frame
        if (lastPlacementFrame == Time.frameCount)
        {
            Debug.LogWarning($"Prevented duplicate tile placement in frame {Time.frameCount}!");
            return;
        }
        
        lastPlacementFrame = Time.frameCount;
        placedThisFrame = true;
        
        Debug.Log($"PlaceTile called on click #{clickNum}, frame {Time.frameCount}, blocks before: {InventoryManager.Instance.currentBlockCount}");

        if (InventoryManager.Instance.currentBlockCount <= 0)
        {
            Debug.Log("No blocks left to place!");
            return;
        }

        // Only set the tile if it doesn't already exist
        if (!targetTilemap.HasTile(cellPos))
        {
            targetTilemap.SetTile(cellPos, tileToPaint);
            
            GameObject newBlockObject = new GameObject("PlacedBlock");
            newBlockObject.transform.position = tileWorldCenter;
            newBlockObject.tag = "placed";
            InventoryManager.Instance.OnBlockPlaced(newBlockObject);
            
            // Decrement block count exactly once
            //InventoryManager.Instance.currentBlockCount--;
            
            if (audioSource != null && placeBlockSound != null)
            {
                audioSource.PlayOneShot(placeBlockSound);
            }
            
            Debug.Log($"Block placed at {cellPos}. Remaining: {InventoryManager.Instance.currentBlockCount}");
        }
    }

    IEnumerator FlickerPreview()
    {
        previewTilemap.ClearAllTiles();
        yield return new WaitForSeconds(flickerDuration);
    }
}