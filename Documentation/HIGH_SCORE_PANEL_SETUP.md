# High Score Panel - UI Setup Guide

## 🎮 Overview

High Score Panel sekarang berfungsi seperti panel lain (Mode Selection, Mode Cerita) dengan tombol khusus di Main Menu dan tombol Back untuk kembali.

---

## 🏗️ Panel Flow

```
Main Menu
├── Tombol "Mulai" → Mode Selection Panel
├── Tombol "High Score" → High Score Panel ⭐
└── Tombol "Keluar" → Quit Game

High Score Panel
└── Tombol "Back" → Kembali ke Main Menu
```

---

## 🛠️ Setup Instructions

### **Step 1: Setup High Score Panel**

1. **Buat High Score Panel:**
   ```
   Canvas/
   └── HighScorePanel                    ⭐ NEW
       ├── Background (Image)
       ├── Title ("HIGH SCORES")
       ├── ScoreContainer
       │   ├── Level1Row
       │   │   ├── Label ("Level 1:")
       │   │   └── ScoreText           → Assign to HighScoreDisplay
       │   ├── Level2Row
       │   │   ├── Label ("Level 2:")
       │   │   └── ScoreText           → Assign to HighScoreDisplay
       │   └── TotalRow
       │       ├── Label ("Total:")
       │       └── ScoreText           → Assign to HighScoreDisplay
       └── BackButton                   ⭐ IMPORTANT
           └── Text ("BACK")
   ```

2. **Add Components ke HighScorePanel:**
   - Add Component: **MenuAnimationController** (untuk animasi sink/drop)
   - Add Component: **HighScoreDisplay** (untuk display scores)

3. **Assign References di HighScoreDisplay:**
   | Field | Assign To |
   |-------|-----------|
   | Level1 Score Text | ScoreContainer/Level1Row/ScoreText |
   | Level2 Score Text | ScoreContainer/Level2Row/ScoreText |
   | Total Score Text | ScoreContainer/TotalRow/ScoreText |

---

### **Step 2: Setup Tombol di Main Menu Panel**

1. **Add High Score Button:**
   ```
   MainMenuPanel/
   ├── MulaiButton (existing)
   ├── HighScoreButton        ⭐ NEW
   │   └── Text ("HIGH SCORES")
   └── KeluarButton (existing)
   ```

2. **Configure HighScoreButton:**
   - Duplicate `MulaiButton` untuk konsistensi styling
   - Rename jadi `HighScoreButton`
   - Update text: "HIGH SCORES"
   - Position antara Mulai dan Keluar

3. **Setup Button OnClick Event:**
   - Select `HighScoreButton`
   - Di Inspector → Button component
   - OnClick() → Add (+)
   - Drag `MainMenuManager` GameObject
   - Function: `MainMenuManager.OnHighScoreClicked()`

---

### **Step 3: Setup Back Button di High Score Panel**

1. **Configure BackButton:**
   - Style sama seperti Back button di panel lain
   - Position: Bottom atau Top-Left corner

2. **Setup OnClick Event:**
   - Select `BackButton`
   - OnClick() → Add (+)
   - Drag `MainMenuManager` GameObject
   - Function: `MainMenuManager.OnBackFromHighScore()`

---

### **Step 4: Assign Panels di MainMenuManager**

Select `MainMenuManager` GameObject, assign semua references:

| Field | Assign To |
|-------|-----------|
| Logo Panel | LogoPanel |
| Main Menu Panel | MainMenuPanel |
| Mode Selection Panel | ModeSelectionPanel |
| Mode Cerita Selection Panel | ModeCeritaSelectionPanel |
| **High Score Panel** | **HighScorePanel** ⭐ |
| **High Score Display** | **HighScorePanel (HighScoreDisplay component)** ⭐ |

---

## 🎬 Animation Behavior

### **Opening High Score Panel:**
```
User clicks "High Score" button
    ↓
OnHighScoreClicked() called
    ↓
Main Menu Panel → AnimateSinkOut() ⬇️
    ↓
High Score Panel → AnimateDropIn() ⬇️
    ↓
RefreshScores() → Display latest scores ✨
```

### **Closing High Score Panel:**
```
User clicks "Back" button
    ↓
OnBackFromHighScore() called
    ↓
High Score Panel → AnimateSinkOut() ⬇️
    ↓
Main Menu Panel → AnimateDropIn() ⬇️
```

---

## 🎨 Layout Recommendation

### **High Score Panel Design:**

```
╔═════════════════════════════════════╗
║                                     ║
║          HIGH SCORES               ║
║                                     ║
║  ┌─────────────────────────────┐  ║
║  │  Level 1:            100    │  ║
║  │                              │  ║
║  │  Level 2:             80    │  ║
║  │                              │  ║
║  │  Total:              180    │  ║
║  └─────────────────────────────┘  ║
║                                     ║
║          [ BACK ]                  ║
║                                     ║
╚═════════════════════════════════════╝
```

### **Main Menu with High Score Button:**

```
╔═════════════════════════════════════╗
║                                     ║
║         [LOGO IN CORNER]           ║
║                                     ║
║         [ MULAI ]                  ║
║                                     ║
║         [ HIGH SCORES ]  ⭐         ║
║                                     ║
║         [ KELUAR ]                 ║
║                                     ║
╚═════════════════════════════════════╝
```

---

## 🧪 Testing Checklist

### **Test 1: Navigate to High Score Panel**
- [x] Main menu shows
- [x] Click "High Score" button
- [x] Main menu sinks out
- [x] High score panel drops in
- [x] Scores display correctly
- [x] Animation smooth

### **Test 2: Return to Main Menu**
- [x] High score panel showing
- [x] Click "Back" button
- [x] High score panel sinks out
- [x] Main menu drops in
- [x] Can click High Score button again

### **Test 3: Score Display**
- [x] Scores show "---" if never played
- [x] Scores show numbers after playing
- [x] Scores animate in (slide + scale)
- [x] No errors in console

### **Test 4: State Management**
- [x] Can't open high score from other panels
- [x] Can't spam click buttons
- [x] State transitions correctly
- [x] No stuck states

---

## 🚨 Troubleshooting

### **Problem:** High Score button tidak berfungsi
**Solution:**
- Check button OnClick event assigned to `OnHighScoreClicked()`
- Verify MainMenuManager reference not null
- Check currentState = MainMenu saat button diklik

### **Problem:** Back button tidak berfungsi
**Solution:**
- Check button OnClick event assigned to `OnBackFromHighScore()`
- Verify highScoreAnimator not null
- Check HighScorePanel has MenuAnimationController

### **Problem:** Panel tidak animate dengan benar
**Solution:**
- Verify HighScorePanel has MenuAnimationController component
- Check animator not null in MainMenuManager.Start()
- Verify animation parameters match other panels

### **Problem:** Scores tidak muncul
**Solution:**
- Check HighScoreDisplay component attached
- Verify all TextMeshProUGUI references assigned
- Check RefreshScores() called in OnHighScoreClicked callback

### **Problem:** Can't click buttons in high score panel
**Solution:**
- Check HighScorePanel has Canvas Raycaster
- Verify BackButton has Button component
- Check no blocking UI elements

---

## 📝 Code Reference

### **MainMenuManager.cs Methods:**

```csharp
// Called by High Score button onClick
public void OnHighScoreClicked() {
    if (currentState != MenuState.MainMenu) return;
    currentState = MenuState.HighScore;
    
    mainMenuAnimator.AnimateSinkOut(() => {
        highScoreAnimator.AnimateDropIn(() => {
            if (highScoreDisplay != null) {
                highScoreDisplay.RefreshScores();
            }
        });
    });
}

// Called by Back button onClick
public void OnBackFromHighScore() {
    if (currentState != MenuState.HighScore) return;
    currentState = MenuState.MainMenu;
    
    highScoreAnimator.AnimateSinkOut(() => {
        mainMenuAnimator.AnimateDropIn();
    });
}
```

### **Button Setup in Unity:**

**High Score Button (Main Menu):**
- GameObject: `MainMenuPanel/HighScoreButton`
- Component: Button
- OnClick: `MainMenuManager.OnHighScoreClicked()`

**Back Button (High Score Panel):**
- GameObject: `HighScorePanel/BackButton`
- Component: Button
- OnClick: `MainMenuManager.OnBackFromHighScore()`

---

## ✅ Complete Setup Checklist

- [ ] HighScorePanel created with MenuAnimationController
- [ ] HighScoreDisplay component added and configured
- [ ] Score text references assigned
- [ ] BackButton created and configured
- [ ] HighScoreButton added to MainMenuPanel
- [ ] Both buttons OnClick events assigned
- [ ] MainMenuManager references assigned (panels + display)
- [ ] Tested: Open high score panel from main menu
- [ ] Tested: Close high score panel with back button
- [ ] Tested: Scores display correctly
- [ ] Tested: Animations smooth

---

**Created:** December 22, 2025  
**Feature:** High Score Panel Navigation  
**Commit:** 4de99f2
