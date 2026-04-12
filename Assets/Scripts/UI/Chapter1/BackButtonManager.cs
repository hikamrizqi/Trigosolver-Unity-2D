using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages single back button behavior based on current game state
/// </summary>
public class BackButtonManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button backButton;
    [SerializeField] private CalculationManager calculationManager;
    [SerializeField] private LevelSelectionManager levelSelectionManager;

    [Header("State")]
    private bool isInLevelSelection = true; // Track current state

    private void Start()
    {
        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackButtonClicked);
        }
        else
        {
            Debug.LogError("[BackButtonManager] Back button reference missing!");
        }

        // Default: Show button, set to level selection mode
        SetLevelSelectionMode();
    }

    /// <summary>
    /// Called when back button is clicked
    /// </summary>
    private void OnBackButtonClicked()
    {
        if (isInLevelSelection)
        {
            // Currently in level selection → Go back to Main Menu
            BackToMainMenu();
        }
        else
        {
            // Currently in game → Go back to level selection
            BackToLevelSelection();
        }
    }

    /// <summary>
    /// Set button to level selection mode (back to main menu)
    /// </summary>
    public void SetLevelSelectionMode()
    {
        isInLevelSelection = true;
        Debug.Log("[BackButton] Mode: Level Selection → Main Menu");
    }

    /// <summary>
    /// Set button to gameplay mode (back to level selection)
    /// </summary>
    public void SetGameplayMode()
    {
        isInLevelSelection = false;
        Debug.Log("[BackButton] Mode: Gameplay → Level Selection");
    }

    /// <summary>
    /// Show back button
    /// </summary>
    public void Show()
    {
        if (backButton != null)
            backButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// Hide back button
    /// </summary>
    public void Hide()
    {
        if (backButton != null)
            backButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// Back to main menu (from level selection)
    /// </summary>
    private void BackToMainMenu()
    {
        Debug.Log("[BackButton] Returning to Main Menu");

        if (levelSelectionManager != null)
        {
            levelSelectionManager.BackToMainMenu();
        }
        else
        {
            Debug.LogError("[BackButton] LevelSelectionManager reference missing!");
        }
    }

    /// <summary>
    /// Back to level selection (from gameplay)
    /// </summary>
    private void BackToLevelSelection()
    {
        Debug.Log("[BackButton] Returning to Level Selection");

        if (calculationManager != null)
        {
            calculationManager.BackToLevelSelection();
        }
        else
        {
            Debug.LogError("[BackButton] CalculationManager reference missing!");
        }
    }
}
