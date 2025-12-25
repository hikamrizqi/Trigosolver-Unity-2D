using UnityEngine;
using UnityEngine.UI; // Untuk elemen UI
using UnityEngine.SceneManagement; // For scene loading
using TMPro;         // Jika menggunakan TextMeshPro (lebih baik)

public class GameManagerChapter2 : MonoBehaviour
{
    // --- Referensi Objek di Inspector ---
    [Header("Game References")]
    public CannonController cannonController; // Seret objek Cannon di sini
    public GameObject projectilePrefab;       // Seret Prefab peluru di sini
    public Transform shootPoint;              // Titik di ujung meriam tempat peluru keluar
    public Transform targetObject;            // Seret objek Target (Kapal Musuh) di sini

    [Header("UI References")]
    public TextMeshProUGUI questionText;    // Teks untuk menampilkan soal
    public TMP_InputField angleInputField;  // Input untuk sudut dari pemain
    public Button shootButton;               // Tombol Tembak
    public TextMeshProUGUI feedbackText;    // Teks untuk feedback ke pemain

    // --- Parameter Fisika Dasar ---
    [Header("Physics Parameters")]
    public float gravity = 9.8f; // Percepatan gravitasi (m/s^2)
    public float initialVelocity = 100f; // Kecepatan awal peluru (m/s)

    // --- Variabel Game ---
    private float currentTargetDistance; // Jarak target untuk soal saat ini
    private float correctAngle;          // Sudut yang benar untuk soal saat ini

    void Start()
    {
        // Inisialisasi UI dan generate soal pertama
        shootButton.onClick.AddListener(OnShootButtonClicked);

        // Tambahkan listener untuk input field agar bisa submit dengan Enter
        angleInputField.onEndEdit.AddListener(OnInputFieldEndEdit);

        GenerateNewQuestion();
    }

    // Fungsi yang dipanggil saat user selesai mengedit input field (tekan Enter atau klik di luar)
    void OnInputFieldEndEdit(string inputValue)
    {
        // Jika user menekan Enter, langsung proses tembakan
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnShootButtonClicked();
        }
    }

    void GenerateNewQuestion()
    {
        // 1. Tentukan Jarak Target Acak
        // Misalnya, antara 100m hingga 900m (agar ada solusi valid)
        // Pastikan targetObject berada pada jarak ini secara visual di scene!
        currentTargetDistance = Random.Range(5f, 100f);

        // 2. Hitung Sudut yang Benar menggunakan Fungsi Invers Trigonometri
        // Rumus: theta = 0.5 * arcsin( (R * g) / v0^2 )
        float sin2Theta = (currentTargetDistance * gravity) / (initialVelocity * initialVelocity);

        // Pastikan sin2Theta valid (-1 <= sin2Theta <= 1)
        // Jika tidak, berarti target terlalu jauh untuk v0 yang ada
        if (sin2Theta > 1f) sin2Theta = 1f;
        if (sin2Theta < -1f) sin2Theta = -1f;

        float twoThetaRad = Mathf.Asin(sin2Theta); // Hasil dalam radian
        correctAngle = twoThetaRad * Mathf.Rad2Deg * 0.5f; // Konversi ke derajat dan bagi 2

        // Opsional: Untuk soal yang lebih kompleks, bisa ada dua solusi sudut
        // Jika 2theta = A, maka solusi lain adalah 180-A.
        // Untuk game, fokus pada solusi pertama (sudut lebih kecil) dulu agar tidak membingungkan.
        // Jika correctAngle > 45, mungkin kita ingin menggunakan solusi yang lebih kecil
        if (correctAngle > 45f)
        {
            correctAngle = (180f - (twoThetaRad * Mathf.Rad2Deg)) * 0.5f;
        }


        // 3. Update UI Soal
        questionText.text = $"Kapal Musuh: {currentTargetDistance:F1} m\n" +
                            $"Kecepatan Awal: {initialVelocity:F1} m/s\n" +
                            $"Gravitasi: {gravity:F1} m/s²\n" +
                            "Input sudut elevasi (derajat):";

        angleInputField.text = ""; // Kosongkan input
        feedbackText.text = ""; // Kosongkan feedback

        // Fokuskan ke input field agar user bisa langsung mengetik
        angleInputField.Select();
        angleInputField.ActivateInputField();

        // Pindahkan targetObject ke posisi yang benar di scene
        // Asumsi targetObject berada pada Y=0 (dataran)
        targetObject.position = new Vector3(currentTargetDistance, targetObject.position.y, targetObject.position.z);
    }

    void OnShootButtonClicked()
    {
        float playerInputAngle;

        // Ambil teks dari input field.
        string rawInput = angleInputField.text;

        // 1. Bersihkan Input (Hapus spasi, pastikan format titik desimal)
        // Walaupun tidak wajib, ini membantu menghindari error format regional
        string cleanedInput = rawInput.Replace(" ", "").Replace(",", ".");

        // 2. Coba konversi teks yang sudah dibersihkan menjadi float
        if (float.TryParse(cleanedInput,
                           System.Globalization.NumberStyles.Any,
                           System.Globalization.CultureInfo.InvariantCulture, // Memaksa penggunaan titik desimal
                           out playerInputAngle))
        {
            // --- JIKA VALID (berhasil diubah jadi angka) ---

            // Panggil fungsi kontrol meriam
            cannonController.SetTargetAngle(playerInputAngle);
            feedbackText.color = Color.white;
            feedbackText.text = $"Memutar meriam ke {playerInputAngle:F1}°...";

            // Tembak setelah sedikit jeda
            Invoke("ShootProjectile", cannonController.rotationSpeed * 0.5f + 0.1f);
        }
        else
        {
            // --- JIKA TIDAK VALID ---
            feedbackText.color = Color.red;
            feedbackText.text = "Input sudut elevasi tidak valid. Masukkan angka.";
        }
    }

    void ShootProjectile()
    {
        // Dapatkan sudut aktual meriam setelah rotasi (untuk akurasi)
        float actualAngle = cannonController.GetCannonAngle();

        // Buat instance peluru
        GameObject projectileGO = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        Rigidbody2D rb = projectileGO.GetComponent<Rigidbody2D>();

        // Hitung komponen kecepatan berdasarkan sudut aktual
        float angleRad = actualAngle * Mathf.Deg2Rad; // Konversi ke radian
        Vector2 launchForce = new Vector2(
            initialVelocity * Mathf.Cos(angleRad),
            initialVelocity * Mathf.Sin(angleRad)
        );

        // Terapkan gaya ke peluru
        rb.gravityScale = gravity / Physics2D.gravity.magnitude; // Sesuaikan skala gravitasi Unity
        rb.linearVelocity = launchForce;

        // Berikan feedback instan tentang tembakan
        feedbackText.text = $"Tembakan dengan sudut: {actualAngle:F1}°. Menunggu hasil...";

        // Untuk deteksi hit, peluru atau Game Manager akan memprosesnya.
        // Anda bisa menambahkan script ke peluru untuk mendeteksi tabrakan dengan target.
    }

    // Dipanggil oleh ProjectileController saat mengenai sesuatu
    public void OnProjectileHit(Vector2 hitPosition)
    {
        float hitDistance = hitPosition.x;
        float accuracy = Mathf.Abs(hitDistance - currentTargetDistance);

        if (accuracy < 10f) // Toleransi hit, misalnya 10 meter
        {
            feedbackText.color = Color.green;
            feedbackText.text = $"TARGET HANCUR! Akurat ({accuracy:F1} m dari target).";
        }
        else
        {
            feedbackText.color = Color.red;
            feedbackText.text = $"Meleset! Jarak tembak: {hitDistance:F1} m. Target: {currentTargetDistance:F1} m. Selisih: {accuracy:F1} m.";
        }

        // Generate soal baru setelah beberapa waktu
        Invoke("GenerateNewQuestion", 3f);
        feedbackText.color = Color.white; // Reset warna teks
    }

    // Visualisasi jalur peluru yang benar (opsional, untuk debugging/bantuan)
    void OnDrawGizmos()
    {
        if (shootPoint == null) return;

        Gizmos.color = Color.blue;
        // Gambar garis arah tembakan dari meriam
        float angleRad = correctAngle * Mathf.Deg2Rad;
        Vector2 startPos = shootPoint.position;
        Vector2 dir = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        Gizmos.DrawRay(startPos, dir * 5f); // Gambar garis pendek

        // Gambar jalur parabola (opsional, untuk debugging)
        Gizmos.color = Color.yellow;
        Vector2 prevPos = startPos;
        for (float t = 0; t < 2 * initialVelocity * Mathf.Sin(angleRad) / gravity; t += 0.1f) // Iterasi waktu
        {
            float x = initialVelocity * Mathf.Cos(angleRad) * t;
            float y = initialVelocity * Mathf.Sin(angleRad) * t - 0.5f * gravity * t * t;
            Vector2 currentPos = startPos + new Vector2(x, y);
            Gizmos.DrawLine(prevPos, currentPos);
            prevPos = currentPos;
        }
    }

    /// <summary>
    /// Back to main menu (Mode Cerita Selection)
    /// </summary>
    public void BackToMainMenu()
    {
        Debug.Log("[GameManagerChapter2] Back to main menu");
        SceneManager.LoadScene("Main Menu");
    }
}