using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Enum untuk tipe pertanyaan
/// </summary>
public enum QuestionType
{
    FindSinValue,       // Cari nilai Sin θ
    FindCosValue,       // Cari nilai Cos θ  
    FindTanValue,       // Cari nilai Tan θ
    FindOpposite,       // Diberikan Sin θ & hypotenuse, cari opposite
    FindAdjacent,       // Diberikan Cos θ & hypotenuse, cari adjacent
    FindHypotenuse,     // Diberikan Sin/Cos θ & sisi, cari hypotenuse
    FindPythagorean     // Diberikan 2 sisi, cari sisi ketiga (Pythagoras)
}

/// <summary>
/// Enum untuk tingkat kesulitan
/// </summary>
public enum DifficultyLevel
{
    Easy,       // Soal 1-10: Segitiga standard, cari rasio trigonometri
    Medium,     // Soal 11-20: Segitiga rotasi, variasi pertanyaan
    Hard        // Soal 21-30: Inverse problems, segitiga rotasi kompleks
}

[System.Serializable]
public class AnswerTileData
{
    public string NumeratorCorrect;     // Pembilang yang benar
    public string DenominatorCorrect;   // Penyebut yang benar
    public List<string> WrongAnswers;   // 3 jawaban salah (distractor)
}

[System.Serializable]
public class TriangleData
{
    // Data segitiga asli (sebelum rotasi visual)
    public int Depan;           // Sisi opposite (tegak)
    public int Samping;         // Sisi adjacent (alas)
    public int Miring;          // Sisi hypotenuse (miring)

    // Rotasi visual segitiga (dalam derajat)
    public float RotationAngle; // 0°, 90°, 180°, 270° untuk variasi visual

    // Pertanyaan & jawaban
    public QuestionType TypeSoal;           // Tipe soal
    public DifficultyLevel Difficulty;      // Tingkat kesulitan
    public string PertanyaanText;           // Text pertanyaan lengkap
    public string SoalDisederhanakan;       // Singkatan soal (untuk kompatibilitas)
    public float JawabanBenar;              // Nilai jawaban yang benar

    // Info tambahan untuk soal yang lebih kompleks
    public string InfoTambahan;             // Informasi yang diberikan (misal: "Sin θ = 0.6")
    public int SisiDiketahui1;              // Nilai sisi 1 yang diketahui (jika applicable)
    public int SisiDiketahui2;              // Nilai sisi 2 yang diketahui (jika applicable)

    // Answer Tile Data (untuk button-based input)
    public AnswerTileData AnswerTileData;   // Data untuk answer tile system
}

public class TriangleDataGenerator : MonoBehaviour
{
    private List<(int, int, int)> triples = new List<(int, int, int)>
    {
        (3, 4, 5),
        (5, 12, 13),
        (8, 15, 17),
        (7, 24, 25),
        (6, 8, 10),     // Multiple of (3,4,5)
        (9, 12, 15),    // Multiple of (3,4,5)
        (12, 16, 20),   // Multiple of (3,4,5)
        (15, 20, 25),   // Multiple of (3,4,5)
        (20, 21, 29),   // New triple
        (9, 40, 41),    // New triple
        (11, 60, 61),   // New triple
        (13, 84, 85)    // New triple
    };

    // Index soal saat ini (untuk progressive difficulty)
    private int currentQuestionIndex = 0;

    /// <summary>
    /// Generate soal berdasarkan nomor urut (1-30)
    /// Menggunakan progressive difficulty
    /// </summary>
    public TriangleData GenerateQuestionByNumber(int questionNumber)
    {
        currentQuestionIndex = questionNumber;

        // Tentukan difficulty berdasarkan nomor soal
        DifficultyLevel difficulty;
        if (questionNumber <= 10)
            difficulty = DifficultyLevel.Easy;
        else if (questionNumber <= 20)
            difficulty = DifficultyLevel.Medium;
        else
            difficulty = DifficultyLevel.Hard;

        // Generate soal sesuai difficulty
        return GenerateQuestionByDifficulty(difficulty, questionNumber);
    }

    /// <summary>
    /// Generate soal random (untuk backward compatibility)
    /// </summary>
    public TriangleData GenerateNewQuestion()
    {
        return GenerateQuestionByNumber(Random.Range(1, 31));
    }

    /// <summary>
    /// Generate soal berdasarkan difficulty level
    /// </summary>
    private TriangleData GenerateQuestionByDifficulty(DifficultyLevel difficulty, int questionNumber)
    {
        TriangleData data = new TriangleData();
        data.Difficulty = difficulty;

        // Pilih triple berdasarkan difficulty
        (int a, int b, int c) triple;
        if (difficulty == DifficultyLevel.Easy)
        {
            // Easy: Gunakan triple sederhana (3,4,5) (5,12,13) (8,15,17)
            triple = triples[questionNumber % 4]; // 4 triple pertama
        }
        else if (difficulty == DifficultyLevel.Medium)
        {
            // Medium: Gunakan triple medium (termasuk multiples)
            triple = triples[(questionNumber - 11) % 8]; // 8 triple pertama
        }
        else
        {
            // Hard: Gunakan semua triple termasuk yang sulit
            triple = triples[(questionNumber - 21) % triples.Count];
        }

        // Random orientasi (a atau b sebagai depan)
        bool isADepan = (questionNumber % 2 == 0);
        data.Depan = isADepan ? triple.a : triple.b;
        data.Samping = isADepan ? triple.b : triple.a;
        data.Miring = triple.c;

        // Tentukan rotasi berdasarkan difficulty
        if (difficulty == DifficultyLevel.Easy)
        {
            data.RotationAngle = 0f; // Tidak ada rotasi
        }
        else if (difficulty == DifficultyLevel.Medium)
        {
            // Rotasi 0° atau 90°
            data.RotationAngle = (questionNumber % 2 == 0) ? 0f : 90f;
        }
        else // Hard
        {
            // Rotasi bervariasi: 0°, 90°, 180°, 270°
            float[] rotations = { 0f, 90f, 180f, 270f };
            data.RotationAngle = rotations[questionNumber % 4];
        }

        // Generate soal berdasarkan difficulty
        GenerateQuestionContent(data, difficulty, questionNumber);

        // Generate answer tile data untuk button-based input
        data.AnswerTileData = GenerateAnswerTileData(data);

        return data;
    }

    /// <summary>
    /// Generate konten pertanyaan berdasarkan difficulty
    /// </summary>
    private void GenerateQuestionContent(TriangleData data, DifficultyLevel difficulty, int questionNumber)
    {
        if (difficulty == DifficultyLevel.Easy)
        {
            // Easy: Hanya cari nilai Sin, Cos, Tan
            GenerateBasicTrigQuestion(data, questionNumber);
        }
        else if (difficulty == DifficultyLevel.Medium)
        {
            // Medium: Mix antara basic dan inverse problems
            if (questionNumber % 3 == 0)
                GenerateInverseTrigQuestion(data);
            else
                GenerateBasicTrigQuestion(data, questionNumber);
        }
        else // Hard
        {
            // Hard: Lebih banyak inverse problems dan Pythagorean
            int variant = questionNumber % 4;
            if (variant == 0)
                GeneratePythagoreanQuestion(data);
            else if (variant == 1)
                GenerateInverseTrigQuestion(data);
            else
                GenerateBasicTrigQuestion(data, questionNumber);
        }
    }

    /// <summary>
    /// Generate pertanyaan dasar: Cari nilai Sin/Cos/Tan θ
    /// </summary>
    private void GenerateBasicTrigQuestion(TriangleData data, int questionNumber)
    {
        int type = questionNumber % 3;

        switch (type)
        {
            case 0: // Sin
                data.TypeSoal = QuestionType.FindSinValue;
                data.SoalDisederhanakan = "Sin\u03B8";
                data.PertanyaanText = "Berapakah nilai Sin\u03B8?";
                data.JawabanBenar = (float)data.Depan / data.Miring;
                data.InfoTambahan = $"Sisi Depan = {data.Depan}, Sisi Miring = {data.Miring}";
                break;

            case 1: // Cos
                data.TypeSoal = QuestionType.FindCosValue;
                data.SoalDisederhanakan = "Cos\u03B8";
                data.PertanyaanText = "Berapakah nilai Cos\u03B8?";
                data.JawabanBenar = (float)data.Samping / data.Miring;
                data.InfoTambahan = $"Sisi Samping = {data.Samping}, Sisi Miring = {data.Miring}";
                break;

            case 2: // Tan
                data.TypeSoal = QuestionType.FindTanValue;
                data.SoalDisederhanakan = "Tan\u03B8";
                data.PertanyaanText = "Berapakah nilai Tan\u03B8?";
                data.JawabanBenar = (float)data.Depan / data.Samping;
                data.InfoTambahan = $"Sisi Depan = {data.Depan}, Sisi Samping = {data.Samping}";
                break;
        }
    }

    /// <summary>
    /// Generate pertanyaan inverse: Diberikan rasio trigonometri, cari sisi
    /// </summary>
    private void GenerateInverseTrigQuestion(TriangleData data)
    {
        int type = Random.Range(0, 3);

        switch (type)
        {
            case 0: // Diberikan Sin θ dan miring, cari depan
                data.TypeSoal = QuestionType.FindOpposite;
                float sinValue = (float)data.Depan / data.Miring;
                data.SoalDisederhanakan = "Sisi Depan";
                data.PertanyaanText = $"Jika Sin\u03B8 = {sinValue:F2} dan sisi miring = {data.Miring}, berapa panjang sisi depan?";
                data.JawabanBenar = data.Depan;
                data.InfoTambahan = $"Sin\u03B8 = {sinValue:F2}";
                data.SisiDiketahui1 = data.Miring;
                break;

            case 1: // Diberikan Cos θ dan miring, cari samping
                data.TypeSoal = QuestionType.FindAdjacent;
                float cosValue = (float)data.Samping / data.Miring;
                data.SoalDisederhanakan = "Sisi Samping";
                data.PertanyaanText = $"Jika Cos\u03B8 = {cosValue:F2} dan sisi miring = {data.Miring}, berapa panjang sisi samping?";
                data.JawabanBenar = data.Samping;
                data.InfoTambahan = $"Cos\u03B8 = {cosValue:F2}";
                data.SisiDiketahui1 = data.Miring;
                break;

            case 2: // Diberikan Sin θ dan depan, cari miring
                data.TypeSoal = QuestionType.FindHypotenuse;
                float sinVal = (float)data.Depan / data.Miring;
                data.SoalDisederhanakan = "Sisi Miring";
                data.PertanyaanText = $"Jika Sin\u03B8 = {sinVal:F2} dan sisi depan = {data.Depan}, berapa panjang sisi miring?";
                data.JawabanBenar = data.Miring;
                data.InfoTambahan = $"Sin\u03B8 = {sinVal:F2}";
                data.SisiDiketahui1 = data.Depan;
                break;
        }
    }

    /// <summary>
    /// Generate pertanyaan Pythagorean: Diberikan 2 sisi, cari sisi ketiga
    /// </summary>
    private void GeneratePythagoreanQuestion(TriangleData data)
    {
        int type = Random.Range(0, 3);

        switch (type)
        {
            case 0: // Diberikan depan & samping, cari miring
                data.TypeSoal = QuestionType.FindPythagorean;
                data.SoalDisederhanakan = "Sisi Miring";
                data.PertanyaanText = $"Jika sisi depan = {data.Depan} dan sisi samping = {data.Samping}, berapa panjang sisi miring?";
                data.JawabanBenar = data.Miring;
                data.InfoTambahan = "Teorema Pythagoras: c² = a² + b²";
                data.SisiDiketahui1 = data.Depan;
                data.SisiDiketahui2 = data.Samping;
                break;

            case 1: // Diberikan depan & miring, cari samping
                data.TypeSoal = QuestionType.FindPythagorean;
                data.SoalDisederhanakan = "Sisi Samping";
                data.PertanyaanText = $"Jika sisi depan = {data.Depan} dan sisi miring = {data.Miring}, berapa panjang sisi samping?";
                data.JawabanBenar = data.Samping;
                data.InfoTambahan = "Teorema Pythagoras: b² = c² - a²";
                data.SisiDiketahui1 = data.Depan;
                data.SisiDiketahui2 = data.Miring;
                break;

            case 2: // Diberikan samping & miring, cari depan
                data.TypeSoal = QuestionType.FindPythagorean;
                data.SoalDisederhanakan = "Sisi Depan";
                data.PertanyaanText = $"Jika sisi samping = {data.Samping} dan sisi miring = {data.Miring}, berapa panjang sisi depan?";
                data.JawabanBenar = data.Depan;
                data.InfoTambahan = "Teorema Pythagoras: a² = c² - b²";
                data.SisiDiketahui1 = data.Samping;
                data.SisiDiketahui2 = data.Miring;
                break;
        }
    }

    /// <summary>
    /// Generate distractor answers untuk Duolingo-style input
    /// Return 3 angka salah untuk pool (total 5: 2 correct + 3 wrong)
    /// </summary>
    public List<string> GenerateDistractors(TriangleData data)
    {
        List<string> distractors = new List<string>();

        // Ambil sisi segitiga sebagai distractor
        List<int> sidesPool = new List<int> { data.Depan, data.Samping, data.Miring };

        // Untuk pecahan, correct answer adalah Depan/Miring atau Samping/Miring
        // Jadi distractor bisa pakai angka sisi yang salah

        // Distractor 1: Sisi yang bukan bagian dari jawaban
        foreach (int side in sidesPool)
        {
            // Check apakah side ini bukan bagian dari correct answer
            string answer = Mathf.RoundToInt(data.JawabanBenar * data.Miring).ToString();
            if (side.ToString() != answer && !distractors.Contains(side.ToString()))
            {
                distractors.Add(side.ToString());
                if (distractors.Count >= 3) break;
            }
        }

        // Distractor 2 & 3: Generate angka random di range yang reasonable
        while (distractors.Count < 3)
        {
            int randomValue = Random.Range(data.Depan - 5, data.Miring + 5);
            if (randomValue <= 0) continue; // Skip non-positive

            string strValue = randomValue.ToString();
            if (!distractors.Contains(strValue) &&
                strValue != data.Depan.ToString() &&
                strValue != data.Samping.ToString() &&
                strValue != data.Miring.ToString())
            {
                distractors.Add(strValue);
            }
        }

        return distractors;
    }

    /// <summary>
    /// Generate answer tile data untuk button-based input (format pecahan)
    /// </summary>
    private AnswerTileData GenerateAnswerTileData(TriangleData data)
    {
        AnswerTileData tileData = new AnswerTileData();

        // Convert jawaban benar ke pecahan (numerator/denominator)
        // Contoh: Sin θ = depan/miring, Cos θ = samping/miring, Tan θ = depan/samping
        switch (data.TypeSoal)
        {
            case QuestionType.FindSinValue:
                tileData.NumeratorCorrect = data.Depan.ToString();
                tileData.DenominatorCorrect = data.Miring.ToString();
                break;

            case QuestionType.FindCosValue:
                tileData.NumeratorCorrect = data.Samping.ToString();
                tileData.DenominatorCorrect = data.Miring.ToString();
                break;

            case QuestionType.FindTanValue:
                tileData.NumeratorCorrect = data.Depan.ToString();
                tileData.DenominatorCorrect = data.Samping.ToString();
                break;

            case QuestionType.FindOpposite:
            case QuestionType.FindAdjacent:
            case QuestionType.FindHypotenuse:
                // Untuk inverse, jawaban adalah integer (sisi segitiga)
                tileData.NumeratorCorrect = ((int)data.JawabanBenar).ToString();
                tileData.DenominatorCorrect = "1"; // Pecahan sederhana x/1
                break;

            default:
                Debug.LogWarning($"Unknown question type: {data.TypeSoal}");
                tileData.NumeratorCorrect = data.Depan.ToString();
                tileData.DenominatorCorrect = data.Miring.ToString();
                break;
        }

        // Generate tiles: 3 angka dari soal + 3 angka random (total 6 tiles)
        tileData.WrongAnswers = new List<string>();

        // Kumpulkan semua angka yang sudah dipakai (termasuk jawaban benar)
        HashSet<string> usedNumbers = new HashSet<string>
        {
            tileData.NumeratorCorrect,
            tileData.DenominatorCorrect
        };

        // Tambahkan 3 angka dari soal (depan, samping, miring) - exclude yang sudah jadi jawaban benar
        List<string> triangleNumbers = new List<string>
        {
            data.Depan.ToString(),
            data.Samping.ToString(),
            data.Miring.ToString()
        };

        foreach (string num in triangleNumbers)
        {
            if (!usedNumbers.Contains(num) && tileData.WrongAnswers.Count < 3)
            {
                tileData.WrongAnswers.Add(num);
                usedNumbers.Add(num);
            }
        }

        // Generate 3 angka random (1-2 digit) yang berbeda
        int attempts = 0;
        while (tileData.WrongAnswers.Count < 6 && attempts < 50) // Total 6 tiles (3 dari soal + 3 random)
        {
            attempts++;

            // Random 1-2 digit (1-20 range)
            int randomNum = Random.Range(1, 21);
            string randomStr = randomNum.ToString();

            // Pastikan unique (belum ada di list)
            if (!usedNumbers.Contains(randomStr))
            {
                tileData.WrongAnswers.Add(randomStr);
                usedNumbers.Add(randomStr);
            }
        }

        Debug.Log($"[TriangleDataGenerator] Generated {tileData.WrongAnswers.Count + 2} tiles (2 correct + {tileData.WrongAnswers.Count} pool)");

        return tileData;
    }
}
