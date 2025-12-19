using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Enum untuk state UI
    public enum UIState
    {
        MainMenu,
        ModeSelection,
        StoryMode
    }

    [Header("UI Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject modeSelectionPanel;
    [SerializeField] private GameObject storyModePanel;

    [Header("Settings")]
    [SerializeField] private UIState initialState = UIState.MainMenu;

    private UIState currentState;

    private void Start()
    {
        // Set initial state saat game dimulai
        ShowPanel(initialState);
    }

    // Method utama untuk mengatur state UI
    private void ShowPanel(UIState newState)
    {
        // Nonaktifkan semua panel terlebih dahulu
        HideAllPanels();

        // Aktifkan panel sesuai state
        switch (newState)
        {
            case UIState.MainMenu:
                mainMenuPanel.SetActive(true);
                break;

            case UIState.ModeSelection:
                modeSelectionPanel.SetActive(true);
                break;

            case UIState.StoryMode:
                storyModePanel.SetActive(true);
                break;
        }

        currentState = newState;
    }

    // Helper method untuk hide semua panel
    private void HideAllPanels()
    {
        mainMenuPanel.SetActive(false);
        modeSelectionPanel.SetActive(false);
        storyModePanel.SetActive(false);
    }

    // Public methods untuk button onClick events
    public void OnClickMulai()
    {
        ShowPanel(UIState.ModeSelection);
    }

    public void OnClickKembaliKeMenu()
    {
        ShowPanel(UIState.MainMenu);
    }

    public void OnClickStoryMode()
    {
        ShowPanel(UIState.StoryMode);
    }

    public void OnClickKembaliKeModeSelection()
    {
        ShowPanel(UIState.ModeSelection);
    }

    // Method untuk load scene Stage 1 (Bagian 1 - Observasi Segitiga)
    public void OnClickBagian1()
    {
        Debug.Log("[UIManager] Loading Chapter 1 - Stage 1");
        SceneManager.LoadScene("Stage 1");
    }

    // Method untuk load scene Stage 2 (Bagian 2 - Tembakan Meriam)
    public void OnClickBagian2()
    {
        Debug.Log("[UIManager] Loading Chapter 2 - Stage 2");
        SceneManager.LoadScene("Stage 2");
    }

    // Optional: Method untuk get current state (untuk debugging atau keperluan lain)
    public UIState GetCurrentState()
    {
        return currentState;
    }
}
