using UnityEngine;
using UnityEngine.UI; // Masih perlu untuk Tombol, Input, Hati
using TMPro;

public class UIManagerChapter1 : MonoBehaviour
{
    [Header("Header Status (UI Canvas)")]
    [SerializeField] private TextMeshProUGUI judulText;
    [SerializeField] private TextMeshProUGUI progresText;
    [SerializeField] private GameObject[] livesIcons;

    [Header("Interaksi & Pertanyaan (UI Canvas)")]
    [SerializeField] private TextMeshProUGUI pertanyaanText;
    // [SerializeField] public TMP_InputField jawabanInput; // DEPRECATED: Now using AnswerTileSystem

    [Header("Umpan Balik (UI Canvas)")]
    [SerializeField] private GameObject feedbackPanel;
    [SerializeField] private TextMeshProUGUI feedbackText;

    [Header("Visualisasi Segitiga (Game World)")]
    [SerializeField] private TriangleVisualizer triangleVisualizer; // Script visualizer untuk menggambar segitiga
    // DEPRECATED: Labels now handled by TriangleVisualizer (World Space)
    // [SerializeField] private TextMeshProUGUI depanLabel_World;
    // [SerializeField] private TextMeshProUGUI sampingLabel_World;
    // [SerializeField] private TextMeshProUGUI miringLabel_World;
    // [SerializeField] private TextMeshPro thetaLabel_World;
    [SerializeField] public SpriteRenderer depanSprite;      // Dulu: depanImg (Image)
    [SerializeField] public SpriteRenderer sampingSprite;    // Dulu: sampingImg (Image)
    [SerializeField] public SpriteRenderer miringSprite;     // Dulu: miringImg (Image)

    [Header("Warna Highlight")]
    public Color defaultColor = Color.white;
    public Color highlightKuning = Color.yellow;
    public Color highlightMerah = Color.red;
    public Color highlightHijau = Color.green;

    [Header("Efek Visual")]
    [SerializeField] private ParticleSystem sparkleEffect; // Opsional: untuk efek sparkle
    [SerializeField] private float highlightDuration = 1.5f;

    [Header("Audio Manager")]
    [SerializeField] private Chapter1AudioManager audioManager; // Opsional: untuk sound effects

    [Header("Answer Tile System")]
    [SerializeField] private AnswerTileSystem answerTileSystem; // Reference to answer tile spawner

    // Fungsi ini dimodifikasi untuk menargetkan objek World Space
    public void SetupNewQuestion(int progres, int totalSoal, TriangleData data)
    {
        // Update UI Canvas
        progresText.text = $"Soal: {progres}/{totalSoal}";

        // Debug log untuk cek nilai
        Debug.Log($"SoalDisederhanakan: '{data.SoalDisederhanakan}' (length: {data.SoalDisederhanakan.Length})");
        for (int i = 0; i < data.SoalDisederhanakan.Length; i++)
        {
            Debug.Log($"  Char[{i}]: '{data.SoalDisederhanakan[i]}' (code: {(int)data.SoalDisederhanakan[i]})");
        }

        // Force clear dan update dengan delay untuk refresh rendering
        pertanyaanText.text = "";
        pertanyaanText.ForceMeshUpdate();

        // Gunakan PertanyaanText yang lebih deskriptif (bukan hanya "Berapakah nilai X?")
        pertanyaanText.text = data.PertanyaanText; // Text pertanyaan lengkap dari generator
        pertanyaanText.ForceMeshUpdate();

        Debug.Log($"pertanyaanText.text setelah set: '{pertanyaanText.text}'");

        // Tampilkan info tambahan jika ada (untuk soal yang lebih kompleks)
        if (!string.IsNullOrEmpty(data.InfoTambahan))
        {
            Debug.Log($"Info Tambahan: {data.InfoTambahan}");
            // Anda bisa tampilkan di UI jika ada label khusus untuk hints
        }

        // jawabanInput.text = ""; // DEPRECATED: Now using AnswerTileSystem
        feedbackPanel.SetActive(false);

        // Labels now handled by TriangleVisualizer automatically
        // if (depanLabel_World != null) depanLabel_World.text = data.Depan.ToString();
        // if (sampingLabel_World != null) sampingLabel_World.text = data.Samping.ToString();
        // if (miringLabel_World != null) miringLabel_World.text = data.Miring.ToString();


        // Gunakan TriangleVisualizer untuk menggambar segitiga DENGAN ROTASI DAN ORIENTATION
        if (triangleVisualizer != null)
        {
            triangleVisualizer.DrawTriangle(data.Depan, data.Samping, data.Miring, data.RotationAngle, data.Orientation);

            // Log difficulty, rotation, dan orientation untuk debugging
            Debug.Log($"[Chapter1] Soal #{progres}/{totalSoal} | Difficulty: {data.Difficulty} | Rotation: {data.RotationAngle}° | Orientation: {data.Orientation}");
        }
        else
        {
            // Fallback: reset warna jika visualizer tidak ada
            ResetSideColors();
        }

        // Setup answer tiles dengan jawaban benar dan distractor
        if (answerTileSystem != null && data.AnswerTileData != null)
        {
            answerTileSystem.SetupQuestion(
                data.AnswerTileData.NumeratorCorrect,
                data.AnswerTileData.DenominatorCorrect,
                data.AnswerTileData.WrongAnswers
            );
            Debug.Log($"[UIManager] Answer tiles spawned for question {progres}");
        }
        else
        {
            if (answerTileSystem == null)
                Debug.LogError("[UIManager] AnswerTileSystem reference is missing!");
            if (data.AnswerTileData == null)
                Debug.LogError("[UIManager] TriangleData.AnswerTileData is null!");
        }
    }

    // Fungsi ini dimodifikasi untuk menerima SpriteRenderer atau gunakan triangleVisualizer
    public void HighlightSide(SpriteRenderer sideSprite)
    {
        if (triangleVisualizer != null)
        {
            // Tentukan nama sisi berdasarkan sprite
            string sideName = "";
            if (sideSprite == depanSprite) sideName = "depan";
            else if (sideSprite == sampingSprite) sideName = "samping";
            else if (sideSprite == miringSprite) sideName = "miring";

            triangleVisualizer.HighlightSide(sideName);
        }
        else
        {
            // Fallback manual
            ResetSideColors();
            sideSprite.color = highlightKuning;
        }
    }

    public void ResetSideColors()
    {
        if (triangleVisualizer != null)
        {
            triangleVisualizer.ResetColors();
        }
        else
        {
            depanSprite.color = defaultColor;
            sampingSprite.color = defaultColor;
            miringSprite.color = defaultColor;
        }
    }

    // Fungsi ini dimodifikasi untuk menargetkan SpriteRenderer
    public void HighlightWrongAnswer(string soalType)
    {
        if (triangleVisualizer != null)
        {
            triangleVisualizer.ResetColors();
            Color wrongColor = highlightMerah;

            if (soalType == "Sinθ")
            {
                triangleVisualizer.HighlightSide("depan", wrongColor);
                triangleVisualizer.HighlightSide("miring", wrongColor);
            }
            else if (soalType == "Cosθ")
            {
                triangleVisualizer.HighlightSide("samping", wrongColor);
                triangleVisualizer.HighlightSide("miring", wrongColor);
            }
            else if (soalType == "Tanθ")
            {
                triangleVisualizer.HighlightSide("depan", wrongColor);
                triangleVisualizer.HighlightSide("samping", wrongColor);
            }
        }
        else
        {
            // Fallback manual
            ResetSideColors();
            if (soalType == "Sinθ")
            {
                depanSprite.color = highlightMerah;
                miringSprite.color = highlightMerah;
            }
            else if (soalType == "Cosθ")
            {
                sampingSprite.color = highlightMerah;
                miringSprite.color = highlightMerah;
            }
            else if (soalType == "Tanθ")
            {
                depanSprite.color = highlightMerah;
                sampingSprite.color = highlightMerah;
            }
        }
    }

    // Fungsi-fungsi ini tidak perlu diubah
    public void UpdateLives(int currentLives)
    {
        for (int i = 0; i < livesIcons.Length; i++)
        {
            livesIcons[i].SetActive(i < currentLives);
        }
    }

    public void ShowFeedback(bool isCorrect, string message)
    {
        feedbackPanel.SetActive(true);
        feedbackText.text = message;
        feedbackText.color = isCorrect ? highlightHijau : highlightMerah;
    }

    // Fungsi khusus untuk feedback jawaban benar dengan efek
    public void ShowCorrectFeedback(string message)
    {
        ShowFeedback(true, message);
    }

    // Highlight semua sisi segitiga menjadi hijau untuk jawaban benar
    public void HighlightCorrectAnswer()
    {
        if (triangleVisualizer != null)
        {
            triangleVisualizer.HighlightSide("depan", highlightHijau);
            triangleVisualizer.HighlightSide("samping", highlightHijau);
            triangleVisualizer.HighlightSide("miring", highlightHijau);
        }
        else
        {
            depanSprite.color = highlightHijau;
            sampingSprite.color = highlightHijau;
            miringSprite.color = highlightHijau;
        }

        // Aktifkan efek sparkle jika ada
        if (sparkleEffect != null)
        {
            sparkleEffect.Play();
        }
    }

    // Fungsi untuk tombol interaktif Depan, Samping, Miring
    public void OnDepanButtonClicked()
    {
        HighlightSide(depanSprite);
    }

    public void OnSampingButtonClicked()
    {
        HighlightSide(sampingSprite);
    }

    public void OnMiringButtonClicked()
    {
        HighlightSide(miringSprite);
    }
}