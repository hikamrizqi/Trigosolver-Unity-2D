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

    private bool gameStarted = false; // Track if game has been started by level selection

    void Start()
    {
        // DON'T auto-start game - wait for level selection
        // Inisialisasi lives saja
        lives = 3;
        uiManager.UpdateLives(lives);
    }

    /// <summary>
    /// Start game from specific question number (called by LevelSelectionManager)
    /// </summary>
    public void StartFromQuestion(int questionNumber)
    {
        if (gameStarted)
        {
            Debug.LogWarning("[CalculationManager] Game already started!");
            return;
        }

        gameStarted = true;
        progres = questionNumber - 1; // Set to question before target (will increment in StartNewRound)
        lives = 3;
        score = 0;
        uiManager.UpdateLives(lives);

        Debug.Log($"[CalculationManager] Starting from question {questionNumber}");
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

        // 2. Cek apakah jawaban sudah lengkap (semua slot terisi)
        if (!answerTileSystem.IsAnswerComplete())
        {
            string message = dataSoalSaatIni.IsDualQuestion ?
                "Isi keempat slot dengan tile terlebih dahulu!" :
                "Isi kedua slot dengan tile terlebih dahulu!";
            uiManager.ShowFeedback(false, message);
            return;
        }

        // 3. Ambil jawaban dari answer tile system
        string answer = answerTileSystem.GetCurrentAnswer();
        Debug.Log($"[CalculationManager] Player answer: {answer}");

        bool isCorrect;

        if (dataSoalSaatIni.IsDualQuestion)
        {
            // DUAL QUESTION: Verifikasi 4 nilai (format: "num1/den1|num2/den2")
            string[] fractions = answer.Split('|');
            if (fractions.Length != 2)
            {
                Debug.LogError($"[CalculationManager] Invalid dual answer format: {answer}");
                HandleWrongAnswer("Format jawaban salah!");
                return;
            }

            // Parse fraction 1
            string[] parts1 = fractions[0].Split('/');
            if (parts1.Length != 2 ||
                !float.TryParse(parts1[0].Trim(), out float num1) ||
                !float.TryParse(parts1[1].Trim(), out float den1) ||
                den1 == 0)
            {
                Debug.LogError($"[CalculationManager] Invalid fraction 1 format: {fractions[0]}");
                HandleWrongAnswer("Format jawaban salah!");
                return;
            }

            // Parse fraction 2
            string[] parts2 = fractions[1].Split('/');
            if (parts2.Length != 2 ||
                !float.TryParse(parts2[0].Trim(), out float num2) ||
                !float.TryParse(parts2[1].Trim(), out float den2) ||
                den2 == 0)
            {
                Debug.LogError($"[CalculationManager] Invalid fraction 2 format: {fractions[1]}");
                HandleWrongAnswer("Format jawaban salah!");
                return;
            }

            float playerAnswer1 = num1 / den1;
            float playerAnswer2 = num2 / den2;

            // Kedua jawaban harus benar
            bool answer1Correct = Mathf.Abs(playerAnswer1 - dataSoalSaatIni.JawabanBenar) <= answerTolerance;
            bool answer2Correct = Mathf.Abs(playerAnswer2 - dataSoalSaatIni.JawabanBenar2) <= answerTolerance;

            isCorrect = answer1Correct && answer2Correct;

            Debug.Log($"[CalculationManager] Dual Answer Check - Answer1: {playerAnswer1:F3} vs {dataSoalSaatIni.JawabanBenar:F3} ({answer1Correct}), Answer2: {playerAnswer2:F3} vs {dataSoalSaatIni.JawabanBenar2:F3} ({answer2Correct})");
        }
        else
        {
            // SINGLE QUESTION: Verifikasi 2 nilai (format: "numerator/denominator")
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
            isCorrect = Mathf.Abs(playerAnswer - dataSoalSaatIni.JawabanBenar) <= answerTolerance;

            Debug.Log($"[CalculationManager] Single Answer Check - Player: {playerAnswer:F3} vs Correct: {dataSoalSaatIni.JawabanBenar:F3} ({isCorrect})");
        }

        // 4. Handle hasil verifikasi
        if (isCorrect)
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

        // Flag untuk tracking animasi selesai
        bool triangleAnimDone = false;
        bool tilesAnimDone = false;

        // Animate triangle keluar
        if (uiManager != null && uiManager.triangleVisualizer != null)
        {
            uiManager.triangleVisualizer.AnimateTriangleOut(() =>
            {
                triangleAnimDone = true;
                if (tilesAnimDone) StartNewRound();
            });
        }
        else
        {
            triangleAnimDone = true;
        }

        // Animate answer tiles keluar (parallel)
        if (AnswerTileSystem.Instance != null)
        {
            AnswerTileSystem.Instance.AnimateTilesOut(() =>
            {
                tilesAnimDone = true;
                if (triangleAnimDone) StartNewRound();
            });
        }
        else
        {
            tilesAnimDone = true;
        }

        // Fallback jika tidak ada animator
        if (triangleAnimDone && tilesAnimDone)
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