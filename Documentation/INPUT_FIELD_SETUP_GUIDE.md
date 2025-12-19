# 📝 Panduan Setup Input Field - Chapter 1

## Komponen Input Field yang Perlu Diisi

Berdasarkan screenshot Inspector Anda, berikut adalah panduan lengkap untuk setup **TMP_InputField** (TextMeshPro Input Field):

---

## 🎯 Lokasi Input Field

```
Hierarchy:
Canvas
└── QuestionPanel
    └── AnswerInputField (TMP_InputField)
```

---

## ⚙️ Konfigurasi Inspector - TMP_InputField Component

### **1. Rect Transform**
```
Anchors: Middle-Center
Pos X: 0
Pos Y: -50 (atau sesuaikan)
Width: 400
Height: 60
```

### **2. Input Field Component Settings**

#### **A. Interactable** ✅
```
☑ Interactable
```
> Pastikan checkbox ini AKTIF agar user bisa mengetik

---

#### **B. Transition**
```
Transition: Color Tint
```

**Target Graphic:**
```
Target Graphic: Background (Image)
```
> Drag komponen Image dari child "Background" ke field ini

**Colors:**
```
Normal Color:   RGB(255, 255, 255) - White
Highlighted:    RGB(245, 245, 245) - Light Gray  
Pressed Color:  RGB(200, 200, 200) - Gray
Selected Color: RGB(245, 245, 245) - Light Gray
Disabled Color: RGB(200, 200, 200, 128) - Semi-transparent Gray
Color Multiplier: 1
Fade Duration: 0.1
```

---

#### **C. Navigation**
```
Navigation: Automatic
```
> Biarkan default "Automatic" untuk keyboard/gamepad navigation

---

#### **D. Text Component (Input)**

**Text Component:**
```
Text Component: Text Area > Text (TMP)
```
> Drag child object "Text Area/Text" (TextMeshProUGUI) ke field ini

**Text:**
```
Text: (kosongkan - akan diisi oleh user)
```

**Character Limit:**
```
Character Limit: 0
```
> 0 = unlimited. Bisa set 10 jika ingin batasi panjang input

**Content Type:**
```
Content Type: Standard
```
> **PENTING:** Gunakan "Standard" bukan "Decimal Number" 
> Agar bisa terima pecahan seperti "3/5"

**Line Type:**
```
Line Type: Single Line
```

---

#### **E. Placeholder**

**Placeholder:**
```
Placeholder: Placeholder (TMP)
```
> Drag child object "Text Area/Placeholder" (TextMeshProUGUI) ke field ini

**Placeholder Text:**
Di komponen TextMeshProUGUI dari Placeholder, isi:
```
Text: "Masukkan jawaban (misal: 0.6 atau 3/5)"
Font Size: 24
Color: RGB(200, 200, 200) - Light Gray dengan Alpha 128
Alignment: Left & Middle
```

---

#### **F. Caret Settings**

**Caret Blink Rate:**
```
Caret Blink Rate: 0.85
```

**Caret Width:**
```
Caret Width: 1
```

**Custom Caret Color:**
```
☑ Enabled
Selection Color: RGB(168, 206, 255) - Light Blue
```
> Warna biru muda untuk text selection

---

#### **G. Input Behavior**

**Read Only:**
```
☐ Read Only (TIDAK dicentang)
```
> Harus TIDAK dicentang agar bisa diketik

**Should Activate On Select:**
```
☑ Should Activate On Select
```
> Centang agar keyboard muncul otomatis saat diklik (mobile support)

---

#### **H. Events**

**On Value Changed (String):**
```
List is Empty (kosongkan dulu)
```
> Opsional: Bisa digunakan untuk real-time validation

**On End Edit (String):**
```
☐ List is Empty

Runtime Only
GameObject: GameManager
Function: InputFieldHandler > OnInputFieldEndEdit(string)
```

**Cara Setup On End Edit:**
1. Klik tombol **[+]** di bawah "On End Edit (String)"
2. Drag GameObject **GameManager** ke field "None (Object)"
3. Dari dropdown, pilih: **InputFieldHandler → OnInputFieldEndEdit**

---

## 🎨 Child Objects yang Dibutuhkan

Input Field harus punya struktur child seperti ini:

```
AnswerInputField (TMP_InputField)
├── Text Area (RectMask2D)
│   ├── Placeholder (TextMeshProUGUI)
│   │   └── Text: "Masukkan jawaban (misal: 0.6 atau 3/5)"
│   │       Font Size: 24
│   │       Color: Gray (200, 200, 200, 128)
│   │
│   └── Text (TextMeshProUGUI)
│       └── Font Size: 28
│           Color: Black (0, 0, 0, 255)
│           Alignment: Left & Middle
│
└── Background (Image)
    └── Source Image: InputFieldBackground (UI Sprite)
        Color: White
```

---

## 📝 Setup Detail Setiap Child

### **1. Text Area (RectMask2D)**
```
Component: RectMask2D (untuk mask overflow text)

Rect Transform:
- Anchors: Stretch (All)
- Left: 10
- Top: 6
- Right: 10
- Bottom: 7
```

### **2. Placeholder (TextMeshProUGUI)**
```
Text: "Masukkan jawaban (misal: 0.6 atau 3/5)"
Font: LiberationSans SDF (default TMP font)
Font Style: Normal
Font Size: 24
Color: RGB(200, 200, 200) Alpha: 128 (semi-transparent gray)
Alignment: Left & Middle
Wrapping: Disabled
Overflow: Overflow
```

### **3. Text (TextMeshProUGUI) - Actual Input Text**
```
Text: (kosong)
Font: LiberationSans SDF
Font Style: Normal
Font Size: 28
Color: RGB(0, 0, 0) - Black
Alignment: Left & Middle
Wrapping: Disabled
Overflow: Overflow
```

### **4. Background (Image)**
```
Source Image: UISprite (atau InputFieldBackground)
Color: White RGB(255, 255, 255)
Material: None (Material)
Raycast Target: ☑ (harus aktif agar bisa diklik)

Image Type: Sliced (jika pakai 9-slice sprite)
Fill Center: ☑
```

---

## 🔗 Script References

Pastikan di **InputFieldHandler.cs** (attach ke GameManager):

```csharp
[SerializeField] private TMP_InputField inputField;
[SerializeField] private CalculationManager calculationManager;
```

**Cara Assign di Inspector GameManager:**

1. Select GameObject **GameManager**
2. Di component **InputFieldHandler**:
   ```
   Input Field: [Drag AnswerInputField dari Canvas]
   Calculation Manager: [Drag CalculationManager component dari GameManager sendiri]
   ```

---

## ✅ Checklist Setup Input Field

- [ ] TMP_InputField component ada di GameObject
- [ ] **Interactable** = ☑ Checked
- [ ] **Content Type** = Standard (bukan Decimal!)
- [ ] **Text Component** = Text Area/Text (TMP) assigned
- [ ] **Placeholder** = Text Area/Placeholder (TMP) assigned
- [ ] Placeholder text sudah diisi: "Masukkan jawaban (misal: 0.6 atau 3/5)"
- [ ] **Read Only** = ☐ Unchecked
- [ ] **Should Activate On Select** = ☑ Checked
- [ ] **On End Edit** event terhubung ke InputFieldHandler.OnInputFieldEndEdit
- [ ] Child object **Text Area** punya RectMask2D component
- [ ] Child object **Background** punya Image component dengan Raycast Target aktif
- [ ] InputFieldHandler script di GameManager sudah assign reference ke input field ini

---

## 🧪 Testing Input Field

Setelah setup, test hal berikut:

1. **Klik Input Field** → Cursor harus muncul
2. **Ketik angka desimal** (0.6) → Harus bisa
3. **Ketik pecahan** (3/5) → Harus bisa (karena Content Type = Standard)
4. **Ketik huruf** (abc) → Harus bisa (akan error saat verify, tapi input field terima)
5. **Press Enter** → Harus trigger VerifyAnswer() dari CalculationManager
6. **Placeholder hilang** saat mulai mengetik
7. **Auto-focus** saat scene dimulai

---

## ⚠️ Common Issues & Solutions

### **Problem 1: Tidak bisa mengetik**
**Solusi:**
- Pastikan **Interactable** = ☑ Checked
- Pastikan **Read Only** = ☐ Unchecked
- Pastikan ada EventSystem di scene (GameObject > UI > Event System)

### **Problem 2: Placeholder tidak muncul**
**Solusi:**
- Pastikan Placeholder (TMP) sudah di-assign
- Pastikan text placeholder kosong: "Masukkan jawaban..."
- Pastikan color Alpha > 0 (tidak transparan total)

### **Problem 3: Tidak bisa input pecahan (3/5)**
**Solusi:**
- **PENTING:** Content Type harus "Standard", bukan "Decimal Number"!
- Decimal Number hanya terima angka dan titik, tidak terima "/"

### **Problem 4: Enter tidak submit**
**Solusi:**
- Pastikan On End Edit event sudah terhubung ke InputFieldHandler
- Pastikan InputFieldHandler script sudah attach ke GameManager
- Check Console untuk error

### **Problem 5: Input field tidak focus otomatis**
**Solusi:**
- Pastikan InputFieldHandler.Start() memanggil `inputField.Select()` dan `ActivateInputField()`

---

## 📸 Visual Reference

```
┌────────────────────────────────────────────┐
│  ┌──────────────────────────────────────┐  │
│  │ Masukkan jawaban (misal: 0.6 atau   │  │ ← Placeholder (gray, semi-transparent)
│  │ 3/5)                          |      │  │ ← Caret (blinking cursor)
│  └──────────────────────────────────────┘  │
└────────────────────────────────────────────┘
         ↑ Background (white rectangle)

Saat user mengetik:
┌────────────────────────────────────────────┐
│  ┌──────────────────────────────────────┐  │
│  │ 0.6                          |       │  │ ← User input (black text)
│  └──────────────────────────────────────┘  │
└────────────────────────────────────────────┘
         ↑ Placeholder otomatis hilang
```

---

## 🎯 Final Setup Summary

**GameObject Name:** `AnswerInputField`

**Components:**
1. ✅ RectTransform (size: 400x60)
2. ✅ TMP_InputField
   - Interactable: Yes
   - Content Type: **Standard** (penting!)
   - Line Type: Single Line
   - Text Component: Text Area/Text (TMP)
   - Placeholder: Text Area/Placeholder (TMP)
3. ✅ Image (Background)

**Events:**
- On End Edit → InputFieldHandler.OnInputFieldEndEdit(string)

**Children:**
- Text Area (RectMask2D)
  - Placeholder (TextMeshProUGUI)
  - Text (TextMeshProUGUI)
- Background (Image)

---

**Setup selesai! Input field siap menerima input desimal (0.6) dan pecahan (3/5)** ✅
