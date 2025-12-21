using UnityEngine;
using UnityEngine.UI;
using TMPro;
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

    [Header("Answer Slots (Tempat Jawaban)")]
    [SerializeField] private Transform slot1Transform;  // Slot kiri (sebelum /)
    [SerializeField] private Transform slot2Transform;  // Slot kanan (setelah /)
    [SerializeField] private TextMeshProUGUI slashText; // "/" text di tengah

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

    // Jawaban yang benar untuk soal ini
    private string correctNumerator;   // Angka atas (15)
    private string correctDenominator; // Angka bawah (17)

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
    /// Setup soal baru dengan jawaban benar dan distractor
    /// </summary>
    public void SetupQuestion(string numerator, string denominator, List<string> wrongAnswers)
    {
        // Simpan jawaban benar
        correctNumerator = numerator;
        correctDenominator = denominator;

        // Reset slots
        ResetSlots();

        // Destroy tiles lama
        foreach (var tile in allTiles)
        {
            if (tile != null) Destroy(tile.gameObject);
        }
        allTiles.Clear();

        // Buat pool: 2 correct + wrong answers (total 6-8 tiles, acak)
        List<string> poolValues = new List<string> { numerator, denominator };
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

        Debug.Log($"[AnswerTileSystem] Setup question: {numerator}/{denominator}, Total tiles: {poolValues.Count}, Pool: {string.Join(", ", poolValues)}");
        
        // Animate tiles masuk dari offscreen
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

        // Tentukan slot target (prioritas kiri)
        Transform targetSlot;
        if (currentSlot1Tile == null)
        {
            // Slot 1 kosong → Isi slot 1
            currentSlot1Tile = tile;
            targetSlot = slot1Transform;
            Debug.Log($"[AnswerTileSystem] Moving {tile.Value} to Slot1");
        }
        else if (currentSlot2Tile == null)
        {
            // Slot 1 terisi, slot 2 kosong → Isi slot 2
            currentSlot2Tile = tile;
            targetSlot = slot2Transform;
            Debug.Log($"[AnswerTileSystem] Moving {tile.Value} to Slot2");
        }
        else
        {
            // Kedua slot penuh
            Debug.LogWarning($"[AnswerTileSystem] Both slots full! Cannot move {tile.Value}");
            return;
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

        // Hapus dari slot
        if (currentSlot1Tile == tile)
        {
            currentSlot1Tile = null;
            Debug.Log($"[AnswerTileSystem] Removed {tile.Value} from Slot1");

            // Auto-shift: Jika slot2 ada isi, geser ke kiri
            if (currentSlot2Tile != null)
            {
                AnswerTile tileToShift = currentSlot2Tile;
                currentSlot2Tile = null;

                // Geser ke slot1
                currentSlot1Tile = tileToShift;
                tileToShift.AnimateToPosition(Vector3.zero, slot1Transform, true, animationDuration);
                Debug.Log($"[AnswerTileSystem] Auto-shifted {tileToShift.Value} from Slot2 to Slot1");
            }
        }
        else if (currentSlot2Tile == tile)
        {
            currentSlot2Tile = null;
            Debug.Log($"[AnswerTileSystem] Removed {tile.Value} from Slot2");
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
        string slot1Value = currentSlot1Tile != null ? currentSlot1Tile.Value : "";
        string slot2Value = currentSlot2Tile != null ? currentSlot2Tile.Value : "";

        string answer = "";
        if (!string.IsNullOrEmpty(slot1Value) && !string.IsNullOrEmpty(slot2Value))
        {
            answer = $"{slot1Value}/{slot2Value}";
        }
        else if (!string.IsNullOrEmpty(slot1Value))
        {
            answer = $"{slot1Value}/";
        }

        // Sync ke hidden input field
        if (hiddenInputField != null)
        {
            hiddenInputField.text = answer;
        }

        Debug.Log($"[AnswerTileSystem] Answer: '{answer}'");
    }

    /// <summary>
    /// Check apakah jawaban sudah lengkap (kedua slot terisi)
    /// </summary>
    public bool IsAnswerComplete()
    {
        return currentSlot1Tile != null && currentSlot2Tile != null;
    }

    /// <summary>
    /// Get current answer
    /// </summary>
    public string GetCurrentAnswer()
    {
        if (!IsAnswerComplete()) return "";
        return $"{currentSlot1Tile.Value}/{currentSlot2Tile.Value}";
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
                .OnComplete(() => {
                    completedCount++;
                    if (completedCount >= totalTiles)
                    {
                        onComplete?.Invoke();
                    }
                });
        }
    }
}
