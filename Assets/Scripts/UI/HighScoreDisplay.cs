using UnityEngine;
using TMPro;
using DG.Tweening;

/// <summary>
/// UI Component untuk menampilkan high score di main menu
/// </summary>
public class HighScoreDisplay : MonoBehaviour
{
    [Header("Score Text References")]
    [SerializeField] private TextMeshProUGUI level1ScoreText;
    [SerializeField] private TextMeshProUGUI level2ScoreText;
    [SerializeField] private TextMeshProUGUI totalScoreText;

    [Header("Date Text References (Optional)")]
    [SerializeField] private TextMeshProUGUI level1DateText;
    [SerializeField] private TextMeshProUGUI level2DateText;
    [SerializeField] private TextMeshProUGUI totalDateText;

    [Header("No Score Text")]
    [SerializeField] private string noScoreText = "---";
    [SerializeField] private string noDateText = "Belum Main";

    [Header("Animation Settings")]
    [SerializeField] private bool animateOnEnable = true;
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float animationDelay = 0.2f; // Delay antara tiap score

    [Header("Highlight Settings")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color highlightColor = Color.yellow;
    [SerializeField] private float highlightDuration = 0.3f;

    private void OnEnable()
    {
        RefreshScores();

        if (animateOnEnable)
        {
            AnimateScoresIn();
        }
    }

    /// <summary>
    /// Refresh semua score dari HighScoreManager
    /// </summary>
    public void RefreshScores()
    {
        ScoreSummary summary = HighScoreManager.Instance.GetScoreSummary();

        // Update Level 1 Score
        if (level1ScoreText != null)
        {
            level1ScoreText.text = summary.level1HighScore > 0
                ? summary.level1HighScore.ToString()
                : noScoreText;
        }

        // Update Level 2 Score
        if (level2ScoreText != null)
        {
            level2ScoreText.text = summary.level2HighScore > 0
                ? summary.level2HighScore.ToString()
                : noScoreText;
        }

        // Update Total Score
        if (totalScoreText != null)
        {
            totalScoreText.text = summary.totalHighScore > 0
                ? summary.totalHighScore.ToString()
                : noScoreText;
        }

        // Update Dates (if text fields assigned)
        if (level1DateText != null)
        {
            level1DateText.text = summary.level1Date != "-"
                ? summary.level1Date
                : noDateText;
        }

        if (level2DateText != null)
        {
            level2DateText.text = summary.level2Date != "-"
                ? summary.level2Date
                : noDateText;
        }

        if (totalDateText != null)
        {
            totalDateText.text = summary.totalDate != "-"
                ? summary.totalDate
                : noDateText;
        }

        Debug.Log($"[HighScoreDisplay] Scores refreshed: {summary}");
    }

    /// <summary>
    /// Animate scores sliding in
    /// </summary>
    private void AnimateScoresIn()
    {
        TextMeshProUGUI[] scoreTexts = { level1ScoreText, level2ScoreText, totalScoreText };

        for (int i = 0; i < scoreTexts.Length; i++)
        {
            if (scoreTexts[i] == null) continue;

            // Set initial state
            scoreTexts[i].alpha = 0;
            scoreTexts[i].transform.localScale = Vector3.zero;

            // Animate in with delay
            float delay = i * animationDelay;

            scoreTexts[i].DOFade(1, animationDuration).SetDelay(delay);
            scoreTexts[i].transform.DOScale(1, animationDuration)
                .SetDelay(delay)
                .SetEase(Ease.OutBack);
        }
    }

    /// <summary>
    /// Highlight specific score (untuk indicate new high score)
    /// </summary>
    public void HighlightLevel1Score()
    {
        if (level1ScoreText != null)
        {
            level1ScoreText.color = normalColor;
            level1ScoreText.DOColor(highlightColor, highlightDuration)
                .SetLoops(3, LoopType.Yoyo);
        }
    }

    /// <summary>
    /// Highlight Level 2 score
    /// </summary>
    public void HighlightLevel2Score()
    {
        if (level2ScoreText != null)
        {
            level2ScoreText.color = normalColor;
            level2ScoreText.DOColor(highlightColor, highlightDuration)
                .SetLoops(3, LoopType.Yoyo);
        }
    }

    /// <summary>
    /// Highlight total score
    /// </summary>
    public void HighlightTotalScore()
    {
        if (totalScoreText != null)
        {
            totalScoreText.color = normalColor;
            totalScoreText.DOColor(highlightColor, highlightDuration)
                .SetLoops(3, LoopType.Yoyo);
        }
    }

    /// <summary>
    /// Format score dengan padding (misal: 0050)
    /// </summary>
    public static string FormatScore(int score, int digits = 4)
    {
        return score.ToString($"D{digits}");
    }

    /// <summary>
    /// Get score rank based on value
    /// </summary>
    public static string GetScoreRank(int score)
    {
        if (score >= 100) return "S"; // Perfect score
        if (score >= 80) return "A";
        if (score >= 60) return "B";
        if (score >= 40) return "C";
        return "D";
    }

    /// <summary>
    /// Reset all displayed scores (for testing)
    /// </summary>
    public void ClearDisplay()
    {
        if (level1ScoreText != null) level1ScoreText.text = noScoreText;
        if (level2ScoreText != null) level2ScoreText.text = noScoreText;
        if (totalScoreText != null) totalScoreText.text = noScoreText;
        if (level1DateText != null) level1DateText.text = noDateText;
        if (level2DateText != null) level2DateText.text = noDateText;
        if (totalDateText != null) totalDateText.text = noDateText;
    }
}
