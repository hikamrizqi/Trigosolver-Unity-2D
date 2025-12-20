# 🎬 Setup Logo Animation & Scene Fade Transition

## 🎯 Overview

Panduan untuk setup logo animation yang lebih smooth:
1. **Logo shrink to corner** setelah di-klik (bukan hilang)
2. **Scene fade transition** dengan black screen hold sebelum load scene
3. **Delayed logo drop** dengan background fade in dulu

---

## 📋 Implementation Steps

### ✅ **STEP 1: Setup SceneFadeController (10 menit)**

#### A. Create SceneFadeController GameObject

1. **Di Main Menu scene:**
   - Hierarchy → Right-click → Create Empty
   - Rename: `SceneFadeController`
   - Add Component → `SceneFadeController.cs`

2. **Inspector settings:**
   ```
   Fade Panel: (leave null - auto-create)
   Fade Canvas: (leave null - auto-create)
   
   Fade In Duration: 1.0 (fade dari hitam ke scene)
   Fade Out Duration: 0.8 (fade scene ke hitam)
   Fade Color: Black (0, 0, 0, 255)
   
   Auto Fade In On Start: ✓ (checked)
   ```

#### B. Test

- Play scene → Should auto fade in dari hitam
- Scene visible setelah 1 detik

---

### ✅ **STEP 2: Update OpeningVideoController (5 menit)**

#### Di Video Opening scene:

1. **Select VideoController GameObject**
2. **Inspector → OpeningVideoController component:**
   ```
   Hold Duration: 1.0 (pause di frame terakhir)
   Fade Duration: 1.0 (fade out video)
   Black Screen Hold Duration: 0.5 ← NEW! (hold hitam sebelum load)
   ```

#### Flow Baru:
```
Video → Pause → Hold 1s → Fade out 1s → Black 0.5s → Load scene → Fade in 1s
```

---

### ✅ **STEP 3: Setup Logo Animation (15 menit)**

#### A. Update Logo GameObject

1. **Di Main Menu scene, select Logo GameObject**
2. **Inspector → MenuAnimationController component:**

**Add New Settings:**
```
[Logo Corner Settings]
Corner Position: (300, -100) ← Pojok kanan atas
  (Adjust sesuai layout kamu)
  
Corner Scale: 0.3 ← 30% dari size original
  (0.2-0.4 recommended)
  
Shrink Duration: 0.8 ← Durasi animasi shrink

[Scene Fade In Settings]
Scene Fade In Duration: 1.0 ← Match dengan SceneFadeController
Delay Before Drop: 0.5 ← Pause setelah fade in sebelum logo drop
```

#### B. Add LogoClickHandler Component

1. **Logo GameObject → Add Component → `LogoClickHandler.cs`**
2. **Inspector:**
   ```
   Animation Controller: (drag MenuAnimationController, atau auto-detect)
   Enable Click: ✓
   Click Delay After Drop: 1.0 (cegah click saat animasi)
   ```

#### C. Add Event System (Jika Belum Ada)

Agar logo bisa di-klik:
```
Hierarchy → Right-click → UI → Event System
```

Check hanya ada 1 Event System di scene.

---

### ✅ **STEP 4: Update Logo Start Animation (5 menit)**

Agar logo pakai delayed drop (background fade in dulu):

#### Option A: Manual di Inspector

1. **Find script yang trigger logo drop** (misal: MenuManager atau MainMenuController)
2. **Change method call dari:**
   ```csharp
   logoAnimationController.AnimateDropIn();
   ```
   **Ke:**
   ```csharp
   logoAnimationController.AnimateDropInDelayed();
   ```

#### Option B: Auto-trigger di Start

Jika logo auto-drop saat scene load, bisa set di MenuAnimationController:
```csharp
private void Start()
{
    // Delayed drop dengan fade in background dulu
    AnimateDropInDelayed();
}
```

---

### ✅ **STEP 5: Adjust Corner Position (5 menit)**

#### Test di Play Mode:

1. **Play Main Menu scene**
2. **Klik logo** → Should shrink dan pindah ke corner
3. **Adjust Corner Position** jika posisi tidak pas:

**Recommended positions:**
- **Pojok kanan atas:** `(300, -100)` atau `(400, -150)`
- **Pojok kiri atas:** `(-300, -100)` atau `(-400, -150)`

**Cara adjust:**
1. Stop Play mode
2. Select Logo → Inspector → Menu Animation Controller
3. Change `Corner Position` X dan Y
4. Play lagi → Test

---

## 🎨 Visual Flow Diagram

### **Before:**
```
Video → Fade out → Scene load
                     ↓
           Logo drop LANGSUNG
```

### **After:**
```
Video → Fade out → Black screen (0.5s) → Scene load
                                            ↓
                                    Background fade in (1s)
                                            ↓
                                    Pause (0.5s)
                                            ↓
                                    Logo drop dengan bounce
                                            ↓
                                    Logo clickable (setelah 1s)
                                            ↓
                                    Logo shrink to corner
```

---

## ⚙️ Parameters Tuning Guide

### A. Scene Fade Speed
**Lambat & Cinematic:**
```
Scene Fade In Duration: 1.5-2.0
Delay Before Drop: 0.8-1.0
```

**Cepat & Snappy:**
```
Scene Fade In Duration: 0.5-0.8
Delay Before Drop: 0.2-0.3
```

### B. Logo Corner Size
**Kecil (Watermark style):**
```
Corner Scale: 0.2-0.25
Corner Position: (350, -120)
```

**Sedang (Visible but not intrusive):**
```
Corner Scale: 0.3-0.35
Corner Position: (300, -100)
```

**Besar (Still prominent):**
```
Corner Scale: 0.4-0.5
Corner Position: (250, -80)
```

### C. Animation Speed
**Smooth & Elegant:**
```
Shrink Duration: 1.0-1.2
Drop Duration: 1.0
```

**Fast & Dynamic:**
```
Shrink Duration: 0.5-0.6
Drop Duration: 0.6-0.8
```

---

## 🧪 Testing Checklist

- [ ] Video Opening scene → Play → Video fade out smooth
- [ ] Black screen hold 0.5s sebelum load scene
- [ ] Main Menu load dengan fade in dari hitam
- [ ] Background visible dulu, delay 0.5s
- [ ] Logo drop dengan bounce setelah delay
- [ ] Logo clickable setelah 1s
- [ ] Klik logo → Logo shrink ke corner smooth
- [ ] Logo di corner position yang tepat (pojok kanan atas)
- [ ] Logo scale di corner pas (30% dari original)
- [ ] No errors di Console

---

## 🚨 Common Issues & Fixes

### Issue 1: Logo tidak bisa di-klik
**Fix:**
- Check ada **Event System** di scene
- Logo GameObject harus punya **LogoClickHandler** component
- Check **Enable Click** = ✓

### Issue 2: Logo position di corner salah
**Fix:**
- Adjust **Corner Position** di Inspector
- Test dengan berbagai resolusi (Game View → Free Aspect / 9:16)

### Issue 3: Fade in tidak smooth
**Fix:**
- Check **SceneFadeController** ada di scene
- Check **Auto Fade In On Start** = ✓
- Check fade panel created (auto-create harus jalan)

### Issue 4: Logo drop langsung tanpa delay
**Fix:**
- Pastikan pakai **AnimateDropInDelayed()** bukan **AnimateDropIn()**
- Check **Delay Before Drop** > 0

### Issue 5: Black screen terlalu cepat/lambat
**Fix:**
- Adjust **Black Screen Hold Duration** di OpeningVideoController
- Adjust **Scene Fade In Duration** di SceneFadeController

---

## 🎬 Advanced: Custom Corner Positions per Scene

Jika mau logo di posisi berbeda per scene:

```csharp
public class LogoPositionManager : MonoBehaviour
{
    [System.Serializable]
    public class SceneLogoPosition
    {
        public string sceneName;
        public Vector2 cornerPosition;
        public float cornerScale;
    }

    public List<SceneLogoPosition> scenePositions;
    public MenuAnimationController logoController;

    private void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        
        var position = scenePositions.Find(p => p.sceneName == currentScene);
        if (position != null)
        {
            logoController.cornerPosition = position.cornerPosition;
            logoController.cornerScale = position.cornerScale;
        }
    }
}
```

---

## 📱 Portrait Mode Notes

Saat migrasi ke portrait:
- **Adjust Corner Position** untuk portrait layout
- **Recommended portrait:** `(150, -150)` (lebih ke tengah karena layar sempit)
- **Corner Scale:** Bisa lebih kecil `0.25` karena layar lebih kecil

---

## ✨ Optional Enhancements

1. **Logo glow effect** saat hover (sebelum click)
2. **Sound effect** saat shrink
3. **Particle effect** saat logo settle di corner
4. **Double-click logo di corner** untuk restore ke tengah

---

Good luck! 🚀✨
