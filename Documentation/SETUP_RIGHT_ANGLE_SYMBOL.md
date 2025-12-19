# 📐 Setup Simbol Sudut Siku-Siku ∟ (Right Angle Symbol)

## 🎯 Overview

Simbol sudut siku-siku `∟` (Unicode U+221F) adalah simbol matematika standar yang ditampilkan di sudut 90° segitiga. Menggunakan **TextMeshPro** dengan Unicode, tidak perlu sprite tambahan - simple dan profesional!

---

## 🛠️ Setup di Unity Editor

### **Step 1: Duplicate Theta Label**

1. **Di Hierarchy, find "Theta Symbol" GameObject** (atau TextMeshPro World Space yang digunakan untuk theta)

2. **Duplicate:**
   - Right-click pada Theta Symbol → Duplicate
   - Rename duplicate menjadi: `RightAngleSymbol`

3. **Atur Properties:**
   - **Text:** `∟` (copy simbol ini atau ketik langsung)
   - **Font Size:** `5` (akan di-override oleh script)
   - **Color:** **Putih** atau sama dengan theta
   - **Alignment:** Center, Middle
   - **Sorting Order:** 8 (akan di-set oleh script)

### **Step 2: Assign ke TriangleVisualizer Component**

1. **Pilih GameObject "Triangle Depan" (atau parent TriangleVisualizer)**

2. **Di Inspector, cari component `TriangleVisualizer`**

3. **Drag RightAngleSymbol ke field:**
   ```
   Triangle Visualizer (Script)
   ├─ Label References
   │  ├─ Theta Label: [...]
   │  └─ Right Angle Label: [DRAG HERE] ← RightAngleSymbol
   ├─ Visual Settings
   │  └─ Right Angle Font Size: 5
   ```

### **Step 3: Konfigurasi Font Size**

Di Inspector, atur:
- **Right Angle Font Size:** `5` (default - pas untuk simbol kecil)
  - Sangat kecil (3-4): Terlalu kecil
  - Pas (5-7): Jelas tanpa mengganggu (recommended)
  - Sedang (8-12): Lebih menonjol

---

## 🎨 Keuntungan Menggunakan Unicode ∟

### **✅ Advantages:**
1. **Sederhana** - Tidak perlu sprite, cukup TextMeshPro
2. **Matematika Standard** - Simbol `∟` adalah notasi resmi untuk right angle
3. **Auto-scaling** - Font size otomatis menyesuaikan
4. **Konsisten** - Gaya sama dengan theta label
5. **No Assets** - Tidak perlu import sprite tambahan

### **Simbol Alternatif:**
- `∟` (U+221F) - **Right Angle** ← RECOMMENDED
- `⊾` (U+22BE) - Right Angle with Arc
- `⦜` (U+299C) - Right Angle Variant with Square
- `⌝` (U+231D) - Top Right Corner (simple L)

**Rekomendasi:** Gunakan `∟` karena paling standar dan jelas.

---

## 🔧 Troubleshooting

### **Problem 1: Simbol Tidak Terlihat**

**Penyebab:**
- Sorting order terlalu rendah
- Z-position salah
- Scale terlalu kecil

**Solusi:**
```csharp
// Di TriangleVisualizer.cs (sudah ada):
rightAngleSymbol.sortingOrder = 5;  // Lebih tinggi dari garis (0-2)
rightAngleSymbol.transform.position = new Vector3(x, y, -1f); // Z negatif
```

---

### **Problem 2: Simbol Tidak Mengikuti Rotasi**

**Penyebab:**
- Rotasi tidak dikalkulasi dengan benar

**Solusi:**
```csharp
// Script otomatis menghitung rotasi:
float angleToRight = Mathf.Atan2(toRight.y, toRight.x) * Mathf.Rad2Deg;
rightAngleSymbol.transform.rotation = Quaternion.Euler(0, 0, angleToRight);
```

---

### **Problem 3: Simbol Terlalu Besar/Kecil**

**Solusi:**
- Atur `Right Angle Size` di Inspector
- Nilai default: **0.6**
- Test dengan nilai berbeda untuk segitiga besar/kecil

---

## 📊 Positioning Logic

### **Posisi Simbol:**
- Selalu di **bottomLeft** (sudut B - sudut siku-siku)
- Offset sedikit ke dalam segitiga: `(toRight + toUp).normalized * symbolScale * 0.3`

### **Rotasi Simbol:**
- Mengikuti arah sisi **Depan BC** (horizontal di rotasi 0°)
- Rumus: `Atan2(toRight.y, toRight.x)` dalam derajat

### **Z-Position:**
- **Garis Segitiga:** Z = 0
- **Right Angle Symbol:** Z = -1 (di depan garis)
- **Theta Label:** Z = -2 (paling depan)

---

## 🎮 Visual Result

### **Rotasi 0° (Standard):**
```
   θ|\
 S  | \ M
 A  | A \
 M  | C  \
    |┌───\
    B  D (BC)
     ↑
  Simbol siku
  di sudut B
```

### **Rotasi 90°:**
```
   θ___
    \┐ |
  M  \| | S
      B
       D
```

### **Rotasi 180°:**
```
       /|
    M / | S
     /┘ |
    B___D
   θ
```

### **Rotasi 270°:**
```
    S
    |┐
    |└\ D
    B  \θ
     M
```

**Catatan:** Simbol `┌` atau `└` atau `┐` atau `┘` menyesuaikan dengan rotasi!

---

## ✅ Checklist Setup

Sebelum test, pastikan:
- [ ] RightAngleSymbol GameObject sudah dibuat
- [ ] Sprite assigned (kotak atau custom sprite)
- [ ] Warna kontras dengan background (putih/kuning/hijau)
- [ ] Assigned ke field `Right Angle Symbol` di TriangleVisualizer
- [ ] Right Angle Size diatur (default: 0.6)
- [ ] Sorting order = 5 (di Inspector atau script)
- [ ] Test di Play mode dengan berbagai rotasi

---

## 🎨 Alternatif Desain (Advanced)

### **Animated Right Angle:**
```csharp
// Tambahkan di TriangleVisualizer.cs (optional)
void AnimateRightAngle()
{
    // Pulse effect
    float pulse = Mathf.PingPong(Time.time * 2f, 0.2f);
    rightAngleSymbol.transform.localScale = 
        new Vector3(baseSize + pulse, baseSize + pulse, 1f);
}
```

### **Color-coded Right Angle:**
```csharp
// Warna berbeda untuk setiap difficulty
if (difficulty == DifficultyLevel.Easy)
    rightAngleSymbol.color = Color.white;
else if (difficulty == DifficultyLevel.Medium)
    rightAngleSymbol.color = Color.yellow;
else
    rightAngleSymbol.color = Color.red;
```

---

## 📝 Notes

- Simbol siku **hanya muncul di sudut 90°** (bottomLeft di rotasi 0°)
- Tidak ada simbol di sudut theta (karena theta adalah sudut lancip)
- Ukuran simbol **auto-scale** mengikuti dynamicScale segitiga
- Simbol **follow rotation** mengikuti orientasi segitiga

---

**🎓 Dengan simbol siku-siku, pemain lebih mudah mengidentifikasi struktur segitiga!**
