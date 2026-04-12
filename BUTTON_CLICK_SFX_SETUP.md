# Button Click SFX Setup Guide

## 📋 Overview
Script `ButtonClickSFX.cs` untuk auto-play sound effect setiap kali button diklik. Component ini dapat diattach ke semua UI button untuk memberikan audio feedback yang konsisten.

---

## 🎯 Features
- ✅ Automatic click sound dengan IPointerClickHandler
- ✅ Support GlobalAudioManager (recommended)
- ✅ Support Chapter1AudioManager
- ✅ Support local AudioSource sebagai fallback
- ✅ Custom SFX per button (optional)
- ✅ Volume control per button
- ✅ Hanya play sound jika button interactable

---

## 📁 File Location
```
Assets/
└── Scripts/
    └── UI/
        └── ButtonClickSFX.cs
```

---

## 🚀 Quick Setup

### Method 1: Auto-detect (Recommended)
1. Select button di hierarchy
2. Add Component → `ButtonClickSFX`
3. **Done!** Script akan auto-use GlobalAudioManager

### Method 2: Custom per Button
1. Add ButtonClickSFX component
2. Assign `Custom Click SFX` di Inspector
3. Uncheck `Use Global Audio Manager`
4. Script akan use custom audio clip

---

## ⚙️ Inspector Settings

### Audio Settings
| Field | Description | Default |
|-------|-------------|---------|
| **Custom Click SFX** | AudioClip khusus untuk button ini (optional) | null |
| **Volume** | Volume SFX (0-1) | 1.0 |

### Audio Source
| Field | Description | Default |
|-------|-------------|---------|
| **Use Global Audio Manager** | Use GlobalAudioManager.Instance | ✅ true |
| **Use Chapter1 Audio Manager** | Use Chapter1AudioManager.Instance | ❌ false |

---

## 💡 Use Cases

### Case 1: Standard Button (Use Global SFX)
```
Button GameObject
├── Button (Component)
└── ButtonClickSFX (Component)
    └── Use Global Audio Manager: ✅
```
**Result:** Play default button click SFX dari GlobalAudioManager

---

### Case 2: Special Button (Custom SFX)
```
Button GameObject
├── Button (Component)
└── ButtonClickSFX (Component)
    ├── Custom Click SFX: [YourCustomClip]
    ├── Use Global Audio Manager: ✅
    └── Volume: 0.8
```
**Result:** Play custom SFX instead of default

---

### Case 3: Chapter 1 Specific Button
```
Button GameObject
├── Button (Component)
└── ButtonClickSFX (Component)
    ├── Use Global Audio Manager: ❌
    └── Use Chapter1 Audio Manager: ✅
```
**Result:** Play button click dari Chapter1AudioManager

---

### Case 4: Standalone Button (No Manager)
```
Button GameObject
├── Button (Component)
├── AudioSource (Auto-added)
└── ButtonClickSFX (Component)
    ├── Custom Click SFX: [YourClip]
    ├── Use Global Audio Manager: ❌
    └── Use Chapter1 Audio Manager: ❌
```
**Result:** Play using local AudioSource

---

## 🔧 How It Works

### Priority Order:
1. **GlobalAudioManager** (if enabled)
   - Use custom clip if assigned
   - Otherwise use default button click SFX
2. **Chapter1AudioManager** (if enabled)
3. **Local AudioSource** (fallback)

### Event Flow:
```
User Clicks Button
     ↓
IPointerClickHandler.OnPointerClick()
     ↓
Check if button.interactable
     ↓ (yes)
PlayClickSound()
     ↓
[Priority Check]
     ↓
Play Audio from Manager/AudioSource
```

---

## 📝 Code Examples

### Setup in Code (Runtime)
```csharp
using UnityEngine;
using UnityEngine.UI;

public class ButtonSetup : MonoBehaviour
{
    void Start()
    {
        Button myButton = GetComponent<Button>();
        
        // Add ButtonClickSFX component
        ButtonClickSFX clickSFX = myButton.gameObject.AddComponent<ButtonClickSFX>();
        
        // Optional: Set custom clip
        AudioClip customClip = Resources.Load<AudioClip>("Audio/SpecialClick");
        clickSFX.SetCustomClickSFX(customClip);
        
        // Optional: Set volume
        clickSFX.SetVolume(0.7f);
    }
}
```

### Manual Trigger
```csharp
// If you need to manually trigger sound
ButtonClickSFX sfx = button.GetComponent<ButtonClickSFX>();
sfx.PlayClickSound();
```

---

## 🎨 Batch Setup untuk Multiple Buttons

### Setup Script (Editor)
```csharp
using UnityEngine;
using UnityEngine.UI;

public class BatchAddButtonSFX : MonoBehaviour
{
    [ContextMenu("Add ButtonClickSFX to All Buttons")]
    void AddSFXToAllButtons()
    {
        Button[] allButtons = FindObjectsOfType<Button>(true);
        
        foreach (Button btn in allButtons)
        {
            if (btn.GetComponent<ButtonClickSFX>() == null)
            {
                btn.gameObject.AddComponent<ButtonClickSFX>();
                Debug.Log($"Added ButtonClickSFX to {btn.gameObject.name}");
            }
        }
        
        Debug.Log($"Processed {allButtons.Length} buttons");
    }
}
```

**Usage:**
1. Create empty GameObject
2. Add script above
3. Right-click script in Inspector
4. Select "Add ButtonClickSFX to All Buttons"

---

## ⚠️ Requirements

### Dependencies:
- ✅ UnityEngine.UI
- ✅ UnityEngine.EventSystems
- ✅ Button component (auto-required)

### Optional Dependencies:
- GlobalAudioManager (for global SFX)
- Chapter1AudioManager (for chapter-specific SFX)

---

## 🐛 Troubleshooting

### Problem: No sound when clicking
**Solutions:**
1. ✅ Check button is `interactable` in Inspector
2. ✅ Check GlobalAudioManager exists in scene
3. ✅ Check Button Click SFX assigned in GlobalAudioManager
4. ✅ Check audio volume settings (not muted)
5. ✅ Check ButtonClickSFX component is enabled

### Problem: Wrong sound playing
**Solutions:**
1. Check `Custom Click SFX` field
2. Check which Audio Manager is enabled
3. Verify GlobalAudioManager has correct clip assigned

### Problem: Sound plays twice
**Cause:** Button has both IPointerClickHandler and onClick event calling PlayClickSound()
**Solution:** Remove manual onClick call, let IPointerClickHandler handle it

---

## 🎯 Best Practices

### ✅ DO:
- Use GlobalAudioManager for consistent button sounds
- Add ButtonClickSFX to all interactive buttons
- Use custom SFX for special buttons (confirm, cancel, etc.)
- Keep volume consistent across similar buttons

### ❌ DON'T:
- Don't manually call PlayClickSound() from onClick event (causes double sound)
- Don't attach to non-button UI elements
- Don't use multiple audio managers simultaneously
- Don't forget to assign SFX clips in AudioManagers

---

## 🔗 Related Files
- `GlobalAudioManager.cs` - Manages global button click SFX
- `Chapter1AudioManager.cs` - Chapter-specific audio
- `ButtonClickSFX.cs` - This component script

---

## 📊 Usage Statistics Template
```
Total Buttons in Scene: [number]
Buttons with ButtonClickSFX: [number]
Coverage: [percentage]%

Audio Manager Used:
- GlobalAudioManager: [number] buttons
- Chapter1AudioManager: [number] buttons
- Local AudioSource: [number] buttons
```

---

## 🎬 Testing Checklist
- [ ] Button plays sound when clicked
- [ ] Button does NOT play sound when disabled (interactable = false)
- [ ] Sound volume is appropriate
- [ ] Custom SFX works correctly
- [ ] Multiple rapid clicks don't cause audio glitches
- [ ] Sound works in all scenes (Main Menu, Level Selection, Gameplay)

---

## 📚 Integration Example: Main Menu

```csharp
// MainMenuManager.cs
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;

    void Start()
    {
        // ButtonClickSFX will auto-play sounds
        // No need to manually add onClick listeners for sound
        
        // Just add game logic onClick listeners
        playButton.onClick.AddListener(OnPlayClicked);
        settingsButton.onClick.AddListener(OnSettingsClicked);
        exitButton.onClick.AddListener(OnExitClicked);
    }

    void OnPlayClicked()
    {
        // Sound already played by ButtonClickSFX
        // Just handle game logic here
        SceneManager.LoadScene("LevelSelection");
    }

    void OnSettingsClicked()
    {
        // Open settings panel
    }

    void OnExitClicked()
    {
        Application.Quit();
    }
}
```

---

## 🎵 Audio Asset Checklist

### GlobalAudioManager Inspector:
```
Global Sound Effects:
├── Button Click SFX: [Assign your button click audio file]
└── Transition SFX: [Assign your transition audio file]
```

### Recommended Audio Format:
- **Format:** .wav or .mp3
- **Sample Rate:** 44100 Hz
- **Bit Depth:** 16-bit
- **Duration:** 0.1 - 0.3 seconds (short and snappy)
- **Volume:** Normalized to -3dB

---

**Setup Complete!** 🎉
Sekarang semua button akan memiliki audio feedback yang konsisten dan professional.
