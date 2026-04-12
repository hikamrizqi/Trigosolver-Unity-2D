using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Global Audio Manager - Persistent across scenes
/// Mengelola BGM untuk Main Menu, Story Panel, dan transisi antar scene
/// Menggunakan DontDestroyOnLoad untuk persistence
/// </summary>
public class GlobalAudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Background Music Clips")]
    [Tooltip("BGM untuk Main Menu")]
    [SerializeField] private AudioClip mainMenuBGM;

    [Tooltip("BGM untuk Story Panel (Chapter 1 intro)")]
    [SerializeField] private AudioClip storyPanelBGM;

    [Tooltip("BGM untuk Gameplay Chapter 1")]
    [SerializeField] private AudioClip gameplayChapter1BGM;

    [Header("Global Sound Effects")]
    [SerializeField] private AudioClip buttonClickSFX;
    [SerializeField] private AudioClip transitionSFX;

    [Header("Volume Settings")]
    [SerializeField][Range(0f, 1f)] private float bgmVolume = 0.5f;
    [SerializeField][Range(0f, 1f)] private float sfxVolume = 0.7f;

    [Header("Fade Settings")]
    [SerializeField] private float fadeInDuration = 1.0f;
    [SerializeField] private float fadeOutDuration = 0.5f;

    private static GlobalAudioManager instance;
    private string currentBGMType = "";
    private bool isFading = false;

    public static GlobalAudioManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        // Singleton pattern with DontDestroyOnLoad
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[GlobalAudioManager] Instance created and marked as DontDestroyOnLoad");
        }
        else
        {
            Debug.Log("[GlobalAudioManager] Duplicate instance detected - destroying");
            Destroy(gameObject);
            return;
        }

        // Setup audio sources
        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
            bgmSource.playOnAwake = false;
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
        }

        bgmSource.volume = bgmVolume;
        sfxSource.volume = sfxVolume;
    }

    void Start()
    {
        // Subscribe to scene change events
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // Unsubscribe when destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Called when a new scene is loaded
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[GlobalAudioManager] Scene loaded: {scene.name}");

        // Auto-play appropriate BGM based on scene name
        // You can customize these scene names based on your project
        if (scene.name.Contains("MainMenu") || scene.name.Contains("Menu"))
        {
            PlayMainMenuBGM();
        }
        else if (scene.name.Contains("Story") || scene.name.Contains("Intro"))
        {
            PlayStoryPanelBGM();
        }
        else if (scene.name.Contains("Chapter1") || scene.name.Contains("Level"))
        {
            // BGM will be controlled by Chapter1AudioManager
            // But we can start with gameplay BGM
            // PlayGameplayChapter1BGM();
        }
    }

    #region BGM Control Methods

    /// <summary>
    /// Play Main Menu BGM
    /// </summary>
    public void PlayMainMenuBGM()
    {
        if (currentBGMType == "MainMenu" && bgmSource.isPlaying)
        {
            Debug.Log("[GlobalAudioManager] Main Menu BGM already playing");
            return;
        }

        PlayBGM(mainMenuBGM, "MainMenu");
    }

    /// <summary>
    /// Play Story Panel BGM
    /// </summary>
    public void PlayStoryPanelBGM()
    {
        if (currentBGMType == "StoryPanel" && bgmSource.isPlaying)
        {
            Debug.Log("[GlobalAudioManager] Story Panel BGM already playing");
            return;
        }

        PlayBGM(storyPanelBGM, "StoryPanel");
    }

    /// <summary>
    /// Play Gameplay Chapter 1 BGM
    /// </summary>
    public void PlayGameplayChapter1BGM()
    {
        if (currentBGMType == "GameplayChapter1" && bgmSource.isPlaying)
        {
            Debug.Log("[GlobalAudioManager] Gameplay Chapter 1 BGM already playing");
            return;
        }

        PlayBGM(gameplayChapter1BGM, "GameplayChapter1");
    }

    /// <summary>
    /// Internal method to play BGM with fade
    /// </summary>
    private void PlayBGM(AudioClip clip, string bgmType)
    {
        if (clip == null)
        {
            Debug.LogWarning($"[GlobalAudioManager] BGM clip for {bgmType} is null!");
            return;
        }

        Debug.Log($"[GlobalAudioManager] Playing {bgmType} BGM");

        // If same clip, just ensure it's playing
        if (bgmSource.clip == clip && bgmSource.isPlaying)
        {
            currentBGMType = bgmType;
            return;
        }

        // Fade out current, then fade in new
        if (bgmSource.isPlaying)
        {
            StartCoroutine(CrossfadeBGM(clip, bgmType));
        }
        else
        {
            // Directly play if nothing is playing
            bgmSource.clip = clip;
            bgmSource.Play();
            currentBGMType = bgmType;
            StartCoroutine(FadeInBGM());
        }
    }

    /// <summary>
    /// Stop BGM with fade out
    /// </summary>
    public void StopBGM()
    {
        if (bgmSource.isPlaying)
        {
            StartCoroutine(FadeOutBGM(true));
        }
    }

    /// <summary>
    /// Pause BGM (can be resumed)
    /// </summary>
    public void PauseBGM()
    {
        if (bgmSource != null)
        {
            bgmSource.Pause();
            Debug.Log("[GlobalAudioManager] BGM Paused");
        }
    }

    /// <summary>
    /// Resume paused BGM
    /// </summary>
    public void ResumeBGM()
    {
        if (bgmSource != null && !bgmSource.isPlaying)
        {
            bgmSource.UnPause();
            Debug.Log("[GlobalAudioManager] BGM Resumed");
        }
    }

    #endregion

    #region Fade Coroutines

    private System.Collections.IEnumerator FadeInBGM()
    {
        if (isFading) yield break;
        isFading = true;

        float startVolume = 0f;
        bgmSource.volume = startVolume;

        float elapsed = 0f;
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, bgmVolume, elapsed / fadeInDuration);
            yield return null;
        }

        bgmSource.volume = bgmVolume;
        isFading = false;
    }

    private System.Collections.IEnumerator FadeOutBGM(bool stopAfterFade = false)
    {
        if (isFading) yield break;
        isFading = true;

        float startVolume = bgmSource.volume;
        float elapsed = 0f;

        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeOutDuration);
            yield return null;
        }

        bgmSource.volume = 0f;

        if (stopAfterFade)
        {
            bgmSource.Stop();
            currentBGMType = "";
        }

        isFading = false;
    }

    private System.Collections.IEnumerator CrossfadeBGM(AudioClip newClip, string newBGMType)
    {
        // Fade out current
        yield return StartCoroutine(FadeOutBGM(false));

        // Switch clip
        bgmSource.clip = newClip;
        bgmSource.Play();
        currentBGMType = newBGMType;

        // Fade in new
        yield return StartCoroutine(FadeInBGM());
    }

    #endregion

    #region SFX Methods

    /// <summary>
    /// Play button click SFX
    /// </summary>
    public void PlayButtonClickSFX()
    {
        Debug.Log("[GlobalAudioManager] PlayButtonClickSFX called");
        if (buttonClickSFX == null)
        {
            Debug.LogWarning("[GlobalAudioManager] Button Click SFX clip is null! Please assign in Inspector.");
        }
        PlaySFX(buttonClickSFX);
    }

    /// <summary>
    /// Play transition SFX
    /// </summary>
    public void PlayTransitionSFX()
    {
        PlaySFX(transitionSFX);
    }

    /// <summary>
    /// Play any SFX
    /// </summary>
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            Debug.Log($"[GlobalAudioManager] Playing SFX: {clip.name} at volume {sfxVolume}");
            sfxSource.PlayOneShot(clip, sfxVolume);
        }
        else
        {
            if (sfxSource == null)
                Debug.LogWarning("[GlobalAudioManager] SFX Source is null!");
            if (clip == null)
                Debug.LogWarning("[GlobalAudioManager] Audio clip is null!");
        }
    }

    #endregion

    #region Volume Control

    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        if (bgmSource != null)
        {
            bgmSource.volume = bgmVolume;
        }
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
        }
    }

    public void MuteAll()
    {
        if (bgmSource != null) bgmSource.mute = true;
        if (sfxSource != null) sfxSource.mute = true;
    }

    public void UnmuteAll()
    {
        if (bgmSource != null) bgmSource.mute = false;
        if (sfxSource != null) sfxSource.mute = false;
    }

    #endregion

    #region Utility

    public bool IsBGMPlaying()
    {
        return bgmSource != null && bgmSource.isPlaying;
    }

    public string GetCurrentBGMType()
    {
        return currentBGMType;
    }

    #endregion
}
