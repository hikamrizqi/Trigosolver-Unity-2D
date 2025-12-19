using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float lifetime = 5f; // Peluru akan hancur setelah waktu tertentu

    private GameManagerChapter2 gameManager; // Referensi ke GameManager

    [System.Obsolete]
    void Start()
    {
        // Cari GameManager di scene
        gameManager = FindObjectOfType<GameManagerChapter2>();

        // Hancurkan peluru setelah 'lifetime' detik agar tidak menumpuk
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Jika menabrak target atau tanah
        if (collision.gameObject.CompareTag("Target") || collision.gameObject.CompareTag("Ground"))
        {
            if (gameManager != null)
            {
                // Beri tahu GameManager bahwa peluru sudah mengenai sesuatu
                gameManager.OnProjectileHit(transform.position);
            }
            Destroy(gameObject); // Hancurkan peluru setelah menabrak
        }
    }
}