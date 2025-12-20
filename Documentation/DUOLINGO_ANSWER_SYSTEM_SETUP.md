# Duolingo-Style Answer System - Setup Guide

## 📖 Overview

System input jawaban seperti Duolingo untuk **10 soal pertama Chapter 1**. Pemain memilih kotak angka yang bergerak ke slot jawaban dengan animasi smooth.

### **Konsep:**
```
┌──────────────────────────────────────────────┐
│  Soal: Segitiga dengan sisi 8, 15, 17        │
│  Berapakah nilai Sin θ?                      │
└──────────────────────────────────────────────┘

        TEMPAT JAWABAN (Answer Slots):
      ┌────────┐          ┌────────┐
      │   15   │    /     │   17   │
      └────────┘          └────────┘
       Slot 1             Slot 2

═══════════════════════════════════════════════

        PILIHAN JAWABAN (Answer Pool):
      ┌────┐  ┌────┐  ┌────┐  ┌────┐  ┌────┐
      │ 15 │  │ 17 │  │ 8  │  │ 25 │  │ 6  │
      └────┘  └────┘  └────┘  └────┘  └────┘

            [VERIFY JAWABAN]
```

---

## 🎯 Fitur

✅ **Tap to Move:** Klik tile → Bergerak ke slot dengan animasi  
✅ **Auto-fill Left First:** Slot kiri diisi dulu, baru kanan  
✅ **Return to Pool:** Klik tile di slot → Kembali ke pool  
✅ **Auto-shift Left:** Jika slot kiri kosong, tile di kanan auto geser kiri  
✅ **Random Pool:** 2 correct + 3 distractor (acak setiap soal)  
✅ **Smooth Animation:** DOTween untuk gerakan & scale bounce  

---

## 📁 File Structure

```
Assets/
├─ Scripts/
│  ├─ UI/
│  │  └─ Chapter1/
│  │     ├─ AnswerTile.cs              # Kotak jawaban individual
│  │     ├─ DuolingoAnswerSystem.cs    # Manager untuk slot & pool
│  │     ├─ UIManagerChapter1.cs       # (existing)
│  │     └─ ButtonAnswerSystem.cs      # (deprecated - tidak dipakai)
│  │
│  └─ Data/
│     └─ Chapter1/
│        └─ TriangleDataGenerator.cs   # (modified - added GenerateDistractors)
│
└─ Prefabs/
   └─ UI/
      └─ AnswerTile.prefab             # Prefab untuk tile
```

---

## 🔧 Setup Instructions

### **Step 1: Buka Stage 1 Scene**

1. Open **Stage 1** scene (atau scene untuk Chapter 1)
2. Locate **Canvas** object di Hierarchy

---

### **Step 2: Buat UI Structure**

Buat struktur UI ini di Canvas:

```
Canvas
└─ QuestionPanel (existing)
   ├─ Question Text (existing)
   ├─ Triangle Visualization (existing)
   │
   ├─ DuolingoAnswerUI (New - Empty GameObject)
   │  │
   │  ├─ AnswerSlotsContainer (New - Empty GameObject)
   │  │  ├─ Slot1 (Empty GameObject)
   │  │  │  └─ Slot1Highlight (Optional - Image untuk border)
   │  │  │
   │  │  ├─ SlashText (TextMeshProUGUI - Text: "/")
   │  │  │
   │  │  └─ Slot2 (Empty GameObject)
   │  │     └─ Slot2Highlight (Optional - Image untuk border)
   │  │
   │  └─ AnswerPoolContainer (New - Empty GameObject)
   │     ├─ (Auto-populated dengan tiles dari prefab)
   │     │
   │     └─ Layout: Horizontal Layout Group
   │        - Spacing: 10
   │        - Child Force Expand: Width & Height
   │
   └─ VerifyButton (existing - Button dengan text "CHECK")
```

#### **Detail Settings:**

**AnswerSlotsContainer:**
- RectTransform: Width 300, Height 80
- Position: Above pool (~200 units)
- Layout: Horizontal Layout Group
  - Spacing: 20
  - Child Alignment: Middle Center
  - Child Control Size: ✓ Width, ✓ Height

**Slot1 & Slot2:**
- RectTransform: Width 100, Height 80
- Pivot: (0.5, 0.5)
- Background: Optional Image dengan border

**SlashText:**
- RectTransform: Width 30, Height 80
- Font Size: 48
- Alignment: Center
- Color: White

**AnswerPoolContainer:**
- RectTransform: Width 500, Height 100
- Position: Below slots (~-150 units)
- Horizontal Layout Group:
  - Spacing: 10
  - Child Force Expand: Width ✓, Height ✓

---

### **Step 3: Create AnswerTile Prefab**

1. **Create Button GameObject:**
   - Hierarchy → Right-click → UI → Button
   - Rename: `AnswerTile`

2. **Setup Visual:**
   - **Image (Background):**
     - Color: White
     - Add Outline component (optional)
   - **TextMeshProUGUI (Value):**
     - Child of Button
     - Font Size: 36
     - Alignment: Center
     - Color: Black

3. **Add AnswerTile Script:**
   - Select AnswerTile GameObject
   - Add Component → AnswerTile.cs
   - Assign:
     - **valueText** → TextMeshProUGUI
     - **button** → Button component
     - **background** → Image component

4. **Set Colors in Inspector:**
   - Normal Color: (1, 1, 1, 1) - White
   - Highlight Color: (0.9, 0.9, 1, 1) - Light Blue
   - Correct Color: (0.5, 1, 0.5, 1) - Light Green
   - Wrong Color: (1, 0.5, 0.5, 1) - Light Red

5. **Save as Prefab:**
   - Drag AnswerTile ke Project window → `Assets/Prefabs/UI/`
   - Delete dari Hierarchy (akan di-instantiate by system)

---

### **Step 4: Setup DuolingoAnswerSystem**

1. **Create GameObject:**
   - Hierarchy → Create Empty GameObject
   - Rename: `DuolingoAnswerSystem`
   - Parent: QuestionPanel atau Canvas

2. **Add Component:**
   - Add Component → DuolingoAnswerSystem.cs

3. **Assign References:**
   - **slot1Transform** → Drag **Slot1** GameObject
   - **slot2Transform** → Drag **Slot2** GameObject
   - **slashText** → Drag **SlashText** TextMeshProUGUI
   - **poolContainer** → Drag **AnswerPoolContainer**
   - **tilePrefab** → Drag **AnswerTile.prefab**
   - **hiddenInputField** → Drag **jawabanInput** dari UIManagerChapter1
     *(Find existing TMP_InputField di scene)*

4. **Settings:**
   - Pool Size: 5
   - Animation Duration: 0.3

---

### **Step 5: Integration dengan Game Manager**

Buka **CalculationManager.cs** atau script yang handle load soal baru.

#### **Tambahkan Code:**

```csharp
using UnityEngine;
using System.Collections.Generic;

public class CalculationManager : MonoBehaviour
{
    [SerializeField] private TriangleDataGenerator dataGenerator;
    [SerializeField] private DuolingoAnswerSystem duolingoSystem; // NEW
    
    private int currentQuestionNumber = 1;
    
    public void LoadNewQuestion()
    {
        // Generate soal
        TriangleData data = dataGenerator.GenerateQuestionByNumber(currentQuestionNumber);
        
        // Setup UI
        uiManager.SetupNewQuestion(currentQuestionNumber, 30, data);
        
        // TAMBAHKAN INI: Setup Duolingo system untuk soal 1-10
        if (currentQuestionNumber <= 10)
        {
            SetupDuolingoAnswer(data);
        }
        else
        {
            // Soal 11+ pakai input field biasa
            DisableDuolingoSystem();
        }
        
        currentQuestionNumber++;
    }
    
    /// <summary>
    /// Setup Duolingo answer system untuk pecahan
    /// </summary>
    private void SetupDuolingoAnswer(TriangleData data)
    {
        // Untuk Sin/Cos/Tan soal, jawaban adalah pecahan
        // Contoh: Sin θ = opposite/hypotenuse = Depan/Miring
        
        string numerator = "";
        string denominator = "";
        
        // Tentukan correct answer berdasarkan tipe soal
        switch (data.TypeSoal)
        {
            case QuestionType.FindSinValue:
                // Sin θ = opposite/hypotenuse = Depan/Miring
                numerator = data.Depan.ToString();
                denominator = data.Miring.ToString();
                break;
                
            case QuestionType.FindCosValue:
                // Cos θ = adjacent/hypotenuse = Samping/Miring
                numerator = data.Samping.ToString();
                denominator = data.Miring.ToString();
                break;
                
            case QuestionType.FindTanValue:
                // Tan θ = opposite/adjacent = Depan/Samping
                numerator = data.Depan.ToString();
                denominator = data.Samping.ToString();
                break;
                
            default:
                Debug.LogWarning($"Duolingo system not implemented for {data.TypeSoal}");
                return;
        }
        
        // Generate 3 distractor answers
        List<string> distractors = dataGenerator.GenerateDistractors(data);
        
        // Setup Duolingo system
        if (DuolingoAnswerSystem.Instance != null)
        {
            DuolingoAnswerSystem.Instance.SetupQuestion(numerator, denominator, distractors);
        }
        
        Debug.Log($"Duolingo Setup: {numerator}/{denominator}, Distractors: {string.Join(", ", distractors)}");
    }
    
    /// <summary>
    /// Disable Duolingo system untuk soal 11+
    /// </summary>
    private void DisableDuolingoSystem()
    {
        if (DuolingoAnswerSystem.Instance != null)
        {
            DuolingoAnswerSystem.Instance.gameObject.SetActive(false);
        }
        
        // Enable input field biasa
        uiManager.jawabanInput.gameObject.SetActive(true);
    }
}
```

#### **Update UIManagerChapter1.cs (Optional):**

Jika mau hide input field untuk soal 1-10:

```csharp
public void SetupNewQuestion(int progres, int totalSoal, TriangleData data)
{
    // ... existing code ...
    
    // Hide/Show input field based on question number
    if (progres <= 10)
    {
        // Soal 1-10: Pakai Duolingo system, hide input field
        jawabanInput.gameObject.SetActive(false);
    }
    else
    {
        // Soal 11+: Pakai input field biasa, hide Duolingo
        jawabanInput.gameObject.SetActive(true);
    }
}
```

---

### **Step 6: Verify Button Integration**

Pastikan Verify Button tetap berfungsi:

```csharp
public void OnVerifyButtonClicked()
{
    // Ambil jawaban dari input field (auto sync by Duolingo system)
    string userAnswer = uiManager.jawabanInput.text;
    
    // Validasi jawaban
    bool isCorrect = ValidateAnswer(userAnswer);
    
    // Highlight tiles (visual feedback)
    if (DuolingoAnswerSystem.Instance != null && 
        DuolingoAnswerSystem.Instance.IsAnswerComplete())
    {
        DuolingoAnswerSystem.Instance.HighlightAnswer(isCorrect);
    }
    
    // Show feedback panel
    ShowFeedback(isCorrect);
}
```

---

## 🎮 Gameplay Flow

### **Player Interaction:**

1. **Soal muncul** (contoh: Segitiga 8, 15, 17 - Sin θ?)
2. **Pool shows 5 tiles:** [15] [17] [8] [25] [6] (random order)
3. **Player taps [15]:**
   - Tile [15] bergerak ke **Slot1** (kiri)
   - Display: `15 / ___`
   - Input field sync: `15/`
4. **Player taps [17]:**
   - Tile [17] bergerak ke **Slot2** (kanan)
   - Display: `15 / 17`
   - Input field sync: `15/17`
5. **Player clicks VERIFY:**
   - System check jawaban
   - Tiles highlight (green = correct, red = wrong)
   - Feedback panel muncul

### **Undo Actions:**

**Scenario 1: Salah tap tile (17 dulu, harusnya 15)**
- Player taps [17] → Slot1: `17 / ___`
- Player taps **[17] di slot** → Tile kembali ke pool
- Player taps [15] → Slot1: `15 / ___`
- Player taps [17] → Slot2: `15 / 17` ✅

**Scenario 2: Auto-shift kiri**
- Slot1: [15], Slot2: [17] → `15 / 17`
- Player taps **[15] di slot** → Kembali ke pool
- **[17] auto geser kiri** → Slot1: `17 / ___`

---

## 🎨 Visual Customization

### **Colors:**

Edit di **AnswerTile** script atau Inspector:
```csharp
public Color normalColor = Color.white;
public Color highlightColor = new Color(0.9f, 0.9f, 1f);
public Color correctColor = new Color(0.5f, 1f, 0.5f);
public Color wrongColor = new Color(1f, 0.5f, 0.5f);
```

### **Animation:**

Edit di **DuolingoAnswerSystem** Inspector:
- **animationDuration:** 0.3f (default)
- Faster: 0.2f
- Slower: 0.5f

Edit di **AnswerTile.cs** untuk tweaking:
```csharp
// Line ~93: Scale bounce
transform.DOScale(1.1f, duration * 0.5f) // Bounce size
```

---

## 🐛 Troubleshooting

### **Tiles tidak bergerak:**
- ✅ Check DOTween installed (Package Manager)
- ✅ Check slot1Transform & slot2Transform assigned
- ✅ Check Console untuk error

### **Jawaban tidak tersync ke input field:**
- ✅ Check hiddenInputField reference di DuolingoAnswerSystem
- ✅ Pastikan jawabanInput masih aktif (atau minimal component enabled)

### **Pool tiles overlap:**
- ✅ Check AnswerPoolContainer punya Horizontal Layout Group
- ✅ Set spacing = 10
- ✅ Check Child Force Expand: Width ✓

### **Tiles stuck di slot:**
- ✅ Check AnswerTile.button assigned
- ✅ Check onClick listener di AnswerTile.Awake()
- ✅ Test dengan Debug.Log di OnTileClicked()

### **Wrong answer generated:**
- ✅ Check GenerateDistractors() logic di TriangleDataGenerator
- ✅ Pastikan numerator/denominator sesuai dengan tipe soal
- ✅ Debug log di SetupDuolingoAnswer()

---

## 📊 Testing Checklist

### **Functional Testing:**
- [ ] Soal 1-10 show Duolingo system
- [ ] Soal 11+ show input field biasa
- [ ] 5 tiles muncul di pool (2 correct + 3 distractor)
- [ ] Tap tile → Bergerak ke slot1 (kiri)
- [ ] Tap tile lagi → Bergerak ke slot2 (kanan)
- [ ] Kedua slot penuh → Tile ketiga tidak bergerak
- [ ] Tap tile di slot → Kembali ke pool
- [ ] Slot1 kosong + slot2 terisi → Auto shift kiri
- [ ] Verify button → Highlight tiles (green/red)
- [ ] Jawaban sync ke input field
- [ ] Next question → Pool reset + new tiles

### **Visual Testing:**
- [ ] Animasi smooth (no jittering)
- [ ] Scale bounce effect visible
- [ ] Colors correct (normal/highlight/correct/wrong)
- [ ] Layout responsive (mobile resolution)
- [ ] Slash "/" visible di tengah slots

### **Edge Cases:**
- [ ] Double-click tile (no duplicate move)
- [ ] Click slot saat empty (no error)
- [ ] Click verify saat jawaban incomplete (validation)
- [ ] Distractor tidak sama dengan correct answer
- [ ] Pool tidak ada duplicate values

---

## 🚀 Next Steps

### **For 10+ Questions:**
Setelah 10 soal pertama selesai, kembali ke input field biasa atau:
- Extend Duolingo system untuk desimal (button: 0-9, .)
- Add calculator-style input untuk soal kompleks
- Add hint system (show partial answer)

### **Enhancement Ideas:**
- Sound effect saat tile bergerak
- Particle effect saat correct answer
- Timer untuk speed challenge
- Streak counter (consecutive correct)
- Achievement badges

---

## 📝 Notes

- **Jawaban untuk 10 soal pertama:** Format pecahan `numerator/denominator`
- **Tile values:** Integer only (1-2 digit, contoh: 8, 15, 17, 25)
- **Answer validation:** Existing system di CalculationManager tetap dipakai
- **Backward compatibility:** Input field tetap tersync untuk legacy code

---

## 🔗 References

**Related Files:**
- `AnswerTile.cs` - Tile component
- `DuolingoAnswerSystem.cs` - Manager
- `TriangleDataGenerator.cs` - Question & distractor generator
- `UIManagerChapter1.cs` - UI coordinator
- `CalculationManager.cs` - Game logic

**Dependencies:**
- DOTween (Animation)
- TextMeshPro (UI Text)
- Unity UI (Button, Image)

---

**Last Updated:** December 20, 2025  
**Version:** 1.0  
**Author:** GitHub Copilot AI Assistant
