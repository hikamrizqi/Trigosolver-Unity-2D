using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Component helper untuk auto-play SFX saat button di-click
/// Attach ke button yang ingin memiliki click sound
/// </summary>
[RequireComponent(typeof(Button))]
public class ButtonClickSFX : MonoBehaviour, IPointerClickHandler
{
    [Header("Audio Settings")]
    [Tooltip("Audio clip untuk button click (optional, akan use dari GlobalAudioManager jika null)")]
    [SerializeField] private AudioClip customClickSFX;

    [Tooltip("Volume untuk SFX (0-1)")]
    [SerializeField][Range(0f, 1f)] private float volume = 1f;

    [Header("Audio Source")]
    [Tooltip("Use global audio manager untuk SFX")]
    [SerializeField] private bool useGlobalAudioManager = true;

    [Tooltip("Use Chapter1 audio manager untuk SFX (jika dalam Chapter 1)")]
    [SerializeField] private bool useChapter1AudioManager = false;

    private Button button;
    private AudioSource localAudioSource;

    void Awake()
    {
        button = GetComponent<Button>();

        // Create local audio source if needed
        if (!useGlobalAudioManager && !useChapter1AudioManager)
        {
            localAudioSource = gameObject.AddComponent<AudioSource>();
            localAudioSource.playOnAwake = false;
            localAudioSource.volume = volume;
        }
    }

    /// <summary>
    /// Called when button is clicked (IPointerClickHandler interface)
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        // Only play if button is interactable
        if (button != null && button.interactable)
        {
            PlayClickSound();
        }
    }

    /// <summary>
    /// Alternative: Call this manually from button onClick event
    /// </summary>
    public void PlayClickSound()
    {
        Debug.Log($"[ButtonClickSFX] PlayClickSound called on {gameObject.name}");

        // Priority 1: Use GlobalAudioManager
        if (useGlobalAudioManager && GlobalAudioManager.Instance != null)
        {
            Debug.Log($"[ButtonClickSFX] Using GlobalAudioManager");
            if (customClickSFX != null)
            {
                GlobalAudioManager.Instance.PlaySFX(customClickSFX);
            }
            else
            {
                GlobalAudioManager.Instance.PlayButtonClickSFX();
            }
        }
        // Priority 2: Use Chapter1AudioManager
        else if (useChapter1AudioManager && Chapter1AudioManager.Instance != null)
        {
            Debug.Log($"[ButtonClickSFX] Using Chapter1AudioManager");
            Chapter1AudioManager.Instance.PlayButtonClickSFX();
        }
        // Priority 3: Use local audio source
        else if (localAudioSource != null && customClickSFX != null)
        {
            Debug.Log($"[ButtonClickSFX] Using local AudioSource");
            localAudioSource.PlayOneShot(customClickSFX, volume);
        }
        else
        {
            Debug.LogWarning($"[ButtonClickSFX] No audio manager or clip available on {gameObject.name}");
            Debug.LogWarning($"  - GlobalAudioManager.Instance: {GlobalAudioManager.Instance}");
            Debug.LogWarning($"  - Chapter1AudioManager.Instance: {Chapter1AudioManager.Instance}");
            Debug.LogWarning($"  - useGlobalAudioManager: {useGlobalAudioManager}");
            Debug.LogWarning($"  - useChapter1AudioManager: {useChapter1AudioManager}");
        }
    }

    /// <summary>
    /// Set custom click SFX at runtime
    /// </summary>
    public void SetCustomClickSFX(AudioClip clip)
    {
        customClickSFX = clip;
    }

    /// <summary>
    /// Set volume at runtime
    /// </summary>
    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        if (localAudioSource != null)
        {
            localAudioSource.volume = volume;
        }
    }
}
