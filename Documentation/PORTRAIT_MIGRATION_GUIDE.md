# 📱 Portrait Mode Migration Guide

## 🎯 Overview

Panduan step-by-step untuk mengubah Trigosolver dari **Landscape (1920x1080)** ke **Portrait (1080x1920)** untuk mobile deployment.

---

## ⚙️ Prerequisites

**PENTING: Install Android Build Support dulu!**

Sebelum mulai, pastikan Unity kamu sudah punya Android Build Support module.

### Cara Install:

1. **Buka Unity Hub**
2. **Tab "Installs"** → Find Unity 6000.0.23f1 → **⚙️ (gear icon)**
3. **"Add modules"** → Centang:
   - ✅ **Android Build Support**
   - ✅ Android SDK & NDK Tools
   - ✅ OpenJDK
4. **Install** (download ~2-5 GB, tunggu 10-30 menit)

**Atau klik "Install with Unity Hub" di Build Settings window jika muncul warning.**

### Cek Installation:

- **File → Build Settings**
- Android platform harus muncul di list (tidak ada warning "No Android module loaded")

---

## 📋 Step-by-Step Implementation

### ✅ **STEP 1: Switch Platform & Player Settings (10 menit)**

#### A. Switch ke Android Platform (PENTING!)

**Default Orientation hanya tersedia untuk mobile platform.**

1. **File → Build Settings**
2. **Di Platform list, select "Android"**
3. **Klik "Switch Platform"** (tunggu proses reimport ~2-5 menit)
4. **Setelah selesai, close Build Settings window**

#### B. Set Portrait Orientation

5. **Edit → Project Settings → Player**
6. **Di sidebar kiri, klik tab "Android"** (icon robot hijau)
7. **Expand "Resolution and Presentation"** (klik arrow)
8. **Set orientation:**
   ```
   Default Orientation: Portrait
   
   Allowed Orientations for Auto Rotation:
     ✓ Portrait
     ✓ Portrait Upside Down (optional - untuk flip device)
     ✗ Landscape Left
     ✗ Landscape Right
   ```

9. **Scroll down ke "Other Settings":**
   ```
   Auto Graphics API: ✓ (checked)
   Minimum API Level: Android 5.1 'Lollipop' (API level 22) atau lebih tinggi
   Target API Level: Automatic (highest installed)
   ```

#### Why:
- Switch platform diperlukan agar Unity compile untuk Android
- Default Orientation hanya ada di mobile platform (Android/iOS)
- Portrait lock mencegah game rotate ke landscape saat device diputar
- Minimum API Level 22 = support Android 5.1+ (95%+ devices)

---

### ✅ **STEP 2: Canvas Resolution (10 menit)**

#### A. Update Canvas Scaler

**Di setiap scene (Main Menu, Stage 1, Stage 2, Video Opening):**

1. **Select Canvas GameObject** di Hierarchy
2. **Di Inspector → Canvas Scaler component:**

   **BEFORE (Landscape):**
   ```
   UI Scale Mode: Scale With Screen Size
   Reference Resolution: 1920 x 1080
   Screen Match Mode: Match Width Or Height
   Match: 0.5
   ```

   **AFTER (Portrait):**
   ```
   UI Scale Mode: Scale With Screen Size
   Reference Resolution: 1080 x 1920  ← SWAP!
   Screen Match Mode: Match Width Or Height
   Match: 0.5 (bisa adjust 0.3-0.7 untuk balance)
   ```

#### Why:
- Reference resolution swap dari 16:9 landscape → 9:16 portrait
- UI akan auto-scale berdasarkan height/width baru

---

### ✅ **STEP 3: Background Assets Handling (30 menit)**

#### Current Situation:
- `table.png` background di Chapter 1 (landscape wood texture)
- Main Menu background (parallax horizontal)

#### Solution Options:

#### **Option A: Rotate 90° (Quickest - 5 menit)**
```
Background GameObject:
  Rotation: (0, 0, 90)  ← Rotate Z-axis
  Scale: Adjust untuk fit height
```

**Pros:** Instant fix  
**Cons:** Texture mungkin terlihat aneh jika directional (wood grain horizontal)

---

#### **Option B: Scale & Crop (Recommended - 15 menit)**

1. **Di Unity, select background sprite:**
   ```
   Inspector → Sprite Renderer
   Draw Mode: Tiled (jika tileable)
   Size: 
     - Width: 12 (untuk fit portrait height)
     - Height: 20 (taller untuk vertical screen)
   ```

2. **Adjust position:**
   ```
   Transform Position: (0, 0, 10)  ← Push back
   Sorting Layer: Background
   Order in Layer: -10
   ```

**Pros:** Reuse existing asset, seamless  
**Cons:** Edges mungkin repeat/crop

---

#### **Option C: Create Portrait Variant (Best Quality - 30 menit)**

1. **Export asset dari project atau recreate di image editor**
2. **Resize/crop:**
   - Original: 1920x1080 (landscape)
   - New: 1080x1920 (portrait)
   - Center crop atau extend top/bottom dengan matching color
   
3. **Import ke Unity:**
   ```
   Save as: table_portrait.png
   Path: Assets/Sprite/Background/Chapter 1/
   Import settings:
     Texture Type: Sprite (2D and UI)
     Pixels Per Unit: 100
     Filter Mode: Bilinear
     Compression: Normal Quality
   ```

4. **Replace di scene:**
   - Drag `table_portrait` sprite ke Background GameObject
   - Scale: (1, 1, 1) atau adjust to fit

**Pros:** Perfect fit, best visual quality  
**Cons:** Extra file size, manual work

---

#### **Option D: Blur/Fill Edges (Creative - 20 menit)**

Untuk background seperti wood/abstract pattern:

1. **Duplicate background sprite**
2. **First layer (behind):**
   ```
   Sprite: table.png (original)
   Scale: (1.5, 1.5, 1) ← Bigger to cover
   Color: (1, 1, 1, 0.5) ← Semi-transparent
   Blur: Add blur shader/material
   Sorting Order: -20
   ```

3. **Second layer (front):**
   ```
   Sprite: table.png
   Scale: (1, 1.5, 1) ← Vertical stretch
   Crop/Tiled mode
   Sorting Order: -10
   ```

**Pros:** Artistic, smooth transition  
**Cons:** Slightly more complex

---

### ✅ **STEP 4: Camera Settings (5 menit)**

#### Main Camera Adjustments:

**BEFORE (Landscape):**
```
Camera Type: Orthographic
Size: 5
Aspect Ratio: 16:9 (1920x1080)
Position: (0, 0, -10)
```

**AFTER (Portrait):**
```
Camera Type: Orthographic
Size: 8-10 ← INCREASE untuk fit tinggi layar
Aspect Ratio: 9:16 (1080x1920)
Position: (0, 0, -10) ← Same
```

#### How to Test:
1. **Game View → Aspect Ratio dropdown**
2. **Select:** `9:16 Portrait` atau `Free Aspect`
3. **Adjust camera Size** sampai triangle & UI fit sempurna

#### Why:
- Portrait screen lebih tinggi, butuh orthographic size lebih besar
- Agar semua game objects visible di viewport

---

### ✅ **STEP 5: UI Layout Restructure (60 menit)**

#### Current Layout (Landscape):
```
┌──────────────────────────────────────────────┐
│ HEADER  (Judul, Progress, Lives)            │
├──────────────────────────────────────────────┤
│                                              │
│         [SEGITIGA DI TENGAH SCREEN]         │
│                                              │
├──────────────────────────────────────────────┤
│   QUESTION PANEL (Pertanyaan + Input)       │
│   [Verify Button]                            │
└──────────────────────────────────────────────┘
```

#### New Layout (Portrait):
```
┌─────────────┐
│   HEADER    │ ← Judul, Progress (compact horizontal)
│  ❤❤❤       │ ← Lives di bawah/samping
├─────────────┤
│             │
│  SEGITIGA   │ ← Triangle area LEBIH BESAR (vertical space)
│             │
│   (θ = ?)   │
│             │
├─────────────┤
│ PERTANYAAN  │ ← Text soal (wrapped)
│             │
├─────────────┤
│  [ INPUT ]  │ ← Input field centered
├─────────────┤
│ [VERIFIKASI]│ ← Button at bottom
└─────────────┘
```

---

#### A. Header Panel - Compact Horizontal

**Current (Landscape):**
```
Header Panel:
  Anchor: Top-Stretch
  Height: 80
  Children: JudulText (Left), ProgresText (Center), Lives (Right)
```

**New (Portrait):**
```
Header Panel:
  Anchor: Top-Stretch
  Height: 120 ← Taller untuk 2 rows
  Layout: Vertical Layout Group
  
  Row 1 (Horizontal Layout Group):
    - JudulText: "Chapter 1 - Observasi"
    - Font Size: 28 (lebih kecil)
    
  Row 2 (Horizontal Layout Group):
    - ProgresText: "Soal: 1/30"
    - Lives: ❤❤❤ (spacing 30px)
```

**Unity Steps:**
1. Select `HeaderPanel` → Add Component → `Vertical Layout Group`
   - Spacing: 10
   - Child Alignment: Upper Center
   - Child Force Expand: Width ✓, Height ✗

2. Create `Row1` (Empty GameObject + Horizontal Layout Group)
   - Add: JudulText (TextMeshPro)
   - Font Size: 28
   
3. Create `Row2` (Empty GameObject + Horizontal Layout Group)
   - Add: ProgresText, LivesPanel
   - Spacing: 20

---

#### B. Triangle Area - Maximize Space

**Current:**
```
TriangleVisualizer:
  centerPosition: (0, 0, 0)
  maxTriangleSize: 8f
  safetyMargin: 1f
```

**New (Portrait - More Vertical Space):**
```
TriangleVisualizer:
  centerPosition: (0, 2, 0) ← Shift up sedikit
  maxTriangleSize: 10f ← Bigger karena vertical space lebih
  safetyMargin: 1.5f
```

**Code Change (TriangleVisualizer.cs):**
```csharp
[Tooltip("Posisi pusat segitiga di world space")]
public Vector3 centerPosition = new Vector3(0, 2, 0); // Shift up untuk portrait

[Tooltip("Maksimal ukuran segitiga untuk auto-scaling (fit di layar)")]
public float maxTriangleSize = 10f; // Bigger untuk portrait
```

---

#### C. Question Panel - Vertical Stack

**Current (Landscape):**
```
QuestionPanel:
  Anchor: Bottom-Center
  Width: 800, Height: 400
  Pos Y: -100
  
  Children:
    PertanyaanText (Top)
    JawabanInput (Middle)
    VerifyButton (Bottom)
```

**New (Portrait - Wider, Taller):**
```
QuestionPanel:
  Anchor: Bottom-Stretch
  Width: 0 (stretch), Height: 500 ← Taller
  Pos Y: 0
  Padding: Left 50, Right 50
  
  Layout: Vertical Layout Group
    Spacing: 20
    Child Force Expand: Width ✓
    
  Children (Auto-arranged top to bottom):
    1. PertanyaanText:
       - Height: 150
       - Font Size: 36 (dari 48)
       - Text Wrapping: Enabled
       - Alignment: Center
       
    2. JawabanInput:
       - Height: 80
       - Width: Stretch (-100 margin)
       - Font Size: 32
       
    3. VerifyButton:
       - Height: 80
       - Width: Stretch (-150 margin)
       - Font Size: 32
```

**Unity Steps:**
1. **Select QuestionPanel:**
   - Anchor: Bottom, Stretch Horizontal
   - Height: 500
   - Add Component: `Vertical Layout Group`
     - Padding: Top 20, Bottom 20, Left 50, Right 50
     - Spacing: 20
     - Child Alignment: Middle Center
     - Child Force Expand: Width ✓, Height ✗

2. **PertanyaanText:**
   - Add Component: `Layout Element`
     - Min Height: 150
     - Preferred Height: 150
   - Text: Enable wrapping
   - Font Size: 36

3. **JawabanInput:**
   - Add Component: `Layout Element`
     - Min Height: 80

4. **VerifyButton:**
   - Add Component: `Layout Element`
     - Min Height: 80
     - Preferred Width: Leave blank (auto-stretch dengan margin)

---

#### D. Feedback Panel

**Current:**
```
FeedbackPanel:
  Anchor: Middle-Center
  Width: 600, Height: 300
```

**New (Portrait):**
```
FeedbackPanel:
  Anchor: Middle-Center
  Width: 800 (wider untuk portrait)
  Height: 350
  
  FeedbackText:
    Font Size: 40 (dari 48)
    Wrapping: Enabled
```

---

### ✅ **STEP 6: Testing (15 menit)**

#### A. Game View Testing

1. **Window → Game**
2. **Aspect Ratio dropdown:**
   - Test: `9:16 Portrait`
   - Test: `9:18 Portrait` (iPhone X/11)
   - Test: `9:19.5 Portrait` (Samsung S20+)
   
3. **Check:**
   - ✅ Semua UI visible (tidak terpotong)
   - ✅ Triangle tidak overflow
   - ✅ Text tidak overlap
   - ✅ Buttons accessible

#### B. Build & Deploy to Android

1. **File → Build Settings**
2. **Switch Platform: Android**
3. **Player Settings → Resolution:**
   ```
   Default Orientation: Portrait
   Use 32-bit Display Buffer: ✓ (untuk color accuracy)
   ```
4. **Build and Run** ke device

#### C. Safe Area Handling (Modern Phones)

Untuk device dengan notch/punch hole:

```csharp
// Add to UI root canvas atau Header panel
void Start()
{
    ApplySafeArea();
}

void ApplySafeArea()
{
    Rect safeArea = Screen.safeArea;
    RectTransform rectTransform = GetComponent<RectTransform>();
    
    Vector2 anchorMin = safeArea.position;
    Vector2 anchorMax = safeArea.position + safeArea.size;
    
    anchorMin.x /= Screen.width;
    anchorMin.y /= Screen.height;
    anchorMax.x /= Screen.width;
    anchorMax.y /= Screen.height;
    
    rectTransform.anchorMin = anchorMin;
    rectTransform.anchorMax = anchorMax;
}
```

---

## 🎨 Background Asset Solutions Summary

| Option | Time | Quality | Best For |
|--------|------|---------|----------|
| **A. Rotate 90°** | 5 min | ⭐⭐ | Quick prototype, abstract patterns |
| **B. Scale & Tile** | 15 min | ⭐⭐⭐ | Seamless textures (wood, stone) |
| **C. Portrait Variant** | 30 min | ⭐⭐⭐⭐⭐ | Final production, custom art |
| **D. Blur Edges** | 20 min | ⭐⭐⭐⭐ | Creative/artistic backgrounds |

**Recommendation untuk Chapter 1 (table.png wood texture):**
- Use **Option B (Scale & Tile)** terlebih dahulu untuk prototyping
- Jika result OK, stick dengan itu
- Jika tidak memuaskan, upgrade ke **Option C (Portrait Variant)**

---

## ⏱️ Time Estimates

| Task | Estimated Time |
|------|----------------|
| Switch Platform + Player Settings | 10 minutes (termasuk reimport) |
| Canvas Resolution (3 scenes) | 10 minutes |
| Background Assets | 15-30 minutes |
| Camera Settings | 5 minutes |
| UI Layout Restructure | 60 minutes |
| Testing & Tweaks | 15 minutes |
| **TOTAL** | **~2-2.5 hours** |

---

## 🚨 Common Issues & Fixes

### Issue 1: UI terpotong di top/bottom
**Fix:** Increase Header/QuestionPanel padding, check anchors

### Issue 2: Triangle terlalu besar/kecil
**Fix:** Adjust `maxTriangleSize` dan `Camera.orthographicSize`

### Issue 3: Background aspect ratio wrong
**Fix:** Use Tiled draw mode atau buat portrait variant

### Issue 4: Text overflow di pertanyaan
**Fix:** Enable text wrapping, reduce font size, increase panel height

### Issue 5: Buttons tidak reachable
**Fix:** Anchor ke bottom, add padding untuk safe area

---

## 📱 Mobile-Specific Recommendations

1. **Font Sizes:**
   - Header: 24-32 (dari 36-48)
   - Body Text: 32-36 (dari 40-48)
   - Buttons: 32 (dari 36)

2. **Touch Targets:**
   - Minimum button size: 80x80 pixels
   - Spacing between buttons: 20px minimum

3. **Input Fields:**
   - Height: 80-100px untuk easy tap
   - Clear placeholder text

4. **Performance:**
   - Optimize sprite compression
   - Use sprite atlases
   - Limit particle effects untuk mobile

---

## ✅ Final Checklist

- [ ] Android Build Support module installed (via Unity Hub)
- [ ] Switch Platform → Android (Build Settings)
- [ ] Player Settings → Android tab → Portrait orientation set
- [ ] Canvas Scaler → 1080x1920 di semua scene
- [ ] Background assets adjusted (rotate/scale/variant)
- [ ] Camera orthographic size increased (8-10)
- [ ] Header panel restructured (compact horizontal)
- [ ] Triangle area maximized (centerPosition shifted up)
- [ ] Question panel converted to vertical stack
- [ ] Feedback panel resized
- [ ] Safe area handling added (untuk notch)
- [ ] Tested di Game View (9:16, 9:18, 9:19.5)
- [ ] Build to Android device tested
- [ ] Performance check (60 FPS maintained)

---

## 🎯 Next Steps

Setelah portrait migration selesai:
1. **Optimize UI untuk small screens** (5.5" phones)
2. **Add landscape lock warning** (optional)
3. **Test di iOS** jika target multi-platform
4. **Submit to Play Store** dengan portrait screenshots

---

Good luck! 📱✨
