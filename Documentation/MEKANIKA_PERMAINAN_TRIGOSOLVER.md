# 🎮 MEKANIKA PERMAINAN TRIGOSOLVER

**Game Edukasi Trigonometri Berbasis Unity 2D**  
**Platform:** Mobile (Android/iOS)  
**Genre:** Educational Puzzle Game  

---

## 📖 PENJELASAN LENGKAP MEKANIKA PERMAINAN

### 🎬 **A. ALUR PERMAINAN DARI AWAL**

Ketika pemain pertama kali membuka game Trigosolver, mereka akan disambut dengan **Logo Screen** yang menampilkan logo game dengan efek animasi drop-in dari atas. Pemain dapat mengklik di mana saja pada layar logo untuk melanjutkan ke Main Menu. Transisi dari logo ke Main Menu menggunakan animasi sink-out (tenggelam ke bawah) yang smooth dan responsif.

Setelah logo menghilang, pemain akan masuk ke **Main Menu** yang merupakan hub utama navigasi game. Di Main Menu, terdapat empat tombol pilihan yang disusun vertikal dengan animasi drop-in yang menarik. Keempat tombol tersebut adalah: **MULAI** untuk memulai permainan, **MATERI** untuk mengakses tutorial dan penjelasan konsep trigonometri secara langsung tanpa harus bermain, **HIGHSCORE** untuk melihat skor tertinggi yang pernah dicapai pada setiap level, dan **KELUAR** untuk menutup aplikasi. Setiap tombol memiliki efek hover dan sound effect saat diklik untuk memberikan feedback interaktif kepada pemain.

---

### 🎯 **B. PEMILIHAN MODE PERMAINAN**

Ketika pemain menekan tombol **MULAI** dari Main Menu, Main Menu akan melakukan animasi sink-out dan muncul panel **Mode Selection**. Panel ini menawarkan dua mode permainan yang berbeda: **Mode Cerita** dan **Mode Bebas**. Kedua mode ini memiliki karakteristik dan alur yang berbeda.

#### **1. Mode Cerita (Story Mode)**

Mode Cerita adalah mode pembelajaran terstruktur yang dirancang untuk pemain yang baru belajar trigonometri atau ingin memahami konsep secara bertahap. Ketika pemain memilih Mode Cerita, panel Mode Selection akan melakukan transisi ke panel **Chapter Selection**. Di panel ini, pemain dapat memilih dari dua chapter yang tersedia:

- **Chapter 1: Observasi Segitiga** - Fokus pada konsep dasar perbandingan trigonometri (Sin, Cos, Tan) dengan visualisasi segitiga siku-siku statis.
- **Chapter 2: Tembakan Meriam** - Aplikasi trigonometri dalam konteks proyektil dan gerak parabola.

Setelah memilih salah satu chapter, pemain akan dibawa ke **Story Panel**. Story Panel menampilkan narasi cerita dengan efek typewriter (text muncul satu karakter per waktu) yang menjelaskan konteks pembelajaran. Untuk Chapter 1, cerita mengisahkan seorang arsitek yang harus menghitung proporsi segitiga untuk membangun struktur yang stabil. Cerita ini terdiri dari 5 slide story yang harus dibaca pemain.

Setelah story slides selesai, Story Panel akan **WAJIB** menampilkan **Materi & Tutorial Panel** (relasi include dalam use case diagram). Panel ini menjelaskan konsep trigonometri secara detail dengan ilustrasi visual, rumus matematika, dan contoh kasus. Materi mencakup:

- Definisi Sin θ = Sisi Depan / Sisi Miring
- Definisi Cos θ = Sisi Samping / Sisi Miring  
- Definisi Tan θ = Sisi Depan / Sisi Samping
- Teorema Pythagoras: c² = a² + b²
- Contoh perhitungan dengan segitiga (3,4,5) dan (5,12,13)

**PENTING:** Pemain **WAJIB** melewati Story Panel dan Materi Panel saat pertama kali masuk Stage 1. Tidak ada opsi untuk skip. Story dan materi harus dibaca secara sekuensial.

Setelah selesai membaca materi, pemain akan dibawa ke panel **Level Selection** untuk memilih level kesulitan. Di panel Level Selection, terdapat tombol **MATERI** yang memungkinkan pemain untuk **membuka kembali panel materi dan tutorial** kapan saja tanpa harus membaca ulang Story Panel. Ini adalah relasi extend dalam use case diagram - fitur opsional untuk review materi.

#### **2. Mode Bebas (Free Play Mode)**

Mode Bebas adalah mode latihan tanpa story untuk pemain yang sudah memahami konsep dan ingin langsung berlatih. Ketika pemain memilih Mode Bebas dari Mode Selection, mereka akan **langsung** dibawa ke panel **Level Selection** tanpa melewati Chapter Selection atau Story Panel. Ini memungkinkan akses cepat ke gameplay untuk sesi latihan yang lebih fokus.

---

### 📚 **C. PEMILIHAN LEVEL**

Di panel **Level Selection**, pemain dapat memilih dari tiga level dengan tingkat kesulitan yang berbeda. Setiap level memiliki mekanisme gameplay yang unik berdasarkan jumlah slot jawaban:

#### **Level 1: Perbandingan Trigonometri Tunggal (2 Slot)**

**Konsep:** Pemain menjawab **satu pertanyaan trigonometri** (Sin θ, Cos θ, atau Tan θ) dengan memilih 2 angka untuk membentuk pecahan.

**Mekanisme:**
- **Pertanyaan:** "Hitunglah Sin θ" atau "Hitunglah Cos θ" atau "Hitunglah Tan θ"
- **Visualisasi:** Gambar segitiga siku-siku dengan label sisi (Depan, Samping, Miring) dan nilai panjangnya
- **Slot Jawaban:** 2 slot kosong untuk membentuk pecahan: `[__] / [__]`
- **Pilihan Angka:** 8 kotak angka (prefab) tersedia, misal: `3, 4, 5, 6, 7, 8, 9, 12`
- **Cara Menjawab:** Pemain **tap angka** yang sesuai untuk mengisi slot. Contoh: Sin θ = 3/5 → tap `3` untuk slot atas, tap `5` untuk slot bawah
- **Triple:** Menggunakan Pythagorean triples sederhana seperti (3,4,5), (5,12,13), (8,15,17)

**Contoh Soal Level 1:**
```
┌─────────────────────────────┐
│   Hitunglah nilai Sin θ     │
│                             │
│        🔺 Segitiga          │
│        /|                   │
│     5 / |  3 (Depan)        │
│      /  |                   │
│     /____|                  │
│       4 (Samping)           │
│   Miring = 5                │
│                             │
│   Jawaban: [__] / [__]      │
│                             │
│  Pilih Angka:               │
│  [3] [4] [5] [6] [7] [8]    │
│  [9] [12]                   │
└─────────────────────────────┘
Jawaban Benar: 3/5 (Depan/Miring)
```

#### **Level 2: Perbandingan Trigonometri Ganda (4 Slot)**

**Konsep:** Pemain menjawab **dua pertanyaan trigonometri sekaligus** (misal: Sin α dan Tan β) dengan memilih 4 angka untuk membentuk 2 pecahan.

**Mekanisme:**
- **Pertanyaan:** "Hitunglah Sin α dan Tan β" atau "Hitunglah Cos α dan Sin β"
- **Visualisasi:** Gambar segitiga dengan 2 sudut yang ditandai (α dan β) dengan label sisi lengkap
- **Slot Jawaban:** 4 slot kosong untuk 2 pecahan: `Sin α = [__]/[__]` dan `Tan β = [__]/[__]`
- **Pilihan Angka:** 10-12 kotak angka tersedia dengan berbagai nilai
- **Cara Menjawab:** Pemain tap angka secara **berurutan** untuk mengisi 4 slot (2 pecahan)
- **Triple:** Menggunakan triples kompleks seperti (5,12,13), (7,24,25), (9,40,41)
- **Kesulitan:** Pemain harus memahami orientasi segitiga dan mengidentifikasi sisi yang tepat untuk 2 sudut berbeda

**Contoh Soal Level 2:**
```
┌─────────────────────────────┐
│ Hitunglah Sin α dan Cos β   │
│                             │
│        🔺 C                 │
│        /|                   │
│    13 / | 5 (Depan α)       │
│   α /  |                    │
│     /β__|                   │
│   A  12  B (Samping α)      │
│                             │
│  Sin α = [__]/[__]          │
│  Cos β = [__]/[__]          │
│                             │
│  Pilih Angka:               │
│  [5] [12] [13] [7] [24]     │
│  [25] [8] [15]              │
└─────────────────────────────┘
Jawaban Benar:
Sin α = 5/13 (Depan/Miring)
Cos β = 5/13 (Samping β/Miring)
```

#### **Level 3: Teorema Pythagoras (6 Slot)**

**Konsep:** Pemain menyelesaikan **rumus Pythagoras** (a² + b² = c²) dengan mencari salah satu sisi yang tidak diketahui, mengisi 6 slot untuk membentuk persamaan lengkap.

**Mekanisme:**
- **Pertanyaan:** "Tentukan panjang sisi Miring" atau "Tentukan panjang sisi Depan" atau "Tentukan panjang sisi Samping"
- **Visualisasi:** Gambar segitiga dengan 2 sisi diketahui (dengan nilai), 1 sisi tidak diketahui (dengan tanda `?`)
- **Slot Jawaban:** 6 slot kosong untuk rumus Pythagoras: `[__]² + [__]² = [__]²`
- **Pilihan Angka:** 12-15 kotak angka tersedia, termasuk angka yang akan dikuadratkan dan hasil kuadratnya
- **Cara Menjawab:** Pemain tap angka untuk mengisi slot membentuk persamaan Pythagoras yang benar
- **Triple:** Menggunakan semua Pythagorean triples termasuk yang menantang seperti (11,60,61), (13,84,85), (36,77,85)
- **Kesulitan:** Pemain harus memahami konsep kuadrat dan operasi Pythagoras, serta mengidentifikasi sisi yang tidak diketahui

**Contoh Soal Level 3:**
```
┌─────────────────────────────┐
│ Tentukan panjang sisi Miring│
│                             │
│        🔺                   │
│        /|                   │
│     ? / | 8                 │
│      /  |                   │
│     /____|                  │
│       15                    │
│                             │
│  Rumus Pythagoras:          │
│  [__]² + [__]² = [__]²      │
│                             │
│  Pilih Angka:               │
│  [8] [15] [17] [64] [225]   │
│  [289] [13] [84] [85]       │
└─────────────────────────────┘
Jawaban Benar:
8² + 15² = 17²
Slot diisi: [8][15][17]
```

**Catatan Penting:**
- Semua level menggunakan **sistem pilihan ganda** dengan tap angka, bukan input ketikan
- Jawaban selalu dalam bentuk **pecahan bulat (a/b)**, tidak ada desimal
- Setiap soal menampilkan **visualisasi segitiga** dengan label sisi dan nilai panjang yang diketahui
- Pemain memilih dari **kotak-kotak angka (prefab)** yang tersedia
- Angka yang dipilih akan mengisi slot secara berurutan

Ketika pemain mengklik salah satu level, game akan memuat scene gameplay (Stage 1) dan memulai gameplay dengan mekanisme yang sesuai.

---

### 🎲 **D. GAMEPLAY CORE - CHAPTER 1 (OBSERVASI SEGITIGA)**

Gameplay Chapter 1 adalah inti dari pengalaman bermain Trigosolver. Setelah pemain memilih level, sistem akan memulai gameplay dengan alur sebagai berikut:

#### **1. Inisialisasi Game**

Ketika scene gameplay dimuat, **CalculationManager** script akan menjalankan method `Start()` yang melakukan inisialisasi:
- Set lives pemain = 3 (ditampilkan sebagai 3 ikon hati merah di UI)
- Set progres = 0 (soal belum dimulai)
- Update UI nyawa
- Panggil `StartNewRound()` untuk generate soal pertama

#### **2. Generate Soal Trigonometri (Include Wajib)**

Method `StartNewRound()` akan memanggil **TriangleDataGenerator** untuk menghasilkan soal. Proses ini adalah relasi **include** dalam use case diagram karena wajib dilakukan setiap kali memulai round baru. Generator akan:

**a. Pilih Pythagorean Triple**  
Sistem menggunakan database Pythagorean triples yang terbukti matematika (triple yang memenuhi a² + b² = c²). Contoh triples:
- (3, 4, 5)
- (5, 12, 13)
- (8, 15, 17)
- (7, 24, 25)
- (20, 21, 29)
- (9, 40, 41)
- (12, 35, 37)
- (11, 60, 61)
- (13, 84, 85)
- (36, 77, 85)

**b. Tentukan Tipe Soal**  
Sistem random memilih salah satu dari 3 tipe soal:
- **Sin θ** (questionType = 0): Hitung Sin θ = Sisi Depan / Sisi Miring
- **Cos θ** (questionType = 1): Hitung Cos θ = Sisi Samping / Sisi Miring
- **Tan θ** (questionType = 2): Hitung Tan θ = Sisi Depan / Sisi Samping

**c. Tentukan Orientasi**  
Untuk menambah variasi, sistem random menentukan mana yang jadi "Depan" dan "Samping":
- Jika `isADepan = true`: Depan = a, Samping = b, Miring = c
- Jika `isADepan = false`: Depan = b, Samping = a, Miring = c

**d. Hitung Jawaban Benar**  
Berdasarkan tipe soal dan orientasi, sistem menghitung jawaban yang benar. Contoh untuk triple (3,4,5) dengan Depan=3, Samping=4, Miring=5:
- Sin θ = 3/5 = 0.6
- Cos θ = 4/5 = 0.8
- Tan θ = 3/4 = 0.75

**e. Generate Data Soal**  
Sistem membuat objek **TriangleData** yang berisi:
```
{
  Depan: 3,
  Samping: 4,
  Miring: 5,
  SoalDisederhanakan: "Sinθ",
  JawabanBenar: 0.6,
  JawabanBenar2: null (untuk soal non-double)
}
```

#### **3. Tampilkan Visualisasi Segitiga (Include Wajib)**

Setelah soal di-generate, **TriangleVisualizer** akan merender visualisasi segitiga menggunakan **LineRenderer** dan **SpriteRenderer**. Visualisasi ini adalah relasi **include** karena wajib ditampilkan setiap soal baru.

**Komponen Visual:**
- **Sisi Depan**: Garis vertikal berwarna **MERAH** dengan label teks menampilkan nilai (misal: "3")
- **Sisi Samping**: Garis horizontal berwarna **HIJAU** dengan label teks (misal: "4")
- **Sisi Miring**: Garis diagonal berwarna **BIRU** dengan label teks (misal: "5")
- **Sudut θ (Theta)**: Icon sudut kecil di corner bawah kiri segitiga
- **Background**: Grid matematika untuk konteks visual

Segitiga dirender secara dinamis menggunakan koordinat 2D:
- Vertex A di (0, 0) - bottom-left
- Vertex B di (samping, 0) - bottom-right  
- Vertex C di (0, depan) - top-left

Untuk level Medium dan Hard, segitiga akan di-rotasi menggunakan `RotationAngle` untuk menambah kesulitan identifikasi sisi.

#### **4. Tampilkan UI Pertanyaan (Include Wajib)**

**UIManagerChapter1** akan setup UI elements:
- **Progress Text**: "Soal: 1/30" (menunjukkan progres)
- **Question Text**: "Hitunglah nilai dari Sinθ" (pertanyaan)
- **Label Depan**: "Depan: 3" dengan warna merah
- **Label Samping**: "Samping: 4" dengan warna hijau
- **Label Miring**: "Miring: 5" dengan warna biru
- **Lives Display**: 3 ikon hati merah (❤️❤️❤️)
- **Score Display**: "Score: 0"
- **Input Field**: Tempat pemain mengetik jawaban
- **Button CHECK**: Tombol untuk submit jawaban

UI juga menampilkan **tombol BACK** untuk kembali ke Level Selection dan **tombol PAUSE** untuk membuka pause menu (relasi extend - opsional).

#### **5. Input Jawaban Pemain (Sistem Pilihan Ganda)**

Sistem input menggunakan **tap-based multiple choice** (pilihan ganda dengan tap), bukan input ketikan. Mekanisme:

**Komponen UI:**
- **Slot Jawaban:** Kotak-kotak kosong sesuai level (2, 4, atau 6 slot)
- **Number Buttons:** 8-15 kotak angka (prefab) yang dapat di-tap
- **Visual Feedback:** Slot yang aktif diberi highlight, angka yang dipilih hilang dari pilihan

**Mekanisme Pilih Angka:**

**Level 1 (2 Slot - Pecahan Tunggal):**
```
UI Layout:
┌────────────────────────────┐
│ Jawaban: [__] / [__]       │  ← 2 slot kosong
│                            │
│ Pilih Angka:               │
│ [3] [4] [5] [6] [7] [8]    │  ← 8 kotak angka
│                            │
└────────────────────────────┘

Proses:
1. Pemain tap [3] → Slot 1 terisi: [3] / [__]
2. Pemain tap [5] → Slot 2 terisi: [3] / [5]
3. Angka 3 dan 5 hilang dari pilihan
4. Tap tombol CHECK untuk validasi
```

**Level 2 (4 Slot - 2 Pecahan):**
```
UI Layout:
┌────────────────────────────┐
│ Sin α = [__]/[__]          │  ← 2 slot pertama
│ Tan β = [__]/[__]          │  ← 2 slot kedua
│                            │
│ Pilih Angka:               │
│ [5] [12] [13] [7] [24]     │  ← 10-12 kotak
│ [25] [8] [15]              │
└────────────────────────────┘

Proses:
1. Pemain tap angka secara berurutan untuk 4 slot
2. Auto-advance ke slot berikutnya setelah diisi
3. Angka yang dipilih hilang dari pilihan
```

**Level 3 (6 Slot - Rumus Pythagoras):**
```
UI Layout:
┌────────────────────────────┐
│ [__]² + [__]² = [__]²      │  ← 6 slot (3 angka)
│                            │
│ Pilih Angka:               │
│ [8] [15] [17] [64] [225]   │  ← 12-15 kotak
│ [289] [13] [84] [85]       │
└────────────────────────────┘

Proses:
1. Tap [8] → [8]² + [__]² = [__]²
2. Tap [15] → [8]² + [15]² = [__]²
3. Tap [17] → [8]² + [15]² = [17]²
4. Sistem auto-hitung: 64 + 225 = 289 ✓
```

**Fitur Interaktif:**
- **Undo:** Tombol backspace/undo untuk hapus angka terakhir yang dipilih
- **Clear:** Tombol reset untuk kosongkan semua slot
- **Visual Feedback:** 
  * Slot aktif: Border biru bercahaya
  * Angka dipilih: Highlight hijau → hilang dari pilihan
  * Slot terisi: Background putih dengan angka hitam tebal

**NumberButton Script:**
```csharp
public class NumberButton : MonoBehaviour
{
    public int number;
    public Button button;
    
    void Start()
    {
        button.onClick.AddListener(OnNumberTapped);
    }
    
    void OnNumberTapped()
    {
        // Kirim angka ke AnswerSlotManager
        AnswerSlotManager.Instance.FillNextSlot(number);
        
        // Disable button dan hide
        button.interactable = false;
        gameObject.SetActive(false);
        
        // Play tap sound
        AudioManager.PlaySFX("tap");
    }
}
```

**AnswerSlotManager Script:**
```csharp
public class AnswerSlotManager : MonoBehaviour
{
    public List<AnswerSlot> slots; // 2, 4, atau 6 slot
    private int currentSlotIndex = 0;
    
    public void FillNextSlot(int number)
    {
        if (currentSlotIndex < slots.Count)
        {
            slots[currentSlotIndex].SetNumber(number);
            currentSlotIndex++;
            
            // Highlight slot berikutnya
            if (currentSlotIndex < slots.Count)
                slots[currentSlotIndex].Highlight();
        }
    }
    
    public List<int> GetAnswers()
    {
        return slots.Select(s => s.GetNumber()).ToList();
    }
}
```

**Keuntungan Sistem Pilihan Ganda:**
- Tidak ada typo atau kesalahan input
- Mobile-friendly (tap, bukan ketik)
- Visual lebih jelas dan intuitif
- Pemain fokus pada logika, bukan input method
- Cocok untuk berbagai usia dan kemampuan

#### **6. Validasi Jawaban (Include Wajib)**

Ketika pemain menekan tombol **CHECK**, method `VerifyAnswer()` akan dipanggil. Ini adalah relasi **include** yang wajib dilakukan. Proses validasi:

**a. Ambil Jawaban dari Slot**
```csharp
// Ambil angka dari slot yang terisi
List<int> playerAnswers = AnswerSlotManager.Instance.GetAnswers();

// Level 1: 2 angka (1 pecahan)
if (currentLevel == 1)
{
    int numerator = playerAnswers[0];   // Slot 1
    int denominator = playerAnswers[1]; // Slot 2
}

// Level 2: 4 angka (2 pecahan)
if (currentLevel == 2)
{
    int num1 = playerAnswers[0];   // Sin α numerator
    int den1 = playerAnswers[1];   // Sin α denominator
    int num2 = playerAnswers[2];   // Tan β numerator
    int den2 = playerAnswers[3];   // Tan β denominator
}

// Level 3: 3 angka (rumus Pythagoras)
if (currentLevel == 3)
{
    int sideA = playerAnswers[0];  // a
    int sideB = playerAnswers[1];  // b
    int sideC = playerAnswers[2];  // c
}
```

**b. Validasi Exact Match (Tanpa Tolerance)**  
Karena semua jawaban berupa **integer** dari pilihan ganda, sistem menggunakan **exact equality check** tanpa tolerance:

**Level 1 Validation:**
```csharp
public bool ValidateLevel1Answer(int playerNum, int playerDen)
{
    // Cek apakah pecahan sama (bisa dalam bentuk simplified)
    // Contoh: 3/5 sama dengan 6/10 (simplify dulu)
    
    int gcdPlayer = GCD(playerNum, playerDen);
    int gcdCorrect = GCD(correctNumerator, correctDenominator);
    
    int playerNumSimplified = playerNum / gcdPlayer;
    int playerDenSimplified = playerDen / gcdPlayer;
    int correctNumSimplified = correctNumerator / gcdCorrect;
    int correctDenSimplified = correctDenominator / gcdCorrect;
    
    return (playerNumSimplified == correctNumSimplified && 
            playerDenSimplified == correctDenSimplified);
}

// Fungsi GCD (Greatest Common Divisor)
int GCD(int a, int b)
{
    while (b != 0)
    {
        int temp = b;
        b = a % b;
        a = temp;
    }
    return a;
}
```

**Level 2 Validation:**
```csharp
public bool ValidateLevel2Answer(List<int> playerAnswers)
{
    // Validasi 2 pecahan sekaligus
    bool fraction1Correct = ValidateFraction(
        playerAnswers[0], playerAnswers[1],
        correctAnswer1Num, correctAnswer1Den
    );
    
    bool fraction2Correct = ValidateFraction(
        playerAnswers[2], playerAnswers[3],
        correctAnswer2Num, correctAnswer2Den
    );
    
    return fraction1Correct && fraction2Correct;
}
```

**Level 3 Validation (Pythagoras):**
```csharp
public bool ValidateLevel3Answer(int a, int b, int c)
{
    // Cek apakah memenuhi a² + b² = c²
    // Juga cek urutan angka (mana yang miring)
    
    int sqA = a * a;
    int sqB = b * b;
    int sqC = c * c;
    
    // Miring harus yang terbesar
    if (c != miringValue)
        return false;
    
    // Cek Pythagoras theorem
    return (sqA + sqB == sqC);
}
```

Contoh Validasi:
```
Level 1:
- Correct: 3/5 (Sin θ)
- Player: [3][5] → 3/5 → ✓ BENAR
- Player: [6][10] → 6/10 = 3/5 (simplified) → ✓ BENAR
- Player: [4][5] → 4/5 → ✗ SALAH

Level 2:
- Correct: Sin α = 5/13, Cos β = 12/13
- Player: [5][13][12][13] → ✓ BENAR
- Player: [5][13][13][12] → ✗ SALAH (urutan terbalik)

Level 3:
- Correct: 8² + 15² = 17² (cari miring)
- Player: [8][15][17] → 64 + 225 = 289 → ✓ BENAR
- Player: [15][8][17] → 225 + 64 = 289 → ✓ BENAR (urutan a,b boleh tukar)
- Player: [8][17][15] → ✗ SALAH (miring harus di akhir)
```

**c. Feedback Visual & Audio**  
Jika jawaban benar:
- Highlight sisi yang relevan dengan warna **HIJAU TERANG**
- Tampilkan teks "BENAR!" dengan animasi scale up
- Play **SFX correct** (suara ding positif)
- Increment score: `score += 10`
- Tampilkan "+10" dengan animasi float up

Jika jawaban salah:
- Highlight sisi yang relevan dengan warna **MERAH TERANG**
- Tampilkan teks "SALAH! Coba lagi!" dengan shake animation
- Play **SFX wrong** (suara buzz negatif)
- Kurangi lives: `lives -= 1`
- Update UI lives (satu hati berubah jadi hitam/hilang)
- Tampilkan feedback: "Jawaban yang benar: 0.6 atau 3/5"

#### **7. Update Progress (Include Wajib)**

Setelah validasi, method `UpdateProgress()` akan dipanggil (relasi include wajib):

**a. Update Progres Soal**
```csharp
progres++; // Increment dari 0 ke 1, 2, 3, ..., 30
uiManager.UpdateProgressText(progres, totalSoal); // "Soal: 2/30"
```

**b. Update Score Display**
```csharp
uiManager.UpdateScore(score); // "Score: 10"
```

**c. Cek Kondisi Game Over**  
Jika lives = 0:
```csharp
if (lives <= 0)
{
    GameOver();
    // Tampilkan Game Over Panel
    // Final Score: 30
    // Options: Restart Level, Back to Menu
}
```

Game Over Panel menggunakan **HighScoreManager** singleton untuk cek apakah score saat ini lebih tinggi dari highscore sebelumnya:
```csharp
int currentHighScore = HighScoreManager.GetInstance().LoadHighScore();
if (score > currentHighScore)
{
    HighScoreManager.GetInstance().SaveScore(score);
    // Tampilkan "NEW HIGH SCORE!" dengan animasi confetti
}
```

**d. Cek Level Complete**  
Jika progres = 30 (semua soal selesai) dan lives > 0:
```csharp
if (progres >= totalSoal)
{
    LevelComplete();
    // Tampilkan End Cutscene
    // Final Score + Stars (1-3 based on performance)
    // Unlock next level
}
```

#### **8. Simpan Score (Include Wajib)**

Setiap kali level selesai (complete atau game over), method `SaveLevelScore()` akan dipanggil (relasi include wajib). Ini menggunakan **HighScoreManager** singleton dengan **PlayerPrefs** untuk persistence:

```csharp
private void SaveLevelScore()
{
    string levelKey = ""; // "Level1Score", "Level2Score", "Level3Score"
    
    // Tentukan key based on starting question
    if (startingQuestion == 1)
        levelKey = "Level1Score";
    else if (startingQuestion == 11)
        levelKey = "Level2Score";
    else if (startingQuestion == 21)
        levelKey = "Level3Score";
    
    // Load current high score
    int currentHighScore = PlayerPrefs.GetInt(levelKey, 0);
    
    // Save only if new score is higher
    if (score > currentHighScore)
    {
        PlayerPrefs.SetInt(levelKey, score);
        PlayerPrefs.Save();
        Debug.Log($"New high score for {levelKey}: {score}");
    }
}
```

Score disimpan secara lokal di device menggunakan Unity's PlayerPrefs system, sehingga tetap ada bahkan setelah game ditutup.

#### **9. Next Round atau End Game**

Setelah feedback ditampilkan (2 detik delay), sistem akan:

**Jika lives > 0 dan progres < 30:**
```csharp
StartCoroutine(NextRoundDelay());
// Wait 2 seconds
// StartNewRound() → Generate soal baru
// Loop kembali ke step 2
```

**Jika lives = 0:**
```csharp
GameOver();
// Tampilkan Game Over Panel dengan score
// Options: Restart, Back to Menu
```

**Jika progres = 30:**
```csharp
LevelComplete();
// Tampilkan End Cutscene
// Calculate stars: 
//   3 stars: score >= 250 (25+ soal benar)
//   2 stars: score >= 150 (15+ soal benar)
//   1 star: score >= 50 (5+ soal benar)
```

---

### 🎲 **E. GAMEPLAY CORE - CHAPTER 2 (TEMBAKAN MERIAM)**

Chapter 2 mengaplikasikan trigonometri dalam konteks proyektil. Gameplay berbeda dari Chapter 1:

#### **1. Konsep Gameplay**

Pemain berperan sebagai operator meriam yang harus menembak target di jarak tertentu. Mereka harus menghitung **sudut elevasi (θ)** yang tepat menggunakan rumus proyektil:

**Rumus Gerak Parabola:**
```
Range (R) = (v₀² × sin(2θ)) / g

Dimana:
- R = jarak target (meter)
- v₀ = kecepatan awal proyektil (m/s)
- θ = sudut elevasi (derajat)
- g = gravitasi (9.8 m/s²)
```

**Inverse untuk cari sudut:**
```
θ = 0.5 × arcsin((R × g) / v₀²)
```

#### **2. Generate Soal**

**GameManagerChapter2** generate soal dengan:
- Random target distance: 5m - 100m
- Fixed initial velocity: v₀ = 30 m/s
- Hitung correct angle menggunakan inverse formula
- Contoh: Target 50m → θ ≈ 35.7°

#### **3. Visualisasi Dinamis**

Berbeda dengan Chapter 1 yang statis, Chapter 2 memiliki:
- **Meriam 3D model** yang bisa rotate
- **Target object** (barrel, dummy) di jarak tertentu
- **Trajectory line** menunjukkan path proyektil (parabola)
- **Physics simulation** dengan Unity Rigidbody2D

#### **4. Input & Validasi**

Pemain input sudut elevasi (misal: "35" atau "35.5"):
```csharp
float playerAngle = float.Parse(inputField.text);
float correctAngle = calculatedAngle;
float tolerance = 2f; // Tolerance lebih besar karena dinamis

bool isCorrect = Mathf.Abs(playerAngle - correctAngle) <= tolerance;
```

#### **5. Simulasi Tembakan**

Jika jawaban benar:
- Meriam rotate ke sudut yang diinput
- Fire projectile dengan Physics2D.AddForce
- Trajectory line muncul menunjukkan path
- Jika hit target → +10 points, next question
- Jika miss → show correct angle, next question

Jika jawaban salah:
- Show error message: "Sudut terlalu tinggi/rendah"
- Lives -1
- Hint: "Target di 50m, coba sudut 30-40 derajat"

---

### 🎯 **F. FITUR EXTEND (OPSIONAL)**

#### **1. Pause Game (Extend)**

Saat gameplay, pemain dapat mengklik tombol **PAUSE** (relasi extend - opsional). Ini akan:
- Freeze gameplay dengan `Time.timeScale = 0`
- Tampilkan Pause Panel dengan 3 options:
  * **RESUME**: Lanjutkan game (`Time.timeScale = 1`)
  * **RESTART**: Reset level (lives=3, progres=0, score=0)
  * **EXIT**: Kembali ke Level Selection
- Background blur/darken untuk fokus pada menu

#### **2. Restart Level (Extend)**

Dari Pause Menu atau Game Over Panel, pemain dapat **RESTART**:
```csharp
public void RestartLevel()
{
    lives = 3;
    progres = 0;
    score = 0;
    uiManager.UpdateLives(lives);
    uiManager.UpdateScore(score);
    StartNewRound();
}
```

#### **3. Buka Materi Ulang (Extend)**

Dari panel **Level Selection**, pemain dapat menekan tombol **MATERI** untuk membuka kembali panel materi dan tutorial tanpa harus membaca ulang Story Panel. Ini adalah relasi extend karena opsional - fitur untuk review materi.

```csharp
public void OpenMateriFromLevelSelection()
{
    // Buka panel materi saja, skip story panel
    materiPanel.SetActive(true);
    storyPanel.SetActive(false);
    
    // Start dari slide materi pertama
    currentMateriIndex = 0;
    DisplayMateriSlide(currentMateriIndex);
}
```

Tombol ini berguna untuk:
- Review konsep trigonometri sebelum memulai level
- Mengingat kembali rumus Sin, Cos, Tan
- Melihat contoh perhitungan Pythagoras
- Belajar ulang tanpa harus replay story

#### **4. Lihat Highscore (Extend)**

Dari Main Menu, pemain dapat menekan tombol **HIGHSCORE** untuk melihat leaderboard:
- **Level 1 High Score**: 280 / 300
- **Level 2 High Score**: 250 / 300  
- **Level 3 High Score**: 210 / 300
- **Total High Score**: 740 / 900

Data diambil dari PlayerPrefs:
```csharp
int level1Score = PlayerPrefs.GetInt("Level1Score", 0);
int level2Score = PlayerPrefs.GetInt("Level2Score", 0);
int level3Score = PlayerPrefs.GetInt("Level3Score", 0);
int totalScore = level1Score + level2Score + level3Score;
```

Panel juga menampilkan **achievement stars** untuk setiap level berdasarkan persentase score.

#### **5. Atur Audio (Extend)**

Dari Main Menu, tombol **SETTINGS** (biasanya icon gear) membuka Audio Settings Panel:
- **BGM Volume Slider**: 0-100%
- **SFX Volume Slider**: 0-100%
- **Mute Toggle**: On/Off untuk BGM dan SFX terpisah

Settings disimpan di PlayerPrefs:
```csharp
PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
AudioManager.SetBGMVolume(bgmVolume);
AudioManager.SetSFXVolume(sfxVolume);
```

---

### 📊 **G. MATERI TRIGONOMETRI YANG DIGUNAKAN**

#### **1. Perbandingan Trigonometri (Chapter 1)**

Game mengajarkan 3 perbandingan dasar dalam segitiga siku-siku:

**Sin θ (Sinus Theta)**
```
Sin θ = Sisi Depan / Sisi Miring
      = Opposite / Hypotenuse

Contoh: Segitiga (3,4,5)
Sin θ = 3/5 = 0.6
```

**Cos θ (Cosinus Theta)**
```
Cos θ = Sisi Samping / Sisi Miring
      = Adjacent / Hypotenuse

Contoh: Segitiga (3,4,5)
Cos θ = 4/5 = 0.8
```

**Tan θ (Tangen Theta)**
```
Tan θ = Sisi Depan / Sisi Samping
      = Opposite / Adjacent

Contoh: Segitiga (3,4,5)
Tan θ = 3/4 = 0.75
```

**Identitas Trigonometri:**
```
Sin²θ + Cos²θ = 1
Tan θ = Sin θ / Cos θ
```

#### **2. Teorema Pythagoras**

Setiap soal menggunakan **Pythagorean triples** (triple Pythagoras) yang memenuhi:
```
a² + b² = c²

Contoh triples:
- (3, 4, 5): 3² + 4² = 9 + 16 = 25 = 5²
- (5, 12, 13): 5² + 12² = 25 + 144 = 169 = 13²
- (8, 15, 17): 8² + 15² = 64 + 225 = 289 = 17²
- (7, 24, 25): 7² + 24² = 49 + 576 = 625 = 25²
```

Penggunaan Pythagorean triples memastikan:
- Semua nilai adalah integer (tidak ada akar irasional)
- Jawaban selalu bisa dinyatakan sebagai pecahan sederhana
- Mudah dipahami oleh pelajar tingkat menengah

#### **3. Sudut dan Radian (Chapter 1 Advanced)**

Untuk level Hard, game memperkenalkan konsep **sudut dalam berbagai orientasi**:
- Sudut 0° (segitiga horizontal)
- Sudut 30° (segitiga miring 30 derajat)
- Sudut 45° (segitiga miring 45 derajat)
- Sudut 60° (segitiga miring 60 derajat)
- Sudut 90° (segitiga vertikal)

Pemain harus mengidentifikasi sisi depan, samping, dan miring berdasarkan orientasi sudut θ.

#### **4. Gerak Proyektil (Chapter 2)**

Chapter 2 mengaplikasikan trigonometri dalam fisika:

**Komponen Vektor Kecepatan:**
```
vₓ = v₀ × cos(θ)  (kecepatan horizontal)
vᵧ = v₀ × sin(θ)  (kecepatan vertikal)
```

**Waktu Tempuh:**
```
t = (2 × v₀ × sin(θ)) / g
```

**Jarak Horizontal (Range):**
```
R = (v₀² × sin(2θ)) / g

Dimana:
- v₀ = kecepatan awal
- θ = sudut elevasi
- g = gravitasi (9.8 m/s²)
```

**Sudut Optimal:**  
Untuk jarak maksimal, sudut optimal adalah **45 derajat**:
```
R_max = v₀² / g  (saat θ = 45°)
```

**Aplikasi Praktis:**
- Menembak target dengan meriam
- Melontarkan bola basket
- Menghitung jarak lompat jauh
- Trajectory roket

Game visualisasi dengan parabola trajectory yang menunjukkan path proyektil, memberikan pemahaman intuitif tentang aplikasi trigonometri dalam gerak 2D.

---

### 🎓 **H. FEEDBACK & PEMBELAJARAN**

#### **1. Feedback Immediate**

Setiap jawaban langsung diberi feedback:
- **Visual**: Highlight hijau (benar) atau merah (salah)
- **Audio**: SFX positif atau negatif
- **Text**: "BENAR! +10 poin" atau "SALAH! Jawaban: 0.6"
- **Animation**: Scale up, shake, atau float

#### **2. Progress Tracking**

UI menampilkan progress real-time:
- Soal saat ini: "Soal: 5/30"
- Lives remaining: ❤️❤️🖤 (2 lives left)
- Score: "Score: 120"
- Accuracy: (dihitung di end screen)

#### **3. End Screen Summary**

Setelah level selesai, tampilkan statistik:
```
╔═════════════════════════════════╗
║      LEVEL 1 COMPLETE!          ║
╠═════════════════════════════════╣
║ Final Score:    250 / 300       ║
║ Accuracy:       83%              ║
║ Time:           5:32             ║
║ Questions:      25 / 30 correct  ║
║ Lives Lost:     1                ║
║ Stars:          ⭐⭐⭐          ║
╚═════════════════════════════════╝
```

**Star Rating:**
- ⭐⭐⭐: 25+ correct (83%+)
- ⭐⭐: 15+ correct (50%+)
- ⭐: 5+ correct (16%+)

---

### 🎮 **I. CONTROL & UI/UX**

#### **1. Input Methods (Mobile Touch)**

Game dirancang khusus untuk **mobile devices** dengan kontrol full touch:

**Touch Controls:**
- **Tap Button**: Navigasi menu, pilih level, pilih mode
- **Tap Number**: Pilih angka untuk mengisi slot jawaban
- **Tap CHECK**: Submit jawaban untuk validasi
- **Tap PAUSE**: Buka pause menu
- **Tap BACK**: Kembali ke menu sebelumnya
- **Swipe**: Navigasi story slides (swipe left/right)
- **Pinch Zoom**: Zoom in/out pada visualisasi segitiga (opsional)
- **Back Button (Android)**: Pause game atau kembali ke menu

**Gesture Support:**
```csharp
public class TouchManager : MonoBehaviour
{
    void Update()
    {
        // Single tap detection
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                HandleTap(touch.position);
            }
        }
        
        // Swipe detection for story slides
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                float swipeDelta = touch.position.x - touch.deltaPosition.x;
                if (Mathf.Abs(swipeDelta) > 100f)
                {
                    if (swipeDelta > 0)
                        NextSlide();
                    else
                        PreviousSlide();
                }
            }
        }
    }
}
```

**Touch Feedback:**
- **Haptic Feedback**: Vibrasi ringan saat tap button atau pilih angka
- **Visual Ripple**: Efek ripple saat tap (Material Design)
- **Scale Animation**: Button scale down 0.9x saat ditekan, scale up 1.0x saat release
- **Sound Effect**: Tap sound untuk setiap interaksi

#### **2. UI Layout (Mobile Portrait)**

**Level 1 Gameplay Screen:**
```
┌─────────────────────────────────────┐
│  [BACK] Soal: 5/30    ❤️❤️❤️ [PAUSE] │ Header (10%)
├─────────────────────────────────────┤
│        🔺 TRIANGLE VISUAL           │ Visual Area
│        /|                           │ (35%)
│     5 / | 3 (Depan)                 │
│      /  |                           │
│     /____|                          │
│       4 (Samping)                   │
│   Miring = 5                        │
├─────────────────────────────────────┤
│  Hitunglah nilai dari Sinθ         │ Question (10%)
├─────────────────────────────────────┤
│      Jawaban: [__] / [__]          │ Answer Slots
│                                     │ (10%)
├─────────────────────────────────────┤
│       Pilih Angka:                  │ Number Grid
│   [3] [4] [5] [6]                   │ (25%)
│   [7] [8] [9] [12]                  │
│                                     │
│   [UNDO] [CLEAR]     [CHECK]        │ Action Buttons
├─────────────────────────────────────┤
│  Score: 40          Lives: ❤️❤️❤️   │ Footer (10%)
└─────────────────────────────────────┘
```

**Level 2 Gameplay Screen:**
```
┌─────────────────────────────────────┐
│  [BACK] Soal: 15/30   ❤️❤️🖤 [PAUSE] │ Header
├─────────────────────────────────────┤
│        🔺 C                         │ Visual Area
│        /|                           │ (30%)
│    13 / | 5                         │
│   α /  | β                          │
│     /____|                          │
│   A  12  B                          │
├─────────────────────────────────────┤
│ Hitunglah Sin α dan Cos β          │ Question
├─────────────────────────────────────┤
│  Sin α = [__] / [__]                │ Answer Slots
│  Cos β = [__] / [__]                │ (15%)
├─────────────────────────────────────┤
│       Pilih Angka:                  │ Number Grid
│   [5] [12] [13] [7] [24]            │ (25%)
│   [25] [8] [15] [17]                │
│                                     │
│   [UNDO] [CLEAR]     [CHECK]        │ Action Buttons
├─────────────────────────────────────┤
│  Score: 120         Lives: ❤️❤️🖤   │ Footer
└─────────────────────────────────────┘
```

**Level 3 Gameplay Screen:**
```
┌─────────────────────────────────────┐
│  [BACK] Soal: 25/30   ❤️❤️❤️ [PAUSE] │ Header
├─────────────────────────────────────┤
│        🔺                           │ Visual Area
│        /|                           │ (30%)
│     ? / | 8                         │
│      /  |                           │
│     /____|                          │
│       15                            │
│  Tentukan panjang sisi Miring       │
├─────────────────────────────────────┤
│  [__]² + [__]² = [__]²              │ Answer Slots
│                                     │ (15%)
├─────────────────────────────────────┤
│       Pilih Angka:                  │ Number Grid
│   [8] [15] [17] [64] [225]          │ (25%)
│   [289] [13] [84] [85] [11]         │
│                                     │
│   [UNDO] [CLEAR]     [CHECK]        │ Action Buttons
├─────────────────────────────────────┤
│  Score: 200         Lives: ❤️❤️❤️   │ Footer
└─────────────────────────────────────┘
```

**Responsive Design:**
- **Portrait Mode**: Layout default seperti di atas
- **Landscape Mode**: Visual segitiga di kiri, slot jawaban + number grid di kanan
- **Small Screen**: Font size auto-adjust, grid 3x3 instead of 4x2
- **Large Screen (Tablet)**: Bigger triangle visual, larger number buttons

#### **3. Animations**

**Drop-In Animation** (Panel masuk):
```csharp
panel.transform.localScale = Vector3.zero;
panel.transform.DOScale(Vector3.one, 0.5f)
     .SetEase(Ease.OutBack);
```

**Sink-Out Animation** (Panel keluar):
```csharp
panel.transform.DOScale(Vector3.zero, 0.3f)
     .SetEase(Ease.InBack)
     .OnComplete(() => panel.SetActive(false));
```

**Feedback Animation** (Benar/Salah):
```csharp
feedbackText.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f);
feedbackText.DOFade(0f, 1f).SetDelay(1f);
```

---

### 🔊 **J. AUDIO SYSTEM**

#### **1. Background Music (BGM)**

- **Main Menu**: Upbeat, catchy tune (loop)
- **Gameplay**: Fokus, konsentrasi musik (ambient)
- **Victory**: Celebratory fanfare
- **Game Over**: Sad, melancholic theme

#### **2. Sound Effects (SFX)**

- **Button Click**: Soft click sound
- **Correct Answer**: Ding! (positive chime)
- **Wrong Answer**: Buzz! (negative buzz)
- **Page Turn**: Swish (story slides)
- **Level Complete**: Victory jingle
- **Lives Lost**: Heart break sound

#### **3. AudioManager Implementation**

```csharp
public class AudioManager : MonoBehaviour
{
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    
    public void PlayBGM(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.Play();
    }
    
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
    
    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }
}
```

---

### 📱 **K. PERSISTENCE & SAVE SYSTEM**

#### **1. PlayerPrefs Keys**

Game menyimpan data menggunakan **Unity PlayerPrefs**:

```csharp
// High Scores
"Level1Score" → int (0-300)
"Level2Score" → int (0-300)
"Level3Score" → int (0-300)

// Audio Settings
"BGMVolume" → float (0.0-1.0)
"SFXVolume" → float (0.0-1.0)

// Progress
"Chapter1Complete" → int (0/1 boolean)
"Chapter2Complete" → int (0/1 boolean)
"UnlockedLevels" → int (bitmask: 001=L1, 011=L1+L2, 111=All)
```

#### **2. Save on Events**

Data disimpan otomatis pada:
- Level complete
- Game over
- Audio settings change
- App pause/quit

```csharp
void OnApplicationPause(bool pauseStatus)
{
    if (pauseStatus)
    {
        PlayerPrefs.Save();
    }
}

void OnApplicationQuit()
{
    PlayerPrefs.Save();
}
```

---

### 🎯 **L. WIN/LOSE CONDITIONS**

#### **1. Level Complete (Victory)**

Kondisi menang:
- Selesaikan 30 soal (progres = 30)
- Lives > 0

Rewards:
- Tampilkan End Cutscene
- Calculate final score & stars
- Save high score jika baru
- Unlock next level (if applicable)
- Achievement unlock (if criteria met)

#### **2. Game Over (Defeat)**

Kondisi kalah:
- Lives = 0 (kehilangan semua nyawa)

Game Over Screen:
- Final score
- Soal dijawab benar/salah
- Options:
  * Retry Level (restart with lives=3)
  * Back to Menu
  * Review Mistakes (optional feature)

---

### 📊 **M. DIFFICULTY PROGRESSION**

#### **1. Adaptive Difficulty**

Level 1 (Easy - 2 Slot Pecahan Tunggal):
- Triple: (3,4,5), (5,12,13), (8,15,17)
- Soal: Perbandingan trigonometri tunggal (Sin/Cos/Tan)
- Slot: 2 slot untuk 1 pecahan
- Pilihan: 8 kotak angka
- Validation: Exact match dengan simplified fraction

Level 2 (Medium - 4 Slot Pecahan Ganda):
- Triple: (5,12,13), (7,24,25), (9,40,41), (12,35,37)
- Soal: 2 perbandingan trigonometri sekaligus (Sin α & Cos β)
- Slot: 4 slot untuk 2 pecahan
- Pilihan: 10-12 kotak angka
- Validation: Kedua pecahan harus benar dan urutan tepat

Level 3 (Hard - 6 Slot Rumus Pythagoras):
- Triple: (8,15,17), (11,60,61), (13,84,85), (36,77,85)
- Soal: Rumus Pythagoras dengan 1 sisi tidak diketahui
- Slot: 3 slot untuk a², b², c²
- Pilihan: 12-15 kotak angka (termasuk angka kuadrat)
- Validation: Harus memenuhi a² + b² = c²

#### **2. Learning Curve**

Level 1: Introduction to Ratios
- Soal 1-10: Perbandingan dasar Sin/Cos/Tan
- Triple sederhana: (3,4,5), (5,12,13)
- Fokus: Identifikasi sisi Depan, Samping, Miring
- Sistem: Pilih 2 angka untuk 1 pecahan

Level 2: Multiple Ratios & Dual Angles
- Soal 11-20: Dua perbandingan sekaligus
- Triple kompleks: (7,24,25), (9,40,41)
- Fokus: Memahami 2 sudut berbeda (α dan β)
- Sistem: Pilih 4 angka untuk 2 pecahan berurutan

Level 3: Pythagorean Theorem Application
- Soal 21-30: Aplikasi teorema Pythagoras
- Triple menantang: (11,60,61), (13,84,85)
- Fokus: Menghitung sisi yang tidak diketahui
- Sistem: Pilih 3 angka untuk rumus a² + b² = c²

---

## 🎓 KESIMPULAN

Game Trigosolver mengimplementasikan pembelajaran trigonometri melalui gameplay interaktif berbasis **mobile touch** dengan sistem **pilihan ganda tap-to-select**. Dengan 2 mode permainan (Cerita dan Bebas), 2 chapter (Observasi Segitiga dan Tembakan Meriam), dan 3 level kesulitan dengan mekanisme unik (2 slot, 4 slot, 6 slot), game ini menyediakan pengalaman belajar yang progresif dan terstruktur.

**Fitur Utama:**
- **Sistem Slot Progresif**: Level 1 (2 slot pecahan tunggal), Level 2 (4 slot pecahan ganda), Level 3 (6 slot rumus Pythagoras)
- **Pilihan Ganda Interaktif**: Tap angka dari kotak-kotak prefab, bukan ketikan manual
- **Validasi Exact Match**: Semua jawaban integer dengan simplified fraction comparison
- **Visualisasi Segitiga**: Setiap soal dilengkapi gambar segitiga dengan label panjang sisi
- **Materi Lengkap**: Sin, Cos, Tan, Teorema Pythagoras dengan Pythagorean triples
- **Story-Driven Learning**: Mode Cerita dengan narasi kontekstual (wajib di awal, bisa review via button MATERI di level selection)

**Mekanika Pembelajaran:**
- Lives system (3 nyawa) untuk challenge
- Score tracking (+10 per benar) untuk motivasi
- High score persistence dengan PlayerPrefs
- Feedback visual & audio immediate tanpa hint system
- Mobile-first design dengan full touch control

Mekanika inti menggunakan **include relationships** untuk proses wajib (generate soal, visualisasi, validasi, simpan score) dan **extend relationships** untuk fitur opsional (pause, restart, review materi, highscore, audio settings), menciptakan pengalaman gameplay mobile yang intuitif, terstruktur, dan efektif untuk pembelajaran trigonometri tingkat menengah.

---

**End of Documentation**
