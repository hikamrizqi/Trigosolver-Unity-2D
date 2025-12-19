# 🔊 Setup Button Sound Effects

## 📋 Cara Setup (2 Method)

---

## Method 1: Global Setup (Recommended - Otomatis untuk SEMUA button)

### 1. Buat GameObject untuk Sound Manager

1. **Hierarchy** → **Right Click** → **Create Empty**
2. Rename: `ButtonSoundManager`
3. **Add Component** → `Global Button Sound Manager`
4. **Add Component** → `Audio Source` (otomatis ditambahkan)

### 2. Setup Inspector

**Global Button Sound Manager:**

| Field | Value |
|-------|-------|
| **Default Click Sound** | *Drag audio clip kesini* |
| **Default Volume** | 1.0 |
| **Default Pitch** | 1.0 |
| **Auto Setup All Buttons** | ✅ Centang |

### 3. Import Sound Effect

1. Buat folder `Assets/Audio/SFX/`
2. Drag file audio (`.wav`, `.mp3`, `.ogg`) kesana
3. Klik audio → Inspector:
   - **Load Type**: Decompress On Load (untuk SFX pendek)
   - **Preload Audio Data**: ✅ Centang

4. **Drag audio** ke field **Default Click Sound** di GlobalButtonSoundManager

### 4. Done!

✅ Semua button di scene akan otomatis mendapat sound effect saat diklik!

---

## Method 2: Manual Setup (Per-Button)

Jika ingin sound berbeda untuk button tertentu:

### 1. Pilih Button di Hierarchy

Contoh: `PlayButton`, `SkipButton`, etc.

### 2. Add Component

**Add Component** → `Button Sound Effect`

### 3. Setup Inspector

**Button Sound Effect:**

| Field | Value |
|-------|-------|
| **Click Sound** | *Drag audio clip* (custom untuk button ini) |
| **Volume** | 1.0 |
| **Pitch** | 1.0 |
| **Audio Source** | Leave empty (use global) |

### 4. Custom Sound Per Button

Kamu bisa assign sound berbeda untuk setiap button:
- **Play Button**: `button_click.wav`
- **Back Button**: `button_back.wav`
- **Skip Button**: `button_skip.wav`

---

## 🎵 Contoh Struktur Audio

```
Assets/
├── Audio/
│   ├── SFX/
│   │   ├── button_click.wav       # Default click
│   │   ├── button_hover.wav       # Hover effect (opsional)
│   │   ├── button_back.wav        # Back button
│   │   ├── button_confirm.wav     # Confirm/OK button
│   │   └── button_cancel.wav      # Cancel button
│   └── Music/
│       └── ...
```

---

## 🎮 Penggunaan

### Otomatis (dengan GlobalButtonSoundManager):

```
1. Start game
2. GlobalButtonSoundManager auto-setup semua button
3. Klik button apapun → Sound play!
```

### Manual Call (dari script):

```csharp
// Play default sound
GlobalButtonSoundManager.Instance.PlayDefaultClickSound();

// Play custom sound
AudioClip customSound = ...; // Load audio clip
GlobalButtonSoundManager.Instance.PlaySound(customSound, volume: 0.8f, pitch: 1.2f);
```

---

## ⚙️ Advanced Features

### Disable Sound untuk Button Tertentu

1. Pilih button yang ingin disable sound
2. **ButtonSoundEffect** component → **Click Sound** = `None`
3. **Volume** = `0`

### Change Sound Runtime

```csharp
// Ganti default sound
AudioClip newSound = Resources.Load<AudioClip>("Audio/SFX/new_click");
GlobalButtonSoundManager.Instance.SetDefaultClickSound(newSound);

// Ganti volume
GlobalButtonSoundManager.Instance.SetDefaultVolume(0.5f);
```

### Button dengan Pitch Berbeda

Untuk variasi sound (tidak monoton):

**Button 1:**
- Pitch: `0.9`

**Button 2:**
- Pitch: `1.0`

**Button 3:**
- Pitch: `1.1`

Ini bikin sound terdengar sedikit beda tapi masih natural!

---

## 🔧 Troubleshooting

### ❌ Sound tidak play

**Cek:**
- Audio clip sudah di-assign?
- Volume > 0?
- Audio Source component ada?
- Console ada error?

### ❌ Sound delay/lag

**Solution:**
- Audio clip → **Load Type** = `Decompress On Load`
- Audio clip → **Compression Format** = `PCM`
- File size kecil (< 100KB untuk SFX)

### ❌ Sound play 2x (double)

**Cause:** Button punya 2 onClick event yang call sound

**Solution:**
- Cek Button → OnClick() event
- Pastikan hanya ada 1 sound call

### ❌ Semua button dapat sound kecuali 1

**Cause:** Button tidak ketemu saat auto-setup (inactive atau di child canvas)

**Solution:**
- Pilih button tersebut
- Manual add **ButtonSoundEffect** component

---

## 📊 Scenes Setup

Untuk multi-scene (Main Menu, Stage 1, Stage 2):

### Option 1: DontDestroyOnLoad (Recommended)

`GlobalButtonSoundManager` sudah pakai `DontDestroyOnLoad()`, jadi:
- Buat sekali di **Opening Video** atau **Main Menu** scene
- Manager akan persist ke scene lain
- Sound work di semua scene!

### Option 2: Per-Scene Manager

Jika ingin sound berbeda per scene:
1. Buat `ButtonSoundManager` di setiap scene
2. Assign default sound berbeda
3. Disable **DontDestroyOnLoad** di script

---

## 🎯 Recommended Settings

### For Click Sound:
- **Duration**: 50-200ms
- **Format**: WAV atau OGG
- **Sample Rate**: 44100 Hz
- **Volume**: 0.7 - 1.0
- **Pitch**: 0.9 - 1.1 (untuk variasi)

### Free Sound Resources:
- [Freesound.org](https://freesound.org/)
- [Zapsplat](https://www.zapsplat.com/)
- [Mixkit](https://mixkit.co/free-sound-effects/)

Search: "button click", "ui click", "menu select"

---

**Setup done! Semua button sekarang punya sound effect! 🔊**
