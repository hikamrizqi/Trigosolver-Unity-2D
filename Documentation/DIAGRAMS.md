# 📊 Diagram & Visualisasi untuk Skripsi
## Chapter 1: Mode Observasi Segitiga

---

## 1. Class Diagram

```
┌─────────────────────────────────────┐
│      CalculationManager             │
├─────────────────────────────────────┤
│ - lives: int                        │
│ - progres: int                      │
│ - score: int                        │
│ - totalSoal: int = 5                │
│ - answerTolerance: float = 0.01     │
│ - dataSoalSaatIni: TriangleData     │
├─────────────────────────────────────┤
│ + Start(): void                     │
│ + StartNewRound(): void             │
│ + VerifyAnswer(): void              │
│ + HandleWrongAnswer(string): void   │
│ + EndChapter(): void                │
│ + GetScore(): int                   │
└────────┬───────────────────┬────────┘
         │ uses              │ uses
         ▼                   ▼
┌──────────────────┐  ┌─────────────────────────┐
│TriangleData      │  │ UIManagerChapter1       │
│Generator         │  ├─────────────────────────┤
├──────────────────┤  │ + depanSprite: SR       │
│ - triples: List  │  │ + sampingSprite: SR     │
├──────────────────┤  │ + miringSprite: SR      │
│ + GenerateNew    │  │ + jawabanInput: TMP_IF  │
│   Question():    │  │ + feedbackText: TMP     │
│   TriangleData   │  ├─────────────────────────┤
└──────────────────┘  │ + SetupNewQuestion()    │
                      │ + HighlightSide()       │
       ┌──────────────┤ + ShowFeedback()        │
       │              │ + UpdateLives()         │
       │              │ + OnDepanButtonClicked()│
       │              └─────────────────────────┘
       │
       │ creates
       ▼
┌──────────────────┐
│  TriangleData    │
├──────────────────┤
│ + Depan: int     │
│ + Samping: int   │
│ + Miring: int    │
│ + SoalDiseder    │
│   hanakan: str   │
│ + JawabanBenar:  │
│   float          │
└──────────────────┘

SR = SpriteRenderer
TMP_IF = TMP_InputField
TMP = TextMeshProUGUI
```

---

## 2. Sequence Diagram - User Answer Flow

```
User          InputField    CalculationMgr   DataGenerator   UIManager
 │                │               │                │             │
 │ Type "0.6"     │               │                │             │
 ├───────────────>│               │                │             │
 │                │               │                │             │
 │ Press Enter    │               │                │             │
 ├───────────────>│               │                │             │
 │                │               │                │             │
 │                │ VerifyAnswer()│                │             │
 │                ├──────────────>│                │             │
 │                │               │                │             │
 │                │               │ Get input text │             │
 │                │               ├───────────────>│             │
 │                │               │                │             │
 │                │               │ Parse input    │             │
 │                │               │ (0.6 or 3/5)   │             │
 │                │               │                │             │
 │                │               │ Compare with   │             │
 │                │               │ correctAnswer  │             │
 │                │               │                │             │
 │                │          [IF CORRECT]          │             │
 │                │               │                │             │
 │                │               │ score += 10    │             │
 │                │               │                │             │
 │                │               │ShowCorrectFeedback()         │
 │                │               ├─────────────────────────────>│
 │                │               │                │             │
 │                │               │                │ Highlight   │
 │                │               │                │ Green +     │
 │                │               │                │ Sparkle     │
 │                │               │                │             │
 │<────────────────────────────────────────────────┤             │
 │  "PENGUKURAN TEPAT! +10 Poin"  │                │             │
 │                │               │                │             │
 │                │   Wait 2s     │                │             │
 │                │               │                │             │
 │                │ StartNewRound()                │             │
 │                │               ├────────────────>│             │
 │                │               │ GenerateNew    │             │
 │                │               │ Question()     │             │
 │                │               │                │             │
```

---

## 3. State Diagram - Chapter 1 Game States

```
     ┌─────────────┐
     │   INIT      │
     │ Lives=3     │
     │ Score=0     │
     └──────┬──────┘
            │
            ▼
     ┌─────────────┐
     │  QUESTION   │◄─────────────┐
     │ Generate    │              │
     │ New Soal    │              │
     └──────┬──────┘              │
            │                     │
            ▼                     │
     ┌─────────────┐              │
     │WAITING_INPUT│              │
     │User Types   │              │
     │& Clicks     │              │
     └──────┬──────┘              │
            │                     │
       Submit Answer              │
            │                     │
            ▼                     │
     ┌─────────────┐              │
     │ VALIDATING  │              │
     │Check Answer │              │
     └──────┬──────┘              │
            │                     │
       ┌────┴────┐                │
       │         │                │
    CORRECT   WRONG               │
       │         │                │
       ▼         ▼                │
  ┌────────┐ ┌────────┐           │
  │+10 pts │ │Lives-1 │           │
  │Green   │ │Red     │           │
  └───┬────┘ └───┬────┘           │
      │          │                │
      │      Lives > 0?           │
      │          │                │
      │     YES──┼────────────────┘
      │          │
      │         NO
      │          │
Progres < 5?     ▼
      │     ┌──────────┐
     YES    │GAME_OVER │
      │     │Show Score│
      │     └──────────┘
      │
     NO
      │
      ▼
┌──────────────┐
│CHAPTER_COMPLETE│
│Show Cutscene │
│Badge & Score │
└──────────────┘
```

---

## 4. Activity Diagram - Complete Gameplay Flow

```
          (Start)
             │
             ▼
    ┌─────────────────┐
    │Initialize Game  │
    │Lives=3, Score=0 │
    └────────┬────────┘
             │
    ┌────────▼────────┐
    │Generate Question│
    │Random Triple    │
    │Random Operation │
    └────────┬────────┘
             │
    ┌────────▼────────┐
    │Display Triangle │
    │Show Question    │
    │Focus Input Field│
    └────────┬────────┘
             │
        ┌────▼────┐
        │User     │
        │Interacts│
        └────┬────┘
             │
    ┌────────┴────────┐
    │                 │
┌───▼───┐      ┌──────▼──────┐
│Click  │      │Type Answer  │
│Depan/ │      │& Submit     │
│Samping│      └──────┬──────┘
│Miring │             │
└───┬───┘             │
    │            ┌────▼────┐
    │            │Parse    │
    │            │Input    │
    │            └────┬────┘
    │                 │
    │          ┌──────▼──────┐
    │          │Valid Format?│
    │          └──────┬──────┘
    │                 │
    │           NO ┌──┴──┐ YES
    │              │     │
    │         ┌────▼──┐  │
    │         │Error  │  │
    │         │Message│  │
    │         └────┬──┘  │
    │              │     │
    │              └─────┤
    │                    │
    │             ┌──────▼──────┐
    │             │Check Answer │
    │             │with Correct │
    │             └──────┬──────┘
    │                    │
    │              ┌─────┴─────┐
    │              │           │
    │          CORRECT       WRONG
    │              │           │
    │      ┌───────▼──┐   ┌────▼────┐
    │      │Score +10 │   │Lives -1 │
    │      │Green     │   │Red      │
    │      │Sparkle   │   │Highlight│
    │      └───────┬──┘   └────┬────┘
    │              │           │
    │              │      ┌────▼────┐
    │              │      │Lives>0? │
    │              │      └────┬────┘
    │              │           │
    │              │      YES ┌┴┐ NO
    │              │          │ │
    │              │          │ └──┐
    │              └──────────┤    │
    │                         │    │
    │                    ┌────▼──┐ │
    │                    │Progres│ │
    │                    │< 5?   │ │
    │                    └────┬──┘ │
    │                         │    │
    │                    YES ┌┴┐ NO│
    │                        │ │   │
    │                        └─┘   │
    │                              │
    └──────────────────────────────┤
                                   │
                            ┌──────┴──────┐
                            │             │
                       ┌────▼──┐    ┌─────▼─────┐
                       │Game   │    │Chapter    │
                       │Over   │    │Complete   │
                       │Screen │    │Cutscene   │
                       └───────┘    └───────────┘
```

---

## 5. Use Case Diagram

```
                 ┌──────────────┐
                 │    PEMAIN    │
                 │   (Rizqi)    │
                 └───────┬──────┘
                         │
          ┌──────────────┼──────────────┐
          │              │              │
          ▼              ▼              ▼
   ┌────────────┐ ┌────────────┐ ┌────────────┐
   │ Menganalisis│ │Mengidentif│ │ Menghitung │
   │   Soal     │ │ikasi Sisi │ │   Rasio    │
   │            │ │(Klik Btn) │ │Trigonometri│
   └────────────┘ └────────────┘ └─────┬──────┘
                                       │
                                       ▼
                                ┌────────────┐
                                │ Memasukkan │
                                │  Jawaban   │
                                └─────┬──────┘
                                      │
                       ┌──────────────┼──────────────┐
                       │              │              │
                       ▼              ▼              ▼
                ┌────────────┐ ┌────────────┐ ┌────────────┐
                │Input Desimal│ │Input Pecah│ │Press Enter │
                │  (0.6)     │ │  (3/5)    │ │  Submit    │
                └────────────┘ └────────────┘ └─────┬──────┘
                                                     │
                                                     ▼
                                              ┌────────────┐
                                              │Menerima    │
                                              │Feedback    │
                                              └────────────┘

  «include»                                   «extend»
┌──────────────────────────────────────────────────────────┐
│                   SISTEM CHAPTER 1                       │
├──────────────────────────────────────────────────────────┤
│ • Generate Soal Random                                   │
│ • Validasi Jawaban                                       │
│ • Update Lives & Score                                   │
│ • Highlight Visual Feedback                              │
│ • Trigger End Cutscene                                   │
└──────────────────────────────────────────────────────────┘
```

---

## 6. Component Diagram

```
┌─────────────────────────────────────────────────────────┐
│              CHAPTER 1 SYSTEM                           │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ┌──────────────────────────────────────────────┐      │
│  │         GAME LOGIC LAYER                     │      │
│  │  ┌────────────────────────────────────┐      │      │
│  │  │   CalculationManager               │      │      │
│  │  │  - Answer validation               │      │      │
│  │  │  - Lives & score management        │      │      │
│  │  │  - Game flow control               │      │      │
│  │  └─────────┬──────────────────────────┘      │      │
│  │            │                                  │      │
│  │  ┌─────────▼──────────────────────────┐      │      │
│  │  │   TriangleDataGenerator            │      │      │
│  │  │  - Random question generation      │      │      │
│  │  │  - Pythagorean triples selection   │      │      │
│  │  └────────────────────────────────────┘      │      │
│  └──────────────────────────────────────────────┘      │
│                      │                                  │
│                      │ Events & Data                    │
│                      ▼                                  │
│  ┌──────────────────────────────────────────────┐      │
│  │         PRESENTATION LAYER                   │      │
│  │  ┌────────────────────────────────────┐      │      │
│  │  │   UIManagerChapter1                │      │      │
│  │  │  - UI element updates              │      │      │
│  │  │  - Color-coded feedback            │      │      │
│  │  │  - Animation triggers              │      │      │
│  │  └────────────────────────────────────┘      │      │
│  │                                              │      │
│  │  ┌────────────────────────────────────┐      │      │
│  │  │   InputFieldHandler                │      │      │
│  │  │  - Input capture                   │      │      │
│  │  │  - Enter key detection             │      │      │
│  │  └────────────────────────────────────┘      │      │
│  └──────────────────────────────────────────────┘      │
│                      │                                  │
│                      │ Render & Display                 │
│                      ▼                                  │
│  ┌──────────────────────────────────────────────┐      │
│  │         VIEW LAYER (Unity UI)                │      │
│  │  - Canvas (Screen Space Overlay)             │      │
│  │  - TextMeshPro elements                      │      │
│  │  - Buttons & Input Fields                    │      │
│  │  - SpriteRenderers (Triangle visualization)  │      │
│  │  - Particle System (Sparkle effect)          │      │
│  └──────────────────────────────────────────────┘      │
│                                                         │
└─────────────────────────────────────────────────────────┘
```

---

## 7. Data Flow Diagram (Level 0 - Context)

```
              ┌─────────────┐
              │   PEMAIN    │
              └──────┬──────┘
                     │
         Input Jawaban (0.6, 3/5)
                     │
                     ▼
         ┌───────────────────────┐
         │   SISTEM CHAPTER 1    │
         │  ┌─────────────────┐  │
         │  │ Validate Answer │  │
         │  │ Calculate Score │  │
         │  │ Update Lives    │  │
         │  └─────────────────┘  │
         └───────────┬───────────┘
                     │
        Feedback Visual & Textual
                     │
                     ▼
              ┌─────────────┐
              │   PEMAIN    │
              └─────────────┘
```

---

## 8. Deployment Diagram

```
┌──────────────────────────────────────────┐
│      USER DEVICE (PC/Laptop)             │
│                                          │
│  ┌────────────────────────────────────┐  │
│  │     Unity Runtime Environment      │  │
│  │                                    │  │
│  │  ┌──────────────────────────────┐  │  │
│  │  │   Chapter1_Scene.unity       │  │  │
│  │  │                              │  │  │
│  │  │  GameManager (GameObject)    │  │  │
│  │  │  ├─ CalculationManager.cs    │  │  │
│  │  │  ├─ TriangleDataGenerator.cs │  │  │
│  │  │  └─ InputFieldHandler.cs     │  │  │
│  │  │                              │  │  │
│  │  │  UIManager (GameObject)      │  │  │
│  │  │  └─ UIManagerChapter1.cs     │  │  │
│  │  │                              │  │  │
│  │  │  Canvas (UI)                 │  │  │
│  │  │  ├─ Header Panel             │  │  │
│  │  │  ├─ Question Panel           │  │  │
│  │  │  └─ Feedback Panel           │  │  │
│  │  └──────────────────────────────┘  │  │
│  └────────────────────────────────────┘  │
│                                          │
│  ┌────────────────────────────────────┐  │
│  │      Graphics Card (GPU)           │  │
│  │  - Render Triangle Sprites         │  │
│  │  - Particle System Effects         │  │
│  └────────────────────────────────────┘  │
│                                          │
│  ┌────────────────────────────────────┐  │
│  │      Audio System (Optional)       │  │
│  │  - SFX (Correct/Wrong answers)     │  │
│  │  - Background Music                │  │
│  └────────────────────────────────────┘  │
└──────────────────────────────────────────┘
```

---

## 9. ER Diagram - Data Model

```
┌─────────────────────┐
│   TriangleData      │
├─────────────────────┤
│ PK: (implicit)      │
│                     │
│ Depan: int          │
│ Samping: int        │
│ Miring: int         │
│ SoalDisederhanakan: │
│   string            │
│ JawabanBenar: float │
└──────────┬──────────┘
           │
           │ 1
           │
           │ generated by
           │
           │ N
           ▼
┌─────────────────────┐
│ PythagoreanTriple   │
├─────────────────────┤
│ a: int              │
│ b: int              │
│ c: int              │
└─────────────────────┘

Instances:
(3, 4, 5)
(5, 12, 13)
(8, 15, 17)
(7, 24, 25)
```

---

## 10. Timing Diagram - Answer Validation

```
Time →
0s    1s    2s    3s    4s    5s
│     │     │     │     │     │
User: ─┐         ┌─────────────
Input  └─────────┘
       Type answer

System:    ┌─┐
Validate   └─┘
           Check format
           Compare answer

UI:           ┌──────┐
Feedback      └──────┘
Show message

Triangle:        ┌───┐
Highlight        └───┘
Color change

Particle:          ┌┐
Effect              └┘
(if correct)

Lives/Score:         ┌────────
Update                └────────
Display

Next:                     ┌────
Round                     └────
(after 2s delay)
```

---

**Catatan:**
Diagram-diagram ini dapat digunakan untuk:
- Bab 4 Skripsi (Perancangan & Implementasi)
- Presentasi/Sidang
- Dokumentasi teknis
- Portfolio

Format dapat di-convert ke:
- Draw.io / Lucidchart (for professional diagrams)
- PlantUML (for code-generated diagrams)
- Microsoft Visio
- PowerPoint (for presentations)
