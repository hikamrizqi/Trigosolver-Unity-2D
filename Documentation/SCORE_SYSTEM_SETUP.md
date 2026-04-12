# Setup Score System di Unity Editor

## Panduan Setup Komponen Score System

Sistem scoring telah selesai dibuat dengan komponen berikut:
- **ScoreDisplayManager**: Menampilkan score dan animasi +10
- **GameOverPanel**: Menampilkan panel game over saat nyawa habis
- **HighscoreUI**: Menampilkan leaderboard di Main Menu
- **HighScoreManager**: Menyimpan data highscore dengan tanggal & waktu

---

## 1. Setup Score Display Manager (Chapter 1 Scene)

### Langkah-langkah:

1. **Buat Score Text (Persistent Display)**
   - Buat GameObject baru di Canvas → UI → Text - TextMeshPro
   - Rename: `TotalScoreText`
   - Position: Top-right atau top-left corner (gunakan Anchors)
   - Text: `"Score: 0"`
   - Font Size: 36-48
   - Color: Putih atau kuning
   - Alignment: Center

2. **Buat Score Popup Prefab (+10 Animation)**
   - Buat GameObject baru di scene → UI → Text - TextMeshPro
   - Rename: `ScorePopup`
   - Add Component: `Canvas Group` (untuk fade animation)
   - Settings:
     * Text: `"+10"`
     * Font Size: 48
     * Color: **Hijau** (0, 255, 0)
     * Alignment: Center
     * Pivot: (0.5, 0.5)
   - Drag ke Project untuk jadikan prefab
   - Hapus dari scene (prefab akan di-instantiate oleh script)

3. **Buat Popup Spawn Point**
   - Buat Empty GameObject di Canvas
   - Rename: `PopupSpawnPoint`
   - Position: Di tengah layar atau dekat Total Score Text
   - Ini lokasi awal munculnya +10 popup

4. **Setup ScoreDisplayManager Component**
   - Pilih GameObject dengan script `CalculationManager`
   - Di Inspector, cari field `Score Display Manager`
   - Jika belum ada GameObject dengan ScoreDisplayManager:
     * Buat GameObject baru (nama: `ScoreDisplayManager`)
     * Add Component → ScoreDisplayManager script
   - Assign references:
     * **Total Score Text**: TotalScoreText yang dibuat di step 1
     * **Score Popup Prefab**: ScorePopup prefab dari Project
     * **Popup Spawn Point**: PopupSpawnPoint Transform
     * **Popup Duration**: 1.5f
     * **Popup Float Distance**: 100
     * **Score Color**: Hijau (0, 255, 0)

5. **Assign ScoreDisplayManager ke CalculationManager**
   - Pilih GameObject dengan `CalculationManager`
   - Drag `ScoreDisplayManager` GameObject ke field `Score Display Manager`

---

## 2. Setup Game Over Panel (Chapter 1 Scene)

### Langkah-langkah:

1. **Buat Game Over Panel**
   - Buat GameObject baru di Canvas → UI → Panel
   - Rename: `GameOverPanel`
   - Settings:
     * Anchor: Stretch both (full screen)
     * Color: Hitam dengan alpha 200 (untuk dim background)
   
2. **Buat Score Text**
   - Child of GameOverPanel → UI → Text - TextMeshPro
   - Rename: `ScoreText`
   - Text: `"Skor Akhir: 0"`
   - Font Size: 48
   - Color: Kuning
   - Alignment: Center
   - Position: Tengah atas panel

3. **Buat Message Text**
   - Child of GameOverPanel → UI → Text - TextMeshPro
   - Rename: `MessageText`
   - Text: 
     ```
     PERMAINAN BERAKHIR
     Nyawa Habis!
     ```
   - Font Size: 56
   - Color: Merah
   - Alignment: Center
   - Position: Tengah panel

4. **Setup GameOverPanel Component**
   - Pilih GameObject `GameOverPanel`
   - Add Component → GameOverPanel script
   - Assign references:
     * **Panel**: GameOverPanel GameObject itu sendiri
     * **Score Text**: ScoreText
     * **Message Text**: MessageText
     * **Display Duration**: 3.0f
     * **Level Selection Manager**: Drag LevelSelectionManager GameObject

5. **Assign GameOverPanel ke CalculationManager**
   - Pilih GameObject dengan `CalculationManager`
   - Drag `GameOverPanel` GameObject ke field `Game Over Panel`

6. **PENTING: Nonaktifkan Panel Awal**
   - Pilih GameOverPanel di Hierarchy
   - **Uncheck** checkbox di Inspector (untuk hide saat game start)

---

## 3. Setup Highscore UI (Main Menu Scene)

### Langkah-langkah:

1. **Buat Highscore Panel**
   - Buat GameObject baru di Canvas → UI → Panel
   - Rename: `HighscorePanel`
   - Settings:
     * Anchor: Center
     * Size: 800 x 600 (atau sesuai layar)
     * Color: Putih dengan alpha 255

2. **Buat Header Text**
   - Child of HighscorePanel → UI → Text - TextMeshPro
   - Rename: `HeaderText`
   - Text: `"HIGHSCORE"`
   - Font Size: 64
   - Alignment: Center
   - Position: Top of panel

3. **Buat Top 10 Section**
   - Child of HighscorePanel → UI → Panel
   - Rename: `Top10Section`
   - Position: Left half of panel
   
   - Add child: Text - TextMeshPro
     * Rename: `Top10Title`
     * Text: `"TOP 10 TERTINGGI"`
     * Font Size: 36
   
   - Add child: Scroll View (UI → Scroll View)
     * Rename: `Top10ScrollView`
     * Remove Horizontal Scrollbar
     * Content → Add Component: **Vertical Layout Group**
       - Child Force Expand: Height = OFF
       - Spacing: 10
     * Content → Add Component: **Content Size Fitter**
       - Vertical Fit: Preferred Size
   
   - Simpan reference ke Content: `Top10Container`

4. **Buat Recent 3 Section**
   - Child of HighscorePanel → UI → Panel
   - Rename: `Recent3Section`
   - Position: Right half of panel
   
   - Add child: Text - TextMeshPro
     * Rename: `Recent3Title`
     * Text: `"RIWAYAT TERAKHIR"`
     * Font Size: 36
   
   - Add child: Scroll View
     * Rename: `Recent3ScrollView`
     * Setup sama seperti Top10ScrollView
   
   - Simpan reference ke Content: `Recent3Container`

5. **Buat Score Entry Prefab**
   - Buat GameObject di scene → UI → Text - TextMeshPro
   - Rename: `ScoreEntryText`
   - Settings:
     * Font Size: 28
     * Color: Hitam
     * Alignment: Left
     * Text: `"1. Score: 100  |  06 Jan 2026, 14:30"`
   - Add Component: **Layout Element**
     * Preferred Height: 40
   - Drag ke Project untuk jadikan prefab
   - Hapus dari scene

6. **Buat Back Button**
   - Child of HighscorePanel → UI → Button
   - Rename: `BackButton`
   - Text: `"KEMBALI"`
   - Position: Bottom of panel
   - OnClick() → Assign `HighscoreUI.HideHighscorePanel()`

7. **Setup HighscoreUI Component**
   - Buat GameObject baru (nama: `HighscoreUI`)
   - Add Component → HighscoreUI script
   - Assign references:
     * **Highscore Panel**: HighscorePanel GameObject
     * **Top10 Container**: Top10ScrollView → Viewport → Content
     * **Recent3 Container**: Recent3ScrollView → Viewport → Content
     * **Score Entry Prefab**: ScoreEntryText prefab dari Project

8. **Buat Highscore Button di Main Menu**
   - Duplicate button yang sudah ada di Main Menu
   - Rename: `HighscoreButton`
   - Text: `"HIGHSCORE"`
   - Position: Di bawah button PLAY atau di tempat yang sesuai
   - OnClick() → Assign:
     * Target: HighscoreUI GameObject
     * Function: `HighscoreUI.ShowHighscorePanel()`

9. **PENTING: Nonaktifkan Panel Awal**
   - Pilih HighscorePanel di Hierarchy
   - **Uncheck** checkbox di Inspector

---

## 4. Verifikasi Setup

### Checklist:

**Chapter 1 Scene:**
- [ ] TotalScoreText sudah terhubung ke ScoreDisplayManager
- [ ] ScorePopup prefab sudah assigned
- [ ] PopupSpawnPoint sudah assigned
- [ ] GameOverPanel sudah assigned ke CalculationManager
- [ ] GameOverPanel **DINONAKTIFKAN** di awal
- [ ] ScoreDisplayManager sudah assigned ke CalculationManager
- [ ] LevelSelectionManager sudah assigned ke GameOverPanel

**Main Menu Scene:**
- [ ] HighscorePanel sudah **DINONAKTIFKAN** di awal
- [ ] Top10Container sudah assigned (Content dari ScrollView)
- [ ] Recent3Container sudah assigned
- [ ] ScoreEntryText prefab sudah assigned
- [ ] Highscore button sudah memanggil ShowHighscorePanel()
- [ ] Back button sudah memanggil HideHighscorePanel()

---

## 5. Testing

### Test Flow:

1. **Test Score Display:**
   - Play Chapter 1 → Level 1
   - Jawab soal dengan benar
   - Cek: Popup +10 muncul, float up, fade out
   - Cek: Total Score bertambah di atas layar

2. **Test Game Over:**
   - Play Chapter 1 → Level 1
   - Sengaja jawab salah 3 kali
   - Cek: Game Over panel muncul
   - Cek: Score akhir ditampilkan
   - Tunggu 3 detik
   - Cek: Otomatis kembali ke level selection

3. **Test Back Button:**
   - Play Chapter 1 → Level 1
   - Jawab beberapa soal (dapat score)
   - Klik tombol Back
   - Cek: Kembali ke level selection
   - Cek: Score ter-reset jika main lagi

4. **Test Highscore Display:**
   - Di Main Menu, klik button "HIGHSCORE"
   - Cek: Panel highscore muncul
   - Cek: TOP 10 menampilkan score tertinggi
   - Cek: RIWAYAT menampilkan 3 score terakhir
   - Cek: Format: "Rank. Score: X | Date, Time"
   - Klik "KEMBALI"
   - Cek: Panel tertutup

5. **Test Score Persistence:**
   - Main beberapa kali (sampai game over)
   - Tutup game
   - Buka lagi
   - Cek highscore panel
   - Cek: Score masih tersimpan

---

## 6. Troubleshooting

### Score popup tidak muncul:
- Pastikan ScorePopup prefab memiliki CanvasGroup component
- Pastikan DOTween sudah di-import ke project
- Check Console untuk error

### Game Over panel tidak muncul:
- Pastikan GameOverPanel dinonaktifkan di awal
- Pastikan Lives berkurang dengan benar
- Check Debug.Log di HandleWrongAnswer()

### Highscore tidak tersimpan:
- Check Console: apakah ada log "[HighScoreManager] Score saved"
- Test ClearLeaderboard() jika data corrupt
- Pastikan HighScoreManager singleton aktif

### Highscore panel kosong:
- Check apakah ada data: Main game sampai game over dulu
- Pastikan ScoreEntryText prefab sudah assigned
- Check Console untuk error di DisplayTop10()

---

## 7. File Script yang Dibuat/Dimodifikasi

### File Baru:
1. `ScoreDisplayManager.cs` - Manage score display & popup animation
2. `GameOverPanel.cs` - Handle game over screen
3. `HighscoreUI.cs` - Display leaderboard di Main Menu

### File Dimodifikasi:
1. `CalculationManager.cs` - Integrasi dengan score system
2. `HighScoreManager.cs` - Tambah fungsi SaveScore(), GetTop10(), GetRecent3()

---

## 8. Catatan Tambahan

- **Score Rules:** +10 per correct answer
- **Save Points:** 
  * Saat nyawa habis (auto via GameOverPanel)
  * Saat klik back button (manual)
  * Saat selesai 30 soal (auto via EndChapter)
- **Reset Points:**
  * Saat start level baru
  * Setelah game over
  * Setelah klik back button
- **Max Entries:** 100 scores tersimpan di PlayerPrefs
- **Display Format:** "Score: X  |  dd MMM yyyy, HH:mm"

---

**Selesai! Sistem scoring sudah siap digunakan setelah setup di Unity Editor.**
