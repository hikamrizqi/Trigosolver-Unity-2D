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

    private void Start()
    {
        // Hide panel on start
        if (panel != null)
            panel.SetActive(false);
    }

    /// <summary>
    /// Show game over panel with score
    /// </summary>
    public void ShowGameOver(int finalScore)
    {
        Debug.Log($"[GameOver] Showing game over panel - Score: {finalScore}");

        if (panel != null)
            panel.SetActive(true);

        if (scoreText != null)
            scoreText.text = $"Skor Akhir: {finalScore}";

        if (messageText != null)
            messageText.text = "PERMAINAN BERAKHIR\nNyawa Habis!";

        // Save score to highscore
        HighscoreManager.Instance.SaveScore(finalScore);

        // Auto return to level selection after delay
        StartCoroutine(ReturnToLevelSelectionAfterDelay());
    }

    private IEnumerator ReturnToLevelSelectionAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);

        Debug.Log("[GameOver] Returning to level selection");

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
