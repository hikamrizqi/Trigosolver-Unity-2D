# Game Over Animation Feature - Documentation

## 🎯 Overview
Fitur tambahan untuk menampilkan animasi karakter MARAH saat game over (nyawa habis). Berbeda dengan animasi correct/wrong yang auto-hide, animasi game over akan **tetap tampil** di layar hingga pemain menekan tombol kembali atau setelah delay tertentu.

---

## 🔄 Flow Game Over Animation

```
Lives = 0 → Game Over Triggered
    ↓
Karakter MARAH muncul dari bawah (0.8s)
    ↓
Bubble chat "Yah, nyawa habis!" (random)
    ↓
Animasi sprite loop TERUS MENERUS ♾️
    ↓
Game Over Panel muncul (setelah 1.2s delay)
    ↓
[KARAKTER TETAP TAMPIL DI BELAKANG PANEL]
    ↓
Tunggu delay (3s) ATAU Pemain klik tombol kembali
    ↓
Karakter turun ke bawah (0.8s)
    ↓
Kembali ke Level Selection
```

---

## 📝 Perubahan Code

### 1. CharacterAnimationController.cs

**Properties Baru:**
```csharp
[SerializeField] private Sprite[] gameOverAnimationSprites; // 5 sprites marah
[SerializeField] private string[] gameOverMessages; // 5 messages game over
```

**Method Baru:**
```csharp
// Play animasi game over (tidak auto-hide)
public void PlayGameOverAnimation(System.Action onComplete = null)

// Sembunyikan karakter (dipanggil dari tombol kembali)
public void HideCharacter()

// Coroutine khusus untuk game over
private IEnumerator AnimateGameOver(System.Action onComplete)

// Coroutine untuk hide dengan animasi
private IEnumerator HideCharacterCoroutine()
```

**Perbedaan dengan AnimateCharacter:**
- ❌ **TIDAK** auto-hide setelah displayDuration
- ✅ Loop sprite animation **terus menerus** hingga HideCharacter() dipanggil
- ✅ Bubble chat tetap tampil hingga hide
- ✅ isAnimating tetap true hingga disembunyikan

### 2. CalculationManager.cs

**Method Dimodifikasi:**
```csharp
void HandleWrongAnswer(string customMessage = "")
{
    lives--;
    uiManager.UpdateLives(lives);

    if (lives <= 0)
    {
        // Trigger character game over animation
        if (characterAnimController != null)
        {
            characterAnimController.PlayGameOverAnimation(() => 
            {
                Debug.Log("Game Over animation started");
            });
        }

        // Show game over panel setelah delay
        StartCoroutine(ShowGameOverPanelAfterDelay());
    }
    // ... rest of code
}
```

**Coroutine Baru:**
```csharp
IEnumerator ShowGameOverPanelAfterDelay()
{
    // Tunggu character muncul dulu (1.2s)
    yield return new WaitForSeconds(1.2f);

    // Show game over panel
    if (gameOverPanel != null)
    {
        // Pass character controller reference
        gameOverPanel.ShowGameOver(score, characterAnimController);
    }

    // Reset score
    if (scoreDisplayManager != null)
    {
        scoreDisplayManager.ResetScore();
    }
}
```

### 3. GameOverPanel.cs

**Property Baru:**
```csharp
private CharacterAnimationController characterController;
```

**Method Signature Changed:**
```csharp
// BEFORE
public void ShowGameOver(int finalScore)

// AFTER
public void ShowGameOver(int finalScore, CharacterAnimationController charController = null)
```

**Logic Update:**
```csharp
private IEnumerator ReturnToLevelSelectionAfterDelay()
{
    yield return new WaitForSeconds(displayDuration);

    // Hide character animation first if exists
    if (characterController != null && characterController.IsAnimating())
    {
        characterController.HideCharacter();
        // Wait for hide animation to complete
        yield return new WaitForSeconds(1.0f);
    }

    // Hide panel and return
    panel.SetActive(false);
    levelSelectionManager.ShowLevelSelection();
}
```

**Method Baru untuk Tombol Manual:**
```csharp
public void OnBackButtonClicked()
{
    StopAllCoroutines();
    StartCoroutine(HideCharacterAndReturn());
}
```

---

## 🎨 Sprite Requirements

### Game Over Sprites (5 frame animation):
**Karakter MARAH/Frustasi:**
- Frame 1: Wajah kesal, tangan di pinggang
- Frame 2: Menghentak kaki
- Frame 3: Kepala geleng-geleng
- Frame 4: Tangan diangkat frustasi
- Frame 5: Pose kesal kembali

**Style:**
- Ekspresi marah tapi tidak terlalu agresif (masih cute/friendly)
- Bisa dengan simbol "steam" di kepala atau tanda seru
- Warna bisa lebih gelap/saturated untuk emphasize frustasi

**Ukuran:**
- 256×256 atau 512×512 pixels
- PNG dengan background transparan
- Import as Sprite (2D and UI)

---

## 🎭 Message Examples

Default messages yang sudah disediakan:
```csharp
private string[] gameOverMessages = {
    "Yah, nyawa habis!",
    "Waduh! Game Over.",
    "Semangat! Coba lagi ya!",
    "Jangan menyerah!",
    "Next time pasti lebih baik!"
};
```

**Tips customization:**
- Gunakan bahasa yang supportive meski game over
- Hindari pesan yang terlalu harsh atau demotivating
- Bisa tambahkan humor untuk lighten the mood
- Sesuaikan dengan tone game (educational = encouraging)

---

## ⚙️ Inspector Setup

### CharacterAnimationController
```yaml
Animation Sprites:
  ✓ Correct Animation Sprites (Size: 5) - existing
  ✓ Wrong Animation Sprites (Size: 5) - existing
  ⭐ Game Over Animation Sprites (Size: 5) - NEW!
     Element 0: sprite_gameover_1
     Element 1: sprite_gameover_2
     Element 2: sprite_gameover_3
     Element 3: sprite_gameover_4
     Element 4: sprite_gameover_5

Random Messages:
  ✓ Correct Messages (Size: 5) - existing
  ✓ Wrong Messages (Size: 5) - existing
  ⭐ Game Over Messages (Size: 5) - NEW!
     Element 0: "Yah, nyawa habis!"
     Element 1: "Waduh! Game Over."
     Element 2: "Semangat! Coba lagi ya!"
     Element 3: "Jangan menyerah!"
     Element 4: "Next time pasti lebih baik!"
```

### GameOverPanel (Jika ada tombol manual kembali)
```yaml
Button "Kembali" onClick():
  → GameOverPanel.OnBackButtonClicked()
```

---

## 🕐 Timing Breakdown

**Skenario Game Over:**
```
[0.0s] Lives = 0, trigger game over
[0.0s] Character starts moving up
[0.8s] Character reaches center
[0.8s] Bubble chat appears
[0.8s] Sprite animation starts looping
[1.0s] Game over panel starts showing (after 1.2s delay from trigger)
[2.0s] Panel fully visible
[2.0s - 5.0s] Character loops animation, panel shows score
[5.0s] Auto-return triggered OR user clicks back button
[5.0s] Character starts moving down
[5.8s] Character hidden
[5.8s] Return to level selection

Total duration: ~5.8 seconds (if auto-return)
```

**User can skip faster:**
- Jika ada tombol kembali manual, pemain bisa skip sebelum 5 detik
- Character hide animation tetap berjalan smooth (tidak instant)

---

## 🔍 Technical Details

### State Management
```csharp
// Character Animation State
isAnimating = true  // Set saat PlayGameOverAnimation() dipanggil
                    // Tetap true hingga HideCharacter() selesai

// Sprite Loop
while (isAnimating)  // Loop forever sampai external stop
{
    currentFrame = (currentFrame + 1) % animSprites.Length;
    characterImage.sprite = animSprites[currentFrame];
    yield return new WaitForSeconds(spriteAnimationSpeed);
}
```

### Callback Flow
```
CalculationManager
    ↓
characterAnimController.PlayGameOverAnimation(callback)
    ↓
[Callback fires after character moves up and bubble shows]
    ↓
ShowGameOverPanelAfterDelay()
    ↓
gameOverPanel.ShowGameOver(score, characterController)
    ↓
[After delay or button click]
    ↓
characterController.HideCharacter()
    ↓
[Character hides with animation]
    ↓
levelSelectionManager.ShowLevelSelection()
```

---

## 🎮 User Experience

### Before (Without Game Over Character):
```
Lives = 0 → Panel muncul langsung → Score shown → Auto return
(Duration: ~3 seconds, cukup abrupt)
```

### After (With Game Over Character):
```
Lives = 0 → Character marah muncul → Panel muncul → 
Character tetap tampil → Delay/Click → Character turun → Return
(Duration: ~5.8 seconds, more engaging and expressive)
```

**Benefits:**
- ✅ Visual feedback lebih jelas dan engaging
- ✅ Pemain punya waktu "process" kekalahan
- ✅ Character expression membantu emotional connection
- ✅ Transition lebih smooth dan tidak tiba-tiba
- ✅ Memberikan moment untuk "breathe" sebelum retry

---

## 🐛 Troubleshooting

### Character tidak muncul saat game over:
- ✅ Cek apakah `gameOverAnimationSprites` sudah di-assign (5 sprites)
- ✅ Cek `characterAnimController` reference di CalculationManager
- ✅ Cek console untuk error log

### Character muncul tapi langsung hilang:
- ✅ Pastikan menggunakan `PlayGameOverAnimation()` bukan `PlayWrongAnimation()`
- ✅ Cek apakah `isAnimating` tetap true (seharusnya loop forever)

### Character tidak hide saat tombol kembali:
- ✅ Cek apakah `OnBackButtonClicked()` terhubung ke button onClick
- ✅ Cek apakah `characterController` reference di-pass ke GameOverPanel
- ✅ Cek console untuk log "Hiding character..."

### Sprite animation tidak loop:
- ✅ Cek apakah semua 5 sprite tidak null
- ✅ Cek `spriteAnimationSpeed` tidak terlalu cepat (default: 0.15)
- ✅ Pastikan while loop condition `isAnimating` tetap true

---

## 📊 Comparison Table

| Feature | Correct/Wrong Animation | Game Over Animation |
|---------|------------------------|---------------------|
| Trigger | Setiap jawab soal | Lives = 0 |
| Sprite Set | Different per type | Angry/Frustrated |
| Auto Hide | ✅ Yes (after 2.5s) | ❌ No (manual/delay) |
| Loop Duration | Limited (2.5s) | ♾️ Infinite until hide |
| User Control | ❌ No | ✅ Yes (back button) |
| Callback Timing | After full animation | After move up only |
| Purpose | Quick feedback | Emotional closure |

---

## 🚀 Future Enhancements (Optional)

1. **Multiple Game Over Characters**: Random character setiap game over
2. **Sound Effects**: Angry sound atau "aww" sound
3. **Particle Effects**: Steam dari kepala karakter
4. **Shake Animation**: Character shake sedikit saat marah
5. **Different Messages per Score**: Message berbeda tergantung score achieved
6. **Retry Button**: Quick retry without full transition

---

## 📦 File Structure Update

```
Assets/
├── Scripts/
│   ├── UI/Chapter1/
│   │   ├── CharacterAnimationController.cs (MODIFIED)
│   │   └── GameOverPanel.cs (MODIFIED)
│   └── Managers/Chapter1/
│       └── CalculationManager.cs (MODIFIED)
│
└── Sprite/Object/Character/
    ├── Correct/ (5 sprites)
    ├── Wrong/ (5 sprites)
    └── GameOver/ ⭐ NEW! (5 sprites)
        ├── character_gameover_1.png
        ├── character_gameover_2.png
        ├── character_gameover_3.png
        ├── character_gameover_4.png
        └── character_gameover_5.png
```

---

## ✅ Summary

**What Changed:**
- ✅ Added game over sprite array (5 sprites)
- ✅ Added game over messages array (5 messages)
- ✅ New method `PlayGameOverAnimation()` tanpa auto-hide
- ✅ New method `HideCharacter()` untuk manual hide
- ✅ Modified `CalculationManager` untuk trigger saat lives = 0
- ✅ Modified `GameOverPanel` untuk integrate dengan character

**What to Prepare:**
- 🎨 5 sprite untuk karakter marah/frustasi
- 🎮 (Optional) Setup tombol kembali manual di Game Over Panel
- ✅ Test flow hingga game over

**Total Additional Time:**
- Setup: ~5 menit (assign sprites & messages)
- Asset Creation: Tergantung designer (~30-60 menit untuk 5 sprites)
- Testing: ~5 menit

---

**Feature Status:** ✅ Ready to Use  
**Version:** 1.1  
**Last Updated:** 2026-01-28
