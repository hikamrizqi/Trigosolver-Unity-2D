using UnityEngine;

/// <summary>
/// Global manager untuk button sound effects
/// Semua button yang tidak punya custom sound akan menggunakan default sound dari manager ini
/// Singleton pattern - hanya ada 1 instance di scene
/// </summary>
public class GlobalButtonSoundManager : MonoBehaviour
{
    [Header("Default Button Sound")]
    [Tooltip("Default audio clip untuk semua button")]
    [SerializeField] private AudioClip defaultClickSound;

    [Header("Audio Source Settings")]
    [Tooltip("Volume default (0-1)")]
    [SerializeField][Range(0f, 1f)] private float defaultVolume = 1f;

    [Tooltip("Pitch default (0.5-2.0)")]
    [SerializeField][Range(0.5f, 2f)] private float defaultPitch = 1f;

    [Header("Auto-Setup")]
    [Tooltip("Otomatis attach ButtonSoundEffect ke semua button di scene saat Start")]
    [SerializeField] private bool autoSetupAllButtons = true;

    private AudioSource audioSource;
    private static GlobalButtonSoundManager instance;

    public static GlobalButtonSoundManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        // Singleton pattern
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // Setup audio source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.volume = defaultVolume;
        audioSource.pitch = defaultPitch;

        Debug.Log("[GlobalButtonSoundManager] Initialized");
    }

    private void Start()
    {
        if (autoSetupAllButtons)
        {
            SetupAllButtonsInScene();
        }
    }

    /// <summary>
    /// Setup semua button di scene dengan ButtonSoundEffect
    /// </summary>
    public void SetupAllButtonsInScene()
    {
        UnityEngine.UI.Button[] allButtons = FindObjectsOfType<UnityEngine.UI.Button>(true);
        int setupCount = 0;

        foreach (UnityEngine.UI.Button button in allButtons)
        {
            // Cek apakah button sudah punya ButtonSoundEffect
            ButtonSoundEffect existingEffect = button.GetComponent<ButtonSoundEffect>();

            if (existingEffect == null)
            {
                // Tambahkan component
                button.gameObject.AddComponent<ButtonSoundEffect>();
                setupCount++;
            }
        }

        Debug.Log($"[GlobalButtonSoundManager] Setup {setupCount} buttons dari total {allButtons.Length} buttons");
    }

    /// <summary>
    /// Play default click sound
    /// </summary>
    public void PlayDefaultClickSound()
    {
        if (defaultClickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(defaultClickSound, defaultVolume);
        }
    }

    /// <summary>
    /// Play custom sound dengan volume dan pitch
    /// </summary>
    public void PlaySound(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        if (clip != null && audioSource != null)
        {
            float previousPitch = audioSource.pitch;
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(clip, volume);
            audioSource.pitch = previousPitch; // Reset pitch
        }
    }

    /// <summary>
    /// Get default click sound
    /// </summary>
    public AudioClip GetDefaultClickSound()
    {
        return defaultClickSound;
    }

    /// <summary>
    /// Get audio source
    /// </summary>
    public AudioSource GetAudioSource()
    {
        return audioSource;
    }

    /// <summary>
    /// Set default click sound runtime
    /// </summary>
    public void SetDefaultClickSound(AudioClip newSound)
    {
        defaultClickSound = newSound;
    }

    /// <summary>
    /// Set default volume runtime
    /// </summary>
    public void SetDefaultVolume(float volume)
    {
        defaultVolume = Mathf.Clamp01(volume);
        if (audioSource != null)
        {
            audioSource.volume = defaultVolume;
        }
    }
}
