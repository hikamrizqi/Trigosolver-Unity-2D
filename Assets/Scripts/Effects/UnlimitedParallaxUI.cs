using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Unlimited parallax untuk UI Canvas (Image/RawImage)
/// Cocok untuk Main Menu background dengan seamless looping
/// </summary>
public class UnlimitedParallaxUI : MonoBehaviour
{
    [Header("Parallax Settings")]
    [Tooltip("Kecepatan scroll horizontal (negatif = kiri, positif = kanan)")]
    [SerializeField] private float scrollSpeedX = -50f;

    [Tooltip("Kecepatan scroll vertikal (negatif = bawah, positif = atas)")]
    [SerializeField] private float scrollSpeedY = 0f;

    [Header("Looping Settings")]
    [Tooltip("Lebar untuk reset position (auto-detect dari RectTransform jika 0)")]
    [SerializeField] private float resetWidth = 0f;

    [Tooltip("Auto-create clone untuk seamless loop")]
    [SerializeField] private bool autoCreateClone = true;

    [Header("Debug")]
    [SerializeField] private GameObject backgroundClone;

    private RectTransform rectTransform;
    private Image imageComponent;
    private RawImage rawImageComponent;
    private Vector2 startAnchoredPosition;
    private float actualResetWidth;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        imageComponent = GetComponent<Image>();
        rawImageComponent = GetComponent<RawImage>();

        if (rectTransform == null)
        {
            Debug.LogError($"[UnlimitedParallaxUI] {gameObject.name} tidak memiliki RectTransform!");
            enabled = false;
            return;
        }

        // Auto-detect width jika belum di-set
        if (resetWidth <= 0f)
        {
            actualResetWidth = rectTransform.rect.width;
        }
        else
        {
            actualResetWidth = resetWidth;
        }

        startAnchoredPosition = rectTransform.anchoredPosition;

        // Auto-create clone jika enabled
        if (autoCreateClone && backgroundClone == null)
        {
            CreateClone();
        }

        Debug.Log($"[UnlimitedParallaxUI] Started on {gameObject.name}");
        Debug.Log($"  - Speed: {scrollSpeedX} px/s");
        Debug.Log($"  - Reset Width: {actualResetWidth}");
        Debug.Log($"  - Start Position: {startAnchoredPosition}");
    }

    private void Update()
    {
        if (rectTransform == null) return;

        // Gerakkan background
        float moveX = scrollSpeedX * Time.deltaTime;
        float moveY = scrollSpeedY * Time.deltaTime;

        rectTransform.anchoredPosition += new Vector2(moveX, moveY);

        // Loop horizontal
        if (Mathf.Abs(scrollSpeedX) > 0.01f)
        {
            float currentX = rectTransform.anchoredPosition.x;

            if (scrollSpeedX < 0)
            {
                // Bergerak ke kiri
                if (currentX <= startAnchoredPosition.x - actualResetWidth)
                {
                    rectTransform.anchoredPosition = new Vector2(
                        startAnchoredPosition.x,
                        rectTransform.anchoredPosition.y
                    );
                }
            }
            else
            {
                // Bergerak ke kanan
                if (currentX >= startAnchoredPosition.x + actualResetWidth)
                {
                    rectTransform.anchoredPosition = new Vector2(
                        startAnchoredPosition.x,
                        rectTransform.anchoredPosition.y
                    );
                }
            }
        }

        // Loop vertikal (jika ada scroll Y)
        if (Mathf.Abs(scrollSpeedY) > 0.01f)
        {
            float currentY = rectTransform.anchoredPosition.y;
            float resetHeight = rectTransform.rect.height;

            if (scrollSpeedY < 0)
            {
                if (currentY <= startAnchoredPosition.y - resetHeight)
                {
                    rectTransform.anchoredPosition = new Vector2(
                        rectTransform.anchoredPosition.x,
                        startAnchoredPosition.y
                    );
                }
            }
            else
            {
                if (currentY >= startAnchoredPosition.y + resetHeight)
                {
                    rectTransform.anchoredPosition = new Vector2(
                        rectTransform.anchoredPosition.x,
                        startAnchoredPosition.y
                    );
                }
            }
        }
    }

    /// <summary>
    /// Buat clone background untuk seamless loop
    /// </summary>
    private void CreateClone()
    {
        // Duplicate GameObject
        backgroundClone = Instantiate(gameObject, transform.parent);
        backgroundClone.name = gameObject.name + "_Clone";

        RectTransform cloneRect = backgroundClone.GetComponent<RectTransform>();

        // PENTING: Set sibling index agar clone render DI BELAKANG original
        // Di UI Canvas, index lebih kecil = render lebih dulu (di belakang)
        int originalSiblingIndex = transform.GetSiblingIndex();
        backgroundClone.transform.SetSiblingIndex(originalSiblingIndex);
        // Original akan otomatis maju 1 index (jadi di depan clone)

        // Posisikan clone di sebelah original
        Vector2 cloneOffset = Vector2.zero;

        if (scrollSpeedX < 0)
        {
            // Scroll ke kiri, clone di kanan
            cloneOffset.x = actualResetWidth;
        }
        else if (scrollSpeedX > 0)
        {
            // Scroll ke kanan, clone di kiri
            cloneOffset.x = -actualResetWidth;
        }

        cloneRect.anchoredPosition = startAnchoredPosition + cloneOffset;

        // Disable auto-create di clone untuk mencegah infinite loop
        UnlimitedParallaxUI cloneScript = backgroundClone.GetComponent<UnlimitedParallaxUI>();
        if (cloneScript != null)
        {
            cloneScript.autoCreateClone = false;
            cloneScript.backgroundClone = null;
            cloneScript.scrollSpeedX = scrollSpeedX;
            cloneScript.scrollSpeedY = scrollSpeedY;
            cloneScript.resetWidth = resetWidth;
        }

        Debug.Log($"[UnlimitedParallaxUI] Clone created: {backgroundClone.name} at offset {cloneOffset}, SiblingIndex: {backgroundClone.transform.GetSiblingIndex()}");
    }

    /// <summary>
    /// Set kecepatan scroll runtime
    /// </summary>
    public void SetScrollSpeed(float speedX, float speedY)
    {
        scrollSpeedX = speedX;
        scrollSpeedY = speedY;

        if (backgroundClone != null)
        {
            UnlimitedParallaxUI cloneScript = backgroundClone.GetComponent<UnlimitedParallaxUI>();
            if (cloneScript != null)
            {
                cloneScript.SetScrollSpeed(speedX, speedY);
            }
        }
    }

    /// <summary>
    /// Pause/Resume parallax
    /// </summary>
    public void SetPaused(bool paused)
    {
        enabled = !paused;

        if (backgroundClone != null)
        {
            UnlimitedParallaxUI cloneScript = backgroundClone.GetComponent<UnlimitedParallaxUI>();
            if (cloneScript != null)
            {
                cloneScript.enabled = !paused;
            }
        }
    }

    private void OnDestroy()
    {
        // Destroy clone jika ada
        if (backgroundClone != null && Application.isPlaying)
        {
            Destroy(backgroundClone);
        }
    }
}
