using UnityEngine;
using System.Collections; // Diperlukan untuk Coroutine (delay)

public class CalculationManager : MonoBehaviour
{
    // Referensi ke skrip lain
    [SerializeField] private UIManagerChapter1 uiManager;
    [SerializeField] private TriangleDataGenerator dataGenerator;
    [SerializeField] private Chapter1EndCutscene endCutscene; // Opsional: untuk cutscene akhir
    [SerializeField] private AnswerTileSystem answerTileSystem; // Reference to answer tile system

    [Header("Status Permainan")]
    private int lives = 3;
    private int progres = 0;
    private int totalSoal = 30; // Update dari 5 ke 30 soal
    private int score = 0; // Tambahan untuk tracking score

    [Header("Gameplay Settings")]
    [SerializeField] private float answerTolerance = 0.01f; // Toleransi untuk jawaban desimal

    private TriangleData dataSoalSaatIni;

    void Start()
    {
        // Inisialisasi permainan
        progres = 0;
        lives = 3;
        uiManager.UpdateLives(lives);
        StartNewRound();
    }

    void StartNewRound()
    {
        if (progres >= totalSoal)
        {
            EndChapter();
            return;
        }

        progres++;
        // Generate soal berdasarkan nomor urut (progressive difficulty)
        dataSoalSaatIni = dataGenerator.GenerateQuestionByNumber(progres);
        uiManager.SetupNewQuestion(progres, totalSoal, dataSoalSaatIni);
    }

    // Fungsi ini akan dihubungkan ke Tombol "CHECK"
    public void VerifyAnswer()
    {
        // 1. Cek apakah answer tile system tersedia
        if (answerTileSystem == null)
        {
            Debug.LogError("[CalculationManager] AnswerTileSystem reference missing!");
            uiManager.ShowFeedback(false, "System error!");
            return;
        }

        // 2. Cek apakah jawaban sudah lengkap (kedua slot terisi)
        if (!answerTileSystem.IsAnswerComplete())
        {
            uiManager.ShowFeedback(false, "Isi kedua slot dengan tile terlebih dahulu!");
            return;
        }

        // 3. Ambil jawaban dari answer tile system (format: "numerator/denominator")
        string answer = answerTileSystem.GetCurrentAnswer();
        Debug.Log($"[CalculationManager] Player answer: {answer}");

        // 4. Parse jawaban pecahan
        string[] parts = answer.Split('/');
        if (parts.Length != 2 ||
            !float.TryParse(parts[0].Trim(), out float numerator) ||
            !float.TryParse(parts[1].Trim(), out float denominator) ||
            denominator == 0)
        {
            Debug.LogError($"[CalculationManager] Invalid answer format: {answer}");
            HandleWrongAnswer("Format jawaban salah!");
            return;
        }

        float playerAnswer = numerator / denominator;

        // 5. Bandingkan jawaban dengan toleransi
        if (Mathf.Abs(playerAnswer - dataSoalSaatIni.JawabanBenar) <= answerTolerance)
        {
            // JAWABAN BENAR
            score += 10;
            answerTileSystem.HighlightAnswer(true); // Highlight hijau
            uiManager.ShowCorrectFeedback("PENGUKURAN TEPAT! +10 Poin.");
            uiManager.HighlightCorrectAnswer(); // Sparkle effect
            StartCoroutine(NextRoundDelay());
        }
        else
        {
            // JAWABAN SALAH
            answerTileSystem.HighlightAnswer(false); // Highlight merah
            HandleWrongAnswer();
        }
    }

    void HandleWrongAnswer(string customMessage = "")
    {
        lives--;
        uiManager.UpdateLives(lives);

        if (lives <= 0)
        {
            // Game Over
            uiManager.ShowFeedback(false, "GAME OVER! Lives habis.");

            // Tampilkan cutscene game over jika ada
            if (endCutscene != null)
            {
                StartCoroutine(ShowGameOverAfterDelay());
            }
        }
        else
        {
            // Masih ada nyawa, ulang soal
            string message = customMessage;
            if (string.IsNullOrEmpty(message))
            {
                message = $"SALAH! Perhatikan rumusnya: {dataSoalSaatIni.SoalDisederhanakan} = {dataSoalSaatIni.JawabanBenar:F2}";
            }

            uiManager.ShowFeedback(false, message);
            uiManager.HighlightWrongAnswer(dataSoalSaatIni.SoalDisederhanakan); // Highlight merah GDD
            StartCoroutine(NextRoundDelay()); // Ganti ke soal baru
        }
    }

    // Coroutine untuk menampilkan game over setelah delay
    IEnumerator ShowGameOverAfterDelay()
    {
        yield return new WaitForSeconds(2.0f);
        endCutscene.ShowGameOver(score);
    }

    // Delay sebelum lanjut ke soal berikutnya
    IEnumerator NextRoundDelay()
    {
        // Beri pemain waktu 1.5 detik untuk membaca feedback
        yield return new WaitForSeconds(1.5f);

        // Animate triangle keluar
        if (uiManager != null && uiManager.triangleVisualizer != null)
        {
            uiManager.triangleVisualizer.AnimateTriangleOut(() =>
            {
                StartNewRound();
            });
        }
        else
        {
            StartNewRound();
        }
    }

    void EndChapter()
    {
        uiManager.ShowFeedback(true, $"CHAPTER 1 SELESAI! Skor Total: {score}");

        // Tampilkan cutscene akhir chapter jika ada
        if (endCutscene != null)
        {
            StartCoroutine(ShowEndCutsceneAfterDelay());
        }
    }

    // Coroutine untuk menampilkan end cutscene setelah delay
    IEnumerator ShowEndCutsceneAfterDelay()
    {
        yield return new WaitForSeconds(2.0f);
        endCutscene.ShowEndCutscene(score, totalSoal);
    }

    // Fungsi publik untuk mendapatkan data soal saat ini (untuk keperluan debugging atau UI)
    public TriangleData GetCurrentQuestionData()
    {
        return dataSoalSaatIni;
    }

    // Fungsi publik untuk mendapatkan score
    public int GetScore()
    {
        return score;
    }
}