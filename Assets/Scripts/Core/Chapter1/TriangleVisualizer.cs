using UnityEngine;
using TMPro;
using DG.Tweening;

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

    [Tooltip("Label untuk simbol theta di sudut lancip (World Space) - SOAL 1-10")]
    public TextMeshPro thetaLabel;

    [Tooltip("Label untuk simbol sudut siku-siku ∟ (World Space)")]
    public TextMeshPro rightAngleLabel;

    [Header("Angle Labels (Soal 11-20)")]
    [Tooltip("Label untuk sudut A (World Space) - SOAL 11-20")]
    public TextMeshPro angleLabelA;

    [Tooltip("Label untuk sudut B (World Space) - SOAL 11-20")]
    public TextMeshPro angleLabelB;

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

    [Header("Animation Settings")]
    [Tooltip("Durasi animasi garis masuk/keluar (detik)")]
    public float animationDuration = 0.5f;

    [Tooltip("Ease type untuk animasi masuk")]
    public Ease entryEase = Ease.OutBack;

    [Tooltip("Ease type untuk animasi keluar")]
    public Ease exitEase = Ease.InBack;

    [Tooltip("Jarak offscreen untuk spawn position (units)")]
    public float offscreenDistance = 15f;

    // Data segitiga saat ini
    private int currentDepan;
    private int currentSamping;
    private int currentMiring;
    private float currentRotation = 0f; // Rotasi segitiga saat ini
    private TriangleOrientation currentOrientation = TriangleOrientation.Normal; // Orientasi saat ini
    private bool currentIsDualQuestion = false; // True jika soal 11-20 (pakai A dan B)

    /// <summary>
    /// Menggambar segitiga dengan nilai yang diberikan (tanpa rotasi - default 0°)
    /// </summary>
    public void DrawTriangle(int depan, int samping, int miring)
    {
        DrawTriangle(depan, samping, miring, 0f, TriangleOrientation.Normal, false);
    }

    /// <summary>
    /// Menggambar segitiga dengan nilai, rotasi, dan orientasi yang diberikan
    /// Rotasi: 0° = standard (theta di kiri bawah), 90° = theta di kiri atas, 180° = theta di kanan atas, 270° = theta di kanan bawah
    /// Orientation: Normal (depan=alas, samping=tegak) atau Swapped (samping=alas, depan=tegak)
    /// </summary>
    public void DrawTriangle(int depan, int samping, int miring, float rotationAngle, TriangleOrientation orientation = TriangleOrientation.Normal)
    {
        DrawTriangle(depan, samping, miring, rotationAngle, orientation, false);
    }

    /// <summary>
    /// Menggambar segitiga dengan nilai, rotasi, orientasi, dan tipe soal yang diberikan
    /// isDualQuestion: true untuk soal 11-20 (pakai A dan B), false untuk soal 1-10 (pakai theta)
    /// </summary>
    public void DrawTriangle(int depan, int samping, int miring, float rotationAngle, TriangleOrientation orientation, bool isDualQuestion)
    {
        currentDepan = depan;
        currentSamping = samping;
        currentMiring = miring;
        currentRotation = rotationAngle;
        currentOrientation = orientation; // Simpan orientasi saat ini
        currentIsDualQuestion = isDualQuestion; // Simpan tipe soal

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
        // NORMAL: Depan=horizontal(alas), Samping=vertical(tegak), Theta di topLeft
        // SWAPPED: Depan=vertical(tegak), Samping=horizontal(alas), Theta di bottomRight
        Vector3 bottomLeft = Vector3.zero;
        Vector3 bottomRight, topLeft;

        if (orientation == TriangleOrientation.Normal)
        {
            // NORMAL: Depan di horizontal, Samping di vertical
            bottomRight = new Vector3(depan * dynamicScale, 0, 0);      // BC = depan (alas)
            topLeft = new Vector3(0, samping * dynamicScale, 0);        // AB = samping (tegak)
            Debug.Log($"[Orientation] NORMAL: Depan={depan} (horizontal/alas), Samping={samping} (vertical/tegak), Theta di topLeft");
        }
        else // Swapped
        {
            // SWAPPED: Samping di horizontal, Depan di vertical
            bottomRight = new Vector3(samping * dynamicScale, 0, 0);    // BC = samping (alas)
            topLeft = new Vector3(0, depan * dynamicScale, 0);          // AB = depan (tegak)
            Debug.Log($"[Orientation] SWAPPED: Samping={samping} (horizontal/alas), Depan={depan} (vertical/tegak), Theta di bottomRight");
        }

        // ROTASI: Rotasi semua vertex di sekitar origin
        float rotRad = rotationAngle * Mathf.Deg2Rad;
        bottomLeft = RotatePoint(bottomLeft, rotRad);
        bottomRight = RotatePoint(bottomRight, rotRad);
        topLeft = RotatePoint(topLeft, rotRad);

        // Offset semua vertex ke basePosition (world position)
        bottomLeft += basePosition;
        bottomRight += basePosition;
        topLeft += basePosition;

        // SISI HORIZONTAL (BC - Alas)
        // Normal: Depan di horizontal, Swapped: Samping di horizontal
        int horizontalValue = (orientation == TriangleOrientation.Normal) ? depan : samping;
        SpriteRenderer horizontalSprite = (orientation == TriangleOrientation.Normal) ? depanSprite : sampingSprite;
        TextMeshPro horizontalLabel = (orientation == TriangleOrientation.Normal) ? depanLabel : sampingLabel;

        PositionSprite(horizontalSprite, bottomLeft, bottomRight, horizontalValue);
        if (horizontalLabel != null)
        {
            horizontalLabel.text = horizontalValue.ToString();
            horizontalLabel.fontSize = labelFontSize;
            Vector3 midPoint = (bottomLeft + bottomRight) / 2f;
            Vector3 direction = (bottomRight - bottomLeft).normalized;
            Vector3 perpendicular = new Vector3(direction.y, -direction.x, 0);

            // AUTO: Set multiplier berdasarkan posisi
            // Alas (horizontal) = 1, Tegak (vertical) = -1
            float multiplier;
            if (orientation == TriangleOrientation.Normal)
            {
                // Normal: Depan di alas → multiplier = 1
                multiplier = 1f;
            }
            else
            {
                // Swapped: Samping di alas → multiplier = 1
                multiplier = 1f;
            }

            Vector3 labelPos = midPoint + perpendicular * (labelOffset * multiplier);
            labelPos.z = labelZOffset;
            horizontalLabel.transform.position = labelPos;
            if (horizontalLabel.GetComponent<MeshRenderer>() != null)
                horizontalLabel.GetComponent<MeshRenderer>().sortingOrder = labelSortingOrder;
        }

        // SISI VERTICAL (AB - Tegak)
        // Normal: Samping di vertical, Swapped: Depan di vertical
        int verticalValue = (orientation == TriangleOrientation.Normal) ? samping : depan;
        SpriteRenderer verticalSprite = (orientation == TriangleOrientation.Normal) ? sampingSprite : depanSprite;
        TextMeshPro verticalLabel = (orientation == TriangleOrientation.Normal) ? sampingLabel : depanLabel;

        PositionSprite(verticalSprite, bottomLeft, topLeft, verticalValue);
        if (verticalLabel != null)
        {
            verticalLabel.text = verticalValue.ToString();
            verticalLabel.fontSize = labelFontSize;
            Vector3 midPoint = (bottomLeft + topLeft) / 2f;
            Vector3 direction = (topLeft - bottomLeft).normalized;
            Vector3 perpendicular = new Vector3(direction.y, -direction.x, 0);

            // AUTO: Set multiplier berdasarkan posisi
            // Alas (horizontal) = 1, Tegak (vertical) = -1
            float multiplier;
            if (orientation == TriangleOrientation.Normal)
            {
                // Normal: Samping di tegak → multiplier = -1
                multiplier = -1f;
            }
            else
            {
                // Swapped: Depan di tegak → multiplier = -1
                multiplier = -1f;
            }

            Vector3 labelPos = midPoint + perpendicular * (labelOffset * multiplier);
            labelPos.z = labelZOffset;
            verticalLabel.transform.position = labelPos;
            if (verticalLabel.GetComponent<MeshRenderer>() != null)
                verticalLabel.GetComponent<MeshRenderer>().sortingOrder = labelSortingOrder;
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

        // SIMBOL THETA (di sudut lancip - antara samping dan miring) - WORLD SPACE
        // NORMAL: Theta di topLeft (antara AB dan AC)
        // SWAPPED: Theta di bottomRight (antara BC dan AC)
        // HANYA UNTUK SOAL 1-10 (bukan dual question)
        if (!currentIsDualQuestion && thetaLabel != null)
        {
            thetaLabel.gameObject.SetActive(true);
            thetaLabel.text = "θ";
            thetaLabel.fontSize = labelFontSize * 0.8f; // Sedikit lebih kecil dari label angka

            // ADJUST: thetaOffsetMultiplier di Inspector (default 1.5) - ubah ke 1.0 atau 0.8 untuk lebih dekat
            float thetaOffsetDistance = labelOffset * thetaOffsetMultiplier;

            Vector3 thetaPosition;
            if (orientation == TriangleOrientation.Normal)
            {
                // Theta di topLeft (sudut A) - antara AB dan AC
                Vector3 toSamping = (bottomLeft - topLeft).normalized;  // Arah ke bawah (AB)
                Vector3 toMiring = (bottomRight - topLeft).normalized;  // Arah ke kanan bawah (AC)
                Vector3 inward = (toSamping + toMiring).normalized;     // Bisector menuju dalam
                thetaPosition = topLeft + inward * thetaOffsetDistance;
            }
            else // Swapped
            {
                // Theta di bottomRight (sudut C) - antara BC dan CA
                Vector3 toSamping = (bottomLeft - bottomRight).normalized;  // Arah ke kiri (BC)
                Vector3 toMiring = (topLeft - bottomRight).normalized;      // Arah ke kiri atas (CA)
                Vector3 inward = (toSamping + toMiring).normalized;         // Bisector menuju dalam
                thetaPosition = bottomRight + inward * thetaOffsetDistance;
            }

            thetaPosition.z = labelZOffset; // Z di depan sprite
            thetaLabel.transform.position = thetaPosition;

            // Set sorting order agar tidak tertutup objek lain
            if (thetaLabel.GetComponent<MeshRenderer>() != null)
            {
                thetaLabel.GetComponent<MeshRenderer>().sortingOrder = labelSortingOrder;
            }
        }
        else if (!currentIsDualQuestion && thetaLabel != null)
        {
            thetaLabel.gameObject.SetActive(false);
        }

        // SIMBOL SUDUT A DAN B (untuk soal 11-20 - dual question)
        // Sudut A di topLeft, Sudut B di bottomRight (untuk orientasi normal)
        // Sudut A di bottomRight, Sudut B di topLeft (untuk orientasi swapped)
        if (currentIsDualQuestion)
        {
            // Hide theta jika ada
            if (thetaLabel != null)
                thetaLabel.gameObject.SetActive(false);

            float angleOffsetDistance = labelOffset * thetaOffsetMultiplier;

            // SUDUT A
            if (angleLabelA != null)
            {
                angleLabelA.gameObject.SetActive(true);
                angleLabelA.text = "A";
                angleLabelA.fontSize = labelFontSize * 0.8f;

                Vector3 angleAPosition;
                if (orientation == TriangleOrientation.Normal)
                {
                    // A di topLeft
                    Vector3 toSamping = (bottomLeft - topLeft).normalized;
                    Vector3 toMiring = (bottomRight - topLeft).normalized;
                    Vector3 inward = (toSamping + toMiring).normalized;
                    angleAPosition = topLeft + inward * angleOffsetDistance;
                }
                else // Swapped
                {
                    // A di bottomRight
                    Vector3 toSamping = (bottomLeft - bottomRight).normalized;
                    Vector3 toMiring = (topLeft - bottomRight).normalized;
                    Vector3 inward = (toSamping + toMiring).normalized;
                    angleAPosition = bottomRight + inward * angleOffsetDistance;
                }

                angleAPosition.z = labelZOffset;
                angleLabelA.transform.position = angleAPosition;
                if (angleLabelA.GetComponent<MeshRenderer>() != null)
                    angleLabelA.GetComponent<MeshRenderer>().sortingOrder = labelSortingOrder;
            }

            // SUDUT B
            if (angleLabelB != null)
            {
                angleLabelB.gameObject.SetActive(true);
                angleLabelB.text = "B";
                angleLabelB.fontSize = labelFontSize * 0.8f;

                Vector3 angleBPosition;
                if (orientation == TriangleOrientation.Normal)
                {
                    // B di bottomRight
                    Vector3 toDepan = (bottomLeft - bottomRight).normalized;
                    Vector3 toMiring = (topLeft - bottomRight).normalized;
                    Vector3 inward = (toDepan + toMiring).normalized;
                    angleBPosition = bottomRight + inward * angleOffsetDistance;
                }
                else // Swapped
                {
                    // B di topLeft
                    Vector3 toDepan = (bottomRight - topLeft).normalized;
                    Vector3 toMiring = (bottomLeft - topLeft).normalized;
                    Vector3 inward = (toDepan + toMiring).normalized;
                    angleBPosition = topLeft + inward * angleOffsetDistance;
                }

                angleBPosition.z = labelZOffset;
                angleLabelB.transform.position = angleBPosition;
                if (angleLabelB.GetComponent<MeshRenderer>() != null)
                    angleLabelB.GetComponent<MeshRenderer>().sortingOrder = labelSortingOrder;
            }
        }
        else // Hide A and B untuk soal 1-10
        {
            if (angleLabelA != null)
                angleLabelA.gameObject.SetActive(false);
            if (angleLabelB != null)
                angleLabelB.gameObject.SetActive(false);
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

        // Animate triangle in dari offscreen
        AnimateTriangleIn(bottomLeft, bottomRight, topLeft);
    }

    /// <summary>
    /// Animasi garis dan simbol masuk dari luar layar
    /// </summary>
    private void AnimateTriangleIn(Vector3 bottomLeft, Vector3 bottomRight, Vector3 topLeft)
    {
        // Kill existing tweens untuk avoid conflict
        DOTween.Kill(depanSprite?.transform);
        DOTween.Kill(sampingSprite?.transform);
        DOTween.Kill(miringSprite?.transform);
        DOTween.Kill(depanLabel?.transform);
        DOTween.Kill(sampingLabel?.transform);
        DOTween.Kill(miringLabel?.transform);
        DOTween.Kill(thetaLabel?.transform);
        DOTween.Kill(rightAngleLabel?.transform);
        DOTween.Kill(angleLabelA?.transform);
        DOTween.Kill(angleLabelB?.transform);

        // HORIZONTAL LINE (bottom): Masuk dari KIRI
        if (sampingSprite != null || depanSprite != null)
        {
            SpriteRenderer horizontalSprite = (currentOrientation == TriangleOrientation.Normal) ? depanSprite : sampingSprite;
            TextMeshPro horizontalLabel = (currentOrientation == TriangleOrientation.Normal) ? depanLabel : sampingLabel;

            if (horizontalSprite != null)
            {
                Vector3 startPos = horizontalSprite.transform.position;
                Vector3 offscreenPos = startPos + Vector3.left * offscreenDistance;
                horizontalSprite.transform.position = offscreenPos;
                horizontalSprite.transform.DOMove(startPos, animationDuration).SetEase(entryEase);
            }

            if (horizontalLabel != null)
            {
                Vector3 startPos = horizontalLabel.transform.position;
                Vector3 offscreenPos = startPos + Vector3.left * offscreenDistance;
                horizontalLabel.transform.position = offscreenPos;
                horizontalLabel.transform.DOMove(startPos, animationDuration).SetEase(entryEase);
            }
        }

        // VERTICAL LINE (left): Masuk dari ATAS
        if (sampingSprite != null || depanSprite != null)
        {
            SpriteRenderer verticalSprite = (currentOrientation == TriangleOrientation.Normal) ? sampingSprite : depanSprite;
            TextMeshPro verticalLabel = (currentOrientation == TriangleOrientation.Normal) ? sampingLabel : depanLabel;

            if (verticalSprite != null)
            {
                Vector3 startPos = verticalSprite.transform.position;
                Vector3 offscreenPos = startPos + Vector3.up * offscreenDistance;
                verticalSprite.transform.position = offscreenPos;
                verticalSprite.transform.DOMove(startPos, animationDuration).SetEase(entryEase);
            }

            if (verticalLabel != null)
            {
                Vector3 startPos = verticalLabel.transform.position;
                Vector3 offscreenPos = startPos + Vector3.up * offscreenDistance;
                verticalLabel.transform.position = offscreenPos;
                verticalLabel.transform.DOMove(startPos, animationDuration).SetEase(entryEase);
            }
        }

        // DIAGONAL LINE (miring): Masuk dari KANAN ATAS
        if (miringSprite != null)
        {
            Vector3 startPos = miringSprite.transform.position;
            Vector3 offscreenPos = startPos + (Vector3.right + Vector3.up).normalized * offscreenDistance;
            miringSprite.transform.position = offscreenPos;
            miringSprite.transform.DOMove(startPos, animationDuration).SetEase(entryEase);
        }

        if (miringLabel != null)
        {
            Vector3 startPos = miringLabel.transform.position;
            Vector3 offscreenPos = startPos + (Vector3.right + Vector3.up).normalized * offscreenDistance;
            miringLabel.transform.position = offscreenPos;
            miringLabel.transform.DOMove(startPos, animationDuration).SetEase(entryEase);
        }

        // THETA SYMBOL: Fade in dan scale (untuk soal 1-10)
        if (!currentIsDualQuestion && thetaLabel != null && thetaLabel.gameObject.activeSelf)
        {
            thetaLabel.alpha = 0;
            thetaLabel.transform.localScale = Vector3.zero;
            thetaLabel.DOFade(1f, animationDuration).SetEase(entryEase);
            thetaLabel.transform.DOScale(Vector3.one, animationDuration).SetEase(entryEase);
        }

        // ANGLE A SYMBOL: Fade in dan scale (untuk soal 11-20)
        if (currentIsDualQuestion && angleLabelA != null && angleLabelA.gameObject.activeSelf)
        {
            angleLabelA.alpha = 0;
            angleLabelA.transform.localScale = Vector3.zero;
            angleLabelA.DOFade(1f, animationDuration).SetEase(entryEase);
            angleLabelA.transform.DOScale(Vector3.one, animationDuration).SetEase(entryEase);
        }

        // ANGLE B SYMBOL: Fade in dan scale (untuk soal 11-20)
        if (currentIsDualQuestion && angleLabelB != null && angleLabelB.gameObject.activeSelf)
        {
            angleLabelB.alpha = 0;
            angleLabelB.transform.localScale = Vector3.zero;
            angleLabelB.DOFade(1f, animationDuration).SetEase(entryEase);
            angleLabelB.transform.DOScale(Vector3.one, animationDuration).SetEase(entryEase);
        }

        // RIGHT ANGLE SYMBOL: Fade in dan scale (untuk semua soal)
        if (rightAngleLabel != null)
        {
            rightAngleLabel.alpha = 0;
            rightAngleLabel.transform.localScale = Vector3.zero;
            rightAngleLabel.DOFade(1f, animationDuration).SetEase(entryEase);
            rightAngleLabel.transform.DOScale(Vector3.one, animationDuration).SetEase(entryEase);
        }
    }

    /// <summary>
    /// Animasi garis dan simbol keluar ke luar layar
    /// </summary>
    public void AnimateTriangleOut(System.Action onComplete = null)
    {
        Sequence exitSequence = DOTween.Sequence();

        // HORIZONTAL LINE: Keluar ke KANAN
        if (sampingSprite != null || depanSprite != null)
        {
            SpriteRenderer horizontalSprite = (currentOrientation == TriangleOrientation.Normal) ? depanSprite : sampingSprite;
            TextMeshPro horizontalLabel = (currentOrientation == TriangleOrientation.Normal) ? depanLabel : sampingLabel;

            if (horizontalSprite != null)
            {
                Vector3 currentPos = horizontalSprite.transform.position;
                Vector3 exitPos = currentPos + Vector3.right * offscreenDistance;
                exitSequence.Join(horizontalSprite.transform.DOMove(exitPos, animationDuration).SetEase(exitEase));
            }

            if (horizontalLabel != null)
            {
                Vector3 currentPos = horizontalLabel.transform.position;
                Vector3 exitPos = currentPos + Vector3.right * offscreenDistance;
                exitSequence.Join(horizontalLabel.transform.DOMove(exitPos, animationDuration).SetEase(exitEase));
            }
        }

        // VERTICAL LINE: Keluar ke BAWAH
        if (sampingSprite != null || depanSprite != null)
        {
            SpriteRenderer verticalSprite = (currentOrientation == TriangleOrientation.Normal) ? sampingSprite : depanSprite;
            TextMeshPro verticalLabel = (currentOrientation == TriangleOrientation.Normal) ? sampingLabel : depanLabel;

            if (verticalSprite != null)
            {
                Vector3 currentPos = verticalSprite.transform.position;
                Vector3 exitPos = currentPos + Vector3.down * offscreenDistance;
                exitSequence.Join(verticalSprite.transform.DOMove(exitPos, animationDuration).SetEase(exitEase));
            }

            if (verticalLabel != null)
            {
                Vector3 currentPos = verticalLabel.transform.position;
                Vector3 exitPos = currentPos + Vector3.down * offscreenDistance;
                exitSequence.Join(verticalLabel.transform.DOMove(exitPos, animationDuration).SetEase(exitEase));
            }
        }

        // DIAGONAL LINE: Keluar ke KIRI BAWAH
        if (miringSprite != null)
        {
            Vector3 currentPos = miringSprite.transform.position;
            Vector3 exitPos = currentPos + (Vector3.left + Vector3.down).normalized * offscreenDistance;
            exitSequence.Join(miringSprite.transform.DOMove(exitPos, animationDuration).SetEase(exitEase));
        }

        if (miringLabel != null)
        {
            Vector3 currentPos = miringLabel.transform.position;
            Vector3 exitPos = currentPos + (Vector3.left + Vector3.down).normalized * offscreenDistance;
            exitSequence.Join(miringLabel.transform.DOMove(exitPos, animationDuration).SetEase(exitEase));
        }

        // THETA, ANGLE A, ANGLE B, & RIGHT ANGLE: Fade out dan scale down
        if (thetaLabel != null && thetaLabel.gameObject.activeSelf)
        {
            exitSequence.Join(thetaLabel.DOFade(0f, animationDuration).SetEase(exitEase));
            exitSequence.Join(thetaLabel.transform.DOScale(Vector3.zero, animationDuration).SetEase(exitEase));
        }

        if (angleLabelA != null && angleLabelA.gameObject.activeSelf)
        {
            exitSequence.Join(angleLabelA.DOFade(0f, animationDuration).SetEase(exitEase));
            exitSequence.Join(angleLabelA.transform.DOScale(Vector3.zero, animationDuration).SetEase(exitEase));
        }

        if (angleLabelB != null && angleLabelB.gameObject.activeSelf)
        {
            exitSequence.Join(angleLabelB.DOFade(0f, animationDuration).SetEase(exitEase));
            exitSequence.Join(angleLabelB.transform.DOScale(Vector3.zero, animationDuration).SetEase(exitEase));
        }

        if (rightAngleLabel != null)
        {
            exitSequence.Join(rightAngleLabel.DOFade(0f, animationDuration).SetEase(exitEase));
            exitSequence.Join(rightAngleLabel.transform.DOScale(Vector3.zero, animationDuration).SetEase(exitEase));
        }

        // Callback setelah animasi selesai
        if (onComplete != null)
        {
            exitSequence.OnComplete(() => onComplete());
        }

        exitSequence.Play();
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
    /// Sprite assignment sudah correct sesuai orientation, tidak perlu swap lagi
    /// </summary>
    public void HighlightSide(string sideName, Color color)
    {
        ResetColors();

        // Langsung highlight sprite yang sesuai nama
        // Sprite assignment di DrawTriangle sudah handle orientation swap
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
