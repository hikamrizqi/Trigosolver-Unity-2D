# 🔄 Triangle Rotation Visual Guide

## 📐 Understanding Triangle Rotation in Trigosolver

Dalam trigonometri, **posisi sudut theta (θ)** menentukan mana sisi yang disebut "depan" (opposite), "samping" (adjacent), dan "miring" (hypotenuse). Dengan merotasi segitiga, pemain ditantang untuk mengidentifikasi sisi-sisi ini dengan benar tanpa tertipu oleh orientasi visual.

---

## 🎯 4 Orientasi Segitiga

### **Rotation 0° - STANDARD ORIENTATION**
```
      Miring (AC)
    ┌─────────┐
    │╲    θ    │
    │ ╲        │
    │  ╲       │  Samping (AB)
    │   ╲      │  Adjacent
    │    ╲     │
    │  90° ╲   │
    └──────╲───┘
      Depan (BC)
      Opposite

Visual ASCII:
   θ|\
 S  | \ M
 A  | A \
 M  | C  \
    |     \
    |______\
      D (BC)
```

**Karakteristik:**
- ✅ Theta (θ) di titik **A** (atas - sudut antara samping AB dan miring AC)
- ✅ Sudut siku-siku (90°) di titik **B** (kiri bawah)
- ✅ Depan BC (opposite): Garis **horizontal** ke kanan - TIDAK menyentuh θ
- ✅ Samping AB (adjacent): Garis **vertikal** ke atas - MENYENTUH θ
- ✅ Miring AC (hypotenuse): Garis **diagonal** dari atas ke kanan-bawah

**Rumus:**
- Sin θ = BC / AC = Depan / Miring (opposite/hypotenuse)
- Cos θ = AB / AC = Samping / Miring (adjacent/hypotenuse)
- Tan θ = BC / AB = Depan / Samping (opposite/adjacent)

**Difficulty:** Easy (Soal 1-10)

---

### **Rotation 90° - ROTATED CLOCKWISE**
```
      Samping (b)
    ┌─────────┐
    │      θ  │╲
    │         │ ╲
    │         │  ╲  Miring (c)
    │  Depan  │   ╲
    │   (a)   │    ╲
    │         │ 90° ╲
    └─────────┴──────┘

Visual ASCII:
   θ___
    \  |
  c  \ | a
      \|
       b
```

**Karakteristik:**
- ✅ Theta (θ) di **kiri atas** (sudut antara samping dan miring)
- ✅ Sudut siku-siku (90°) di **kanan bawah**
- ✅ Depan (opposite): Garis **vertikal** ke bawah (dari theta)
- ✅ Samping (adjacent): Garis **horizontal** ke kanan (dari theta)
- ✅ Miring (hypotenuse): Garis **diagonal** dari kiri-atas ke kanan-bawah

**Catatan Penting:**
- Meskipun visual berubah, **rumus tetap sama**!
- Yang "depan" adalah sisi **berlawanan** dari theta
- Yang "samping" adalah sisi **bersebelahan** dengan theta
- Jangan tertipu oleh orientasi visual!

**Rumus:**
- Sin θ = a / c (Depan tetap opposite dari θ)
- Cos θ = b / c (Samping tetap adjacent ke θ)
- Tan θ = a / b (Depan / Samping)

**Difficulty:** Medium (Soal 11-20)

---

### **Rotation 180° - INVERTED**
```
      Samping (b)
    ┌─────────┐
    │  90°  ╱ │
    │      ╱  │
    │  Depan │  Miring (c)
    │   (a) ╱  │
    │     ╱   │
    │  θ ╱    │
    └───╱─────┘

Visual ASCII:
       ╱|
      ╱ | a
  c  ╱  |
    ╱___|
   θ   b
```

**Karakteristik:**
- ✅ Theta (θ) di **kiri bawah** (sudut antara samping dan miring)
- ✅ Sudut siku-siku (90°) di **kanan atas**
- ✅ Depan (opposite): Garis **vertikal** ke bawah
- ✅ Samping (adjacent): Garis **horizontal** ke kiri
- ✅ Miring (hypotenuse): Garis **diagonal** dari kiri-bawah ke kanan-atas

**Challenge:**
- Segitiga terbalik dari orientasi standard
- Pemain harus **mental rotation** untuk identifikasi sisi
- "Depan" sekarang mengarah ke **bawah** secara visual

**Rumus:**
- Sin θ = a / c
- Cos θ = b / c
- Tan θ = a / b

**Difficulty:** Hard (Soal 21-30)

---

### **Rotation 270° - ROTATED COUNTER-CLOCKWISE**
```
    ┌──────┐
    │      │ Depan (a)
    │      │
    │Miring│
    │  (c) │╲
    │   90°│ ╲ Samping (b)
    │      │θ ╲
    └──────┴───┘

Visual ASCII:
    a
    |╲
    | ╲ b
    |  ╲θ
     c
```

**Karakteristik:**
- ✅ Theta (θ) di **kanan bawah** (sudut antara depan dan miring)
- ✅ Sudut siku-siku (90°) di **kiri atas**
- ✅ Depan (opposite): Garis **vertikal** ke atas
- ✅ Samping (adjacent): Garis **horizontal** ke kanan
- ✅ Miring (hypotenuse): Garis **diagonal** dari kiri-bawah ke kanan-atas

**Challenge:**
- Mirror image dari rotasi 90°
- Depan dan samping **bertukar posisi visual** dari standard
- Tetap gunakan definisi: opposite vs adjacent dari theta

**Rumus:**
- Sin θ = a / c
- Cos θ = b / c
- Tan θ = a / b

**Difficulty:** Hard (Soal 21-30)

---

## 🧠 Mental Strategy - How to Identify Sides

### **Step-by-Step Identification:**

1. **Find Theta (θ) First**
   - Theta adalah **sudut lancip** yang sedang dianalisis
   - Berada di **salah satu sudut non-siku** (biasanya di atas)
   - Look for the symbol "θ" in the triangle
   - This is your reference point!

2. **Identify Miring (Hypotenuse)**
   - **LONGEST** side of the triangle
   - Side **OPPOSITE** the right angle (90°)
   - Connects the two non-right-angle vertices
   - Formula: c² = a² + b²

3. **Identify Depan (Opposite)**
   - Side **OPPOSITE** to theta
   - Does NOT touch the theta vertex
   - In standard: Vertical line (from right angle to opposite vertex)
   - After rotation: Still the side opposite θ!

4. **Identify Samping (Adjacent)**
   - Side **NEXT TO** theta
   - Touches the theta vertex AND the right angle vertex
   - In standard: One of the sides forming the right angle
   - After rotation: Still the side adjacent to θ!

---

## 📊 Rotation Effect on Calculations

### **Important:** Rotation does NOT change the mathematical relationships!

| Rotation | Sin θ | Cos θ | Tan θ |
|----------|-------|-------|-------|
| 0°       | a/c   | b/c   | a/b   |
| 90°      | a/c   | b/c   | a/b   |
| 180°     | a/c   | b/c   | a/b   |
| 270°     | a/c   | b/c   | a/b   |

**Why?**
- Sin θ = **Opposite** / Hypotenuse (definition doesn't change)
- Cos θ = **Adjacent** / Hypotenuse (definition doesn't change)
- Tan θ = **Opposite** / Adjacent (definition doesn't change)

**What Changes:**
- ✅ **Visual orientation** of the triangle
- ✅ **Label positions** (depan, samping, miring)
- ✅ **Theta position** (but still at right angle)

**What Stays the Same:**
- ❌ Trigonometric ratios (Sin/Cos/Tan values)
- ❌ Mathematical relationships
- ❌ Answer to the question

---

## 🎓 Educational Value

### **Why Rotate Triangles?**

1. **Develop Spatial Reasoning:**
   - Students learn to recognize patterns regardless of orientation
   - Prevents "formula memorization" without understanding

2. **Real-World Application:**
   - In navigation, engineering, physics: triangles appear in any orientation
   - Students must identify components based on **relationship**, not position

3. **Deeper Understanding:**
   - Forces students to understand **WHY** depan is opposite (not just "it's the vertical line")
   - Reinforces that trig is about **angles and ratios**, not visual orientation

4. **Prevent Common Mistakes:**
   - Students who only memorize positions will struggle
   - Rotation reveals true understanding vs. pattern matching

---

## 💡 Tips for Players

### **Easy Level (0° only):**
- 📌 Build confidence with standard orientation
- 📌 Learn the basic definitions: SOH CAH TOA
- 📌 Get comfortable identifying depan, samping, miring

### **Medium Level (0° & 90°):**
- 📌 Always locate theta first!
- 📌 Trace from theta to identify adjacent vs opposite
- 📌 Don't rely on "vertical = depan" anymore

### **Hard Level (All rotations):**
- 📌 Ignore the visual orientation completely
- 📌 Focus on **relationships**: Which side is across from θ?
- 📌 Use the right angle as your anchor point
- 📌 Practice mental rotation

---

## 🔍 Common Mistakes

### ❌ **Mistake 1:** "Depan is always vertical"
**Reality:** Depan is the side **opposite** theta, regardless of orientation

### ❌ **Mistake 2:** "Samping is always horizontal"
**Reality:** Samping is the side **adjacent** to theta (next to it)

### ❌ **Mistake 3:** "Miring changes position"
**Reality:** Miring is ALWAYS the longest side (hypotenuse), opposite the right angle

### ❌ **Mistake 4:** "Rotation changes the answer"
**Reality:** Same triangle, same angle → Same Sin/Cos/Tan values!

---

## 🎮 In-Game Visual Cues

### **What You'll See:**

1. **Triangle Sprites:**
   - 3 colored lines forming the triangle
   - Rotated together as one unit

2. **Theta Label:**
   - Positioned at the right angle vertex
   - Moves with rotation to stay at correct corner

3. **Side Labels:**
   - Numbers showing side lengths
   - Positioned perpendicular to each side
   - Rotate to stay readable

4. **Console Debug:**
   - Shows rotation angle: "Rotation: 90°"
   - Shows difficulty: "Difficulty: Medium"

---

## 🧮 Practice Problems

### **Problem 1:** Triple (3, 4, 5) at 90°
```
Visual looks like:
    ___
    \  |
  5  \ | 3
      \|θ
       4

Question: Berapakah nilai Sinθ?
Answer: ?
```
<details>
<summary>Click for answer</summary>

**Answer:** 0.6 (3/5)

**Explanation:**
- Find theta → Bottom-right corner
- Depan (opposite) → The side NOT touching theta → 3
- Miring (hypotenuse) → Longest side → 5
- Sin θ = Depan/Miring = 3/5 = 0.6

Even though visually "3" looks vertical going up, it's still the **opposite** side from theta's perspective.
</details>

---

### **Problem 2:** Triple (5, 12, 13) at 180°
```
Visual looks like:
       /|
      / | 5
 13  /  |
    /_θ_|
       12

Question: Berapakah nilai Cosθ?
Answer: ?
```
<details>
<summary>Click for answer</summary>

**Answer:** 0.923 (12/13)

**Explanation:**
- Find theta → Top-right corner
- Samping (adjacent) → The side NEXT to theta (horizontal) → 12
- Miring (hypotenuse) → Longest side → 13
- Cos θ = Samping/Miring = 12/13 ≈ 0.923

The horizontal line going LEFT from theta is the adjacent side, even though it's at the top of the screen.
</details>

---

**🎯 Master these rotations, and you'll truly understand trigonometry!**
