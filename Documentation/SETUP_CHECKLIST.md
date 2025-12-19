# ✅ Checklist Setup Unity Editor - Print & Check!

## 📋 Stage 1: Observasi Segitiga

### Asset Preparation
- [ ] Buat folder `Assets/Sprites/Stage1`
- [ ] Buat sprite garis biru (depan)
- [ ] Buat sprite garis hijau (samping)
- [ ] Buat sprite garis merah (miring)
- [ ] Buat sprite heart_full
- [ ] Buat sprite heart_empty

### Scene Setup
- [ ] Buat scene baru `Stage1_Scene.unity`
- [ ] Save scene

### Canvas UI
- [ ] Buat Canvas (Screen Space Overlay)
- [ ] Set Canvas Scaler: 1920x1080
- [ ] Buat Header Panel
  - [ ] Add JudulText (TMP)
  - [ ] Add ProgresText (TMP)
  - [ ] Add LivesPanel dengan 3 Hearts
- [ ] Buat QuestionPanel
  - [ ] Add PertanyaanText (TMP)
  - [ ] Add JawabanInputField (TMP)
  - [ ] Add VerifyButton
- [ ] Buat FeedbackPanel (inactive)
  - [ ] Add FeedbackText (TMP)

### Game World
- [ ] Buat GameObject "GameWorld"
- [ ] Buat GameObject "Triangle"
- [ ] Tambah DepanSide (Sprite)
  - [ ] Set position, rotation, scale
  - [ ] Add DepanLabel (TMP World)
- [ ] Tambah SampingSide (Sprite)
  - [ ] Set position, rotation, scale
  - [ ] Add SampingLabel (TMP World)
- [ ] Tambah MiringSide (Sprite)
  - [ ] Set position, rotation, scale
  - [ ] Add MiringLabel (TMP World)
- [ ] Tambah SparkleEffect (Particle System)

### Scripts
- [ ] Buat GameObject "GameManagers"
- [ ] Add CalculationManager script
- [ ] Add TriangleDataGeneratorStage2 script
- [ ] Add UIManagerChapter1 script
- [ ] Add Chapter1AudioManager script (opsional)
- [ ] Add Chapter1EndCutscene script (opsional)

### Connect References (CalculationManager)
- [ ] UI Manager
- [ ] Data Generator
- [ ] End Cutscene

### Connect References (UIManagerChapter1)
- [ ] Judul Text
- [ ] Progres Text
- [ ] Lives Icons (3 hearts)
- [ ] Pertanyaan Text
- [ ] Jawaban Input
- [ ] Feedback Panel
- [ ] Feedback Text
- [ ] Depan Label World
- [ ] Samping Label World
- [ ] Miring Label World
- [ ] Depan Sprite
- [ ] Samping Sprite
- [ ] Miring Sprite
- [ ] Sparkle Effect
- [ ] Audio Manager

### Button Events
- [ ] VerifyButton → CalculationManager.VerifyAnswer()

### Camera
- [ ] Set Orthographic, Size: 5
- [ ] Position: (0, 0, -10)

### Testing
- [ ] Play scene
- [ ] Soal muncul
- [ ] Input 0.6 → works
- [ ] Input 3/5 → works
- [ ] Highlight berubah
- [ ] Lives berkurang
- [ ] Sparkle muncul (benar)

---

## 📋 Stage 2: Cannon Challenge

### Asset Preparation
- [ ] Buat folder `Assets/Sprites/Stage2`
- [ ] Buat sprite cannon_base
- [ ] Buat sprite cannon_barrel
- [ ] Buat sprite projectile (circle)
- [ ] Buat sprite target_ship
- [ ] Buat sprite ground
- [ ] Buat sprite water

### Scene Setup
- [ ] Buat scene baru `Stage2_Scene.unity`
- [ ] Save scene

### Canvas UI
- [ ] Buat Canvas (Screen Space Overlay)
- [ ] Buat Header Panel
  - [ ] Add QuestionText (TMP)
- [ ] Buat InputPanel
  - [ ] Add AngleInputField (TMP)
  - [ ] Add ShootButton
- [ ] Add FeedbackText (TMP)

### Environment
- [ ] Buat GameObject "Environment"
- [ ] Add Ground (Sprite + Box Collider 2D)
  - [ ] Tag: "Ground"
- [ ] Add Water (Sprite, order -1)

### Cannon Setup
- [ ] Buat GameObject "Cannon"
- [ ] Add CannonBase (Sprite child)
- [ ] Add CannonBarrel (Sprite child)
  - [ ] Add CannonController script
- [ ] Add ShootPoint (Empty child of CannonBarrel)

### Target Setup
- [ ] Buat GameObject "Target"
  - [ ] Tag: "Target"
- [ ] Add TargetShip (Sprite + Box Collider 2D)

### Projectile Prefab
- [ ] Buat GameObject Projectile (Circle sprite)
- [ ] Add Rigidbody2D
  - [ ] Gravity Scale: 1
  - [ ] Collision: Continuous
- [ ] Add Circle Collider 2D
- [ ] Add ProjectileController script
- [ ] Drag ke folder Prefabs
- [ ] Delete dari Hierarchy

### Scripts
- [ ] Buat GameObject "GameManagers"
- [ ] Add GameManagerChapter2 script

### Connect References (GameManagerChapter2)
- [ ] Cannon Controller
- [ ] Projectile Prefab
- [ ] Shoot Point
- [ ] Target Object
- [ ] Question Text
- [ ] Angle Input Field
- [ ] Shoot Button
- [ ] Feedback Text

### Button Events
- [ ] ShootButton → GameManagerChapter2.OnShootButtonClicked()

### Physics Settings
- [ ] Edit → Project Settings → Physics2D
- [ ] Gravity Y: -9.81

### Camera
- [ ] Set Orthographic, Size: 6
- [ ] Position: (0, 0, -10)
- [ ] Background: Sky Blue

### Testing
- [ ] Play scene
- [ ] Soal muncul (jarak)
- [ ] Input sudut 45
- [ ] Click TEMBAK
- [ ] Cannon rotate
- [ ] Peluru spawn
- [ ] Parabola trajectory
- [ ] Hit/miss detection works

---

## 🔍 Common Errors Checklist

### NullReferenceException
- [ ] Semua fields di Inspector terisi
- [ ] Scripts di-assign ke GameObject
- [ ] Button OnClick connected

### Object tidak terlihat
- [ ] Position Z = 0 (bukan -10)
- [ ] Scale > 0
- [ ] Camera Size cukup (5-6)
- [ ] Sorting Order benar

### Particle tidak muncul
- [ ] Emission Rate > 0
- [ ] Play On Awake = false
- [ ] Script activate particle

### Physics tidak bekerja
- [ ] Rigidbody2D attached
- [ ] Gravity Scale = 1
- [ ] Collider attached
- [ ] Tag benar ("Ground", "Target")

### Button tidak bisa diklik
- [ ] EventSystem ada di scene
- [ ] Button Interactable = true
- [ ] OnClick event connected

---

## ⚙️ Final Verification

### Stage 1
- [ ] All scripts no errors
- [ ] All references connected
- [ ] Play → no red errors
- [ ] Gameplay loop works
- [ ] Can complete 5 questions
- [ ] Lives system works
- [ ] Score system works

### Stage 2
- [ ] All scripts no errors
- [ ] All references connected
- [ ] Play → no red errors
- [ ] Cannon rotates
- [ ] Projectile physics works
- [ ] Collision detection works
- [ ] Hit/miss feedback correct

---

## 📝 Notes & Customization

### Warna yang bisa diubah:
```
Stage 1:
- Background color
- Triangle colors
- UI colors
- Particle colors

Stage 2:
- Sky color
- Water color
- Cannon color
- Target color
```

### Difficulty Settings:
```
Stage 1:
- Total soal (5 → 10)
- Lives (3 → 5)
- Answer tolerance (0.01 → 0.05)

Stage 2:
- Initial velocity (100 → 150)
- Target distance range
- Angle tolerance
```

### Audio (Opsional):
```
Import audio files:
- correct.wav
- wrong.wav
- shoot.wav
- background_music.mp3

Assign di:
- Chapter1AudioManager
- GameManagerChapter2
```

---

**SELAMAT! Jika semua checklist ✅ = Project Ready! 🎉**

Print halaman ini dan centang satu per satu sambil setup!
