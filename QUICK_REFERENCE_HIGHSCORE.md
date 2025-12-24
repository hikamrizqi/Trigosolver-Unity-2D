# High Score Panel - Quick Setup Reference

## 🎯 Quick Setup Steps

### 1. Create Panel
```
HighScorePanel (GameObject)
├── Add: MenuAnimationController
├── Add: HighScoreDisplay
└── Children:
    ├── ScoreTexts (assign to HighScoreDisplay)
    └── BackButton (onClick → OnBackFromHighScore)
```

### 2. Add Button to Main Menu
```
MainMenuPanel/HighScoreButton
└── onClick → MainMenuManager.OnHighScoreClicked()
```

### 3. Assign References
```
MainMenuManager Inspector:
├── High Score Panel → HighScorePanel GameObject
└── High Score Display → HighScorePanel (HighScoreDisplay component)
```

---

## 📋 Implementation Checklist

- [ ] HighScorePanel created with MenuAnimationController
- [ ] HighScoreDisplay component added
- [ ] Score texts assigned (Level1, Level2, Total)
- [ ] BackButton created with onClick event
- [ ] HighScoreButton added to MainMenuPanel
- [ ] HighScoreButton onClick assigned
- [ ] MainMenuManager panels assigned
- [ ] Tested: Click High Score → Panel opens
- [ ] Tested: Click Back → Return to Main Menu
- [ ] Tested: Scores display correctly

---

## 🎬 Navigation Flow

```
Main Menu
    ↓ [High Score Button]
High Score Panel (scores refresh)
    ↓ [Back Button]
Main Menu
```

---

## 🔧 Key Methods

**MainMenuManager.cs:**
- `OnHighScoreClicked()` - Open high score panel
- `OnBackFromHighScore()` - Close high score panel

**Button Events:**
- HighScoreButton → `OnHighScoreClicked()`
- BackButton → `OnBackFromHighScore()`

---

## 🚨 Common Issues

**Button tidak berfungsi?**
→ Check onClick event assigned

**Panel tidak animate?**
→ Check MenuAnimationController attached

**Scores tidak muncul?**
→ Check HighScoreDisplay references

---

## 📖 Full Documentation

- **Setup Guide:** [HIGH_SCORE_PANEL_SETUP.md](HIGH_SCORE_PANEL_SETUP.md)
- **Flow Diagram:** [HighScorePanelFlow.txt](Assets/Scripts/Main%20Menu/HighScorePanelFlow.txt)
- **Score System:** [HIGH_SCORE_SETUP.md](HIGH_SCORE_SETUP.md)
