using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;

/// <summary>
/// Menampilkan slideshow gambar (story dengan dialog, materi, tutorial) saat pertama kali masuk Stage 1
/// 4 panel pertama = Story dengan typewriter effect
/// Panel selanjutnya = Materi/Tutorial tanpa dialog
/// </summary>
public class StoryPanel : MonoBehaviour
{
    [Header("Story Panel UI")]
    [SerializeField] private GameObject storyPanel;
    [SerializeField] private Image storyImage; // Gambar yang akan berubah
    [SerializeField] private GameObject clickToContinueText; // Text "Click anywhere to continue"

    [Header("Story Dialog System (Panel 1-4)")]
    [SerializeField] private GameObject dialogBox; // Panel dialog box
    [SerializeField] private TextMeshProUGUI dialogText; // Text untuk dialog
    [Tooltip("4 dialog untuk 4 panel story pertama")]
    [SerializeField][TextArea(3, 10)] private List<string> storyDialogs = new List<string>(); // Dialog untuk story panels

    [Header("Slideshow Images")]
    [Tooltip("Urutan: 5 Story panels (0-4) → 2 Materi (5-6) → 1 Tutorial (7)")]
    [SerializeField] private List<Sprite> slideImages = new List<Sprite>(); // List gambar untuk slideshow

    [Header("Materi & Tutorial Settings")]
    [Tooltip("Index awal untuk materi (setelah story panels)")]
    [SerializeField] private int materiStartIndex = 5; // Materi dimulai dari index 5
    [Tooltip("Jumlah slide materi + tutorial")]
    [SerializeField] private int materiTutorialCount = 3; // 2 materi + 1 tutorial

    [Header("Typewriter Settings")]
    [SerializeField] private float typewriterSpeed = 0.05f; // Delay per karakter
    [SerializeField] private bool skipTypewriterOnClick = true; // Skip typewriter saat klik

    [Header("Animation Settings")]
    [SerializeField] private float fadeInDuration = 0.5f;
    [SerializeField] private float fadeOutDuration = 0.3f;
    [SerializeField] private float slideTransitionDuration = 0.3f; // Durasi transition antar slide
    [SerializeField] private float textBlinkSpeed = 1f; // Kecepatan blink text

    [Header("Manager References")]
    [SerializeField] private LevelSelectionManager levelSelectionManager;

    private bool canClick = false;
    private bool isTransitioning = false;
    private int currentSlideIndex = 0;
    private CanvasGroup panelCanvasGroup;
    private CanvasGroup imageCanvasGroup;

    // Typewriter state
    private bool isTyping = false;
    private bool isDialogComplete = false;
    private Coroutine typewriterCoroutine;
    private int storyPanelCount = 5; // 5 panel story dengan dialog
    private bool skipStoryMode = false; // Mode untuk skip story (langsung ke materi)

    private void Awake()
    {
        // Get or add CanvasGroup untuk fade animation
        if (storyPanel != null)
        {
            panelCanvasGroup = storyPanel.GetComponent<CanvasGroup>();
            if (panelCanvasGroup == null)
            {
                panelCanvasGroup = storyPanel.AddComponent<CanvasGroup>();
            }
        }

        // Get or add CanvasGroup untuk image fade transition
        if (storyImage != null)
        {
            imageCanvasGroup = storyImage.GetComponent<CanvasGroup>();
            if (imageCanvasGroup == null)
            {
                imageCanvasGroup = storyImage.gameObject.AddComponent<CanvasGroup>();
            }
        }
    }

    private void Start()
    {
        // Validate slides
        if (slideImages == null || slideImages.Count == 0)
        {
            Debug.LogWarning("[StoryPanel] No slide images assigned! Skipping to level selection.");
            if (levelSelectionManager != null)
                levelSelectionManager.ShowLevelSelection();
            return;
        }

        // Show first slide
        ShowStoryPanel();
    }

    private void Update()
    {
        // Detect click anywhere
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log($"[StoryPanel] Click detected - canClick: {canClick}, isTransitioning: {isTransitioning}");
        }

        if (canClick && !isTransitioning && Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
    }

    /// <summary>
    /// Handle click berdasarkan state
    /// - Jika sedang typing → complete instantly
    /// - Jika dialog complete → next slide
    /// </summary>
    private void HandleClick()
    {
        Debug.Log($"[StoryPanel] HandleClick - Index: {currentSlideIndex}, IsStoryPanel: {IsStoryPanel()}, skipStoryMode: {skipStoryMode}");

        // Jika di story panel (0-3) dan ada dialog
        if (IsStoryPanel())
        {
            if (isTyping)
            {
                // Klik pertama: skip typewriter, complete dialog instantly
                CompleteTypewriterInstantly();
            }
            else if (isDialogComplete)
            {
                // Klik kedua: next slide
                if (currentSlideIndex < slideImages.Count - 1)
                {
                    NextSlide();
                }
                else
                {
                    CloseStoryPanel();
                }
            }
        }
        else
        {
            // Materi/tutorial panel (no dialog, langsung next)
            // Cek apakah masih ada slide berikutnya berdasarkan mode
            int maxSlideIndex = skipStoryMode
                ? (materiStartIndex + materiTutorialCount - 1) // Materi mode: hanya sampai slide materi terakhir
                : (slideImages.Count - 1); // Normal mode: sampai slide terakhir

            Debug.Log($"[StoryPanel] Materi mode - currentIndex: {currentSlideIndex}, maxIndex: {maxSlideIndex}");

            if (currentSlideIndex < maxSlideIndex)
            {
                NextSlide();
            }
            else
            {
                CloseStoryPanel();
            }
        }
    }

    /// <summary>
    /// Check apakah current slide adalah story panel (dengan dialog)
    /// </summary>
    private bool IsStoryPanel()
    {
        return currentSlideIndex < storyPanelCount && currentSlideIndex < storyDialogs.Count;
    }

    /// <summary>
    /// Tampilkan story panel dengan fade in animation
    /// </summary>
    private void ShowStoryPanel()
    {
        if (storyPanel == null)
        {
            Debug.LogWarning("[StoryPanel] Story panel not assigned!");
            if (levelSelectionManager != null)
                levelSelectionManager.ShowLevelSelection();
            return;
        }

        // Set first slide image
        if (storyImage != null && slideImages.Count > 0)
        {
            storyImage.sprite = slideImages[0];
            currentSlideIndex = 0;
        }

        // Activate panel
        storyPanel.SetActive(true);

        // Show dialog box jika story panel
        if (dialogBox != null)
        {
            dialogBox.SetActive(IsStoryPanel());
        }

        // Start with alpha 0
        if (panelCanvasGroup != null)
        {
            panelCanvasGroup.alpha = 0f;

            // Fade in
            panelCanvasGroup.DOFade(1f, fadeInDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    canClick = true;

                    // Jika story panel, mulai typewriter
                    if (IsStoryPanel())
                    {
                        StartTypewriter();
                    }
                    else
                    {
                        // Materi/tutorial panel, show blink text
                        StartTextBlink();
                    }
                });
        }
        else
        {
            canClick = true;
            if (IsStoryPanel())
            {
                StartTypewriter();
            }
            else
            {
                StartTextBlink();
            }
        }

        Debug.Log($"[StoryPanel] Showing slide 1/{slideImages.Count}");
    }

    /// <summary>
    /// Next slide dengan fade transition
    /// </summary>
    private void NextSlide()
    {
        if (isTransitioning) return;
        isTransitioning = true;
        canClick = false;

        // Reset dialog state
        isDialogComplete = false;
        isTyping = false;

        // Stop typewriter jika sedang berjalan
        if (typewriterCoroutine != null)
        {
            StopCoroutine(typewriterCoroutine);
            typewriterCoroutine = null;
        }

        currentSlideIndex++;

        Debug.Log($"[StoryPanel] Transitioning to slide {currentSlideIndex + 1}/{slideImages.Count}");

        // Show/hide dialog box based on panel type
        if (dialogBox != null)
        {
            dialogBox.SetActive(IsStoryPanel());
        }

        // Fade out current image
        if (imageCanvasGroup != null)
        {
            imageCanvasGroup.DOFade(0f, slideTransitionDuration)
                .SetEase(Ease.InQuad)
                .OnComplete(() =>
                {
                    // Change sprite
                    if (storyImage != null && currentSlideIndex < slideImages.Count)
                    {
                        storyImage.sprite = slideImages[currentSlideIndex];
                    }

                    // Fade in new image
                    imageCanvasGroup.DOFade(1f, slideTransitionDuration)
                        .SetEase(Ease.OutQuad)
                        .OnComplete(() =>
                        {
                            isTransitioning = false;
                            canClick = true;

                            // Start typewriter jika story panel, atau text blink jika materi
                            if (IsStoryPanel())
                            {
                                StartTypewriter();
                            }
                            else
                            {
                                StartTextBlink();
                            }

                            Debug.Log($"[StoryPanel] Now showing slide {currentSlideIndex + 1}/{slideImages.Count}");
                        });
                });
        }
        else
        {
            // No fade animation, instant change
            if (storyImage != null && currentSlideIndex < slideImages.Count)
            {
                storyImage.sprite = slideImages[currentSlideIndex];
            }
            isTransitioning = false;
            canClick = true;

            if (IsStoryPanel())
            {
                StartTypewriter();
            }
            else
            {
                StartTextBlink();
            }
        }
    }

    /// <summary>
    /// Tutup story panel dengan fade out animation
    /// </summary>
    private void CloseStoryPanel()
    {
        if (isTransitioning) return;
        isTransitioning = true;
        canClick = false;

        Debug.Log("[StoryPanel] Closing story panel...");

        // Stop typewriter jika sedang berjalan
        if (typewriterCoroutine != null)
        {
            StopCoroutine(typewriterCoroutine);
            typewriterCoroutine = null;
        }

        // Stop text blink animation
        if (clickToContinueText != null)
        {
            DOTween.Kill(clickToContinueText);
        }

        // Fade out panel
        if (panelCanvasGroup != null)
        {
            panelCanvasGroup.DOFade(0f, fadeOutDuration)
                .SetEase(Ease.InQuad)
                .OnComplete(() =>
                {
                    storyPanel.SetActive(false);

                    // Reset skip story mode
                    skipStoryMode = false;

                    // Show level selection setelah story panel tertutup
                    if (levelSelectionManager != null)
                    {
                        levelSelectionManager.ShowLevelSelection();
                    }
                    else
                    {
                        Debug.LogError("[StoryPanel] LevelSelectionManager not assigned!");
                    }
                });
        }
        else
        {
            // No animation, just hide
            storyPanel.SetActive(false);
            skipStoryMode = false; // Reset skip story mode
            if (levelSelectionManager != null)
                levelSelectionManager.ShowLevelSelection();
        }
    }

    /// <summary>
    /// Animasi blink untuk "Click to continue" text
    /// </summary>
    private void StartTextBlink()
    {
        if (clickToContinueText == null) return;

        CanvasGroup textCanvasGroup = clickToContinueText.GetComponent<CanvasGroup>();
        if (textCanvasGroup == null)
        {
            textCanvasGroup = clickToContinueText.AddComponent<CanvasGroup>();
        }

        // Loop blink animation
        textCanvasGroup.DOFade(0.3f, textBlinkSpeed)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    #region Typewriter System

    /// <summary>
    /// Mulai typewriter effect untuk dialog
    /// </summary>
    private void StartTypewriter()
    {
        if (dialogText == null || !IsStoryPanel())
        {
            Debug.LogWarning("[StoryPanel] Cannot start typewriter: dialogText not assigned or not story panel");
            return;
        }

        // Get dialog untuk current slide
        string dialog = storyDialogs[currentSlideIndex];

        // Clear previous text
        dialogText.text = "";
        isDialogComplete = false;
        isTyping = true;

        // Start typewriter coroutine
        typewriterCoroutine = StartCoroutine(TypewriterCoroutine(dialog));

        Debug.Log($"[StoryPanel] Starting typewriter for panel {currentSlideIndex + 1}");
    }

    /// <summary>
    /// Coroutine untuk typewriter effect
    /// </summary>
    private IEnumerator TypewriterCoroutine(string fullText)
    {
        isTyping = true;
        dialogText.text = "";

        foreach (char letter in fullText.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(typewriterSpeed);
        }

        // Typewriter selesai
        isTyping = false;
        isDialogComplete = true;
        typewriterCoroutine = null;

        Debug.Log("[StoryPanel] Typewriter complete");
    }

    /// <summary>
    /// Complete typewriter instantly (skip animation)
    /// </summary>
    private void CompleteTypewriterInstantly()
    {
        if (typewriterCoroutine != null)
        {
            StopCoroutine(typewriterCoroutine);
            typewriterCoroutine = null;
        }

        // Set full text langsung
        if (dialogText != null && IsStoryPanel())
        {
            dialogText.text = storyDialogs[currentSlideIndex];
        }

        isTyping = false;
        isDialogComplete = true;

        Debug.Log("[StoryPanel] Typewriter skipped to complete");
    }

    #endregion

    /// <summary>
    /// Public method untuk show story panel dari script lain jika diperlukan
    /// </summary>
    public void Show()
    {
        ShowStoryPanel();
    }

    /// <summary>
    /// Public method untuk show HANYA materi & tutorial (skip story)
    /// Dipanggil dari button "Materi" di level selection
    /// </summary>
    public void ShowMateriOnly()
    {
        Debug.Log("[StoryPanel] Showing Materi & Tutorial only (skipping story panels)");

        // Reset states
        skipStoryMode = true;
        canClick = false;
        isTransitioning = false;
        isTyping = false;
        isDialogComplete = false;

        // Validate materi slides
        if (slideImages == null || slideImages.Count < materiStartIndex + materiTutorialCount)
        {
            Debug.LogWarning($"[StoryPanel] Not enough slides for materi! Need at least {materiStartIndex + materiTutorialCount} slides.");
            if (levelSelectionManager != null)
                levelSelectionManager.ShowLevelSelection();
            return;
        }

        // Set index ke materi pertama
        currentSlideIndex = materiStartIndex;

        // Activate panel
        if (storyPanel != null)
            storyPanel.SetActive(true);

        // Hide dialog box (no dialog for materi)
        if (dialogBox != null)
            dialogBox.SetActive(false);

        // Set materi image
        if (storyImage != null)
        {
            storyImage.sprite = slideImages[currentSlideIndex];
        }

        // Reset image alpha to 1 (visible)
        if (imageCanvasGroup != null)
        {
            imageCanvasGroup.alpha = 1f;
        }

        // Fade in panel
        if (panelCanvasGroup != null)
        {
            panelCanvasGroup.alpha = 0f;
            panelCanvasGroup.DOFade(1f, fadeInDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    canClick = true;
                    isTransitioning = false;
                    StartTextBlink(); // Show "click to continue"
                    Debug.Log($"[StoryPanel] Materi ready - canClick: {canClick}, isTransitioning: {isTransitioning}, currentIndex: {currentSlideIndex}");
                });
        }
        else
        {
            canClick = true;
            isTransitioning = false;
            StartTextBlink();
            Debug.Log($"[StoryPanel] Materi ready (no fade) - canClick: {canClick}, currentIndex: {currentSlideIndex}");
        }

        Debug.Log($"[StoryPanel] Showing materi slide {currentSlideIndex + 1}/{slideImages.Count}");
    }

    /// <summary>
    /// Public method untuk close story panel dari script lain jika diperlukan
    /// </summary>
    public void Close()
    {
        CloseStoryPanel();
    }

    private void OnDestroy()
    {
        // Kill all tweens untuk prevent memory leak
        if (panelCanvasGroup != null)
            DOTween.Kill(panelCanvasGroup);

        if (imageCanvasGroup != null)
            DOTween.Kill(imageCanvasGroup);

        if (clickToContinueText != null)
            DOTween.Kill(clickToContinueText);
    }
}
