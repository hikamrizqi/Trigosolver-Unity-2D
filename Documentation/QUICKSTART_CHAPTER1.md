# 🚀 Quick Start Guide - Chapter 1 Implementation
## Panduan Cepat Implementasi Chapter 1

---

## ✅ Checklist Implementasi

### 1. Script Files (Sudah Dibuat) ✓
Pastikan semua file script berikut ada di folder `Assets/Scripts/Stage 1/`:

- [x] `CalculationManager.cs` - Manager utama gameplay
- [x] `TriangleDataGenerator.cs` - Generator soal segitiga
- [x] `UIManagerChapter1.cs` - Manager UI
- [x] `InputFieldHandler.cs` - Handler untuk input field & Enter key
- [x] `Chapter1EndCutscene.cs` - Cutscene akhir chapter
- [x] `TriangleVisualizer.cs` - Visualizer segitiga (opsional)
- [x] `Chapter1AudioManager.cs` - Audio manager (opsional)

### 2. Setup Scene di Unity Editor

#### A. Buat Scene Baru
1. File > New Scene
2. Save As: `Chapter1_Scene` di folder `Assets/Scenes/`

#### B. Setup Canvas (UI)
```
Klik kanan di Hierarchy > UI > Canvas
Nama: "Canvas_Chapter1"
Canvas Scaler:
  - UI Scale Mode: Scale With Screen Size
  - Reference Resolution: 1920 x 1080
```

#### C. Buat UI Elements

**Header Panel:**
```
Create > UI > Panel (nama: "HeaderPanel")
Anchors: Top-Center
Height: 100

Children:
1. TextMeshPro - Text (nama: "TitleText")
   - Text: "CHAPTER 1: LATIHAN DASAR TEODOLIT"
   - Font Size: 36
   - Color: White
   - Alignment: Center

2. TextMeshPro - Text (nama: "ProgressText")
   - Text: "Soal: 1/5"
   - Font Size: 24
   - Alignment: Center

3. Panel (nama: "LivesPanel")
   - Layout: Horizontal Layout Group
   - Children: 3x Image (Heart Icons)
     * Nama: Heart_1, Heart_2, Heart_3
     * Size: 40x40 setiap heart
```

**Question Panel:**
```
Create > UI > Panel (nama: "QuestionPanel")
Anchors: Middle-Center
Size: 800x400

Children:
1. TextMeshPro - Text (nama: "QuestionText")
   - Text: "Berapakah nilai Sinθ?"
   - Font Size: 32
   - Color: Yellow (#FFFF00)

2. TMP_InputField (nama: "AnswerInputField")
   - Placeholder: "Masukkan jawaban (0.6 atau 3/5)"
   - Font Size: 28
   - Width: 400, Height: 60

3. Button (nama: "VerifyButton")
   - Text: "VERIFIKASI"
   - Size: 300x70
   - Normal Color: Green
   - Highlighted Color: Light Green
```

**Interactive Buttons Panel:**
```
Create > UI > Panel (nama: "InteractiveButtonsPanel")
Anchors: Bottom-Center
Offset Y: 150

Layout: Horizontal Layout Group
Spacing: 20

Children (3 Buttons):
1. Button_Depan - Text: "DEPAN"
2. Button_Samping - Text: "SAMPING"  
3. Button_Miring - Text: "MIRING"

Size setiap button: 200x60
Color: Blue (#0099FF)
```

**Feedback Panel:**
```
Create > UI > Panel (nama: "FeedbackPanel")
Anchors: Bottom-Center
Size: 1000x100
Background Color: Semi-transparent black (#000000AA)
Active: FALSE (akan diaktifkan oleh script)

Children:
1. TextMeshPro - Text (nama: "FeedbackText")
   - Font Size: 28
   - Alignment: Center
   - Color: White (akan berubah via script)
```

#### D. Setup Visualisasi Segitiga (World Space)

**Setup Sederhana dengan Sprites Tegak Lurus:**

Karena semua sprite default tegak lurus, TriangleVisualizer akan mengatur posisi, rotasi, dan skala secara otomatis!

```
1. Buat Empty GameObject (nama: "TriangleVisualization")
   Position: (-3, 0, 0) - Sesuaikan agar terlihat di kamera
   
2. Attach Script: TriangleVisualizer
   (Script ini akan mengatur positioning otomatis)

3. Buat 3 GameObject dengan SpriteRenderer sebagai CHILD dari TriangleVisualization:
   
   a. GameObject: "Line_Depan"
      - Add Component: Sprite Renderer
      - Sprite: Sprite putih polos tegak lurus (1x1 pixel white square juga bisa)
      - Color: White
      - Sorting Layer: Default
      - Order in Layer: 1
      
   b. GameObject: "Line_Samping"
      - Add Component: Sprite Renderer
      - Sprite: Sprite putih polos tegak lurus (sama seperti Depan)
      - Color: White
      - Sorting Layer: Default
      - Order in Layer: 1
      
   c. GameObject: "Line_Miring"
      - Add Component: Sprite Renderer
      - Sprite: Sprite putih polos tegak lurus (sama seperti Depan)
      - Color: White
      - Sorting Layer: Default
      - Order in Layer: 1

   CATATAN: Tidak perlu mengatur posisi/rotasi/scale manual!
           TriangleVisualizer akan mengatur semuanya otomatis.

4. Buat 3 TextMeshPro (World Space) sebagai CHILD dari TriangleVisualization:
   
   a. GameObject: "Label_Depan"
      - Create > 3D Object > Text - TextMeshPro (World Space)
      - Font Size: 2
      - Text: "?" (akan di-update otomatis)
      - Alignment: Center + Middle
      - Color: White
      - Sorting Layer: Default
      - Order in Layer: 2 (di atas garis)
      
   b. GameObject: "Label_Samping"
      - Create > 3D Object > Text - TextMeshPro (World Space)
      - Font Size: 2
      - Text: "?" (akan di-update otomatis)
      - Alignment: Center + Middle
      - Color: White
      
   c. GameObject: "Label_Miring"
      - Create > 3D Object > Text - TextMeshPro (World Space)
      - Font Size: 2
      - Text: "?" (akan di-update otomatis)
      - Alignment: Center + Middle
      - Color: White

   CATATAN: Label akan ditempatkan otomatis di tengah setiap sisi!

5. Konfigurasi TriangleVisualizer di Inspector:
   
   Sprite References:
   - Depan Sprite: [Drag Line_Depan SpriteRenderer]
   - Samping Sprite: [Drag Line_Samping SpriteRenderer]
   - Miring Sprite: [Drag Line_Miring SpriteRenderer]
   
   Label References:
   - Depan Label: [Drag Label_Depan TextMeshPro]
   - Samping Label: [Drag Label_Samping TextMeshPro]
   - Miring Label: [Drag Label_Miring TextMeshPro]
   
   Visual Settings:
   - Base Scale: 0.5 (1 nilai segitiga = 0.5 unit Unity)
   - Center Position: (0, 0, 0) (relatif terhadap parent)
   - Label Offset: 0.5 (jarak label dari garis)
   
   Colors:
   - Normal Color: White (FFFFFF)
   - Highlight Color: Yellow (FFFF00)
   - Correct Color: Green (00FF00)
   - Wrong Color: Red (FF0000)
```

**Tips Membuat Sprite Sederhana:**
```
Tidak punya sprite? Buat sendiri dengan cepat:

Opsi 1 - Gunakan Unity Default Sprite:
1. Klik kanan di Assets > Create > Sprites > Square
2. Gunakan sprite ini untuk semua 3 garis
3. TriangleVisualizer akan stretch dan rotate otomatis

Opsi 2 - Buat di Paint/Photoshop:
1. Buat gambar 100x100 pixels
2. Fill dengan warna putih
3. Save as PNG dengan transparent background
4. Import ke Unity
5. Set Texture Type: Sprite (2D and UI)
```

#### E. Setup Particle System (Sparkle Effect)
```
1. Klik kanan di TriangleVisualization
2. Effects > Particle System (nama: "SparkleEffect")

3. Configure Particle System:
   Main:
   - Duration: 1.0
   - Start Lifetime: 0.5-1.0
   - Start Speed: 1-3
   - Start Size: 0.1-0.3
   - Start Color: Gradient (Yellow → White)
   - Play On Awake: FALSE
   
   Emission:
   - Rate over Time: 0
   - Bursts: Count = 20, Time = 0
   
   Shape:
   - Shape: Sphere
   - Radius: 1
   
   Renderer:
   - Render Mode: Billboard
   - Material: Default-Particle
```

### 3. Assign Scripts ke GameObjects

#### A. GameManager GameObject
```
1. Create Empty GameObject (nama: "GameManager")
2. Attach 3 Scripts:
   - CalculationManager
   - TriangleDataGenerator
   - InputFieldHandler
```

#### B. UIManagerChapter1 GameObject
```
1. Create Empty GameObject (nama: "UIManager_Chapter1")
2. Attach Script: UIManagerChapter1

3. Assign di Inspector:
   Header Status:
   - Judul Text: [Drag TitleText]
   - Progres Text: [Drag ProgressText]
   - Lives Icons: [Drag Heart_1, Heart_2, Heart_3]
   
   Interaksi & Pertanyaan:
   - Pertanyaan Text: [Drag QuestionText]
   - Jawaban Input: [Drag AnswerInputField]
   
   Umpan Balik:
   - Feedback Panel: [Drag FeedbackPanel]
   - Feedback Text: [Drag FeedbackText]
   
   Visualisasi Segitiga:
   - Triangle Visualizer: [Drag TriangleVisualization GameObject]
   - Depan Label_World: [Drag Label_Depan]
   - Samping Label_World: [Drag Label_Samping]
   - Miring Label_World: [Drag Label_Miring]
   - Depan Sprite: [Drag Line_Depan SpriteRenderer]
   - Samping Sprite: [Drag Line_Samping SpriteRenderer]
   - Miring Sprite: [Drag Line_Miring SpriteRenderer]
   
   Efek Visual:
   - Sparkle Effect: [Drag SparkleEffect Particle System (jika ada)]
   - Highlight Duration: 1.5
```

#### C. CalculationManager Configuration
```
Di Inspector GameManager:

CalculationManager:
- UI Manager: [Drag UIManager_Chapter1]
- Data Generator: [Drag TriangleDataGenerator component dari GameManager]
- End Cutscene: [Akan dibuat nanti]
- Answer Tolerance: 0.01
```

#### D. InputFieldHandler Configuration
```
Di Inspector GameManager:

InputFieldHandler:
- Input Field: [Drag AnswerInputField]
- Calculation Manager: [Drag CalculationManager component]
```

### 4. Setup Button OnClick Events

**Verify Button:**
```
1. Select VerifyButton
2. Inspector > Button Component > OnClick()
3. Click [+]
4. Drag GameManager GameObject
5. Select Function: CalculationManager > VerifyAnswer()
```

**Interactive Buttons:**
```
Button_Depan:
- OnClick: UIManager_Chapter1 > OnDepanButtonClicked()

Button_Samping:
- OnClick: UIManager_Chapter1 > OnSampingButtonClicked()

Button_Miring:
- OnClick: UIManager_Chapter1 > OnMiringButtonClicked()
```

### 5. Setup End Cutscene (Opsional)

```
1. Di Canvas, Create > UI > Panel (nama: "EndCutscenePanel")
   - Anchors: Stretch to fill screen
   - Background: Semi-transparent black (#000000DD)
   - Active: FALSE

2. Children:
   a. TextMeshPro - Text (nama: "EndTitle")
      - Text: "CHAPTER 1 SELESAI!"
      - Font Size: 48
      
   b. TextMeshPro - Text (nama: "EndScore")
      - Text: "Skor: 50/50"
      - Font Size: 36
      
   c. TextMeshPro - Text (nama: "EndMessage")
      - Text: "LUAR BIASA!"
      - Font Size: 28
      
   d. Image (nama: "BadgeImage")
      - Size: 200x200
      
   e. Button (nama: "ContinueButton")
      - Text: "LANJUTKAN"
      
   f. Button (nama: "RetryButton")
      - Text: "ULANGI"

3. Create Empty GameObject (nama: "EndCutsceneManager")
4. Attach Script: Chapter1EndCutscene
5. Assign references di Inspector
```

### 6. Testing

```
1. Save Scene
2. File > Build Settings > Add Open Scenes
3. Play Mode

Test Scenarios:
✓ Soal pertama muncul dengan segitiga tergambar otomatis
✓ Segitiga terbentuk dengan ukuran yang sesuai (misal: 3-4-5)
✓ Label menampilkan nilai yang benar di setiap sisi
✓ Input field fokus otomatis
✓ Klik tombol Depan/Samping/Miring → sisi yang sesuai highlight kuning
✓ Input jawaban benar (misal: 0.6) → semua sisi hijau + sparkle
✓ Input jawaban salah → sisi yang relevan merah + lives -1
✓ Soal berganti → segitiga redraw dengan ukuran baru
✓ 5 soal selesai → end cutscene
✓ Lives habis → game over

Test Khusus Visualisasi:
✓ Segitiga (3,4,5) → Depan lebih pendek dari Samping
✓ Segitiga (5,12,13) → Depan lebih pendek dari Samping
✓ Segitiga (8,15,17) → Proporsi benar
✓ Label tidak overlap dengan garis
✓ Semua garis terlihat jelas
```

---

## 🐛 Troubleshooting

### Error: NullReferenceException
**Penyebab:** Ada reference yang belum di-assign di Inspector
**Solusi:** Cek semua field di Inspector yang bertanda merah

### Error: TMP not found
**Penyebab:** TextMeshPro belum di-import
**Solusi:** Window > TextMeshPro > Import TMP Essential Resources

### Input field tidak bisa diketik
**Penyebab:** EventSystem tidak ada di scene
**Solusi:** GameObject > UI > Event System

### Particle effect tidak muncul
**Penyebab:** Play On Awake = true atau reference null
**Solusi:** Set Play On Awake = false, assign reference di script

### Segitiga tidak muncul / salah posisi
**Penyebab:** 
1. Sprite references tidak di-assign di TriangleVisualizer
2. Camera tidak melihat posisi segitiga
3. Sorting layer salah

**Solusi:**
1. Pastikan semua sprite dan label di-assign di TriangleVisualizer Inspector
2. Set camera position agar melihat area segitiga (contoh: x=-3 sampai 3, y=-3 sampai 3)
3. Set camera ke Orthographic mode
4. Pastikan Sorting Layer = Default, Order in Layer > 0

### Segitiga terlalu besar/kecil
**Penyebab:** Base Scale tidak sesuai dengan ukuran scene
**Solusi:** Adjust `Base Scale` di TriangleVisualizer Inspector:
- Terlalu besar: Kurangi menjadi 0.3 atau 0.2
- Terlalu kecil: Naikkan menjadi 0.7 atau 1.0

### Label overlap dengan garis
**Penyebab:** Label Offset terlalu kecil
**Solusi:** Naikkan `Label Offset` di TriangleVisualizer Inspector menjadi 0.7 atau 1.0

### Garis segitiga tidak terlihat
**Penyebab:** 
1. Sprite terlalu tipis atau transparan
2. Scale Y sprite terlalu kecil
3. Camera terlalu jauh

**Solusi:**
1. Pastikan sprite berwarna putih solid (tidak transparan)
2. Check scale Y di TriangleVisualizer.cs line 83: ubah dari 0.1f ke 0.2f jika terlalu tipis
3. Zoom in camera atau perbesar Base Scale

### Warna highlight tidak berubah
**Penyebab:** Material sprite tidak support tint/color change
**Solusi:** 
1. Pastikan sprite menggunakan material default: Sprites-Default
2. Di Sprite Renderer, pastikan Material = None (Default)

---

## 📚 Resources yang Dibutuhkan

### Sprites:
- [ ] Heart icon (untuk lives)
- [ ] Line sprites (untuk sisi segitiga)
- [ ] Badge icons (bronze, silver, gold)
- [ ] Background image (opsional)

### Audio (Opsional):
- [ ] Correct answer SFX
- [ ] Wrong answer SFX
- [ ] Button click SFX
- [ ] Background music
- [ ] Victory music

### Fonts:
- [ ] Title font (bold, besar)
- [ ] Body font (readable)

---

## 🎯 Next Steps

Setelah Chapter 1 selesai:
1. Export scene sebagai prefab/template
2. Buat Chapter 2 (Uji Coba Meriam)
3. Implementasi scene transition
4. Add persistent data (PlayerPrefs) untuk save score
5. Analytics & telemetry (opsional)

---

**Happy Coding! 🚀**
