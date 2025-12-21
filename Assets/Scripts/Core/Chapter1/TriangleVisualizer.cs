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

    [Header("Label References (TextMeshPro World Space)")]
    [Tooltip("Label untuk menampilkan nilai sisi Depan (World Space)")]
    public TextMeshPro depanLabel;

    [Tooltip("Label untuk menampilkan nilai sisi Samping (World Space)")]
    public TextMeshPro sampingLabel;

    [Tooltip("Label untuk menampilkan nilai sisi Miring (World Space)")]
    public TextMeshPro miringLabel;

    [Tooltip("Label untuk simbol theta di sudut lancip (World Space)")]
    public TextMeshPro thetaLabel;

    [Tooltip("Label untuk simbol sudut siku-siku ∟ (World Space)")]
    public TextMeshPro rightAngleLabel;

    [Header("Camera Reference")]
    [Tooltip("Camera untuk konversi world to screen point")]
    public Camera mainCamera;

    [Header("Visual Settings")]
    [Tooltip("Skala dasar untuk sprites (1 = 1 unit Unity per nilai segitiga)")]
    public float baseScale = 0.3f; // Reduced from 0.5f for portrait mode

    [Tooltip("Maksimal ukuran segitiga untuk auto-scaling (fit di layar)")]
    public float maxTriangleSize = 5f; // Reduced from 8f for portrait mode

    [Tooltip("Safety margin dari batas layar (dalam units)")]
    public float safetyMargin = 0.5f; // Reduced from 1f for tighter fit

    [Tooltip("Gunakan auto-scaling agar semua segitiga fit di layar")]
    public bool useAutoScaling = true;

    [Tooltip("Posisi pusat segitiga di world space")]
    public Vector3 centerPosition = Vector3.zero;

    [Tooltip("Offset label dari garis (jarak)")]
    public float labelOffset = 0.5f;

    [Tooltip("Ukuran font simbol sudut siku-siku ∟")]
    public float rightAngleFontSize = 5f;

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
    private float currentRotation = 0f; // Rotasi segitiga saat ini

    /// <summary>
    /// Menggambar segitiga dengan nilai yang diberikan (tanpa rotasi - default 0°)
    /// </summary>
    public void DrawTriangle(int depan, int samping, int miring)
    {
        DrawTriangle(depan, samping, miring, 0f);
    }

    /// <summary>
    /// Menggambar segitiga dengan nilai dan rotasi yang diberikan
    /// Rotasi: 0° = standard (theta di kiri bawah), 90° = theta di kiri atas, 180° = theta di kanan atas, 270° = theta di kanan bawah
    /// </summary>
    public void DrawTriangle(int depan, int samping, int miring, float rotationAngle)
    {
        currentDepan = depan;
        currentSamping = samping;
        currentMiring = miring;
        currentRotation = rotationAngle;

        // Hitung scale dinamis agar segitiga fit di layar
        float dynamicScale = baseScale;

        if (useAutoScaling)
        {
            // Hitung ukuran segitiga yang akan di-render (width x height)
            float widthNeeded = samping * baseScale;   // Lebar segitiga
            float heightNeeded = depan * baseScale;     // Tinggi segitiga

            // Maksimal ukuran yang aman (dikurangi safety margin)
            float safeMaxSize = maxTriangleSize - safetyMargin;

            // Tentukan scale berdasarkan dimensi yang paling besar
            float scaleByWidth = widthNeeded > safeMaxSize ? safeMaxSize / samping : baseScale;
            float scaleByHeight = heightNeeded > safeMaxSize ? safeMaxSize / depan : baseScale;

            // Gunakan scale yang paling kecil (paling membatasi) agar fit di KEDUA dimensi
            dynamicScale = Mathf.Min(scaleByWidth, scaleByHeight);

            if (dynamicScale < baseScale)
            {
                Debug.Log($"[TriangleVisualizer] Auto-scaling: {baseScale:F2} → {dynamicScale:F2} (W:{samping}, H:{depan} → Final: {samping * dynamicScale:F1} x {depan * dynamicScale:F1} units, Max allowed: {safeMaxSize:F1})");
            }
        }

        // Hitung posisi-posisi vertex segitiga SEBELUM rotasi
        Vector3 basePosition = transform.position + centerPosition;

        // Segitiga siku-siku dengan sudut siku di origin (0,0) relatif
        // PENTING: Theta di topLeft (sudut A), sudut siku di bottomLeft (sudut B)
        // BC (horizontal) = DEPAN (opposite dari theta)
        // AB (vertical) = SAMPING (adjacent ke theta)
        // AC (diagonal) = MIRING (hypotenuse)
        Vector3 bottomLeft = Vector3.zero;
        Vector3 bottomRight = new Vector3(depan * dynamicScale, 0, 0);  // BC = horizontal = DEPAN
        Vector3 topLeft = new Vector3(0, samping * dynamicScale, 0);    // AB = vertical = SAMPING

        // ROTASI: Rotasi semua vertex di sekitar origin
        float rotRad = rotationAngle * Mathf.Deg2Rad;
        bottomLeft = RotatePoint(bottomLeft, rotRad);
        bottomRight = RotatePoint(bottomRight, rotRad);
        topLeft = RotatePoint(topLeft, rotRad);

        // Offset semua vertex ke basePosition (world position)
        bottomLeft += basePosition;
        bottomRight += basePosition;
        topLeft += basePosition;

        // SISI DEPAN (BC - Horizontal di rotasi 0° - OPPOSITE dari theta)
        PositionSprite(depanSprite, bottomLeft, bottomRight, depan);
        if (depanLabel != null)
        {
            depanLabel.text = depan.ToString();
            Vector3 midPoint = (bottomLeft + bottomRight) / 2f;
            // Offset label perpendicular ke garis (ke bawah dari garis)
            Vector3 direction = (bottomRight - bottomLeft).normalized;
            Vector3 perpendicular = new Vector3(-direction.y, direction.x, 0); // Rotate 90° counterclockwise
            // Posisi label di bawah garis depan
            depanLabel.transform.position = midPoint + perpendicular * (-labelOffset * 1.5f);
        }

        // SISI SAMPING (AB - Vertical di rotasi 0° - ADJACENT ke theta)
        PositionSprite(sampingSprite, bottomLeft, topLeft, samping);
        if (sampingLabel != null)
        {
            sampingLabel.text = samping.ToString();
            Vector3 midPoint = (bottomLeft + topLeft) / 2f;
            // Offset label perpendicular ke garis (ke kiri dari garis)
            Vector3 direction = (topLeft - bottomLeft).normalized;
            Vector3 perpendicular = new Vector3(-direction.y, direction.x, 0);
            // Posisi label di kiri garis samping
            sampingLabel.transform.position = midPoint + perpendicular * (-labelOffset * 1.5f);
        }

        // SISI MIRING (Diagonal - Hypotenuse)
        PositionSprite(miringSprite, topLeft, bottomRight, miring);
        if (miringLabel != null)
        {
            miringLabel.text = miring.ToString();
            Vector3 midPoint = (topLeft + bottomRight) / 2f;

            // Hitung perpendicular offset untuk label miring (ke kanan atas dari garis)
            Vector3 direction = (bottomRight - topLeft).normalized;
            Vector3 perpendicular = new Vector3(-direction.y, direction.x, 0);
            // Posisi label di luar segitiga (kanan atas dari garis miring)
            miringLabel.transform.position = midPoint + perpendicular * (labelOffset * 1.5f);
        }

        // SIMBOL THETA (di sudut lancip atas A - antara samping AB dan miring AC) - WORLD SPACE
        if (thetaLabel != null)
        {
            thetaLabel.text = "θ";
            // Posisi theta di topLeft (titik A - sudut antara sisi samping AB dan sisi miring AC)
            float thetaOffsetDistance = 0.8f;

            // Hitung arah menuju "dalam" segitiga dari sudut theta
            // Arah dari topLeft ke bottomLeft (sepanjang sisi samping AB)
            Vector3 toSamping = (bottomLeft - topLeft).normalized;
            // Arah dari topLeft ke bottomRight (sepanjang sisi miring AC)
            Vector3 toMiring = (bottomRight - topLeft).normalized;
            // Bisector (tengah-tengah antara dua arah)
            Vector3 inward = (toSamping + toMiring).normalized;

            Vector3 thetaPosition = topLeft + inward * thetaOffsetDistance;
            thetaLabel.transform.position = thetaPosition;

            // Set Z-position agar theta di depan garis segitiga
            thetaLabel.transform.position = new Vector3(
                thetaLabel.transform.position.x,
                thetaLabel.transform.position.y,
                -2f  // Z negatif = lebih dekat ke kamera = di depan
            );

            // Atur sorting order untuk TextMeshPro renderer
            if (thetaLabel.GetComponent<MeshRenderer>() != null)
            {
                thetaLabel.GetComponent<MeshRenderer>().sortingOrder = 10; // Lebih tinggi = di depan
            }
        }

        // SIMBOL SUDUT SIKU-SIKU ∟ (di sudut B - bottomLeft) - 90 derajat
        if (rightAngleLabel != null)
        {
            rightAngleLabel.text = "∟"; // Unicode U+221F - Right Angle symbol

            // Posisi di sudut siku-siku (bottomLeft = titik B)
            // Hitung arah untuk offset simbol ke dalam segitiga
            Vector3 toRight = (bottomRight - bottomLeft).normalized; // Arah ke kanan (depan BC)
            Vector3 toUp = (topLeft - bottomLeft).normalized;        // Arah ke atas (samping AB)

            // Offset dari vertex agar simbol tidak pas di pojok
            float offsetDistance = 0.5f;
            Vector3 offset = (toRight + toUp).normalized * offsetDistance;
            Vector3 symbolPosition = bottomLeft + offset;

            // Set posisi dengan Z di depan garis
            rightAngleLabel.transform.position = new Vector3(
                symbolPosition.x,
                symbolPosition.y,
                -1.5f  // Di antara garis (0) dan theta (-2)
            );

            // Rotasi simbol ∟ mengikuti sudut segitiga
            // Simbol ∟ default menghadap kanan-atas, rotasi sesuai orientasi sisi
            float angleToRight = Mathf.Atan2(toRight.y, toRight.x) * Mathf.Rad2Deg;
            rightAngleLabel.transform.rotation = Quaternion.Euler(0, 0, angleToRight);

            // Set font size
            rightAngleLabel.fontSize = rightAngleFontSize;

            // Set sorting order agar terlihat di depan garis
            if (rightAngleLabel.GetComponent<MeshRenderer>() != null)
            {
                rightAngleLabel.GetComponent<MeshRenderer>().sortingOrder = 8; // Di bawah theta (10) tapi di atas garis (0)
            }
        }

        // Reset warna ke normal
        ResetColors();
    }

    /// <summary>
    /// Rotasi point di sekitar origin (0,0) sebesar angle (dalam radian)
    /// </summary>
    private Vector3 RotatePoint(Vector3 point, float angleRad)
    {
        float cos = Mathf.Cos(angleRad);
        float sin = Mathf.Sin(angleRad);

        float newX = point.x * cos - point.y * sin;
        float newY = point.x * sin + point.y * cos;

        return new Vector3(newX, newY, point.z);
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
        // Auto-adjust scale untuk portrait mode
        bool isPortrait = Screen.height > Screen.width;

        if (isPortrait)
        {
            // Portrait mode: Kurangi scale agar fit
            float portraitScaleFactor = 0.5f; // 50% dari landscape
            baseScale *= portraitScaleFactor;
            maxTriangleSize *= 0.6f;
            safetyMargin *= 0.5f;

            Debug.Log($"[TriangleVisualizer] Portrait mode detected - Scale adjusted: baseScale={baseScale:F2}, maxSize={maxTriangleSize:F1}");
        }

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
