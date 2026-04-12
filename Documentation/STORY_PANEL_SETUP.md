# Setup Story Panel - Stage 1 Intro (Story + Materi System)

## 📖 Overview

Story Panel sekarang memiliki **2 mode**:

### **Mode 1: Story Panels (5 Panel Pertama)**
- Menampilkan gambar story dengan **typewriter effect** (teks mengetik otomatis)
- Seperti game RPG/Visual Novel
- **Klik pertama** → Skip typewriter, teks langsung komplit
- **Klik kedua** → Next panel
- Total: **5 panel story** dengan dialog

### **Mode 2: Materi/Tutorial (Panel Selanjutnya)**
- Slideshow biasa tanpa dialog
- Klik anywhere → Next slide langsung
- Bisa berisi materi trigonometri, tutorial, dll

---

## 🎯 Flow Sistem

```
LOAD CHAPTER 1 SCENE
  ↓
═══════════════════════════════════════
  STORY PANELS (5 Panel dengan Dialog)
═══════════════════════════════════════
  ↓
PANEL 1 (Story) muncul
Dialog mulai mengetik otomatis: "Pada suatu hari..."
  ↓ KLIK PERTAMA
Dialog langsung komplit (skip typewriter)
  ↓ KLIK KEDUA
  ↓
PANEL 2 (Story) muncul  
Dialog mengetik: "Mereka menemukan..."
  ↓ KLIK PERTAMA
Dialog komplit
  ↓ KLIK KEDUA
  ↓
PANEL 3 (Story) muncul
Dialog mengetik: "Dan kemudian..."
  ↓ KLIK PERTAMA
Dialog komplit
  ↓ KLIK KEDUA
  ↓
PANEL 4 (Story) muncul
Dialog mengetik: "Dan kemudian..."
  ↓ KLIK PERTAMA
Dialog komplit
  ↓ KLIK KEDUA
  ↓
PANEL 5 (Story) muncul
Dialog mengetik: "Akhirnya..."
  ↓ KLIK PERTAMA
Dialog komplit
  ↓ KLIK KEDUA
  ↓
═══════════════════════════════════════
  MATERI/TUTORIAL (Slideshow Normal)
═══════════════════════════════════════
  ↓
SLIDE 5 (Materi 1) muncul
  ↓ KLIK
SLIDE 6 (Materi 2) muncul
  ↓ KLIK
SLIDE 7 (Tutorial) muncul
  ↓ KLIK (pada slide terakhir)
  ↓
Story Panel tutup
  ↓
Level Selection muncul
```

---

## 📝 Setup di Unity Editor

### 1. Buat Story Panel

1. **Buat Panel GameObject**
   - Di Canvas Chapter 1 → Create Empty GameObject
   - Rename: `StoryPanel`
   - Add Component: `Canvas Group`
   - RectTransform: Anchor Stretch both, Left/Top/Right/Bottom = 0

2. **Buat Background Dim**
   - Child of StoryPanel → UI → Image
   - Rename: `BackgroundDim`
   - Color: Hitam alpha 200-220
   - Anchor: Stretch both

3. **Buat Story Image Container**
   - Child of StoryPanel → UI → Image
   - Rename: `StoryImage`
   - Settings:
     * Anchor: Center
     * Size: 1200 x 800 (atau sesuai gambar)
     * Preserve Aspect: ✅
   - Add Component: `Canvas Group`
   - **JANGAN assign Source Image** (diatur dari list)

4. **Buat Dialog Box** ⭐ **BARU!**
   - Child of StoryPanel → UI → Image
   - Rename: `DialogBox`
   - Settings:
     * Anchor: Bottom center
     * Position: Bottom screen (Y = 100-150)
     * Width: 1600, Height: 300-400
     * Color: Hitam dengan alpha 180 (semi-transparent)
   - Ini container untuk dialog text

5. **Buat Dialog Text** ⭐ **BARU!**
   - Child of DialogBox → UI → Text - TextMeshPro
   - Rename: `DialogText`
   - Settings:
     * Font Size: 36-48
     * Color: Putih
     * Alignment: Left + Top
     * Text: "" (kosong, diisi dari script)
     * Wrapping: Enabled
     * Overflow: Overflow
   - Add Padding (left: 40, right: 40, top: 30, bottom: 30)

6. **Buat "Click to Continue" Text**
   - Child of StoryPanel → UI → Text - TextMeshPro
   - Rename: `ClickToContinueText`
   - Text: `"Click anywhere to continue..."`
   - Settings:
     * Font Size: 32-40
     * Color: Putih/Kuning
     * Alignment: Center
     * Position: Bottom center (below dialog box atau di corner)
   - Add Component: `Canvas Group`

---

### 2. Setup StoryPanel Script

1. **Add StoryPanel Component**
   - Pilih GameObject `StoryPanel`
   - Add Component → StoryPanel script

2. **Assign Story Panel UI**
   - **Story Panel**: StoryPanel GameObject (dirinya sendiri)
   - **Story Image**: StoryImage GameObject
   - **Click To Continue Text**: ClickToContinueText GameObject

3. **Assign Story Dialog System** ⭐ **PENTING!**
   - **Dialog Box**: DialogBox GameObject
   - **Dialog Text**: DialogText GameObject
   - **Story Dialogs** (Size = 5):
     * **Element 0**: Dialog untuk panel 1 (misal: "Pada suatu hari di kota Trigono...")
     * **Element 1**: Dialog untuk panel 2 (misal: "Mereka menemukan rahasia segitiga...")
     * **Element 2**: Dialog untuk panel 3 (misal: "Dengan menggunakan rumus...")
     * **Element 3**: Dialog untuk panel 4 (misal: "Setelah berhasil memecahkan...")
     * **Element 4**: Dialog untuk panel 5 (misal: "Akhirnya mereka berhasil! Mari kita belajar...")

4. **Assign Slideshow Images**
   - **Slide Images** (Size = total panel, misal 8):
     * **Element 0-4**: 5 Gambar story (dengan dialog)
     * **Element 5-7**: Gambar materi/tutorial (tanpa dialog)

5. **Typewriter Settings** ⭐ **BARU!**
   - **Typewriter Speed**: 0.05f (delay per karakter, makin kecil makin cepat)
   - **Skip Typewriter On Click**: ✅ (default true)

6. **Animation Settings**
   - **Fade In Duration**: 0.5f
   - **Fade Out Duration**: 0.3f
   - **Slide Transition Duration**: 0.3f
   - **Text Blink Speed**: 1f

7. **Manager References**
   - **Level Selection Manager**: GameObject dengan LevelSelectionManager

---

### 3. Contoh Dialog Text

**Panel 1:**
```
Pada suatu hari di kota Trigono, hiduplah seorang petualang muda bernama Azura.
Ia memiliki misi untuk menemukan Kristal Sudut yang tersembunyi.
```

**Panel 2:**
```
Di dalam gua rahasia, Azura menemukan sebuah puzzle segitiga misterius.
"Aku harus menghitung sisi dan sudut dengan tepat untuk membuka pintunya!"
```

**Panel 3:**
```
Dengan menggunakan rumus trigonometri, Azura mulai menghitung...
Sin, Cos, Tan... setiap angka membawa dia lebih dekat pada jawabannya.
```

**Panel 4:**
```
Setelah beberapa percobaan, perhitungan Azura semakin mendekati jawaban yang tepat.
"Aku hampir berhasil! Tinggal sedikit lagi..."
```

**Panel 5:**
```
Akhirnya, pintu terbuka! Kristal Sudut bersinar terang.
"Sekarang aku harus berlatih lebih banyak. Mari kita mulai petualangan!"
```

---

### 4. Struktur Hierarchy

```
Canvas (Chapter 1)
└── StoryPanel (Canvas Group)
    ├── BackgroundDim (Image - hitam alpha 220)
    ├── StoryImage (Image + Canvas Group)
    ├── DialogBox (Image - hitam alpha 180)
    │   └── DialogText (TextMeshPro)
    └── ClickToContinueText (TextMeshPro + Canvas Group)
```

---

## 🎮 Testing

### Test Story Panels (1-5):

1. **Panel 1 Muncul**
   - Gambar story 1 terlihat
   - Dialog box muncul di bawah
   - Teks mulai mengetik otomatis: "Pada suatu hari..."
   
2. **Klik Pertama (saat typing)**
   - Teks langsung komplit
   - Typewriter animation berhenti
   - Siap untuk next panel

3. **Klik Kedua (saat complete)**
   - Fade transition
   - Panel 2 muncul
   - Dialog baru mulai mengetik: "Di dalam gua..."

4. **Repeat untuk Panel 3, 4 & 5**

### Test Materi Panels (6+):

1. **Panel 6 Muncul**
   - Dialog box **HILANG** (no dialog)
   - Hanya gambar materi
   - Click to continue text muncul

2. **Klik Anywhere**
   - Langsung next slide (no typewriter)
   - Normal slideshow

3. **Last Slide → Close**
   - Fade out
   - Level selection muncul

---

## ⚙️ Customization

### Ubah Kecepatan Typewriter:

1. Pilih StoryPanel GameObject
2. **Typewriter Speed**: 
   - 0.03f = Sangat cepat
   - 0.05f = Normal (recommended)
   - 0.08f = Lambat (dramatic)

### Ubah Dialog Text:

1. Pilih StoryPanel script
2. Expand **Story Dialogs** list
3. Edit text di Element 0-3
4. Support multi-line (gunakan Enter)

### Tambah Story Panel (lebih dari 5):

Di script StoryPanel.cs, ubah:
```csharp
private int storyPanelCount = 5; // Ubah jadi 6, 7, dst
```

Lalu tambahkan dialog ke **Story Dialogs** list

### Styling Dialog Box:

1. Pilih DialogBox GameObject
2. Ubah color, alpha, size
3. Add Image (sprite) untuk border/frame
4. Add shadow/outline effect

### Custom Font:

1. Import custom font (TrueType/OTF)
2. Create TextMeshPro Font Asset
3. Assign ke DialogText → Font Asset

---

## 🐛 Troubleshooting

### Typewriter tidak jalan:
- Pastikan **Dialog Text** sudah assigned
- Check **Story Dialogs** list tidak kosong (min 5 dialog)
- Console log: "[StoryPanel] Starting typewriter for panel X"

### Teks tidak muncul:
- Check DialogBox **AKTIF** di hierarchy
- Verify DialogText → Color alpha = 255 (opaque)
- Pastikan dialog string tidak kosong

### Klik tidak skip typewriter:
- Check **Skip Typewriter On Click** = true
- Verify `canClick = true` (setelah fade in)
- Console log: "[StoryPanel] Typewriter skipped to complete"

### Dialog box muncul di materi panel:
- Check logic `IsStoryPanel()` (index 0-4)
- DialogBox harus hide saat `currentSlideIndex >= 5`

### Typewriter terlalu cepat/lambat:
- Adjust **Typewriter Speed** (0.01f - 0.1f)
- Test dengan dialog panjang dan pendek

### Memory leak setelah scene change:
- Verify coroutine stopped di OnDestroy
- Check `typewriterCoroutine = null` setelah stop

---

## 📋 Script Details

### **StoryPanel.cs** (Updated)

**New Features:**
- ✅ Typewriter effect untuk story panels
- ✅ Dialog system dengan TextMeshPro
- ✅ 2-state click: skip typewriter vs next slide
- ✅ Auto-hide dialog box untuk materi panels
- ✅ Coroutine management (prevent memory leak)

**Key Variables:**
```csharp
List<string> storyDialogs;     // 5 dialog untuk story
TextMeshProUGUI dialogText;    // Text component
bool isTyping;                 // Sedang mengetik?
bool isDialogComplete;         // Dialog sudah komplit?
Coroutine typewriterCoroutine; // Coroutine reference
int storyPanelCount = 5;       // Jumlah story panels
```

**Flow Logic:**
```
HandleClick()
├─ IsStoryPanel() (index 0-4)?
│  ├─ isTyping? → CompleteTypewriterInstantly()
│  └─ isDialogComplete? → NextSlide()
└─ Materi panel (index 4+)?
   └─ NextSlide() (langsung)
```

---

## ✅ Checklist Final

**Setup UI:**
- [ ] StoryPanel dengan Canvas Group
- [ ] StoryImage dengan Canvas Group
- [ ] **DialogBox dibuat (Image + semi-transparent)**
- [ ] **DialogText dibuat (TextMeshPro)**
- [ ] ClickToContinueText dengan Canvas Group

**Setup Script:**
- [ ] StoryPanel script attached
- [ ] Dialog Box & Dialog Text assigned
- [ ] **Story Dialogs list filled (5 dialog)**
- [ ] **Slide Images list filled (5 story + N materi)**
- [ ] Typewriter Speed diatur (default 0.05f)
- [ ] Level Selection Manager assigned

**Testing:**
- [ ] Panel 1 → Typewriter mulai otomatis
- [ ] Klik 1 → Teks komplit instantly
- [ ] Klik 2 → Next panel dengan dialog baru
- [ ] Repeat untuk panel 2, 3, 4, 5
- [ ] Panel 6+ → Dialog box hilang
- [ ] Materi panels → Click langsung next
- [ ] Last slide → Close → Level selection

---

## 💡 Tips

1. **Dialog Length:** 2-4 kalimat per panel (jangan terlalu panjang)
2. **Typewriter Speed:** 0.05f optimal untuk readability
3. **Dialog Box Design:** Tambahkan border/shadow untuk clarity
4. **Font Choice:** Pilih font yang mudah dibaca (readable)
5. **Testing:** Test dengan orang lain untuk pacing feedback
6. **Story Quality:** Buat story yang relate dengan materi trigonometri

---

## 🎨 Contoh Implementasi

**Story Theme: "Petualangan Azura di Kota Trigono"**

**Gambar:**
- Panel 1: Azura di depan gua
- Panel 2: Puzzle segitiga misterius
- Panel 3: Azura menghitung dengan alat
- Panel 4: Hampir berhasil
- Panel 5: Kristal Sudut bersinar

**Dialog:**
- Panel 1: Intro character + setting
- Panel 2: Menemukan problem (puzzle)
- Panel 3: Menggunakan trigonometri
- Panel 4: Progress menuju solusi
- Panel 5: Berhasil + motivasi belajar

**Materi:**
- Slide 6: Penjelasan Sin, Cos, Tan
- Slide 7: Rumus-rumus dasar
- Slide 8: Tutorial cara bermain

---

**Selesai! Story + Dialog system siap digunakan! 🎉**

## 📖 Overview

Story Panel sekarang adalah **slideshow system** yang menampilkan beberapa gambar secara berurutan:
1. **Story/Cerita** - Intro cerita
2. **Materi 1** - Penjelasan materi
3. **Materi 2** - Materi lanjutan (optional)
4. **Tutorial 1** - Cara bermain
5. **Tutorial 2** - Tutorial lanjutan (optional)
6. **...dan seterusnya**

Player klik anywhere untuk next slide, setelah slide terakhir baru masuk ke level selection.

---

## 🎯 Flow Baru

```
LOAD CHAPTER 1 SCENE
  ↓
Story Panel muncul (fade in)
Menampilkan SLIDE 1 (Story/Cerita)
  ↓
PLAYER KLIK ANYWHERE
  ↓
Fade transition ke SLIDE 2 (Materi 1)
  ↓
PLAYER KLIK ANYWHERE
  ↓
Fade transition ke SLIDE 3 (Materi 2)
  ↓
PLAYER KLIK ANYWHERE
  ↓
Fade transition ke SLIDE 4 (Tutorial 1)
  ↓
... (slide lainnya)
  ↓
PLAYER KLIK PADA SLIDE TERAKHIR
  ↓
Story Panel tutup (fade out)
  ↓
Level Selection Panel muncul
```

---

## 📝 Setup di Unity Editor

### 1. Buat Story Panel

1. **Buat Panel GameObject**
   - Di Canvas Chapter 1 → Create Empty GameObject
   - Rename: `StoryPanel`
   - Add Component: `Canvas Group` (untuk fade animation)
   - RectTransform:
     * Anchor: Stretch both (full screen)
     * Left, Top, Right, Bottom = 0

2. **Buat Background Dim**
   - Child of StoryPanel → UI → Image
   - Rename: `BackgroundDim`
   - Color: Hitam dengan alpha 200-220 (untuk dim)
   - Anchor: Stretch both

3. **Buat Story Image Container**
   - Child of StoryPanel → UI → Image
   - Rename: `StoryImage`
   - Settings:
     * Anchor: Center
     * Size: Sesuaikan dengan gambar (misal 1200 x 800)
     * Preserve Aspect: ✅ CHECK
   - Add Component: `Canvas Group` (untuk slide transition)
   - **PENTING:** **JANGAN** assign gambar di Source Image (akan diatur dari list)

4. **Buat "Click to Continue" Text**
   - Child of StoryPanel → UI → Text - TextMeshPro
   - Rename: `ClickToContinueText`
   - Text: `"Click anywhere to continue..."`
   - Settings:
     * Font Size: 32-40
     * Color: Putih atau kuning
     * Alignment: Center
     * Position: Bottom center
   - Add Component: `Canvas Group` (untuk blink animation)

---

### 2. Setup StoryPanel Script

1. **Add StoryPanel Component**
   - Pilih GameObject `StoryPanel`
   - Add Component → StoryPanel script

2. **Assign UI References**
   - **Story Panel**: Drag StoryPanel GameObject (dirinya sendiri)
   - **Story Image**: Drag StoryImage GameObject
   - **Click To Continue Text**: Drag ClickToContinueText GameObject
   - **Level Selection Manager**: Drag GameObject dengan LevelSelectionManager

3. **Assign Slideshow Images** ⭐ **PENTING!**
   - Di Inspector StoryPanel script, cari **"Slide Images"** list
   - Set **Size** = jumlah gambar yang ingin ditampilkan
     * Contoh: 5 (Story + 2 Materi + 2 Tutorial)
   - Assign gambar **secara berurutan**:
     * **Element 0**: Gambar Story/Cerita
     * **Element 1**: Gambar Materi 1
     * **Element 2**: Gambar Materi 2
     * **Element 3**: Gambar Tutorial 1
     * **Element 4**: Gambar Tutorial 2
     * ...dst

4. **Animation Settings** (Optional)
   - **Fade In Duration**: 0.5f (durasi fade in panel)
   - **Fade Out Duration**: 0.3f (durasi fade out panel)
   - **Slide Transition Duration**: 0.3f (durasi transition antar slide)
   - **Text Blink Speed**: 1f (kecepatan blink text)

---

### 3. Persiapan Gambar

**Format Gambar:**
- Resolution: 1920x1080 (landscape) atau sesuai kebutuhan
- Format: PNG (recommended) atau JPG
- Transparent background: Optional (bisa pakai BackgroundDim)

**Naming Convention** (Recommended):
```
chapter1_story.png       → Gambar cerita intro
chapter1_materi_01.png   → Materi slide 1
chapter1_materi_02.png   → Materi slide 2
chapter1_tutorial_01.png → Tutorial slide 1
chapter1_tutorial_02.png → Tutorial slide 2
```

**Import Settings:**
1. Import gambar ke Unity (folder: `Assets/Sprites/Story/`)
2. Pilih gambar → Inspector
3. Texture Type: **Sprite (2D and UI)**
4. Max Size: 2048 atau sesuai kebutuhan
5. Click **Apply**

---

## 🎮 Testing

### Test Flow:

1. **Play Chapter 1 Scene**
   - Story Panel muncul dengan fade in
   - **Slide 1** (Story) terlihat
   - Text "Click anywhere to continue" blink

2. **Klik Anywhere (Slide 1 → 2)**
   - Gambar fade out
   - **Slide 2** (Materi 1) fade in
   - Smooth transition

3. **Klik Lagi (Slide 2 → 3)**
   - Gambar fade out
   - **Slide 3** (Materi 2) fade in

4. **Klik Lagi (Slide 3 → 4)**
   - Continue sampai slide terakhir

5. **Klik pada Slide Terakhir**
   - Story Panel fade out
   - Level Selection Panel muncul
   - 3 button level terlihat

6. **Pilih Level → Back Button**
   - Kembali ke Level Selection
   - **BUKAN** ke Story Panel lagi (sekali show saja)

---

## ⚙️ Customization

### Tambah/Kurangi Slide:

**Di Unity Editor:**
1. Pilih StoryPanel GameObject
2. Di Inspector → StoryPanel script → **Slide Images**
3. Ubah **Size** (misal dari 5 menjadi 7)
4. Assign gambar baru di Element 5, 6, dst

### Ubah Urutan Slide:

1. Di **Slide Images** list
2. Drag element untuk reorder
3. Atau copy-paste sprite ke index yang berbeda

### Ubah Kecepatan Transition:

1. Pilih StoryPanel GameObject
2. Di Inspector → StoryPanel script:
   - **Slide Transition Duration**: 0.2f (lebih cepat) atau 0.5f (lebih lambat)

### Tambah Progress Indicator (Optional):

Tambahkan UI Text untuk menampilkan "1/5", "2/5", dst:

```csharp
// Di StoryPanel.cs, tambahkan:
[SerializeField] private TextMeshProUGUI progressText;

// Di ShowStoryPanel() dan NextSlide():
if (progressText != null)
    progressText.text = $"{currentSlideIndex + 1}/{slideImages.Count}";
```

---

## 🐛 Troubleshooting

### Story Panel tidak muncul:
- Pastikan **Slide Images list tidak kosong** (min 1 gambar)
- Check Console: "[StoryPanel] No slide images assigned!"
- Verify StoryPanel GameObject **AKTIF** di hierarchy

### Gambar tidak terlihat:
- Pastikan gambar sudah di-assign di **Slide Images** list
- Check StoryImage → Canvas Group → Alpha = 1
- Verify gambar sudah di-import sebagai Sprite

### Slide tidak berganti saat klik:
- Check Console logs: harus ada "Transitioning to slide X/Y"
- Pastikan tidak ada UI blocker di depan panel
- Verify Raycast Target enabled di Image component

### Transition terlalu cepat/lambat:
- Adjust **Slide Transition Duration** di Inspector
- Default: 0.3f (balance speed & smoothness)

### Klik tidak terdeteksi:
- Check Console: harus ada log saat klik
- Pastikan `canClick = true` (check setelah fade in selesai)
- Verify tidak ada error di DOTween

### Last slide tidak close panel:
- Check jumlah Element di **Slide Images**
- Verify currentSlideIndex logic (0-based indexing)
- Console log harus menampilkan "[StoryPanel] Closing story panel..."

---

## 📁 File Structure

```
Assets/
├── Scripts/
│   └── UI/
│       └── Chapter1/
│           ├── StoryPanel.cs (UPDATED - Slideshow)
│           └── LevelSelectionManager.cs
├── Sprites/
│   └── Story/
│       ├── chapter1_story.png
│       ├── chapter1_materi_01.png
│       ├── chapter1_materi_02.png
│       ├── chapter1_tutorial_01.png
│       └── chapter1_tutorial_02.png
└── Scenes/
    └── Chapter1.unity
```

---

## 📋 Script Details

### **StoryPanel.cs** (Updated)

**New Features:**
- ✅ Support multiple images (List<Sprite>)
- ✅ Auto-advance dengan click anywhere
- ✅ Smooth fade transition antar slide
- ✅ Canvas Group pada Image untuk slide transition
- ✅ Auto-detect last slide → close panel
- ✅ Prevent click spam dengan `isTransitioning` flag

**Key Variables:**
```csharp
List<Sprite> slideImages;           // List gambar slideshow
int currentSlideIndex = 0;          // Index slide saat ini (0-based)
bool isTransitioning = false;       // Flag prevent double click
float slideTransitionDuration = 0.3f; // Durasi fade antar slide
```

**Flow:**
1. `Start()` → `ShowStoryPanel()` → Show slide 0
2. Player click → `HandleClick()`
3. If not last slide → `NextSlide()` → Fade out → Change sprite → Fade in
4. If last slide → `CloseStoryPanel()` → Fade out panel → Show level selection

---

## ✅ Checklist Final

**Setup:**
- [ ] StoryPanel GameObject dibuat dengan Canvas Group
- [ ] StoryImage dengan Canvas Group (untuk transition)
- [ ] Click to Continue text dengan Canvas Group (untuk blink)
- [ ] StoryPanel script attached
- [ ] **Slide Images list filled** (min 1 gambar)
- [ ] Gambar diurutkan dengan benar (Story → Materi → Tutorial)
- [ ] Level Selection Manager reference assigned

**Testing:**
- [ ] Slide 1 muncul dengan fade in
- [ ] Click anywhere → Next slide dengan smooth transition
- [ ] Semua slide dapat diakses
- [ ] Last slide → Close panel → Show level selection
- [ ] Text "click to continue" blink dengan smooth
- [ ] Back button dari gameplay → Level selection (bukan story panel)

---

## 🎯 Tips

1. **Rekomendasi Jumlah Slide:** 3-7 slide (tidak terlalu panjang)
2. **Durasi Optimal:** 
   - Fade In: 0.5s
   - Transition: 0.3s
   - Fade Out: 0.3s
3. **Desain Gambar:** Konsisten dalam style dan color scheme
4. **Text Overlay:** Jika ada text di gambar, pastikan readable
5. **Testing:** Test dengan player lain untuk feedback pacing

---

**Selesai! Slideshow system siap digunakan! 🎉**
