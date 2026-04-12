# Dokumentasi Teknis: Sistem Animasi Karakter Chapter 1

## Ringkasan
Implementasi sistem animasi karakter interaktif yang muncul setelah pemain menjawab soal di Chapter 1 (Level 1-3). Sistem ini memberikan feedback visual yang menarik dengan menampilkan karakter animasi dan bubble chat dengan pesan random.

## Flow Animasi

### Jawaban BENAR:
```
Player submit jawaban → Verifikasi benar → Feedback hijau + score
    ↓
Delay 1.5 detik
    ↓
Karakter muncul dari bawah (0.8s dengan ease out back)
    ↓
Animasi sprite loop (5 frame × 0.15s) + Bubble chat random (2.5s total)
    ↓
Karakter turun ke bawah (0.8s dengan ease in back)
    ↓
Triangle & tiles animasi keluar
    ↓
Soal berikutnya
```

### Jawaban SALAH:
```
Player submit jawaban → Verifikasi salah → Feedback merah + lives -1
    ↓
Delay 1.5 detik
    ↓
Karakter muncul dari bawah (sprite berbeda)
    ↓
Animasi sprite loop + Bubble chat error random (2.5s total)
    ↓
Karakter turun ke bawah
    ↓
Triangle & tiles animasi keluar
    ↓
Soal berikutnya (atau Game Over jika lives = 0)
```

## Komponen Code

### 1. CharacterAnimationController.cs

**Properties:**
- `characterImage`: Image component untuk menampilkan sprite
- `characterTransform`: RectTransform untuk animasi posisi
- `correctAnimationSprites[]`: 5 sprite untuk animasi benar
- `wrongAnimationSprites[]`: 5 sprite untuk animasi salah
- `bubbleChatPanel`: Panel untuk bubble chat
- `bubbleChatText`: Text untuk pesan
- `correctMessages[]`: Array pesan random untuk jawaban benar
- `wrongMessages[]`: Array pesan random untuk jawaban salah

**Methods:**
- `PlayCorrectAnimation(Action onComplete)`: Trigger animasi jawaban benar
- `PlayWrongAnimation(Action onComplete)`: Trigger animasi jawaban salah
- `AnimateCharacter(bool isCorrect, Action onComplete)`: Coroutine utama animasi
- `StopAnimation()`: Force stop animasi (emergency)
- `IsAnimating()`: Check status animasi

**Animation Stages:**
1. **Setup**: Pilih sprite set dan pesan sesuai hasil (benar/salah)
2. **Move Up**: DOTween anchored position dari hiddenPosition ke centerPosition
3. **Display**: Loop sprite animation + tampilkan bubble chat
4. **Move Down**: DOTween anchored position kembali ke hiddenPosition
5. **Cleanup**: Disable image, callback onComplete

### 2. CalculationManager.cs (Modified)

**New Field:**
```csharp
[SerializeField] private CharacterAnimationController characterAnimController;
```

**New Method:**
```csharp
IEnumerator NextRoundDelayWithCharacterAnimation(bool isCorrect)
{
    yield return new WaitForSeconds(1.5f); // Feedback display time
    
    // Play character animation (3-4 seconds total)
    if (characterAnimController != null && progres >= 1 && progres <= 30)
    {
        bool characterAnimDone = false;
        
        if (isCorrect)
            characterAnimController.PlayCorrectAnimation(() => characterAnimDone = true);
        else
            characterAnimController.PlayWrongAnimation(() => characterAnimDone = true);
        
        yield return new WaitUntil(() => characterAnimDone);
    }
    
    // Continue with triangle & tiles exit animation
    // ... (existing code)
}
```

**Modified Logic:**
- `VerifyAnswer()`: Memanggil `NextRoundDelayWithCharacterAnimation(true)` untuk jawaban benar
- `HandleWrongAnswer()`: Memanggil `NextRoundDelayWithCharacterAnimation(false)` untuk jawaban salah

## Timing Breakdown

**Total waktu per soal (dengan animasi karakter):**
```
Feedback display:     1.5s
Character move up:    0.8s
Character display:    2.5s (animasi sprite + bubble)
Character move down:  0.8s
-----------------------------------
Total character:      ~5.6s
```

Dibandingkan tanpa animasi karakter:
```
Feedback display:     1.5s
Direct to next:       0s
-----------------------------------
Total:                1.5s
```

**Trade-off:** +4 detik per soal, namun memberikan experience lebih engaging dan feedback visual yang lebih jelas.

## Customization Points

### Dalam Inspector (Tanpa Code):
1. **Durasi animasi**: Move up/down duration, display duration
2. **Posisi**: Hidden position, center position
3. **Sprite**: Ganti sprite set untuk karakter berbeda
4. **Pesan**: Tambah/edit pesan random
5. **Speed**: Sprite animation speed

### Dalam Code (Jika Perlu):
1. **Ease type**: Ubah `Ease.OutBack` / `Ease.InBack` di DOTween
2. **Bubble animation**: Tambah animasi bounce, fade, dll
3. **Character effects**: Tambah particle effect, shadow, dll
4. **Conditional display**: Tampilkan karakter berbeda per level

## Integration dengan Sistem Lain

### Dependencies:
- ✓ CalculationManager: Memanggil animasi setelah verifikasi
- ✓ UIManagerChapter1: Menampilkan feedback sebelum animasi
- ✓ AnswerTileSystem: Highlight answer sebelum animasi
- ✓ TriangleVisualizer: Exit animation setelah character animation

### No Conflict:
- ✓ ScoreDisplayManager: Berjalan parallel dengan animasi
- ✓ LevelSelectionManager: Tidak terpengaruh
- ✓ GameOverPanel: Tidak terpengaruh (game over skip animasi)

## Performance Considerations

**Memory:**
- Sprites loaded: 10 sprites (5 correct + 5 wrong) × ~50-100KB = ~500KB-1MB
- Impact: Minimal (sprites kecil untuk 2D character)

**CPU:**
- DOTween animations: Very optimized
- Coroutine sprite loop: Negligible
- Overall: < 1% CPU usage

**Best Practices:**
- ✓ Gunakan sprite atlas untuk mengurangi draw calls
- ✓ Compress sprites dengan format optimal (ETC2, ASTC)
- ✓ Pastikan sprite resolution tidak terlalu besar (max 512×512)

## Testing Checklist

### Functional:
- [ ] Karakter muncul setelah jawaban benar
- [ ] Karakter muncul setelah jawaban salah
- [ ] Sprite animation loop dengan smooth
- [ ] Bubble chat tampil dengan pesan random
- [ ] Bubble chat berbeda untuk benar/salah
- [ ] Karakter turun sebelum next question
- [ ] Tidak ada blocking atau freeze

### Visual:
- [ ] Ease animation smooth (tidak patah-patah)
- [ ] Bubble chat tidak terpotong
- [ ] Sprite scale dan posisi sesuai
- [ ] Text readable dan tidak overflow

### Edge Cases:
- [ ] Game over tidak trigger animasi karakter
- [ ] Multiple answer cek tidak duplicate animasi
- [ ] Scene transition tidak crash
- [ ] Fast-forward / skip tidak error

## Future Enhancements (Optional)

1. **Multiple Characters**: Karakter berbeda per level
2. **Voice Lines**: Audio random untuk karakter
3. **Particle Effects**: Confetti untuk jawaban benar, sweat untuk salah
4. **Character Emotions**: Lebih banyak variasi ekspresi
5. **Interactive Bubble**: Bubble dapat di-klik untuk skip
6. **Achievement Integration**: Special character untuk milestone tertentu

## File Structure
```
Assets/
├── Scripts/
│   ├── UI/
│   │   └── Chapter1/
│   │       ├── CharacterAnimationController.cs (NEW)
│   │       └── CHARACTER_ANIMATION_SETUP_GUIDE.md (NEW)
│   └── Managers/
│       └── Chapter1/
│           └── CalculationManager.cs (MODIFIED)
│
└── Sprite/
    └── Object/
        └── Character/  (PERLU DIBUAT)
            ├── Correct/
            │   ├── character_correct_1.png
            │   ├── character_correct_2.png
            │   ├── character_correct_3.png
            │   ├── character_correct_4.png
            │   └── character_correct_5.png
            └── Wrong/
                ├── character_wrong_1.png
                ├── character_wrong_2.png
                ├── character_wrong_3.png
                ├── character_wrong_4.png
                └── character_wrong_5.png
```

## Kesimpulan

Sistem animasi karakter ini menambahkan layer interaktif yang membuat gameplay Chapter 1 lebih engaging. Implementasi menggunakan coroutine dan DOTween untuk performa optimal, dengan customization penuh melalui Inspector tanpa perlu edit code.

**Advantages:**
- ✓ User experience lebih engaging
- ✓ Feedback visual yang jelas dan menarik
- ✓ Mudah di-customize tanpa code
- ✓ Performance impact minimal
- ✓ Scalable untuk future enhancements

**Considerations:**
- Menambah ~4 detik per soal
- Perlu aset sprite karakter (10 sprites)
- Perlu testing untuk memastikan timing pas
