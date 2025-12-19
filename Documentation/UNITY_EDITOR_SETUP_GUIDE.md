# 🎮 Panduan Lengkap Setup Unity Editor - Trigosolver

## 📋 Daftar Isi
1. [Stage 1 - Chapter 1: Observasi Segitiga](#stage-1---chapter-1-observasi-segitiga)
2. [Stage 2 - Chapter 2: Cannon Challenge](#stage-2---chapter-2-cannon-challenge)
3. [Tips & Troubleshooting](#tips--troubleshooting)

---

# Stage 1 - Chapter 1: Observasi Segitiga

## 🎯 Overview
Pemain mengamati segitiga dan menghitung nilai Sin θ, Cos θ, atau Tan θ

---

## 📦 Asset yang Dibutuhkan

### 1. **Sprites (Gambar 2D)**
Buat di **Assets/Sprites/Stage1/**:
- `triangle_depan.png` - Garis vertikal (Sisi Depan) - Warna: Biru
- `triangle_samping.png` - Garis horizontal (Sisi Samping) - Warna: Hijau  
- `triangle_miring.png` - Garis diagonal (Sisi Miring/Hipotenusa) - Warna: Merah
- `heart_full.png` - Icon nyawa penuh
- `heart_empty.png` - Icon nyawa habis

**Cara Mudah Membuat:**
```
1. Buka Paint / Photoshop / GIMP
2. Buat canvas 200x10 pixel (untuk garis)
3. Isi dengan warna solid
4. Export sebagai PNG
5. Import ke Unity (drag ke folder Sprites)
```

### 2. **UI Elements**
Tidak perlu asset khusus, gunakan built-in Unity UI

---

## 🏗️ Hierarchy Setup (Scene Structure)

```
Stage1_Scene
├── Main Camera
├── EventSystem
│
├── Canvas (UI)
│   ├── Header
│   │   ├── JudulText (TextMeshPro)
│   │   ├── ProgresText (TextMeshPro)
│   │   └── LivesPanel
│   │       ├── Heart1 (Image)
│   │       ├── Heart2 (Image)
│   │       └── Heart3 (Image)
│   │
│   ├── QuestionPanel
│   │   ├── PertanyaanText (TextMeshPro)
│   │   ├── JawabanInputField (TMP_InputField)
│   │   └── VerifyButton (Button)
│   │
│   └── FeedbackPanel
│       └── FeedbackText (TextMeshPro)
│
├── GameWorld
│   ├── Triangle (GameObject)
│   │   ├── DepanSide (SpriteRenderer)
│   │   │   └── DepanLabel (TextMeshPro - World Space)
│   │   ├── SampingSide (SpriteRenderer)
│   │   │   └── SampingLabel (TextMeshPro - World Space)
│   │   └── MiringSide (SpriteRenderer)
│   │       └── MiringLabel (TextMeshPro - World Space)
│   │
│   └── SparkleEffect (Particle System)
│
└── GameManagers
    ├── CalculationManager (Script)
    ├── TriangleDataGenerator (Script)
    ├── UIManagerChapter1 (Script)
    ├── Chapter1AudioManager (Script)
    └── Chapter1EndCutscene (Script)
```

---

## 🔧 Step-by-Step Setup

### **STEP 1: Buat Scene Baru**
1. File → New Scene
2. Save as: `Assets/Scenes/Stage1_Scene.unity`

---

### **STEP 2: Setup Canvas UI**

#### A. Buat Canvas
```
1. Right-click Hierarchy → UI → Canvas
2. Canvas Settings:
   - Render Mode: Screen Space - Overlay
   - UI Scale Mode: Scale With Screen Size
   - Reference Resolution: 1920 x 1080
```

#### B. Header Panel
```
1. Canvas → Right-click → UI → Panel (rename: Header)
2. Header:
   - Anchor: Top-Stretch
   - Height: 100
   - Color: Semi-transparent (R:0, G:0, B:0, A:150)

3. Tambah children:
   a) JudulText (TextMeshPro - Text):
      - Text: "Chapter 1 - Observasi Segitiga"
      - Font Size: 36
      - Alignment: Center
      - Anchor: Top-Center
      - Pos Y: -30

   b) ProgresText (TextMeshPro):
      - Text: "Soal: 1/5"
      - Font Size: 24
      - Anchor: Top-Left
      - Pos: (100, -30)

   c) LivesPanel (Empty GameObject):
      - Anchor: Top-Right
      - Pos X: -100
      
      Tambah 3 Image sebagai children:
      - Heart1, Heart2, Heart3
      - Source Image: heart_full sprite
      - Width: 40, Height: 40
      - Horizontal spacing: 50
```

#### C. Question Panel
```
1. Canvas → Right-click → UI → Panel (rename: QuestionPanel)
2. QuestionPanel:
   - Anchor: Middle-Center
   - Width: 800, Height: 400
   - Pos Y: -100

3. Tambah children:
   a) PertanyaanText (TextMeshPro):
      - Text: "Berapakah nilai Sin θ?"
      - Font Size: 48
      - Alignment: Center
      - Anchor: Top-Stretch
      - Height: 100

   b) JawabanInputField (UI → Input Field - TextMeshPro):
      - Placeholder: "Contoh: 0.6 atau 3/5"
      - Font Size: 36
      - Anchor: Middle-Center
      - Width: 400, Height: 80
      - Pos Y: -50

   c) VerifyButton (UI → Button - TextMeshPro):
      - Text: "VERIFIKASI"
      - Font Size: 32
      - Anchor: Bottom-Center
      - Width: 300, Height: 70
      - Pos Y: 50
      - Colors: Normal (Green), Highlighted (Light Green)
```

#### D. Feedback Panel
```
1. Canvas → Right-click → UI → Panel (rename: FeedbackPanel)
2. FeedbackPanel:
   - Anchor: Bottom-Stretch
   - Height: 150
   - Initially INACTIVE (uncheck di Inspector)

3. Tambah child:
   FeedbackText (TextMeshPro):
   - Text: "Feedback muncul di sini"
   - Font Size: 28
   - Alignment: Center-Middle
   - Anchor: Stretch-All
```

---

### **STEP 3: Setup Game World (Segitiga)**

#### A. Buat Empty GameObject "GameWorld"
```
1. Right-click Hierarchy → Create Empty (rename: GameWorld)
2. Position: (0, 0, 0)
```

#### B. Buat Triangle GameObject
```
1. GameWorld → Right-click → Create Empty (rename: Triangle)
2. Position: (0, 1, 0) - Agar terlihat di tengah screen
```

#### C. Tambah Sisi Segitiga (3 Sprite)

**Sisi Depan (Vertikal):**
```
1. Triangle → Right-click → 2D Object → Sprite
2. Rename: DepanSide
3. Inspector:
   - Sprite: triangle_depan (garis vertikal biru)
   - Transform:
     * Position: (-2, 0, 0)
     * Rotation: (0, 0, 90)  // Rotate 90° agar vertikal
     * Scale: (3, 0.1, 1)     // Panjang 3 unit
   - Color: White (default)
   - Sorting Layer: Default
   - Order in Layer: 1

4. Tambah child TextMeshPro - World Space:
   - Rename: DepanLabel
   - Text: "3"
   - Font Size: 5
   - Alignment: Center
   - Position: (0.5, 0, 0) - Di samping garis
   - Color: Blue
```

**Sisi Samping (Horizontal):**
```
1. Triangle → Right-click → 2D Object → Sprite
2. Rename: SampingSide
3. Inspector:
   - Sprite: triangle_samping (garis horizontal hijau)
   - Transform:
     * Position: (-0.5, -1.5, 0)
     * Rotation: (0, 0, 0)  // Horizontal
     * Scale: (4, 0.1, 1)    // Panjang 4 unit
   - Color: White
   - Order in Layer: 1

4. Tambah child TextMeshPro - World Space:
   - Rename: SampingLabel
   - Text: "4"
   - Font Size: 5
   - Position: (0, -0.5, 0) - Di bawah garis
   - Color: Green
```

**Sisi Miring (Diagonal/Hipotenusa):**
```
1. Triangle → Right-click → 2D Object → Sprite
2. Rename: MiringSide
3. Inspector:
   - Sprite: triangle_miring (garis diagonal merah)
   - Transform:
     * Position: (0.5, 0, 0)
     * Rotation: (0, 0, -37)  // Sudut miring ~37° (3-4-5 triangle)
     * Scale: (5, 0.1, 1)      // Panjang 5 unit (hipotenusa)
   - Color: White
   - Order in Layer: 1

4. Tambah child TextMeshPro - World Space:
   - Rename: MiringLabel
   - Text: "5"
   - Font Size: 5
   - Position: (0, 0.5, 0) - Di atas garis
   - Color: Red
```

#### D. Particle Effect (Opsional)
```
1. GameWorld → Right-click → Effects → Particle System
2. Rename: SparkleEffect
3. Position: (0, 1, 0) - Di tengah segitiga
4. Inspector:
   - Start Lifetime: 1
   - Start Speed: 2
   - Start Size: 0.2
   - Start Color: Yellow/Gold
   - Emission: Rate over Time = 50
   - Shape: Circle, Radius = 1
   - Initially INACTIVE (Stop saat play)
```

---

### **STEP 4: Tambah Scripts ke Scene**

#### A. Buat GameObject "GameManagers"
```
1. Right-click Hierarchy → Create Empty (rename: GameManagers)
```

#### B. Assign Scripts
```
1. GameManagers → Add Component → CalculationManager
2. GameManagers → Add Component → TriangleDataGeneratorStage2
3. GameManagers → Add Component → UIManagerChapter1
4. GameManagers → Add Component → Chapter1AudioManager (opsional)
5. GameManagers → Add Component → Chapter1EndCutscene (opsional)
```

---

### **STEP 5: Connect References di Inspector**

#### A. CalculationManager (di Inspector)
```
Drag & Drop ke slot:
✓ UI Manager → UIManagerChapter1 (GameObject GameManagers)
✓ Data Generator → TriangleDataGeneratorStage2 (GameObject GameManagers)
✓ End Cutscene → Chapter1EndCutscene (GameObject GameManagers)

Settings:
✓ Answer Tolerance: 0.01
```

#### B. UIManagerChapter1 (di Inspector)
```
Drag & Drop ke slot:

Header Status:
✓ Judul Text → JudulText (dari Canvas)
✓ Progres Text → ProgresText (dari Canvas)
✓ Lives Icons → Array Size: 3
  - Element 0 → Heart1
  - Element 1 → Heart2
  - Element 2 → Heart3

Interaksi & Pertanyaan:
✓ Pertanyaan Text → PertanyaanText
✓ Jawaban Input → JawabanInputField

Umpan Balik:
✓ Feedback Panel → FeedbackPanel
✓ Feedback Text → FeedbackText

Visualisasi Segitiga:
✓ Depan Label World → DepanLabel (TextMeshPro di GameWorld)
✓ Samping Label World → SampingLabel
✓ Miring Label World → MiringLabel
✓ Depan Sprite → DepanSide (SpriteRenderer)
✓ Samping Sprite → SampingSide
✓ Miring Sprite → MiringSide

Warna Highlight:
✓ Default Color: White
✓ Highlight Kuning: Yellow
✓ Highlight Merah: Red
✓ Highlight Hijau: Green

Efek Visual:
✓ Sparkle Effect → SparkleEffect (Particle System)
✓ Highlight Duration: 1.5

Audio Manager:
✓ Audio Manager → Chapter1AudioManager (jika ada)
```

#### C. VerifyButton OnClick Event
```
1. Select VerifyButton di Canvas
2. Inspector → Button → OnClick()
3. Click [+] untuk tambah event
4. Drag GameManagers ke slot None (Object)
5. Dropdown: CalculationManager → VerifyAnswer()
```

---

### **STEP 6: Camera Setup**
```
1. Select Main Camera
2. Inspector:
   - Projection: Orthographic
   - Size: 5
   - Position: (0, 0, -10)
   - Background: Dark Blue atau Gradient
```

---

### **STEP 7: Test Play!**
```
1. Click Play ▶
2. Coba input jawaban: 0.6 atau 3/5
3. Click VERIFIKASI
4. Perhatikan:
   ✓ Highlight berubah warna
   ✓ Lives berkurang saat salah
   ✓ Progress bertambah
   ✓ Sparkle muncul saat benar
```

---

# Stage 2 - Chapter 2: Cannon Challenge

## 🎯 Overview
Pemain menghitung sudut elevasi meriam untuk mengenai target

---

## 📦 Asset yang Dibutuhkan

### 1. **Sprites**
Buat di **Assets/Sprites/Stage2/**:
- `cannon_base.png` - Base meriam (100x80px) - Warna: Dark Gray
- `cannon_barrel.png` - Laras meriam (120x30px) - Warna: Gray
- `projectile.png` - Peluru/bola (20x20px circle) - Warna: Black
- `target_ship.png` - Kapal musuh (150x100px) - Warna: Red
- `ground.png` - Tanah/dasar (2000x100px) - Warna: Brown
- `water.png` - Laut (2000x500px) - Warna: Blue gradient

**Alternatif Cepat:**
```
Gunakan Shapes dari Unity:
1. Right-click → 2D Object → Sprites → Circle/Square
2. Ubah warna via SpriteRenderer Color
```

---

## 🏗️ Hierarchy Setup

```
Stage2_Scene
├── Main Camera
├── EventSystem
│
├── Canvas (UI)
│   ├── Header
│   │   └── QuestionText (TextMeshPro)
│   │
│   ├── InputPanel
│   │   ├── AngleInputField (TMP_InputField)
│   │   └── ShootButton (Button)
│   │
│   └── FeedbackText (TextMeshPro)
│
├── GameWorld
│   ├── Environment
│   │   ├── Ground (SpriteRenderer)
│   │   ├── Water (SpriteRenderer)
│   │   └── Sky (Camera Background)
│   │
│   ├── Cannon (GameObject)
│   │   ├── CannonBase (SpriteRenderer)
│   │   ├── CannonBarrel (SpriteRenderer) ← CannonController di sini!
│   │   └── ShootPoint (Transform - Empty)
│   │
│   ├── Target (GameObject)
│   │   └── TargetShip (SpriteRenderer)
│   │       └── Collider2D (Box)
│   │
│   └── TriangleHelper (GameObject - Opsional)
│       └── TriangleVisualizer (Script)
│
└── GameManagers
    └── GameManagerChapter2 (Script)
```

---

## 🔧 Step-by-Step Setup

### **STEP 1: Buat Scene**
```
1. File → New Scene
2. Save as: Assets/Scenes/Stage2_Scene.unity
```

---

### **STEP 2: Setup Canvas UI**

#### A. Header
```
1. Canvas → UI → Panel (rename: Header)
2. Anchor: Top-Stretch, Height: 80

3. QuestionText (TextMeshPro):
   - Text: "Jarak target: 100m. Hitung sudut elevasi!"
   - Font Size: 32
   - Alignment: Center
   - Anchor: Stretch-All
```

#### B. Input Panel
```
1. Canvas → UI → Panel (rename: InputPanel)
2. Anchor: Bottom-Center
3. Width: 600, Height: 150
4. Pos Y: 100

Children:
a) AngleInputField (TMP_InputField):
   - Placeholder: "Masukkan sudut (0-90)"
   - Font Size: 32
   - Width: 250, Height: 70
   - Anchor: Left

b) ShootButton (Button):
   - Text: "TEMBAK!"
   - Font Size: 36
   - Width: 250, Height: 70
   - Anchor: Right
   - Color: Red
```

#### C. Feedback
```
FeedbackText (TextMeshPro):
- Text: ""
- Font Size: 28
- Anchor: Middle-Center
- Pos Y: -200
- Alignment: Center
```

---

### **STEP 3: Setup Game World**

#### A. Environment
```
1. Hierarchy → Create Empty (rename: Environment)

a) Ground:
   - 2D Object → Sprite → Square
   - Position: (0, -4, 0)
   - Scale: (20, 1, 1)
   - Color: Brown
   - Add Component: Box Collider 2D
   - Tag: "Ground"

b) Water:
   - 2D Object → Sprite → Square
   - Position: (0, -3, 1) - Di belakang ground
   - Scale: (20, 5, 1)
   - Color: Light Blue
   - Order in Layer: -1
```

#### B. Cannon Setup
```
1. Hierarchy → Create Empty (rename: Cannon)
2. Position: (-8, -3, 0) - Di kiri layar, di atas ground

a) CannonBase (Child):
   - 2D Object → Sprite → Square
   - Scale: (1, 0.8, 1)
   - Color: Dark Gray
   - Order in Layer: 2

b) CannonBarrel (Child):
   - 2D Object → Sprite → Square
   - Scale: (1.2, 0.3, 1)
   - Position: (0.6, 0.2, 0) - Sedikit di atas base
   - Color: Gray
   - Pivot: Set ke (0, 0.5) - Agar rotasi dari pangkal
   - Order in Layer: 3
   
   ⚠️ PENTING: Tambah Script CannonController di CannonBarrel!
   
c) ShootPoint (Child of CannonBarrel):
   - Create Empty
   - Position: (1.2, 0, 0) - Di ujung laras
```

#### C. Target Setup
```
1. Hierarchy → Create Empty (rename: Target)
2. Position: (8, -3, 0) - Di kanan layar
3. Tag: "Target" (buat tag baru di Inspector)

a) TargetShip (Child):
   - 2D Object → Sprite → Square
   - Scale: (1.5, 1, 1)
   - Color: Red
   - Add Component: Box Collider 2D
   - Order in Layer: 2
```

#### D. Projectile Prefab
```
1. Hierarchy → 2D Object → Sprite → Circle (rename: Projectile)
2. Inspector:
   - Scale: (0.3, 0.3, 1)
   - Color: Black
   - Add Component: Rigidbody 2D
     * Gravity Scale: 1
     * Collision Detection: Continuous
   - Add Component: Circle Collider 2D
     * Radius: 0.5
   - Add Component: ProjectileController (Script)
   
3. Drag Projectile ke folder Assets/Prefabs/
4. Delete Projectile dari Hierarchy
```

---

### **STEP 4: Connect Scripts**

#### A. GameManagerChapter2
```
1. Create Empty → Rename: GameManagers
2. Add Component → GameManagerChapter2

Inspector - Drag & Drop:
Game References:
✓ Cannon Controller → CannonBarrel (yang punya CannonController script)
✓ Projectile Prefab → Projectile prefab dari folder Prefabs
✓ Shoot Point → ShootPoint (child of CannonBarrel)
✓ Target Object → Target (GameObject)

UI References:
✓ Question Text → QuestionText
✓ Angle Input Field → AngleInputField
✓ Shoot Button → ShootButton
✓ Feedback Text → FeedbackText

Physics Parameters:
✓ Gravity: 9.8
✓ Initial Velocity: 100
```

#### B. ShootButton OnClick
```
1. Select ShootButton
2. Inspector → Button → OnClick()
3. [+] Add event
4. Drag GameManagers
5. Function: GameManagerChapter2 → OnShootButtonClicked()
```

---

### **STEP 5: Camera & Physics**

#### A. Camera
```
Main Camera:
- Projection: Orthographic
- Size: 6
- Position: (0, 0, -10)
- Background: Sky Blue (R:135, G:206, B:235)
```

#### B. Physics2D Settings
```
Edit → Project Settings → Physics2D:
✓ Gravity Y: -9.81
```

---

### **STEP 6: Test!**
```
1. Play ▶
2. Input sudut: 45
3. Click TEMBAK
4. Lihat:
   ✓ Cannon rotate ke 45°
   ✓ Peluru keluar dari ShootPoint
   ✓ Peluru mengikuti parabola
   ✓ Hit target atau miss
```

---

# 🎨 Tips & Troubleshooting

## Cara Cepat Buat Sprite Sederhana

### Opsi 1: Gunakan Unity Shapes (TERCEPAT!)
```
Right-click Hierarchy → 2D Object → Sprites → Square/Circle
Ubah warna via SpriteRenderer → Color
```

### Opsi 2: Buat di Paint
```
1. Buka Paint
2. Buat canvas 200x200 pixel
3. Isi dengan warna solid
4. Save as PNG
5. Drag ke Unity
```

### Opsi 3: Free Assets
```
Download dari:
- OpenGameArt.org
- Itch.io (Free Assets)
- Kenney.nl (Free Game Assets)
```

---

## Common Issues

### ❌ "NullReferenceException" saat Play
```
✓ Pastikan SEMUA references di Inspector terisi
✓ Check apakah script sudah di-assign ke GameObject
✓ Jangan lupa connect Button OnClick events
```

### ❌ Segitiga tidak terlihat
```
✓ Check Position Z (harus 0, bukan -10)
✓ Check Camera Orthographic Size (coba 5-8)
✓ Check Sorting Order (Triangle harus > 0)
✓ Check Scale (jangan terlalu kecil)
```

### ❌ Particle tidak muncul
```
✓ Check apakah Particle System di-activate via script
✓ Play mode: Particle harus Playing, bukan Stopped
✓ Emission Rate > 0
```

### ❌ Cannon tidak rotate
```
✓ CannonController script harus di CannonBarrel, BUKAN di Cannon parent
✓ Check Pivot Point di Sprite (harus di ujung kiri)
```

### ❌ Projectile tidak muncul
```
✓ Pastikan Projectile adalah PREFAB (di folder Prefabs)
✓ ShootPoint position harus di ujung laras
✓ Check Layer collision matrix (Edit → Project Settings → Physics2D)
```

### ❌ UI tidak terlihat
```
✓ Canvas Render Mode: Screen Space - Overlay
✓ Check EventSystem ada di scene
✓ TextMeshPro: Font Asset harus ter-assign
```

---

## Testing Checklist

### Stage 1 (Observasi):
- [ ] Soal muncul dengan benar (Sin θ / Cos θ / Tan θ)
- [ ] Input field bisa diketik
- [ ] Jawaban desimal (0.6) diterima ✓
- [ ] Jawaban pecahan (3/5) diterima ✓
- [ ] Highlight hijau saat benar
- [ ] Highlight merah saat salah
- [ ] Lives berkurang saat salah
- [ ] Progress bertambah
- [ ] Sparkle muncul saat benar
- [ ] Cutscene muncul di akhir

### Stage 2 (Cannon):
- [ ] Soal muncul (jarak target)
- [ ] Input sudut 0-90 bisa dimasukkan
- [ ] Cannon rotate sesuai input
- [ ] Peluru keluar dari ShootPoint
- [ ] Peluru mengikuti parabola
- [ ] Collision dengan target terdeteksi
- [ ] Feedback benar/salah muncul
- [ ] Triangle helper muncul (opsional)

---

## 🎯 Quick Start Summary

**Untuk yang Baru Pertama Kali:**

1. **Buat Scene → Save**
2. **Tambah Canvas (UI)**
   - Buat Text, InputField, Button
3. **Tambah GameWorld**
   - Buat Sprite shapes (Square/Circle)
   - Atur position & scale
4. **Buat GameObject "GameManagers"**
   - Add semua scripts
5. **DRAG & DROP semua references di Inspector**
6. **Connect Button OnClick events**
7. **PLAY!**

**Waktu Setup:**
- Stage 1: ~30-45 menit
- Stage 2: ~45-60 menit

---

**Dibuat:** December 10, 2025  
**Versi:** 1.0 - Complete Setup Guide
