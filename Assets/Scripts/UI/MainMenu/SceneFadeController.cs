using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Collections;

/// <summary>
/// Controller untuk fade in/out scene dengan black screen
/// Digunakan untuk smooth transition dari opening video ke main menu
/// </summary>
public class SceneFadeController : MonoBehaviour
{
    [Header("Fade Panel")]
    [Tooltip("Image panel untuk fade effect (auto-create jika null)")]
    [SerializeField] private Image fadePanel;

    [Tooltip("Canvas untuk fade panel (auto-create jika null)")]
    [SerializeField] private Canvas fadeCanvas;

    [Header("Fade Settings")]
    [Tooltip("Durasi fade in dari hitam (detik)")]
    [SerializeField] private float fadeInDuration = 1f;

    [Tooltip("Durasi fade out ke hitam (detik)")]
    [SerializeField] private float fadeOutDuration = 0.8f;

    [Tooltip("Warna fade panel (default hitam)")]
    [SerializeField] private Color fadeColor = Color.black;

    [Header("Auto Fade On Start")]
    [Tooltip("Fade in otomatis saat scene load")]
    [SerializeField] private bool autoFadeInOnStart = true;

    private static SceneFadeController instance;

    /// <summary>
    /// Singleton instance untuk akses global
    /// </summary>
    public static SceneFadeController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SceneFadeController>();
                
                if (instance == null)
                {
                    Debug.LogWarning("[SceneFadeController] Instance not found, creating new one");
                    GameObject go = new GameObject("SceneFadeController");
                    instance = go.AddComponent<SceneFadeController>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Setup fade panel jika belum ada
        SetupFadePanel();
    }

    private void Start()
    {
        if (autoFadeInOnStart)
        {
            // Start dari black screen, fade in ke scene
            SetFadeAlpha(1f); // Black screen
            FadeIn();
        }
    }

    /// <summary>
    /// Setup fade panel dan canvas
    /// </summary>
    private void SetupFadePanel()
    {
        // Cek apakah fade panel sudah ada
        if (fadePanel != null)
        {
            Debug.Log("[SceneFadeController] Fade panel sudah ada");
            return;
        }

        // Auto-create canvas
        if (fadeCanvas == null)
        {
            GameObject canvasGO = new GameObject("FadeCanvas");
            canvasGO.transform.SetParent(transform);
            
            fadeCanvas = canvasGO.AddComponent<Canvas>();
            fadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            fadeCanvas.sortingOrder = 9999; // Paling depan
            
            CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            
            canvasGO.AddComponent<GraphicRaycaster>();
            
            Debug.Log("[SceneFadeController] Fade canvas auto-created");
        }

        // Auto-create fade panel
        GameObject panelGO = new GameObject("FadePanel");
        panelGO.transform.SetParent(fadeCanvas.transform, false);
        
        fadePanel = panelGO.AddComponent<Image>();
        fadePanel.color = fadeColor;
        
        // Fullscreen panel
        RectTransform panelRect = panelGO.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.sizeDelta = Vector2.zero;
        panelRect.anchoredPosition = Vector2.zero;
        
        Debug.Log("[SceneFadeController] Fade panel auto-created");
    }

    /// <summary>
    /// Fade in dari hitam (reveal scene)
    /// </summary>
    public void FadeIn(Action onComplete = null)
    {
        if (fadePanel == null)
        {
            Debug.LogError("[SceneFadeController] Fade panel null!");
            onComplete?.Invoke();
            return;
        }

        Debug.Log($"[SceneFadeController] Fade In (duration: {fadeInDuration}s)");

        // Fade dari alpha 1 (black) ke 0 (transparent)
        fadePanel.DOFade(0f, fadeInDuration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                Debug.Log("[SceneFadeController] Fade In complete");
                onComplete?.Invoke();
            });
    }

    /// <summary>
    /// Fade out ke hitam (hide scene)
    /// </summary>
    public void FadeOut(Action onComplete = null)
    {
        if (fadePanel == null)
        {
            Debug.LogError("[SceneFadeController] Fade panel null!");
            onComplete?.Invoke();
            return;
        }

        Debug.Log($"[SceneFadeController] Fade Out (duration: {fadeOutDuration}s)");

        // Fade dari alpha 0 (transparent) ke 1 (black)
        fadePanel.DOFade(1f, fadeOutDuration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                Debug.Log("[SceneFadeController] Fade Out complete");
                onComplete?.Invoke();
            });
    }

    /// <summary>
    /// Set fade alpha langsung tanpa animasi
    /// </summary>
    /// <param name="alpha">0 = transparent (scene visible), 1 = black (scene hidden)</param>
    public void SetFadeAlpha(float alpha)
    {
        if (fadePanel == null)
        {
            Debug.LogWarning("[SceneFadeController] Fade panel null, cannot set alpha");
            return;
        }

        Color color = fadePanel.color;
        color.a = Mathf.Clamp01(alpha);
        fadePanel.color = color;
    }

    /// <summary>
    /// Fade out, tunggu delay, kemudian fade in
    /// Useful untuk transition antar scene
    /// </summary>
    public void FadeOutInSequence(float delayBetween = 0f, Action onComplete = null)
    {
        FadeOut(() =>
        {
            if (delayBetween > 0f)
            {
                StartCoroutine(WaitThenFadeIn(delayBetween, onComplete));
            }
            else
            {
                FadeIn(onComplete);
            }
        });
    }

    private IEnumerator WaitThenFadeIn(float delay, Action onComplete)
    {
        yield return new WaitForSeconds(delay);
        FadeIn(onComplete);
    }

    /// <summary>
    /// Show black screen (instant)
    /// </summary>
    public void ShowBlackScreen()
    {
        SetFadeAlpha(1f);
    }

    /// <summary>
    /// Hide black screen (instant)
    /// </summary>
    public void HideBlackScreen()
    {
        SetFadeAlpha(0f);
    }

    private void OnDestroy()
    {
        // Kill all tweens
        if (fadePanel != null)
        {
            DOTween.Kill(fadePanel);
        }
    }
}
