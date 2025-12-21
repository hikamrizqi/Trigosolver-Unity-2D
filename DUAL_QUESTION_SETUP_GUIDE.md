# Dual Question System (Soal 11-20) - Setup Guide

## 📋 Overview

Sistem Dual Question menambahkan mekanik baru untuk soal nomor 11-20 di Chapter 1, dimana pemain harus menjawab **2 pertanyaan trigonometri sekaligus** dengan **4 angka** (2 pecahan).

### Perbedaan dengan Soal 1-10:

| Aspek | Soal 1-10 (Single) | Soal 11-20 (Dual) |
|-------|-------------------|-------------------|
| **Sudut** | θ (theta) | A dan B |
| **Label Sisi** | Depan, Samping, Miring | AB, BC, AC |
| **Pertanyaan** | 1 (contoh: sin θ) | 2 (contoh: sin A dan cos B) |
| **Answer Slots** | 2 (numerator/denominator) | 4 (num1/den1, num2/den2) |
| **Button Panel** | DEPAN, SAMPING, MIRING | AB, BC, AC |
| **Sudut Siku** | ∟ (selalu ada) | ∟ (selalu ada) |

---

## 🛠️ Perubahan di Unity Hierarchy

### 1. **TriangleVisualizer GameObject**

Tambahkan 2 TextMeshPro object baru untuk sudut A dan B:

```
TriangleVisualizer/
├── Lines/
│   ├── DepanLine
│   ├── SampingLine
│   └── MiringLine
├── Labels/
│   ├── DepanLabel
│   ├── SampingLabel
│   ├── MiringLabel
│   ├── ThetaLabel          (soal 1-10)
│   ├── RightAngleLabel     (semua soal)
│   ├── AngleLabelA         ⭐ BARU - untuk soal 11-20
│   └── AngleLabelB         ⭐ BARU - untuk soal 11-20
```

**Cara Membuat:**
1. Duplicate `ThetaLabel` GameObject
2. Rename menjadi `AngleLabelA`
3. Duplicate lagi menjadi `AngleLabelB`
4. Inspector settings (sama seperti ThetaLabel):
   - **Font Size:** 18-20
   - **Alignment:** Center/Middle
   - **Color:** White/Yellow
   - **Sorting Layer:** Default
   - **Order in Layer:** 10

### 2. **AnswerTileSystem GameObject**

Tambahkan 2 slot baru (Slot3 dan Slot4) untuk jawaban kedua:

```
AnswerTileSystem/
├── AnswerSlots/
│   ├── Slot1               (Numerator 1)
│   ├── SlashText           (/)
│   ├── Slot2               (Denominator 1)
│   ├── Slot3               ⭐ BARU - Numerator 2
│   ├── SlashText2          ⭐ BARU - (/)
│   └── Slot4               ⭐ BARU - Denominator 2
└── TilePool/
```

**Layout Slot (2x2 Grid) untuk Soal 11-20:**
```
[Slot1]  [Slot3]    ← Numerators (angka atas)
-------  -------
[Slot2]  [Slot4]    ← Denominators (angka bawah)
```

**Cara Membuat:**
1. Duplicate `Slot1` → Rename menjadi `Slot3`
2. Duplicate `Slot2` → Rename menjadi `Slot4`
3. Duplicate `SlashText` → Rename menjadi `SlashText2`
4. Atur posisi dengan Grid Layout Group atau Manual:
   - **Slot1:** Top-Left
   - **Slot2:** Bottom-Left
   - **Slot3:** Top-Right
   - **Slot4:** Bottom-Right
   - **SlashText:** Between Slot1 and Slot2
   - **SlashText2:** Between Slot3 and Slot4

5. **PENTING:** Slot3, Slot4, dan SlashText2 akan **auto-hide** untuk soal 1-10, dan **auto-show** untuk soal 11-20.

### 3. **InteractiveButtonPanel GameObject**

**❗ PERHATIAN:** Button images/text harus **CONDITIONAL** berdasarkan nomor soal:

- **Soal 1-10:** Tampilkan `DEPAN`, `SAMPING`, `MIRING`
- **Soal 11-20:** Tampilkan `AB`, `BC`, `AC`

**TODO untuk User:**
```
InteractiveButtonPanel/
├── ButtonDepan_Samping    → Ganti image/text menjadi "AB" untuk soal 11-20
├── ButtonSamping_Samping  → Ganti image/text menjadi "BC" untuk soal 11-20
└── ButtonMiring_Samping   → Ganti image/text menjadi "AC" untuk soal 11-20
```

> **Catatan:** Anda perlu menyediakan asset image berbeda untuk button AB, BC, AC. Script sudah siap dengan placeholder untuk conditional UI, tinggal assign sprite/text baru di Inspector.

---

## ⚙️ Perubahan di Unity Inspector

### **TriangleVisualizer Component**

Tambahkan 2 reference baru:

| Field Name | Type | Assign To |
|------------|------|-----------|
| `Angle Label A` | TextMeshPro | `TriangleVisualizer/Labels/AngleLabelA` |
| `Angle Label B` | TextMeshPro | `TriangleVisualizer/Labels/AngleLabelB` |

### **AnswerTileSystem Component**

Tambahkan 3 reference baru:

| Field Name | Type | Assign To |
|------------|------|-----------|
| `Slot3 Transform` | Transform | `AnswerSlots/Slot3` |
| `Slot4 Transform` | Transform | `AnswerSlots/Slot4` |
| `Slash Text2` | TextMeshProUGUI | `AnswerSlots/SlashText2` |

### **UIManagerChapter1 Component**

Tidak ada perubahan field Inspector, tapi pastikan:
- `Answer Tile System` tetap ter-assign
- `Triangle Visualizer` tetap ter-assign

---

## 🎮 Cara Kerja Sistem

### **Flow Soal 1-10 (SINGLE QUESTION):**
1. Generator membuat 1 pertanyaan (contoh: "sin θ")
2. Triangle menampilkan simbol **θ** di sudut lancip
3. Sistem spawn **6 tiles** (2 correct + 4 distractors)
4. Player mengisi **2 slots** (numerator/denominator)
5. Verification check: **2 nilai** harus benar

### **Flow Soal 11-20 (DUAL QUESTION):**
1. Generator membuat 2 pertanyaan (contoh: "sin A dan cos B")
2. Triangle menampilkan simbol **A** dan **B** di 2 sudut lancip
3. Label berubah dari Depan/Samping/Miring ke **AB/BC/AC**
4. Sistem spawn **6 tiles** (4 correct + 2 distractors)
5. Player mengisi **4 slots** (num1/den1, num2/den2)
6. Verification check: **SEMUA 4 nilai** harus benar untuk lanjut

### **Fill Order (Soal 11-20):**
```
Klik Tile #1 → Slot1 (num1)
Klik Tile #2 → Slot2 (den1)
Klik Tile #3 → Slot3 (num2)
Klik Tile #4 → Slot4 (den2)
```

---

## 🧪 Testing Checklist

### **Test Soal 1-10 (Harus TIDAK BERUBAH):**
- [ ] Simbol **θ** muncul di sudut lancip
- [ ] Label **Depan, Samping, Miring** muncul
- [ ] Interactive buttons: **DEPAN, SAMPING, MIRING**
- [ ] **2 slots** muncul untuk answer tiles
- [ ] Slot3, Slot4, SlashText2 **HIDDEN**
- [ ] Verifikasi: 2 angka benar = lanjut

### **Test Soal 11-20 (Sistem Baru):**
- [ ] Simbol **A** dan **B** muncul di 2 sudut lancip
- [ ] Simbol **θ** HIDDEN
- [ ] Label **AB, BC, AC** muncul (BUKAN Depan/Samping/Miring)
- [ ] Interactive buttons: **AB, BC, AC** (setelah user ganti image)
- [ ] **4 slots** muncul (layout 2x2)
- [ ] Slot3, Slot4, SlashText2 **VISIBLE**
- [ ] 6 tiles spawn (4 correct + 2 distractors)
- [ ] Verifikasi: SEMUA 4 angka harus benar untuk lanjut

### **Test Transisi (Soal 10 → 11):**
- [ ] Soal 10: Menampilkan sistem lama (θ, 2 slots)
- [ ] **Animasi exit** berjalan normal
- [ ] Soal 11: Menampilkan sistem baru (A & B, 4 slots)
- [ ] **Animasi entry** berjalan normal untuk A dan B symbols
- [ ] Tidak ada glitch/flash antar transition

---

## 🚨 Troubleshooting

### **Problem:** Slot3 dan Slot4 tidak muncul di soal 11-20
**Solution:** 
- Pastikan `Slot3Transform` dan `Slot4Transform` sudah ter-assign di Inspector `AnswerTileSystem`
- Check Console log untuk error "slot3Transform is null"

### **Problem:** Simbol A dan B tidak muncul
**Solution:**
- Pastikan `AngleLabelA` dan `AngleLabelB` sudah ter-assign di Inspector `TriangleVisualizer`
- Check GameObject `AngleLabelA` dan `AngleLabelB` aktif di Hierarchy

### **Problem:** Simbol θ masih muncul di soal 11-20
**Solution:**
- Check `TriangleDataGenerator` - pastikan `GenerateDualAngleQuestion()` set `IsDualQuestion = true`
- Check log: "Type: DUAL (A & B)" harus muncul untuk soal 11-20

### **Problem:** Button masih menampilkan "DEPAN/SAMPING/MIRING" di soal 11-20
**Solution:**
- Ini **EXPECTED** - user harus menyediakan asset image baru untuk AB/BC/AC
- Script sudah siap, tinggal ganti image/sprite di Inspector

### **Problem:** Jawaban benar tapi dianggap salah di soal 11-20
**Solution:**
- Check Console log: "Dual Answer Check - Answer1: X vs Y, Answer2: X vs Y"
- Pastikan SEMUA 4 slot terisi dengan benar
- Cek urutan fill: Slot1 → Slot2 → Slot3 → Slot4

---

## 📝 Code Changes Summary

### **Modified Files:**
1. `TriangleDataGenerator.cs` - Generate dual question data ✅ COMMITTED
2. `TriangleVisualizer.cs` - Conditional symbol rendering (θ vs A/B) ✅ DONE
3. `AnswerTileSystem.cs` - Support 4 slots dynamically ✅ DONE
4. `UIManagerChapter1.cs` - Pass IsDualQuestion flag ✅ DONE
5. `CalculationManager.cs` - Verify 4 answers for dual questions ✅ DONE

### **Key Logic:**
- **Conditional Rendering:** All systems check `IsDualQuestion` flag
- **Slot Visibility:** Auto-show/hide Slot3, Slot4, SlashText2 based on question type
- **Answer Format:** Single `"num/den"` vs Dual `"num1/den1|num2/den2"`
- **Verification:** Single checks 2 values, Dual checks 4 values (ALL must be correct)

---

## 📚 Reference

### **Trigonometric Ratios:**

**Normal Angle Ratios (A):**
- sin A = depan / miring
- cos A = samping / miring
- tan A = depan / samping

**Complementary Angle Ratios (B):**
- sin B = samping / miring
- cos B = depan / miring
- tan B = samping / depan

### **Triangle Vertices:**
- **Point A:** Top-Left (one acute angle)
- **Point B:** Bottom-Left (right angle - 90°, selalu ∟)
- **Point C:** Bottom-Right (other acute angle)

---

## ✅ Setup Completion

Setelah mengikuti guide ini, konfirmasi:

1. ✅ Hierarchy updated dengan object baru
2. ✅ Inspector references assigned
3. ✅ Tested soal 1-10 (harus TIDAK berubah)
4. ✅ Tested soal 11-20 (sistem baru berjalan)
5. ⏳ Button images untuk AB/BC/AC (waiting for user asset)

---

**Last Updated:** 2024
**System Version:** Unity 6.0 (6000.0.23f1)
**Feature:** Dual Question System for Questions 11-20
