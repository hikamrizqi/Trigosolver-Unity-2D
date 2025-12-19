# 🎓 Trigosolver - Game Edukasi Trigonometri

<div align="center">

![Unity](https://img.shields.io/badge/Unity-6.0-black?style=for-the-badge&logo=unity)
![C#](https://img.shields.io/badge/C%23-.NET_4.x-239120?style=for-the-badge&logo=c-sharp)
![Platform](https://img.shields.io/badge/Platform-Windows-0078D6?style=for-the-badge&logo=windows)
![License](https://img.shields.io/badge/License-Educational-blue?style=for-the-badge)

**Game edukasi interaktif untuk mempelajari trigonometri dengan cara yang menyenangkan!**

[Features](#-fitur-utama) • [Installation](#-instalasi) • [Gameplay](#-cara-bermain) • [Documentation](#-dokumentasi) • [Credits](#-credits)

</div>

---

## 📖 Tentang Project

**Trigosolver** adalah game edukasi 2D berbasis Unity yang dirancang untuk membantu siswa memahami konsep trigonometri, khususnya **Teorema Pythagoras** dan **perbandingan trigonometri** (Sin, Cos, Tan) dengan pendekatan pembelajaran yang interaktif dan visual.

Dikembangkan sebagai bagian dari penelitian skripsi untuk meningkatkan pemahaman konsep matematika melalui gamification dan visualisasi dinamis.

### 🎯 Tujuan Pembelajaran

- Memahami hubungan antara sisi-sisi segitiga siku-siku
- Menguasai konsep perbandingan trigonometri (Sinus, Cosinus, Tangen)
- Melatih kemampuan menghitung dengan Teorema Pythagoras
- Meningkatkan motivasi belajar matematika melalui gameplay yang engaging

---

## ✨ Fitur Utama

### 🔺 Visualisasi Segitiga Dinamis
- **Real-time Triangle Rendering**: Segitiga siku-siku digambar secara dinamis dengan sprite rendering
- **Interactive Labels**: Label sisi (depan, samping, miring) dan sudut (θ) yang responsif
- **Color-coded System**: Setiap sisi memiliki warna berbeda untuk memudahkan identifikasi

### 📊 Sistem Soal Berbasis Pythagoras
- Menggunakan **Pythagorean Triples** untuk soal yang akurat: (3,4,5), (5,12,13), (8,15,17), (7,24,25)
- Perhitungan otomatis nilai Sin, Cos, Tan dengan presisi tinggi
- Sistem validasi jawaban dengan feedback langsung

### 🎮 Mode Pembelajaran
- **Mode Cerita**: Belajar melalui narasi dan cutscene yang menarik
- **Mode Latihan**: Tantangan soal dengan level bertingkat
- **Mode Chapter 2**: Gameplay dengan meriam dan proyektil (physics-based)

### 🎨 UI/UX Modern
- **Animated Menu System**: Animasi drop, bounce, dan sink menggunakan DOTween
- **Parallax Background**: Efek kedalaman visual pada background
- **Responsive Design**: UI yang adaptif untuk berbagai resolusi

### 🔊 Audio System
- Sound effects untuk setiap interaksi
- Audio manager terpisah per chapter
- Support untuk background music

---

## 🛠️ Teknologi yang Digunakan

| Technology | Purpose |
|------------|---------|
| **Unity 6.0** (6000.0.23f1) | Game Engine |
| **Universal Render Pipeline (URP)** | 2D Rendering |
| **TextMesh Pro** | Advanced text rendering |
| **DOTween** | Professional animation library |
| **C# (.NET 4.x)** | Scripting language |
| **Input System** | Modern input handling |

---

## 📥 Instalasi

### Requirements
- **Unity Hub** 3.x atau lebih baru
- **Unity Editor** 6.0 (6000.0.23f1)
- **Windows** 10/11 64-bit
- **RAM** minimal 8GB (rekomendasi 16GB)

### Setup Project

1. **Clone Repository**
   ```bash
   git clone https://github.com/hikamrizqi/Trigosolver-Unity-2D.git
   cd Trigosolver-Unity-2D
   ```

2. **Buka di Unity Hub**
   - Buka Unity Hub
   - Klik **Add** > **Add project from disk**
   - Pilih folder `Trigosolver-Unity-2D`
   - Pastikan Unity version **6.0.23f1** terinstall

3. **Install Dependencies**
   - Unity akan otomatis menginstall packages yang diperlukan
   - Tunggu hingga proses import selesai

4. **Play Project**
   - Buka scene `Assets/Scenes/Main Menu.unity`
   - Klik tombol **Play** di Unity Editor

---

## 🎮 Cara Bermain

### Chapter 1: Perbandingan Trigonometri

1. **Mulai dari Main Menu**
   - Pilih mode pembelajaran (Cerita atau Latihan)
   
2. **Pahami Visualisasi**
   - Perhatikan segitiga yang muncul
   - **Merah**: Sisi Depan (opposite)
   - **Hijau**: Sisi Samping (adjacent)
   - **Biru**: Sisi Miring (hypotenuse)
   - **θ**: Sudut theta yang dimaksud

3. **Hitung Nilai Trigonometri**
   - Sin θ = Depan / Miring
   - Cos θ = Samping / Miring
   - Tan θ = Depan / Samping

4. **Input Jawaban**
   - Masukkan hasil perhitungan pada input field
   - Klik tombol **Check** untuk validasi

### Chapter 2: Proyektil & Sudut Tembak

1. **Atur Sudut Meriam**
   - Gunakan input untuk mengatur sudut elevasi (0-90°)

2. **Tentukan Kecepatan**
   - Set kecepatan awal proyektil

3. **Tembak dan Amati**
   - Klik tombol tembak
   - Perhatikan lintasan parabola peluru
   - Pelajari hubungan sudut dengan jarak tempuh

---

## 📁 Struktur Project

```
Trigosolver-Unity-2D/
├── Assets/
│   ├── Animation/           # Animator controllers & animations
│   ├── Plugins/
│   │   └── Demigiant/
│   │       └── DOTween/     # Animation library
│   ├── Prefabs/            # Reusable game objects
│   ├── Scenes/
│   │   ├── Main Menu.unity
│   │   ├── Stage 1.unity   # Chapter 1: Trigonometry
│   │   └── Stage 2.unity   # Chapter 2: Projectile
│   ├── Scripts/
│   │   ├── Audio/          # Audio management
│   │   ├── Core/
│   │   │   ├── Chapter1/   # Triangle visualizer
│   │   │   └── Chapter2/   # Cannon & projectile
│   │   ├── Data/           # Data generators
│   │   ├── Managers/       # Game managers
│   │   ├── UI/             # UI controllers
│   │   └── Utils/          # Helper utilities
│   ├── Sprite/
│   │   ├── Background/     # Background assets
│   │   ├── Main Menu/      # Menu graphics
│   │   └── Object/         # Game objects sprites
│   ├── TextMesh Pro/       # TMP assets & fonts
│   └── Settings/           # URP & render settings
├── Documentation/
│   ├── USE_CASE_DIAGRAM.md
│   ├── CLASS_DIAGRAM.md
│   ├── SEQUENCE_DIAGRAM.md
│   └── *.md               # Various guides
├── Packages/              # Package dependencies
├── ProjectSettings/       # Unity project config
└── README.md
```

---

## 📚 Dokumentasi

Dokumentasi lengkap tersedia di folder `Documentation/`:

- **[Use Case Diagram](Documentation/USE_CASE_DIAGRAM.md)** - Diagram use case sistem
- **[Class Diagram](Documentation/CLASS_DIAGRAM.md)** - Arsitektur class
- **[Sequence Diagram](Documentation/SEQUENCE_DIAGRAM.md)** - Flow interaksi sistem
- **[Setup Guide](Documentation/SETUP_VISUALISASI.md)** - Panduan setup visualisasi
- **[Troubleshooting](Documentation/TROUBLESHOOTING_VISUALISASI.md)** - Solusi masalah umum

---

## 🎯 Roadmap

- [x] Chapter 1: Perbandingan Trigonometri
- [x] Chapter 2: Proyektil dan Sudut Tembak
- [x] Animated Menu System
- [x] Triangle Visualizer dengan sprites
- [ ] Chapter 3: Grafik Fungsi Trigonometri
- [ ] Sistem Achievement & Progress Tracking
- [ ] Multiplayer Mode (PvP Quiz)
- [ ] Mobile Version (Android/iOS)
- [ ] Localization (Bahasa Indonesia & English)

---

## 🤝 Contributing

Project ini merupakan bagian dari penelitian skripsi dan saat ini belum menerima kontribusi eksternal. Namun, feedback dan saran sangat dihargai!

Jika menemukan bug atau punya ide fitur:
1. Buka [Issues](https://github.com/hikamrizqi/Trigosolver-Unity-2D/issues)
2. Jelaskan masalah/saran dengan detail
3. Attach screenshot jika diperlukan

---

## 📄 License

Project ini dikembangkan untuk tujuan **edukasi dan penelitian**. 

⚠️ **Penggunaan Komersial**: Memerlukan izin tertulis dari penulis.

### Assets Credits

- **DOTween**: © Demigiant - [License](http://dotween.demigiant.com/license.php)
- **TextMesh Pro**: © Unity Technologies
- **Fonts**: M PLUS Rounded 1c (OFL License)
- **Art Assets**: Various free assets from OpenGameArt & itch.io

---

## 👨‍💻 Author

**Rizqi Ackerman Hikam**
- GitHub: [@hikamrizqi](https://github.com/hikamrizqi)
- Project: Skripsi - Game Edukasi Trigonometri

---

## 🙏 Credits

Special thanks to:
- **Unity Community**: Untuk resources dan tutorials
- **DOTween**: Demigiant untuk animation library yang powerful
- **OpenGameArt & Itch.io**: Untuk free art assets

---

<div align="center">

**Made with ❤️ and Unity**

⭐ **Star this repo jika project ini membantu!** ⭐

</div>
