using UnityEngine;
using TMPro;
using DG.Tweening;

/// <summary>
/// Manages score display and +score animation in Chapter 1
/// </summary>
public class ScoreDisplayManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI totalScoreText;
    [SerializeField] private TextMeshProUGUI scorePopupPrefab; // Prefab for +10 animation
    [SerializeField] private Transform popupSpawnPoint; // Where +10 appears

    [Header("Animation Settings")]
    [SerializeField] private float popupDuration = 1.5f;
    [SerializeField] private float popupFloatDistance = 100f;
    [SerializeField] private Color scoreColor = Color.green;

    private int currentScore = 0;

    private void Start()
    {
        UpdateScoreDisplay();
    }

    /// <summary>
    /// Add score with popup animation
    /// </summary>
    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreDisplay();
        ShowScorePopup(amount);
    }

    /// <summary>
    /// Get current score
    /// </summary>
    public int GetScore()
    {
        return currentScore;
    }

    /// <summary>
    /// Reset score to 0
    /// </summary>
    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreDisplay();
        Debug.Log("[ScoreDisplay] Score reset to 0");
    }

    /// <summary>
    /// Update total score display
    /// </summary>
    private void UpdateScoreDisplay()
    {
        if (totalScoreText != null)
        {
            totalScoreText.text = $"Score: {currentScore}";
        }
    }

    /// <summary>
    /// Show +score popup animation
    /// </summary>
    private void ShowScorePopup(int amount)
    {
        if (scorePopupPrefab == null || popupSpawnPoint == null)
        {
            Debug.LogWarning("[ScoreDisplay] Popup prefab or spawn point not set");
            return;
        }

        // Instantiate popup
        TextMeshProUGUI popup = Instantiate(scorePopupPrefab, popupSpawnPoint.position, Quaternion.identity, transform);
        popup.text = $"+{amount}";
        popup.color = scoreColor;

        RectTransform rectTransform = popup.GetComponent<RectTransform>();
        CanvasGroup canvasGroup = popup.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = popup.gameObject.AddComponent<CanvasGroup>();
        }

        // Animation sequence
        Vector2 startPos = rectTransform.anchoredPosition;
        Vector2 endPos = startPos + Vector2.up * popupFloatDistance;

        Sequence sequence = DOTween.Sequence();

        // Float up
        sequence.Append(rectTransform.DOAnchorPos(endPos, popupDuration).SetEase(Ease.OutQuad));

        // Fade out (start fading halfway through)
        sequence.Join(canvasGroup.DOFade(0f, popupDuration * 0.6f).SetDelay(popupDuration * 0.4f));

        // Destroy after animation
        sequence.OnComplete(() =>
        {
            if (popup != null)
                Destroy(popup.gameObject);
        });
    }
}
