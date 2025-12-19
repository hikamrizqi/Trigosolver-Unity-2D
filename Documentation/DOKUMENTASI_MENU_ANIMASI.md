# 📚 DOKUMENTASI SISTEM MENU ANIMASI - TRIGOSOLVER

## 📋 Daftar Isi
1. [Arsitektur Sistem](#arsitektur-sistem)
2. [MenuAnimationController.cs](#menuanimationcontrollercs)
3. [MainMenuManager.cs](#mainmenumanagercs)
4. [Diagram Flow](#diagram-flow)
5. [Setup Guide](#setup-guide)
6. [Troubleshooting](#troubleshooting)

---

## 🏗️ ARSITEKTUR SISTEM

Sistem menu animasi terdiri dari 2 komponen utama yang bekerja sama:

### **1. MenuAnimationController** (Per-Panel Animation)
- **Fungsi**: Mengatur animasi individual untuk setiap panel (drop, bounce, sink)
- **Attached ke**: Setiap panel menu (Logo, MainMenu, ModeSelection, ModeCeritaSelection)
- **Dependency**: DOTween (asset animasi)

### **2. MainMenuManager** (State Management)
- **Fungsi**: Mengatur state dan transisi antar panel
- **Attached ke**: GameObject MenuManager (singleton di Canvas)
- **Dependency**: MenuAnimationController pada setiap panel

---

## 🎬 MenuAnimationController.cs

### **📌 Variabel Public (Configurable di Inspector)**

| Variabel | Tipe | Default | Fungsi |
|----------|------|---------|--------|
| `dropDuration` | float | 0.8f | Durasi animasi jatuh dari atas (detik) |
| `dropStartHeight` | float | 1.5f | Tinggi awal drop (multiplier dari screen height) |
| `bounceCount` | int | 2 | Jumlah bouncing (deprecated, tidak terpakai) |
| `bounceStrength` | float | 0.3f | Kekuatan bounce (deprecated, tidak terpakai) |
| `sinkDuration` | float | 0.6f | Durasi animasi tenggelam ke bawah (detik) |
| `dropEase` | Ease | OutBounce | Tipe easing untuk drop (efek memantul) |
| `sinkEase` | Ease | InBack | Tipe easing untuk sink (efek tertarik) |

**Penjelasan Ease Types:**
- `Ease.OutBounce`: Gerakan memantul-mantul di akhir (seperti bola jatuh)
- `Ease.InBack`: Gerakan mundur sedikit sebelum maju (efek "ditarik")

### **🔒 Variabel Private (Internal State)**

| Variabel | Tipe | Fungsi |
|----------|------|--------|
| `rectTransform` | RectTransform | Komponen untuk mengatur posisi UI panel |
| `originalPosition` | Vector2 | Posisi tengah panel (target akhir animasi) |
| `canvasGroup` | CanvasGroup | Komponen untuk fade in/out (mengatur alpha/transparency) |
| `isInitialized` | bool | Flag apakah komponen sudah diinisialisasi |

### **🛠️ Fungsi-fungsi**

#### `Initialize()`
```csharp
private void Initialize()
```

**Fungsi:** Inisialisasi komponen yang dibutuhkan untuk animasi

**Alur Eksekusi:**
```
1. Cek isInitialized → Jika sudah true, return (skip)
2. Ambil RectTransform component dari GameObject
   - Jika null → LogError dan return
3. Simpan posisi original (rectTransform.anchoredPosition)
4. Ambil/buat CanvasGroup component
   - Jika tidak ada → AddComponent<CanvasGroup>()
5. Set isInitialized = true
6. Log "initialized"
```

**Kapan dipanggil:**
- Di `Awake()` (untuk GameObject yang active sejak awal)
- Di `AnimateDropIn()` (lazy init untuk GameObject inactive)

---

#### `AnimateDropIn(Action onComplete = null)`
```csharp
public void AnimateDropIn(Action onComplete = null)
```

**Fungsi:** Animasi panel drop dari atas layar dengan bounce effect

**Parameter:**
- `onComplete`: Callback function yang dipanggil setelah animasi selesai (optional)

**Alur Eksekusi:**
```
1. Panggil Initialize() untuk memastikan komponen ready
2. Null check rectTransform dan canvasGroup
3. Hitung posisi awal:
   startY = Screen.height × dropStartHeight (di atas layar)
4. Set state awal:
   - Position Y = startY (di luar layar atas)
   - Alpha = 0 (transparan)
5. SetActive(true) → Tampilkan GameObject
6. Buat DOTween Sequence:
   a. Fade In: Alpha 0 → 1 (0.3 detik)
   b. Drop: Position Y → originalPosition.y (dropDuration detik)
      - Gunakan dropEase (OutBounce) untuk efek memantul
7. OnComplete:
   - Log "selesai"
   - Panggil callback onComplete (jika ada)
```

**Timeline Animasi:**
```
0.0s ────────────────────── 0.3s ──────────── 0.8s
│                            │                  │
Alpha: 0 ───────────────► 1.0                  │
Position Y: startY ────────────────────────► original
                                         (with bounce)
```

---

#### `AnimateSinkOut(Action onComplete = null)`
```csharp
public void AnimateSinkOut(Action onComplete = null)
```

**Fungsi:** Animasi panel tenggelam ke bawah layar dan fade out

**Parameter:**
- `onComplete`: Callback function setelah animasi selesai

**Alur Eksekusi:**
```
1. Hitung posisi target:
   targetY = -Screen.height × dropStartHeight (di bawah layar)
2. Buat DOTween Sequence:
   a. Sink: Position Y → targetY (sinkDuration detik)
      - Gunakan sinkEase (InBack) untuk efek tertarik
   b. Fade Out: Alpha 1.0 → 0 (sinkDuration × 0.7 detik)
3. OnComplete:
   - SetActive(false) → Sembunyikan GameObject
   - Reset position ke originalPosition
   - Reset alpha ke 1.0
   - Panggil callback onComplete (jika ada)
```

**Timeline Animasi:**
```
0.0s ──────────────────────── 0.6s
│                              │
Position Y: original ───────► targetY (bawah layar)
                        (InBack easing)
Alpha: 1.0 ──────────────► 0.0
            (0.42s duration)
```

---

#### `ShowInstant()`
```csharp
public void ShowInstant()
```

**Fungsi:** Tampilkan panel langsung tanpa animasi (untuk testing)

**Alur:**
```
1. SetActive(true)
2. Set position = originalPosition
3. Set alpha = 1.0
```

---

#### `HideInstant()`
```csharp
public void HideInstant()
```

**Fungsi:** Sembunyikan panel langsung tanpa animasi

**Alur:**
```
1. SetActive(false)
2. Set position = originalPosition (reset)
3. Set alpha = 1.0 (reset)
```

---

#### `OnDestroy()`
```csharp
private void OnDestroy()
```

**Fungsi:** Cleanup DOTween animations saat GameObject dihancurkan

**Alur:**
```
1. DOTween.Kill(rectTransform) → Stop semua animasi pada rectTransform
2. DOTween.Kill(canvasGroup) → Stop semua animasi pada canvasGroup
```

**Penting:** Mencegah error saat scene unload atau GameObject destroy

---

## 🎮 MainMenuManager.cs

### **📌 Variabel Public (Inspector References)**

| Variabel | Tipe | Fungsi |
|----------|------|--------|
| `logoPanel` | GameObject | Reference ke panel Logo Trigosolver |
| `mainMenuPanel` | GameObject | Reference ke panel Main Menu (Mulai, Keluar) |
| `modeSelectionPanel` | GameObject | Reference ke panel Mode Selection |
| `modeCeritaSelectionPanel` | GameObject | Reference ke panel Mode Cerita (Chapter) |
| `clickAnywhereEnabled` | bool | Flag untuk enable/disable "click anywhere" pada logo |

### **🔒 Variabel Private (State & Controllers)**

| Variabel | Tipe | Fungsi |
|----------|------|--------|
| `logoAnimator` | MenuAnimationController | Controller animasi untuk logoPanel |
| `mainMenuAnimator` | MenuAnimationController | Controller animasi untuk mainMenuPanel |
| `modeSelectionAnimator` | MenuAnimationController | Controller animasi untuk modeSelectionPanel |
| `modeCeritaAnimator` | MenuAnimationController | Controller animasi untuk modeCeritaSelectionPanel |
| `currentState` | MenuState (enum) | State menu saat ini |

### **📊 Enum MenuState**

```csharp
private enum MenuState
{
    Logo,                    // State 0: Logo splash screen
    MainMenu,               // State 1: Menu utama (Mulai, Keluar)
    ModeSelection,          // State 2: Pilih mode (Cerita, Bebas)
    ModeCeritaSelection     // State 3: Pilih chapter
}
```

### **🛠️ Fungsi-fungsi**

#### `Awake()`
```csharp
private void Awake()
```

**Fungsi:** Setup awal SEBELUM frame pertama render

**Alur:**
```
1. mainMenuPanel.SetActive(false)
2. modeSelectionPanel.SetActive(false)
3. modeCeritaSelectionPanel.SetActive(false)
```

**Penting:** Dipanggil di `Awake()` bukan `Start()` untuk mencegah panel terlihat sejenak saat game mulai

---

#### `Start()`
```csharp
private void Start()
```

**Fungsi:** Inisialisasi game flow

**Alur:**
```
1. Get MenuAnimationController dari setiap panel:
   - logoAnimator = logoPanel.GetComponent<>()
   - mainMenuAnimator = mainMenuPanel.GetComponent<>()
   - modeSelectionAnimator = modeSelectionPanel.GetComponent<>()
   - modeCeritaAnimator = modeCeritaSelectionPanel.GetComponent<>()

2. Validasi (LogError jika null):
   - Cek apakah semua animator tidak null

3. Set state awal:
   - currentState = MenuState.Logo

4. Mulai game:
   - ShowLogo()
```

---

#### `Update()`
```csharp
private void Update()
```

**Fungsi:** Deteksi input "click anywhere" pada logo screen

**Alur:**
```
IF (currentState == Logo) AND (clickAnywhereEnabled == true):
    IF (Mouse Left Click) OR (Any Key Pressed):
        TransitionToMainMenu()
```

---

#### `ShowLogo()`
```csharp
private void ShowLogo()
```

**Fungsi:** Tampilkan logo dengan animasi drop

**Alur:**
```
1. logoAnimator.AnimateDropIn(callback)
2. Callback setelah animasi selesai:
   - clickAnywhereEnabled = true
```

---

#### `TransitionToMainMenu()`
```csharp
public void TransitionToMainMenu()
```

**Fungsi:** Transisi dari Logo ke Main Menu

**Guard Clause:** Return jika currentState != Logo

**Alur:**
```
1. Cek state harus Logo (jika bukan, return)
2. clickAnywhereEnabled = false (disable input)
3. currentState = MenuState.MainMenu
4. Null check mainMenuAnimator
5. logoAnimator.AnimateSinkOut(callback):
   6. Callback: mainMenuAnimator.AnimateDropIn()
```

**Flow:**
```
Logo (visible) 
  ↓ AnimateSinkOut (0.6s)
Logo (hidden) 
  ↓ Callback triggered
MainMenu AnimateDropIn (0.8s)
  ↓
MainMenu (visible)
```

---

#### `OnMulaiClicked()`
```csharp
public void OnMulaiClicked()
```

**Fungsi:** Handler untuk button "Mulai" (MainMenu → ModeSelection)

**Triggered by:** Button.onClick di Inspector

**Alur:**
```
1. Guard: return jika currentState != MainMenu
2. currentState = MenuState.ModeSelection
3. mainMenuAnimator.AnimateSinkOut(callback):
   4. Callback: modeSelectionAnimator.AnimateDropIn()
```

---

#### `OnModeCeritaClicked()`
```csharp
public void OnModeCeritaClicked()
```

**Fungsi:** Handler untuk button "Mode Cerita" (ModeSelection → ModeCeritaSelection)

**Alur:**
```
1. Guard: return jika currentState != ModeSelection
2. currentState = MenuState.ModeCeritaSelection
3. modeSelectionAnimator.AnimateSinkOut(callback):
   4. Callback: modeCeritaAnimator.AnimateDropIn()
```

---

#### `OnBackFromModeCerita()`
```csharp
public void OnBackFromModeCerita()
```

**Fungsi:** Handler untuk button "Back" (ModeCeritaSelection → ModeSelection)

**Alur:**
```
1. Guard: return jika currentState != ModeCeritaSelection
2. currentState = MenuState.ModeSelection
3. modeCeritaAnimator.AnimateSinkOut(callback):
   4. Callback: modeSelectionAnimator.AnimateDropIn()
```

---

#### `OnBackFromModeSelection()`
```csharp
public void OnBackFromModeSelection()
```

**Fungsi:** Handler untuk button "Back" (ModeSelection → MainMenu)

**Alur:**
```
1. Guard: return jika currentState != ModeSelection
2. currentState = MenuState.MainMenu
3. modeSelectionAnimator.AnimateSinkOut(callback):
   4. Callback: mainMenuAnimator.AnimateDropIn()
```

---

#### `OnKeluarClicked()`
```csharp
public void OnKeluarClicked()
```

**Fungsi:** Handler untuk button "Keluar" (Quit game)

**Alur:**
```
1. Log "Keluar dari game"
2. Application.Quit() → Tutup aplikasi
3. (Jika di Editor) EditorApplication.isPlaying = false
```

---

#### `LoadScene(string sceneName)`
```csharp
public void LoadScene(string sceneName)
```

**Fungsi:** Load scene tertentu (untuk mode cerita/bebas)

**Parameter:**
- `sceneName`: Nama scene yang akan di-load (contoh: "Chapter1")

**Alur:**
```
1. SceneManager.LoadScene(sceneName)
```

---

## 📊 DIAGRAM FLOW

### **1. Game Start Flow**

```
┌─────────────────────────────────────────────────────┐
│              UNITY GAME START                        │
└───────────────────┬─────────────────────────────────┘
                    │
                    ▼
         ┌──────────────────────┐
         │  Awake()             │
         │  ─────────────       │
         │  • Hide MainMenu     │
         │  • Hide ModeSelection│
         │  • Hide ModeCerita   │
         └──────────┬───────────┘
                    │
                    ▼
         ┌──────────────────────┐
         │  Start()             │
         │  ─────────────       │
         │  • Get animators     │
         │  • Validate          │
         │  • Set state = Logo  │
         │  • ShowLogo()        │
         └──────────┬───────────┘
                    │
                    ▼
    ╔═══════════════════════════════════╗
    ║   LOGO PANEL - AnimateDropIn      ║
    ║   ────────────────────────────    ║
    ║   • Start Y = Screen.height × 1.5 ║
    ║   • Alpha = 0                     ║
    ║   • ──────────────────────────    ║
    ║   • Fade In (0.3s)                ║
    ║   • Drop to center (0.8s)         ║
    ║   • Bounce effect (OutBounce)     ║
    ║   • ──────────────────────────    ║
    ║   • OnComplete:                   ║
    ║     - clickAnywhereEnabled = true ║
    ╚═══════════════╦═══════════════════╝
                    │
         ┌──────────▼───────────┐
         │  Update() Loop       │
         │  ────────────        │
         │  Wait for input...   │
         └──────────┬───────────┘
                    │
           [User: Click / Key Press]
                    │
                    ▼
         ┌──────────────────────────┐
         │  TransitionToMainMenu()  │
         │  ────────────────────    │
         │  • clickAnywhereEnabled  │
         │    = false               │
         │  • state = MainMenu      │
         └──────────┬───────────────┘
                    │
                    ▼
    ╔═══════════════════════════════════╗
    ║   LOGO - AnimateSinkOut           ║
    ║   ────────────────────────────    ║
    ║   • Target Y = -Screen.height×1.5 ║
    ║   • ──────────────────────────    ║
    ║   • Sink down (0.6s, InBack)      ║
    ║   • Fade Out (0.42s)              ║
    ║   • ──────────────────────────    ║
    ║   • OnComplete:                   ║
    ║     - SetActive(false)            ║
    ║     - Trigger callback ───────┐   ║
    ╚═══════════════════════════════╧═══╝
                                    │
                                    ▼
                    ╔═══════════════════════════════════╗
                    ║  MAINMENU - AnimateDropIn         ║
                    ║  ────────────────────────────     ║
                    ║  • Initialize() (lazy init)       ║
                    ║  • Start Y = Screen.height × 1.5  ║
                    ║  • Alpha = 0                      ║
                    ║  • SetActive(true)                ║
                    ║  • ──────────────────────────     ║
                    ║  • Fade In (0.3s)                 ║
                    ║  • Drop to center (0.8s, bounce)  ║
                    ╚═══════════════╦═══════════════════╝
                                    │
                                    ▼
                          [Main Menu Visible]
                    [User: Click "Mulai" / "Keluar"]
```

---

### **2. Navigation Flow (State Machine)**

```
     ┌────────────┐
     │    Logo    │ ◄─────────────────────────┐
     │  (State 0) │                           │
     └──────┬─────┘                           │
            │                                 │
            │ Click Anywhere                  │
            ▼                                 │
     ┌────────────┐                           │
     │  MainMenu  │                           │
     │  (State 1) │                           │
     └──┬─────┬───┘                           │
        │     │                               │
        │     └── Button: "Keluar"            │
        │           │                         │
        │           ▼                         │
        │     [Application.Quit()]            │
        │                                     │
        │ Button: "Mulai"                     │
        ▼                                     │
  ┌──────────────────┐                        │
  │  ModeSelection   │                        │
  │    (State 2)     │                        │
  └──┬───────────┬───┘                        │
     │           │                            │
     │           └── Button: "Back" ──────────┘
     │
     │ Button: "Mode Cerita"
     ▼
┌──────────────────────┐
│ ModeCeritaSelection  │
│      (State 3)       │
└──┬──────────────┬────┘
   │              │
   │              └── Button: "Back" ────┐
   │                                     │
   │ Button: "Chapter X"                 │
   │                                     │
   ▼                                     │
[SceneManager.LoadScene("ChapterX")]    │
                                        │
                    ┌───────────────────┘
                    │
                    ▼
            [Back to ModeSelection]
```

---

### **3. Animation Sequence Detail**

```
PANEL TRANSITION: Panel A → Panel B
═══════════════════════════════════

Timeline:
0.0s ─────────── 0.6s ─────────── 1.4s
│                 │                 │
│                 │                 │
Panel A:          │                 │
┌─────────┐       │                 │
│ Visible │       │                 │
│ Alpha:1 │       │                 │
└────┬────┘       │                 │
     │            │                 │
     │ AnimateSinkOut()             │
     ▼            ▼                 │
┌─────────┐  ┌─────────┐           │
│ Sinking │  │ Hidden  │           │
│ Alpha:  │  │ Active: │           │
│ 1→0     │  │ false   │           │
└─────────┘  └────┬────┘           │
                  │                │
                  │ Callback       │
                  │ triggered      │
Panel B:          │                │
                  ▼                ▼
             ┌─────────┐      ┌─────────┐
             │ Dropping│      │ Visible │
             │ Alpha:  │      │ Alpha:1 │
             │ 0→1     │      │ Bounce! │
             └─────────┘      └─────────┘

Total Duration: ~1.4 seconds
  - Sink: 0.6s
  - Drop: 0.8s
  - (animations overlap slightly)
```

---

## 🚀 SETUP GUIDE

### **Step 1: Buat Hierarchy Structure**

```
Canvas
├── MenuManager (Empty GameObject)
├── Logo (Panel/GameObject)
│   └── [Logo Image, Text "Click Anywhere", dll]
├── MainMenu (Panel/GameObject)
│   ├── PlayButton (Button: Text "Mulai")
│   └── ExitButton (Button: Text "Keluar")
├── ModeSelection (Panel/GameObject)
│   ├── ModeCeritaButton (Button)
│   ├── ModeBebasButton (Button)
│   └── BackButton (Button)
└── ModeCeritaSelection (Panel/GameObject)
    ├── Chapter1Button (Button)
    ├── Chapter2Button (Button)
    └── BackButton (Button)
```

---

### **Step 2: Attach Scripts**

1. **Pada MenuManager:**
   - Add Component → **MainMenuManager**

2. **Pada setiap Panel (Logo, MainMenu, ModeSelection, ModeCeritaSelection):**
   - Add Component → **MenuAnimationController**

---

### **Step 3: Assign References di Inspector**

**MainMenuManager (MenuManager GameObject):**

| Field | Assign GameObject |
|-------|-------------------|
| Logo Panel | Logo |
| Main Menu Panel | MainMenu |
| Mode Selection Panel | ModeSelection |
| Mode Cerita Selection Panel | ModeCeritaSelection |

---

### **Step 4: Setup Button Events**

**MainMenu Panel:**
- **PlayButton** → OnClick() → MainMenuManager.OnMulaiClicked

**ModeSelection Panel:**
- **ModeCeritaButton** → OnClick() → MainMenuManager.OnModeCeritaClicked
- **BackButton** → OnClick() → MainMenuManager.OnBackFromModeSelection

**ModeCeritaSelection Panel:**
- **Chapter1Button** → OnClick() → MainMenuManager.LoadScene("Chapter1")
- **BackButton** → OnClick() → MainMenuManager.OnBackFromModeCerita

---

### **Step 5: Tweak Animation Settings**

Pada **MenuAnimationController** di setiap panel, atur:

**Logo Panel:**
- Drop Duration: 1.0s (lebih dramatis)
- Drop Ease: OutBounce
- Sink Duration: 0.6s

**MainMenu Panel:**
- Drop Duration: 0.8s
- Drop Ease: OutBounce
- Sink Duration: 0.6s

**ModeSelection & ModeCerita:**
- Drop Duration: 0.7s (sedikit lebih cepat)
- Drop Ease: OutBounce
- Sink Duration: 0.5s

---

### **Step 6: Initial State Setup**

Di Unity Hierarchy, **disable** (uncheck) panel berikut:
- ❌ MainMenu
- ❌ ModeSelection
- ❌ ModeCeritaSelection
- ✅ Logo (tetap active)

---

## 🐛 TROUBLESHOOTING

### **Problem 1: MainMenu tidak muncul setelah logo sink**

**Symptoms:**
- Logo tenggelam dengan benar
- MainMenu tidak muncul / muncul tanpa animasi

**Possible Causes & Solutions:**

1. **MainMenuAnimator is NULL**
   - **Cek:** Console log "mainMenuAnimator NULL!"
   - **Fix:** Pastikan MainMenu panel memiliki MenuAnimationController script

2. **RectTransform/CanvasGroup NULL**
   - **Cek:** Console log "AnimateDropIn gagal"
   - **Fix:** MenuAnimationController harus attached ke GameObject dengan RectTransform

3. **Initialize() tidak dipanggil**
   - **Fix:** Sudah diperbaiki dengan lazy initialization di AnimateDropIn()

4. **Panel tidak di-assign di MainMenuManager**
   - **Fix:** Drag MainMenu panel ke field "Main Menu Panel" di Inspector

---

### **Problem 2: Animasi tidak smooth / laggy**

**Solutions:**
- Pastikan **DOTween** sudah terinstall (Window → Package Manager)
- Reduce dropDuration dan sinkDuration (misalnya 0.5s dan 0.4s)
- Gunakan Ease type yang lebih simple (Linear, InOutQuad)

---

### **Problem 3: Click Anywhere tidak bekerja**

**Cek:**
1. Console log "clickAnywhereEnabled" → Pastikan true setelah logo drop selesai
2. State harus Logo saat klik
3. Event System harus ada di Scene (Canvas sudah include otomatis)

---

### **Problem 4: Panel muncul sejenak saat game start**

**Fix:**
- Pastikan `Awake()` di MainMenuManager dipanggil SEBELUM `Start()`
- Atau manual disable panel di Inspector (uncheck checkbox)

---

### **Problem 5: Button tidak bisa diklik**

**Cek:**
1. Pastikan ada **Event System** di Scene
2. Button harus memiliki **Graphic Raycaster** (auto ada di Canvas)
3. CanvasGroup di panel tidak boleh:
   - `Interactable = false`
   - `Blocks Raycasts = false`

---

## 📖 REFERENSI API

### **DOTween Methods Used:**

| Method | Fungsi |
|--------|--------|
| `DOTween.Sequence()` | Buat timeline animasi |
| `DOFade(target, duration)` | Fade alpha ke target value |
| `DOAnchorPosY(target, duration)` | Animasi posisi Y (RectTransform) |
| `.SetEase(easeType)` | Set easing function |
| `.Join()` | Jalankan animasi bersamaan |
| `.Append()` | Jalankan animasi setelah animasi sebelumnya |
| `.OnComplete(callback)` | Callback setelah animasi selesai |
| `DOTween.Kill(target)` | Stop semua animasi pada target |

### **Unity Lifecycle Methods:**

| Method | Kapan Dipanggil |
|--------|-----------------|
| `Awake()` | Sebelum Start, bahkan jika GameObject inactive |
| `Start()` | Sebelum Update pertama, setelah semua Awake |
| `Update()` | Setiap frame |
| `OnDestroy()` | Saat GameObject dihancurkan |

---

## 🎯 BEST PRACTICES

1. **Gunakan Lazy Initialization** untuk GameObject yang inactive
2. **Guard Clauses** di semua transisi untuk mencegah state invalid
3. **Callback Chain** untuk animasi sequential yang smooth
4. **Reset state** setelah AnimateSinkOut (posisi + alpha)
5. **Null check** sebelum memanggil method animator
6. **Kill tweens** di OnDestroy untuk cleanup

---

## 📝 CHANGELOG

### Version 1.0 (18 Des 2025)
- ✅ Implementasi MenuAnimationController dengan DOTween
- ✅ Implementasi MainMenuManager dengan State Machine
- ✅ Lazy initialization untuk GameObject inactive
- ✅ Callback chain untuk sequential animations
- ✅ Debug logging untuk troubleshooting
- ✅ Documentation lengkap dengan flow diagram

---

## 👨‍💻 KONTRIBUTOR

- **Rizqi Ackerman** - Developer
- **GitHub Copilot** - Assistant

---

## 📜 LICENSE

Proprietary - Trigosolver Unity 2D Game Project

---

**Last Updated:** 18 Desember 2025
