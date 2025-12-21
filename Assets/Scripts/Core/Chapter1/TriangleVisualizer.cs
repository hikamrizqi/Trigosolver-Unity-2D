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
    [Tooltip("Skala dasar untuk sprites - UBAH INI untuk memperbesar/memperkecil SEMUA segitiga (0.4=kecil, 0.6=besar)")]
    public float baseScale = 0.5f;

    [Tooltip("Maksimal ukuran SETELAH baseScale (units) - Jika sisi terpanjang × baseScale > ini, segitiga dikecilkan. Set 10+ untuk hampir tidak pernah rescale")]
    public float maxTriangleSize = 9f; // Tingkatkan ke 9 agar jarang rescale

    [Tooltip("Safety margin dari batas layar (dalam units)")]
    public float safetyMargin = 0.5f;

    [Tooltip("Gunakan auto-scaling - UNCHECK ini jika mau ukuran tetap baseScale tanpa rescale")]
    public bool useAutoScaling = true;

    [Header("Symbol Position Settings")]
    [Tooltip("Jarak simbol theta dari sudut (lebih kecil = lebih dekat ke garis)")]
    public float thetaOffsetMultiplier = 1.5f; // Ubah ke 1.0 atau 0.8 untuk lebih dekat

    [Tooltip("Jarak simbol siku dari sudut (lebih kecil = lebih dekat ke garis)")]
    public float rightAngleOffsetMultiplier = 1.2f; // Ubah ke 0.8 atau 0.6 untuk lebih dekat

    [Tooltip("Posisi pusat segitiga di world space")]
    public Vector3 centerPosition = Vector3.zero;

    [Tooltip("Offset label dari garis (jarak)")]
    public float labelOffset = 0.5f;

    [Header("Manual Position Adjustment")]
    [Tooltip("Multiplier offset untuk label DEPAN (default 2.0)")]
    public float depanLabelMultiplier = 2f;

    [Tooltip("Multiplier offset untuk label SAMPING (default 2.0)")]
    public float sampingLabelMultiplier = 2f;

    [Tooltip("Multiplier offset untuk label MIRING (default 2.0)")]
    public float miringLabelMultiplier = 2f;

    [Tooltip("Z-offset untuk label agar muncul di depan sprite (lebih negatif = lebih depan)")]
    public float labelZOffset = -2f;

    [Tooltip("Sorting order untuk label (lebih tinggi = lebih depan)")]
    public int labelSortingOrder = 100;

    [Tooltip("Ukuran font untuk label angka sisi")]
    public float labelFontSize = 10f;

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
            depanLabel.fontSize = labelFontSize;
            Vector3 midPoint = (bottomLeft + bottomRight) / 2f;
            Vector3 direction = (bottomRight - bottomLeft).normalized;
            // Perpendicular ke BAWAH (untuk label di bawah garis)
            Vector3 perpendicular = new Vector3(direction.y, -direction.x, 0);
            // ADJUST: depanLabelMultiplier default 2.0 - ubah di Inspector untuk geser posisi
            Vector3 labelPos = midPoint + perpendicular * (labelOffset * depanLabelMultiplier);
            labelPos.z = labelZOffset;
            depanLabel.transform.position = labelPos;
            if (depanLabel.GetComponent<MeshRenderer>() != null)
                depanLabel.GetComponent<MeshRenderer>().sortingOrder = labelSortingOrder;
        }

        // SISI SAMPING (AB - Vertical di rotasi 0° - ADJACENT ke theta)
        PositionSprite(sampingSprite, bottomLeft, topLeft, samping);
        if (sampingLabel != null)
        {
            sampingLabel.text = samping.ToString();
            sampingLabel.fontSize = labelFontSize;
            Vector3 midPoint = (bottomLeft + topLeft) / 2f;
            Vector3 direction = (topLeft - bottomLeft).normalized;
            // Perpendicular ke KIRI (untuk label di kiri garis)
            Vector3 perpendicular = new Vector3(direction.y, -direction.x, 0);
            // ADJUST: sampingLabelMultiplier default 2.0 - ubah di Inspector untuk geser posisi
            Vector3 labelPos = midPoint + perpendicular * (labelOffset * sampingLabelMultiplier);
            labelPos.z = labelZOffset;
            sampingLabel.transform.position = labelPos;
            if (sampingLabel.GetComponent<MeshRenderer>() != null)
                sampingLabel.GetComponent<MeshRenderer>().sortingOrder = labelSortingOrder;
        }

        // SISI MIRING (Diagonal - Hypotenuse)
        PositionSprite(miringSprite, topLeft, bottomRight, miring);
        if (miringLabel != null)
        {
            miringLabel.text = miring.ToString();
            miringLabel.fontSize = labelFontSize;
            Vector3 midPoint = (topLeft + bottomRight) / 2f;
            Vector3 direction = (bottomRight - topLeft).normalized;
            // Perpendicular ke KANAN (untuk label di luar segitiga)
            Vector3 perpendicular = new Vector3(-direction.y, direction.x, 0);
            // ADJUST: miringLabelMultiplier default 2.0 - ubah di Inspector untuk geser posisi
            Vector3 labelPos = midPoint + perpendicular * (labelOffset * miringLabelMultiplier);
            labelPos.z = labelZOffset;
            miringLabel.transform.position = labelPos;
            if (miringLabel.GetComponent<MeshRenderer>() != null)
                miringLabel.GetComponent<MeshRenderer>().sortingOrder = labelSortingOrder;
        }

        // SIMBOL THETA (di sudut lancip atas A - antara samping AB dan miring AC) - WORLD SPACE
        if (thetaLabel != null)
        {
            thetaLabel.text = "θ";
            thetaLabel.fontSize = labelFontSize * 0.8f; // Sedikit lebih kecil dari label angka

            // ADJUST: thetaOffsetMultiplier di Inspector (default 1.5) - ubah ke 1.0 atau 0.8 untuk lebih dekat
            float thetaOffsetDistance = labelOffset * thetaOffsetMultiplier;

            // Hitung arah menuju "dalam" segitiga dari sudut theta
            Vector3 toSamping = (bottomLeft - topLeft).normalized;  // Arah ke bawah (samping AB)
            Vector3 toMiring = (bottomRight - topLeft).normalized;   // Arah ke kanan bawah (miring AC)
            // Bisector (tengah-tengah antara dua arah) - menuju dalam segitiga
            Vector3 inward = (toSamping + toMiring).normalized;

            Vector3 thetaPosition = topLeft + inward * thetaOffsetDistance;
            thetaPosition.z = labelZOffset; // Z di depan sprite
            thetaLabel.transform.position = thetaPosition;

            // Set sorting order agar tidak tertutup objek lain
            if (thetaLabel.GetComponent<MeshRenderer>() != null)
            {
                thetaLabel.GetComponent<MeshRenderer>().sortingOrder = labelSortingOrder;
            }
        }

        // SIMBOL SUDUT SIKU-SIKU ∟ (di sudut B - bottomLeft) - 90 derajat
        if (rightAngleLabel != null)
        {
            rightAngleLabel.text = "∟"; // Unicode U+221F - Right Angle symbol
            rightAngleLabel.fontSize = labelFontSize * 1.2f; // Lebih besar untuk visibility

            // Posisi di sudut siku-siku (bottomLeft = titik B)
            Vector3 toRight = (bottomRight - bottomLeft).normalized; // Arah ke kanan (depan BC)
            Vector3 toUp = (topLeft - bottomLeft).normalized;        // Arah ke atas (samping AB)

            // ADJUST: rightAngleOffsetMultiplier di Inspector (default 1.2) - ubah ke 0.8 atau 0.6 untuk lebih dekat
            float offsetDistance = labelOffset * rightAngleOffsetMultiplier;
            Vector3 offset = (toRight + toUp).normalized * offsetDistance;
            Vector3 symbolPosition = bottomLeft + offset;
            symbolPosition.z = labelZOffset; // Z di depan sprite

            rightAngleLabel.transform.position = symbolPosition;

            // Set sorting order agar tidak tertutup objek lain
            if (rightAngleLabel.GetComponent<MeshRenderer>() != null)
            {
                rightAngleLabel.GetComponent<MeshRenderer>().sortingOrder = labelSortingOrder;
            }

            // Rotasi simbol ∟ mengikuti sudut segitiga
            // Simbol ∟ default menghadap kanan-atas, rotasi sesuai orientasi sisi
            float angleToRight = Mathf.Atan2(toRight.y, toRight.x) * Mathf.Rad2Deg;
            rightAngleLabel.transform.rotation = Quaternion.Euler(0, 0, angleToRight);

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
