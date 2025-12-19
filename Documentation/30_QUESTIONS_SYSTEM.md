# 📚 Sistem 30 Soal dengan Progressive Difficulty & Rotasi Segitiga

## 🎯 Overview

Sistem telah di-upgrade dari **5 soal** menjadi **30 soal** dengan fitur-fitur baru:
- ✅ **Progressive Difficulty** (Easy → Medium → Hard)
- ✅ **Triangle Rotation** (0°, 90°, 180°, 270°)
- ✅ **Varied Question Types** (7 tipe pertanyaan berbeda)
- ✅ **12 Pythagorean Triples** (dari sederhana hingga kompleks)

---

## 📊 Struktur Soal

### **Soal 1-10: EASY** 🟢
- **Difficulty:** Basic
- **Rotation:** 0° (standard orientation)
- **Question Types:** Sin θ, Cos θ, Tan θ
- **Triples:** (3,4,5), (5,12,13), (8,15,17), (7,24,25)
- **Goal:** Pemain terbiasa dengan konsep dasar trigonometri

**Contoh Soal:**
```
Soal 1: Berapakah nilai Sinθ?
Soal 2: Berapakah nilai Cosθ?
Soal 3: Berapakah nilai Tanθ?
```

---

### **Soal 11-20: MEDIUM** 🟡
- **Difficulty:** Intermediate
- **Rotation:** 0° dan 90° (bervariasi)
- **Question Types:** 
  - Basic trig (Sin/Cos/Tan)
  - **Inverse problems** (diberikan rasio, cari sisi)
- **Triples:** 8 triple pertama termasuk multiples
- **Goal:** Pemain mulai menghadapi rotasi dan soal yang lebih kompleks

**Contoh Soal:**
```
Soal 12: Berapakah nilai Sinθ? (Rotasi 90°)
Soal 15: Jika Sinθ = 0.60 dan sisi miring = 5, berapa panjang sisi depan?
Soal 18: Jika Cosθ = 0.92 dan sisi miring = 13, berapa panjang sisi samping?
```

---

### **Soal 21-30: HARD** 🔴
- **Difficulty:** Advanced
- **Rotation:** 0°, 90°, 180°, 270° (semua variasi)
- **Question Types:**
  - Basic trig (dengan rotasi kompleks)
  - Inverse problems
  - **Pythagorean theorem** (cari sisi ketiga dari 2 sisi)
- **Triples:** Semua 12 triple termasuk yang sulit
- **Goal:** Pemain benar-benar memahami konsep dan bisa beradaptasi dengan berbagai orientasi

**Contoh Soal:**
```
Soal 21: Jika sisi depan = 20 dan sisi samping = 21, berapa panjang sisi miring? (Rotasi 0°)
Soal 24: Berapakah nilai Tanθ? (Rotasi 180°)
Soal 27: Jika Sinθ = 0.48 dan sisi depan = 12, berapa panjang sisi miring? (Rotasi 90°)
Soal 30: Jika sisi samping = 84 dan sisi miring = 85, berapa panjang sisi depan? (Rotasi 270°)
```

---

## 🔢 12 Pythagorean Triples

| No | Triple | Type | Usage |
|----|--------|------|-------|
| 1 | (3, 4, 5) | Basic | Easy |
| 2 | (5, 12, 13) | Basic | Easy |
| 3 | (8, 15, 17) | Basic | Easy |
| 4 | (7, 24, 25) | Basic | Easy |
| 5 | (6, 8, 10) | Multiple of (3,4,5) | Medium |
| 6 | (9, 12, 15) | Multiple of (3,4,5) | Medium |
| 7 | (12, 16, 20) | Multiple of (3,4,5) | Medium |
| 8 | (15, 20, 25) | Multiple of (3,4,5) | Medium |
| 9 | (20, 21, 29) | Advanced | Hard |
| 10 | (9, 40, 41) | Advanced | Hard |
| 11 | (11, 60, 61) | Advanced | Hard |
| 12 | (13, 84, 85) | Advanced | Hard |

---

## 🎲 7 Tipe Pertanyaan

### 1️⃣ **FindSinValue** - Cari nilai Sin θ
```csharp
Pertanyaan: "Berapakah nilai Sinθ?"
Jawaban: Depan / Miring
Contoh: 3/5 = 0.6
```

### 2️⃣ **FindCosValue** - Cari nilai Cos θ
```csharp
Pertanyaan: "Berapakah nilai Cosθ?"
Jawaban: Samping / Miring
Contoh: 4/5 = 0.8
```

### 3️⃣ **FindTanValue** - Cari nilai Tan θ
```csharp
Pertanyaan: "Berapakah nilai Tanθ?"
Jawaban: Depan / Samping
Contoh: 3/4 = 0.75
```

### 4️⃣ **FindOpposite** - Diberikan Sin θ & miring, cari depan
```csharp
Pertanyaan: "Jika Sinθ = 0.60 dan sisi miring = 5, berapa panjang sisi depan?"
Jawaban: 3
Rumus: Depan = Sin θ × Miring
```

### 5️⃣ **FindAdjacent** - Diberikan Cos θ & miring, cari samping
```csharp
Pertanyaan: "Jika Cosθ = 0.80 dan sisi miring = 5, berapa panjang sisi samping?"
Jawaban: 4
Rumus: Samping = Cos θ × Miring
```

### 6️⃣ **FindHypotenuse** - Diberikan Sin θ & depan, cari miring
```csharp
Pertanyaan: "Jika Sinθ = 0.60 dan sisi depan = 3, berapa panjang sisi miring?"
Jawaban: 5
Rumus: Miring = Depan / Sin θ
```

### 7️⃣ **FindPythagorean** - Diberikan 2 sisi, cari sisi ketiga
```csharp
Pertanyaan: "Jika sisi depan = 3 dan sisi samping = 4, berapa panjang sisi miring?"
Jawaban: 5
Rumus: c² = a² + b² (Pythagoras)
```

---

## 🔄 Rotasi Segitiga

### **0° - Standard** (Theta di kiri bawah)
```
    |\
  D | \  M
    |  \
    |___\
      S
```

### **90° - Rotasi Searah Jarum Jam** (Theta di kiri atas)
```
    ___
    \  |
  M  \ | D
      \|
       S
```

### **180° - Terbalik** (Theta di kanan atas)
```
       /|
    M / | D
     /  |
    /___|
       S
```

### **270° - Rotasi 3/4 Putaran** (Theta di kanan bawah)
```
    S
    |  /
  D | / M
    |/
```

**Catatan:** Posisi theta (θ) selalu di **sudut siku-siku** (90°), hanya orientasinya yang berubah!

---

## 🎮 Scoring System

| Aspect | Detail |
|--------|--------|
| **Total Questions** | 30 soal |
| **Points per Question** | +10 poin |
| **Maximum Score** | 300 poin |
| **Lives** | 3 nyawa |
| **Answer Tolerance** | ±0.01 (untuk desimal) |

---

## 💡 Tips untuk Pemain

### **Level Easy (1-10)**
- Fokus memahami konsep dasar Sin, Cos, Tan
- Segitiga tidak dirotasi, mudah dikenali
- Gunakan mnemonic: **SOH CAH TOA**
  - **S**in = **O**pposite/**H**ypotenuse
  - **C**os = **A**djacent/**H**ypotenuse
  - **T**an = **O**pposite/**A**djacent

### **Level Medium (11-20)**
- Segitiga mulai dirotasi, perhatikan posisi theta!
- Muncul soal inverse (diberikan rasio, cari sisi)
- Rumus inverse:
  - Jika Sin θ = x, maka Depan = x × Miring
  - Jika Cos θ = x, maka Samping = x × Miring

### **Level Hard (21-30)**
- Rotasi bervariasi (0°, 90°, 180°, 270°)
- Soal Pythagorean: a² + b² = c²
- Identifikasi sisi yang **benar-benar** depan/samping dari posisi theta!
- Jangan tertipu oleh orientasi visual

---

## 🛠️ Technical Implementation

### **TriangleData Structure**
```csharp
public class TriangleData
{
    // Triangle dimensions
    public int Depan;           // Opposite side
    public int Samping;         // Adjacent side
    public int Miring;          // Hypotenuse
    
    // Rotation
    public float RotationAngle; // 0°, 90°, 180°, 270°
    
    // Question
    public QuestionType TypeSoal;
    public DifficultyLevel Difficulty;
    public string PertanyaanText;       // Full question text
    public string SoalDisederhanakan;   // Short form
    public float JawabanBenar;          // Correct answer
    
    // Additional info
    public string InfoTambahan;         // Hints (e.g., "Sin θ = 0.6")
    public int SisiDiketahui1;          // Known side 1
    public int SisiDiketahui2;          // Known side 2
}
```

### **Question Generation Flow**
```
CalculationManager.StartNewRound()
    ↓
dataGenerator.GenerateQuestionByNumber(progres) // 1-30
    ↓
Determine Difficulty (1-10=Easy, 11-20=Medium, 21-30=Hard)
    ↓
Select Pythagorean Triple
    ↓
Assign Rotation (Easy=0°, Medium=0°/90°, Hard=0°/90°/180°/270°)
    ↓
Generate Question Content (Basic/Inverse/Pythagorean)
    ↓
Return TriangleData
    ↓
UIManager.SetupNewQuestion(data)
    ↓
TriangleVisualizer.DrawTriangle(depan, samping, miring, rotation)
```

### **Triangle Rotation Math**
```csharp
private Vector3 RotatePoint(Vector3 point, float angleRad)
{
    float cos = Mathf.Cos(angleRad);
    float sin = Mathf.Sin(angleRad);
    
    float newX = point.x * cos - point.y * sin;
    float newY = point.x * sin + point.y * cos;
    
    return new Vector3(newX, newY, point.z);
}
```

---

## 📈 Progression Curve

```
Difficulty
   🔴 Hard      ████████████████████
   🟡 Medium    ██████████
   🟢 Easy      █████
                ├──┼──┼──┼──┼──┼──┤
                1  10  20  30
                    Question Number
```

---

## 🎨 Visual Indicators

### **Difficulty Badge**
- 🟢 **Easy:** Green badge "MUDAH"
- 🟡 **Medium:** Yellow badge "SEDANG"  
- 🔴 **Hard:** Red badge "SULIT"

### **Rotation Indicator**
- Display rotation angle: "Rotasi: 90°"
- Theta symbol (θ) position changes based on rotation
- Visual cue: Arrow showing rotation direction

---

## 🧪 Testing Checklist

- [ ] Soal 1-10: Hanya Sin/Cos/Tan, rotasi 0°
- [ ] Soal 11-20: Mix basic + inverse, rotasi 0°/90°
- [ ] Soal 21-30: Semua tipe, rotasi 0°/90°/180°/270°
- [ ] Auto-scaling: Segitiga besar (84, 85) tidak overflow
- [ ] Rotasi visual: Label depan/samping/miring sesuai rotasi
- [ ] Theta position: Selalu di sudut siku setelah rotasi
- [ ] Answer validation: Toleransi ±0.01 untuk semua tipe soal
- [ ] Progressive difficulty: Soal bertambah sulit secara konsisten

---

## 🚀 Future Enhancements

### **Ide Tambahan:**
1. **Visual Hints:**
   - Tampilkan rumus yang relevan sebagai hint
   - Animasi rotasi segitiga saat soal muncul
   - Highlight sisi yang ditanyakan dengan warna berbeda

2. **Question Variations:**
   - **Angle Finding:** Diberikan rasio, cari sudut θ (dalam derajat)
   - **Multiple Choice:** Pilihan ganda untuk level hard
   - **Time Challenge:** Bonus poin jika jawab dalam waktu tertentu

3. **Educational Feedback:**
   - Jika salah, tunjukkan langkah perhitungan yang benar
   - "Hint Mode" untuk pemain yang stuck
   - Review soal yang pernah salah di akhir chapter

4. **Adaptive Difficulty:**
   - Jika pemain sering salah di level medium, turunkan ke easy
   - Jika terlalu mudah, skip beberapa soal easy

---

**🎓 Happy Learning Trigonometry!**
