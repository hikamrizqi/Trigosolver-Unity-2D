using UnityEngine;

/// <summary>
/// Enhanced Audio Manager untuk Chapter 1
/// Mengelola sound effects dan BGM gameplay
/// Bekerja sama dengan GlobalAudioManager untuk BGM persistence
/// </summary>
public class Chapter1AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip correctAnswerSFX;
    [SerializeField] private AudioClip wrongAnswerSFX;
    [SerializeField] private AudioClip buttonClickSFX;
    [SerializeField] private AudioClip highlightSFX;
    [SerializeField] private AudioClip gameOverSFX;
    [SerializeField] private AudioClip victoryMusicSFX;

    [Header("Background Music")]
    [SerializeField] private AudioClip backgroundMusic;

    [Header("Volume Settings")]
    [SerializeField][Range(0f, 1f)] private float sfxVolume = 0.7f;
    [SerializeField][Range(0f, 1f)] private float musicVolume = 0.5f;

    [Header("Integration")]
    [SerializeField] private bool useGlobalAudioManagerForBGM = true;

    private static Chapter1AudioManager instance;
    private bool bgmWasPlayingBeforeGameOver = false;

    public static Chapter1AudioManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        // Singleton pattern (opsional)
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Set volume
        if (sfxSource != null)
            sfxSource.volume = sfxVolume;

        if (musicSource != null)
            musicSource.volume = musicVolume;

        // Play background music
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        // Check if should use GlobalAudioManager
        if (useGlobalAudioManagerForBGM && GlobalAudioManager.Instance != null)
        {
            Debug.Log("[Chapter1Audio] Using GlobalAudioManager for BGM");
            GlobalAudioManager.Instance.PlayGameplayChapter1BGM();
        }
        else if (musicSource != null && backgroundMusic != null)
        {
            Debug.Log("[Chapter1Audio] Using local BGM");
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlayCorrectAnswerSFX()
    {
        PlaySFX(correctAnswerSFX);
    }

    public void PlayWrongAnswerSFX()
    {
        PlaySFX(wrongAnswerSFX);
    }

    public void PlayButtonClickSFX()
    {
        PlaySFX(buttonClickSFX);
    }

    public void PlayHighlightSFX()
    {
        PlaySFX(highlightSFX);
    }

    public void PlayGameOverSFX()
    {
        PlaySFX(gameOverSFX);
    }

    public void PlayVictoryMusic()
    {
        if (musicSource != null && victoryMusicSFX != null)
        {
            musicSource.Stop();
            musicSource.clip = victoryMusicSFX;
            musicSource.loop = false;
            musicSource.Play();
        }
    }

    /// <summary>
    /// Stop BGM saat game over
    /// </summary>
    public void StopBGMForGameOver()
    {
        Debug.Log("[Chapter1Audio] Stopping BGM for Game Over");

        if (useGlobalAudioManagerForBGM && GlobalAudioManager.Instance != null)
        {
            // Stop global BGM
            bgmWasPlayingBeforeGameOver = GlobalAudioManager.Instance.IsBGMPlaying();
            GlobalAudioManager.Instance.StopBGM();
        }
        else if (musicSource != null && musicSource.isPlaying)
        {
            // Stop local BGM
            bgmWasPlayingBeforeGameOver = true;
            musicSource.Stop();
        }
    }

    /// <summary>
    /// Resume BGM setelah game over (saat back button clicked)
    /// </summary>
    public void ResumeBGMAfterGameOver()
    {
        Debug.Log("[Chapter1Audio] Resuming BGM after Game Over");

        if (useGlobalAudioManagerForBGM && GlobalAudioManager.Instance != null)
        {
            // Resume global BGM (akan play gameplay BGM)
            if (bgmWasPlayingBeforeGameOver)
            {
                GlobalAudioManager.Instance.PlayGameplayChapter1BGM();
            }
        }
        else if (bgmWasPlayingBeforeGameOver && musicSource != null)
        {
            // Resume local BGM
            PlayBackgroundMusic();
        }

        bgmWasPlayingBeforeGameOver = false;
    }

    /// <summary>
    /// Mute/Unmute all audio
    /// </summary>
    public void SetMute(bool mute)
    {
        if (sfxSource != null)
            sfxSource.mute = mute;
        if (musicSource != null)
            musicSource.mute = mute;
    }

    private void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip, sfxVolume);
        }
    }

    // Methods untuk mengubah volume saat runtime
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        if (sfxSource != null)
            sfxSource.volume = sfxVolume;
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
            musicSource.volume = musicVolume;
    }

    public void MuteAll()
    {
        if (sfxSource != null)
            sfxSource.mute = true;
        if (musicSource != null)
            musicSource.mute = true;
    }

    public void UnmuteAll()
    {
        if (sfxSource != null)
            sfxSource.mute = false;
        if (musicSource != null)
            musicSource.mute = false;
    }
}
