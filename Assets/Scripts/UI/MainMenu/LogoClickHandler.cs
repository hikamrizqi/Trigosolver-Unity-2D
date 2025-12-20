using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handler untuk logo click - shrink to corner
/// Attach ke logo GameObject di Main Menu
/// </summary>
[RequireComponent(typeof(MenuAnimationController))]
public class LogoClickHandler : MonoBehaviour, IPointerClickHandler
{
    [Header("References")]
    [Tooltip("Menu Animation Controller (auto-detect jika null)")]
    [SerializeField] private MenuAnimationController animationController;

    [Tooltip("Main Menu Manager (auto-detect jika null) - untuk disable click anywhere")]
    [SerializeField] private MainMenuManager mainMenuManager;

    [Header("Settings")]
    [Tooltip("Enable click functionality")]
    [SerializeField] private bool enableClick = true;

    [Tooltip("Delay sebelum logo bisa di-klik (detik) - cegah click tidak sengaja saat animasi drop")]
    [SerializeField] private float clickDelayAfterDrop = 1f;

    private bool canClick = false;
    private bool hasBeenClicked = false;

    private void Awake()
    {
        // Auto-detect MenuAnimationController
        if (animationController == null)
        {
            animationController = GetComponent<MenuAnimationController>();
        }

        if (animationController == null)
        {
            Debug.LogError("[LogoClickHandler] MenuAnimationController not found!");
            enabled = false;
        }

        // Auto-detect MainMenuManager
        if (mainMenuManager == null)
        {
            mainMenuManager = FindObjectOfType<MainMenuManager>();
        }

        if (mainMenuManager == null)
        {
            Debug.LogWarning("[LogoClickHandler] MainMenuManager not found - click anywhere won't be disabled");
        }
    }

    private void Start()
    {
        // Enable click setelah delay
        if (enableClick)
        {
            Invoke(nameof(EnableClick), clickDelayAfterDrop);
        }
    }

    /// <summary>
    /// Called by Unity Event System when pointer clicks
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!enableClick || !canClick || hasBeenClicked)
        {
            return;
        }

        OnLogoClicked();
    }

    /// <summary>
    /// Handle logo clicked - shrink to corner
    /// </summary>
    public void OnLogoClicked()
    {
        if (hasBeenClicked)
        {
            Debug.Log("[LogoClickHandler] Logo sudah pernah di-klik, skip");
            return;
        }

        Debug.Log("[LogoClickHandler] Logo clicked! Shrinking to corner...");

        hasBeenClicked = true;
        canClick = false;

        // CRITICAL: Disable "click anywhere" behavior IMMEDIATELY to prevent race condition
        if (mainMenuManager != null)
        {
            mainMenuManager.clickAnywhereEnabled = false;
            Debug.Log("[LogoClickHandler] Disabled click anywhere to prevent AnimateSinkOut");
        }

        // Trigger shrink animation
        if (animationController != null)
        {
            animationController.AnimateShrinkToCorner(() =>
            {
                Debug.Log("[LogoClickHandler] Logo sekarang di corner - bisa di-klik lagi untuk restore");
                // Bisa enable click lagi jika mau restore functionality
                canClick = false; // Set true jika mau bisa restore
            });
        }
    }

    /// <summary>
    /// Enable click functionality
    /// </summary>
    private void EnableClick()
    {
        if (enableClick)
        {
            canClick = true;
            Debug.Log("[LogoClickHandler] Logo click enabled");
        }
    }

    /// <summary>
    /// Reset state (untuk testing)
    /// </summary>
    public void ResetState()
    {
        hasBeenClicked = false;
        canClick = enableClick;

        if (animationController != null)
        {
            animationController.RestoreFromCorner();
        }
    }

    /// <summary>
    /// Manually trigger shrink (untuk button/event)
    /// </summary>
    public void TriggerShrink()
    {
        OnLogoClicked();
    }

#if UNITY_EDITOR
    // For testing in editor without event system
    private void Update()
    {
        // Alt+Click untuk test di editor
        if (Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKey(KeyCode.LeftAlt))
        {
            Debug.Log("[LogoClickHandler] Debug click triggered!");
            OnLogoClicked();
        }
    }
#endif
}
