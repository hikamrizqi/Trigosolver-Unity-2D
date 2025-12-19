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

    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private CanvasGroup canvasGroup;
    private bool isInitialized = false;

    private void Awake()
    {
        Initialize();
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

        // Simpan posisi original (posisi tengah target)
        originalPosition = rectTransform.anchoredPosition;

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
    /// Animasi drop dari atas dengan bounce effect
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
            // Reset posisi untuk animasi berikutnya
            rectTransform.anchoredPosition = originalPosition;
            canvasGroup.alpha = 1f;
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
        canvasGroup.alpha = 1f;
    }

    /// <summary>
    /// Hide langsung tanpa animasi
    /// </summary>
    public void HideInstant()
    {
        gameObject.SetActive(false);
        rectTransform.anchoredPosition = originalPosition;
        canvasGroup.alpha = 1f;
    }

    private void OnDestroy()
    {
        // Kill semua tween untuk mencegah error
        DOTween.Kill(rectTransform);
        DOTween.Kill(canvasGroup);
    }
}
