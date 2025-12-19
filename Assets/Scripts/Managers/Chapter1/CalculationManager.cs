using UnityEngine;
using System.Collections; // Diperlukan untuk Coroutine (delay)

public class CalculationManager : MonoBehaviour
{
    // Referensi ke skrip lain
    [SerializeField] private UIManagerChapter1 uiManager;
    [SerializeField] private TriangleDataGenerator dataGenerator;
    [SerializeField] private Chapter1EndCutscene endCutscene; // Opsional: untuk cutscene akhir

    [Header("Status Permainan")]
    private int lives = 3;
    private int progres = 0;
    private int totalSoal = 5;
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
        dataSoalSaatIni = dataGenerator.GenerateNewQuestion();
        uiManager.SetupNewQuestion(progres, totalSoal, dataSoalSaatIni);
    }

    // Fungsi ini akan dihubungkan ke Tombol "VERIFIKASI"
    public void VerifyAnswer()
    {
        // 1. Ambil input pemain dari UIManager
        string input = uiManager.jawabanInput.text.Trim();

        // Cek jika input kosong
        if (string.IsNullOrEmpty(input))
        {
            uiManager.ShowFeedback(false, "Masukkan jawaban terlebih dahulu!");
            return;
        }

        // 2. Coba konversi input (support desimal dan pecahan)
        float playerAnswer = 0f;
        bool isValidInput = false;

        // Cek apakah input adalah pecahan (misal: 3/5)
        if (input.Contains("/"))
        {
            string[] parts = input.Split('/');
            if (parts.Length == 2 &&
                float.TryParse(parts[0].Trim(), out float numerator) &&
                float.TryParse(parts[1].Trim(), out float denominator) &&
                denominator != 0)
            {
                playerAnswer = numerator / denominator;
                isValidInput = true;
            }
        }
        else
        {
            // Konversi input ke float (angka desimal)
            // Kita gunakan CultureInfo.InvariantCulture agar tanda titik (.) selalu dikenali
            if (float.TryParse(input.Replace(",", "."),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out playerAnswer))
            {
                isValidInput = true;
            }
        }

        if (isValidInput)
            if (isValidInput)
            {
                // 3. Bandingkan jawaban dengan toleransi
                if (Mathf.Abs(playerAnswer - dataSoalSaatIni.JawabanBenar) <= answerTolerance)
                {
                    // JAWABAN BENAR
                    score += 10;
                    uiManager.ShowCorrectFeedback("PENGUKURAN TEPAT! +10 Poin.");
                    uiManager.HighlightCorrectAnswer(); // Hijau + Sparkle
                    StartCoroutine(NextRoundDelay());
                }
                else
                {
                    // JAWABAN SALAH
                    HandleWrongAnswer();
                }
            }
            else
            {
                // JAWABAN SALAH (Format tidak valid)
                HandleWrongAnswer("Format jawaban salah!");
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
        // Beri pemain waktu 2 detik untuk membaca feedback
        yield return new WaitForSeconds(2.0f);
        StartNewRound();
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