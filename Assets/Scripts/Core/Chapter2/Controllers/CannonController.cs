using UnityEngine;

public class CannonController : MonoBehaviour
{
    public float rotationSpeed = 5f; // Kecepatan putar meriam (untuk animasi rotasi yang mulus)

    private float _targetAngle = 0f; // Sudut target yang akan dituju meriam

    // Dipanggil oleh GameManager untuk mengatur sudut rotasi yang diinginkan
    public void SetTargetAngle(float newAngle)
    {
        // Batasi sudut antara 0 dan 90 derajat (sesuai fisika balistik)
        _targetAngle = Mathf.Clamp(newAngle, 0f, 90f);
    }

    // Mengembalikan sudut meriam saat ini (misalnya, untuk akurasi tembakan)
    public float GetCannonAngle()
    {
        // Ambil sudut Z dari rotasi objek, ini adalah sudut elevasi
        return transform.rotation.eulerAngles.z;
    }

    void Update()
    {
        // Rotasi meriam secara bertahap ke sudut target
        RotateCannonToTargetAngle();
    }

    void RotateCannonToTargetAngle()
    {
        // Buat Rotasi Target (Quaternion) pada sumbu Z
        // Penting: Rotasi di Unity bisa sedikit membingungkan untuk sudut 2D.
        // Jika meriam menunjuk ke kanan pada 0 derajat, dan Anda ingin 30 derajat ke atas,
        // maka sudut Z harus 30. Tapi Unity bisa menafsirkan 90 derajat sebagai "ke atas".
        // Coba atur 'targetRotation' di inspector objek Cannon, lalu lihat nilai Z-nya
        // Misalnya, jika '0' di inspector menunjuk ke kanan, maka targetAngle Anda langsung adalah nilai Z.
        Quaternion targetRotation = Quaternion.Euler(0, 0, _targetAngle);

        // Rotasi meriam secara bertahap menggunakan Quaternion.Slerp untuk kehalusan
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}