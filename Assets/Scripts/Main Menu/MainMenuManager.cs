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
    public GameObject highScorePanel;

    [Header("High Score Display")]
    [Tooltip("High Score Display component (optional)")]
    public HighScoreDisplay highScoreDisplay;

    [Header("Animation Controllers")]
    private MenuAnimationController logoAnimator;
    private MenuAnimationController mainMenuAnimator;
    private MenuAnimationController modeSelectionAnimator;
    private MenuAnimationController modeCeritaAnimator;
    private MenuAnimationController highScoreAnimator;

    [Header("Click to Continue")]
    [Tooltip("Aktifkan click anywhere untuk logo panel")]
    public bool clickAnywhereEnabled = false;

    private enum MenuState
    {
        Logo,
        MainMenu,
        ModeSelection,
        ModeCeritaSelection,
        HighScore
    }

    private MenuState currentState;

    private void Awake()
    {
        // Hide semua panel kecuali logo SEBELUM frame pertama
        mainMenuPanel.SetActive(false);
        modeSelectionPanel.SetActive(false);
        modeCeritaSelectionPanel.SetActive(false);
        highScorePanel.SetActive(false);
    }

    private void Start()
    {
        // Get animation controllers dari masing-masing panel
        logoAnimator = logoPanel.GetComponent<MenuAnimationController>();
        mainMenuAnimator = mainMenuPanel.GetComponent<MenuAnimationController>();
        modeSelectionAnimator = modeSelectionPanel.GetComponent<MenuAnimationController>();
        modeCeritaAnimator = modeCeritaSelectionPanel.GetComponent<MenuAnimationController>();
        highScoreAnimator = highScorePanel.GetComponent<MenuAnimationController>();

        // Validasi
        if (logoAnimator == null) Debug.LogError("Logo panel tidak memiliki MenuAnimationController!");
        if (mainMenuAnimator == null) Debug.LogError("Main Menu panel tidak memiliki MenuAnimationController!");
        if (modeSelectionAnimator == null) Debug.LogError("Mode Selection panel tidak memiliki MenuAnimationController!");
        if (modeCeritaAnimator == null) Debug.LogError("Mode Cerita Selection panel tidak memiliki MenuAnimationController!");
        if (highScoreAnimator == null) Debug.LogWarning("High Score panel tidak memiliki MenuAnimationController!");
        // Mulai dari logo
        currentState = MenuState.Logo;
        ShowLogo();
    }

    private void Update()
    {
        // Click anywhere untuk logo panel
        // SKIP jika logo sudah di corner atau sedang animasi ke corner
        if (currentState == MenuState.Logo && clickAnywhereEnabled)
        {
            // Check: Jika logo sudah/sedang di corner, JANGAN detect click
            if (logoAnimator != null && logoAnimator.IsInCorner())
            {
                Debug.Log("[MainMenuManager] Update: Logo di corner, skip click detection");
                return;
            }

            if (Input.GetMouseButtonDown(0) || Input.anyKeyDown)
            {
                Debug.Log("[MainMenuManager] Update: Input detected, calling TransitionToMainMenu");
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

        // CEK: Jika logo sudah di corner, langsung show main menu (jangan animasi logo!)
        if (logoAnimator != null && logoAnimator.IsInCorner())
        {
            Debug.Log("Logo sudah di corner, langsung show main menu tanpa animasi logo");
            if (mainMenuPanel != null)
            {
                mainMenuPanel.SetActive(true);
            }
            mainMenuAnimator.AnimateDropIn();
            return;
        }

        // PENTING: Logo shrink ke corner (BUKAN sink out!)
        // Ini adalah transisi pertama dari logo penuh ke logo pojok
        logoAnimator.AnimateShrinkToCorner(() =>
        {
            Debug.Log("Logo shrink to corner selesai, show main menu");

            if (mainMenuPanel != null)
            {
                mainMenuPanel.SetActive(true);
            }

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
    /// Transisi dari Main Menu ke High Score Panel
    /// </summary>
    public void OnHighScoreClicked()
    {
        if (currentState != MenuState.MainMenu) return;

        Debug.Log("[MainMenu] OnHighScoreClicked called");

        currentState = MenuState.HighScore;

        // Main menu sink, lalu high score drop
        mainMenuAnimator.AnimateSinkOut(() =>
        {
            Debug.Log("[MainMenu] Main menu sink complete, showing high score panel");

            // PENTING: Aktifkan panel sebelum animasi!
            if (highScorePanel != null)
            {
                highScorePanel.SetActive(true);
            }
            else
            {
                Debug.LogError("[MainMenu] highScorePanel is NULL!");
                return;
            }

            if (highScoreAnimator != null)
            {
                highScoreAnimator.AnimateDropIn(() =>
                {
                    Debug.Log("[MainMenu] High score panel animation complete");
                    // Refresh high score display setelah panel muncul
                    if (highScoreDisplay != null)
                    {
                        highScoreDisplay.RefreshScores();
                    }
                });
            }
            else
            {
                Debug.LogError("[MainMenu] highScoreAnimator is NULL!");
            }
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
    /// Back dari High Score Panel ke Main Menu
    /// </summary>
    public void OnBackFromHighScore()
    {
        if (currentState != MenuState.HighScore) return;

        Debug.Log("[MainMenu] OnBackFromHighScore called");

        currentState = MenuState.MainMenu;

        // High score sink, lalu main menu drop
        highScoreAnimator.AnimateSinkOut(() =>
        {
            Debug.Log("[MainMenu] High score sink complete, showing main menu");

            // Aktifkan main menu panel
            if (mainMenuPanel != null)
            {
                mainMenuPanel.SetActive(true);
            }

            if (mainMenuAnimator != null)
            {
                mainMenuAnimator.AnimateDropIn();
            }
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
