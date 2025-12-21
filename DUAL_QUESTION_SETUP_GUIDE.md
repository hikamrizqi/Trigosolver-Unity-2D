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

⚠️ **PENTING:** Buat **2 CONTAINER TERPISAH** karena layout berbeda!

#### **Struktur Hierarchy yang Benar:**

```
AnswerTileSystem/
├── AnswerSlots_Single/          ⭐ Container untuk Soal 1-10
│   ├── SingleSlot1              (Numerator - posisi KIRI)
│   ├── SlashText_Single         (/) 
│   └── SingleSlot2              (Denominator - posisi KANAN)
│
├── AnswerSlots_Dual/            ⭐ Container untuk Soal 11-20
│   ├── Fraction1/               (Pecahan pertama)
│   │   ├── DualSlot1            (Numerator - posisi ATAS)
│   │   ├── SlashText_Dual1      (/)
│   │   └── DualSlot2            (Denominator - posisi BAWAH)
│   └── Fraction2/               (Pecahan kedua)
│       ├── DualSlot3            (Numerator - posisi ATAS)
│       ├── SlashText_Dual2      (/)
│       └── DualSlot4            (Denominator - posisi BAWAH)
│
└── TilePool/
```

#### **Layout Comparison:**

**Soal 1-10 (HORIZONTAL):**
```
AnswerSlots_Single:  [15] / [17]
                      ↑       ↑
                   Slot1   Slot2
                   (berjejer kanan-kiri)
```

**Soal 11-20 (2x2 GRID):**
```
AnswerSlots_Dual:
   Fraction1     Fraction2
    [15]           [8]         ← DualSlot1, DualSlot3 (numerators)
    ----           ---
    [17]           [17]        ← DualSlot2, DualSlot4 (denominators)
```

#### **Cara Membuat:**

**Step 1: Buat Container untuk Single Question (Soal 1-10)**
1. Buat Empty GameObject: `AnswerSlots_Single`
2. Add Component: **Horizontal Layout Group**
   - Child Alignment: Middle Center
   - Spacing: 20
   - Child Force Expand: Width ✅, Height ✅
3. Pindahkan existing Slot1, SlashText, Slot2 ke dalam container ini
4. Rename menjadi: `SingleSlot1`, `SlashText_Single`, `SingleSlot2`

**Step 2: Buat Container untuk Dual Question (Soal 11-20)**
1. Buat Empty GameObject: `AnswerSlots_Dual`
2. Add Component: **Horizontal Layout Group** (untuk 2 pecahan side-by-side)
   - Child Alignment: Middle Center
   - Spacing: 40
3. Di dalam `AnswerSlots_Dual`, buat 2 sub-container:
   
   **Sub-Container: Fraction1**
   - Add Component: **Vertical Layout Group**
   - Child Alignment: Middle Center
   - Spacing: 5
   - Isi dengan: `DualSlot1` (atas), `SlashText_Dual1` (/), `DualSlot2` (bawah)
   
   **Sub-Container: Fraction2**
   - Add Component: **Vertical Layout Group**
   - Child Alignment: Middle Center
   - Spacing: 5
   - Isi dengan: `DualSlot3` (atas), `SlashText_Dual2` (/), `DualSlot4` (bawah)

**Step 3: Script akan Auto Show/Hide Container**
- Soal 1-10: `AnswerSlots_Single` ACTIVE, `AnswerSlots_Dual` INACTIVE
- Soal 11-20: `AnswerSlots_Single` INACTIVE, `AnswerSlots_Dual` ACTIVE

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

Tambahkan reference untuk **2 CONTAINER** dan **SEMUA SLOTS**:

#### **Single Question (Soal 1-10):**
| Field Name | Type | Assign To |
|------------|------|-----------|
| `Single Question Slot Container` | GameObject | `AnswerSlots_Single` |
| `Single Slot1 Transform` | Transform | `AnswerSlots_Single/SingleSlot1` |
| `Single Slot2 Transform` | Transform | `AnswerSlots_Single/SingleSlot2` |
| `Single Slash Text` | TextMeshProUGUI | `AnswerSlots_Single/SlashText_Single` |

#### **Dual Question (Soal 11-20):**
| Field Name | Type | Assign To |
|------------|------|-----------|
| `Dual Question Slot Container` | GameObject | `AnswerSlots_Dual` |
| `Dual Slot1 Transform` | Transform | `AnswerSlots_Dual/Fraction1/DualSlot1` |
| `Dual Slot2 Transform` | Transform | `AnswerSlots_Dual/Fraction1/DualSlot2` |
| `Dual Slot3 Transform` | Transform | `AnswerSlots_Dual/Fraction2/DualSlot3` |
| `Dual Slot4 Transform` | Transform | `AnswerSlots_Dual/Fraction2/DualSlot4` |
| `Dual Slash Text1` | TextMeshProUGUI | `AnswerSlots_Dual/Fraction1/SlashText_Dual1` |
| `Dual Slash Text2` | TextMeshProUGUI | `AnswerSlots_Dual/Fraction2/SlashText_Dual2` |

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
- [ ] **2 slots HORIZONTAL** (berjejer kiri-kanan: `[15] / [17]`)
- [ ] Container `AnswerSlots_Single` **VISIBLE**
- [ ] Container `AnswerSlots_Dual` **HIDDEN**
- [ ] Verifikasi: 2 angka benar = lanjut

### **Test Soal 11-20 (Sistem Baru):**
- [ ] Simbol **A** dan **B** muncul di 2 sudut lancip
- [ ] Simbol **θ** HIDDEN
- [ ] Label **AB, BC, AC** muncul (BUKAN Depan/Samping/Miring)
- [ ] Interactive buttons: **AB, BC, AC** (setelah user ganti image)
- [ ] **4 slots dalam 2x2 GRID** (2 pecahan vertikal side-by-side)
- [ ] Container `AnswerSlots_Single` **HIDDEN**
- [ ] Container `AnswerSlots_Dual` **VISIBLE**
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
- Pastikan `dualQuestionSlotContainer` sudah ter-assign di Inspector `AnswerTileSystem`
- Pastikan semua slot di `AnswerSlots_Dual/Fraction1` dan `Fraction2` sudah ter-assign
- Check Console log untuk error "dualSlot1Transform is null"
- **Pastikan Layout Group** di Fraction1 dan Fraction2 sudah di-set (Vertical Layout Group)

### **Problem:** Layout slots terlihat sama untuk soal 1-10 dan 11-20
**Solution:**
- Pastikan menggunakan **2 CONTAINER TERPISAH** (`AnswerSlots_Single` dan `AnswerSlots_Dual`)
- `AnswerSlots_Single` harus punya **Horizontal Layout Group** (slots berjejer kiri-kanan)
- `AnswerSlots_Dual` harus punya **Horizontal Layout Group** dengan 2 sub-container `Fraction1` dan `Fraction2`
- Masing-masing `Fraction1` dan `Fraction2` harus punya **Vertical Layout Group**
- Script akan auto show/hide container berdasarkan tipe soal

### **Problem:** Simbol A dan B tidak muncul
**Solution:**
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
