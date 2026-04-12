using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// Controller untuk menampilkan gambar materi
/// Navigasi: Gambar 1 → Gambar 2 → Kembali ke Main Menu
/// </summary>
public class MaterialDisplayController : MonoBehaviour
{
    [Header("Material Images")]
    [Tooltip("Gambar materi pertama")]
    [SerializeField] private GameObject materialImage1;

    [Tooltip("Gambar materi kedua")]
    [SerializeField] private GameObject materialImage2;

    [Header("Navigation Settings")]
    [SerializeField] private bool enableClickAnywhere = true;
    [SerializeField] private KeyCode nextKey = KeyCode.Space;
    [SerializeField] private KeyCode backKey = KeyCode.Escape;

    [Header("Animation Settings")]
    [SerializeField] private float fadeDuration = 0.3f;
    [SerializeField] private float scaleDuration = 0.3f;

    [Header("UI References")]
    [SerializeField] private Image image1Component;
    [SerializeField] private Image image2Component;

    private int currentImageIndex = 0; // 0 = image1, 1 = image2, 2 = close
    private bool isTransitioning = false;

    // Event untuk notify main menu manager
    public System.Action OnMaterialClosed;

    private void Start()
    {
        // Setup initial state
        if (materialImage1 == null || materialImage2 == null)
        {
            Debug.LogError("[MaterialDisplay] Material images not assigned!");
            return;
        }

        // Get Image components if not assigned
        if (image1Component == null && materialImage1 != null)
            image1Component = materialImage1.GetComponent<Image>();

        if (image2Component == null && materialImage2 != null)
            image2Component = materialImage2.GetComponent<Image>();

        // Hide both images initially
        materialImage1.SetActive(false);
        materialImage2.SetActive(false);
    }

    private void Update()
    {
        if (isTransitioning) return;

        // Click anywhere atau tekan space untuk next
        if (enableClickAnywhere && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(nextKey)))
        {
            OnNextClicked();
        }

        // ESC untuk back (optional)
        if (Input.GetKeyDown(backKey))
        {
            OnBackClicked();
        }
    }

    /// <summary>
    /// Show material display, mulai dari gambar pertama
    /// </summary>
    public void ShowMaterial()
    {
        Debug.Log("[MaterialDisplay] ShowMaterial called");

        currentImageIndex = 0;
        ShowImage1();
    }

    /// <summary>
    /// Handle next button click atau click anywhere
    /// </summary>
    public void OnNextClicked()
    {
        if (isTransitioning) return;

        Debug.Log($"[MaterialDisplay] OnNextClicked - Current index: {currentImageIndex}");

        switch (currentImageIndex)
        {
            case 0: // Gambar 1 → Gambar 2
                TransitionToImage2();
                break;
            case 1: // Gambar 2 → Close
                CloseMaterial();
                break;
        }
    }

    /// <summary>
    /// Handle back button click
    /// </summary>
    public void OnBackClicked()
    {
        if (isTransitioning) return;

        Debug.Log($"[MaterialDisplay] OnBackClicked - Current index: {currentImageIndex}");

        switch (currentImageIndex)
        {
            case 0: // Gambar 1 → Close
                CloseMaterial();
                break;
            case 1: // Gambar 2 → Gambar 1
                TransitionToImage1();
                break;
        }
    }

    /// <summary>
    /// Show gambar pertama dengan animasi
    /// </summary>
    private void ShowImage1()
    {
        Debug.Log("[MaterialDisplay] ShowImage1");

        materialImage1.SetActive(true);
        materialImage2.SetActive(false);

        // Animate fade in
        if (image1Component != null)
        {
            image1Component.color = new Color(1, 1, 1, 0);
            image1Component.DOFade(1f, fadeDuration);
        }

        // Animate scale
        materialImage1.transform.localScale = Vector3.zero;
        materialImage1.transform.DOScale(1f, scaleDuration).SetEase(Ease.OutBack);

        currentImageIndex = 0;
    }

    /// <summary>
    /// Transisi dari gambar 1 ke gambar 2
    /// </summary>
    private void TransitionToImage2()
    {
        Debug.Log("[MaterialDisplay] TransitionToImage2");

        isTransitioning = true;

        // Fade out + scale down gambar 1
        if (image1Component != null)
        {
            image1Component.DOFade(0f, fadeDuration);
        }
        materialImage1.transform.DOScale(0.8f, fadeDuration).OnComplete(() =>
        {
            materialImage1.SetActive(false);

            // Show gambar 2
            materialImage2.SetActive(true);

            if (image2Component != null)
            {
                image2Component.color = new Color(1, 1, 1, 0);
                image2Component.DOFade(1f, fadeDuration);
            }

            materialImage2.transform.localScale = Vector3.zero;
            materialImage2.transform.DOScale(1f, scaleDuration).SetEase(Ease.OutBack).OnComplete(() =>
            {
                currentImageIndex = 1;
                isTransitioning = false;
            });
        });
    }

    /// <summary>
    /// Transisi dari gambar 2 ke gambar 1 (back)
    /// </summary>
    private void TransitionToImage1()
    {
        Debug.Log("[MaterialDisplay] TransitionToImage1");

        isTransitioning = true;

        // Fade out + scale down gambar 2
        if (image2Component != null)
        {
            image2Component.DOFade(0f, fadeDuration);
        }
        materialImage2.transform.DOScale(0.8f, fadeDuration).OnComplete(() =>
        {
            materialImage2.SetActive(false);

            // Show gambar 1
            materialImage1.SetActive(true);

            if (image1Component != null)
            {
                image1Component.color = new Color(1, 1, 1, 0);
                image1Component.DOFade(1f, fadeDuration);
            }

            materialImage1.transform.localScale = Vector3.zero;
            materialImage1.transform.DOScale(1f, scaleDuration).SetEase(Ease.OutBack).OnComplete(() =>
            {
                currentImageIndex = 0;
                isTransitioning = false;
            });
        });
    }

    /// <summary>
    /// Close material display dan kembali ke main menu
    /// </summary>
    private void CloseMaterial()
    {
        Debug.Log("[MaterialDisplay] CloseMaterial");

        isTransitioning = true;

        GameObject activeImage = currentImageIndex == 0 ? materialImage1 : materialImage2;
        Image activeImageComponent = currentImageIndex == 0 ? image1Component : image2Component;

        // Fade out + scale down
        if (activeImageComponent != null)
        {
            activeImageComponent.DOFade(0f, fadeDuration);
        }

        activeImage.transform.DOScale(0f, scaleDuration).SetEase(Ease.InBack).OnComplete(() =>
        {
            materialImage1.SetActive(false);
            materialImage2.SetActive(false);

            // Notify main menu manager
            OnMaterialClosed?.Invoke();

            isTransitioning = false;
            currentImageIndex = 0;

            Debug.Log("[MaterialDisplay] Material closed, notify main menu");
        });
    }

    /// <summary>
    /// Force close tanpa animasi (untuk cleanup)
    /// </summary>
    public void ForceClose()
    {
        DOTween.Kill(materialImage1);
        DOTween.Kill(materialImage2);

        materialImage1.SetActive(false);
        materialImage2.SetActive(false);

        currentImageIndex = 0;
        isTransitioning = false;
    }

    /// <summary>
    /// Check apakah sedang menampilkan materi
    /// </summary>
    public bool IsShowing()
    {
        return materialImage1.activeSelf || materialImage2.activeSelf;
    }

    /// <summary>
    /// Get current image index (untuk debug)
    /// </summary>
    public int GetCurrentImageIndex()
    {
        return currentImageIndex;
    }
}
