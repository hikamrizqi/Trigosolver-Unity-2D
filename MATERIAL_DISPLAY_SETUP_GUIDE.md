# 📚 Material Display Setup Guide

## 🎯 Overview

Sistem untuk menampilkan 2 gambar materi di main menu dengan navigasi:
- **Klik 1:** Tampilkan gambar materi 1
- **Klik 2:** Tampilkan gambar materi 2  
- **Klik 3:** Kembali ke main menu

---

## 🏗️ Structure Setup

### Step 1: Create Material Panel Hierarchy

```
Canvas (Main Menu Scene)
└── MaterialPanel
    ├── MaterialDisplayController (Script)
    ├── MaterialImage1
    │   └── Image (Component)
    └── MaterialImage2
        └── Image (Component)
```

### Detailed Setup:

1. **Create MaterialPanel GameObject:**
   ```
   Hierarchy → Right-click Canvas → Create Empty
   Name: "MaterialPanel"
   
   RectTransform Settings:
   - Anchors: Stretch-Stretch (full screen)
   - Left: 0, Right: 0, Top: 0, Bottom: 0
   - Scale: (1, 1, 1)
   ```

2. **Create MaterialImage1:**
   ```
   Right-click MaterialPanel → UI → Image
   Name: "MaterialImage1"
   
   RectTransform Settings:
   - Anchors: Middle-Center
   - Width: 1080 (adjust to your image size)
   - Height: 720 (adjust to your image size)
   - Pos X: 0, Pos Y: 0
   
   Image Component:
   - Source Image: [Drag your first material image here]
   - Color: (255, 255, 255, 255)
   - Preserve Aspect: ✅ (recommended)
   - Raycast Target: ✅ (untuk detect click)
   ```

3. **Create MaterialImage2:**
   ```
   Right-click MaterialPanel → UI → Image
   Name: "MaterialImage2"
   
   RectTransform Settings:
   - Same as MaterialImage1
   
   Image Component:
   - Source Image: [Drag your second material image here]
   - Color: (255, 255, 255, 255)
   - Preserve Aspect: ✅
   - Raycast Target: ✅
   ```

---

## 🔧 Component Configuration

### Step 2: Add MaterialDisplayController Script

1. **Select MaterialPanel**
2. **Add Component → MaterialDisplayController**
3. **Configure Inspector:**

```
Material Images:
├── Material Image 1: [Drag MaterialImage1 GameObject here]
└── Material Image 2: [Drag MaterialImage2 GameObject here]

Navigation Settings:
├── Enable Click Anywhere: ✅ true
├── Next Key: Space
└── Back Key: Escape

Animation Settings:
├── Fade Duration: 0.3
└── Scale Duration: 0.3

UI References:
├── Image 1 Component: [Drag MaterialImage1's Image component here]
└── Image 2 Component: [Drag MaterialImage2's Image component here]
```

**Tips:**
- Untuk assign Image component: Select MaterialImage1 → Inspector → Image component → Drag ke field
- Atau biarkan kosong, script akan auto-detect

---

## 🔗 Integration with MainMenuManager

### Step 3: Update MainMenuManager

1. **Select MainMenuManager GameObject** (biasanya di Main Menu Canvas)
2. **Find MainMenuManager component di Inspector**
3. **Assign Material Display references:**

```
Material Display:
├── Material Panel: [Drag MaterialPanel GameObject here]
└── Material Display Controller: [Drag MaterialDisplayController component here]
```

**Cara assign controller component:**
- Select MaterialPanel di Hierarchy
- Drag MaterialDisplayController script dari Inspector ke MainMenuManager field

---

## 🎨 Button Setup (Tombol Materi di Main Menu)

### Step 4: Create or Modify Material Button

1. **Find atau create button "Materi" di Main Menu Panel:**
   ```
   Main Menu Panel → MateriButton (or create new Button)
   ```

2. **Configure Button onClick event:**
   ```
   Inspector → Button Component → OnClick()
   
   Click '+' to add event:
   - Runtime: MainMenuManager GameObject
   - Function: MainMenuManager.OnMateriClicked()
   ```

3. **Button position example:**
   ```
   Biasanya di main menu ada:
   - Mulai
   - Materi ← Tombol ini
   - High Score
   - Keluar
   ```

---

## 🎭 Visual Design Tips

### Material Images Best Practices:

**Image Size:**
- Recommended: 1920x1080 (16:9 ratio)
- Minimum: 1280x720
- Format: PNG (dengan transparency) atau JPG

**Content Guidelines:**
```
Gambar 1: Materi Trigonometri Dasar
- Sin, Cos, Tan definitions
- Unit circle diagram
- Common angles table

Gambar 2: Rumus-rumus Penting
- Identitas trigonometri
- Rumus sudut ganda
- Contoh soal
```

**Design Tips:**
- Use high contrast colors untuk readability
- Include diagrams and visual aids
- Keep text readable (min 24pt font)
- Add padding around content (50px margin)

---

## 🎬 Animation Behavior

### Navigation Flow:

```
Main Menu
    ↓ (Click "Materi")
[Main Menu Sink Out Animation]
    ↓
MaterialImage1 Appears
├── Fade In (0.3s)
└── Scale In (0.3s, OutBack ease)
    ↓ (Click anywhere / Space)
MaterialImage1 → MaterialImage2 Transition
├── Image1 Fade Out + Scale Down (0.3s)
└── Image2 Fade In + Scale In (0.3s)
    ↓ (Click anywhere / Space)
MaterialImage2 → Close
├── Fade Out + Scale In (0.3s, InBack ease)
└── Return to Main Menu
    ↓
[Main Menu Drop In Animation]
```

### Alternative Navigation:
- **ESC key:** Go back one step
  - From Image1: Close → Main Menu
  - From Image2: Back to Image1
- **Space key:** Next step (same as click)

---

## 🧪 Testing Checklist

### Test Sequence:

1. **Test Basic Flow:**
   - [ ] Click "Materi" button in main menu
   - [ ] Verify main menu sinks out
   - [ ] Verify MaterialImage1 appears with animation
   - [ ] Click anywhere → MaterialImage2 appears
   - [ ] Click anywhere → Return to main menu

2. **Test Animations:**
   - [ ] Images fade in smoothly
   - [ ] Scale animation has bounce effect (OutBack)
   - [ ] Transitions are smooth (no flicker)
   - [ ] Main menu properly animates back in

3. **Test Navigation:**
   - [ ] Click anywhere works
   - [ ] Space key works for next
   - [ ] ESC key goes back
   - [ ] No response during transitions (isTransitioning flag)

4. **Test Edge Cases:**
   - [ ] Rapid clicking doesn't break state
   - [ ] Images display at correct resolution
   - [ ] No memory leaks (images properly disposed)
   - [ ] Console shows proper debug logs

---

## 🐛 Troubleshooting

### Issue: Images not showing

**Check:**
1. MaterialPanel.SetActive(true) called?
2. Image sprites assigned in Inspector?
3. Canvas sorting order correct?
4. Images not behind other panels?

**Solution:**
```csharp
// In Unity Console, check for:
[MaterialDisplay] ShowMaterial called
[MaterialDisplay] ShowImage1
```

---

### Issue: Click not working

**Check:**
1. Image component has "Raycast Target" enabled?
2. MaterialPanel has GraphicRaycaster?
3. EventSystem exists in scene?
4. isTransitioning = false?

**Solution:**
- Add EventSystem if missing: GameObject → UI → Event System
- Ensure Canvas has GraphicRaycaster component

---

### Issue: Animation jittery/laggy

**Check:**
1. DOTween properly imported?
2. Images too large (file size)?
3. Multiple animations running?

**Solution:**
- Optimize image file sizes (compress)
- Use POT (Power of Two) texture sizes
- Check DOTween.Kill() calls in OnDisable

---

### Issue: Return to main menu doesn't work

**Check:**
1. OnMaterialClosed callback assigned?
2. MainMenuManager reference correct?
3. currentState = MenuState.Material?

**Debug:**
```csharp
// Add in MaterialDisplayController.CloseMaterial():
Debug.Log("Invoking OnMaterialClosed: " + (OnMaterialClosed != null));

// Add in MainMenuManager.OnMaterialClosed():
Debug.Log("OnMaterialClosed received, currentState: " + currentState);
```

---

## 📊 Inspector Settings Reference

### MaterialDisplayController Settings:

| Field | Type | Default | Description |
|-------|------|---------|-------------|
| Material Image 1 | GameObject | null | First material image |
| Material Image 2 | GameObject | null | Second material image |
| Enable Click Anywhere | bool | true | Click to navigate |
| Next Key | KeyCode | Space | Keyboard next |
| Back Key | KeyCode | Escape | Keyboard back |
| Fade Duration | float | 0.3 | Fade transition time |
| Scale Duration | float | 0.3 | Scale transition time |
| Image 1 Component | Image | null | Auto-detected if null |
| Image 2 Component | Image | null | Auto-detected if null |

### MainMenuManager New Fields:

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| Material Panel | GameObject | ✅ | MaterialPanel root |
| Material Display Controller | MaterialDisplayController | ✅ | Controller script |

---

## 🎯 Quick Setup Checklist

### Before Testing:

- [ ] MaterialPanel created in hierarchy
- [ ] MaterialImage1 and MaterialImage2 created
- [ ] Images assigned to Image components
- [ ] MaterialDisplayController attached to MaterialPanel
- [ ] References assigned in MaterialDisplayController
- [ ] MainMenuManager references assigned
- [ ] Materi button onClick event configured
- [ ] MaterialPanel initially inactive
- [ ] DOTween imported in project

### First Test:

1. Run game
2. Click "Materi" button
3. Verify image 1 appears
4. Click → Image 2 appears
5. Click → Back to main menu

**Expected Console Output:**
```
[MainMenu] OnMateriClicked called
[MainMenu] Main menu sink complete, showing material display
[MaterialDisplay] ShowMaterial called
[MaterialDisplay] ShowImage1
[MaterialDisplay] OnNextClicked - Current index: 0
[MaterialDisplay] TransitionToImage2
[MaterialDisplay] OnNextClicked - Current index: 1
[MaterialDisplay] CloseMaterial
[MaterialDisplay] Material closed, notify main menu
[MainMenu] OnMaterialClosed called
```

---

## 🎨 Example Material Images

### Image 1 - Trigonometri Dasar:
```
Content suggestions:
- Judul: "Materi Trigonometri - Dasar"
- Definisi Sin, Cos, Tan
- Gambar segitiga siku-siku
- Lingkaran satuan
- Tabel nilai sudut istimewa (0°, 30°, 45°, 60°, 90°)
```

### Image 2 - Rumus & Identitas:
```
Content suggestions:
- Judul: "Rumus Trigonometri"
- Identitas dasar: sin²θ + cos²θ = 1
- Rumus sudut ganda
- Rumus penjumlahan
- Contoh soal sederhana
```

---

## 📝 Code Integration Examples

### Example: Custom Navigation Button

If you want custom next/back buttons instead of click anywhere:

```csharp
// In MaterialDisplayController, add public methods:

public void OnCustomNextButtonClicked()
{
    OnNextClicked();
}

public void OnCustomBackButtonClicked()
{
    OnBackClicked();
}
```

Then in Unity:
```
Button (Next) → onClick → MaterialDisplayController.OnCustomNextButtonClicked()
Button (Back) → onClick → MaterialDisplayController.OnCustomBackButtonClicked()
```

### Example: Add Page Indicator

```csharp
// Add to MaterialDisplayController:
[SerializeField] private TextMeshProUGUI pageIndicator;

private void UpdatePageIndicator()
{
    if (pageIndicator != null)
    {
        pageIndicator.text = $"{currentImageIndex + 1} / 2";
    }
}

// Call in ShowImage1(), TransitionToImage2(), etc.
```

---

## 🔗 Related Documentation

- [MainMenuManager.cs](../Scripts/Main Menu/MainMenuManager.cs)
- [MaterialDisplayController.cs](../Scripts/UI/MaterialDisplayController.cs)
- [DOTween Documentation](http://dotween.demigiant.com/documentation.php)

---

## ✅ Setup Complete!

Your material display system is ready! Users can now:
- ✅ View educational material images
- ✅ Navigate between 2 material pages
- ✅ Return to main menu easily
- ✅ Enjoy smooth animations

**Next Steps:**
1. Prepare your material image assets
2. Import into Unity project
3. Assign to MaterialImage1 and MaterialImage2
4. Test the complete flow
5. Adjust animation timings if needed

🎉 **Happy Teaching!**
