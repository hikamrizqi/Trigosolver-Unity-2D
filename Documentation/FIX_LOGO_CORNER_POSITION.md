# 🔧 Fix Logo Position - Anchor & Corner Setup

## 🚨 Problem: Logo Mengecil Tapi Posisi Salah

Logo shrink tapi tidak pindah ke posisi yang diset di Inspector.

---

## 🎯 Root Cause: Anchor Position vs Pivot

**Corner Position** di MenuAnimationController menggunakan **Anchored Position**, yang relative terhadap **Anchor Point** logo.

### **Contoh:**

Jika Logo anchor di **Center (0.5, 0.5)**:
- `cornerPosition = (300, -100)` → 300 pixels ke kanan, 100 pixels ke atas dari CENTER screen

Jika Logo anchor di **Top-Right (1, 1)**:
- `cornerPosition = (-50, -50)` → 50 pixels ke kiri, 50 pixels ke bawah dari TOP-RIGHT corner

---

## ✅ Solution: Set Correct Anchor & Position

### **Option A: Recommended - Anchor Top-Right**

Untuk logo di pojok kanan atas:

1. **Select Logo GameObject** di Inspector
2. **RectTransform:**
   ```
   Anchors: 
     Min: (1, 1)  ← Top-Right
     Max: (1, 1)  ← Top-Right
   
   Pivot: (0.5, 0.5)  ← Center pivot OK
   
   Position:
     Pos X: -100  ← 100px dari kanan
     Pos Y: -100  ← 100px dari atas
   
   (Ignore Width/Height jika auto dari content)
   ```

3. **MenuAnimationController → Corner Position:**
   ```
   Corner Position: (-100, -100)
   ```

**Result:** Logo akan pindah ke 100px dari pojok kanan atas.

---

### **Option B: Anchor Center (Current)**

Jika logo anchor di center screen:

1. **Select Logo GameObject**
2. **RectTransform:**
   ```
   Anchors:
     Min: (0.5, 0.5)  ← Center
     Max: (0.5, 0.5)  ← Center
   
   Pivot: (0.5, 0.5)
   ```

3. **MenuAnimationController → Corner Position:**
   ```
   Corner Position X: (Screen Width / 2) - margin
   Corner Position Y: (Screen Height / 2) - margin
   
   Untuk 1920x1080:
     X: 960 - 150 = 810
     Y: 540 - 100 = 440
   
   Atau lebih simple:
     X: 800 (pojok kanan)
     Y: 450 (atas)
   ```

---

## 🛠️ Setup Steps

### **STEP 1: Check Logo Current Anchor**

1. **Play scene**
2. **Select Logo** di Hierarchy
3. **Inspector → RectTransform → Anchors**
4. **Lihat posisi anchor** (visualized with 4 flower petals)

**Common Anchors:**
- **Center (0.5, 0.5):** Anchor di tengah screen
- **Top-Right (1, 1):** Anchor di pojok kanan atas
- **Stretch:** Anchor spread (beda min/max)

---

### **STEP 2: Choose Strategy**

#### **Strategy A: Change Logo Anchor to Top-Right** (RECOMMENDED)

**Keuntungan:** Position value langsung = offset dari pojok

1. **Select Logo**
2. **Inspector → RectTransform**
3. **Klik Anchor Presets** (kotak kecil di kiri atas RectTransform)
4. **Hold Shift+Alt → Click "Top-Right" preset**
   - Shift = set pivot juga
   - Alt = set position juga
5. **Adjust Position:**
   ```
   Pos X: -150  (150px dari kanan)
   Pos Y: -100  (100px dari atas)
   ```

6. **MenuAnimationController:**
   ```
   Corner Position: (-150, -100)
   ```

---

#### **Strategy B: Keep Center Anchor, Calculate Position**

**Keuntungan:** Logo tetap centered saat drop

1. **Logo anchor tetap Center (0.5, 0.5)**

2. **Hitung Corner Position:**

**Formula:**
```csharp
// Untuk pojok kanan atas:
float cornerX = (Screen.width / 2) - offsetFromRight;
float cornerY = (Screen.height / 2) - offsetFromTop;

// Contoh untuk 1920x1080:
// Offset 150px dari kanan, 100px dari atas:
cornerX = (1920 / 2) - 150 = 960 - 150 = 810
cornerY = (1080 / 2) - 100 = 540 - 100 = 440
```

3. **Set di Inspector:**
   ```
   Corner Position: (810, 440)
   Corner Scale: 0.3
   ```

**ATAU untuk resolution-independent:**

Buat duplicate logo secara manual di scene:

1. **Duplicate Logo** (Ctrl+D)
2. **Rename: "LogoCornerReference"**
3. **Move ke pojok kanan atas** (manual drag)
4. **Scale ke 30%** (Transform scale)
5. **Inspector → RectTransform:**
   - Copy **Anchored Position** values
6. **Delete "LogoCornerReference"**
7. **Paste values ke MenuAnimationController → Corner Position**

---

### **STEP 3: Test**

1. **Play scene**
2. **Klik logo**
3. **Check Console:**
   ```
   AnimateShrinkToCorner dimulai - Target: (X, Y), Current: (X, Y)
   AnimateShrinkToCorner selesai - Final: (X, Y), Target: (X, Y)
   ```

4. **If Final ≠ Target:**
   - Anchor issue
   - Follow Strategy A or B above

---

## 📐 Position Cheat Sheet

### **For 1920x1080 Resolution:**

#### **Anchor: Center (0.5, 0.5)**
```
Top-Right:    (810, 440)
Top-Left:     (-810, 440)
Bottom-Right: (810, -440)
Bottom-Left:  (-810, -440)
```

#### **Anchor: Top-Right (1, 1)**
```
Corner:       (-150, -100)
Far corner:   (-300, -200)
Edge:         (-50, -50)
```

#### **Anchor: Top-Left (0, 1)**
```
Corner:       (150, -100)
Far corner:   (300, -200)
Edge:         (50, -50)
```

---

## 🧪 Debug Test

Add this debug visualization:

1. **Create empty GameObject: "CornerMarker"**
2. **Add UI → Image**
3. **Set:**
   ```
   Anchor: Same as logo target (Top-Right or Center)
   Position: Same as Corner Position
   Color: Red (for visibility)
   Size: 50x50
   ```

4. **Play scene:**
   - Red marker shows target position
   - Logo should shrink to exactly that marker

---

## 🚨 Common Mistakes

### ❌ Mistake 1: Mixed Anchors
Logo drop anchor = Center, but corner position calculated for Top-Right anchor.

**Fix:** Keep consistent anchor for both animations.

---

### ❌ Mistake 2: Wrong Resolution Reference
Set corner position for 1920x1080 but testing in 1280x720.

**Fix:** Use resolution-independent values or anchor presets.

---

### ❌ Mistake 3: Pivot Confusion
Pivot affects rotation/scale center, not position.

**Fix:** Pivot can be (0.5, 0.5), focus on Anchor for position.

---

## ✅ Final Check

After fix:
- [ ] Logo shrink smooth
- [ ] Logo pindah ke posisi yang tepat
- [ ] Logo **TETAP VISIBLE** (tidak menghilang)
- [ ] Logo scale 30% (atau sesuai setting)
- [ ] Console log: Final position = Target position

---

## 📱 Portrait Mode Note

Saat switch ke portrait (1080x1920):
- Corner position values **harus diubah**
- Recommended: Use **Anchor Top-Right** method
- Test di berbagai aspect ratios

---

**Need exact position values? Check Console log saat animation selesai!** 🎯
