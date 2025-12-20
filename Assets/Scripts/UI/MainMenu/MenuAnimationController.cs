using UnityEngine;
using DG.Tweening;
using System;

/// <summary>
/// Controller untuk animasi menu: drop dari atas, bounce, dan sink ke bawah
/// </summary>
public class MenuAnimationController : MonoBehaviour
{
    [Header("Animation Settings")]
    [Tooltip("Durasi animasi drop dari atas (detik)")]
    public float dropDuration = 0.8f;

    [Tooltip("Tinggi awal drop dari atas layar (screen height multiplier)")]
    public float dropStartHeight = 1.5f;

    [Tooltip("Jumlah bounce saat landing")]
    public int bounceCount = 2;

    [Tooltip("Kekuatan bounce (0-1)")]
    public float bounceStrength = 0.3f;

    [Tooltip("Durasi animasi sink ke bawah (detik)")]
    public float sinkDuration = 0.6f;

    [Tooltip("Ease type untuk drop animation")]
    public Ease dropEase = Ease.OutBounce;

    [Tooltip("Ease type untuk sink animation")]
    public Ease sinkEase = Ease.InBack;

    [Header("Logo Corner Settings")]
    [Tooltip("Posisi logo di pojok kanan atas (anchor position)")]
    public Vector2 cornerPosition = new Vector2(300f, -100f);

    [Tooltip("Scale logo di pojok (0-1, default 0.3 = 30% dari size asli)")]
    public float cornerScale = 0.3f;

    [Tooltip("Durasi animasi shrink to corner (detik)")]
    public float shrinkDuration = 0.8f;

    [Header("Scene Fade In Settings")]
    [Tooltip("Durasi fade in background saat scene load (detik)")]
    public float sceneFadeInDuration = 1f;

    [Tooltip("Delay setelah fade in sebelum logo drop (detik)")]
    public float delayBeforeDrop = 0.5f;

    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private Vector3 originalScale;
    private CanvasGroup canvasGroup;
    private bool isInitialized = false;
    private bool isInCorner = false;

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        // Delayed drop dengan fade in background dulu
        AnimateDropInDelayed();
    }

    /// <summary>
    /// Inisialisasi komponen (dipanggil di Awake atau saat pertama kali digunakan)
    /// </summary>
    private void Initialize()
    {
        if (isInitialized) return;

        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogError($"[{gameObject.name}] MenuAnimationController memerlukan RectTransform!");
            return;
        }

        // Simpan posisi dan scale original
        originalPosition = rectTransform.anchoredPosition;
        originalScale = rectTransform.localScale;

        // Tambahkan CanvasGroup jika belum ada (untuk fade effect)
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        isInitialized = true;
        Debug.Log($"[{gameObject.name}] MenuAnimationController initialized");
    }

    /// <summary>
    /// Animasi drop dari atas dengan bounce effect (DELAYED VERSION - untuk scene start)
    /// Background fade in dulu, pause, baru logo drop
    /// </summary>
    public void AnimateDropInDelayed(Action onComplete = null)
    {
        // Pastikan sudah diinisialisasi
        Initialize();

        // Null check
        if (rectTransform == null || canvasGroup == null)
        {
            Debug.LogError($"[{gameObject.name}] AnimateDropInDelayed gagal: rectTransform atau canvasGroup null!");
            return;
        }

        Debug.Log($"[{gameObject.name}] AnimateDropInDelayed dimulai (fade background → pause {delayBeforeDrop}s → logo drop)");

        // Set posisi awal di atas layar
        float startY = Screen.height * dropStartHeight;
        rectTransform.anchoredPosition = new Vector2(originalPosition.x, startY);

        // Logo invisible dulu
        canvasGroup.alpha = 0f;
        gameObject.SetActive(true);

        // Sequence dengan delay:
        // 1. Background sudah fade in (handled by scene fade controller)
        // 2. Wait delay
        // 3. Baru logo drop
        Sequence dropSequence = DOTween.Sequence();

        // Delay sebelum mulai drop
        dropSequence.AppendInterval(delayBeforeDrop);

        // Fade in logo
        dropSequence.Append(canvasGroup.DOFade(1f, 0.3f));

        // Drop dengan bounce
        dropSequence.Join(rectTransform.DOAnchorPosY(originalPosition.y, dropDuration)
            .SetEase(dropEase));

        // Callback saat selesai
        dropSequence.OnComplete(() =>
        {
            Debug.Log($"[{gameObject.name}] AnimateDropInDelayed selesai");
            isInCorner = false;
            onComplete?.Invoke();
        });
    }

    /// <summary>
    /// Animasi drop dari atas dengan bounce effect (INSTANT VERSION - tanpa delay)
    /// </summary>
    public void AnimateDropIn(Action onComplete = null)
    {
        // Pastikan sudah diinisialisasi (untuk GameObject yang inactive sejak awal)
        Initialize();

        // Null check
        if (rectTransform == null || canvasGroup == null)
        {
            Debug.LogError($"[{gameObject.name}] AnimateDropIn gagal: rectTransform atau canvasGroup null!");
            return;
        }

        Debug.Log($"[{gameObject.name}] AnimateDropIn dimulai");

        // Set posisi awal di atas layar
        float startY = Screen.height * dropStartHeight;
        rectTransform.anchoredPosition = new Vector2(originalPosition.x, startY);

        // Set alpha awal
        canvasGroup.alpha = 0f;
        gameObject.SetActive(true);

        // Sequence: fade in + drop dengan bounce
        Sequence dropSequence = DOTween.Sequence();

        // Fade in
        dropSequence.Append(canvasGroup.DOFade(1f, 0.3f));

        // Drop dengan bounce
        dropSequence.Join(rectTransform.DOAnchorPosY(originalPosition.y, dropDuration)
            .SetEase(dropEase));

        // Callback saat selesai
        dropSequence.OnComplete(() =>
        {
            Debug.Log($"[{gameObject.name}] AnimateDropIn selesai");
            isInCorner = false;
            onComplete?.Invoke();
        });
    }

    /// <summary>
    /// Animasi shrink dan pindah ke pojok kanan atas (seperti watermark)
    /// Dipanggil saat logo di-klik
    /// </summary>
    public void AnimateShrinkToCorner(Action onComplete = null)
    {
        if (isInCorner)
        {
            Debug.Log($"[{gameObject.name}] Sudah di corner, skip animation");
            return;
        }

        Debug.Log($"[{gameObject.name}] AnimateShrinkToCorner dimulai");

        isInCorner = true;

        Sequence shrinkSequence = DOTween.Sequence();

        // Parallel: shrink scale + move ke corner + fade sedikit
        shrinkSequence.Append(rectTransform.DOScale(cornerScale, shrinkDuration)
            .SetEase(Ease.OutBack));

        shrinkSequence.Join(rectTransform.DOAnchorPos(cornerPosition, shrinkDuration)
            .SetEase(Ease.OutBack));

        shrinkSequence.Join(canvasGroup.DOFade(0.8f, shrinkDuration * 0.5f)); // Sedikit transparent

        // Callback
        shrinkSequence.OnComplete(() =>
        {
            Debug.Log($"[{gameObject.name}] AnimateShrinkToCorner selesai - logo sekarang di corner");
            onComplete?.Invoke();
        });
    }

    /// <summary>
    /// Restore logo dari corner ke posisi tengah (jika perlu)
    /// </summary>
    public void RestoreFromCorner(Action onComplete = null)
    {
        if (!isInCorner)
        {
            Debug.Log($"[{gameObject.name}] Tidak di corner, skip restore");
            return;
        }

        Debug.Log($"[{gameObject.name}] RestoreFromCorner dimulai");

        Sequence restoreSequence = DOTween.Sequence();

        // Kembalikan ke posisi dan scale original
        restoreSequence.Append(rectTransform.DOScale(originalScale, shrinkDuration)
            .SetEase(Ease.OutBack));

        restoreSequence.Join(rectTransform.DOAnchorPos(originalPosition, shrinkDuration)
            .SetEase(Ease.OutBack));

        restoreSequence.Join(canvasGroup.DOFade(1f, shrinkDuration * 0.5f));

        restoreSequence.OnComplete(() =>
        {
            Debug.Log($"[{gameObject.name}] RestoreFromCorner selesai");
            isInCorner = false;
            onComplete?.Invoke();
        });
    }

    /// <summary>
    /// Animasi sink ke bawah dan hilang
    /// </summary>
    public void AnimateSinkOut(Action onComplete = null)
    {
        float targetY = -Screen.height * dropStartHeight;

        Sequence sinkSequence = DOTween.Sequence();

        // Sink ke bawah
        sinkSequence.Append(rectTransform.DOAnchorPosY(targetY, sinkDuration)
            .SetEase(sinkEase));

        // Fade out
        sinkSequence.Join(canvasGroup.DOFade(0f, sinkDuration * 0.7f));

        // Callback dan hide
        sinkSequence.OnComplete(() =>
        {
            gameObject.SetActive(false);
            // Reset posisi dan scale untuk animasi berikutnya
            rectTransform.anchoredPosition = originalPosition;
            rectTransform.localScale = originalScale;
            canvasGroup.alpha = 1f;
            isInCorner = false;
            onComplete?.Invoke();
        });
    }

    /// <summary>
    /// Show langsung tanpa animasi (untuk testing)
    /// </summary>
    public void ShowInstant()
    {
        gameObject.SetActive(true);
        rectTransform.anchoredPosition = originalPosition;
        rectTransform.localScale = originalScale;
        canvasGroup.alpha = 1f;
        isInCorner = false;
    }

    /// <summary>
    /// Hide langsung tanpa animasi
    /// </summary>
    public void HideInstant()
    {
        gameObject.SetActive(false);
        rectTransform.anchoredPosition = originalPosition;
        rectTransform.localScale = originalScale;
        canvasGroup.alpha = 1f;
        isInCorner = false;
    }

    private void OnDestroy()
    {
        // Kill semua tween untuk mencegah error
        DOTween.Kill(rectTransform);
        DOTween.Kill(canvasGroup);
    }
}
