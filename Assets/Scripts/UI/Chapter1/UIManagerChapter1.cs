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
    [SerializeField] public TriangleVisualizer triangleVisualizer; // Script visualizer untuk menggambar segitiga
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

    [Header("Interactive Buttons (Soal 1-10 vs 11-20 vs 21-30)")]
    [Tooltip("Button references untuk ganti image berdasarkan level")]
    [SerializeField] private UnityEngine.UI.Button depanButton;
    [SerializeField] private UnityEngine.UI.Button sampingButton;
    [SerializeField] private UnityEngine.UI.Button miringButton;

    [Header("Button Images - Level 1 (Soal 1-10)")]
    [Tooltip("Image untuk button DEPAN (Level 1)")]
    [SerializeField] private Sprite depanImageLevel1;
    [Tooltip("Image untuk button SAMPING (Level 1)")]
    [SerializeField] private Sprite sampingImageLevel1;
    [Tooltip("Image untuk button MIRING (Level 1)")]
    [SerializeField] private Sprite miringImageLevel1;

    [Header("Button Images - Level 2 (Soal 11-20: AB/BC/AC)")]
    [Tooltip("Image untuk button AB (Level 2)")]
    [SerializeField] private Sprite abImageLevel2;
    [Tooltip("Image untuk button BC (Level 2)")]
    [SerializeField] private Sprite bcImageLevel2;
    [Tooltip("Image untuk button AC (Level 2)")]
    [SerializeField] private Sprite acImageLevel2;

    [Header("Button Images - Level 3 (Soal 21-30: AB/BC/AC)")]
    [Tooltip("Image untuk button AB (Level 3)")]
    [SerializeField] private Sprite abImageLevel3;
    [Tooltip("Image untuk button BC (Level 3)")]
    [SerializeField] private Sprite bcImageLevel3;
    [Tooltip("Image untuk button AC (Level 3)")]
    [SerializeField] private Sprite acImageLevel3;

    // Track question type untuk conditional UI
    private bool currentIsDualQuestion = false;

    // Fungsi ini dimodifikasi untuk menargetkan objek World Space
    public void SetupNewQuestion(int progres, int totalSoal, TriangleData data)
    {
        // Track question type untuk conditional UI
        currentIsDualQuestion = data.IsDualQuestion;

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
            // Detect if Level 3 (Pythagoras questions) to show vertex labels A, B, C
            bool isLevel3 = data.AnswerTileData != null && data.AnswerTileData.IsMultiStepAnswer;
            string hiddenSide = isLevel3 ? data.HiddenSideLabel : "";
            triangleVisualizer.DrawTriangle(data.Depan, data.Samping, data.Miring, data.RotationAngle, data.Orientation, data.IsDualQuestion, isLevel3, hiddenSide);

            // Update button images berdasarkan level
            UpdateButtonImagesForLevel(progres, isLevel3);

            // Log difficulty, rotation, orientation, dan question type untuk debugging
            string questionType = data.IsDualQuestion ? "DUAL (A & B)" : isLevel3 ? "LEVEL 3 (A, B, C)" : "SINGLE (θ)";
            Debug.Log($"[Chapter1] Soal #{progres}/{totalSoal} | Type: {questionType} | Difficulty: {data.Difficulty} | Rotation: {data.RotationAngle}° | Orientation: {data.Orientation}");
        }
        else
        {
            // Fallback: reset warna jika visualizer tidak ada
            ResetSideColors();
        }

        // Setup answer tiles dengan jawaban benar dan distractor
        if (answerTileSystem != null && data.AnswerTileData != null)
        {
            // Check if this is Level 3 (multi-step answer mode)
            if (data.AnswerTileData.IsMultiStepAnswer)
            {
                // Debug: Verify formula data
                Debug.Log($"[UIManager] Level 3 Formula Data - QuestionSide: {data.AnswerTileData.QuestionSide}, Side1: {data.AnswerTileData.Side1Name}, Side2: {data.AnswerTileData.Side2Name}, Operator: {data.AnswerTileData.OperatorSymbol}");
                Debug.Log($"[UIManager] HiddenSideLabel: {data.HiddenSideLabel}");

                // Level 3: Multi-step Pythagoras solution (6 sequential slots)
                answerTileSystem.SetupMultiStepQuestion(
                    data.AnswerTileData.MultiStepCorrectAnswers,
                    data.AnswerTileData.WrongAnswers,
                    data.AnswerTileData.QuestionSide,
                    data.AnswerTileData.Side1Name,
                    data.AnswerTileData.Side2Name,
                    data.AnswerTileData.OperatorSymbol
                );
                Debug.Log($"[UIManager] Answer tiles spawned for question {progres} - Type: MULTI-STEP (Level 3, 6 slots)");
            }
            else
            {
                // Level 1-2: Fraction mode
                answerTileSystem.SetupQuestion(
                    data.AnswerTileData.NumeratorCorrect,
                    data.AnswerTileData.DenominatorCorrect,
                    data.AnswerTileData.NumeratorCorrect2,
                    data.AnswerTileData.DenominatorCorrect2,
                    data.AnswerTileData.WrongAnswers,
                    data.IsDualQuestion
                );
                Debug.Log($"[UIManager] Answer tiles spawned for question {progres} - Type: {(data.IsDualQuestion ? "DUAL" : "SINGLE")}");
            }
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

    /// <summary>
    /// Update button images berdasarkan level/difficulty
    /// Level 1 (Soal 1-10): DEPAN, SAMPING, MIRING
    /// Level 2 (Soal 11-20): AB, BC, AC
    /// Level 3 (Soal 21-30): AB, BC, AC (dengan vertex labels A, B, C)
    /// </summary>
    private void UpdateButtonImagesForLevel(int questionNumber, bool isLevel3)
    {
        if (depanButton == null || sampingButton == null || miringButton == null)
        {
            Debug.LogWarning("[UIManagerChapter1] Button references not assigned!");
            return;
        }

        UnityEngine.UI.Image depanImage = depanButton.GetComponent<UnityEngine.UI.Image>();
        UnityEngine.UI.Image sampingImage = sampingButton.GetComponent<UnityEngine.UI.Image>();
        UnityEngine.UI.Image miringImage = miringButton.GetComponent<UnityEngine.UI.Image>();

        if (depanImage == null || sampingImage == null || miringImage == null)
        {
            Debug.LogWarning("[UIManagerChapter1] Button Image components not found!");
            return;
        }

        // Determine level based on question number
        if (questionNumber >= 1 && questionNumber <= 10)
        {
            // Level 1: DEPAN, SAMPING, MIRING
            if (depanImageLevel1 != null) depanImage.sprite = depanImageLevel1;
            if (sampingImageLevel1 != null) sampingImage.sprite = sampingImageLevel1;
            if (miringImageLevel1 != null) miringImage.sprite = miringImageLevel1;
            Debug.Log("[UIManagerChapter1] Updated buttons for Level 1 (DEPAN/SAMPING/MIRING)");
        }
        else if (questionNumber >= 11 && questionNumber <= 20)
        {
            // Level 2: AB, BC, AC
            if (abImageLevel2 != null) depanImage.sprite = abImageLevel2;
            if (bcImageLevel2 != null) sampingImage.sprite = bcImageLevel2;
            if (acImageLevel2 != null) miringImage.sprite = acImageLevel2;
            Debug.Log("[UIManagerChapter1] Updated buttons for Level 2 (AB/BC/AC)");
        }
        else if (questionNumber >= 21 && questionNumber <= 30)
        {
            // Level 3: AB, BC, AC (dengan vertex labels)
            if (abImageLevel3 != null) depanImage.sprite = abImageLevel3;
            if (bcImageLevel3 != null) sampingImage.sprite = bcImageLevel3;
            if (acImageLevel3 != null) miringImage.sprite = acImageLevel3;
            Debug.Log("[UIManagerChapter1] Updated buttons for Level 3 (AB/BC/AC with vertex labels)");
        }
    }
}
