# 🔧 Troubleshooting Visualisasi Segitiga

## Masalah: Garis Segitiga Tidak Pas / Posisi Salah

### **Gejala yang Sering Terjadi:**

1. ❌ Garis terlalu pendek/panjang
2. ❌ Garis tidak membentuk segitiga
3. ❌ Posisi garis meleset
4. ❌ Segitiga terlalu besar/kecil
5. ❌ Label overlap dengan garis

---

## ✅ Checklist Perbaikan (Ikuti Urutan Ini!)

### **1. Pastikan Sprite Pivot di CENTER**

**PENTING!** Sprite HARUS memiliki pivot di tengah (0.5, 0.5)

**Cara Cek:**
```
1. Select sprite di Assets folder
2. Inspector > Sprite Editor
3. Klik "Sprite Editor"
4. Lihat Pivot Point
   ✅ HARUS: Center (0.5, 0.5)
   ❌ JANGAN: Bottom Left, Top, dll

5. Jika bukan Center:
   - Set Pivot: Center
   - Click Apply
   - Save
```

**Alternatif - Import Setting:**
```
1. Select sprite di Assets
2. Inspector > Texture Type: Sprite (2D and UI)
3. Sprite Mode: Single
4. Pixels Per Unit: 100
5. Mesh Type: Full Rect
6. Pivot: Center ← PENTING!
7. Generate Physics Shape: Unchecked
8. Apply
```

---

### **2. Adjust Base Scale di Inspector**

Base Scale menentukan ukuran segitiga di layar.

**Panduan:**
```
Scene terlalu penuh UI → Base Scale = 0.3 - 0.4
Scene balanced → Base Scale = 0.5 - 0.6 (default)
Scene fokus segitiga → Base Scale = 0.8 - 1.0
```

**Testing:**
```
1. Select TriangleVisualization GameObject
2. Inspector > Triangle Visualizer > Visual Settings
3. Base Scale: Ubah sambil Play Mode
4. Lihat hasilnya real-time
5. Stop Play Mode, set nilai yang pas
```

---

### **3. Adjust Line Thickness**

Ketebalan garis agar terlihat jelas.

**Nilai Rekomendasi:**
```
Garis tipis (minimalist): 0.08 - 0.10
Garis normal (default): 0.15 - 0.20
Garis tebal (bold): 0.25 - 0.35
```

**Cara Setting:**
```
TriangleVisualization > Inspector
Triangle Visualizer > Visual Settings
Line Thickness: [Adjust nilai]
```

---

### **4. Posisi Center Position**

Posisi pusat segitiga di world space.

**Untuk Scene dengan UI di kanan:**
```
Center Position: (-3, 0, 0)  ← Geser ke kiri
```

**Untuk Scene dengan UI di kiri:**
```
Center Position: (3, 0, 0)  ← Geser ke kanan
```

**Untuk Scene UI di atas/bawah:**
```
Center Position: (0, -1, 0)  ← Turun sedikit
```

**Testing dengan Gizmos:**
```
1. Play Mode
2. Scene View (bukan Game View)
3. Lihat garis CYAN dan sphere MERAH
4. Sesuaikan Center Position agar pas di frame
```

---

### **5. Camera Setup**

Pastikan Camera melihat area segitiga.

**Untuk 2D Game:**
```
Main Camera:
- Projection: Orthographic
- Size: 5-7 (tergantung seberapa luas area)
- Position: (0, 0, -10)
- Clear Flags: Solid Color
- Background: Warna gelap (untuk kontras dengan garis putih)
```

**Testing:**
```
Jika segitiga terpotong:
→ Naikkan Camera Size

Jika segitiga terlalu kecil:
→ Turunkan Camera Size atau naikkan Base Scale
```

---

### **6. Sorting Layer & Order**

Pastikan garis muncul di atas background.

**Settings untuk Sprites:**
```
Line_Depan SpriteRenderer:
- Sorting Layer: Default (atau buat layer "Triangle")
- Order in Layer: 10

Line_Samping SpriteRenderer:
- Sorting Layer: Default
- Order in Layer: 10

Line_Miring SpriteRenderer:
- Sorting Layer: Default
- Order in Layer: 10

Label_Depan, Samping, Miring:
- Sorting Layer: Default
- Order in Layer: 11 (di atas garis)
```

**Background:**
```
Background SpriteRenderer:
- Sorting Layer: Background (atau Default)
- Order in Layer: 0 (paling belakang)
```

---

### **7. Label Offset**

Jarak label dari garis agar tidak overlap.

**Nilai Rekomendasi:**
```
Label dekat (minim space): 0.3
Label sedang (default): 0.5
Label jauh (clear): 0.8
```

---

## 🎯 Quick Fix Guide

### **Masalah: Garis terlalu tipis/tidak terlihat**

**Solusi:**
1. Naikkan `Line Thickness` ke 0.25
2. Pastikan sprite warna putih solid (Alpha = 255)
3. Pastikan `Material: None` (default sprite material)

---

### **Masalah: Segitiga tidak membentuk sudut siku-siku**

**Solusi:**
1. Cek Sprite Pivot HARUS Center
2. Cek apakah sprites ter-rotasi manual di Inspector
   - Reset rotation: (0, 0, 0) untuk semua Line_*
3. Pastikan Base Scale konsisten

---

### **Masalah: Label overlap dengan garis**

**Solusi:**
1. Naikkan `Label Offset` ke 0.7 atau 1.0
2. Atau kecilkan Font Size label ke 1.5 atau 1.8

---

### **Masalah: Segitiga terlalu besar, keluar frame**

**Solusi:**
1. Kurangi `Base Scale` ke 0.3 atau 0.4
2. Atau perbesar Camera Orthographic Size

---

### **Masalah: Garis bergerak/berubah posisi sendiri**

**Solusi:**
1. Pastikan parent TriangleVisualization Transform:
   - Position: Sesuai Center Position
   - Rotation: (0, 0, 0)
   - Scale: (1, 1, 1)
2. Pastikan child Line_* Transform:
   - Position: (0, 0, 0) ← PENTING!
   - Rotation: (0, 0, 0) ← Akan diatur script
   - Scale: (1, 1, 1) ← Akan diatur script

---

## 🧪 Testing dengan Debug Gizmos

**Aktifkan Gizmos:**
```
1. Play Mode
2. Click tab "Scene" (bukan Game)
3. Lihat garis CYAN (gizmos outline segitiga)
4. Lihat sphere MERAH di vertex
5. Bandingkan dengan sprite putih

Jika gizmos cyan COCOK tapi sprite putih TIDAK:
→ Masalah di Sprite Pivot atau Scale

Jika gizmos cyan dan sprite putih SAMA-SAMA salah:
→ Masalah di Base Scale atau Center Position
```

---

## 📐 Contoh Setting yang BENAR

```
TriangleVisualization GameObject:
├── Transform:
│   ├── Position: (-3, 0, 0)
│   ├── Rotation: (0, 0, 0)
│   └── Scale: (1, 1, 1)
│
├── Triangle Visualizer Component:
│   ├── Base Scale: 0.5
│   ├── Center Position: (0, 0, 0) [Relative to parent]
│   ├── Label Offset: 0.5
│   ├── Line Thickness: 0.15
│   ├── Normal Color: White
│   ├── Highlight Color: Yellow
│   ├── Correct Color: Green
│   └── Wrong Color: Red
│
├── Line_Depan:
│   ├── Transform: (0, 0, 0) rotasi & scale diatur script
│   ├── SpriteRenderer:
│   │   ├── Sprite: Square (Pivot: Center!)
│   │   ├── Color: White
│   │   ├── Material: None
│   │   ├── Sorting Layer: Default
│   │   └── Order in Layer: 10
│
├── Line_Samping: [Same as Line_Depan]
├── Line_Miring: [Same as Line_Depan]
│
├── Label_Depan:
│   ├── Transform: Position diatur script
│   └── TextMeshPro:
│       ├── Font Size: 2
│       ├── Color: White
│       ├── Alignment: Center + Middle
│       ├── Sorting Layer: Default
│       └── Order in Layer: 11
│
├── Label_Samping: [Same as Label_Depan]
└── Label_Miring: [Same as Label_Depan]
```

---

## 🎨 Sprite Requirements

**Buat Sprite Sederhana di Unity:**
```
1. Assets > Create > Sprites > Square
2. Nama: "WhiteLine"
3. Gunakan untuk semua 3 garis
4. Import Settings:
   - Texture Type: Sprite (2D and UI)
   - Sprite Mode: Single
   - Pixels Per Unit: 100
   - Filter Mode: Point (no filter) untuk garis tajam
   - Compression: None
   - Pivot: Center ← KRUSIAL!
```

**Atau Import PNG:**
```
Buat PNG 100x100 putih solid di Paint/Photoshop
Import ke Unity
Set pivot: Center
Apply
```

---

## 💡 Pro Tips

1. **Gunakan Scene View untuk Debug:**
   - Play Mode → Scene View (bukan Game View)
   - Lihat gizmos untuk debugging visual
   - Adjust real-time sambil melihat efeknya

2. **Test dengan Segitiga Berbeda:**
   ```csharp
   // Di Start() TriangleVisualizer, test berbagai ukuran:
   DrawTriangle(3, 4, 5);   // Kecil
   DrawTriangle(5, 12, 13); // Sedang
   DrawTriangle(8, 15, 17); // Besar
   ```

3. **Gunakan Color Coding untuk Debug:**
   ```csharp
   depanSprite.color = Color.red;    // Debug: Depan merah
   sampingSprite.color = Color.blue; // Debug: Samping biru
   miringSprite.color = Color.green; // Debug: Miring hijau
   ```
   Ganti kembali ke putih setelah pasti posisinya benar.

4. **Screenshot Scene View untuk Dokumentasi:**
   - Capture hasil yang benar
   - Jadikan referensi untuk setup di scene lain

---

## 📞 Checklist Akhir

Sebelum declare "FIXED":

- [ ] Sprite pivot di center
- [ ] Base Scale pas dengan scene (0.3 - 1.0)
- [ ] Line Thickness terlihat jelas (0.15 - 0.25)
- [ ] Center Position segitiga tidak keluar frame
- [ ] Label tidak overlap dengan garis
- [ ] Camera Orthographic size pas
- [ ] Sorting Layer benar (garis > background)
- [ ] Test dengan segitiga 3-4-5, 5-12-13, 8-15-17
- [ ] Garis membentuk sudut siku-siku 90°
- [ ] Proporsi benar (misal: 3-4-5 → 3 lebih pendek dari 4)

---

**Jika masih ada masalah setelah semua langkah:**

1. Screenshot Scene View + Inspector settings
2. Export GameObject sebagai Prefab
3. Share untuk review

**Good luck! Segitiga pasti bisa pas! 📐✨**
