using UnityEngine;

/// <summary>
/// Unlimited parallax effect untuk background yang bergerak terus menerus (looping seamless)
/// Cocok untuk main menu dengan background yang bergerak infinite
/// </summary>
public class UnlimitedParallax : MonoBehaviour
{
    [Header("Parallax Settings")]
    [Tooltip("Kecepatan scroll horizontal (negatif = kiri, positif = kanan)")]
    [SerializeField] private float scrollSpeedX = 1f;

    [Tooltip("Kecepatan scroll vertikal (negatif = bawah, positif = atas)")]
    [SerializeField] private float scrollSpeedY = 0f;

    [Header("Looping Settings")]
    [Tooltip("Panjang sprite untuk reset position (auto-detect jika 0)")]
    [SerializeField] private float spriteWidth = 0f;

    [Tooltip("Tinggi sprite untuk reset position (auto-detect jika 0)")]
    [SerializeField] private float spriteHeight = 0f;

    [Header("Optional: Multiple Layers")]
    [Tooltip("Background kedua untuk seamless loop (opsional, auto-create jika null)")]
    [SerializeField] private GameObject backgroundClone;

    private SpriteRenderer spriteRenderer;
    private Vector3 startPosition;
    private float resetPositionX;
    private float resetPositionY;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer != null && (spriteWidth == 0f || spriteHeight == 0f))
        {
            // Auto-detect ukuran sprite
            Bounds bounds = spriteRenderer.bounds;
            if (spriteWidth == 0f) spriteWidth = bounds.size.x;
            if (spriteHeight == 0f) spriteHeight = bounds.size.y;
        }

        startPosition = transform.position;
        
        // Set reset position berdasarkan sprite size
        resetPositionX = spriteWidth;
        resetPositionY = spriteHeight;

        // Auto-create clone jika belum ada
        if (backgroundClone == null && spriteRenderer != null)
        {
            CreateBackgroundClone();
        }

        Debug.Log($"[UnlimitedParallax] Initialized on {gameObject.name}");
        Debug.Log($"  - Speed: ({scrollSpeedX}, {scrollSpeedY})");
        Debug.Log($"  - Sprite Size: {spriteWidth} x {spriteHeight}");
    }

    private void Update()
    {
        // Gerakkan background berdasarkan kecepatan scroll
        float moveX = scrollSpeedX * Time.deltaTime;
        float moveY = scrollSpeedY * Time.deltaTime;

        transform.position += new Vector3(moveX, moveY, 0);

        // Loop horizontal (reset jika sudah bergeser sejauh sprite width)
        if (Mathf.Abs(scrollSpeedX) > 0.01f)
        {
            if (scrollSpeedX < 0 && transform.position.x <= startPosition.x - resetPositionX)
            {
                // Bergerak ke kiri, reset ke kanan
                transform.position = new Vector3(
                    startPosition.x,
                    transform.position.y,
                    transform.position.z
                );
            }
            else if (scrollSpeedX > 0 && transform.position.x >= startPosition.x + resetPositionX)
            {
                // Bergerak ke kanan, reset ke kiri
                transform.position = new Vector3(
                    startPosition.x,
                    transform.position.y,
                    transform.position.z
                );
            }
        }

        // Loop vertikal (reset jika sudah bergeser sejauh sprite height)
        if (Mathf.Abs(scrollSpeedY) > 0.01f)
        {
            if (scrollSpeedY < 0 && transform.position.y <= startPosition.y - resetPositionY)
            {
                // Bergerak ke bawah, reset ke atas
                transform.position = new Vector3(
                    transform.position.x,
                    startPosition.y,
                    transform.position.z
                );
            }
            else if (scrollSpeedY > 0 && transform.position.y >= startPosition.y + resetPositionY)
            {
                // Bergerak ke atas, reset ke bawah
                transform.position = new Vector3(
                    transform.position.x,
                    startPosition.y,
                    transform.position.z
                );
            }
        }
    }

    /// <summary>
    /// Membuat clone background untuk seamless looping
    /// </summary>
    private void CreateBackgroundClone()
    {
        // Duplicate sprite untuk seamless loop
        backgroundClone = new GameObject(gameObject.name + "_Clone");
        backgroundClone.transform.parent = transform.parent;
        backgroundClone.transform.localScale = transform.localScale;
        backgroundClone.transform.rotation = transform.rotation;

        // Copy sprite renderer
        SpriteRenderer cloneRenderer = backgroundClone.AddComponent<SpriteRenderer>();
        cloneRenderer.sprite = spriteRenderer.sprite;
        cloneRenderer.color = spriteRenderer.color;
        cloneRenderer.sortingLayerName = spriteRenderer.sortingLayerName;
        cloneRenderer.sortingOrder = spriteRenderer.sortingOrder;

        // Posisikan clone bersebelahan dengan original
        Vector3 cloneOffset = Vector3.zero;
        
        if (Mathf.Abs(scrollSpeedX) > 0.01f)
        {
            // Scroll horizontal
            cloneOffset.x = scrollSpeedX < 0 ? spriteWidth : -spriteWidth;
        }
        
        if (Mathf.Abs(scrollSpeedY) > 0.01f)
        {
            // Scroll vertical
            cloneOffset.y = scrollSpeedY < 0 ? spriteHeight : -spriteHeight;
        }

        backgroundClone.transform.position = startPosition + cloneOffset;

        // Add parallax script ke clone dengan posisi offset
        UnlimitedParallax cloneScript = backgroundClone.AddComponent<UnlimitedParallax>();
        cloneScript.scrollSpeedX = scrollSpeedX;
        cloneScript.scrollSpeedY = scrollSpeedY;
        cloneScript.spriteWidth = spriteWidth;
        cloneScript.spriteHeight = spriteHeight;
        cloneScript.backgroundClone = null; // Jangan auto-create lagi

        Debug.Log($"[UnlimitedParallax] Auto-created clone: {backgroundClone.name}");
    }

    /// <summary>
    /// Set kecepatan scroll secara runtime
    /// </summary>
    public void SetScrollSpeed(float speedX, float speedY)
    {
        scrollSpeedX = speedX;
        scrollSpeedY = speedY;

        // Update clone juga
        if (backgroundClone != null)
        {
            UnlimitedParallax cloneScript = backgroundClone.GetComponent<UnlimitedParallax>();
            if (cloneScript != null)
            {
                cloneScript.scrollSpeedX = speedX;
                cloneScript.scrollSpeedY = speedY;
            }
        }
    }

    /// <summary>
    /// Pause/Resume parallax effect
    /// </summary>
    public void SetPaused(bool paused)
    {
        enabled = !paused;

        if (backgroundClone != null)
        {
            UnlimitedParallax cloneScript = backgroundClone.GetComponent<UnlimitedParallax>();
            if (cloneScript != null)
            {
                cloneScript.enabled = !paused;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualisasi area reset di editor
        Gizmos.color = Color.yellow;
        
        Vector3 pos = Application.isPlaying ? startPosition : transform.position;
        
        // Draw reset boundaries
        if (spriteWidth > 0)
        {
            Gizmos.DrawLine(
                pos + new Vector3(-spriteWidth, -1, 0),
                pos + new Vector3(-spriteWidth, 1, 0)
            );
            Gizmos.DrawLine(
                pos + new Vector3(spriteWidth, -1, 0),
                pos + new Vector3(spriteWidth, 1, 0)
            );
        }
        
        if (spriteHeight > 0)
        {
            Gizmos.DrawLine(
                pos + new Vector3(-1, -spriteHeight, 0),
                pos + new Vector3(1, -spriteHeight, 0)
            );
            Gizmos.DrawLine(
                pos + new Vector3(-1, spriteHeight, 0),
                pos + new Vector3(1, spriteHeight, 0)
            );
        }
    }
}
