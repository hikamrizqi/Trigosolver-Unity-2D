using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Manager untuk mengatur state dan transisi antar panel main menu
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    [Tooltip("Panel Logo Trigosolver")]
    public GameObject logoPanel;

    [Tooltip("Panel Main Menu (Mulai, Keluar)")]
    public GameObject mainMenuPanel;

    [Tooltip("Panel Mode Selection (Mode Cerita, Mode Bebas, dll)")]
    public GameObject modeSelectionPanel;

    [Tooltip("Panel Mode Cerita Selection (Chapter selection)")]
    public GameObject modeCeritaSelectionPanel;

    [Header("Animation Controllers")]
    private MenuAnimationController logoAnimator;
    private MenuAnimationController mainMenuAnimator;
    private MenuAnimationController modeSelectionAnimator;
    private MenuAnimationController modeCeritaAnimator;

    [Header("Click to Continue")]
    [Tooltip("Aktifkan click anywhere untuk logo panel")]
    public bool clickAnywhereEnabled = false;

    private enum MenuState
    {
        Logo,
        MainMenu,
        ModeSelection,
        ModeCeritaSelection
    }

    private MenuState currentState;

    private void Awake()
    {
        // Hide semua panel kecuali logo SEBELUM frame pertama
        mainMenuPanel.SetActive(false);
        modeSelectionPanel.SetActive(false);
        modeCeritaSelectionPanel.SetActive(false);
    }

    private void Start()
    {
        // Get animation controllers dari masing-masing panel
        logoAnimator = logoPanel.GetComponent<MenuAnimationController>();
        mainMenuAnimator = mainMenuPanel.GetComponent<MenuAnimationController>();
        modeSelectionAnimator = modeSelectionPanel.GetComponent<MenuAnimationController>();
        modeCeritaAnimator = modeCeritaSelectionPanel.GetComponent<MenuAnimationController>();

        // Validasi
        if (logoAnimator == null) Debug.LogError("Logo panel tidak memiliki MenuAnimationController!");
        if (mainMenuAnimator == null) Debug.LogError("Main Menu panel tidak memiliki MenuAnimationController!");
        if (modeSelectionAnimator == null) Debug.LogError("Mode Selection panel tidak memiliki MenuAnimationController!");
        if (modeCeritaAnimator == null) Debug.LogError("Mode Cerita Selection panel tidak memiliki MenuAnimationController!");

        // Mulai dari logo
        currentState = MenuState.Logo;
        ShowLogo();
    }

    private void Update()
    {
        // Click anywhere untuk logo panel
        if (currentState == MenuState.Logo && clickAnywhereEnabled)
        {
            if (Input.GetMouseButtonDown(0) || Input.anyKeyDown)
            {
                TransitionToMainMenu();
            }
        }
    }

    /// <summary>
    /// Tampilkan logo dengan animasi drop
    /// </summary>
    private void ShowLogo()
    {
        logoAnimator.AnimateDropIn(() =>
        {
            // Setelah animasi selesai, enable click anywhere
            clickAnywhereEnabled = true;
        });
    }

    /// <summary>
    /// Transisi dari Logo ke Main Menu
    /// </summary>
    public void TransitionToMainMenu()
    {
        if (currentState != MenuState.Logo) return;

        Debug.Log("TransitionToMainMenu dipanggil");

        clickAnywhereEnabled = false;
        currentState = MenuState.MainMenu;

        // Null check
        if (mainMenuAnimator == null)
        {
            Debug.LogError("mainMenuAnimator NULL! Pastikan MainMenu panel memiliki MenuAnimationController!");
            return;
        }

        // Logo sink, lalu main menu drop
        logoAnimator.AnimateSinkOut(() =>
        {
            Debug.Log("Logo sink selesai, panggil main menu drop");
            mainMenuAnimator.AnimateDropIn();
        });
    }

    /// <summary>
    /// Transisi dari Main Menu ke Mode Selection
    /// </summary>
    public void OnMulaiClicked()
    {
        if (currentState != MenuState.MainMenu) return;

        currentState = MenuState.ModeSelection;

        mainMenuAnimator.AnimateSinkOut(() =>
        {
            modeSelectionAnimator.AnimateDropIn();
        });
    }

    /// <summary>
    /// Transisi dari Mode Selection ke Mode Cerita Selection
    /// </summary>
    public void OnModeCeritaClicked()
    {
        if (currentState != MenuState.ModeSelection) return;

        currentState = MenuState.ModeCeritaSelection;

        modeSelectionAnimator.AnimateSinkOut(() =>
        {
            modeCeritaAnimator.AnimateDropIn();
        });
    }

    /// <summary>
    /// Back dari Mode Cerita Selection ke Mode Selection
    /// </summary>
    public void OnBackFromModeCerita()
    {
        if (currentState != MenuState.ModeCeritaSelection) return;

        currentState = MenuState.ModeSelection;

        modeCeritaAnimator.AnimateSinkOut(() =>
        {
            modeSelectionAnimator.AnimateDropIn();
        });
    }

    /// <summary>
    /// Back dari Mode Selection ke Main Menu
    /// </summary>
    public void OnBackFromModeSelection()
    {
        if (currentState != MenuState.ModeSelection) return;

        currentState = MenuState.MainMenu;

        modeSelectionAnimator.AnimateSinkOut(() =>
        {
            mainMenuAnimator.AnimateDropIn();
        });
    }

    /// <summary>
    /// Keluar dari game
    /// </summary>
    public void OnKeluarClicked()
    {
        Debug.Log("Keluar dari game");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    /// <summary>
    /// Load scene tertentu (untuk mode cerita/bebas)
    /// </summary>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
