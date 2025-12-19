using UnityEngine;

/// <summary>
/// Simple Audio Manager untuk Chapter 1
/// Mengelola sound effects untuk feedback (benar/salah)
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

    private static Chapter1AudioManager instance;

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
        if (musicSource != null && backgroundMusic != null)
        {
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

    private void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip, sfxVolume);
        }
    }

    // Public static method untuk akses global (opsional)
    public static Chapter1AudioManager Instance
    {
        get { return instance; }
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
