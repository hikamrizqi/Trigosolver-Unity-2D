# Setup Guide: Character Animation System untuk Chapter 1

## Overview
Sistem animasi karakter yang akan muncul setelah setiap jawaban (benar/salah) di Chapter 1 Level 1, 2, dan 3. Karakter akan muncul dari bawah ke tengah layar, menampilkan animasi berjalan dengan bubble chat, lalu turun kembali sebelum lanjut ke soal berikutnya.

## Komponen yang Dibuat

### 1. Script: CharacterAnimationController.cs
Path: `Assets/Scripts/UI/Chapter1/CharacterAnimationController.cs`

**Fungsi utama:**
- Mengatur animasi karakter muncul dari bawah ke tengah layar
- Menampilkan animasi sprite (5 frame) secara loop
- Menampilkan bubble chat dengan teks random
- Menganimasikan karakter turun kembali

### 2. Modifikasi: CalculationManager.cs
**Perubahan:**
- Menambahkan referensi `CharacterAnimationController`
- Membuat coroutine baru `NextRoundDelayWithCharacterAnimation()`
- Mengintegrasikan animasi karakter ke dalam flow jawaban benar/salah

---

## Setup di Unity Editor

### Step 1: Setup GameObject Hierarchy
1. Buat GameObject baru di scene Chapter 1:
   - Nama: `CharacterAnimationSystem`
   - Parent: Canvas utama Chapter 1

2. Di bawah `CharacterAnimationSystem`, buat struktur berikut:
   ```
   CharacterAnimationSystem (GameObject)
   └── CharacterImage (Image)
       └── BubbleChatPanel (GameObject)
           └── ChatText (TextMeshProUGUI)
   ```

### Step 2: Setup CharacterImage (Image Component)
1. Select `CharacterImage`
2. Add Component → UI → Image
3. Settings:
   - **Anchor**: Center-Bottom
   - **Pivot**: (0.5, 0)
   - **Width**: 200-300 (sesuaikan dengan ukuran sprite karakter)
   - **Height**: 200-300
   - **Color**: White (255, 255, 255, 255)

### Step 3: Setup BubbleChatPanel
1. Select `BubbleChatPanel`
2. Add Component → UI → Image (untuk background bubble)
3. Settings:
   - **Anchor**: Top center dari CharacterImage
   - **Pivot**: (0.5, 0)
   - **Position Y**: Sedikit di atas karakter (misal: +250)
   - **Width**: 300-400
   - **Height**: 100-150
   - **Sprite**: Gunakan sprite bubble chat (rounded rectangle)
   - **Color**: White dengan sedikit transparency (255, 255, 255, 230)

### Step 4: Setup ChatText (TextMeshProUGUI)
1. Select `ChatText`
2. Add Component → UI → TextMeshProUGUI
3. Settings:
   - **Font**: Thaleah atau font pixel yang sesuai
   - **Font Size**: 18-24
   - **Alignment**: Center (horizontal & vertical)
   - **Color**: Black (0, 0, 0, 255)
   - **Wrapping**: Enabled
   - **Overflow**: Truncate
   - **Auto Size**: Optional (untuk menyesuaikan teks panjang/pendek)

### Step 5: Setup CharacterAnimationController Script
1. Select `CharacterAnimationSystem`
2. Add Component → `CharacterAnimationController`
3. Assign references di Inspector:
   
   **Character Setup:**
   - **Character Image**: Drag `CharacterImage` (Image component)
   - **Character Transform**: Drag `CharacterImage` (RectTransform)
   
   **Animation Sprites:**
   - **Correct Animation Sprites** (Size: 5):
     - Drag 5 sprite untuk animasi jawaban BENAR
     - Urutan: frame 1, 2, 3, 4, 5
   - **Wrong Animation Sprites** (Size: 5):
     - Drag 5 sprite untuk animasi jawaban SALAH
     - Urutan: frame 1, 2, 3, 4, 5
   - **Game Over Animation Sprites** (Size: 5): ⭐ NEW!
     - Drag 5 sprite untuk animasi GAME OVER (karakter marah)
     - Urutan: frame 1, 2, 3, 4, 5
   - **Sprite Animation Speed**: 0.15 (default, bisa disesuaikan)
   
   **Bubble Chat:**
   - **Bubble Chat Panel**: Drag `BubbleChatPanel`
   - **Bubble Chat Text**: Drag `ChatText`
   
   **Animation Settings:**
   - **Move Up Duration**: 0.8 (waktu muncul dari bawah)
   - **Move Down Duration**: 0.8 (waktu turun ke bawah)
   - **Display Duration**: 2.5 (waktu tampil di tengah)
   - **Hidden Position**: (0, -800) - posisi awal di bawah layar
   - **Center Position**: (0, 0) - posisi tengah layar
   
   **Random Messages:**
   - **Correct Messages** (Size: 5):
     1. "Hebat! Jawabanmu benar!"
     2. "Luar biasa! Kamu pintar!"
     3. "Sempurna! Pertahankan!"
     4. "Bagus sekali! Terus seperti itu!"
     5. "Mantap! Kamu memahaminya!"
   
   - **Wrong Messages** (Size: 5):
     1. "Oops! Coba periksa lagi."
     2. "Hmm, belum tepat. Semangat!"
     3. "Jangan menyerah! Coba lagi."
     4. "Hampir! Periksa perhitunganmu."
     5. "Yuk, fokus dan coba lagi!"
   
   - **Game Over Messages** (Size: 5): ⭐ NEW!
     1. "Yah, nyawa habis!"
     2. "Waduh! Game Over."
     3. "Semangat! Coba lagi ya!"
     4. "Jangan menyerah!"
     5. "Next time pasti lebih baik!"

### Step 6: Setup CalculationManager Reference
1. Select GameObject yang memiliki `CalculationManager` script
2. Locate field baru: **Character Anim Controller**
3. Drag `CharacterAnimationSystem` ke field tersebut

---

## Lokasi Sprite Karakter

Berdasarkan struktur project:
- Path sprite: `Assets/Sprite/Object/`

**Yang perlu disiapkan:**
1. **5 sprite untuk animasi BENAR** (misal: character_correct_1.png sampai character_correct_5.png)
   - Bisa berupa karakter dengan ekspresi senang/gembira
   - Pose berjalan atau merayakan
   
2. **5 sprite untuk animasi SALAH** (misal: character_wrong_1.png sampai character_wrong_5.png)
   - Bisa berupa karakter dengan ekspresi sedih/bingung
   - Pose merenung atau menggaruk kepala

3. **5 sprite untuk animasi GAME OVER** ⭐ NEW! (misal: character_gameover_1.png sampai character_gameover_5.png)
   - Bisa berupa karakter dengan ekspresi MARAH/frustasi
   - Pose menghentak kaki, tangan di pinggang, atau ekspresi kesal

**Import Settings untuk Sprite:**
- Texture Type: Sprite (2D and UI)
- Pixels Per Unit: 100 (default)
- Filter Mode: Point (untuk pixel art) atau Bilinear
- Compression: None atau Low Quality

---

## Testing

### Test Flow:
1. Play scene Chapter 1
2. Pilih Level 1, 2, atau 3
3. Jawab soal dengan benar:
   - ✓ Karakter muncul dari bawah
   - ✓ Animasi sprite berjalan
   - ✓ Bubble chat muncul dengan teks random
   - ✓ Karakter turun kembali
   - ✓ Lanjut ke soal berikutnya
4. Jawab soal dengan salah:
   - ✓ Karakter muncul dengan sprite berbeda
   - ✓ Bubble chat muncul dengan teks error random
   - ✓ Karakter turun kembali
   - ✓ Lanjut ke soal berikutnya
5. **Test Game Over** ⭐ NEW!:
   - ✓ Jawab salah hingga lives = 0
   - ✓ Karakter marah muncul dari bawah
   - ✓ Bubble chat game over muncul
   - ✓ Karakter TETAP TAMPIL (tidak auto-hide)
   - ✓ Game over panel muncul
   - ✓ Setelah delay atau tekan tombol kembali
   - ✓ Karakter turun baru hilang
   - ✓ Kembali ke level selection

### Debug Console:
Periksa log untuk memastikan animasi berjalan:
```
[CharacterAnimation] Moving up from (0, -800) to (0, 0)
[CharacterAnimation] Moving down from (0, 0) to (0, -800)
[CharacterAnimation] Animation complete!
```

---

## Customization

### Mengubah Durasi Animasi:
Di Inspector `CharacterAnimationController`:
- **Move Up Duration**: Lebih cepat = nilai lebih kecil
- **Display Duration**: Lebih lama tampil = nilai lebih besar
- **Sprite Animation Speed**: Lebih cepat = nilai lebih kecil

### Menambah/Mengubah Pesan:
Di Inspector `CharacterAnimationController`:
- Expand **Correct Messages** atau **Wrong Messages**
- Ubah **Size** untuk menambah/mengurangi pesan
- Edit teks di setiap element

### Mengubah Posisi:
Di Inspector `CharacterAnimationController`:
- **Hidden Position Y**: Lebih negatif = mulai lebih jauh di bawah
- **Center Position Y**: Ubah untuk menampilkan di posisi berbeda

---

## Troubleshooting

### Karakter tidak muncul:
- Cek apakah `characterAnimController` sudah di-assign di `CalculationManager`
- Cek apakah `CharacterImage` aktif di hierarchy
- Cek apakah sprites sudah di-assign

### Bubble chat tidak muncul:
- Cek apakah `BubbleChatPanel` dan `ChatText` sudah di-assign
- Cek apakah ada minimal 1 message di array `correctMessages`/`wrongMessages`

### Animasi sprite tidak smooth:
- Kurangi nilai `Sprite Animation Speed` (misal: 0.1)
- Pastikan semua 5 sprite sudah di-assign dan tidak null

### DOTween error:
- Pastikan DOTween sudah ter-install di project
- Jika belum, bisa diganti dengan `AnimationCurve` atau `Coroutine` manual

---

## Dependencies

- **DOTween**: Untuk animasi smooth (ease in/out)
- **TextMesh Pro**: Untuk bubble chat text
- **Unity UI**: Untuk Image components

---

## Notes

- Animasi karakter hanya aktif untuk soal 1-30 (Level 1, 2, 3)
- Setiap level bisa menggunakan sprite karakter yang sama atau berbeda
- Sistem mendukung customization penuh tanpa mengubah code
- Animasi berjalan secara sequential: feedback → character anim → triangle/tiles exit → next question
