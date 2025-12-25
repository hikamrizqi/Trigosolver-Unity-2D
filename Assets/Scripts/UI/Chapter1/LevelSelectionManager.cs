using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;

/// <summary>
/// Manages level selection UI at the start of Chapter 1
/// Shows 3 buttons: Level 1 (Soal 1-10), Level 2 (Soal 11-20), and Level 3 (Soal 21-30)
/// </summary>
public class LevelSelectionManager : MonoBehaviour
{
    [Header("Level Selection UI")]
    [SerializeField] private GameObject levelSelectionPanel;
    [SerializeField] private Button level1Button;
    [SerializeField] private Button level2Button;
    [SerializeField] private Button level3Button;

    [Header("Game Objects to Hide/Show")]
    [SerializeField] private GameObject triangleVisualizerObject;
    [SerializeField] private GameObject answerTileSystemObject;
    [SerializeField] private GameObject backgroundObject; // Background meja
    [SerializeField] private GameObject questionPanelObject; // Panel pertanyaan
    [SerializeField] private GameObject interactiveButtonPanel; // Tombol DEPAN/SAMPING/MIRING
    [SerializeField] private GameObject checkButtonObject; // Tombol CHECK

    [Header("Level Specific Objects (Optional)")]
    [SerializeField] private GameObject answerSlotsLevel1; // Slot khusus Level 1 (jika ada)
    [SerializeField] private GameObject answerSlotsLevel2; // Slot khusus Level 2 (jika ada)
    [SerializeField] private GameObject answerSlotsLevel3; // Slot khusus Level 3 (jika ada)

    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float slideDistance = 1000f; // Distance to slide off screen
    [SerializeField] private Ease slideEase = Ease.OutCubic;

    [Header("Manager References")]
    [SerializeField] private CalculationManager calculationManager;

    private bool levelSelected = false;

    private void Awake()
    {
        // Setup button listeners
        if (level1Button != null)
            level1Button.onClick.AddListener(() => OnLevelSelected(1));

        if (level2Button != null)
            level2Button.onClick.AddListener(() => OnLevelSelected(2));

        if (level3Button != null)
            level3Button.onClick.AddListener(() => OnLevelSelected(3));
    }

    private void Start()
    {
        // Hide all game objects at start
        HideAllGameObjects(false); // No animation, instant hide

        // Show level selection panel
        if (levelSelectionPanel != null)
        {
            levelSelectionPanel.SetActive(true);
            // Animate panel in from top
            RectTransform panelRect = levelSelectionPanel.GetComponent<RectTransform>();
            if (panelRect != null)
            {
                Vector2 originalPos = panelRect.anchoredPosition;
                panelRect.anchoredPosition = originalPos + Vector2.up * slideDistance;
                panelRect.DOAnchorPos(originalPos, animationDuration).SetEase(slideEase);
            }
        }
    }

    /// <summary>
    /// Called when level button is clicked
    /// </summary>
    private void OnLevelSelected(int level)
    {
        if (levelSelected) return; // Prevent multiple clicks
        levelSelected = true;

        Debug.Log($"[LevelSelection] Level {level} selected");

        // Start animation sequence
        StartCoroutine(LevelSelectionSequence(level));
    }

    /// <summary>
    /// Animate level selection out, then game objects in
    /// </summary>
    private IEnumerator LevelSelectionSequence(int level)
    {
        // 1. Animate level selection panel OUT (slide up)
        if (levelSelectionPanel != null)
        {
            RectTransform panelRect = levelSelectionPanel.GetComponent<RectTransform>();
            if (panelRect != null)
            {
                Vector2 targetPos = panelRect.anchoredPosition + Vector2.up * slideDistance;
                panelRect.DOAnchorPos(targetPos, animationDuration).SetEase(slideEase);
            }
        }

        // Wait for panel to slide out
        yield return new WaitForSeconds(animationDuration);

        // Hide panel completely
        if (levelSelectionPanel != null)
            levelSelectionPanel.SetActive(false);

        // 2. Show and animate game objects IN
        ShowAllGameObjects(true); // With animation

        // Wait for game objects to slide in
        yield return new WaitForSeconds(animationDuration + 0.2f);

        // 3. Start game from appropriate question number
        int startingQuestion;
        if (level == 1)
            startingQuestion = 1;
        else if (level == 2)
            startingQuestion = 11;
        else // level == 3
            startingQuestion = 21;

        if (calculationManager != null)
        {
            calculationManager.StartFromQuestion(startingQuestion);
        }
        else
        {
            Debug.LogError("[LevelSelection] CalculationManager reference missing!");
        }
    }

    /// <summary>
    /// Hide all game objects (instant or animated)
    /// </summary>
    private void HideAllGameObjects(bool animated)
    {
        GameObject[] objectsToHide = {
            triangleVisualizerObject,
            answerTileSystemObject,
            backgroundObject,
            questionPanelObject,
            interactiveButtonPanel,
            checkButtonObject
        };

        foreach (GameObject obj in objectsToHide)
        {
            if (obj == null) continue;

            if (animated)
            {
                // Animate out based on object type
                RectTransform rectTransform = obj.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    // UI element - slide based on position
                    Vector2 offscreenPos = rectTransform.anchoredPosition;
                    if (rectTransform.anchoredPosition.x < 0)
                        offscreenPos += Vector2.left * slideDistance;
                    else
                        offscreenPos += Vector2.right * slideDistance;

                    rectTransform.DOAnchorPos(offscreenPos, animationDuration).SetEase(slideEase)
                        .OnComplete(() => obj.SetActive(false));
                }
                else
                {
                    // World object - just hide
                    obj.SetActive(false);
                }
            }
            else
            {
                // Instant hide
                obj.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Show all game objects (instant or animated)
    /// </summary>
    private void ShowAllGameObjects(bool animated)
    {
        // Only animate UI elements, world objects use their own animations
        GameObject[] uiObjectsToAnimate = {
            questionPanelObject,
            interactiveButtonPanel,
            checkButtonObject
        };

        // World objects - activate tanpa animasi (mereka punya animasi sendiri)
        GameObject[] worldObjectsToShow = {
            backgroundObject,
            triangleVisualizerObject,
            answerTileSystemObject
        };

        // Show world objects first (no animation dari LevelSelectionManager)
        foreach (GameObject obj in worldObjectsToShow)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        // Animate UI elements
        foreach (GameObject obj in uiObjectsToAnimate)
        {
            if (obj == null) continue;

            obj.SetActive(true);

            if (animated)
            {
                RectTransform rectTransform = obj.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    // UI element - slide in from offscreen
                    Vector2 finalPos = rectTransform.anchoredPosition;
                    Vector2 startPos = finalPos;

                    // Determine slide direction based on position
                    if (finalPos.y > 0)
                        startPos += Vector2.up * slideDistance; // From top
                    else if (finalPos.y < 0)
                        startPos += Vector2.down * slideDistance; // From bottom
                    else if (finalPos.x < 0)
                        startPos += Vector2.left * slideDistance; // From left
                    else
                        startPos += Vector2.right * slideDistance; // From right

                    rectTransform.anchoredPosition = startPos;
                    rectTransform.DOAnchorPos(finalPos, animationDuration).SetEase(slideEase);
                }
            }
        }
    }

    /// <summary>
    /// Tampilkan kembali panel level selection (dipanggil dari tombol BACK saat bermain)
    /// </summary>
    public void ShowLevelSelection()
    {
        Debug.Log("[LevelSelection] Menampilkan kembali panel level selection");

        // Reset flag
        levelSelected = false;

        // Sembunyikan semua game objects
        HideAllGameObjects();

        // Tampilkan panel level selection dengan animasi slide in
        if (levelSelectionPanel != null)
        {
            levelSelectionPanel.SetActive(true);

            RectTransform panelRect = levelSelectionPanel.GetComponent<RectTransform>();
            if (panelRect != null)
            {
                // Slide in dari atas
                Vector2 finalPos = Vector2.zero; // Posisi center
                Vector2 startPos = finalPos + Vector2.up * slideDistance;

                panelRect.anchoredPosition = startPos;
                panelRect.DOAnchorPos(finalPos, animationDuration).SetEase(slideEase);
            }
        }
    }

    /// <summary>
    /// Hide all game objects (untuk kembali ke level selection)
    /// </summary>
    private void HideAllGameObjects()
    {
        GameObject[] allGameObjects = {
            backgroundObject,
            triangleVisualizerObject,
            answerTileSystemObject,
            questionPanelObject,
            interactiveButtonPanel,
            checkButtonObject,
            // Level specific objects
            answerSlotsLevel1,
            answerSlotsLevel2,
            answerSlotsLevel3
        };

        foreach (GameObject obj in allGameObjects)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }

    /// <summary>
    /// Kembali ke Main Menu (dipanggil dari tombol BACK di panel level selection)
    /// </summary>
    public void BackToMainMenu()
    {
        Debug.Log("[LevelSelection] Kembali ke Main Menu");
        SceneManager.LoadScene("Main Menu");
    }
}
