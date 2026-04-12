using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// Game Over panel - shows when lives run out
/// </summary>
public class GameOverPanel : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI messageText;

    [Header("Settings")]
    [SerializeField] private float displayDuration = 3f;

    [Header("Manager References")]
    [SerializeField] private LevelSelectionManager levelSelectionManager;
    [SerializeField] private Chapter1AudioManager audioManager; // Reference to audio manager

    private CharacterAnimationController characterController; // Reference to hide character on back

    private void Start()
    {
        // Hide panel on start
        if (panel != null)
            panel.SetActive(false);
    }

    /// <summary>
    /// Show game over panel with score
    /// </summary>
    public void ShowGameOver(int finalScore, CharacterAnimationController charController = null)
    {
        Debug.Log($"[GameOver] Showing game over panel - Score: {finalScore}");

        // Store character controller reference
        characterController = charController;

        if (panel != null)
            panel.SetActive(true);

        if (scoreText != null)
            scoreText.text = $"Skor Akhir: {finalScore}";

        if (messageText != null)
            messageText.text = "PERMAINAN BERAKHIR\nNyawa Habis!";

        // Save score to highscore
        HighScoreManager.Instance.SaveScore(finalScore);

        // Auto return to level selection after delay
        StartCoroutine(ReturnToLevelSelectionAfterDelay());
    }

    private IEnumerator ReturnToLevelSelectionAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);

        Debug.Log("[GameOver] Returning to level selection");

        // Hide character animation first if exists
        if (characterController != null)
        {
            characterController.HideCharacter();
            // Wait for hide animation to complete
            yield return new WaitForSeconds(1.0f);
        }

        // Resume BGM
        if (audioManager != null)
        {
            audioManager.ResumeBGMAfterGameOver();
        }

        // Hide panel
        if (panel != null)
            panel.SetActive(false);

        // Return to level selection
        if (levelSelectionManager != null)
        {
            levelSelectionManager.ShowLevelSelection();
        }
        else
        {
            Debug.LogError("[GameOver] LevelSelectionManager reference missing!");
        }
    }

    /// <summary>
    /// Method untuk dipanggil dari tombol kembali (jika ada tombol manual)
    /// </summary>
    public void OnBackButtonClicked()
    {
        Debug.Log("[GameOver] Back button clicked - hiding character and returning");

        // Stop auto return coroutine
        StopAllCoroutines();

        // Hide character first, then return
        StartCoroutine(HideCharacterAndReturn());
    }

    private IEnumerator HideCharacterAndReturn()
    {
        // Hide character animation if exists
        if (characterController != null)
        {
            characterController.HideCharacter();
            // Wait for hide animation to complete
            yield return new WaitForSeconds(1.0f);
        }

        // Resume BGM
        if (audioManager != null)
        {
            audioManager.ResumeBGMAfterGameOver();
        }

        // Hide panel
        if (panel != null)
            panel.SetActive(false);

        // Return to level selection
        if (levelSelectionManager != null)
        {
            levelSelectionManager.ShowLevelSelection();
        }
        else
        {
            Debug.LogError("[GameOver] LevelSelectionManager reference missing!");
        }
    }
}
