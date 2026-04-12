using UnityEngine;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Highscore display UI for Main Menu
/// Shows TOP 10 and RECENT 3 scores
/// </summary>
public class HighscoreUI : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject highscorePanel;

    [Header("Top 10 Section")]
    [SerializeField] private Transform top10Container;
    [SerializeField] private GameObject scoreEntryPrefab; // Prefab dengan TextMeshPro untuk 1 entry

    [Header("Recent 3 Section")]
    [SerializeField] private Transform recent3Container;

    private void Start()
    {
        // Hide panel on start (akan ditampilkan lewat tombol)
        if (highscorePanel != null)
            highscorePanel.SetActive(false);
    }

    /// <summary>
    /// Show highscore panel with updated data
    /// </summary>
    public void ShowHighscorePanel()
    {
        if (highscorePanel != null)
            highscorePanel.SetActive(true);

        RefreshHighscores();
    }

    /// <summary>
    /// Hide highscore panel
    /// </summary>
    public void HideHighscorePanel()
    {
        if (highscorePanel != null)
            highscorePanel.SetActive(false);
    }

    /// <summary>
    /// Refresh highscore display
    /// </summary>
    public void RefreshHighscores()
    {
        DisplayTop10();
        DisplayRecent3();
    }

    /// <summary>
    /// Display top 10 highest scores
    /// </summary>
    private void DisplayTop10()
    {
        if (top10Container == null || scoreEntryPrefab == null)
        {
            Debug.LogWarning("[HighscoreUI] Top10 container or prefab not set");
            return;
        }

        // Clear existing entries
        foreach (Transform child in top10Container)
        {
            Destroy(child.gameObject);
        }

        // Get top 10 scores
        List<HighScoreManager.HighscoreEntry> top10 = HighScoreManager.Instance.GetTop10();

        if (top10.Count == 0)
        {
            // No scores yet
            GameObject emptyEntry = Instantiate(scoreEntryPrefab, top10Container);
            TextMeshProUGUI text = emptyEntry.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
                text.text = "Belum ada highscore";
            return;
        }

        // Display each entry
        for (int i = 0; i < top10.Count; i++)
        {
            GameObject entry = Instantiate(scoreEntryPrefab, top10Container);
            TextMeshProUGUI text = entry.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                int rank = i + 1;
                HighScoreManager.HighscoreEntry data = top10[i];
                text.text = $"{rank}. Score: {data.score}  |  {data.date}, {data.time}";
            }
        }

        Debug.Log($"[HighscoreUI] Displayed {top10.Count} top scores");
    }

    /// <summary>
    /// Display 3 most recent scores
    /// </summary>
    private void DisplayRecent3()
    {
        if (recent3Container == null || scoreEntryPrefab == null)
        {
            Debug.LogWarning("[HighscoreUI] Recent3 container or prefab not set");
            return;
        }

        // Clear existing entries
        foreach (Transform child in recent3Container)
        {
            Destroy(child.gameObject);
        }

        // Get recent 3 scores
        List<HighScoreManager.HighscoreEntry> recent3 = HighScoreManager.Instance.GetRecent3();

        if (recent3.Count == 0)
        {
            // No scores yet
            GameObject emptyEntry = Instantiate(scoreEntryPrefab, recent3Container);
            TextMeshProUGUI text = emptyEntry.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
                text.text = "Belum ada riwayat";
            return;
        }

        // Display each entry
        for (int i = 0; i < recent3.Count; i++)
        {
            GameObject entry = Instantiate(scoreEntryPrefab, recent3Container);
            TextMeshProUGUI text = entry.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                HighScoreManager.HighscoreEntry data = recent3[i];
                text.text = $"Score: {data.score}  |  {data.date}, {data.time}";
            }
        }

        Debug.Log($"[HighscoreUI] Displayed {recent3.Count} recent scores");
    }
}
