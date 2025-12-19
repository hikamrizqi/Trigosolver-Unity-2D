using UnityEngine;
using TMPro;

/// <summary>
/// Script untuk visualisasi segitiga secara dinamis menggunakan 3 sprites yang tegak lurus
/// Sprites akan diposisikan dan diskala berdasarkan nilai Depan, Samping, dan Miring
/// </summary>
public class TriangleVisualizer : MonoBehaviour
{
    [Header("Sprite References")]
    [Tooltip("Sprite untuk sisi Depan (vertikal)")]
    public SpriteRenderer depanSprite;

    [Tooltip("Sprite untuk sisi Samping (horizontal)")]
    public SpriteRenderer sampingSprite;

    [Tooltip("Sprite untuk sisi Miring (diagonal)")]
    public SpriteRenderer miringSprite;

    [Header("Label References (TextMeshPro UI)")]
    [Tooltip("Label untuk menampilkan nilai sisi Depan")]
    public TextMeshProUGUI depanLabel;

    [Tooltip("Label untuk menampilkan nilai sisi Samping")]
    public TextMeshProUGUI sampingLabel;

    [Tooltip("Label untuk menampilkan nilai sisi Miring")]
    public TextMeshProUGUI miringLabel;

    [Tooltip("Label untuk simbol theta di sudut siku-siku (World Space)")]
    public TextMeshPro thetaLabel;

    [Header("Camera Reference")]
    [Tooltip("Camera untuk konversi world to screen point")]
    public Camera mainCamera;

    [Header("Visual Settings")]
    [Tooltip("Skala dasar untuk sprites (1 = 1 unit Unity per nilai segitiga)")]
    public float baseScale = 0.5f;

    [Tooltip("Posisi pusat segitiga di world space")]
    public Vector3 centerPosition = Vector3.zero;

    [Tooltip("Offset label dari garis (jarak)")]
    public float labelOffset = 0.5f;

    [Tooltip("Ketebalan garis segitiga")]
    public float lineThickness = 0.5f;

    [Tooltip("Gunakan tiling mode untuk sprite agar ujung tidak distorsi")]
    public bool useTiling = false;

    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;

    // Data segitiga saat ini
    private int currentDepan;
    private int currentSamping;
    private int currentMiring;

    /// <summary>
    /// Menggambar segitiga dengan nilai yang diberikan
    /// </summary>
    public void DrawTriangle(int depan, int samping, int miring)
    {
        currentDepan = depan;
        currentSamping = samping;
        currentMiring = miring;

        // Hitung posisi-posisi vertex segitiga
        // Gunakan transform.position sebagai base + centerPosition sebagai offset
        Vector3 basePosition = transform.position + centerPosition;

        // Segitiga siku-siku dengan sudut di kiri bawah
        Vector3 bottomLeft = basePosition;
        Vector3 bottomRight = bottomLeft + new Vector3(samping * baseScale, 0, 0);
        Vector3 topLeft = bottomLeft + new Vector3(0, depan * baseScale, 0);

        // SISI SAMPING (Horizontal - Bottom)
        PositionSprite(sampingSprite, bottomLeft, bottomRight, samping);
        if (sampingLabel != null)
        {
            sampingLabel.text = samping.ToString();
            Vector3 midPoint = (bottomLeft + bottomRight) / 2f;
            PositionUILabel(sampingLabel, midPoint + new Vector3(0, -labelOffset, 0));
        }

        // SISI DEPAN (Vertical - Left)
        PositionSprite(depanSprite, bottomLeft, topLeft, depan);
        if (depanLabel != null)
        {
            depanLabel.text = depan.ToString();
            Vector3 midPoint = (bottomLeft + topLeft) / 2f;
            PositionUILabel(depanLabel, midPoint + new Vector3(-labelOffset, 0, 0));
        }

        // SISI MIRING (Diagonal - Hypotenuse)
        PositionSprite(miringSprite, topLeft, bottomRight, miring);
        if (miringLabel != null)
        {
            miringLabel.text = miring.ToString();
            Vector3 midPoint = (topLeft + bottomRight) / 2f;

            // Hitung perpendicular offset untuk label miring
            Vector3 direction = (bottomRight - topLeft).normalized;
            Vector3 perpendicular = new Vector3(-direction.y, direction.x, 0);
            PositionUILabel(miringLabel, midPoint + perpendicular * labelOffset);
        }

        // SIMBOL THETA (di sudut siku-siku antara depan dan samping) - WORLD SPACE
        if (thetaLabel != null)
        {
            thetaLabel.text = "θ";
            // Posisi theta di sudut kiri bawah (bottomLeft), offset sedikit ke dalam segitiga
            float thetaOffsetDistance = 0.8f; // Jarak dari vertex ke arah dalam segitiga
            Vector3 thetaPosition = bottomLeft + new Vector3(thetaOffsetDistance, thetaOffsetDistance, 0);
            thetaLabel.transform.position = thetaPosition;
        }

        // Reset warna ke normal
        ResetColors();
    }

    /// <summary>
    /// Memposisikan dan merotasi sprite untuk membentuk garis dari start ke end
    /// ASUMSI: Sprite default orientasi VERTIKAL (memanjang di Y-axis)
    /// </summary>
    private void PositionSprite(SpriteRenderer sprite, Vector3 start, Vector3 end, float value)
    {
        if (sprite == null) return;

        // Hitung tengah dan panjang
        Vector3 midPoint = (start + end) / 2f;
        float distance = Vector3.Distance(start, end);

        // Set posisi ke tengah garis
        sprite.transform.position = new Vector3(midPoint.x, midPoint.y, 0f);

        // Hitung rotasi
        Vector3 direction = end - start;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // PENTING: Sprite default vertikal (90°), jadi kurangi 90° untuk alignment
        sprite.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

        // Dapatkan ukuran aktual sprite untuk perhitungan yang lebih akurat
        float spriteHeight = 1f; // Default height sprite (vertikal)
        float spriteWidth = 2f; // Default width sprite
        if (sprite.sprite != null)
        {
            // Gunakan bounds Y karena sprite memanjang di Y
            spriteHeight = sprite.sprite.bounds.size.y;
            // Gunakan bounds X untuk ketebalan
            spriteWidth = sprite.sprite.bounds.size.x;
        }

        // Set scale:
        // Y = panjang garis (karena sprite memanjang di Y)
        // X = ketebalan garis (langsung tanpa normalisasi)
        float scaleY = distance / spriteHeight;
        sprite.transform.localScale = new Vector3(lineThickness, scaleY, 1f);
    }

    /// <summary>
    /// Posisikan UI Label (TextMeshProUGUI) berdasarkan world position
    /// </summary>
    private void PositionUILabel(TextMeshProUGUI label, Vector3 worldPosition)
    {
        if (label == null) return;

        // Gunakan camera untuk konversi world ke screen point
        Camera cam = mainCamera != null ? mainCamera : Camera.main;
        if (cam == null)
        {
            Debug.LogWarning("TriangleVisualizer: Main Camera tidak ditemukan!");
            return;
        }

        // Konversi world position ke screen point
        Vector3 screenPoint = cam.WorldToScreenPoint(worldPosition);

        // Set posisi RectTransform
        RectTransform rectTransform = label.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.position = screenPoint;
        }
    }

    /// <summary>
    /// Highlight salah satu sisi segitiga dengan warna tertentu
    /// </summary>
    public void HighlightSide(string sideName, Color color)
    {
        ResetColors();

        switch (sideName.ToLower())
        {
            case "depan":
                if (depanSprite != null) depanSprite.color = color;
                break;
            case "samping":
                if (sampingSprite != null) sampingSprite.color = color;
                break;
            case "miring":
                if (miringSprite != null) miringSprite.color = color;
                break;
        }
    }

    /// <summary>
    /// Reset semua warna ke warna normal
    /// </summary>
    public void ResetColors()
    {
        if (depanSprite != null) depanSprite.color = normalColor;
        if (sampingSprite != null) sampingSprite.color = normalColor;
        if (miringSprite != null) miringSprite.color = normalColor;
    }

    /// <summary>
    /// Highlight sisi dengan warna highlight default (kuning)
    /// </summary>
    public void HighlightSide(string sideName)
    {
        HighlightSide(sideName, highlightColor);
    }

    /// <summary>
    /// Tampilkan feedback warna pada sisi tertentu (hijau untuk benar, merah untuk salah)
    /// </summary>
    public void ShowFeedback(string sideName, bool isCorrect)
    {
        Color feedbackColor = isCorrect ? correctColor : wrongColor;
        HighlightSide(sideName, feedbackColor);
    }

    /// <summary>
    /// Update label salah satu sisi (berguna jika ada animasi atau perubahan nilai)
    /// </summary>
    public void UpdateLabel(string sideName, string value)
    {
        switch (sideName.ToLower())
        {
            case "depan":
                if (depanLabel != null) depanLabel.text = value;
                break;
            case "samping":
                if (sampingLabel != null) sampingLabel.text = value;
                break;
            case "miring":
                if (miringLabel != null) miringLabel.text = value;
                break;
        }
    }

    // Debug visualization di Editor
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.cyan;

        Vector3 basePosition = transform.position + centerPosition;
        Vector3 bottomLeft = basePosition;
        Vector3 bottomRight = bottomLeft + new Vector3(currentSamping * baseScale, 0, 0);
        Vector3 topLeft = bottomLeft + new Vector3(0, currentDepan * baseScale, 0);

        // Gambar garis outline segitiga untuk debug
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomLeft, topLeft);
        Gizmos.DrawLine(topLeft, bottomRight);

        // Gambar sphere kecil di setiap vertex
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(bottomLeft, 0.1f);
        Gizmos.DrawSphere(bottomRight, 0.1f);
        Gizmos.DrawSphere(topLeft, 0.1f);
    }

    /// <summary>
    /// Pastikan sprite pivot di center saat Start
    /// </summary>
    private void Start()
    {
        // Auto-assign main camera jika belum
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // Verifikasi sprite references
        if (depanSprite == null || sampingSprite == null || miringSprite == null)
        {
            Debug.LogWarning("TriangleVisualizer: Ada sprite yang belum di-assign di Inspector!");
            return;
        }

        // Debug sprite bounds info
        if (depanSprite.sprite != null)
        {
            Debug.Log($"Sprite '{depanSprite.sprite.name}' bounds size: {depanSprite.sprite.bounds.size}");
            Debug.Log($"Sprite pivot: {depanSprite.sprite.pivot}");
            Debug.Log($"Sprite pixels per unit: {depanSprite.sprite.pixelsPerUnit}");
        }

        // Optional: Gambar segitiga test untuk preview di editor
        if (currentDepan == 0 && currentSamping == 0 && currentMiring == 0)
        {
            // Gambar segitiga default 3-4-5 untuk testing
            DrawTriangle(3, 4, 5);
        }
    }
}
