using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Manager untuk cutscene akhir Chapter 1
/// Menampilkan summary dan transisi ke chapter berikutnya
/// </summary>
public class Chapter1EndCutscene : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject cutscenePanel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button retryButton;

    [Header("Badge/Achievement")]
    [SerializeField] private Image badgeImage;
    [SerializeField] private Sprite bronzeBadge;
    [SerializeField] private Sprite silverBadge;
    [SerializeField] private Sprite goldBadge;

    [Header("Scene Management")]
    [SerializeField] private string nextSceneName = "Chapter2_Scene";
    [SerializeField] private string currentSceneName = "Chapter1_Scene";

    [Header("Animation")]
    [SerializeField] private Animator cutsceneAnimator;

    private void Start()
    {
        // Setup button listeners
        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinueClicked);

        if (retryButton != null)
            retryButton.onClick.AddListener(OnRetryClicked);

        // Hide cutscene panel di awal
        if (cutscenePanel != null)
            cutscenePanel.SetActive(false);
    }

    /// <summary>
    /// Tampilkan cutscene akhir chapter dengan score
    /// Dipanggil dari CalculationManager.EndChapter()
    /// </summary>
    public void ShowEndCutscene(int finalScore, int totalQuestions)
    {
        if (cutscenePanel != null)
            cutscenePanel.SetActive(true);

        // Hitung persentase
        int maxScore = totalQuestions * 10; // Asumsi setiap soal = 10 poin
        float percentage = (float)finalScore / maxScore * 100f;

        // Update teks
        if (titleText != null)
            titleText.text = "CHAPTER 1 SELESAI!";

        if (scoreText != null)
            scoreText.text = $"Skor: {finalScore}/{maxScore} ({percentage:F1}%)";

        // Tentukan badge berdasarkan persentase
        DetermineBadge(percentage);

        // Tentukan message
        if (messageText != null)
        {
            if (percentage >= 90f)
                messageText.text = "LUAR BIASA! Kamu menguasai dasar-dasar trigonometri!\n✨ Lencana Emas";
            else if (percentage >= 70f)
                messageText.text = "BAGUS! Kamu memahami konsep dengan baik!\n⭐ Lencana Perak";
            else if (percentage >= 50f)
                messageText.text = "CUKUP BAIK! Terus berlatih untuk hasil lebih baik!\n🥉 Lencana Perunggu";
            else
                messageText.text = "Masih perlu belajar lagi. Jangan menyerah!\n💪 Coba Lagi?";
        }

        // Mainkan animasi jika ada
        if (cutsceneAnimator != null)
            cutsceneAnimator.SetTrigger("ShowCutscene");

        // Auto-enable continue button jika score cukup
        if (continueButton != null)
            continueButton.interactable = (percentage >= 50f);
    }

    private void DetermineBadge(float percentage)
    {
        if (badgeImage == null) return;

        if (percentage >= 90f && goldBadge != null)
        {
            badgeImage.sprite = goldBadge;
            badgeImage.color = Color.yellow;
        }
        else if (percentage >= 70f && silverBadge != null)
        {
            badgeImage.sprite = silverBadge;
            badgeImage.color = Color.white;
        }
        else if (percentage >= 50f && bronzeBadge != null)
        {
            badgeImage.sprite = bronzeBadge;
            badgeImage.color = new Color(0.8f, 0.5f, 0.2f); // Bronze color
        }
        else
        {
            badgeImage.gameObject.SetActive(false);
        }
    }

    private void OnContinueClicked()
    {
        // Load scene berikutnya
        StartCoroutine(LoadNextScene());
    }

    private void OnRetryClicked()
    {
        // Reload scene saat ini
        SceneManager.LoadScene(currentSceneName);
    }

    private IEnumerator LoadNextScene()
    {
        // Opsional: Tambahkan fade out effect
        yield return new WaitForSeconds(0.5f);

        // Load scene berikutnya
        SceneManager.LoadScene(nextSceneName);
    }

    /// <summary>
    /// Alternatif: Tampilkan Game Over screen
    /// </summary>
    public void ShowGameOver(int finalScore)
    {
        if (cutscenePanel != null)
            cutscenePanel.SetActive(true);

        if (titleText != null)
            titleText.text = "GAME OVER";

        if (scoreText != null)
            scoreText.text = $"Skor Akhir: {finalScore}";

        if (messageText != null)
            messageText.text = "Lives habis! Jangan khawatir, belajar dari kesalahan adalah kunci sukses.\n\nCoba lagi?";

        if (badgeImage != null)
            badgeImage.gameObject.SetActive(false);

        // Hanya tampilkan tombol retry
        if (continueButton != null)
            continueButton.gameObject.SetActive(false);

        if (retryButton != null)
            retryButton.gameObject.SetActive(true);
    }
}
