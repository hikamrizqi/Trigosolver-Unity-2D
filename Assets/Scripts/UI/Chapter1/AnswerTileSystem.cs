using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

/// <summary>
/// System untuk input jawaban berbasis tile (interactive tile-based input)
/// Tile bergerak dari pool ke slot dengan animasi
/// </summary>
public class AnswerTileSystem : MonoBehaviour
{
    public static AnswerTileSystem Instance { get; private set; }

    [Header("Answer Slots - Single Question (Soal 1-10)")]
    [SerializeField] private GameObject singleQuestionSlotContainer;  // Container untuk layout horizontal
    [SerializeField] private Transform singleSlot1Transform;  // Numerator (kiri)
    [SerializeField] private Transform singleSlot2Transform;  // Denominator (kanan)
    [SerializeField] private TextMeshProUGUI singleSlashText; // "/" di tengah

    [Header("Answer Slots - Dual Question (Soal 11-20)")]
    [SerializeField] private GameObject dualQuestionSlotContainer;  // Container untuk layout 2x2
    [SerializeField] private Transform dualSlot1Transform;  // Fraction 1 - Numerator
    [SerializeField] private Transform dualSlot2Transform;  // Fraction 1 - Denominator
    [SerializeField] private Transform dualSlot3Transform;  // Fraction 2 - Numerator
    [SerializeField] private Transform dualSlot4Transform;  // Fraction 2 - Denominator
    [SerializeField] private TextMeshProUGUI dualSlashText1; // "/" fraction 1
    [SerializeField] private TextMeshProUGUI dualSlashText2; // "/" fraction 2

    [Header("Answer Slots - Single Answer (Soal 21-30 - Level 3)")]
    [SerializeField] private GameObject singleAnswerSlotContainer;  // Container untuk 1 slot (multiple choice)
    [SerializeField] private Transform singleAnswerSlotTransform;   // Single slot untuk jawaban integer

    [Header("Answer Slots - Multi-Step (Level 3 Pythagoras Step-by-step)")]
    [SerializeField] private GameObject multiStepSlotContainer;     // Container untuk 6 sequential slots
    [SerializeField] private Transform[] multiStepSlotTransforms;   // Array 6 slots untuk formula step-by-step
    [SerializeField] private TextMeshProUGUI questionSideText;      // Text untuk sisi yang ditanya (AC/AB/BC)
    [SerializeField] private TextMeshProUGUI operatorText1;         // Text untuk operator pertama dalam kurung (AB² + BC²)
    [SerializeField] private TextMeshProUGUI operatorText2;         // Text untuk operator kedua dalam kurung (9 + 16)

    [Header("Answer Pool (Pilihan Jawaban)")]
    [SerializeField] private Transform poolContainer;   // Container untuk 5 tiles
    [SerializeField] private GameObject tilePrefab;     // Prefab untuk AnswerTile

    [Header("Integration")]
    [SerializeField] private TMP_InputField hiddenInputField; // Sync dengan UIManager jawabanInput

    [Header("Settings")]
    [SerializeField] private int poolSize = 5;          // Jumlah tiles (default 5)
    [SerializeField] private float animationDuration = 0.3f;

    [Header("Entry Animation")]
    [SerializeField] private float entryAnimDuration = 0.5f;
    [SerializeField] private float entryStaggerDelay = 0.1f; // Delay antar tile
    [SerializeField] private float offscreenDistance = 500f; // Pixel distance
    [SerializeField] private Ease entryEase = Ease.OutBack;

    private List<AnswerTile> allTiles = new List<AnswerTile>();
    private AnswerTile currentSlot1Tile = null;
    private AnswerTile currentSlot2Tile = null;
    private AnswerTile currentSlot3Tile = null; // Soal 11-20 only
    private AnswerTile currentSlot4Tile = null; // Soal 11-20 only

    // Multi-step slots (Level 3 sequential)
    private AnswerTile[] multiStepSlotTiles = new AnswerTile[6]; // Track tiles in 6 sequential slots
    private int currentMultiStepSlotIndex = 0; // Next empty slot index (0-5)

    // Jawaban yang benar untuk soal ini
    private string correctNumerator;   // Angka atas 1 (untuk soal 1-10 atau soal 11-20 pertanyaan pertama)
    private string correctDenominator; // Angka bawah 1
    private string correctNumerator2;  // Angka atas 2 (untuk soal 11-20 pertanyaan kedua)
    private string correctDenominator2; // Angka bawah 2
    private string correctSingleAnswer; // Jawaban tunggal (untuk soal 21-30 Level 3)
    private List<string> correctMultiStepAnswers; // 6 jawaban sequential untuk Level 3

    // Track question type
    private bool currentIsDualQuestion = false;
    private bool currentIsSingleAnswer = false; // Level 3 mode - single answer
    private bool currentIsMultiStepAnswer = false; // Level 3 mode - multi-step sequential

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Setup soal baru dengan jawaban benar dan distractor (untuk soal 1-10 - single question)
    /// </summary>
    public void SetupQuestion(string numerator, string denominator, List<string> wrongAnswers)
    {
        SetupQuestion(numerator, denominator, "", "", wrongAnswers, false);
    }

    /// <summary>
    /// Setup soal baru dengan jawaban benar dan distractor (untuk semua soal - dual or single)
    /// </summary>
    public void SetupQuestion(string numerator, string denominator, string numerator2, string denominator2, List<string> wrongAnswers, bool isDualQuestion)
    {
        // Simpan jawaban benar
        correctNumerator = numerator;
        correctDenominator = denominator;
        correctNumerator2 = numerator2;
        correctDenominator2 = denominator2;
        currentIsDualQuestion = isDualQuestion;

        // Show/hide ENTIRE CONTAINER based on question type
        if (singleQuestionSlotContainer != null)
            singleQuestionSlotContainer.SetActive(!isDualQuestion);

        if (dualQuestionSlotContainer != null)
            dualQuestionSlotContainer.SetActive(isDualQuestion);

        // Reset slots
        ResetSlots();

        // Destroy tiles lama
        foreach (var tile in allTiles)
        {
            if (tile != null) Destroy(tile.gameObject);
        }
        allTiles.Clear();

        // Buat pool
        List<string> poolValues = new List<string>();
        if (isDualQuestion)
        {
            // Dual question: 4 correct + wrong answers (total 6 tiles)
            poolValues.Add(numerator);
            poolValues.Add(denominator);
            poolValues.Add(numerator2);
            poolValues.Add(denominator2);
        }
        else
        {
            // Single question: 2 correct + wrong answers (total 6 tiles)
            poolValues.Add(numerator);
            poolValues.Add(denominator);
        }
        poolValues.AddRange(wrongAnswers);

        // Shuffle untuk random order
        poolValues = poolValues.OrderBy(x => Random.value).ToList();

        // Instantiate tiles (dynamic count based on pool size)
        for (int i = 0; i < poolValues.Count; i++)
        {
            GameObject tileObj = Instantiate(tilePrefab, poolContainer);
            AnswerTile tile = tileObj.GetComponent<AnswerTile>();
            tile.Setup(poolValues[i]);
            allTiles.Add(tile);
        }

        string questionType = isDualQuestion ? "DUAL (4 answers)" : "SINGLE (2 answers)";
        Debug.Log($"[AnswerTileSystem] Setup {questionType}: {numerator}/{denominator}" +
                  (isDualQuestion ? $" and {numerator2}/{denominator2}" : "") +
                  $", Total tiles: {poolValues.Count}, Pool: {string.Join(", ", poolValues)}");

        // Wait for layout group to position tiles, then animate
        StartCoroutine(AnimateTilesInDelayed());
    }

    /// <summary>
    /// Setup soal Level 3 (single answer multiple choice) - soal 21-30
    /// 1 jawaban benar + 5 pilihan salah = total 6 tiles
    /// </summary>
    public void SetupSingleAnswerQuestion(string correctAnswer, List<string> wrongAnswers)
    {
        // Simpan jawaban benar
        correctSingleAnswer = correctAnswer;
        currentIsSingleAnswer = true;
        currentIsDualQuestion = false;

        // Show/hide containers
        if (singleQuestionSlotContainer != null)
            singleQuestionSlotContainer.SetActive(false);

        if (dualQuestionSlotContainer != null)
            dualQuestionSlotContainer.SetActive(false);

        if (singleAnswerSlotContainer != null)
            singleAnswerSlotContainer.SetActive(true);

        // Reset slots
        ResetSlots();

        // Destroy tiles lama
        foreach (var tile in allTiles)
        {
            if (tile != null) Destroy(tile.gameObject);
        }
        allTiles.Clear();

        // Buat pool: 1 correct + 5 wrong = 6 tiles
        List<string> poolValues = new List<string>();
        poolValues.Add(correctAnswer);
        poolValues.AddRange(wrongAnswers);

        // Shuffle untuk random order
        poolValues = poolValues.OrderBy(x => Random.value).ToList();

        // Instantiate tiles
        for (int i = 0; i < poolValues.Count; i++)
        {
            GameObject tileObj = Instantiate(tilePrefab, poolContainer);
            AnswerTile tile = tileObj.GetComponent<AnswerTile>();
            tile.Setup(poolValues[i]);
            allTiles.Add(tile);
        }

        Debug.Log($"[AnswerTileSystem] Setup SINGLE ANSWER (Level 3): Correct={correctAnswer}, Total tiles: {poolValues.Count}, Pool: {string.Join(", ", poolValues)}");

        // Wait for layout group to position tiles, then animate
        StartCoroutine(AnimateTilesInDelayed());
    }

    /// <summary>
    /// Setup untuk Level 3 multi-step answer (6 sequential slots)
    /// 6 jawaban benar (langkah-langkah) + 2 pilihan salah = total 8 tiles
    /// </summary>
    public void SetupMultiStepQuestion(List<string> correctAnswers, List<string> wrongAnswers, string questionSide, string side1, string side2, string operatorSymbol)
    {
        // Simpan jawaban benar (6 langkah)
        correctMultiStepAnswers = new List<string>(correctAnswers);
        currentIsMultiStepAnswer = true;
        currentIsSingleAnswer = false;
        currentIsDualQuestion = false;

        // Set dynamic formula parts
        if (questionSideText != null)
        {
            questionSideText.text = questionSide;
            Debug.Log($"[AnswerTileSystem] Question Side: {questionSide}");
        }

        // Set operator untuk kedua tempat (dalam kurung pertama dan kedua)
        if (operatorText1 != null)
        {
            operatorText1.text = operatorSymbol;
            Debug.Log($"[AnswerTileSystem] Operator 1: {operatorSymbol}");
        }

        if (operatorText2 != null)
        {
            operatorText2.text = operatorSymbol;
            Debug.Log($"[AnswerTileSystem] Operator 2: {operatorSymbol}");
        }

        // Show/hide containers
        if (singleQuestionSlotContainer != null)
            singleQuestionSlotContainer.SetActive(false);

        if (dualQuestionSlotContainer != null)
            dualQuestionSlotContainer.SetActive(false);

        if (singleAnswerSlotContainer != null)
            singleAnswerSlotContainer.SetActive(false);

        if (multiStepSlotContainer != null)
            multiStepSlotContainer.SetActive(true);

        // Reset multi-step slots
        ResetMultiStepSlots();

        // Destroy tiles lama
        foreach (var tile in allTiles)
        {
            if (tile != null) Destroy(tile.gameObject);
        }
        allTiles.Clear();

        // Buat pool: 6 correct + 2 wrong = 8 tiles
        List<string> poolValues = new List<string>();
        poolValues.AddRange(correctAnswers);
        poolValues.AddRange(wrongAnswers);

        // Shuffle untuk random order
        poolValues = poolValues.OrderBy(x => Random.value).ToList();

        // Instantiate tiles
        for (int i = 0; i < poolValues.Count; i++)
        {
            GameObject tileObj = Instantiate(tilePrefab, poolContainer);
            AnswerTile tile = tileObj.GetComponent<AnswerTile>();
            tile.Setup(poolValues[i]);
            allTiles.Add(tile);
        }

        Debug.Log($"[AnswerTileSystem] Setup MULTI-STEP (Level 3): 6 Steps={string.Join(", ", correctAnswers)}, Total tiles: {poolValues.Count}");

        // Wait for layout group to position tiles, then animate
        StartCoroutine(AnimateTilesInDelayed());
    }

    /// <summary>
    /// Reset multi-step slots (clear all 6 slots)
    /// </summary>
    private void ResetMultiStepSlots()
    {
        for (int i = 0; i < 6; i++)
        {
            multiStepSlotTiles[i] = null;
        }
        currentMultiStepSlotIndex = 0;
    }

    /// <summary>
    /// Delayed animation agar layout group selesai calculate positions
    /// </summary>
    private System.Collections.IEnumerator AnimateTilesInDelayed()
    {
        // Wait 1 frame untuk layout group selesai
        yield return null;

        // Force layout rebuild
        Canvas.ForceUpdateCanvases();
        if (poolContainer != null)
        {
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(poolContainer as RectTransform);
        }

        AnimateTilesIn();
    }

    /// <summary>
    /// Move tile ke slot kosong
    /// </summary>
    public void MoveTileToSlot(AnswerTile tile)
    {
        if (tile.IsInSlot)
        {
            Debug.LogWarning($"[AnswerTileSystem] Tile {tile.Value} already in slot!");
            return;
        }

        // Tentukan slot target berdasarkan question type
        Transform targetSlot;

        if (currentIsMultiStepAnswer)
        {
            // MULTI-STEP (Level 3): Sequential filling, 6 slots
            if (currentMultiStepSlotIndex >= 6)
            {
                Debug.LogWarning($"[AnswerTileSystem] All 6 multi-step slots full! Cannot move {tile.Value}");
                return;
            }

            // Place tile in next empty slot
            int slotIndex = currentMultiStepSlotIndex;
            multiStepSlotTiles[slotIndex] = tile;
            targetSlot = multiStepSlotTransforms[slotIndex];
            currentMultiStepSlotIndex++;

            Debug.Log($"[AnswerTileSystem] MULTI-STEP: Moving {tile.Value} to Slot {slotIndex + 1}/6");
        }
        else if (currentIsSingleAnswer)
        {
            // SINGLE ANSWER (Level 3): Only 1 slot
            if (currentSlot1Tile == null)
            {
                currentSlot1Tile = tile;
                targetSlot = singleAnswerSlotTransform;
                Debug.Log($"[AnswerTileSystem] SINGLE ANSWER (Level 3): Moving {tile.Value} to SingleAnswerSlot");
            }
            else
            {
                Debug.LogWarning($"[AnswerTileSystem] Single answer slot full! Cannot move {tile.Value}");
                return;
            }
        }
        else if (currentIsDualQuestion)
        {
            // DUAL QUESTION: Fill order → dualSlot1 → dualSlot2 → dualSlot3 → dualSlot4
            if (currentSlot1Tile == null)
            {
                currentSlot1Tile = tile;
                targetSlot = dualSlot1Transform;
                Debug.Log($"[AnswerTileSystem] DUAL: Moving {tile.Value} to DualSlot1");
            }
            else if (currentSlot2Tile == null)
            {
                currentSlot2Tile = tile;
                targetSlot = dualSlot2Transform;
                Debug.Log($"[AnswerTileSystem] DUAL: Moving {tile.Value} to DualSlot2");
            }
            else if (currentSlot3Tile == null)
            {
                currentSlot3Tile = tile;
                targetSlot = dualSlot3Transform;
                Debug.Log($"[AnswerTileSystem] DUAL: Moving {tile.Value} to DualSlot3");
            }
            else if (currentSlot4Tile == null)
            {
                currentSlot4Tile = tile;
                targetSlot = dualSlot4Transform;
                Debug.Log($"[AnswerTileSystem] DUAL: Moving {tile.Value} to DualSlot4");
            }
            else
            {
                Debug.LogWarning($"[AnswerTileSystem] All 4 dual slots full! Cannot move {tile.Value}");
                return;
            }
        }
        else
        {
            // SINGLE QUESTION: Fill order → singleSlot1 → singleSlot2
            if (currentSlot1Tile == null)
            {
                currentSlot1Tile = tile;
                targetSlot = singleSlot1Transform;
                Debug.Log($"[AnswerTileSystem] SINGLE: Moving {tile.Value} to SingleSlot1");
            }
            else if (currentSlot2Tile == null)
            {
                currentSlot2Tile = tile;
                targetSlot = singleSlot2Transform;
                Debug.Log($"[AnswerTileSystem] SINGLE: Moving {tile.Value} to SingleSlot2");
            }
            else
            {
                Debug.LogWarning($"[AnswerTileSystem] Both single slots full! Cannot move {tile.Value}");
                return;
            }
        }

        // Animate tile ke slot
        tile.AnimateToPosition(Vector3.zero, targetSlot, true, animationDuration);

        UpdateAnswerDisplay();
    }

    /// <summary>
    /// Return tile dari slot ke pool
    /// </summary>
    public void ReturnTileToPool(AnswerTile tile)
    {
        if (!tile.IsInSlot)
        {
            Debug.LogWarning($"[AnswerTileSystem] Tile {tile.Value} not in slot!");
            return;
        }

        // Handle multi-step mode dengan backward shifting
        if (currentIsMultiStepAnswer)
        {
            // Find which slot this tile is in
            int removedSlotIndex = -1;
            for (int i = 0; i < 6; i++)
            {
                if (multiStepSlotTiles[i] == tile)
                {
                    removedSlotIndex = i;
                    break;
                }
            }

            if (removedSlotIndex >= 0)
            {
                Debug.Log($"[AnswerTileSystem] MULTI-STEP: Removing tile from Slot {removedSlotIndex + 1}");

                // Shift all tiles after this one backward
                for (int i = removedSlotIndex; i < currentMultiStepSlotIndex - 1; i++)
                {
                    multiStepSlotTiles[i] = multiStepSlotTiles[i + 1];

                    // Animate shift
                    if (multiStepSlotTiles[i] != null)
                    {
                        multiStepSlotTiles[i].AnimateToPosition(Vector3.zero, multiStepSlotTransforms[i], true, animationDuration);
                        Debug.Log($"[AnswerTileSystem] MULTI-STEP: Shifted tile {multiStepSlotTiles[i].Value} from Slot {i + 2} to Slot {i + 1}");
                    }
                }

                // Clear last slot
                multiStepSlotTiles[currentMultiStepSlotIndex - 1] = null;
                currentMultiStepSlotIndex--;
            }
        }
        // Hapus dari slot untuk single/dual mode
        else if (currentSlot1Tile == tile)
        {
            currentSlot1Tile = null;
            Debug.Log($"[AnswerTileSystem] Removed {tile.Value} from Slot1");

            // Auto-shift untuk single question: Jika slot2 ada isi, geser ke kiri
            if (!currentIsDualQuestion && !currentIsSingleAnswer && currentSlot2Tile != null)
            {
                AnswerTile tileToShift = currentSlot2Tile;
                currentSlot2Tile = null;

                // Geser ke slot1
                currentSlot1Tile = tileToShift;
                Transform targetSlot = currentIsDualQuestion ? dualSlot1Transform : singleSlot1Transform;
                tileToShift.AnimateToPosition(Vector3.zero, targetSlot, true, animationDuration);
                Debug.Log($"[AnswerTileSystem] Auto-shifted {tileToShift.Value} from Slot2 to Slot1");
            }
        }
        else if (currentSlot2Tile == tile)
        {
            currentSlot2Tile = null;
            Debug.Log($"[AnswerTileSystem] Removed {tile.Value} from Slot2");
        }
        else if (currentSlot3Tile == tile)
        {
            currentSlot3Tile = null;
            Debug.Log($"[AnswerTileSystem] Removed {tile.Value} from Slot3");
        }
        else if (currentSlot4Tile == tile)
        {
            currentSlot4Tile = null;
            Debug.Log($"[AnswerTileSystem] Removed {tile.Value} from Slot4");
        }

        // Return ke pool
        tile.ReturnToOriginalPosition(animationDuration);

        UpdateAnswerDisplay();
    }

    /// <summary>
    /// Update display dan sync ke input field
    /// </summary>
    private void UpdateAnswerDisplay()
    {
        string answer = "";

        if (currentIsMultiStepAnswer)
        {
            // Multi-step: format "step1,step2,step3,step4,step5,step6"
            List<string> stepValues = new List<string>();
            for (int i = 0; i < 6; i++)
            {
                if (multiStepSlotTiles[i] != null)
                {
                    stepValues.Add(multiStepSlotTiles[i].Value);
                }
                else
                {
                    stepValues.Add(""); // Empty slot
                }
            }
            answer = string.Join(",", stepValues);
            Debug.Log($"[AnswerTileSystem] MULTI-STEP Display: {answer}");
        }
        else if (currentIsSingleAnswer)
        {
            // Single answer (Level 3 old mode): just the one value
            answer = currentSlot1Tile != null ? currentSlot1Tile.Value : "";
            Debug.Log($"[AnswerTileSystem] SINGLE ANSWER Display: {answer}");
        }
        else
        {
            string slot1Value = currentSlot1Tile != null ? currentSlot1Tile.Value : "";
            string slot2Value = currentSlot2Tile != null ? currentSlot2Tile.Value : "";
            string slot3Value = currentSlot3Tile != null ? currentSlot3Tile.Value : "";
            string slot4Value = currentSlot4Tile != null ? currentSlot4Tile.Value : "";

            if (currentIsDualQuestion)
            {
                // Dual question: format "num1/den1|num2/den2"
                if (!string.IsNullOrEmpty(slot1Value) && !string.IsNullOrEmpty(slot2Value) &&
                    !string.IsNullOrEmpty(slot3Value) && !string.IsNullOrEmpty(slot4Value))
                {
                    answer = $"{slot1Value}/{slot2Value}|{slot3Value}/{slot4Value}";
                }
                else
                {
                    // Incomplete dual answer
                    answer = $"{slot1Value}/{slot2Value}|{slot3Value}/{slot4Value}";
                }
            }
            else
            {
                // Single question: format "num/den"
                if (!string.IsNullOrEmpty(slot1Value) && !string.IsNullOrEmpty(slot2Value))
                {
                    answer = $"{slot1Value}/{slot2Value}";
                }
                else if (!string.IsNullOrEmpty(slot1Value))
                {
                    answer = $"{slot1Value}/";
                }
            }
        }

        // Sync ke hidden input field
        if (hiddenInputField != null)
        {
            hiddenInputField.text = answer;
        }

        Debug.Log($"[AnswerTileSystem] Answer: '{answer}'");
    }

    /// <summary>
    /// Check apakah jawaban sudah lengkap (semua slot terisi)
    /// </summary>
    public bool IsAnswerComplete()
    {
        if (currentIsMultiStepAnswer)
        {
            // Multi-step (Level 3): All 6 slots must be filled
            return currentMultiStepSlotIndex == 6;
        }
        else if (currentIsSingleAnswer)
        {
            // Single answer (Level 3): hanya 1 slot yang perlu terisi
            return currentSlot1Tile != null;
        }
        else if (currentIsDualQuestion)
        {
            // Dual question: 4 slots harus terisi semua
            return currentSlot1Tile != null && currentSlot2Tile != null &&
                   currentSlot3Tile != null && currentSlot4Tile != null;
        }
        else
        {
            // Single question: 2 slots harus terisi
            return currentSlot1Tile != null && currentSlot2Tile != null;
        }
    }

    /// <summary>
    /// Get current answer
    /// </summary>
    public string GetCurrentAnswer()
    {
        if (!IsAnswerComplete()) return "";

        if (currentIsMultiStepAnswer)
        {
            // Multi-step (Level 3): Return "step1,step2,step3,step4,step5,step6"
            List<string> stepValues = new List<string>();
            for (int i = 0; i < 6; i++)
            {
                stepValues.Add(multiStepSlotTiles[i].Value);
            }
            return string.Join(",", stepValues);
        }
        else if (currentIsSingleAnswer)
        {
            // Level 3: Return single integer value
            return currentSlot1Tile.Value;
        }
        else if (currentIsDualQuestion)
        {
            return $"{currentSlot1Tile.Value}/{currentSlot2Tile.Value}|{currentSlot3Tile.Value}/{currentSlot4Tile.Value}";
        }
        else
        {
            return $"{currentSlot1Tile.Value}/{currentSlot2Tile.Value}";
        }
    }

    /// <summary>
    /// Highlight tiles (correct/wrong)
    /// </summary>
    public void HighlightAnswer(bool correct)
    {
        if (currentSlot1Tile != null) currentSlot1Tile.Highlight(correct);
        if (currentSlot2Tile != null) currentSlot2Tile.Highlight(correct);
    }

    /// <summary>
    /// Reset untuk soal baru
    /// </summary>
    private void ResetSlots()
    {
        if (currentSlot1Tile != null)
        {
            currentSlot1Tile.ReturnToOriginalPosition(0f);
            currentSlot1Tile = null;
        }

        if (currentSlot2Tile != null)
        {
            currentSlot2Tile.ReturnToOriginalPosition(0f);
            currentSlot2Tile = null;
        }

        if (currentSlot3Tile != null)
        {
            currentSlot3Tile.ReturnToOriginalPosition(0f);
            currentSlot3Tile = null;
        }

        if (currentSlot4Tile != null)
        {
            currentSlot4Tile.ReturnToOriginalPosition(0f);
            currentSlot4Tile = null;
        }

        if (hiddenInputField != null)
        {
            hiddenInputField.text = "";
        }
    }

    /// <summary>
    /// Animasi tiles masuk dari offscreen dengan stagger
    /// Tiles dari KIRI, KANAN, dan BAWAH secara bergiliran
    /// </summary>
    private void AnimateTilesIn()
    {
        if (allTiles == null || allTiles.Count == 0) return;

        for (int i = 0; i < allTiles.Count; i++)
        {
            AnswerTile tile = allTiles[i];
            if (tile == null) continue;

            RectTransform rectTransform = tile.GetComponent<RectTransform>();
            if (rectTransform == null) continue;

            // Simpan posisi target
            Vector2 targetPos = rectTransform.anchoredPosition;

            // Tentukan arah masuk berdasarkan index (pattern: kiri, kanan, bawah, repeat)
            Vector2 offscreenPos;
            switch (i % 3)
            {
                case 0: // KIRI
                    offscreenPos = targetPos + Vector2.left * offscreenDistance;
                    break;
                case 1: // KANAN
                    offscreenPos = targetPos + Vector2.right * offscreenDistance;
                    break;
                case 2: // BAWAH
                    offscreenPos = targetPos + Vector2.down * offscreenDistance;
                    break;
                default:
                    offscreenPos = targetPos;
                    break;
            }

            // Set ke posisi offscreen
            rectTransform.anchoredPosition = offscreenPos;

            // Animate ke target position dengan stagger delay
            float delay = i * entryStaggerDelay;
            rectTransform.DOAnchorPos(targetPos, entryAnimDuration)
                .SetDelay(delay)
                .SetEase(entryEase);
        }
    }

    /// <summary>
    /// Animasi tiles keluar ke offscreen (untuk next question)
    /// </summary>
    public void AnimateTilesOut(System.Action onComplete = null)
    {
        if (allTiles == null || allTiles.Count == 0)
        {
            onComplete?.Invoke();
            return;
        }

        int completedCount = 0;
        int totalTiles = allTiles.Count;

        for (int i = 0; i < allTiles.Count; i++)
        {
            AnswerTile tile = allTiles[i];
            if (tile == null) continue;

            RectTransform rectTransform = tile.GetComponent<RectTransform>();
            if (rectTransform == null) continue;

            Vector2 currentPos = rectTransform.anchoredPosition;

            // Keluar ke arah berlawanan dari masuk
            Vector2 exitPos;
            switch (i % 3)
            {
                case 0: // Keluar ke KANAN (masuk dari kiri)
                    exitPos = currentPos + Vector2.right * offscreenDistance;
                    break;
                case 1: // Keluar ke KIRI (masuk dari kanan)
                    exitPos = currentPos + Vector2.left * offscreenDistance;
                    break;
                case 2: // Keluar ke ATAS (masuk dari bawah)
                    exitPos = currentPos + Vector2.up * offscreenDistance;
                    break;
                default:
                    exitPos = currentPos;
                    break;
            }

            // Animate keluar dengan stagger
            float delay = i * (entryStaggerDelay * 0.5f); // Lebih cepat keluar
            rectTransform.DOAnchorPos(exitPos, entryAnimDuration * 0.8f)
                .SetDelay(delay)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    completedCount++;
                    if (completedCount >= totalTiles)
                    {
                        onComplete?.Invoke();
                    }
                });
        }
    }
}
