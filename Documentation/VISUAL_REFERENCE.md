# 🎨 Visual Reference - Setup Unity Editor

## Stage 1: Layout Visual

### Canvas UI Layout
```
┌──────────────────────────────────────────────┐
│ HEADER                                       │
│ Chapter 1 - Observasi    Soal: 1/5  ❤❤❤    │
├──────────────────────────────────────────────┤
│                                              │
│                                              │
│         [SEGITIGA DI TENGAH SCREEN]         │
│                                              │
│              5 (Miring)                      │
│             /│                               │
│            / │ 3 (Depan)                     │
│           /  │                               │
│          /___│                               │
│            4 (Samping)                       │
│                                              │
│                                              │
├──────────────────────────────────────────────┤
│   QUESTION PANEL                             │
│   Berapakah nilai Sin θ?                     │
│                                              │
│   ┌────────────────────┐                     │
│   │  0.6 atau 3/5      │ (InputField)        │
│   └────────────────────┘                     │
│                                              │
│        [ VERIFIKASI ]   (Button)             │
├──────────────────────────────────────────────┤
│ FEEDBACK (muncul saat jawab)                 │
│ ✓ BENAR! +10 poin                           │
└──────────────────────────────────────────────┘
```

### Triangle Detail (GameWorld)
```
      Miring Label "5" (Red)
            ↓
    ╱───────────╲
   ╱ MiringSide  ╲ (Red Sprite)
  ╱               ╲
 ╱                 ╲
┌─────────────────┐←─── DepanSide (Blue Sprite)
│                 │
│  Depan Label    │
│     "3"         │
│    (Blue)       │
│                 │
└─────────────────┘
        ↓
  Samping Label "4" (Green)
  SampingSide (Green Sprite)
```

---

## Stage 2: Layout Visual

### Canvas UI Layout
```
┌──────────────────────────────────────────────┐
│ HEADER                                       │
│ Jarak target: 100m. Hitung sudut elevasi!   │
├──────────────────────────────────────────────┤
│                                              │
│  🏴 (Target Ship)                            │
│   ↑                                          │
│    ╲                                         │
│     ╲ (Projectile Path)                     │
│      ╲                                       │
│       ○ ← Peluru                             │
│        ╲                                     │
│         ⟋  ← Cannon (rotated)               │
│        ▓▓                                    │
│ ▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁ (Ground)       │
│ ≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈ (Water)        │
│                                              │
├──────────────────────────────────────────────┤
│   ┌────────┐          ┌─────────┐           │
│   │ 45°    │          │ TEMBAK! │           │
│   └────────┘          └─────────┘           │
│  (Input)              (Button)               │
└──────────────────────────────────────────────┘
```

### Cannon Detail
```
Hierarchy:
  Cannon (Empty GameObject)
    ├─ CannonBase (Sprite - Dark Gray Square)
    │   [  ▓▓  ] ← Base tidak rotate
    │
    └─ CannonBarrel (Sprite - Gray Rectangle) ← CannonController DI SINI!
        │  ═══════╗ ← Rotate berdasarkan angle
        │         ║
        └─ ShootPoint (Empty Transform)
            ↑ 
         Posisi spawn peluru
```

---

## Color Scheme Reference

### Stage 1 (Observasi)
```
Sisi Depan:   🔵 Blue    (R: 0,   G: 100, B: 255)
Sisi Samping: 🟢 Green   (R: 0,   G: 200, B: 0)
Sisi Miring:  🔴 Red     (R: 255, G: 0,   B: 0)

Highlights:
- Default:    ⚪ White   (R: 255, G: 255, B: 255)
- Kuning:     🟡 Yellow  (R: 255, G: 255, B: 0)
- Benar:      🟢 Green   (R: 0,   G: 255, B: 0)
- Salah:      🔴 Red     (R: 255, G: 0,   B: 0)
```

### Stage 2 (Cannon)
```
Cannon Base:  ⬛ Dark Gray (R: 50,  G: 50,  B: 50)
Cannon Barrel:⬜ Gray      (R: 128, G: 128, B: 128)
Projectile:   ⚫ Black     (R: 0,   G: 0,   B: 0)
Target:       🔴 Red       (R: 200, G: 0,   B: 0)
Ground:       🟤 Brown     (R: 139, G: 69,  B: 19)
Water:        🔵 Blue      (R: 100, G: 180, B: 255)
Sky:          💙 Light Blue(R: 135, G: 206, B: 235)
```

---

## Inspector Settings Quick Reference

### TextMeshPro Settings (UI)
```
Font Size:
- Title/Header: 36-48
- Question: 32-40
- Feedback: 28-32
- Small text: 20-24

Alignment:
- Center (untuk judul, pertanyaan)
- Left (untuk progress, info)

Best Practice:
✓ Enable Auto Size: false
✓ Enable Word Wrapping: true
✓ Overflow: Truncate atau Ellipsis
```

### TextMeshPro (World Space)
```
Font Size: 5-10 (world space lebih kecil)
Alignment: Center
Sorting Layer: Default
Order in Layer: 5 (di atas sprite)

Position: Relative ke parent sprite
- Label di samping: X offset +0.5
- Label di atas: Y offset +0.5
```

### SpriteRenderer Settings
```
Sprite: Assign sprite asset
Color: Sesuai skema warna
Material: Sprites-Default
Sorting Layer: Default
Order in Layer:
  - Background: -1
  - Game objects: 0-3
  - UI elements: 4-10
```

### Button Settings
```
Target Graphic: Image (background)
Interactable: ✓
Transition: Color Tint

Colors:
- Normal: Sesuai tema
- Highlighted: Sedikit lebih terang
- Pressed: Lebih gelap
- Disabled: Gray (R:128, G:128, B:128)

Navigation: Automatic
```

### Rigidbody2D (Projectile)
```
Body Type: Dynamic
Material: None
Simulated: ✓
Use Auto Mass: □
Mass: 1
Linear Drag: 0
Angular Drag: 0.05
Gravity Scale: 1 (untuk physics realistis)
Collision Detection: Continuous (untuk high-speed)
Sleeping Mode: Start Awake
Interpolate: None
Constraints: □ Freeze Rotation (jika ingin bola tidak spin)
```

---

## Transform Settings Quick Ref

### Stage 1 Triangle Positions
```
Triangle Center: (0, 1, 0)

DepanSide:
  Position: (-2, 0, 0)
  Rotation: (0, 0, 90)
  Scale: (3, 0.1, 1)

SampingSide:
  Position: (-0.5, -1.5, 0)
  Rotation: (0, 0, 0)
  Scale: (4, 0.1, 1)

MiringSide:
  Position: (0.5, 0, 0)
  Rotation: (0, 0, -37)
  Scale: (5, 0.1, 1)
```

### Stage 2 Cannon Positions
```
Cannon: (-8, -3, 0)

CannonBase (child):
  Position: (0, 0, 0)
  Scale: (1, 0.8, 1)

CannonBarrel (child):
  Position: (0.6, 0.2, 0)
  Rotation: (0, 0, 0) ← Will rotate via script
  Scale: (1.2, 0.3, 1)

ShootPoint (child of CannonBarrel):
  Position: (1.2, 0, 0)
```

---

## Particle System Quick Setup

### Sparkle Effect (Stage 1)
```
Main Module:
✓ Duration: 1
✓ Looping: □ (false)
✓ Start Lifetime: 0.5-1
✓ Start Speed: 1-3
✓ Start Size: 0.1-0.3
✓ Start Color: Yellow/Gold
✓ Gravity Modifier: 0
✓ Play On Awake: □ (false)

Emission:
✓ Rate over Time: 50-100

Shape:
✓ Shape: Circle
✓ Radius: 0.5-1
✓ Emit from: Edge

Color over Lifetime:
✓ Gradient: Yellow → Transparent

Size over Lifetime:
✓ Curve: Start 1 → End 0
```

---

## Testing Positions

### Stage 1 - Klik Test Points:
```
1. Play Scene
2. Input "0.6" atau "3/5"
3. Click Verify
4. Expected:
   - Highlight changes color ✓
   - Feedback shows ✓
   - Progress updates ✓
   - Lives update (if wrong) ✓
```

### Stage 2 - Physics Test:
```
1. Play Scene
2. Input angle "45"
3. Click Shoot
4. Expected:
   - Cannon rotates to 45° ✓
   - Projectile spawns at ShootPoint ✓
   - Parabolic trajectory ✓
   - Hits target or ground ✓
   - Feedback appears ✓
```

---

## Common Unity Shortcuts

```
F - Focus on selected GameObject
W - Move tool
E - Rotate tool
R - Scale tool
Q - Hand tool (pan view)

Ctrl + D - Duplicate
Ctrl + Shift + N - New empty GameObject
Alt + Shift + C - Create child GameObject

Space - Play/Pause
Shift + Space - Pause
Ctrl + Space - Step forward one frame
```

---

## Asset Organization (Recommended)

```
Assets/
├── Scenes/
│   ├── MainMenu.unity
│   ├── Stage1_Scene.unity
│   └── Stage2_Scene.unity
│
├── Scripts/
│   ├── Stage 1/ (sudah ada)
│   └── Stage 2/ (sudah ada)
│
├── Sprites/
│   ├── Stage1/
│   │   ├── triangle_depan.png
│   │   ├── triangle_samping.png
│   │   ├── triangle_miring.png
│   │   ├── heart_full.png
│   │   └── heart_empty.png
│   │
│   └── Stage2/
│       ├── cannon_base.png
│       ├── cannon_barrel.png
│       ├── projectile.png
│       ├── target_ship.png
│       └── ground.png
│
├── Prefabs/
│   ├── Projectile.prefab
│   └── SparkleEffect.prefab
│
├── Audio/ (opsional)
│   ├── SFX/
│   │   ├── correct.wav
│   │   ├── wrong.wav
│   │   └── shoot.wav
│   └── Music/
│       └── background.mp3
│
└── Fonts/
    └── (TextMeshPro fonts)
```

---

**Tips Terakhir:**
1. ✅ Save Scene sering-sering (Ctrl+S)
2. ✅ Test setelah setiap langkah
3. ✅ Gunakan Prefabs untuk object yang reusable
4. ✅ Organize Hierarchy dengan Empty GameObjects sebagai folder
5. ✅ Beri nama yang jelas dan konsisten

**Happy Creating! 🎮**
