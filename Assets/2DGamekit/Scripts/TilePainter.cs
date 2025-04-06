using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePainter : MonoBehaviour
{
    [Header("Tile Painting Settings")]
    [SerializeField] private Tilemap targetTilemap;
    [SerializeField] private TileBase tileToPaint;
    [SerializeField] private float maxPaintDistance = 3f;
    [SerializeField] private Transform playerTransform;

    [Header("Visual Feedback")]
    [SerializeField] private Color validColor = new Color(0, 1, 0, 0.7f);
    [SerializeField] private Color invalidColor = new Color(1, 0, 0, 0.7f);

    private GameObject cursorIndicator;
    private Vector3 mouseWorldPos;
    private Vector3Int tileCellPos;
    private bool tryPlaceBlock = false;

    void Start()
    {
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            playerTransform = playerObj != null ? playerObj.transform : transform;
        }

        if (targetTilemap == null)
        {
            targetTilemap = FindObjectOfType<Tilemap>();
        }

        if (tileToPaint == null)
        {
            Debug.LogError("TilePainter: No tile assigned!");
        }

        CreateCursorIndicator();
    }

    void CreateCursorIndicator()
    {
        cursorIndicator = new GameObject("CursorIndicator");
        cursorIndicator.transform.SetParent(null);

        SpriteRenderer renderer = cursorIndicator.AddComponent<SpriteRenderer>();
        Texture2D texture = new Texture2D(16, 16);
        Color[] pixels = new Color[16 * 16];
        float radius = 8f;

        for (int i = 0; i < pixels.Length; i++)
        {
            int x = i % 16;
            int y = i / 16;
            float dx = x - radius + 0.5f;
            float dy = y - radius + 0.5f;
            pixels[i] = (dx * dx + dy * dy <= radius * radius) ? Color.white : Color.clear;
        }

        texture.SetPixels(pixels);
        texture.Apply();

        renderer.sprite = Sprite.Create(texture, new Rect(0, 0, 16, 16), new Vector2(0.5f, 0.5f));
        renderer.sortingOrder = 100;
        renderer.color = validColor;

        cursorIndicator.transform.localScale = Vector3.one * 2f;
    }

    void Update()
    {
        // Just check for mouse click in Update — raw input is most accurate here
        if (Input.GetMouseButtonDown(0))
        {
            tryPlaceBlock = true;
        }
    }

    void LateUpdate()
    {
        Camera cam = Camera.main;
        if (!cam || !playerTransform || !tileToPaint || !targetTilemap) return;

        // Mouse world position AFTER camera moves
        Vector3 screen = Input.mousePosition;
        screen.z = -cam.transform.position.z;
        mouseWorldPos = cam.ScreenToWorldPoint(screen);
        mouseWorldPos.z = 0;

        tileCellPos = targetTilemap.WorldToCell(mouseWorldPos);
        cursorIndicator.transform.position = mouseWorldPos;

        // Distance from player to tile cell center (not raw mouse!)
        Vector3 tileWorldCenter = targetTilemap.GetCellCenterWorld(tileCellPos);
        float distance = Vector2.Distance(playerTransform.position, tileWorldCenter);

        // Optional: Relaxed buffer for smoother placement while jumping
        float buffer = 0.4f;
        bool inRange = distance <= (maxPaintDistance + buffer);

        // Update dot color
        var rend = cursorIndicator.GetComponent<SpriteRenderer>();
        rend.color = inRange ? validColor : invalidColor;

        // Now place tile if requested
        if (tryPlaceBlock && inRange)
        {
            targetTilemap.SetTile(tileCellPos, tileToPaint);
            Debug.Log($"Placed tile at {tileCellPos} (distance: {distance:F2})");
        }

        tryPlaceBlock = false;
    }
}
