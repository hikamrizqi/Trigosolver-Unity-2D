using UnityEngine;
using System.Collections; // Diperlukan untuk Coroutine (delay)

/// <summary>
/// Calculation Manager untuk verifikasi jawaban dan game logic Chapter 1
/// </summary>
public class CalculationManager : MonoBehaviour
{
    // Referensi ke skrip lain
    [SerializeField] private UIManagerChapter1 uiManager;
    [SerializeField] private TriangleDataGenerator dataGenerator;
    [SerializeField] private Chapter1EndCutscene endCutscene; // Opsional: untuk cutscene akhir
    [SerializeField] private AnswerTileSystem answerTileSystem; // Reference to answer tile system
    [SerializeField] private LevelSelectionManager levelSelectionManager; // Reference to level selection manager

    [Header("Status Permainan")]
    private int lives = 3;
    private int progres = 0;
    private int totalSoal = 30; // Update dari 5 ke 30 soal
    private int score = 0; // Tambahan untuk tracking score
    private int startingQuestion = 1; // Track which level was selected (1 or 11)

    [Header("Gameplay Settings")]
    [SerializeField] private float answerTolerance = 0.01f; // Toleransi untuk jawaban desimal

    private TriangleData dataSoalSaatIni; // Data soal yang sedang aktif

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
        startingQuestion = questionNumber; // Save which level player started from
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
            string message;
            if (dataSoalSaatIni.AnswerTileData != null && dataSoalSaatIni.AnswerTileData.IsMultiStepAnswer)
            {
                message = "Isi semua 6 slot dengan tile terlebih dahulu!";
            }
            else if (dataSoalSaatIni.AnswerTileData != null && dataSoalSaatIni.AnswerTileData.IsSingleAnswer)
            {
                message = "Pilih salah satu jawaban terlebih dahulu!";
            }
            else if (dataSoalSaatIni.IsDualQuestion)
            {
                message = "Isi keempat slot dengan tile terlebih dahulu!";
            }
            else
            {
                message = "Isi kedua slot dengan tile terlebih dahulu!";
            }
            uiManager.ShowFeedback(false, message);
            return;
        }

        // 3. Ambil jawaban dari answer tile system
        string answer = answerTileSystem.GetCurrentAnswer();
        Debug.Log($"[CalculationManager] Player answer: {answer}");

        bool isCorrect;

        // Check if this is Level 3 Multi-Step mode
        if (dataSoalSaatIni.AnswerTileData != null && dataSoalSaatIni.AnswerTileData.IsMultiStepAnswer)
        {
            // LEVEL 3 MULTI-STEP: Verify all 6 steps (format: "step1,step2,step3,step4,step5,step6")
            string[] steps = answer.Split(',');
            if (steps.Length != 6)
            {
                Debug.LogError($"[CalculationManager] Invalid multi-step answer format: {answer}");
                HandleWrongAnswer("Format jawaban salah!");
                return;
            }

            // Verify each step matches expected value (check default order)
            isCorrect = true;
            for (int i = 0; i < 6; i++)
            {
                string playerStep = steps[i].Trim();
                string correctStep = dataSoalSaatIni.AnswerTileData.MultiStepCorrectAnswers[i];

                if (playerStep != correctStep)
                {
                    isCorrect = false;
                    Debug.Log($"[CalculationManager] Multi-Step: Step {i + 1} WRONG in default order - Player: {playerStep} vs Correct: {correctStep}");
                    break;
                }
                else
                {
                    Debug.Log($"[CalculationManager] Multi-Step: Step {i + 1} CORRECT - {playerStep}");
                }
            }

            // Jika salah dan ada alternative answers (untuk operator +), cek urutan alternatif
            if (!isCorrect && dataSoalSaatIni.AnswerTileData.MultiStepAlternativeAnswers != null &&
                dataSoalSaatIni.AnswerTileData.MultiStepAlternativeAnswers.Count == 6)
            {
                Debug.Log($"[CalculationManager] Checking alternative order (swapped)...");
                isCorrect = true;
                for (int i = 0; i < 6; i++)
                {
                    string playerStep = steps[i].Trim();
                    string altStep = dataSoalSaatIni.AnswerTileData.MultiStepAlternativeAnswers[i];

                    if (playerStep != altStep)
                    {
                        isCorrect = false;
                        Debug.Log($"[CalculationManager] Multi-Step: Step {i + 1} WRONG in alternative order - Player: {playerStep} vs Alt: {altStep}");
                        break;
                    }
                    else
                    {
                        Debug.Log($"[CalculationManager] Multi-Step: Step {i + 1} CORRECT (alternative) - {playerStep}");
                    }
                }
            }

            Debug.Log($"[CalculationManager] Multi-Step Answer (Level 3) Check - Result: {isCorrect}");
        }
        // Check if this is Level 3 (single answer mode - OLD)
        else if (dataSoalSaatIni.AnswerTileData != null && dataSoalSaatIni.AnswerTileData.IsSingleAnswer)
        {
            // LEVEL 3: Single answer integer verification
            if (!int.TryParse(answer.Trim(), out int playerAnswer))
            {
                Debug.LogError($"[CalculationManager] Invalid single answer format: {answer}");
                HandleWrongAnswer("Format jawaban salah!");
                return;
            }

            // Compare integer answer with correct answer (exact match)
            int correctAnswer = (int)dataSoalSaatIni.JawabanBenar;
            isCorrect = (playerAnswer == correctAnswer);

            Debug.Log($"[CalculationManager] Single Answer (Level 3) Check - Player: {playerAnswer} vs Correct: {correctAnswer} ({isCorrect})");
        }
        else if (dataSoalSaatIni.IsDualQuestion)
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
        // Save high score based on which level was played
        SaveLevelScore();

        uiManager.ShowFeedback(true, $"CHAPTER 1 SELESAI! Skor Total: {score}");

        // Tampilkan cutscene akhir chapter jika ada
        if (endCutscene != null)
        {
            StartCoroutine(ShowEndCutsceneAfterDelay());
        }
    }

    /// <summary>
    /// Save score to HighScoreManager based on level played
    /// </summary>
    private void SaveLevelScore()
    {
        if (startingQuestion == 1)
        {
            // Level 1 (Soal 1-10)
            HighScoreManager.Instance.SaveLevel1Score(score);
            Debug.Log($"[Score] Level 1 completed with score: {score}");
        }
        else if (startingQuestion == 11)
        {
            // Level 2 (Soal 11-20)
            HighScoreManager.Instance.SaveLevel2Score(score);
            Debug.Log($"[Score] Level 2 completed with score: {score}");
        }
        else if (startingQuestion == 21)
        {
            // Level 3 (Soal 21-30)
            HighScoreManager.Instance.SaveLevel3Score(score);
            Debug.Log($"[Score] Level 3 completed with score: {score}");
        }

        // If player completes entire chapter (30 questions), save total score
        if (progres >= 30)
        {
            int totalScore = HighScoreManager.Instance.GetLevel1HighScore() +
                           HighScoreManager.Instance.GetLevel2HighScore();
            HighScoreManager.Instance.SaveTotalScore(totalScore);
            Debug.Log($"[Score] Total Chapter 1 score: {totalScore}");
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

    /// <summary>
    /// Kembali ke pemilihan level (dipanggil dari tombol BACK)
    /// </summary>
    public void BackToLevelSelection()
    {
        Debug.Log("[CalculationManager] Kembali ke pemilihan level");

        // Stop game
        gameStarted = false;

        // Show level selection panel lagi
        if (levelSelectionManager != null)
        {
            levelSelectionManager.ShowLevelSelection();
        }
        else
        {
            Debug.LogError("[CalculationManager] LevelSelectionManager reference missing!");
        }
    }
}