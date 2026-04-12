# ⚡ Quick Setup Guide: Character Animation

## 🎯 Tujuan
Menambahkan animasi karakter yang muncul setiap kali pemain menjawab soal di Chapter 1 Level 1-3.

---

## 📋 Checklist Setup (5 Menit)

### ✅ Step 1: Buat GameObject
```
Hierarchy → Right Click → Create Empty
Nama: "CharacterAnimationSystem"
Parent: Canvas Chapter 1
```

### ✅ Step 2: Buat Child Objects
```
CharacterAnimationSystem
└─ CharacterImage (Add: Image)
   └─ BubbleChatPanel (Add: Image)
      └─ ChatText (Add: TextMeshProUGUI)
```

### ✅ Step 3: Setup Positions
**CharacterImage:**
- Anchor: Center-Bottom
- Position: (0, -800, 0) ← Mulai di bawah layar
- Size: 250 × 250

**BubbleChatPanel:**
- Anchor: Top-Center
- Position: (0, 300, 0) ← Di atas karakter
- Size: 350 × 120

**ChatText:**
- Stretch to fill BubbleChatPanel
- Alignment: Center
- Font Size: 20-24

### ✅ Step 4: Add Script
```
CharacterAnimationSystem → Add Component
Search: "CharacterAnimationController"
```

### ✅ Step 5: Assign Sprites (PENTING!)
Di Inspector `CharacterAnimationController`:

**Correct Animation Sprites (Size: 5):**
- Drag 5 sprite untuk animasi BENAR

**Wrong Animation Sprites (Size: 5):**
- Drag 5 sprite untuk animasi SALAH

**Game Over Animation Sprites (Size: 5):** ⭐ NEW!
- Drag 5 sprite untuk animasi GAME OVER (marah)

### ✅ Step 6: Assign References
Di Inspector `CharacterAnimationController`:
- Character Image → Drag `CharacterImage`
- Character Transform → Drag `CharacterImage`
- Bubble Chat Panel → Drag `BubbleChatPanel`
- Bubble Chat Text → Drag `ChatText`

### ✅ Step 7: Connect to CalculationManager
```
Hierarchy → Find "CalculationManager"
Inspector → Find "Character Anim Controller"
Drag: CharacterAnimationSystem
```

### ✅ Step 8: Test!
```
Play → Pilih Level 1/2/3 → Jawab soal
```

---

## 📁 Lokasi File

### Scripts:
- ✅ `CharacterAnimationController.cs` → Assets/Scripts/UI/Chapter1/
- ✅ `CalculationManager.cs` (modified) → Assets/Scripts/Managers/Chapter1/

### Sprites (Perlu disiapkan):
- ❓ 5 sprite BENAR → Assets/Sprite/Object/Character/Correct/
- ❓ 5 sprite SALAH → Assets/Sprite/Object/Character/Wrong/
- ❓ 5 sprite GAME OVER → Assets/Sprite/Object/Character/GameOver/ ⭐ NEW!

---

## 🎨 Sprite Requirements

### Format:
- PNG dengan background transparan
- Ukuran: 256×256 atau 512×512 pixels
- Import Settings: Sprite (2D and UI)

### Animasi BENAR (5 frame):
- Karakter senang/gembira
- Pose: merayakan, melompat, atau berjalan riang

### Animasi SALAH (5 frame):
- Karakter sedih/bingung
- Pose: berpikir, menggaruk kepala, atau kecewa

### Animasi GAME OVER (5 frame): ⭐ NEW!
- Karakter MARAH/frustasi
- Pose: menghentak kaki, tangan di pinggang, ekspresi kesal

---

## ⚙️ Default Settings (Sudah OK)

```yaml
Move Up Duration: 0.8s
Move Down Duration: 0.8s
Display Duration: 2.5s
Sprite Speed: 0.15s per frame

Hidden Position: (0, -800)
Center Position: (0, 0)
```

**Jangan ubah kecuali perlu customization!**

---

## 📝 Messages (Sudah Include)

### Benar (5 pesan):
1. "Hebat! Jawabanmu benar!"
2. "Luar biasa! Kamu pintar!"
3. "Sempurna! Pertahankan!"
4. "Bagus sekali! Terus seperti itu!"
5. "Mantap! Kamu memahaminya!"

### Salah (5 pesan):
1. "Oops! Coba periksa lagi."
2. "Hmm, belum tepat. Semangat!"
3. "Jangan menyerah! Coba lagi."
4. "Hampir! Periksa perhitunganmu."
5. "Yuk, fokus dan coba lagi!"

### Game Over (5 pesan): ⭐ NEW!
1. "Yah, nyawa habis!"
2. "Waduh! Game Over."
3. "Semangat! Coba lagi ya!"
4. "Jangan menyerah!"
5. "Next time pasti lebih baik!"

---

## 🐛 Troubleshooting

### Karakter tidak muncul?
→ Cek `CalculationManager` → `Character Anim Controller` sudah di-assign?

### Bubble tidak tampil?
→ Cek `BubbleChatPanel` dan `ChatText` sudah di-assign di Inspector?

### Animasi tidak jalan?
→ Cek apakah sudah assign 5 sprite di `Correct/Wrong Animation Sprites`?

### Error DOTween?
→ DOTween harus ter-install di project (biasanya sudah ada)

---

## 🎬 Test Flow

1. **Play scene Chapter 1**
2. **Pilih Level 1, 2, atau 3**
3. **Jawab BENAR:**
   - ✓ Feedback hijau muncul
   - ✓ Karakter naik dari bawah
   - ✓ Animasi sprite berjalan
   - ✓ Bubble chat muncul
   - ✓ Karakter turun
   - ✓ Next question
4. **Jawab SALAH:**
   - ✓ Feedback merah muncul
   - ✓ Karakter berbeda muncul
   - ✓ Bubble chat error
   - ✓ Next question
5. **GAME OVER:** ⭐ NEW!
   - ✓ Jawab salah 3x (lives = 0)
   - ✓ Karakter MARAH muncul
   - ✓ Karakter TETAP tampil (tidak turun)
   - ✓ Panel game over muncul
   - ✓ Tunggu atau klik tombol kembali
   - ✓ Karakter turun baru hilang

---

## ⏱️ Timeline

```
Player Answer
    ↓
Feedback (1.5s)
    ↓
Character Up (0.8s)
    ↓
Display + Animation (2.5s)
    ↓
Character Down (0.8s)
    ↓
Next Question

Total: ~5.6 detik per soal
```

---

## 💾 Save & Backup

Sebelum test, pastikan:
- ✅ Save Scene
- ✅ Save Project
- ✅ Commit ke Git (jika pakai version control)

---

## 🚀 Ready to Go!

Setelah setup selesai:
1. Test di Unity Editor
2. Test di Build (mobile/PC)
3. Pastikan smooth di berbagai device

**Enjoy! 🎉**

---

## 📚 Dokumentasi Lengkap

Untuk detail lebih lanjut, baca:
- `CHARACTER_ANIMATION_SETUP_GUIDE.md` → Setup lengkap
- `CHARACTER_ANIMATION_TECHNICAL_DOC.md` → Dokumentasi teknis
- `CHARACTER_ANIMATION_DIAGRAM.md` → Visual flow diagram

---

**Created:** 2026-01-28  
**Version:** 1.0  
**Compatible:** Unity 2021.3+, Chapter 1 Level 1-3
