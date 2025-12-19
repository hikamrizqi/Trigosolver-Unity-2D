# 🎬 Setup Opening Video Scene

## 📋 Langkah-langkah Setup

### 1. Buat Scene Baru

1. **File** → **New Scene**
2. Pilih **Empty** atau **Basic (Built-in)**
3. **Ctrl+S** → Save as `Opening Video.unity` di folder `Assets/Scenes/`

---

### 2. Setup Hierarchy

Buat struktur berikut di Hierarchy:

```
Opening Video (Scene)
├── Main Camera
├── VideoPlayerObject
│   └── Video Player (Component)
│   └── Audio Source (Component)
│   └── OpeningVideoController (Script)
└── Canvas (Optional - untuk Skip UI)
    └── SkipButton (Optional)
```

---

### 3. Setup Main Camera

**Pilih Main Camera:**
- **Position**: (0, 0, -10)
- **Projection**: Perspective
- **Background**: Solid Color Black (#000000)
- **Clear Flags**: Solid Color

---

### 4. Setup Video Player GameObject

#### 4.1 Create GameObject
1. **Hierarchy** → **Right Click** → **Create Empty**
2. Rename: `VideoPlayerObject`
3. **Position**: (0, 0, 0)

#### 4.2 Add Video Player Component
1. **Add Component** → Ketik `Video Player` → Add
2. **Setup Video Player:**

| Property | Value |
|----------|-------|
| **Source** | Video Clip |
| **Video Clip** | *Drag MP4 file kesini* |
| **Play On Awake** | ✅ Centang |
| **Wait For First Frame** | ✅ Centang |
| **Loop** | ❌ Uncheck |
| **Playback Speed** | 1 |
| **Render Mode** | Camera Near Plane |
| **Target Camera** | Main Camera |
| **Audio Output Mode** | Audio Source |

#### 4.3 Add Audio Source Component
1. **Add Component** → `Audio Source`
2. Audio Source akan otomatis terisi dari Video Player
3. **Volume**: 1.0

#### 4.4 Add OpeningVideoController Script
1. **Add Component** → Ketik `OpeningVideoController` → Add
2. **Setup Inspector:**

| Field | Value |
|-------|-------|
| **Video Player** | VideoPlayerObject (auto-detect) |
| **Next Scene Name** | "Main Menu" |
| **Transition Delay** | 0.5 |
| **Allow Skip** | ✅ Centang |
| **Skip Delay Time** | 1.0 |
| **Video Volume** | 1.0 |

---

### 5. Import Video File

#### 5.1 Import MP4
1. Buat folder `Assets/Videos/`
2. **Drag & Drop** file `.mp4` ke folder `Assets/Videos/`
3. **Klik video di Project** → Inspector:

| Property | Recommended Value |
|----------|-------------------|
| **Transcode** | ✅ Yes (untuk compatibility) |
| **Codec** | H.264 |
| **Dimensions** | Keep Original atau 1920x1080 |
| **Aspect Ratio** | Keep Original |

#### 5.2 Assign Video ke Video Player
1. Pilih `VideoPlayerObject` di Hierarchy
2. Di Inspector → **Video Player** → **Video Clip**
3. Drag video dari `Assets/Videos/` kesini

---

### 6. Setup Build Settings (PENTING!)

1. **File** → **Build Settings**
2. **Add Open Scenes** → Tambahkan `Opening Video`
3. **Drag** scene `Opening Video` ke **index 0** (paling atas)
4. **Drag** scene `Main Menu` ke **index 1**

**Urutan Scene:**
```
✅ 0: Opening Video
✅ 1: Main Menu
   2: Stage 1
   3: Stage 2
```

---

### 7. (Optional) Tambahkan Skip Button UI

#### 7.1 Create Canvas
1. **Hierarchy** → **Right Click** → **UI** → **Canvas**
2. **Canvas Scaler** → **UI Scale Mode** → `Scale With Screen Size`
3. **Reference Resolution**: 1920 x 1080

#### 7.2 Create Skip Button
1. **Canvas** → **Right Click** → **UI** → **Button - TextMeshPro**
2. Rename: `SkipButton`
3. **Position**: Bottom Right corner
4. **RectTransform:**
   - **Anchor**: Bottom-Right
   - **Pos X**: -150
   - **Pos Y**: 100
   - **Width**: 200
   - **Height**: 60

#### 7.3 Setup Button Text
1. Pilih **SkipButton** → Child **Text (TMP)**
2. **Text**: "Skip >>"
3. **Font Size**: 24
4. **Alignment**: Center

#### 7.4 Link Button ke Script
1. Pilih `SkipButton`
2. **On Click ()** → **+** (Add Event)
3. Drag `VideoPlayerObject` ke object field
4. Function: `OpeningVideoController` → `OnSkipButtonClicked()`

---

## 🎮 Cara Test

### Test di Editor:
1. Buka scene `Opening Video`
2. Klik **Play** ▶️
3. Video akan play otomatis
4. Setelah selesai → Auto pindah ke Main Menu
5. **Atau**: Klik mouse/keyboard untuk skip (setelah 1 detik)

### Test Build:
1. **File** → **Build Settings**
2. Pastikan `Opening Video` di index 0
3. **Build And Run**

---

## 🔧 Troubleshooting

### ❌ Video tidak play
- **Cek**: Video Clip sudah di-assign?
- **Cek**: Camera assigned di Video Player?
- **Cek**: Console ada error?

### ❌ Video play tapi no audio
- **Cek**: Audio Output Mode = Audio Source
- **Cek**: Audio Source component ada?
- **Cek**: Volume > 0?

### ❌ Tidak pindah ke Main Menu
- **Cek**: Next Scene Name = "Main Menu" (exact)
- **Cek**: Scene "Main Menu" sudah di Build Settings?
- **Cek**: Console ada error?

### ❌ Video lag/stuttering
- **Solution**: Re-import video dengan Transcode = Yes
- **Solution**: Reduce video resolution (max 1920x1080)
- **Solution**: Use H.264 codec

---

## 📝 Fitur

✅ Auto-play video on scene load  
✅ Auto-transition ke Main Menu setelah video selesai  
✅ Skip dengan mouse click atau keyboard (setelah 1 detik)  
✅ Smooth audio fade out saat transition  
✅ Optional Skip Button UI  
✅ Error handling & debug logs  

---

## 🎯 Next Steps

Setelah Opening Video selesai:
1. ✅ Video play normal
2. ✅ Auto pindah ke Main Menu
3. ✅ Skip berfungsi
4. Update Build Settings untuk final build
5. (Optional) Add fade transition effect

---

**Happy Creating! 🎬**
