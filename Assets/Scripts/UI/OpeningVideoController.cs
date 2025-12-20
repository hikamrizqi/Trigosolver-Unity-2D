using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Controller untuk opening video scene
/// Auto-play video dan transition ke main menu setelah selesai
/// </summary>
public class OpeningVideoController : MonoBehaviour
{
    [Header("Video Player")]
    [Tooltip("Video Player component (auto-detect jika null)")]
    [SerializeField] private VideoPlayer videoPlayer;

    [Header("Scene Transition")]
    [Tooltip("Nama scene yang akan di-load setelah video selesai")]
    [SerializeField] private string nextSceneName = "Main Menu";

    [Tooltip("Durasi hold di frame terakhir (detik)")]
    [SerializeField] private float holdDuration = 1f;

    [Tooltip("Durasi fade out video (detik)")]
    [SerializeField] private float fadeDuration = 1f;

    [Tooltip("Durasi black screen hold sebelum load scene (detik)")]
    [SerializeField] private float blackScreenHoldDuration = 0.5f;

    [Header("Skip Settings")]
    [Tooltip("Allow skip video dengan tombol/klik")]
    [SerializeField] private bool allowSkip = true;

    [Tooltip("Delay sebelum bisa skip (detik) - cegah skip tidak sengaja")]
    [SerializeField] private float skipDelayTime = 1f;

    [Header("Audio")]
    [Tooltip("Volume video (0-1)")]
    [SerializeField][Range(0f, 1f)] private float videoVolume = 1f;

    [Header("Fade Panel")]
    [Tooltip("Panel untuk fade effect (auto-create jika null)")]
    [SerializeField] private Image fadePanel;

    private bool canSkip = false;
    private bool isTransitioning = false;
    private Canvas fadeCanvas;

    private void Awake()
    {
        // Auto-detect VideoPlayer component
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();

            if (videoPlayer == null)
            {
                videoPlayer = FindObjectOfType<VideoPlayer>();
            }

            if (videoPlayer == null)
            {
                Debug.LogError("[OpeningVideoController] VideoPlayer component tidak ditemukan!");
                LoadNextScene();
                return;
            }
        }

        // Setup video player
        videoPlayer.playOnAwake = true;
        videoPlayer.isLooping = false;
        videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;

        // Set audio
        if (videoPlayer.audioOutputMode == VideoAudioOutputMode.AudioSource)
        {
            AudioSource audioSource = videoPlayer.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.volume = videoVolume;
            }
        }
        else if (videoPlayer.audioOutputMode == VideoAudioOutputMode.Direct)
        {
            videoPlayer.SetDirectAudioVolume(0, videoVolume);
        }

        // Setup fade panel
        SetupFadePanel();
    }

    private void Start()
    {
        // Subscribe ke event video selesai
        videoPlayer.loopPointReached += OnVideoFinished;

        // Prepare video
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += OnVideoPrepared;

        // Start skip delay timer
        if (allowSkip)
        {
            StartCoroutine(EnableSkipAfterDelay());
        }

        Debug.Log("[OpeningVideoController] Video player initialized");
    }

    private void OnVideoPrepared(VideoPlayer vp)
    {
        Debug.Log("[OpeningVideoController] Video prepared, starting playback...");
        videoPlayer.Play();
    }

    private void Update()
    {
        // Check untuk skip input
        if (allowSkip && canSkip && !isTransitioning)
        {
            if (Input.GetMouseButtonDown(0) || Input.anyKeyDown)
            {
                Debug.Log("[OpeningVideoController] Video skipped by user");
                SkipVideo();
            }
        }
    }

    /// <summary>
    /// Called when video reaches end
    /// </summary>
    private void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("[OpeningVideoController] Video finished");

        if (!isTransitioning)
        {
            StartCoroutine(TransitionToNextScene());
        }
    }

    /// <summary>
    /// Skip video dan langsung ke main menu
    /// </summary>
    private void SkipVideo()
    {
        if (isTransitioning) return;

        videoPlayer.Stop();
        StartCoroutine(TransitionToNextScene());
    }

    /// <summary>
    /// Enable skip setelah delay
    /// </summary>
    private IEnumerator EnableSkipAfterDelay()
    {
        yield return new WaitForSeconds(skipDelayTime);
        canSkip = true;
        Debug.Log("[OpeningVideoController] Skip enabled");
    }

    /// <summary>
    /// Transition ke scene berikutnya dengan hold dan fade effect
    /// Flow: Video pause → Hold → Fade to black → Black screen hold → Load scene (dengan SceneFadeController)
    /// </summary>
    private IEnumerator TransitionToNextScene()
    {
        isTransitioning = true;

        // Pause video di frame terakhir
        videoPlayer.Pause();
        Debug.Log("[OpeningVideoController] Video paused at last frame");

        // Hold selama duration yang ditentukan
        yield return new WaitForSeconds(holdDuration);

        // Fade out audio
        StartCoroutine(FadeOutAudio());

        // Fade to black
        yield return StartCoroutine(FadeToBlack());

        // Hold di black screen sebentar sebelum load scene
        Debug.Log($"[OpeningVideoController] Holding black screen for {blackScreenHoldDuration}s");
        yield return new WaitForSeconds(blackScreenHoldDuration);

        // Load scene berikutnya
        // SceneFadeController akan auto fade in di scene baru
        LoadNextScene();
    }

    /// <summary>
    /// Fade audio to zero
    /// </summary>
    private IEnumerator FadeOutAudio()
    {
        if (videoPlayer.audioOutputMode == VideoAudioOutputMode.AudioSource)
        {
            AudioSource audioSource = videoPlayer.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                float startVolume = audioSource.volume;
                float elapsed = 0f;

                while (elapsed < fadeDuration)
                {
                    elapsed += Time.deltaTime;
                    audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeDuration);
                    yield return null;
                }

                audioSource.volume = 0f;
            }
        }
    }

    /// <summary>
    /// Fade to black dengan panel UI
    /// </summary>
    private IEnumerator FadeToBlack()
    {
        if (fadePanel == null)
        {
            Debug.LogWarning("[OpeningVideoController] Fade panel tidak ada, skip fade effect");
            yield break;
        }

        // Set panel aktif dan alpha awal
        fadePanel.gameObject.SetActive(true);
        Color startColor = fadePanel.color;
        startColor.a = 0f;
        fadePanel.color = startColor;

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);

            Color newColor = fadePanel.color;
            newColor.a = alpha;
            fadePanel.color = newColor;

            yield return null;
        }

        // Pastikan alpha full
        Color finalColor = fadePanel.color;
        finalColor.a = 1f;
        fadePanel.color = finalColor;

        Debug.Log("[OpeningVideoController] Fade to black completed");
    }

    /// <summary>
    /// Setup fade panel (auto-create jika belum ada)
    /// </summary>
    private void SetupFadePanel()
    {
        if (fadePanel != null) return;

        // Find existing fade panel
        fadePanel = GameObject.Find("FadePanel")?.GetComponent<Image>();

        if (fadePanel == null)
        {
            // Auto-create fade canvas dan panel
            GameObject canvasObj = new GameObject("FadeCanvas");
            fadeCanvas = canvasObj.AddComponent<Canvas>();
            fadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            fadeCanvas.sortingOrder = 9999; // Render paling depan

            canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
            canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();

            // Create fade panel
            GameObject panelObj = new GameObject("FadePanel");
            panelObj.transform.SetParent(canvasObj.transform, false);

            fadePanel = panelObj.AddComponent<Image>();
            fadePanel.color = new Color(0, 0, 0, 0); // Black, transparent

            RectTransform rect = panelObj.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;

            panelObj.SetActive(false);

            Debug.Log("[OpeningVideoController] Fade panel auto-created");
        }
    }

    /// <summary>
    /// Load scene berikutnya
    /// </summary>
    private void LoadNextScene()
    {
        Debug.Log($"[OpeningVideoController] Loading scene: {nextSceneName}");

        // Cek apakah scene ada di build settings
        if (Application.CanStreamedLevelBeLoaded(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError($"[OpeningVideoController] Scene '{nextSceneName}' tidak ditemukan di Build Settings!");
            Debug.LogError("Tambahkan scene ke File > Build Settings > Scenes In Build");
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe events
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
            videoPlayer.prepareCompleted -= OnVideoPrepared;
        }
    }

    /// <summary>
    /// Public method untuk skip dari UI button
    /// </summary>
    public void OnSkipButtonClicked()
    {
        if (canSkip)
        {
            SkipVideo();
        }
    }
}
