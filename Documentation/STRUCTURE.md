# Struktur Folder Scripts - Trigosolver

## 📁 Organisasi Folder

### **Stage 1** - Chapter 1: Observasi Segitiga (Trigonometri)
**Konsep**: Pemain mengamati segitiga dan menghitung nilai Sin/Cos/Tan

**File Utama:**
- `CalculationManager.cs` - Manager utama gameplay observasi
- `TriangleDataGenerator.cs` - Generator soal trigonometri (Pythagorean triples)
- `UIManagerChapter1.cs` - UI manager untuk tampilan soal dan feedback
- `Chapter1AudioManager.cs` - Audio manager untuk sound effects
- `Chapter1EndCutscene.cs` - Cutscene akhir chapter
- `InputFieldHandler.cs` - Handler untuk input jawaban

**Data Classes:**
- `TriangleDataStage2` - Data segitiga (Depan, Samping, Miring)
- `TriangleDataGeneratorStage2` - Generator dengan Pythagorean triples

**Gameplay:**
1. Sistem menampilkan segitiga dengan nilai sisi
2. Pemain diminta menghitung Sin θ, Cos θ, atau Tan θ
3. Input jawaban (desimal atau pecahan: 3/5)
4. Feedback visual: hijau (benar), merah (salah)
5. Lives system: 3 nyawa, 5 soal

---

### **Stage 2** - Chapter 2: Cannon Challenge (Balistik)
**Konsep**: Pemain harus menghitung sudut elevasi meriam untuk mengenai target

**File Utama:**
- `GameManagerChapter2.cs` - Manager utama gameplay meriam
- `CannonController.cs` - Kontrol rotasi dan animasi meriam
- `ProjectileController.cs` - Kontrol peluru yang ditembakkan
- `TriangleVisualizer.cs` - Visualisasi segitiga siku-siku untuk bantuan kalkulasi

**Gameplay:**
1. Pemain diberikan jarak target
2. Harus menghitung sudut elevasi yang tepat
3. Input sudut ke meriam
4. Tembak dan lihat apakah mengenai target

---

## 🎮 Perbedaan Utama

| Aspek | Stage 1 (Observasi) | Stage 2 (Cannon) |
|-------|---------------------|------------------|
| **Chapter** | Chapter 1 | Chapter 2 |
| **Fokus** | Trigonometri murni | Fisika balistik |
| **Input** | Nilai rasio (0-1 atau pecahan) | Sudut elevasi (0-90°) |
| **Visual** | Segitiga statis | Meriam + Proyektil |
| **Kesulitan** | Hitung rasio dari sisi | Hitung sudut dari jarak |
| **Feedback** | Benar/Salah dengan highlight | Hit/Miss target |

---

## 📝 Catatan Pengembangan

**Sebelumnya:**
- Stage 1 dan Stage 2 tercampur dan terbalik
- Class names tidak konsisten

**Sekarang:**
- ✅ **Stage 1**: Chapter 1 - Observasi Segitiga (CalculationManager, TriangleDataGenerator)
- ✅ **Stage 2**: Chapter 2 - Cannon Challenge (GameManagerChapter2, CannonController)
- ✅ Class names konsisten dengan chapter number
- ✅ Tidak ada konflik atau duplicate

**Scene Names:**
- Stage 1 Scene: `Stage 1` (Chapter 1 - Observasi)
- Stage 2 Scene: `Stage 2` (Chapter 2 - Cannon)

---

## ✅ Checklist Setup

### Stage 1 (Observasi Segitiga):
- [ ] Create scene "Stage 1"  
- [ ] Setup CalculationManager di scene
- [ ] Setup TriangleDataGeneratorStage2
- [ ] Assign UIManagerChapter1 dengan semua UI references
- [ ] Setup SpriteRenderers untuk visualisasi segitiga
- [ ] Setup ParticleSystem untuk sparkle effect
- [ ] Connect Chapter1AudioManager dan Chapter1EndCutscene

### Stage 2 (Cannon Challenge):
- [ ] Create scene "Stage 2"
- [ ] Setup Cannon GameObject dengan CannonController
- [ ] Setup ProjectilePrefab dengan ProjectileController
- [ ] Assign GameManagerChapter2 ke scene
- [ ] Connect UI elements (angleInputField, shootButton, questionText)
- [ ] Setup target dengan tag "Target"
- [ ] Setup TriangleVisualizer untuk helper

---

**Updated:** December 10, 2025
**Version:** 2.0 - Struktur sudah benar: Stage 1 = Chapter 1 Observasi, Stage 2 = Chapter 2 Cannon
