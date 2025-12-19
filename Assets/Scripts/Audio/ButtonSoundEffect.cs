using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Component untuk play sound effect saat button diklik
/// Attach ke Button GameObject atau gunakan GlobalButtonSoundManager
/// </summary>
[RequireComponent(typeof(Button))]
public class ButtonSoundEffect : MonoBehaviour, IPointerClickHandler
{
    [Header("Sound Settings")]
    [Tooltip("Audio clip untuk button click (opsional, jika null akan gunakan global sound)")]
    [SerializeField] private AudioClip clickSound;

    [Tooltip("Volume sound (0-1)")]
    [SerializeField][Range(0f, 1f)] private float volume = 1f;

    [Tooltip("Pitch sound (0.5-2.0, default 1.0)")]
    [SerializeField][Range(0.5f, 2f)] private float pitch = 1f;

    [Header("Audio Source")]
    [Tooltip("Audio source untuk play sound (opsional, akan gunakan global jika null)")]
    [SerializeField] private AudioSource audioSource;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        // Jika tidak ada audio source, coba cari atau buat
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();

            if (audioSource == null)
            {
                // Coba gunakan global button sound manager
                GlobalButtonSoundManager globalManager = FindObjectOfType<GlobalButtonSoundManager>();
                if (globalManager != null)
                {
                    audioSource = globalManager.GetAudioSource();
                }
            }
        }
    }

    private void Start()
    {
        // Subscribe ke button onClick event
        if (button != null)
        {
            button.onClick.AddListener(PlayClickSound);
        }
    }

    /// <summary>
    /// Play click sound
    /// </summary>
    public void PlayClickSound()
    {
        // Cek apakah ada audio clip
        AudioClip soundToPlay = clickSound;

        // Jika tidak ada clip local, coba ambil dari global manager
        if (soundToPlay == null)
        {
            GlobalButtonSoundManager globalManager = FindObjectOfType<GlobalButtonSoundManager>();
            if (globalManager != null)
            {
                soundToPlay = globalManager.GetDefaultClickSound();
            }
        }

        // Play sound
        if (soundToPlay != null && audioSource != null)
        {
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(soundToPlay, volume);
        }
        else if (soundToPlay == null)
        {
            Debug.LogWarning($"[ButtonSoundEffect] {gameObject.name}: Tidak ada audio clip untuk button sound!");
        }
        else if (audioSource == null)
        {
            Debug.LogWarning($"[ButtonSoundEffect] {gameObject.name}: Tidak ada audio source!");
        }
    }

    /// <summary>
    /// Interface IPointerClickHandler - dipanggil saat pointer click
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        // Backup method jika onClick tidak terpanggil
        if (button != null && button.interactable)
        {
            // onClick sudah handle, jadi skip
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe
        if (button != null)
        {
            button.onClick.RemoveListener(PlayClickSound);
        }
    }
}
