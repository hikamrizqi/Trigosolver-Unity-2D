# Level Selection System - Setup Guide

## 📋 Overview

Sistem Level Selection menampilkan 2 tombol di awal game:
- **Level 1:** Soal 1-10 (Single Question - 2 slots)
- **Level 2:** Soal 11-20 (Dual Question - 4 slots)

Semua objek game (segitiga, tiles, background) **HIDDEN** saat awal, baru muncul setelah pilih level dengan animasi slide in.

---

## 🛠️ Setup di Unity

### 1. **Buat UI Panel untuk Level Selection**

Di Hierarchy:
```
Canvas (Chapter1)/
├── LevelSelectionPanel         ⭐ BARU - Parent panel
│   ├── Background              (Optional - dark overlay)
│   ├── TitleText              ("Pilih Level")
│   ├── Level1Button           ⭐ Button: "Level 1 - Soal 1-10"
│   └── Level2Button           ⭐ Button: "Level 2 - Soal 11-20"
```

#### **Layout Settings:**

**LevelSelectionPanel:**
- Anchor: Center
- Pos X: 0, Pos Y: 0
- Width: 800, Height: 600
- Add Component: **CanvasGroup** (untuk fade effect nanti)

**Level1Button:**
- Pos X: 0, Pos Y: 100
- Width: 400, Height: 80
- Text: "Level 1\nSoal 1-10"
- Font Size: 28

**Level2Button:**
- Pos X: 0, Pos Y: -100
- Width: 400, Height: 80
- Text: "Level 2\nSoal 11-20"
- Font Size: 28

---

### 2. **Buat GameObject untuk LevelSelectionManager**

Di Hierarchy:
```
Managers/
└── LevelSelectionManager       ⭐ BARU - Empty GameObject
```

1. Buat Empty GameObject, rename jadi `LevelSelectionManager`
2. Add Component: **LevelSelectionManager** script

---

### 3. **Assign References di Inspector**

Pilih `LevelSelectionManager` GameObject, lalu assign:

#### **Level Selection UI:**
| Field | Assign To |
|-------|-----------|
| `Level Selection Panel` | `Canvas/LevelSelectionPanel` |
| `Level1 Button` | `Canvas/LevelSelectionPanel/Level1Button` |
| `Level2 Button` | `Canvas/LevelSelectionPanel/Level2Button` |

#### **Game Objects to Hide/Show:**
| Field | Assign To |
|-------|-----------|
| `Triangle Visualizer Object` | GameObject yang punya TriangleVisualizer |
| `Answer Tile System Object` | GameObject yang punya AnswerTileSystem |
| `Background Object` | Background meja kayu |
| `Question Panel Object` | Panel pertanyaan |
| `Interactive Button Panel` | Panel tombol DEPAN/SAMPING/MIRING |
| `Check Button Object` | Tombol CHECK |

#### **Manager References:**
| Field | Assign To |
|-------|-----------|
| `Calculation Manager` | GameObject yang punya CalculationManager |

#### **Animation Settings:**
| Field | Value | Description |
|-------|-------|-------------|
| `Animation Duration` | 0.5 | Durasi animasi slide (detik) |
| `Slide Distance` | 1000 | Jarak slide offscreen (pixel) |
| `Slide Ease` | OutCubic | Easing function |

---

## 🎮 How It Works

### **Flow Diagram:**

```
Game Start
    ↓
Hide All Game Objects (instant)
    ↓
Show Level Selection Panel (slide in from top)
    ↓
User Clicks Level 1 or Level 2
    ↓
Panel Slides Out (up) ← 0.5 detik
    ↓
All Game Objects Slide In (from edges) ← 0.5 detik
    ↓
Start Game from Question 1 or 11
```

### **Animation Directions:**

**Level Selection Panel:**
- Entry: Slide DOWN from TOP
- Exit: Slide UP to TOP

**Game Objects (setelah level dipilih):**
- Background: Slide in from BOTTOM
- Triangle: Slide in from LEFT
- Answer Tiles: Slide in from RIGHT
- Question Panel: Slide in from TOP
- Buttons: Slide in from BOTTOM

---

## 🧪 Testing Checklist

### **Initial State:**
- [ ] Saat game start, HANYA Level Selection Panel terlihat
- [ ] Semua objek lain HIDDEN (segitiga, tiles, background, dll)
- [ ] Panel slide in dari atas dengan smooth

### **Level 1 Button:**
- [ ] Klik Level 1 → Panel slide out ke atas
- [ ] Semua game objects slide in dari edges
- [ ] Game mulai dari **Soal 1**
- [ ] Soal 1-10 menggunakan **Single Question** (2 slots, θ)

### **Level 2 Button:**
- [ ] Klik Level 2 → Panel slide out ke atas
- [ ] Semua game objects slide in dari edges
- [ ] Game mulai dari **Soal 11**
- [ ] Soal 11-20 menggunakan **Dual Question** (4 slots, A & B)

### **Animation Quality:**
- [ ] Tidak ada glitch/jump saat animasi
- [ ] Durasi animasi smooth (0.5 detik)
- [ ] Easing terasa natural (OutCubic)

---

## 🚨 Troubleshooting

### **Problem:** Game langsung start tanpa level selection
**Solution:**
- Check `CalculationManager.Start()` - pastikan tidak ada `StartNewRound()` call
- Pastikan `LevelSelectionManager` aktif di scene

### **Problem:** Objects tidak hidden di awal
**Solution:**
- Check apakah semua game objects sudah ter-assign di Inspector
- Pastikan `LevelSelectionManager.Start()` dipanggil (GameObject aktif)

### **Problem:** Button tidak berfungsi
**Solution:**
- Check Button component - pastikan ada EventSystem di scene
- Check Console - pastikan tidak ada error saat klik
- Verify `level1Button` dan `level2Button` ter-assign di Inspector

### **Problem:** Animation tidak smooth
**Solution:**
- Adjust `animationDuration` (coba 0.7 atau 0.3)
- Adjust `slideDistance` (coba 1500 atau 800)
- Coba Ease function lain (OutQuad, OutQuart, OutBack)

### **Problem:** Game objects tidak slide in dengan benar
**Solution:**
- Pastikan objects menggunakan **RectTransform** (UI elements)
- Check hierarchy - pastikan objects adalah children of Canvas
- Verify anchored positions di Inspector

---

## 📝 Code Summary

### **New Files:**
- `LevelSelectionManager.cs` - Manages level selection UI and game object visibility

### **Modified Files:**
- `CalculationManager.cs`:
  - Added `gameStarted` flag
  - Added `StartFromQuestion(int)` public method
  - Modified `Start()` to NOT auto-start game

### **Key Methods:**

**LevelSelectionManager:**
- `Start()` - Hide game objects, show level panel
- `OnLevelSelected(int level)` - Handle button click
- `LevelSelectionSequence()` - Animate panel out, objects in, start game
- `HideAllGameObjects()` - Hide with/without animation
- `ShowAllGameObjects()` - Show with/without animation

**CalculationManager:**
- `StartFromQuestion(int)` - Start game from specific question number

---

## 🎨 Customization

### **Button Styling:**
Edit di Unity Inspector:
- Font: TextMeshPro
- Color: Normal (white), Highlighted (yellow), Pressed (green)
- Background: Rounded rectangle sprite
- Shadow/Outline untuk depth

### **Animation Tweaking:**
Di `LevelSelectionManager` Inspector:
- `Animation Duration`: 0.3 (fast) - 0.7 (slow)
- `Slide Distance`: 800 (short) - 1500 (long)
- `Slide Ease`: Try OutBack untuk bounce effect

### **Panel Design:**
- Add blur background (UI Blur shader)
- Add title text dengan glow effect
- Add level icons/images
- Add difficulty indicators (⭐⭐ vs ⭐⭐⭐⭐)

---

## ✅ Setup Complete

Setelah setup:
1. ✅ Level Selection Panel created with 2 buttons
2. ✅ LevelSelectionManager GameObject with script
3. ✅ All references assigned in Inspector
4. ✅ Tested Level 1 starts from question 1
5. ✅ Tested Level 2 starts from question 11
6. ✅ All animations working smoothly

---

**Created:** December 22, 2025
**Feature:** Level Selection System
**Commit:** becd672
