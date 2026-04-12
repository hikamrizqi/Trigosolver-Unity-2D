using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening; // Menggunakan DOTween untuk animasi smooth

/// <summary>
/// Controller untuk animasi karakter yang muncul saat jawaban benar/salah
/// Menampilkan animasi karakter muncul dari bawah, sprite animation, bubble chat, lalu turun kembali
/// </summary>
public class CharacterAnimationController : MonoBehaviour
{
    [Header("Character Setup")]
    [SerializeField] private Image characterImage; // Karakter sprite renderer
    [SerializeField] private RectTransform characterTransform; // Transform untuk animasi gerak

    [Header("Animation Sprites")]
    [SerializeField] private Sprite[] correctAnimationSprites; // 5 sprites untuk animasi jawaban benar
    [SerializeField] private Sprite[] wrongAnimationSprites; // 5 sprites untuk animasi jawaban salah
    [SerializeField] private Sprite[] gameOverAnimationSprites; // 5 sprites untuk animasi game over (marah)
    [SerializeField] private float spriteAnimationSpeed = 0.15f; // Delay antar frame sprite

    [Header("Bubble Chat")]
    [SerializeField] private GameObject bubbleChatPanel; // Panel bubble chat
    [SerializeField] private TextMeshProUGUI bubbleChatText; // Text dalam bubble

    [Header("Animation Settings")]
    [SerializeField] private float moveUpDuration = 0.8f; // Durasi muncul dari bawah
    [SerializeField] private float moveDownDuration = 0.8f; // Durasi turun ke bawah
    [SerializeField] private float displayDuration = 2.5f; // Durasi tampil di tengah
    [SerializeField] private Vector2 hiddenPosition = new Vector2(0, -800f); // Posisi awal (bawah layar)
    [SerializeField] private Vector2 centerPosition = new Vector2(0, 0f); // Posisi tengah layar

    [Header("Random Messages")]
    [Tooltip("Pesan-pesan yang muncul secara random saat jawaban BENAR")]
    [SerializeField]
    private string[] correctMessages = {
        "Hebat! Jawabanmu benar!",
        "Luar biasa! Kamu pintar!",
        "Sempurna! Pertahankan!",
        "Bagus sekali! Terus seperti itu!",
        "Mantap! Kamu memahaminya!"
    };

    [Tooltip("Pesan-pesan yang muncul secara random saat jawaban SALAH")]
    [SerializeField]
    private string[] wrongMessages = {
        "Oops! Coba periksa lagi.",
        "Hmm, belum tepat. Semangat!",
        "Jangan menyerah! Coba lagi.",
        "Hampir! Periksa perhitunganmu.",
        "Yuk, fokus dan coba lagi!"
    };

    [Tooltip("Pesan-pesan yang muncul secara random saat GAME OVER")]
    [SerializeField]
    private string[] gameOverMessages = {
        "Yah, nyawa habis!",
        "Waduh! Game Over.",
        "Semangat! Coba lagi ya!",
        "Jangan menyerah!",
        "Next time pasti lebih baik!"
    };

    private Coroutine currentAnimationCoroutine;
    private bool isAnimating = false;

    private void Start()
    {
        // Setup posisi awal (tersembunyi di bawah)
        if (characterTransform != null)
        {
            characterTransform.anchoredPosition = hiddenPosition;
        }

        // Sembunyikan bubble chat di awal
        if (bubbleChatPanel != null)
        {
            bubbleChatPanel.SetActive(false);
        }

        // Sembunyikan karakter di awal
        if (characterImage != null)
        {
            characterImage.enabled = false;
        }
    }

    /// <summary>
    /// Trigger animasi untuk jawaban BENAR
    /// </summary>
    public void PlayCorrectAnimation(System.Action onComplete = null)
    {
        if (isAnimating)
        {
            Debug.LogWarning("[CharacterAnimation] Animation already playing!");
            return;
        }

        if (currentAnimationCoroutine != null)
        {
            StopCoroutine(currentAnimationCoroutine);
        }

        currentAnimationCoroutine = StartCoroutine(AnimateCharacter(true, onComplete));
    }

    /// <summary>
    /// Trigger animasi untuk jawaban SALAH
    /// </summary>
    public void PlayWrongAnimation(System.Action onComplete = null)
    {
        if (isAnimating)
        {
            Debug.LogWarning("[CharacterAnimation] Animation already playing!");
            return;
        }

        if (currentAnimationCoroutine != null)
        {
            StopCoroutine(currentAnimationCoroutine);
        }

        currentAnimationCoroutine = StartCoroutine(AnimateCharacter(false, onComplete));
    }

    /// <summary>
    /// Trigger animasi untuk GAME OVER (tidak auto-hide)
    /// Karakter akan tetap di tengah layar sampai HideCharacter() dipanggil
    /// </summary>
    public void PlayGameOverAnimation(System.Action onComplete = null)
    {
        if (isAnimating)
        {
            Debug.LogWarning("[CharacterAnimation] Animation already playing!");
            return;
        }

        if (currentAnimationCoroutine != null)
        {
            StopCoroutine(currentAnimationCoroutine);
        }

        currentAnimationCoroutine = StartCoroutine(AnimateGameOver(onComplete));
    }

    /// <summary>
    /// Sembunyikan karakter dengan animasi turun (dipanggil dari tombol kembali)
    /// </summary>
    public void HideCharacter()
    {
        Debug.Log("[CharacterAnimation] HideCharacter called - isAnimating: " + isAnimating);

        // Stop current animation loop if any
        if (currentAnimationCoroutine != null)
        {
            StopCoroutine(currentAnimationCoroutine);
            currentAnimationCoroutine = null;
        }

        // Always start hide animation regardless of current state
        StartCoroutine(HideCharacterCoroutine());
    }

    /// <summary>
    /// Coroutine utama untuk animasi karakter
    /// </summary>
    private IEnumerator AnimateCharacter(bool isCorrect, System.Action onComplete)
    {
        isAnimating = true;

        // 1. Pilih sprite dan pesan yang sesuai
        Sprite[] animSprites = isCorrect ? correctAnimationSprites : wrongAnimationSprites;
        string[] messages = isCorrect ? correctMessages : wrongMessages;

        // Validasi sprites
        if (animSprites == null || animSprites.Length == 0)
        {
            Debug.LogError($"[CharacterAnimation] {(isCorrect ? "Correct" : "Wrong")} animation sprites not assigned!");
            isAnimating = false;
            onComplete?.Invoke();
            yield break;
        }

        // 2. Setup karakter dengan sprite pertama
        characterImage.sprite = animSprites[0];
        characterImage.enabled = true;

        // 3. NAIK: Karakter muncul dari bawah ke tengah
        Debug.Log($"[CharacterAnimation] Moving up from {hiddenPosition} to {centerPosition}");
        characterTransform.anchoredPosition = hiddenPosition;
        characterTransform.DOAnchorPos(centerPosition, moveUpDuration)
            .SetEase(Ease.OutBack); // Ease untuk efek bounce saat muncul

        yield return new WaitForSeconds(moveUpDuration);

        // 4. ANIMASI SPRITE: Loop animasi berjalan
        float spriteAnimationTime = 0f;
        int currentFrame = 0;

        // Tampilkan bubble chat dengan teks random
        if (bubbleChatPanel != null && bubbleChatText != null && messages.Length > 0)
        {
            string randomMessage = messages[Random.Range(0, messages.Length)];
            bubbleChatText.text = randomMessage;
            bubbleChatPanel.SetActive(true);

            // Animasi bubble muncul (scale dari 0 ke 1)
            bubbleChatPanel.transform.localScale = Vector3.zero;
            bubbleChatPanel.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        }

        // Loop animasi sprite selama displayDuration
        while (spriteAnimationTime < displayDuration)
        {
            // Update sprite frame
            currentFrame = (currentFrame + 1) % animSprites.Length;
            characterImage.sprite = animSprites[currentFrame];

            yield return new WaitForSeconds(spriteAnimationSpeed);
            spriteAnimationTime += spriteAnimationSpeed;
        }

        // 5. Sembunyikan bubble chat
        if (bubbleChatPanel != null)
        {
            bubbleChatPanel.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack);
            yield return new WaitForSeconds(0.3f);
            bubbleChatPanel.SetActive(false);
        }

        // 6. TURUN: Karakter turun ke bawah hingga menghilang
        Debug.Log($"[CharacterAnimation] Moving down from {centerPosition} to {hiddenPosition}");
        characterTransform.DOAnchorPos(hiddenPosition, moveDownDuration)
            .SetEase(Ease.InBack);

        yield return new WaitForSeconds(moveDownDuration);

        // 7. Sembunyikan karakter
        characterImage.enabled = false;

        isAnimating = false;
        Debug.Log("[CharacterAnimation] Animation complete!");

        // Callback setelah animasi selesai
        onComplete?.Invoke();
    }

    /// <summary>
    /// Coroutine khusus untuk animasi Game Over (tidak auto-hide)
    /// </summary>
    private IEnumerator AnimateGameOver(System.Action onComplete)
    {
        isAnimating = true;

        // 1. Pilih sprite dan pesan game over
        Sprite[] animSprites = gameOverAnimationSprites;
        string[] messages = gameOverMessages;

        // Validasi sprites
        if (animSprites == null || animSprites.Length == 0)
        {
            Debug.LogError("[CharacterAnimation] Game Over animation sprites not assigned!");
            isAnimating = false;
            onComplete?.Invoke();
            yield break;
        }

        // 2. Setup karakter dengan sprite pertama
        characterImage.sprite = animSprites[0];
        characterImage.enabled = true;

        // 3. NAIK: Karakter muncul dari bawah ke tengah
        Debug.Log($"[CharacterAnimation] Game Over - Moving up from {hiddenPosition} to {centerPosition}");
        characterTransform.anchoredPosition = hiddenPosition;
        characterTransform.DOAnchorPos(centerPosition, moveUpDuration)
            .SetEase(Ease.OutBack);

        yield return new WaitForSeconds(moveUpDuration);

        // 4. Tampilkan bubble chat dengan teks random
        if (bubbleChatPanel != null && bubbleChatText != null && messages.Length > 0)
        {
            string randomMessage = messages[Random.Range(0, messages.Length)];
            bubbleChatText.text = randomMessage;
            bubbleChatPanel.SetActive(true);

            // Animasi bubble muncul
            bubbleChatPanel.transform.localScale = Vector3.zero;
            bubbleChatPanel.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        }

        // 5. LOOP ANIMASI SPRITE TERUS MENERUS (sampai HideCharacter dipanggil)
        int currentFrame = 0;
        while (isAnimating)
        {
            currentFrame = (currentFrame + 1) % animSprites.Length;
            characterImage.sprite = animSprites[currentFrame];
            yield return new WaitForSeconds(spriteAnimationSpeed);
        }

        // Callback setelah animasi muncul selesai (tapi karakter tetap tampil)
        onComplete?.Invoke();
    }

    /// <summary>
    /// Coroutine untuk menyembunyikan karakter dengan animasi
    /// </summary>
    private IEnumerator HideCharacterCoroutine()
    {
        Debug.Log("[CharacterAnimation] Hiding character...");

        // 1. Sembunyikan bubble chat
        if (bubbleChatPanel != null)
        {
            bubbleChatPanel.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack);
            yield return new WaitForSeconds(0.3f);
            bubbleChatPanel.SetActive(false);
        }

        // 2. TURUN: Karakter turun ke bawah hingga menghilang
        Debug.Log($"[CharacterAnimation] Moving down from {centerPosition} to {hiddenPosition}");
        characterTransform.DOAnchorPos(hiddenPosition, moveDownDuration)
            .SetEase(Ease.InBack);

        yield return new WaitForSeconds(moveDownDuration);

        // 3. Sembunyikan karakter
        characterImage.enabled = false;
        isAnimating = false;

        Debug.Log("[CharacterAnimation] Character hidden!");
    }

    /// <summary>
    /// Stop animasi jika sedang berjalan (untuk force stop)
    /// </summary>
    public void StopAnimation()
    {
        if (currentAnimationCoroutine != null)
        {
            StopCoroutine(currentAnimationCoroutine);
            currentAnimationCoroutine = null;
        }

        // Reset ke posisi awal
        if (characterTransform != null)
        {
            characterTransform.anchoredPosition = hiddenPosition;
        }

        if (characterImage != null)
        {
            characterImage.enabled = false;
        }

        if (bubbleChatPanel != null)
        {
            bubbleChatPanel.SetActive(false);
        }

        isAnimating = false;
    }

    /// <summary>
    /// Check apakah animasi sedang berjalan
    /// </summary>
    public bool IsAnimating()
    {
        return isAnimating;
    }
}
