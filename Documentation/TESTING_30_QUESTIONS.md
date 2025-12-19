# 🧪 Testing Guide - 30 Questions System

## ✅ Pre-Testing Checklist

### **Files Modified:**
- ✅ `TriangleDataGenerator.cs` - Added 7 question types, 12 triples, rotation support
- ✅ `TriangleVisualizer.cs` - Added rotation functionality with RotatePoint()
- ✅ `CalculationManager.cs` - Changed totalSoal from 5 to 30
- ✅ `UIManagerChapter1.cs` - Updated to use PertanyaanText and pass rotation angle

### **New Features:**
1. **Progressive Difficulty** - Easy (1-10), Medium (11-20), Hard (21-30)
2. **Triangle Rotation** - 0°, 90°, 180°, 270° based on difficulty
3. **7 Question Types** - FindSin/Cos/Tan, FindOpposite/Adjacent/Hypotenuse, FindPythagorean
4. **12 Pythagorean Triples** - From simple (3,4,5) to complex (13,84,85)

---

## 🎮 Testing Scenarios

### **Test 1: Easy Questions (1-10)**
**Expected Behavior:**
- ✅ All questions are basic trig (Sin/Cos/Tan)
- ✅ No rotation (0° only)
- ✅ Uses simple triples: (3,4,5), (5,12,13), (8,15,17), (7,24,25)
- ✅ Questions cycle through Sin → Cos → Tan → Sin...

**How to Test:**
1. Start Chapter 1
2. Play through questions 1-10
3. Verify:
   - Question text: "Berapakah nilai Sinθ/Cosθ/Tanθ?"
   - Triangle orientation: Standard (theta bottom-left)
   - Console log: `Difficulty: Easy | Rotation: 0°`

**Sample Answers:**
```
Soal 1: 0.6 (or 3/5)
Soal 2: 0.385 (or 5/13)
Soal 3: 1.875 (or 15/8)
```

---

### **Test 2: Medium Questions (11-20)**
**Expected Behavior:**
- ✅ Mix of basic trig and inverse problems
- ✅ Rotation: 0° and 90° alternating
- ✅ Some questions ask to find sides (not just ratios)
- ✅ Uses triples including multiples: (6,8,10), (9,12,15), etc.

**How to Test:**
1. Continue to questions 11-20
2. Verify:
   - Every 3rd question is inverse type (Find Opposite/Adjacent/Hypotenuse)
   - Triangle rotates every other question (90° on even numbers)
   - Question text includes "Jika Sinθ = ... dan sisi miring = ..."
   - Console log: `Difficulty: Medium | Rotation: 0°/90°`

**Sample Answers:**
```
Soal 12: 5 (integer answer for inverse problem)
Soal 15: 8 (Samping from Cos θ and miring)
Soal 18: 25 (Miring from Sin θ and depan)
```

---

### **Test 3: Hard Questions (21-30)**
**Expected Behavior:**
- ✅ All rotation angles: 0°, 90°, 180°, 270°
- ✅ Pythagorean questions appear
- ✅ Uses complex triples: (20,21,29), (9,40,41), (11,60,61), (13,84,85)
- ✅ Question types vary significantly

**How to Test:**
1. Continue to questions 21-30
2. Verify:
   - Triangle rotates through all 4 orientations
   - Questions include Pythagorean type: "Jika sisi depan = ... dan sisi samping = ..."
   - Theta label position changes with rotation
   - Console log: `Difficulty: Hard | Rotation: 0°/90°/180°/270°`

**Sample Answers:**
```
Soal 21: 29 (Pythagorean: find hypotenuse)
Soal 25: 20 (Pythagorean: find adjacent)
Soal 29: 21 (Pythagorean: find opposite)
```

---

## 🔍 Visual Verification

### **Rotation Check:**

**0° (Standard):**
```
    |\
    | \  θ di kiri bawah
    |__\
```
✅ Theta label: Bottom-left corner  
✅ Depan: Vertical line (left)  
✅ Samping: Horizontal line (bottom)  
✅ Miring: Diagonal line  

**90° (Rotated CW):**
```
    ___
    \  |  θ di kiri atas
     \ |
      \|
```
✅ Theta label: Top-left corner  
✅ Segitiga miring ke kanan  
✅ Label posisi berubah sesuai rotasi  

**180° (Inverted):**
```
       /|
      / |  θ di kanan atas
     /__|
```
✅ Theta label: Top-right corner  
✅ Segitiga terbalik  
✅ Miring: Diagonal ke kiri-bawah  

**270° (Rotated CCW):**
```
    |
    |\  θ di kanan bawah
    | \
    |__\
```
✅ Theta label: Bottom-right corner  
✅ Segitiga miring ke kiri  
✅ Depan: Horizontal (bottom)  

---

## 🐛 Common Issues & Solutions

### **Issue 1: Triangle Overflow (Large Numbers)**
**Symptom:** Triangle with large values (e.g., 84, 85) exceeds screen bounds

**Solution:** Auto-scaling should handle this
```csharp
// In TriangleVisualizer.cs
float safeMaxSize = maxTriangleSize - safetyMargin; // 8 - 1 = 7
float scaleByWidth = widthNeeded > safeMaxSize ? safeMaxSize / samping : baseScale;
float scaleByHeight = heightNeeded > safeMaxSize ? safeMaxSize / depan : baseScale;
dynamicScale = Mathf.Min(scaleByWidth, scaleByHeight);
```

**Test:** Question 24 uses triple (13, 84, 85) - verify it fits on screen

---

### **Issue 2: Wrong Question Generated**
**Symptom:** Question doesn't match difficulty level

**Check:**
```csharp
// In TriangleDataGenerator.cs
public TriangleData GenerateQuestionByNumber(int questionNumber)
{
    // Verify this is being called, not GenerateNewQuestion()
    if (questionNumber <= 10)
        difficulty = DifficultyLevel.Easy;
    else if (questionNumber <= 20)
        difficulty = DifficultyLevel.Medium;
    else
        difficulty = DifficultyLevel.Hard;
}
```

**Test:** Check console log for correct difficulty assignment

---

### **Issue 3: Rotation Not Visible**
**Symptom:** Triangle doesn't rotate even on hard questions

**Check:**
```csharp
// In UIManagerChapter1.cs - SetupNewQuestion()
triangleVisualizer.DrawTriangle(data.Depan, data.Samping, data.Miring, data.RotationAngle);
// NOT: DrawTriangle(depan, samping, miring) without rotation!
```

**Test:** Question 11+ should show rotation in console log

---

### **Issue 4: Theta Label Wrong Position**
**Symptom:** Theta (θ) not at the right angle corner after rotation

**Check:**
```csharp
// In TriangleVisualizer.cs
Vector3 toRight = (bottomRight - bottomLeft).normalized;
Vector3 toTop = (topLeft - bottomLeft).normalized;
Vector3 inward = (toRight + toTop).normalized;
Vector3 thetaPosition = bottomLeft + inward * thetaOffsetDistance;
```

**Test:** Verify theta is always at the 90° corner, inside the triangle

---

### **Issue 5: Wrong Answer Tolerance**
**Symptom:** Correct answer marked as wrong (or vice versa)

**Current Tolerance:** ±0.01

**Test Cases:**
- Answer: 0.6, Input: 0.6 → ✅ Correct
- Answer: 0.6, Input: 0.605 → ✅ Correct (within tolerance)
- Answer: 0.6, Input: 3/5 → ✅ Correct (fraction support)
- Answer: 5, Input: 5 → ✅ Correct (integer)
- Answer: 5, Input: 5.009 → ✅ Correct (within tolerance)

---

## 📊 Console Log Verification

### **Expected Logs:**

**Question 1 (Easy):**
```
[Chapter1] Soal #1/30 | Difficulty: Easy | Rotation: 0°
[TriangleVisualizer] Auto-scaling: 0.50 → 0.50 (W:4, H:3 → Final: 2.0 x 1.5 units, Max allowed: 7.0)
```

**Question 12 (Medium, Inverse):**
```
[Chapter1] Soal #12/30 | Difficulty: Medium | Rotation: 0°
PertanyaanText: "Jika Sinθ = 0.38 dan sisi miring = 13, berapa panjang sisi depan?"
InfoTambahan: "Sinθ = 0.38"
```

**Question 21 (Hard, Pythagorean):**
```
[Chapter1] Soal #21/30 | Difficulty: Hard | Rotation: 90°
PertanyaanText: "Jika sisi depan = 20 dan sisi samping = 21, berapa panjang sisi miring?"
InfoTambahan: "Teorema Pythagoras: c² = a² + b²"
```

**Question 24 (Hard, Large Triple):**
```
[Chapter1] Soal #24/30 | Difficulty: Hard | Rotation: 0°
[TriangleVisualizer] Auto-scaling: 0.50 → 0.08 (W:13, H:84 → Final: 1.1 x 6.9 units, Max allowed: 7.0)
```

---

## 🎯 Acceptance Criteria

### **✅ Must Pass:**
1. All 30 questions generate without errors
2. Progressive difficulty works (Easy → Medium → Hard)
3. Rotation changes correctly based on difficulty
4. Auto-scaling prevents overflow for all triples
5. Theta label always at right angle corner
6. Question text displays correctly (no missing characters)
7. Answer validation works for all question types
8. Score increments by 10 for each correct answer
9. Lives decrement on wrong answers
10. Game ends at question 30 or when lives = 0

### **✅ Should Pass:**
1. Smooth transition between questions
2. Visual feedback (correct/wrong) displays properly
3. Labels (depan, samping, miring) positioned correctly after rotation
4. No Z-fighting or sprite overlap
5. Performance stable (60 FPS) throughout all 30 questions

### **✅ Nice to Have:**
1. Rotation animation when triangle rotates
2. Difficulty badge shown (Easy/Medium/Hard)
3. Hint system for stuck players
4. Progress bar showing 1/30, 2/30... 30/30

---

## 🚀 Quick Test Script

**Run this in Unity Play Mode:**
```csharp
// In Unity Console, type these commands:

// Test Question 1 (Easy, basic)
GameObject.Find("GameManager").GetComponent<CalculationManager>().StartNewRound();

// Skip to Question 11 (Medium, rotation)
for (int i = 0; i < 10; i++) {
    // Answer correctly to skip
    CalculationManager cm = GameObject.Find("GameManager").GetComponent<CalculationManager>();
    cm.VerifyAnswer(); // with correct answer input
}

// Skip to Question 21 (Hard, Pythagorean)
// Repeat above loop to reach question 21
```

---

## 📝 Test Report Template

```
Date: __________
Unity Version: 6.0 (6000.0.23f1)
Build: ________

Test Results:

✅ Easy Questions (1-10):
   - All basic trig: [ PASS / FAIL ]
   - No rotation: [ PASS / FAIL ]
   - Simple triples only: [ PASS / FAIL ]

✅ Medium Questions (11-20):
   - Rotation 0°/90°: [ PASS / FAIL ]
   - Inverse problems: [ PASS / FAIL ]
   - Medium triples: [ PASS / FAIL ]

✅ Hard Questions (21-30):
   - All rotations: [ PASS / FAIL ]
   - Pythagorean questions: [ PASS / FAIL ]
   - Complex triples: [ PASS / FAIL ]

✅ Auto-Scaling:
   - Triple (13, 84, 85): [ PASS / FAIL ]
   - Triple (11, 60, 61): [ PASS / FAIL ]
   - No overflow: [ PASS / FAIL ]

✅ Rotation Visual:
   - 0°: [ PASS / FAIL ]
   - 90°: [ PASS / FAIL ]
   - 180°: [ PASS / FAIL ]
   - 270°: [ PASS / FAIL ]

✅ Theta Position:
   - Always at right angle: [ PASS / FAIL ]
   - Inside triangle: [ PASS / FAIL ]

Issues Found:
1. ___________________________
2. ___________________________
3. ___________________________

Overall Status: [ READY / NEEDS FIX ]
```

---

**Happy Testing! 🎮📐**
