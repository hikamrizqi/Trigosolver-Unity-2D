# 📚 DOKUMENTASI SISTEM CHAPTER 1 - TRIGOSOLVER

## 📋 Daftar Isi
1. [Arsitektur Sistem](#arsitektur-sistem)
2. [Script-script Utama](#script-script-utama)
3. [Data Flow](#data-flow)
4. [Gameplay Loop](#gameplay-loop)
5. [Diagram Flow Lengkap](#diagram-flow-lengkap)
6. [Setup Guide](#setup-guide)
7. [Troubleshooting](#troubleshooting)

---

## 🏗️ ARSITEKTUR SISTEM

Chapter 1 adalah gameplay inti untuk latihan dasar trigonometri dengan sistem tanya-jawab berbasis visualisasi segitiga siku-siku.

### **Komponen Utama:**

```
┌─────────────────────────────────────────────────┐
│           CHAPTER 1 ARCHITECTURE                │
├─────────────────────────────────────────────────┤
│                                                 │
│  CalculationManager (Game Controller)          │
│         │                                       │
│         ├──► TriangleDataGenerator             │
│         │         (Question Generator)          │
│         │                                       │
│         ├──► UIManagerChapter1                 │
│         │         (UI Controller)               │
│         │         │                             │
│         │         └──► TriangleVisualizer       │
│         │                   (Visual Renderer)   │
│         │                                       │
│         └──► Chapter1EndCutscene                │
│                   (End Game Handler)            │
│                                                 │
└─────────────────────────────────────────────────┘
```

---

## 📜 SCRIPT-SCRIPT UTAMA

### **1. CalculationManager.cs** (Game Controller)

**Fungsi:** Mengatur game loop, validasi jawaban, score, lives

**Variabel Public:**

| Variabel | Tipe | Fungsi |
|----------|------|--------|
| `uiManager` | UIManagerChapter1 | Reference ke UI controller |
| `dataGenerator` | TriangleDataGenerator | Reference ke question generator |
| `endCutscene` | Chapter1EndCutscene | Reference ke end game handler |
| `answerTolerance` | float | Toleransi jawaban desimal (0.01) |

**Variabel Private:**

| Variabel | Tipe | Fungsi |
|----------|------|--------|
| `lives` | int | Nyawa pemain (default: 3) |
| `progres` | int | Soal ke berapa (1-5) |
| `totalSoal` | int | Total soal per chapter (default: 5) |
| `score` | int | Score pemain (benar +10 poin) |
| `dataSoalSaatIni` | TriangleData | Data soal yang sedang ditampilkan |

**Fungsi-fungsi:**

#### `Start()`
```csharp
void Start()
```

**Dipanggil:** Unity lifecycle (otomatis saat scene load)

**Alur:**
```
1. progres = 0
2. lives = 3
3. uiManager.UpdateLives(3) → Update UI nyawa
4. StartNewRound() → Generate soal pertama
```

---

#### `StartNewRound()`
```csharp
void StartNewRound()
```

**Dipanggil:** 
- Dari `Start()` (soal pertama)
- Dari `NextRoundDelay()` coroutine (soal selanjutnya)

**Alur:**
```
1. Cek progres >= totalSoal:
   → Jika ya: EndChapter()
   → Jika tidak: Lanjut ke step 2

2. progres++  (increment soal)

3. dataSoalSaatIni = dataGenerator.GenerateNewQuestion()
   → Generate triple Pythagoras random
   → Generate soal random (Sin/Cos/Tan)
   → Hitung jawaban benar

4. uiManager.SetupNewQuestion(progres, totalSoal, dataSoalSaatIni)
   → Update UI pertanyaan
   → Update visualisasi segitiga
   → Reset input field
```

**Guard Clause:**
```
IF progres >= totalSoal THEN
    EndChapter()
    RETURN
END IF
```

---

#### `VerifyAnswer()`
```csharp
public void VerifyAnswer()
```

**Dipanggil:** 
- Button "CHECK" onClick (dari Inspector)
- User tekan Enter di input field

**Alur:**
```
1. Ambil input dari jawabanInput.text
2. Trim whitespace
3. Validasi input kosong:
   → Jika kosong: ShowFeedback("Masukkan jawaban!")
   
4. Parse input:
   a. Jika format pecahan (3/5):
      - Split dengan '/'
      - Parse numerator dan denominator
      - playerAnswer = numerator / denominator
      
   b. Jika format desimal (0.6):
      - Replace koma dengan titik
      - Parse dengan InvariantCulture
      
   c. Jika invalid:
      - HandleWrongAnswer("Format salah!")

5. Bandingkan jawaban:
   absError = |playerAnswer - JawabanBenar|
   
   a. Jika absError <= answerTolerance (0.01):
      ✅ BENAR:
      - score += 10
      - ShowCorrectFeedback("+10 Poin")
      - HighlightCorrectAnswer() → Hijau + sparkle
      - StartCoroutine(NextRoundDelay(2s))
      
   b. Jika absError > tolerance:
      ❌ SALAH:
      - HandleWrongAnswer()
```

**Input Parsing Logic:**

```
Input: "3/5"
  ↓ Split('/')
parts[0] = "3", parts[1] = "5"
  ↓ Parse
numerator = 3.0f, denominator = 5.0f
  ↓ Calculate
playerAnswer = 3.0 / 5.0 = 0.6

Input: "0,6"
  ↓ Replace(',', '.')
"0.6"
  ↓ Parse (InvariantCulture)
playerAnswer = 0.6
```

---

#### `HandleWrongAnswer(string customMessage = "")`
```csharp
void HandleWrongAnswer(string customMessage = "")
```

**Dipanggil:** 
- Dari `VerifyAnswer()` saat jawaban salah
- Dengan atau tanpa custom message

**Alur:**
```
1. lives--  (kurangi nyawa)
2. uiManager.UpdateLives(lives) → Update UI

3. Cek lives <= 0:
   
   a. GAME OVER:
      - ShowFeedback("GAME OVER!")
      - StartCoroutine(ShowGameOverAfterDelay(2s))
         → endCutscene.ShowGameOver(score)
   
   b. MASIH ADA NYAWA:
      - Default message = "SALAH! Perhatikan rumusnya..."
      - Atau gunakan customMessage jika ada
      - ShowFeedback(false, message)
      - HighlightWrongAnswer(SoalType) → Merah
      - StartCoroutine(NextRoundDelay(2s))
         → Ganti soal baru
```

**Decision Tree:**
```
lives-- 
  ↓
lives > 0? ──No──► ShowGameOverAfterDelay(2s)
  │                     ↓
  Yes              ShowGameOver(score)
  ↓
ShowFeedback("SALAH!")
  ↓
HighlightWrongAnswer()
  ↓
NextRoundDelay(2s)
  ↓
StartNewRound()
```

---

#### `NextRoundDelay()` (Coroutine)
```csharp
IEnumerator NextRoundDelay()
```

**Dipanggil:**
- Setelah jawaban benar
- Setelah jawaban salah (masih ada nyawa)

**Alur:**
```
1. yield return WaitForSeconds(2.0f)
   → Beri pemain waktu baca feedback

2. StartNewRound()
   → Generate soal baru
```

**Timeline:**
```
0.0s ────────────── 2.0s
│                    │
Feedback visible     StartNewRound()
(user membaca)       (soal baru muncul)
```

---

#### `EndChapter()`
```csharp
void EndChapter()
```

**Dipanggil:** Dari `StartNewRound()` saat progres >= totalSoal

**Alur:**
```
1. ShowFeedback(true, "CHAPTER 1 SELESAI! Skor: {score}")

2. Jika endCutscene ada:
   StartCoroutine(ShowEndCutsceneAfterDelay(2s))
      ↓
   endCutscene.ShowEndCutscene(score, totalSoal)
```

---

### **2. TriangleDataGenerator.cs** (Question Generator)

**Fungsi:** Generate soal trigonometri dengan Pythagoras triples

**Variabel:**

| Variabel | Tipe | Nilai | Fungsi |
|----------|------|-------|--------|
| `triples` | List<(int,int,int)> | (3,4,5), (5,12,13), (8,15,17), (7,24,25) | Pythagoras triples |

**Class TriangleData:**

```csharp
public class TriangleData
{
    public int Depan;              // Sisi depan (opposite)
    public int Samping;            // Sisi samping (adjacent)
    public int Miring;             // Sisi miring (hypotenuse)
    public string SoalDisederhanakan;  // "Sinθ", "Cosθ", atau "Tanθ"
    public float JawabanBenar;     // Hasil perhitungan
}
```

#### `GenerateNewQuestion()`
```csharp
public TriangleData GenerateNewQuestion()
```

**Alur:**
```
1. Random pilih triple dari list:
   triple = triples[Random.Range(0, 4)]
   Contoh: (3, 4, 5)

2. Random orientasi (a atau b sebagai depan):
   isADepan = Random.Range(0, 2) == 0
   
   Jika isADepan = true:
      Depan = triple.a = 3
      Samping = triple.b = 4
      Miring = triple.c = 5
   
   Jika isADepan = false:
      Depan = triple.b = 4
      Samping = triple.a = 3
      Miring = triple.c = 5

3. Random tipe soal:
   questionType = Random.Range(0, 3)
   
   Case 0 (Sin):
      SoalDisederhanakan = "Sinθ"
      JawabanBenar = Depan / Miring
      Contoh: 3/5 = 0.6
   
   Case 1 (Cos):
      SoalDisederhanakan = "Cosθ"
      JawabanBenar = Samping / Miring
      Contoh: 4/5 = 0.8
   
   Case 2 (Tan):
      SoalDisederhanakan = "Tanθ"
      JawabanBenar = Depan / Samping
      Contoh: 3/4 = 0.75

4. Return TriangleData
```

**Contoh Output:**
```csharp
{
    Depan: 3,
    Samping: 4,
    Miring: 5,
    SoalDisederhanakan: "Sinθ",
    JawabanBenar: 0.6f
}
```

**Rumus Trigonometri:**
```
       |\
       | \
Depan  |  \ Miring
       |   \
       |____\
      Samping

Sinθ = Depan / Miring    (opposite / hypotenuse)
Cosθ = Samping / Miring  (adjacent / hypotenuse)
Tanθ = Depan / Samping   (opposite / adjacent)
```

---

### **3. UIManagerChapter1.cs** (UI Controller)

**Fungsi:** Mengatur semua UI update, feedback, dan visual highlight

**Referensi UI Canvas:**

| Variabel | Tipe | Fungsi |
|----------|------|--------|
| `judulText` | TextMeshProUGUI | Judul chapter |
| `progresText` | TextMeshProUGUI | "Soal: 1/5" |
| `livesIcons` | GameObject[] | Array icon hati (3 nyawa) |
| `pertanyaanText` | TextMeshProUGUI | "Berapakah nilai Sinθ?" |
| `jawabanInput` | TMP_InputField | Input field untuk jawaban |
| `feedbackPanel` | GameObject | Panel feedback (benar/salah) |
| `feedbackText` | TextMeshProUGUI | Text di feedback panel |

**Referensi World Space:**

| Variabel | Tipe | Fungsi |
|----------|------|--------|
| `triangleVisualizer` | TriangleVisualizer | Controller visualisasi segitiga |
| `depanLabel_World` | TextMeshProUGUI | Label nilai depan (UI) |
| `sampingLabel_World` | TextMeshProUGUI | Label nilai samping (UI) |
| `miringLabel_World` | TextMeshProUGUI | Label nilai miring (UI) |
| `thetaLabel_World` | TextMeshPro | Label simbol θ (World Space) |
| `depanSprite` | SpriteRenderer | Sprite sisi depan |
| `sampingSprite` | SpriteRenderer | Sprite sisi samping |
| `miringSprite` | SpriteRenderer | Sprite sisi miring |

**Settings:**

| Variabel | Tipe | Default | Fungsi |
|----------|------|---------|--------|
| `defaultColor` | Color | White | Warna normal sprite |
| `highlightKuning` | Color | Yellow | Warna highlight biasa |
| `highlightMerah` | Color | Red | Warna jawaban salah |
| `highlightHijau` | Color | Green | Warna jawaban benar |
| `sparkleEffect` | ParticleSystem | - | Efek sparkle untuk benar |
| `audioManager` | Chapter1AudioManager | - | Audio controller |

#### `SetupNewQuestion(int progres, int totalSoal, TriangleData data)`
```csharp
public void SetupNewQuestion(int progres, int totalSoal, TriangleData data)
```

**Dipanggil:** Dari `CalculationManager.StartNewRound()`

**Alur:**
```
1. Update UI Canvas:
   progresText.text = "Soal: {progres}/{totalSoal}"
   Contoh: "Soal: 1/5"

2. Update pertanyaan:
   pertanyaanText.text = ""  (clear dulu)
   pertanyaanText.ForceMeshUpdate()  (refresh mesh)
   pertanyaanText.text = "Berapakah nilai {SoalDisederhanakan}?"
   Contoh: "Berapakah nilai Sinθ?"
   pertanyaanText.ForceMeshUpdate()

3. Reset input:
   jawabanInput.text = ""
   feedbackPanel.SetActive(false)

4. Update label nilai:
   depanLabel_World.text = data.Depan.ToString()
   sampingLabel_World.text = data.Samping.ToString()
   miringLabel_World.text = data.Miring.ToString()

5. Gambar segitiga:
   IF triangleVisualizer != null:
      triangleVisualizer.DrawTriangle(depan, samping, miring)
   ELSE:
      ResetSideColors() (fallback)
```

**Double ForceMeshUpdate():**
- Pertama: Clear rendering
- Kedua: Force render text baru
- Fix untuk bug "Cosθ" tidak muncul karena text overflow

---

#### `UpdateLives(int currentLives)`
```csharp
public void UpdateLives(int currentLives)
```

**Dipanggil:** 
- `CalculationManager.Start()` (init)
- `CalculationManager.HandleWrongAnswer()` (update)

**Alur:**
```
FOR i = 0 TO livesIcons.Length - 1:
    IF i < currentLives:
        livesIcons[i].SetActive(true)  → Tampilkan hati
    ELSE:
        livesIcons[i].SetActive(false) → Sembunyikan hati
```

**Contoh:**
```
currentLives = 2

livesIcons[0].SetActive(true)   → ❤️ (visible)
livesIcons[1].SetActive(true)   → ❤️ (visible)
livesIcons[2].SetActive(false)  → 💔 (hidden)
```

---

#### `ShowFeedback(bool isCorrect, string message)`
```csharp
public void ShowFeedback(bool isCorrect, string message)
```

**Dipanggil:**
- `VerifyAnswer()` (input kosong)
- `ShowCorrectFeedback()`
- `HandleWrongAnswer()`
- `EndChapter()`

**Alur:**
```
1. feedbackPanel.SetActive(true)
2. feedbackText.text = message
3. feedbackText.color = isCorrect ? hijau : merah
```

---

#### `HighlightCorrectAnswer()`
```csharp
public void HighlightCorrectAnswer()
```

**Dipanggil:** Setelah jawaban benar

**Alur:**
```
1. Highlight SEMUA sisi hijau:
   triangleVisualizer.HighlightSide("depan", hijau)
   triangleVisualizer.HighlightSide("samping", hijau)
   triangleVisualizer.HighlightSide("miring", hijau)

2. Aktifkan sparkle effect:
   IF sparkleEffect != null:
      sparkleEffect.Play()
```

---

#### `HighlightWrongAnswer(string soalType)`
```csharp
public void HighlightWrongAnswer(string soalType)
```

**Dipanggil:** Setelah jawaban salah

**Alur:**
```
1. Reset warna semua sisi

2. Highlight merah berdasarkan soal:
   
   Sinθ = Depan/Miring:
      - Highlight depan → merah
      - Highlight miring → merah
   
   Cosθ = Samping/Miring:
      - Highlight samping → merah
      - Highlight miring → merah
   
   Tanθ = Depan/Samping:
      - Highlight depan → merah
      - Highlight samping → merah
```

**Visual Feedback:**
```
Sinθ (salah):
   |\
   |●\     ● = merah (depan & miring)
 3 |  \ 5
   |___\
     4

Cosθ (salah):
   |\
   | \●    ● = merah (samping & miring)
 3 |  \ 5
   |●__\
     4

Tanθ (salah):
   |\
   |●\     ● = merah (depan & samping)
 3 |  \
   |●__\
     4
```

---

### **4. TriangleVisualizer.cs** (Visual Renderer)

**Fungsi:** Render segitiga dinamis dengan 3 sprite vertikal

**Referensi Sprite:**

| Variabel | Tipe | Fungsi |
|----------|------|--------|
| `depanSprite` | SpriteRenderer | Sprite sisi depan (vertikal) |
| `sampingSprite` | SpriteRenderer | Sprite sisi samping (horizontal) |
| `miringSprite` | SpriteRenderer | Sprite sisi miring (diagonal) |

**Settings:**

| Variabel | Tipe | Default | Fungsi |
|----------|------|---------|--------|
| `baseScale` | float | 0.5f | Skala sprite (1 unit = 1 nilai) |
| `centerPosition` | Vector3 | (0,0,0) | Offset pusat segitiga |
| `labelOffset` | float | 0.5f | Jarak label dari garis |
| `lineThickness` | float | 20f | Ketebalan garis |

#### `DrawTriangle(int depan, int samping, int miring)`
```csharp
public void DrawTriangle(int depan, int samping, int miring)
```

**Dipanggil:** Dari `UIManagerChapter1.SetupNewQuestion()`

**Alur:**
```
1. Simpan data:
   currentDepan = depan
   currentSamping = samping
   currentMiring = miring

2. Hitung vertex positions:
   basePosition = transform.position + centerPosition
   
   bottomLeft = basePosition
   bottomRight = bottomLeft + (samping × baseScale, 0, 0)
   topLeft = bottomLeft + (0, depan × baseScale, 0)

3. Position & scale sprites:
   a. Sisi Samping (horizontal):
      - Start: bottomLeft
      - End: bottomRight
      - PositionSprite(sampingSprite, start, end, samping)
      - Label di tengah bawah
   
   b. Sisi Depan (vertikal):
      - Start: bottomLeft
      - End: topLeft
      - PositionSprite(depanSprite, start, end, depan)
      - Label di tengah kiri
   
   c. Sisi Miring (diagonal):
      - Start: topLeft
      - End: bottomRight
      - PositionSprite(miringSprite, start, end, miring)
      - Label di tengah diagonal (perpendicular offset)

4. Position theta label:
   thetaPosition = bottomLeft + (0.8, 0.8, 0)
   thetaLabel.transform.position = thetaPosition
   thetaLabel.text = "θ"

5. Reset colors ke normal
```

**Koordinat System:**
```
Contoh: depan=3, samping=4, miring=5, baseScale=0.5

basePosition = (0, 0, 0)

topLeft (0, 1.5)
   |\
   | \
   |  \ miringSprite
   |   \
   |    \
   |_____\ bottomRight (2, 0)
bottomLeft (0, 0)

sampingSprite length = 4 × 0.5 = 2.0 units
depanSprite length = 3 × 0.5 = 1.5 units
miringSprite length = 5 × 0.5 = 2.5 units
```

---

#### `PositionSprite(SpriteRenderer sprite, Vector3 start, Vector3 end, float value)`
```csharp
private void PositionSprite(SpriteRenderer sprite, Vector3 start, Vector3 end, float value)
```

**Fungsi:** Position, rotate, dan scale sprite untuk membentuk garis

**Alur:**
```
1. Hitung tengah garis:
   midPoint = (start + end) / 2
   sprite.transform.position = midPoint

2. Hitung panjang garis:
   distance = Vector3.Distance(start, end)

3. Hitung sudut rotasi:
   direction = end - start
   angle = Atan2(direction.y, direction.x) × Rad2Deg
   
   ASUMSI: Sprite vertikal (Y-axis default)
   rotationOffset = angle - 90°
   sprite.transform.rotation = Quaternion.Euler(0, 0, rotationOffset)

4. Hitung scale:
   scaleY = distance / baseScale  (panjang garis)
   scaleX = lineThickness (ketebalan garis)
   sprite.transform.localScale = (scaleX, scaleY, 1)
```

**Sprite Orientation Fix:**
- Sprite default: Vertikal ↕ (elongate di Y-axis)
- Untuk horizontal: Rotate 90° → `angle - 90`
- Scale Y = panjang, Scale X = thickness

---

#### `HighlightSide(string sideName, Color color)`
```csharp
public void HighlightSide(string sideName, Color color)
```

**Alur:**
```
1. ResetColors() → Semua jadi normal

2. Switch sideName:
   "depan": depanSprite.color = color
   "samping": sampingSprite.color = color
   "miring": miringSprite.color = color
```

---

## 🔄 DATA FLOW

### **Complete Data Flow Diagram**

```
┌────────────────────────────────────────────────────────┐
│               USER INTERACTION                         │
│  (Player menjawab soal trigonometri)                   │
└───────────────────┬────────────────────────────────────┘
                    │
                    ▼
         ┌──────────────────────┐
         │ Button "CHECK" Click │
         │ (UI Event)           │
         └──────────┬───────────┘
                    │
                    ▼
    ╔═══════════════════════════════════════╗
    ║  CalculationManager.VerifyAnswer()    ║
    ║  ────────────────────────────────     ║
    ║  1. Ambil input dari TMP_InputField   ║
    ║  2. Validasi format (pecahan/desimal) ║
    ║  3. Parse input → playerAnswer        ║
    ║  4. Bandingkan dengan JawabanBenar    ║
    ╚═══════════════╦═══════════════════════╝
                    │
         ┌──────────┴──────────┐
         │                     │
    [Benar?]              [Salah?]
         │                     │
         ▼                     ▼
    ┌─────────┐          ┌─────────┐
    │ score++ │          │ lives-- │
    └────┬────┘          └────┬────┘
         │                    │
         ▼                    ▼
╔════════════════════╗  ╔═══════════════════╗
║ UIManager         ║  ║ UIManager        ║
║ ShowCorrect       ║  ║ HandleWrong      ║
║ Feedback()        ║  ║ Answer()         ║
║ ───────────       ║  ║ ──────────       ║
║ • Hijau           ║  ║ • Merah          ║
║ • Sparkle         ║  ║ • Update Lives   ║
║ • "+10 Poin"      ║  ║ • Show Answer    ║
╚════════╦═══════════╝  ╚═══════╦═══════════╝
         │                      │
         │                 ┌────┴─────┐
         │                 │          │
         │            [lives>0?]  [lives=0?]
         │                 │          │
         │                 ▼          ▼
         │          NextRoundDelay  GameOver
         │                 │
         └─────────┬───────┘
                   │
                   ▼
          WaitForSeconds(2s)
                   │
                   ▼
    ╔═══════════════════════════════════════╗
    ║  CalculationManager.StartNewRound()   ║
    ║  ────────────────────────────────     ║
    ║  1. progres++                         ║
    ║  2. IF progres >= 5: EndChapter()     ║
    ║  3. ELSE: Generate soal baru          ║
    ╚═══════════════╦═══════════════════════╝
                    │
                    ▼
    ╔═══════════════════════════════════════╗
    ║  TriangleDataGenerator               ║
    ║  GenerateNewQuestion()               ║
    ║  ────────────────────────────────    ║
    ║  1. Random Pythagoras triple         ║
    ║     (3,4,5) / (5,12,13) / dll        ║
    ║  2. Random orientasi (a/b sebagai    ║
    ║     depan)                           ║
    ║  3. Random soal type:                ║
    ║     - Sinθ = Depan/Miring            ║
    ║     - Cosθ = Samping/Miring          ║
    ║     - Tanθ = Depan/Samping           ║
    ║  4. Return TriangleData              ║
    ╚═══════════════╦═══════════════════════╝
                    │
                    ▼
              [TriangleData]
         ┌──────────┴──────────┐
         │  Depan: 3           │
         │  Samping: 4         │
         │  Miring: 5          │
         │  Soal: "Sinθ"       │
         │  JawabanBenar: 0.6  │
         └──────────┬──────────┘
                    │
                    ▼
    ╔═══════════════════════════════════════╗
    ║  UIManagerChapter1                   ║
    ║  SetupNewQuestion()                  ║
    ║  ────────────────────────────────    ║
    ║  1. Update progresText: "Soal 1/5"   ║
    ║  2. Update pertanyaanText:           ║
    ║     "Berapakah nilai Sinθ?"          ║
    ║  3. Update label: 3, 4, 5            ║
    ║  4. Reset input field                ║
    ║  5. Call TriangleVisualizer          ║
    ╚═══════════════╦═══════════════════════╝
                    │
                    ▼
    ╔═══════════════════════════════════════╗
    ║  TriangleVisualizer                  ║
    ║  DrawTriangle(3, 4, 5)               ║
    ║  ────────────────────────────────    ║
    ║  1. Hitung vertex positions:         ║
    ║     bottomLeft, bottomRight, topLeft ║
    ║  2. PositionSprite() untuk 3 sisi:   ║
    ║     - sampingSprite (horizontal)     ║
    ║     - depanSprite (vertikal)         ║
    ║     - miringSprite (diagonal)        ║
    ║  3. Position theta label di sudut    ║
    ║  4. ResetColors() → Putih            ║
    ╚═══════════════════════════════════════╝
                    │
                    ▼
         ┌──────────────────────┐
         │  VISUAL UPDATE       │
         │  ────────────────    │
         │  [Triangle rendered] │
         │  Labels displayed    │
         │  UI ready for input  │
         └──────────────────────┘
```

---

## 🎮 GAMEPLAY LOOP

### **Main Game Loop**

```
START GAME
    ↓
┌─────────────────────────────────┐
│  CalculationManager.Start()     │ ◄────────┐
│  ─────────────────────────      │          │
│  • lives = 3                    │          │
│  • progres = 0                  │          │
│  • StartNewRound()              │          │
└───────────────┬─────────────────┘          │
                │                            │
                ▼                            │
        ┌───────────────┐                    │
        │ progres++     │                    │
        │ (soal 1 → 5)  │                    │
        └───────┬───────┘                    │
                │                            │
                ▼                            │
    ┌───────────────────────┐                │
    │ Generate Question     │                │
    │ Update UI & Visual    │                │
    └───────────┬───────────┘                │
                │                            │
           [Wait User Input]                 │
                │                            │
                ▼                            │
        ┌───────────────┐                    │
        │ User Submit   │                    │
        │ Answer        │                    │
        └───────┬───────┘                    │
                │                            │
                ▼                            │
        ┌───────────────┐                    │
        │ Verify Answer │                    │
        └───┬───────┬───┘                    │
            │       │                        │
        [Benar] [Salah]                      │
            │       │                        │
            │       ▼                        │
            │   ┌────────┐                   │
            │   │lives-- │                   │
            │   └───┬────┘                   │
            │       │                        │
            │   [lives>0?]                   │
            │    │     │                     │
            │   Yes   No                     │
            │    │     │                     │
            │    │     ▼                     │
            │    │  [GAME OVER]              │
            │    │     │                     │
            │    │  EndCutscene              │
            │    │                           │
            ▼    ▼                           │
       ┌──────────────┐                     │
       │ Feedback 2s  │                     │
       └──────┬───────┘                     │
              │                             │
              ▼                             │
       ┌──────────────┐                     │
       │ progres >= 5?│                     │
       └──┬────────┬──┘                     │
         No      Yes                        │
          │       │                         │
          │       ▼                         │
          │  [CHAPTER COMPLETE]             │
          │       │                         │
          │  EndCutscene                    │
          │                                 │
          └─────────────────────────────────┘
```

---

## 📊 DIAGRAM FLOW LENGKAP

### **Scene Load → First Question**

```
SCENE "Chapter1" LOADED
        │
        ▼
┌───────────────────────┐
│ Unity Lifecycle       │
│ ─────────────────     │
│ GameObject dengan     │
│ CalculationManager    │
│ attached              │
└───────┬───────────────┘
        │
        ▼
┌───────────────────────┐
│ Awake()               │  ◄─ Otomatis (Unity)
│ (Jika ada)            │
└───────┬───────────────┘
        │
        ▼
┌───────────────────────┐
│ Start()               │  ◄─ Otomatis (Unity)
│ ─────────────────     │
│ • progres = 0         │
│ • lives = 3           │
│ • UpdateLives(3)      │
│ • StartNewRound()     │
└───────┬───────────────┘
        │
        ▼
┌───────────────────────┐
│ StartNewRound()       │  ◄─ Manual call
│ ─────────────────     │
│ • progres = 1         │
│ • Generate Question   │
│ • Setup UI            │
└───────┬───────────────┘
        │
        ▼
┌─────────────────────────────────┐
│ TriangleDataGenerator           │
│ GenerateNewQuestion()           │
│ ───────────────────────────     │
│ Random: (3,4,5), Sinθ           │
│ Return:                         │
│   Depan=3, Samping=4, Miring=5  │
│   Soal="Sinθ"                   │
│   JawabanBenar=0.6              │
└─────────┬───────────────────────┘
          │
          ▼
┌─────────────────────────────────┐
│ UIManagerChapter1               │
│ SetupNewQuestion(1, 5, data)    │
│ ───────────────────────────     │
│ • progresText = "Soal: 1/5"     │
│ • pertanyaanText =              │
│   "Berapakah nilai Sinθ?"       │
│ • depanLabel = "3"              │
│ • sampingLabel = "4"            │
│ • miringLabel = "5"             │
│ • jawabanInput = "" (clear)     │
│ • feedbackPanel hidden          │
└─────────┬───────────────────────┘
          │
          ▼
┌─────────────────────────────────┐
│ TriangleVisualizer              │
│ DrawTriangle(3, 4, 5)           │
│ ───────────────────────────     │
│ Calculate vertices:             │
│   bottomLeft = (0, 0)           │
│   bottomRight = (2, 0)          │
│   topLeft = (0, 1.5)            │
│                                 │
│ Position sprites:               │
│   sampingSprite: horizontal     │
│   depanSprite: vertical         │
│   miringSprite: diagonal        │
│                                 │
│ Position thetaLabel at corner   │
│ ResetColors() → white           │
└─────────────────────────────────┘
          │
          ▼
    [SOAL PERTAMA READY]
    [MENUNGGU INPUT USER]
```

---

### **User Answer Flow (Correct Answer)**

```
USER INPUT: "0.6"
      │
      ▼
┌──────────────────┐
│ User Click       │
│ Button "CHECK"   │
└────┬─────────────┘
     │
     ▼
┌────────────────────────────────┐
│ CalculationManager            │
│ VerifyAnswer()                │
│ ──────────────────────────    │
│ 1. input = "0.6"              │
│ 2. Trim → "0.6"               │
│ 3. Not empty ✓                │
│ 4. Not fraction → Parse float │
│    playerAnswer = 0.6f        │
│ 5. Compare:                   │
│    |0.6 - 0.6| = 0.0          │
│    0.0 <= 0.01 ✓ BENAR!       │
└────────┬───────────────────────┘
         │
         ▼
┌────────────────────────────────┐
│ JAWABAN BENAR PATH            │
│ ──────────────────────────    │
│ • score += 10 (score = 10)    │
│ • ShowCorrectFeedback()       │
│ • HighlightCorrectAnswer()    │
│ • StartCoroutine(             │
│     NextRoundDelay())         │
└────────┬───────────────────────┘
         │
         ├─────────────────┐
         │                 │
         ▼                 ▼
┌──────────────┐    ┌──────────────┐
│ UIManager    │    │ UIManager    │
│ ShowCorrect  │    │ Highlight    │
│ Feedback()   │    │ Correct()    │
│ ────────     │    │ ────────     │
│ • Panel on   │    │ • Semua sisi │
│ • Text:      │    │   → HIJAU    │
│   "TEPAT!    │    │ • Sparkle    │
│   +10 Poin"  │    │   effect     │
│ • Color:     │    │   Play()     │
│   Hijau      │    │              │
└──────────────┘    └──────────────┘
         │                 │
         └────────┬────────┘
                  │
                  ▼
         ┌────────────────┐
         │ Coroutine      │
         │ NextRoundDelay │
         │ ──────────     │
         │ yield 2.0s     │
         └────────┬───────┘
                  │
                  ▼
         ┌────────────────┐
         │ StartNewRound()│  → SOAL BERIKUTNYA
         │ progres = 2    │
         └────────────────┘
```

---

### **User Answer Flow (Wrong Answer)**

```
USER INPUT: "0.8"  (SALAH, seharusnya 0.6)
      │
      ▼
┌────────────────────────────────┐
│ CalculationManager            │
│ VerifyAnswer()                │
│ ──────────────────────────    │
│ 1. input = "0.8"              │
│ 2. playerAnswer = 0.8f        │
│ 3. Compare:                   │
│    |0.8 - 0.6| = 0.2          │
│    0.2 > 0.01 ✗ SALAH!        │
└────────┬───────────────────────┘
         │
         ▼
┌────────────────────────────────┐
│ HandleWrongAnswer()           │
│ ──────────────────────────    │
│ 1. lives-- (3 → 2)            │
│ 2. UpdateLives(2)             │
│ 3. lives > 0? YES             │
│ 4. ShowFeedback(false, ...)   │
│ 5. HighlightWrongAnswer(      │
│      "Sinθ")                  │
│ 6. NextRoundDelay()           │
└────────┬───────────────────────┘
         │
         ├─────────────────┐
         │                 │
         ▼                 ▼
┌──────────────┐    ┌──────────────┐
│ UIManager    │    │ UIManager    │
│ UpdateLives()│    │ Highlight    │
│ ────────     │    │ Wrong()      │
│ ❤️ ❤️ 💔     │    │ ────────     │
│ (2 lives)    │    │ Sinθ =       │
│              │    │ Depan/Miring │
│              │    │ → Depan RED  │
│              │    │ → Miring RED │
└──────────────┘    └──────────────┘
         │                 │
         └────────┬────────┘
                  │
                  ▼
         ┌────────────────┐
         │ Feedback Panel │
         │ ──────────     │
         │ "SALAH!        │
         │  Perhatikan    │
         │  rumusnya:     │
         │  Sinθ = 0.60"  │
         │ (Red color)    │
         └────────┬───────┘
                  │
                  ▼
         ┌────────────────┐
         │ Wait 2.0s      │
         └────────┬───────┘
                  │
                  ▼
         ┌────────────────┐
         │ StartNewRound()│  → SOAL BARU
         │ progres = 2    │
         └────────────────┘
```

---

### **Game Over Flow**

```
USER INPUT: SALAH (lives = 1)
      │
      ▼
┌────────────────────────────────┐
│ HandleWrongAnswer()           │
│ ──────────────────────────    │
│ 1. lives-- (1 → 0)            │
│ 2. UpdateLives(0)             │
│ 3. lives <= 0? YES            │
└────────┬───────────────────────┘
         │
         ▼
┌────────────────────────────────┐
│ GAME OVER PATH                │
│ ──────────────────────────    │
│ • ShowFeedback(false,         │
│     "GAME OVER!")             │
│ • StartCoroutine(             │
│     ShowGameOverAfterDelay()) │
└────────┬───────────────────────┘
         │
         ├─────────────────┐
         │                 │
         ▼                 ▼
┌──────────────┐    ┌──────────────┐
│ UIManager    │    │ Coroutine    │
│ UpdateLives()│    │ ──────────   │
│ ────────     │    │ yield 2.0s   │
│ 💔 💔 💔     │    │              │
│ (0 lives)    │    │              │
└──────────────┘    └──────┬───────┘
                           │
                           ▼
                  ┌────────────────┐
                  │ endCutscene    │
                  │ ShowGameOver(  │
                  │   score)       │
                  │ ──────────     │
                  │ • Show score   │
                  │ • Retry button │
                  │ • Back to menu │
                  └────────────────┘
```

---

### **Chapter Complete Flow**

```
PROGRES = 5
      │
      ▼
┌────────────────────────────────┐
│ StartNewRound()               │
│ ──────────────────────────    │
│ IF progres >= totalSoal:      │
│    EndChapter() → TRUE        │
│    RETURN                     │
└────────┬───────────────────────┘
         │
         ▼
┌────────────────────────────────┐
│ EndChapter()                  │
│ ──────────────────────────    │
│ • ShowFeedback(true,          │
│     "SELESAI! Skor: {score}") │
│ • StartCoroutine(             │
│     ShowEndCutsceneAfter      │
│     Delay())                  │
└────────┬───────────────────────┘
         │
         ▼
┌────────────────────────────────┐
│ Coroutine                     │
│ ──────────────────────────    │
│ yield 2.0s                    │
└────────┬───────────────────────┘
         │
         ▼
┌────────────────────────────────┐
│ endCutscene                   │
│ ShowEndCutscene(score, 5)     │
│ ──────────────────────────    │
│ • Congratulations screen      │
│ • Final score display         │
│ • Stars rating (based score)  │
│ • Next chapter button         │
│ • Back to menu button         │
└────────────────────────────────┘
```

---

## 🚀 SETUP GUIDE

### **Hierarchy Structure**

```
Chapter1 Scene
├── Canvas
│   ├── Header
│   │   ├── JudulText (TMP)
│   │   ├── ProgresText (TMP)
│   │   └── LivesPanel
│   │       ├── Heart1 (Image)
│   │       ├── Heart2 (Image)
│   │       └── Heart3 (Image)
│   │
│   ├── QuestionPanel
│   │   ├── PertanyaanText (TMP)
│   │   └── JawabanInput (TMP_InputField)
│   │
│   ├── FeedbackPanel
│   │   └── FeedbackText (TMP)
│   │
│   └── CheckButton
│       └── Text "CHECK" (TMP)
│
├── TriangleContainer (Empty GameObject)
│   ├── DepanSprite (SpriteRenderer)
│   ├── SampingSprite (SpriteRenderer)
│   ├── MiringSprite (SpriteRenderer)
│   ├── DepanLabel (TMP UI)
│   ├── SampingLabel (TMP UI)
│   ├── MiringLabel (TMP UI)
│   ├── ThetaLabel (TMP World Space)
│   └── SparkleEffect (ParticleSystem)
│
├── GameManager (Empty GameObject)
│   ├── CalculationManager (Script)
│   ├── TriangleDataGenerator (Script)
│   └── Chapter1EndCutscene (Script)
│
└── UIManager (Empty GameObject)
    ├── UIManagerChapter1 (Script)
    └── TriangleVisualizer (Script)
```

---

### **Script Assignments**

**GameManager GameObject:**
- Add Component: `CalculationManager`
  - Assign uiManager → UIManager
  - Assign dataGenerator → GameManager (TriangleDataGenerator)
  - Assign endCutscene → GameManager (Chapter1EndCutscene)
  - Answer Tolerance: 0.01

**UIManager GameObject:**
- Add Component: `UIManagerChapter1`
  - Header Status:
    - judulText → Canvas/Header/JudulText
    - progresText → Canvas/Header/ProgresText
    - livesIcons → Array[3]: Heart1, Heart2, Heart3
  
  - Interaksi & Pertanyaan:
    - pertanyaanText → Canvas/QuestionPanel/PertanyaanText
    - jawabanInput → Canvas/QuestionPanel/JawabanInput
  
  - Umpan Balik:
    - feedbackPanel → Canvas/FeedbackPanel
    - feedbackText → Canvas/FeedbackPanel/FeedbackText
  
  - Visualisasi Segitiga:
    - triangleVisualizer → UIManager (TriangleVisualizer script)
    - depanLabel_World → TriangleContainer/DepanLabel
    - sampingLabel_World → TriangleContainer/SampingLabel
    - miringLabel_World → TriangleContainer/MiringLabel
    - thetaLabel_World → TriangleContainer/ThetaLabel
    - depanSprite → TriangleContainer/DepanSprite
    - sampingSprite → TriangleContainer/SampingSprite
    - miringSprite → TriangleContainer/MiringSprite
  
  - Efek Visual:
    - sparkleEffect → TriangleContainer/SparkleEffect
    - highlightDuration: 1.5
  
  - Warna:
    - defaultColor: White
    - highlightKuning: Yellow
    - highlightMerah: Red
    - highlightHijau: Green

- Add Component: `TriangleVisualizer`
  - Sprite References:
    - depanSprite → TriangleContainer/DepanSprite
    - sampingSprite → TriangleContainer/SampingSprite
    - miringSprite → TriangleContainer/MiringSprite
  
  - Label References:
    - depanLabel → TriangleContainer/DepanLabel
    - sampingLabel → TriangleContainer/SampingLabel
    - miringLabel → TriangleContainer/MiringLabel
    - thetaLabel → TriangleContainer/ThetaLabel
  
  - Camera:
    - mainCamera → Main Camera
  
  - Visual Settings:
    - baseScale: 0.5
    - centerPosition: (0, 0, 0)
    - labelOffset: 0.5
    - lineThickness: 20
  
  - Colors:
    - normalColor: White
    - highlightColor: Yellow
    - correctColor: Green
    - wrongColor: Red

**CheckButton:**
- Button Component → OnClick()
  - Add: CalculationManager.VerifyAnswer

**JawabanInput:**
- TMP_InputField → OnSubmit()
  - Add: CalculationManager.VerifyAnswer

---

## 🐛 TROUBLESHOOTING

### **Problem 1: Segitiga tidak muncul**

**Symptoms:**
- Label angka muncul
- Sprite segitiga tidak terlihat

**Solutions:**
1. Cek SpriteRenderer di TriangleContainer aktif
2. Cek Z-position sprites (harus di depan background)
3. Cek Camera Orthographic Size (pastikan segitiga dalam view)
4. Cek Color sprites tidak transparan (Alpha = 255)
5. Cek Sprite assigned di SpriteRenderer component

---

### **Problem 2: Input tidak bisa diketik**

**Solutions:**
1. Pastikan ada EventSystem di scene
2. Cek TMP_InputField Interactable = true
3. Cek Canvas Render Mode = Screen Space - Overlay
4. Cek tidak ada Panel blocking input (Raycast Target)

---

### **Problem 3: Jawaban benar tapi dianggap salah**

**Solutions:**
1. Cek answerTolerance (set 0.01 atau lebih besar)
2. Debug log playerAnswer vs JawabanBenar
3. Cek parsing input (gunakan InvariantCulture)
4. Cek pembagian integer (cast ke float)

**Debug Code:**
```csharp
Debug.Log($"Player: {playerAnswer}, Correct: {dataSoalSaatIni.JawabanBenar}");
Debug.Log($"Error: {Mathf.Abs(playerAnswer - dataSoalSaatIni.JawabanBenar)}");
```

---

### **Problem 4: Theta (θ) tidak muncul**

**Solutions:**
1. Pastikan font support Greek characters
2. Gunakan Unicode escape: `\u03B8` bukan karakter langsung
3. Cek TMP_InputField width (text overflow)
4. Force mesh update: `ForceMeshUpdate()` dua kali

---

### **Problem 5: Highlight warna tidak kelihatan**

**Solutions:**
1. Cek Color values di Inspector (bukan transparan)
2. Cek SpriteRenderer.color assignment
3. Cek ResetColors() dipanggil sebelum highlight
4. Debug: `Debug.Log($"Color: {sprite.color}")`

---

## 📚 REFERENSI

### **Pythagoras Triples**

| Triple | a | b | c | Rumus |
|--------|---|---|---|-------|
| 1 | 3 | 4 | 5 | 3² + 4² = 5² |
| 2 | 5 | 12 | 13 | 5² + 12² = 13² |
| 3 | 8 | 15 | 17 | 8² + 15² = 17² |
| 4 | 7 | 24 | 25 | 7² + 24² = 25² |

### **Trigonometry Formulas**

```
       |\
       | \
Depan  |  \ Miring (Hypotenuse)
(Opp)  |   \
       |____\
      Samping (Adjacent)

Sinθ = Opposite / Hypotenuse = Depan / Miring
Cosθ = Adjacent / Hypotenuse = Samping / Miring
Tanθ = Opposite / Adjacent = Depan / Samping
```

### **Score System**

- Jawaban Benar: +10 poin
- Jawaban Salah: -1 life (no points)
- Total Soal: 5
- Max Score: 50 poin
- Lives: 3

---

## 🎯 KEY EXECUTION POINTS

### **Lifecycle Methods (Otomatis)**

| Method | Script | Fungsi |
|--------|--------|--------|
| `Start()` | CalculationManager | Init game, first question |

### **Public Methods (Dipanggil via Button/Event)**

| Method | Script | Trigger |
|--------|--------|---------|
| `VerifyAnswer()` | CalculationManager | Button "CHECK" onClick |

### **Private Methods (Dipanggil Internal)**

| Method | Caller | Fungsi |
|--------|--------|--------|
| `StartNewRound()` | Start(), NextRoundDelay() | Generate question |
| `HandleWrongAnswer()` | VerifyAnswer() | Process wrong answer |
| `NextRoundDelay()` | VerifyAnswer(), HandleWrongAnswer() | Delay before next |
| `EndChapter()` | StartNewRound() | Finish chapter |

### **Coroutines (Async Operations)**

| Coroutine | Duration | Purpose |
|-----------|----------|---------|
| `NextRoundDelay()` | 2.0s | Feedback delay |
| `ShowGameOverAfterDelay()` | 2.0s | Before game over screen |
| `ShowEndCutsceneAfterDelay()` | 2.0s | Before end cutscene |

---

**Last Updated:** 18 Desember 2025  
**Version:** 1.0  
**Author:** Rizqi Ackerman with GitHub Copilot
