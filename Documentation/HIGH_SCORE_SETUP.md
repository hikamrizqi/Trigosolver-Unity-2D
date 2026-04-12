# High Score System - Setup Guide

## 📋 Overview

Sistem High Score menggunakan **PlayerPrefs** untuk menyimpan score secara persistent. Score disimpan per-level (Level 1, Level 2) dan total Chapter 1.

### Features:
- ✅ Save high score untuk Level 1 (Soal 1-10)
- ✅ Save high score untuk Level 2 (Soal 11-20)
- ✅ Save total high score Chapter 1
- ✅ Simpan tanggal pencapaian high score
- ✅ Display high score di Main Menu dengan animasi
- ✅ Highlight high score baru
- ✅ Singleton pattern untuk easy access

---

## 🏗️ Architecture

### **1. HighScoreManager.cs** (Singleton)
- Mengelola save/load score menggunakan PlayerPrefs
- Persistent object (DontDestroyOnLoad)
- API methods untuk save dan get scores

### **2. CalculationManager.cs** (Updated)
- Calls `HighScoreManager.Instance.SaveLevel1Score()` saat Level 1 selesai
- Calls `HighScoreManager.Instance.SaveLevel2Score()` saat Level 2 selesai
- Tracks `startingQuestion` untuk tahu level mana yang dimainkan

### **3. HighScoreDisplay.cs** (UI Component)
- Menampilkan high scores di Main Menu
- Animasi slide in untuk scores
- Highlight effect untuk new high scores

### **4. MainMenuManager.cs** (Updated)
- Refresh scores saat main menu muncul
- Optional reference ke HighScoreDisplay

---

## 🛠️ Setup Instructions

### **Step 1: Setup HighScoreManager**

1. Buat Empty GameObject di scene **MainMenu** (atau scene awal)
   ```
   Managers/
   └── HighScoreManager (Empty GameObject)
   ```

2. Add Component: **HighScoreManager** script
   
3. ✅ **DONE!** HighScoreManager akan auto-create singleton dan persist antar scenes

> **Note:** HighScoreManager tidak perlu ada di Chapter1 scene karena menggunakan singleton pattern.

---

### **Step 2: Setup High Score Display di Main Menu**

1. **Buat UI untuk High Score Display:**

Di Hierarchy Main Menu:
```
Canvas/
└── MainMenuPanel/
    └── HighScorePanel                  ⭐ NEW
        ├── Title ("HIGH SCORES")
        ├── Level1Container
        │   ├── Label ("Level 1:")
        │   ├── ScoreText               → Assign to level1ScoreText
        │   └── DateText (optional)     → Assign to level1DateText
        ├── Level2Container
        │   ├── Label ("Level 2:")
        │   ├── ScoreText               → Assign to level2ScoreText
        │   └── DateText (optional)     → Assign to level2DateText
        └── TotalContainer
            ├── Label ("Total:")
            ├── ScoreText               → Assign to totalScoreText
            └── DateText (optional)     → Assign to totalDateText
```

2. **Add HighScoreDisplay Component:**
   - Select `HighScorePanel`
   - Add Component: **HighScoreDisplay** script

3. **Assign References di Inspector:**

| Field | Assign To |
|-------|-----------|
| `Level1 Score Text` | Level1Container/ScoreText |
| `Level2 Score Text` | Level2Container/ScoreText |
| `Total Score Text` | TotalContainer/ScoreText |
| `Level1 Date Text` | Level1Container/DateText (optional) |
| `Level2 Date Text` | Level2Container/DateText (optional) |
| `Total Date Text` | TotalContainer/DateText (optional) |

4. **Configure Animation Settings:**
   - `Animate On Enable`: ✅ Checked
   - `Animation Duration`: 0.5
   - `Animation Delay`: 0.2
   - `Normal Color`: White
   - `Highlight Color`: Yellow

---

### **Step 3: Link HighScoreDisplay ke MainMenuManager**

1. Select `MainMenuManager` GameObject
2. Di Inspector, find field: **High Score Display**
3. Drag `HighScorePanel` (yang punya HighScoreDisplay component) ke field tersebut

✅ Sekarang score akan auto-refresh saat main menu muncul!

---

## 📊 Score Calculation

### **Score Rules:**
- **Correct Answer:** +10 score
- **Wrong Answer:** No score change (only lose lives)

### **Level Completion:**
- **Level 1:** 10 questions × 10 points = **Max 100 points**
- **Level 2:** 10 questions × 10 points = **Max 100 points**
- **Total Chapter 1:** **Max 200 points**

### **High Score Logic:**
```csharp
// Level 1 selesai (progres >= 10)
if (startingQuestion == 1) {
    HighScoreManager.Instance.SaveLevel1Score(score);
}

// Level 2 selesai (progres >= 20)
if (startingQuestion == 11) {
    HighScoreManager.Instance.SaveLevel2Score(score);
}

// Total score dihitung otomatis
if (progres >= 20) {
    int total = level1HighScore + level2HighScore;
    HighScoreManager.Instance.SaveTotalScore(total);
}
```

**Important:** Score hanya disimpan jika **lebih tinggi** dari high score sebelumnya!

---

## 🎨 UI Design Recommendations

### **Layout Suggestion:**

```
┌─────────────────────────────────┐
│       HIGH SCORES               │
├─────────────────────────────────┤
│                                 │
│  Level 1:         100      ⭐   │
│  (2025-12-22)                   │
│                                 │
│  Level 2:          80      ⭐   │
│  (2025-12-22)                   │
│                                 │
│  Total:           180      🏆   │
│  (2025-12-22)                   │
│                                 │
└─────────────────────────────────┘
```

### **Styling Tips:**
- **Font:** Bold untuk score numbers
- **Color:** 
  - Normal: White/Light Gray
  - Highlight: Yellow/Gold
- **Size:** Score numbers lebih besar dari labels
- **Background:** Semi-transparent panel dengan border
- **Icons:** ⭐ untuk levels, 🏆 untuk total

---

## 🧪 Testing Guide

### **Test 1: First Time Playing**
1. Fresh install (atau reset PlayerPrefs)
2. Main menu shows: `---` untuk semua scores
3. Date text shows: `Belum Main`

### **Test 2: Complete Level 1**
1. Pilih Level 1
2. Complete dengan score misalnya 80
3. Return ke main menu
4. Verify: Level 1 shows `80`, date shows today

### **Test 3: Beat High Score**
1. Pilih Level 1 lagi
2. Complete dengan score 90 (lebih tinggi dari 80)
3. Return ke main menu
4. Verify: Level 1 updated ke `90`

### **Test 4: Lower Score (Should NOT Save)**
1. Pilih Level 1 lagi
2. Complete dengan score 70 (lebih rendah dari 90)
3. Return ke main menu
4. Verify: Level 1 tetap `90` (tidak berubah)

### **Test 5: Complete Both Levels**
1. Complete Level 1 dengan score 100
2. Complete Level 2 dengan score 90
3. Return ke main menu
4. Verify: Total shows `190` (100 + 90)

### **Test 6: Animation**
1. Transition ke main menu
2. Verify: Scores slide in dengan delay
3. Verify: Scale animation (OutBack ease)

---

## 🔧 Advanced Usage

### **Get High Score dari Script Lain:**

```csharp
// Get specific level score
int level1Score = HighScoreManager.Instance.GetLevel1HighScore();
int level2Score = HighScoreManager.Instance.GetLevel2HighScore();
int totalScore = HighScoreManager.Instance.GetTotalHighScore();

// Get score summary (all scores at once)
ScoreSummary summary = HighScoreManager.Instance.GetScoreSummary();
Debug.Log(summary); // "Level 1: 100 | Level 2: 90 | Total: 190"

// Check if player has played before
bool hasPlayed = HighScoreManager.Instance.HasPlayedBefore();
```

### **Highlight New High Score:**

```csharp
// Saat dapat new high score, bisa trigger highlight effect
if (newHighScore) {
    highScoreDisplay.HighlightLevel1Score(); // Yellow flash 3x
}
```

### **Reset All Scores (for Testing):**

```csharp
// Console command atau debug button
HighScoreManager.Instance.ResetAllScores();
highScoreDisplay.RefreshScores(); // Refresh display
```

### **Format Score dengan Padding:**

```csharp
// Display score dengan leading zeros: 0050
string formatted = HighScoreDisplay.FormatScore(50, digits: 4);
// Output: "0050"
```

### **Get Score Rank:**

```csharp
string rank = HighScoreDisplay.GetScoreRank(85);
// Output: "A"
// S = 100+, A = 80+, B = 60+, C = 40+, D = <40
```

---

## 📂 File Structure

```
Assets/
├── Scripts/
│   ├── Managers/
│   │   ├── HighScoreManager.cs          ⭐ NEW
│   │   └── Chapter1/
│   │       └── CalculationManager.cs    (UPDATED)
│   ├── UI/
│   │   └── HighScoreDisplay.cs          ⭐ NEW
│   └── Main Menu/
│       └── MainMenuManager.cs           (UPDATED)
```

---

## 🚨 Troubleshooting

### **Problem:** Scores tidak tersimpan
**Solution:**
- Check Console untuk errors
- Verify `HighScoreManager` exist di scene
- Check `PlayerPrefs` dengan `Debug.Log(PlayerPrefs.GetInt("Chapter1_Level1_HighScore"))`

### **Problem:** Scores tidak muncul di Main Menu
**Solution:**
- Verify `HighScoreDisplay` component attached
- Check all TextMeshProUGUI references assigned
- Call `RefreshScores()` manually untuk test

### **Problem:** Score saved tapi tidak update di UI
**Solution:**
- Check `MainMenuManager` punya reference ke `HighScoreDisplay`
- Verify `RefreshScores()` dipanggil saat main menu show
- Check `OnEnable()` di `HighScoreDisplay` berfungsi

### **Problem:** Animation tidak smooth
**Solution:**
- Adjust `animationDuration` dan `animationDelay`
- Check DOTween installed dan berfungsi
- Verify texts punya alpha channel (RGBA, bukan RGB)

### **Problem:** PlayerPrefs corrupt atau strange values
**Solution:**
```csharp
// Reset PlayerPrefs completely
PlayerPrefs.DeleteAll();
PlayerPrefs.Save();
```

---

## 🔐 PlayerPrefs Keys Reference

| Key | Description | Type |
|-----|-------------|------|
| `Chapter1_Level1_HighScore` | High score Level 1 | int |
| `Chapter1_Level2_HighScore` | High score Level 2 | int |
| `Chapter1_Total_HighScore` | Total high score | int |
| `Chapter1_Level1_Date` | Date Level 1 achieved | string |
| `Chapter1_Level2_Date` | Date Level 2 achieved | string |
| `Chapter1_Total_Date` | Date total achieved | string |

**Location:**
- **Windows:** Registry `HKCU\Software\[CompanyName]\[ProductName]`
- **Mac:** `~/Library/Preferences/com.CompanyName.ProductName.plist`
- **Linux:** `~/.config/unity3d/CompanyName/ProductName/prefs`

---

## ✅ Setup Complete Checklist

- [ ] HighScoreManager GameObject created in Main Menu scene
- [ ] HighScorePanel UI created dengan score texts
- [ ] HighScoreDisplay component added dan references assigned
- [ ] MainMenuManager linked ke HighScoreDisplay
- [ ] Tested: Scores save dan load correctly
- [ ] Tested: UI displays scores dengan animasi
- [ ] Tested: High scores only update when beaten
- [ ] Tested: Dates show correctly

---

## 🎮 User Experience Flow

```
Player completes Level 1 with score 80
    ↓
CalculationManager.EndChapter() called
    ↓
SaveLevelScore() checks: startingQuestion == 1
    ↓
HighScoreManager.Instance.SaveLevel1Score(80)
    ↓
Score saved to PlayerPrefs
    ↓
Player returns to Main Menu
    ↓
MainMenuManager shows Main Menu
    ↓
Calls highScoreDisplay.RefreshScores()
    ↓
HighScoreDisplay gets score from HighScoreManager
    ↓
Display "80" dengan slide-in animation ✨
    ↓
Player sees their high score! 🎉
```

---

**Created:** December 22, 2025  
**Feature:** High Score System with PlayerPrefs  
**Dependencies:** DOTween, TextMeshPro
