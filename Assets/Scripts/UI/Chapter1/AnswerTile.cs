using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

/// <summary>
/// Kotak jawaban individual yang bisa di-tap dan bergerak
/// (Duolingo-style answer tile)
/// </summary>
public class AnswerTile : MonoBehaviour
{
    [Header("Visual")]
    public TextMeshProUGUI valueText;
    public Button button;
    public Image background;

    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color highlightColor = new Color(0.9f, 0.9f, 1f);
    public Color correctColor = new Color(0.5f, 1f, 0.5f);
    public Color wrongColor = new Color(1f, 0.5f, 0.5f);

    private string tileValue;
    private bool isInSlot = false;
    private Vector3 originalPosition;
    private Transform originalParent;

    public string Value => tileValue;
    public bool IsInSlot => isInSlot;

    private void Awake()
    {
        if (button == null) button = GetComponent<Button>();
        if (background == null) background = GetComponent<Image>();

        button.onClick.AddListener(OnTileClicked);
    }

    /// <summary>
    /// Setup tile dengan nilai
    /// </summary>
    public void Setup(string value)
    {
        tileValue = value;
        if (valueText != null)
        {
            valueText.text = value;
        }

        // Simpan posisi original
        originalPosition = transform.localPosition;
        originalParent = transform.parent;
        isInSlot = false;

        SetColor(normalColor);
    }

    /// <summary>
    /// Tile di-tap
    /// </summary>
    private void OnTileClicked()
    {
        if (isInSlot)
        {
            // Tile di slot → Return ke pool
            AnswerTileSystem.Instance.ReturnTileToPool(this);
        }
        else
        {
            // Tile di pool → Move ke slot
            AnswerTileSystem.Instance.MoveTileToSlot(this);
        }
    }

    /// <summary>
    /// Animasi bergerak ke posisi target
    /// </summary>
    public void AnimateToPosition(Vector3 targetPosition, Transform newParent, bool inSlot, float duration = 0.3f)
    {
        isInSlot = inSlot;

        // Kill any existing tweens on this transform
        transform.DOKill();

        // Change parent tapi keep world position
        Vector3 worldPos = transform.position;
        transform.SetParent(newParent, true);
        transform.position = worldPos;

        // Animate ke target local position
        transform.DOLocalMove(targetPosition, duration)
            .SetEase(Ease.OutBack);

        // Scale bounce effect
        transform.localScale = Vector3.one;
        transform.DOScale(1.1f, duration * 0.5f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                transform.DOScale(1f, duration * 0.5f).SetEase(Ease.InQuad);
            });
    }

    /// <summary>
    /// Return ke posisi original
    /// </summary>
    public void ReturnToOriginalPosition(float duration = 0.3f)
    {
        isInSlot = false;

        // Kill any existing tweens on this transform
        transform.DOKill();

        // Return to original parent
        transform.SetParent(originalParent, false); // FALSE = reset local position

        // Force reset anchored position (penting untuk Grid Layout Group)
        RectTransform rect = transform as RectTransform;
        if (rect != null)
        {
            rect.anchoredPosition = Vector2.zero;
            rect.localScale = Vector3.one;
            rect.localRotation = Quaternion.identity;
        }

        // Force rebuild layout
        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(originalParent as RectTransform);

        Debug.Log($"[AnswerTile] {Value} returned to pool, anchored position reset to (0,0)");
    }

    /// <summary>
    /// Set color untuk visual feedback
    /// </summary>
    public void SetColor(Color color)
    {
        if (background != null)
        {
            background.color = color;
        }
    }

    /// <summary>
    /// Highlight tile
    /// </summary>
    public void Highlight(bool correct)
    {
        SetColor(correct ? correctColor : wrongColor);
    }

    /// <summary>
    /// Reset ke normal
    /// </summary>
    public void ResetVisual()
    {
        SetColor(normalColor);
    }
}
