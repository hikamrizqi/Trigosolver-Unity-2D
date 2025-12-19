using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    [Header("Pengaturan Parallax")]
    [Tooltip("Seberapa cepat BG bergerak di sumbu X. 0 = diam.")]
    public float parallaxMultiplierX = 0f;

    [Tooltip("Seberapa cepat BG bergerak di sumbu Y. 0 = diam.")]
    public float parallaxMultiplierY = 0f;

    void Start()
    {
        // Cari kamera utama dan simpan posisinya
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
    }

    void FixedUpdate()
    {
        // Hitung seberapa jauh kamera bergerak sejak frame terakhir
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        // Hitung pergerakan parallax baru berdasarkan multiplier X dan Y
        float moveX = deltaMovement.x * parallaxMultiplierX;
        float moveY = deltaMovement.y * parallaxMultiplierY;

        // Terapkan pergerakan ke background ini
        transform.position += new Vector3(moveX, moveY, 0);

        // Simpan posisi kamera untuk perhitungan frame berikutnya
        lastCameraPosition = cameraTransform.position;
    }
}