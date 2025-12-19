# 📐 Setup Visualisasi Segitiga - Panduan Singkat

## Cara Kerja Sistem

**TriangleVisualizer** akan mengatur SEMUA positioning, rotation, dan scaling secara otomatis!

Anda hanya perlu:
1. 3 Sprites sederhana (bisa pakai sprite yang sama untuk ketiga-tiganya)
2. 3 TextMeshPro labels
3. Assign references di Inspector

## Langkah Setup (5 Menit)

### 1. Buat Sprite Sederhana (1 menit)

**Cara Tercepat - Gunakan Unity Sprite:**
```
1. Di Assets, Klik kanan > Create > Sprites > Square
2. Nama: "WhiteLine"
3. Selesai! Gunakan sprite ini untuk semua 3 garis
```

**Alternatif - Buat Custom:**
```
1. Buka Paint atau Photoshop
2. Buat canvas 100x100 pixels
3. Fill dengan warna putih (#FFFFFF)
4. Save as PNG dengan background transparent
5. Drag PNG ke Unity Assets folder
6. Klik PNG > Inspector > Texture Type: Sprite (2D and UI)
7. Apply
```

### 2. Buat GameObject Structure (2 menit)

```
Hierarchy View:
└── TriangleVisualization (Empty GameObject)
    ├── Line_Depan (GameObject + SpriteRenderer)
    ├── Line_Samping (GameObject + SpriteRenderer)
    ├── Line_Miring (GameObject + SpriteRenderer)
    ├── Label_Depan (TextMeshPro - World Space)
    ├── Label_Samping (TextMeshPro - World Space)
    └── Label_Miring (TextMeshPro - World Space)
```

**Step-by-step:**
```
1. Klik kanan di Hierarchy > Create Empty
   Nama: "TriangleVisualization"
   Position: (-3, 0, 0) atau sesuaikan dengan view kamera

2. Klik kanan di TriangleVisualization > Create Empty (ulangi 3x)
   Nama: Line_Depan, Line_Samping, Line_Miring
   
3. Untuk setiap Line_*, Add Component > Sprite Renderer
   - Sprite: [Drag sprite WhiteLine atau sprite putih anda]
   - Color: White
   - Material: None (Default)
   - JANGAN atur Position/Rotation/Scale!

4. Klik kanan di TriangleVisualization > 3D Object > Text - TextMeshPro (ulangi 3x)
   Nama: Label_Depan, Label_Samping, Label_Miring
   
   Untuk setiap Label:
   - Text: "?"
   - Font Size: 2
   - Alignment: Center + Middle
   - Color: White
   - JANGAN atur Position!
```

### 3. Attach & Configure Script (2 menit)

```
1. Select TriangleVisualization GameObject
2. Add Component > TriangleVisualizer

3. Di Inspector, assign references:
   
   Sprite References:
   - Depan Sprite: [Drag Line_Depan dari Hierarchy]
   - Samping Sprite: [Drag Line_Samping dari Hierarchy]
   - Miring Sprite: [Drag Line_Miring dari Hierarchy]
   
   Label References:
   - Depan Label: [Drag Label_Depan dari Hierarchy]
   - Samping Label: [Drag Label_Samping dari Hierarchy]
   - Miring Label: [Drag Label_Miring dari Hierarchy]
   
   Visual Settings (biarkan default):
   - Base Scale: 0.5
   - Center Position: (0, 0, 0)
   - Label Offset: 0.5
   
   Colors (biarkan default):
   - Normal Color: White
   - Highlight Color: Yellow
   - Correct Color: Green
   - Wrong Color: Red
```

### 4. Hubungkan dengan UIManager

```
1. Select UIManager_Chapter1 GameObject
2. Di Inspector > UIManagerChapter1 component:
   
   Visualisasi Segitiga:
   - Triangle Visualizer: [Drag TriangleVisualization GameObject]
   - Depan Label World: [Drag Label_Depan]
   - Samping Label World: [Drag Label_Samping]
   - Miring Label World: [Drag Label_Miring]
   - Depan Sprite: [Expand Line_Depan di Hierarchy, drag SpriteRenderer component]
   - Samping Sprite: [Expand Line_Samping, drag SpriteRenderer]
   - Miring Sprite: [Expand Line_Miring, drag SpriteRenderer]
```

### 5. Test!

```
1. Press Play
2. Segitiga seharusnya muncul otomatis dengan proporsi yang benar!
3. Klik tombol Depan/Samping/Miring untuk test highlight
```

---

## Customization

### Mengubah Ukuran Segitiga
**Lokasi:** TriangleVisualization > Inspector > Visual Settings > Base Scale
- **0.2** = Kecil (untuk layar penuh elemen UI)
- **0.5** = Sedang (default, balance)
- **1.0** = Besar (untuk focus pada segitiga)

### Mengubah Posisi Segitiga
**Lokasi:** TriangleVisualization > Transform > Position
- **(-3, 0, 0)** = Kiri layar
- **(3, 0, 0)** = Kanan layar
- **(0, 0, 0)** = Tengah

### Mengubah Ketebalan Garis
**Lokasi:** Edit script TriangleVisualizer.cs baris 83
```csharp
// Ubah dari 0.1f ke nilai lain:
sprite.transform.localScale = new Vector3(distance, 0.2f, 1f); // Lebih tebal
sprite.transform.localScale = new Vector3(distance, 0.05f, 1f); // Lebih tipis
```

### Mengubah Jarak Label dari Garis
**Lokasi:** TriangleVisualization > Inspector > Visual Settings > Label Offset
- **0.3** = Dekat dengan garis
- **0.5** = Sedang (default)
- **1.0** = Jauh dari garis

### Mengubah Warna
**Lokasi:** TriangleVisualization > Inspector > Colors

Edit sesuai kebutuhan brand/tema:
```
Normal Color: #FFFFFF (putih)
Highlight Color: #FFFF00 (kuning) - untuk tombol interaktif
Correct Color: #00FF00 (hijau) - jawaban benar
Wrong Color: #FF0000 (merah) - jawaban salah
```

---

## FAQ Cepat

**Q: Segitiga tidak muncul?**
A: Pastikan Camera Orthographic dan melihat area z=0. Set camera position (0, 0, -10)

**Q: Garis terlalu tipis/tidak terlihat?**
A: Edit TriangleVisualizer.cs line 83, ubah 0.1f ke 0.2f atau lebih

**Q: Label overlap?**
A: Naikkan Label Offset di Inspector

**Q: Segitiga terpotong kamera?**
A: Perbesar Camera Orthographic Size atau kecilkan Base Scale

**Q: Warna tidak berubah saat highlight?**
A: Pastikan Material sprite = None (menggunakan default Sprite material)

**Q: Posisi segitiga aneh?**
A: Reset Transform TriangleVisualization ke (0,0,0), adjust via Center Position di script settings

---

## Video Tutorial (Recommended)

Jika masih bingung, lihat video setup di folder:
`Assets/Tutorials/TriangleSetup.mp4` (jika tersedia)

Atau buat sendiri screen recording saat setup pertama kali untuk referensi tim!

---

**Selamat! Sistem visualisasi segitiga sudah siap digunakan! 🎉**

Setiap kali ada soal baru, segitiga akan otomatis:
✅ Menggambar dengan ukuran proporsional
✅ Menempatkan label di posisi yang tepat
✅ Merotasi garis sesuai sudut yang benar
✅ Highlight saat diklik atau dijawab
