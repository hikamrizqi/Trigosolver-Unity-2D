# 🔙 Panduan Setup Back Button yang Sudah Diperbaiki

## 📋 Perubahan yang Dilakukan

Sistem back button telah diperbaiki menjadi **SATU tombol dinamis di header** yang berubah fungsinya berdasarkan konteks:

- ✅ **Di Level Selection** → Klik back = Kembali ke Main Menu
- ✅ **Saat Bermain** → Klik back = Kembali ke Level Selection

---

## 📂 File yang Dimodifikasi/Dibuat

### ✨ **File Baru:**
1. **BackButtonManager.cs**
   - Lokasi: `Assets/Scripts/UI/Chapter1/BackButtonManager.cs`
   - Fungsi: Mengatur satu tombol back dengan mode dinamis

### 🔧 **File yang Diupdate:**
1. **LevelSelectionManager.cs**
   - Ditambahkan: Reference ke `BackButtonManager`
   - Ditambahkan: `backButtonManager.SetLevelSelectionMode()` di `ShowLevelSelection()`
   - Ditambahkan: `backButtonManager.SetGameplayMode()` di `LevelSelectionSequence()`

2. **CalculationManager.cs**
   - Sudah ada: Score save logic di `BackToLevelSelection()`
   - Sudah lengkap: Save score sebelum kembali ke level selection

---

## 🎮 Langkah Setup di Unity Editor

### **LANGKAH 1: Hapus Tombol Back yang Lama**

1. Buka **Scene Chapter 1** di Unity
2. Di Hierarchy, cari dan **HAPUS** semua tombol back yang ada di:
   - Answer Slot Panel Level 1
   - Answer Slot Panel Level 2  
   - Answer Slot Panel Level 3
3. Tombol-tombol ini tidak diperlukan lagi karena diganti satu tombol di header

---

### **LANGKAH 2: Buat Tombol Back Baru di Header**

#### A. Buat UI Button di Header

1. Di Hierarchy, cari objek **Canvas** → Expand
2. Cari objek yang berisi UI header/top bar (biasanya `TopPanel` atau `HeaderPanel`)
3. **Klik kanan** pada panel header → UI → **Button - TextMeshPro**
4. Rename button menjadi: `BackButton`

#### B. Posisikan Button di Pojok Kiri Atas

1. Pilih `BackButton` di Hierarchy
2. Di Inspector, atur **Rect Transform**:
   ```
   Anchor Presets: Top-Left
   Pos X: 80
   Pos Y: -80
   Width: 140
   Height: 70
   ```

#### C. Sesuaikan Tampilan Button

1. **Button Component:**
   - Normal Color: Putih/abu-abu terang
   - Highlighted Color: Kuning/hijau terang
   - Pressed Color: Hijau tua
   - Transition: Color Tint

2. **Button Image:**
   - Pilih sprite yang sesuai (misal: rounded rectangle atau ikon panah)
   - Atau buat background sederhana

3. **Text (TMP):**
   - Text: `← KEMBALI` atau `BACK`
   - Font Size: 24-28
   - Alignment: Center
   - Color: Hitam atau putih (tergantung background)

---

### **LANGKAH 3: Assign BackButtonManager Component**

1. Pilih objek **Canvas** atau objek parent dari UI Chapter 1
2. Cari objek yang cocok untuk menampung script manager (misal: `UIManagers` atau `Canvas`)
3. Di Inspector, klik **Add Component**
4. Ketik: `BackButtonManager` → Enter
5. Component akan muncul di Inspector

---

### **LANGKAH 4: Assign References di BackButtonManager**

Di Inspector, component **BackButtonManager** akan menampilkan 3 field:

#### 1️⃣ **Back Button** (Button)
- Drag objek `BackButton` yang baru dibuat ke field ini

#### 2️⃣ **Calculation Manager** (CalculationManager)
- Drag objek yang memiliki component `CalculationManager` ke field ini
- Biasanya ada di objek bernama `GameManager` atau `CalculationManager`

#### 3️⃣ **Level Selection Manager** (LevelSelectionManager)
- Drag objek yang memiliki component `LevelSelectionManager` ke field ini
- Biasanya ada di objek bernama `LevelSelectionManager`

---

### **LANGKAH 5: Assign BackButtonManager di LevelSelectionManager**

1. Di Hierarchy, pilih objek yang memiliki **LevelSelectionManager** component
2. Di Inspector, scroll ke section **Manager References**
3. Akan ada field baru: **Back Button Manager**
4. Drag objek yang memiliki **BackButtonManager** component ke field ini

---

### **LANGKAH 6: Testing**

#### ✅ **Test Skenario 1: Level Selection → Main Menu**
1. Play Scene Chapter 1
2. Akan muncul **Level Selection Panel** (pilihan Level 1/2/3)
3. Klik tombol **BACK** di header
4. **Hasil:** Scene berpindah ke Main Menu

#### ✅ **Test Skenario 2: Gameplay → Level Selection**
1. Play Scene Chapter 1
2. Pilih salah satu level (misal: Level 1)
3. Tunggu game dimulai (muncul soal)
4. Klik tombol **BACK** di header
5. **Hasil:** Kembali ke Level Selection Panel (tanpa keluar scene)

#### ✅ **Test Skenario 3: Score Save saat Back**
1. Play Scene Chapter 1
2. Pilih level dan jawab beberapa soal dengan benar
3. Perhatikan score bertambah (misal: 30)
4. Klik tombol **BACK**
5. Buka Main Menu → Highscore
6. **Hasil:** Score 30 tersimpan dengan tanggal/waktu hari ini

---

## 🧪 Debugging / Troubleshooting

### ❌ **Problem:** Tombol back tidak muncul
**Solusi:**
- Pastikan tombol tidak ter-disable di Inspector
- Check apakah BackButton ada di layer yang benar (UI layer)
- Pastikan Canvas Scaler sudah diatur dengan benar

### ❌ **Problem:** Klik tombol tidak ada efek
**Solusi:**
- Pastikan ada **EventSystem** di scene
- Check apakah button memiliki Raycast Target enabled
- Pastikan BackButtonManager sudah di-assign dengan benar

### ❌ **Problem:** Mode tidak berubah (selalu ke main menu atau selalu ke level selection)
**Solusi:**
- Check Console log: Cari pesan `[BackButton] Mode: ...`
- Pastikan LevelSelectionManager memanggil `SetLevelSelectionMode()` dan `SetGameplayMode()`
- Pastikan BackButtonManager reference sudah di-assign di LevelSelectionManager

### ❌ **Problem:** Score tidak tersimpan
**Solusi:**
- Check Console log: Cari pesan `[CalculationManager] Saving score before exit: ...`
- Pastikan HighScoreManager.Instance tidak null
- Test dengan menambahkan Debug.Log di BackToLevelSelection()

---

## 📊 Struktur Kode

```
BackButtonManager.cs
├── SetLevelSelectionMode()    // Mode: Back → Main Menu
├── SetGameplayMode()           // Mode: Back → Level Selection
├── OnBackButtonClicked()       // Router berdasarkan mode
├── ShowButton()                // Tampilkan button
├── HideButton()                // Sembunyikan button
├── BackToMainMenu()            // Load scene "Main Menu"
└── BackToLevelSelection()      // Panggil CalculationManager
```

**Integration Flow:**
```
LevelSelectionManager.ShowLevelSelection()
    └── backButtonManager.SetLevelSelectionMode()
        └── Button Mode: Main Menu

LevelSelectionManager.LevelSelectionSequence()
    └── [Game objects shown]
    └── backButtonManager.SetGameplayMode()
        └── Button Mode: Level Selection

OnBackButtonClicked()
    ├── IF isInLevelSelection → BackToMainMenu()
    └── ELSE → BackToLevelSelection()
                    └── CalculationManager.BackToLevelSelection()
                        ├── Save Score (if > 0)
                        ├── Reset Score Display
                        └── ShowLevelSelection()
```

---

## ✅ Checklist Sebelum Commit

- [ ] Tombol back lama sudah dihapus dari answer slot panels
- [ ] Tombol back baru sudah dibuat di header dengan tampilan yang bagus
- [ ] BackButtonManager component sudah di-assign
- [ ] Semua references (BackButton, CalculationManager, LevelSelectionManager) sudah di-assign
- [ ] BackButtonManager reference di LevelSelectionManager sudah di-assign
- [ ] Test Skenario 1 berhasil (Level Selection → Main Menu)
- [ ] Test Skenario 2 berhasil (Gameplay → Level Selection)
- [ ] Test Skenario 3 berhasil (Score tersimpan saat back)

---

## 📝 Catatan Tambahan

- Tombol back akan **SELALU MUNCUL** di header, tidak perlu hide/show manual
- Fungsi tombol berubah **OTOMATIS** berdasarkan state game
- Score **SELALU TERSIMPAN** saat klik back dari gameplay (tidak hilang)
- System ini lebih clean dan user-friendly dibanding multiple buttons

---

**Selesai! 🎉**

Jika ada pertanyaan atau masalah, cek section **Debugging / Troubleshooting** atau lihat Console log di Unity untuk detail error.
