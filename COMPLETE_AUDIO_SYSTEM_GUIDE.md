# 🎵 Complete Audio System Setup Guide

## 📋 Table of Contents
1. [System Overview](#system-overview)
2. [Architecture](#architecture)
3. [Component Details](#component-details)
4. [Setup Instructions](#setup-instructions)
5. [Integration Guide](#integration-guide)
6. [Testing Procedures](#testing-procedures)

---

## 🎯 System Overview

Sistem audio ini dirancang untuk memberikan experience audio yang complete dengan:
- ✅ **BGM (Background Music)** yang berbeda per scene
- ✅ **SFX (Sound Effects)** untuk interaksi user
- ✅ **Persistent audio** across scenes dengan DontDestroyOnLoad
- ✅ **Smooth transitions** dengan audio crossfade
- ✅ **Special game states** (game over stops BGM, resume on back)

### Audio Components:
1. **GlobalAudioManager** - Persistent BGM untuk Main Menu, Story Panel, Gameplay
2. **Chapter1AudioManager** - Scene-specific SFX dan BGM control untuk Chapter 1
3. **ButtonClickSFX** - Helper component untuk button click sounds
4. **CharacterAnimationController** - Integrated dengan audio system

---

## 🏗️ Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                    AUDIO SYSTEM ARCHITECTURE                  │
└─────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────┐
│  GlobalAudioManager (Persistent - DontDestroyOnLoad)        │
│  ┌──────────────────────────────────────────────────────┐  │
│  │  BGM Control:                                         │  │
│  │  - Main Menu BGM                                      │  │
│  │  - Story Panel BGM                                    │  │
│  │  - Gameplay Chapter 1 BGM                             │  │
│  │                                                        │  │
│  │  Global SFX:                                          │  │
│  │  - Button Click SFX                                   │  │
│  │  - Transition SFX                                     │  │
│  │  - PlaySFX(AudioClip) for custom sounds              │  │
│  └──────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
                             │
                             ├─── Auto Scene Detection
                             │    (SceneManager.sceneLoaded)
                             │
                             ▼
    ┌────────────────────────────────────────────────┐
    │  Scene-Specific Managers                       │
    └────────────────────────────────────────────────┘
                             │
                             ├─── Chapter 1 Scene
                             │
                             ▼
    ┌────────────────────────────────────────────────┐
    │  Chapter1AudioManager                          │
    │  ┌─────────────────────────────────────────┐  │
    │  │  Chapter 1 SFX:                         │  │
    │  │  - Correct Answer SFX                   │  │
    │  │  - Wrong Answer SFX                     │  │
    │  │  - Game Over SFX                        │  │
    │  │  - Button Click SFX                     │  │
    │  │                                          │  │
    │  │  BGM Control Integration:               │  │
    │  │  - StopBGMForGameOver()                 │  │
    │  │  - ResumeBGMAfterGameOver()             │  │
    │  │    (delegates to GlobalAudioManager)    │  │
    │  └─────────────────────────────────────────┘  │
    └────────────────────────────────────────────────┘
                             │
                             ├─── Used by:
                             │
    ┌────────────────┬───────┴────────┬──────────────────┐
    │                │                 │                  │
    ▼                ▼                 ▼                  ▼
CalculationManager  GameOverPanel  ButtonClickSFX  CharacterAnimationController
    (Answers)      (Back Button)   (All Buttons)   (Animation Triggers)
```

---

## 📦 Component Details

### 1. GlobalAudioManager.cs
**Location:** `Assets/Scripts/Audio/GlobalAudioManager.cs`

**Purpose:** 
- Central audio controller yang persistent across all scenes
- Manages BGM transitions dengan smooth crossfade
- Provides global SFX playback

**Key Methods:**
```csharp
// BGM Control
PlayMainMenuBGM()              // Play main menu background music
PlayStoryPanelBGM()            // Play story panel background music
PlayGameplayChapter1BGM()       // Play chapter 1 gameplay BGM
StopBGM()                      // Stop BGM with fade out
PauseBGM()                     // Pause BGM (can resume)
ResumeBGM()                    // Resume paused BGM

// SFX Control
PlayButtonClickSFX()           // Play default button click sound
PlayTransitionSFX()            // Play scene transition sound
PlaySFX(AudioClip clip)        // Play any custom audio clip

// Volume Control
SetBGMVolume(float volume)     // Set BGM volume (0-1)
SetSFXVolume(float volume)     // Set SFX volume (0-1)
MuteAll() / UnmuteAll()        // Mute/unmute all audio

// Utility
IsBGMPlaying()                 // Check if BGM is currently playing
GetCurrentBGMType()            // Get current BGM type string
```

**Inspector Fields:**
```
Audio Source:
├── BGM Source: [AudioSource]
└── SFX Source: [AudioSource]

Background Music Clips:
├── Main Menu BGM: [AudioClip]
├── Story Panel BGM: [AudioClip]
└── Gameplay Chapter 1 BGM: [AudioClip]

Global Sound Effects:
├── Button Click SFX: [AudioClip]
└── Transition SFX: [AudioClip]

Volume Settings:
├── BGM Volume: 0.5 (0-1)
└── SFX Volume: 0.7 (0-1)

Fade Settings:
├── Fade In Duration: 1.0s
└── Fade Out Duration: 0.5s
```

---

### 2. Chapter1AudioManager.cs
**Location:** `Assets/Scripts/Chapter1/Chapter1AudioManager.cs`

**Purpose:**
- Chapter-specific SFX management
- BGM control delegation to GlobalAudioManager
- Game state audio handling (game over logic)

**Key Methods:**
```csharp
// SFX Methods
PlayCorrectAnswerSFX()         // Play correct answer sound
PlayWrongAnswerSFX()           // Play wrong answer sound
PlayGameOverSFX()              // Play game over sound
PlayButtonClickSFX()           // Play button click sound

// BGM Control (delegates to GlobalAudioManager)
StopBGMForGameOver()           // Stop BGM when game over occurs
ResumeBGMAfterGameOver()       // Resume BGM when returning from game over
```

**Inspector Fields:**
```
Audio Sources:
├── SFX Source: [AudioSource]
└── Use Global Audio Manager For BGM: ✅ true

Sound Effect Clips:
├── Correct Answer SFX: [AudioClip]
├── Wrong Answer SFX: [AudioClip]
├── Game Over SFX: [AudioClip]
└── Button Click SFX: [AudioClip]

Volume Settings:
└── SFX Volume: 0.7 (0-1)
```

---

### 3. ButtonClickSFX.cs
**Location:** `Assets/Scripts/UI/ButtonClickSFX.cs`

**Purpose:**
- Reusable component untuk button click sounds
- Automatically plays sound saat button diklik
- Support custom SFX per button

**Key Methods:**
```csharp
OnPointerClick(PointerEventData)  // Auto-triggered saat button clicked
PlayClickSound()                  // Manual trigger untuk play sound
SetCustomClickSFX(AudioClip)      // Set custom SFX runtime
SetVolume(float)                  // Set volume runtime
```

**Inspector Fields:**
```
Audio Settings:
├── Custom Click SFX: [AudioClip] (optional)
└── Volume: 1.0 (0-1)

Audio Source:
├── Use Global Audio Manager: ✅ true
└── Use Chapter1 Audio Manager: ❌ false
```

---

## 🚀 Setup Instructions

### Step 1: Create GlobalAudioManager GameObject

1. **Create Empty GameObject in PERSISTENT Scene** (biasanya scene awal yang load):
   ```
   Hierarchy:
   └── GlobalAudioManager
       └── GlobalAudioManager (Script)
   ```

2. **Add AudioSource Components** (2x):
   - BGM Source (loop enabled)
   - SFX Source (loop disabled)

3. **Assign Script**:
   - Add Component → `GlobalAudioManager`

4. **Configure Inspector**:
   ```
   BGM Source: [Drag BGM AudioSource here]
   SFX Source: [Drag SFX AudioSource here]
   
   Main Menu BGM: [Your main menu music file]
   Story Panel BGM: [Your story panel music file]
   Gameplay Chapter 1 BGM: [Your chapter 1 gameplay music file]
   
   Button Click SFX: [Your button click sound file]
   Transition SFX: [Your transition sound file]
   
   BGM Volume: 0.5
   SFX Volume: 0.7
   
   Fade In Duration: 1.0
   Fade Out Duration: 0.5
   ```

5. **Mark as DontDestroyOnLoad** (automatic via script)

---

### Step 2: Setup Chapter1AudioManager

1. **Create Chapter1AudioManager GameObject in Chapter 1 Scene**:
   ```
   Hierarchy (Chapter 1 Scene):
   └── AudioManager
       └── Chapter1AudioManager (Script)
   ```

2. **Add AudioSource Component** (1x):
   - SFX Source (loop disabled)

3. **Configure Inspector**:
   ```
   SFX Source: [Drag AudioSource here]
   Use Global Audio Manager For BGM: ✅ true
   
   Correct Answer SFX: [Your correct sound file]
   Wrong Answer SFX: [Your wrong sound file]
   Game Over SFX: [Your game over sound file]
   Button Click SFX: [Your button click sound file]
   
   SFX Volume: 0.7
   ```

---

### Step 3: Add ButtonClickSFX to Buttons

**Option A: Manual (per button)**
1. Select button in hierarchy
2. Add Component → `ButtonClickSFX`
3. Configure:
   ```
   Use Global Audio Manager: ✅ true
   Volume: 1.0
   ```

**Option B: Batch (all buttons)**
1. Create temporary GameObject
2. Add this script:
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
               }
           }
       }
   }
   ```
3. Right-click script in Inspector → "Add ButtonClickSFX to All Buttons"
4. Delete temporary GameObject

---

### Step 4: Connect Audio References

#### In CalculationManager.cs:
Already integrated! Check Inspector:
```
Audio Manager: [Drag Chapter1AudioManager here]
Character Controller: [Already assigned]
```

#### In GameOverPanel.cs:
Already integrated! Check Inspector:
```
Audio Manager: [Drag Chapter1AudioManager here]
Character Controller: [Already assigned]
```

---

## 🔧 Integration Guide

### Integration Points Already Completed:

#### 1. Answer Verification (CalculationManager)
```csharp
// Correct Answer
if (isCorrect) {
    audioManager.PlayCorrectAnswerSFX();  // ✅ Play correct SFX
    characterController.PlayCorrectAnimation();
    // ... score logic
}

// Wrong Answer
else {
    audioManager.PlayWrongAnswerSFX();  // ✅ Play wrong SFX
    characterController.PlayWrongAnimation();
    currentLives--;
    
    // Game Over
    if (currentLives <= 0) {
        audioManager.PlayGameOverSFX();  // ✅ Play game over SFX
        audioManager.StopBGMForGameOver();  // ✅ Stop BGM
        characterController.PlayGameOverAnimation();
        // ... show game over panel
    }
}
```

#### 2. Return from Game Over (GameOverPanel)
```csharp
// When back button clicked or auto-return
audioManager.ResumeBGMAfterGameOver();  // ✅ Resume BGM
characterController.HideCharacter();  // Hide angry character
// ... return to level selection
```

#### 3. Button Clicks (All Buttons)
```
ButtonClickSFX component automatically handles:
- Play sound on click
- Check if button interactable
- Use GlobalAudioManager or Chapter1AudioManager
```

---

### Additional Integration TODO:

#### Story Panel BGM Trigger
**File to modify:** `StoryPanel.cs` or similar

```csharp
using UnityEngine;

public class StoryPanel : MonoBehaviour
{
    void Start()
    {
        // Play story panel BGM when panel opens
        if (GlobalAudioManager.Instance != null)
        {
            GlobalAudioManager.Instance.PlayStoryPanelBGM();
        }
    }
}
```

#### Main Menu BGM Trigger
**File to modify:** `MainMenuManager.cs` or similar

```csharp
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    void Start()
    {
        // Play main menu BGM
        if (GlobalAudioManager.Instance != null)
        {
            GlobalAudioManager.Instance.PlayMainMenuBGM();
        }
    }
}
```

#### Chapter 1 Gameplay BGM Trigger
**Already handled automatically** by GlobalAudioManager scene detection!

Alternative manual trigger in `Chapter1GameManager.cs`:
```csharp
void Start()
{
    if (GlobalAudioManager.Instance != null)
    {
        GlobalAudioManager.Instance.PlayGameplayChapter1BGM();
    }
}
```

---

## 🧪 Testing Procedures

### Test Suite 1: BGM Transitions
```
Test Steps:
1. ✅ Launch game (Main Menu scene)
   Expected: Main Menu BGM plays automatically
   
2. ✅ Navigate to Story Panel
   Expected: Smooth crossfade to Story Panel BGM
   
3. ✅ Start Chapter 1
   Expected: Crossfade to Gameplay Chapter 1 BGM
   
4. ✅ Return to Main Menu
   Expected: Crossfade back to Main Menu BGM
   
5. ✅ Check GlobalAudioManager persists
   Expected: Same GameObject instance across scenes (DontDestroyOnLoad)
```

### Test Suite 2: Gameplay SFX
```
Test Steps:
1. ✅ Answer correctly
   Expected: 
   - Correct answer SFX plays
   - Character correct animation
   - BGM continues playing
   
2. ✅ Answer incorrectly (not game over)
   Expected:
   - Wrong answer SFX plays
   - Character wrong animation
   - BGM continues playing
   - Lives decrease
   
3. ✅ Answer incorrectly (trigger game over)
   Expected:
   - Wrong answer SFX plays
   - Game over SFX plays
   - BGM stops/pauses
   - Character game over animation
   - Game over panel shows
   
4. ✅ Click back button from game over
   Expected:
   - Button click SFX plays
   - BGM resumes
   - Character hides
   - Return to level selection
```

### Test Suite 3: Button Click SFX
```
Test Steps:
1. ✅ Click any button in Main Menu
   Expected: Button click SFX plays
   
2. ✅ Click any button in Level Selection
   Expected: Button click SFX plays
   
3. ✅ Click any button in Chapter 1
   Expected: Button click SFX plays
   
4. ✅ Try clicking disabled button
   Expected: No sound (button not interactable)
   
5. ✅ Rapid click button multiple times
   Expected: Sounds play without glitching/overlapping issues
```

### Test Suite 4: Volume Controls
```
Test Steps:
1. ✅ Adjust BGM volume via settings
   Expected: BGM volume changes in real-time
   
2. ✅ Adjust SFX volume via settings
   Expected: All SFX play at new volume
   
3. ✅ Mute all audio
   Expected: No sounds play
   
4. ✅ Unmute all audio
   Expected: Audio resumes at previous volumes
```

---

## 📊 Audio Asset Checklist

### GlobalAudioManager Assets Required:
- [ ] **Main Menu BGM** (.mp3/.wav, loop-friendly, ~2-3 minutes)
- [ ] **Story Panel BGM** (.mp3/.wav, atmospheric, ~1-2 minutes)
- [ ] **Gameplay Chapter 1 BGM** (.mp3/.wav, loop-friendly, upbeat, ~2-3 minutes)
- [ ] **Button Click SFX** (.wav, short ~0.1-0.2s, crisp)
- [ ] **Transition SFX** (.wav, short ~0.3-0.5s, whoosh/fade)

### Chapter1AudioManager Assets Required:
- [ ] **Correct Answer SFX** (.wav, positive, ~0.3-0.5s)
- [ ] **Wrong Answer SFX** (.wav, negative/buzz, ~0.3-0.5s)
- [ ] **Game Over SFX** (.wav, dramatic, ~0.5-1.0s)
- [ ] **Button Click SFX** (.wav, optional if using global)

### Recommended Audio Specifications:
```
Format: .wav (uncompressed) or .mp3 (compressed)
Sample Rate: 44100 Hz
Bit Depth: 16-bit
Channels: Stereo (BGM) / Mono (SFX)
Normalization: -3dB peak to prevent clipping
```

---

## 🐛 Troubleshooting

### Issue: No BGM plays
**Check:**
1. GlobalAudioManager GameObject exists in scene?
2. AudioClips assigned in Inspector?
3. BGM Source AudioSource configured? (loop enabled)
4. Volume > 0?
5. Audio not muted?

### Issue: SFX not playing
**Check:**
1. Chapter1AudioManager exists in Chapter 1 scene?
2. SFX AudioClips assigned?
3. SFX Source AudioSource configured?
4. Volume > 0?
5. ButtonClickSFX component attached to buttons?

### Issue: BGM doesn't stop on game over
**Check:**
1. Chapter1AudioManager.StopBGMForGameOver() called?
2. GlobalAudioManager reference correct?
3. useGlobalAudioManagerForBGM = true?
4. Check CalculationManager audio integration

### Issue: BGM doesn't resume after back
**Check:**
1. GameOverPanel.audioManager assigned?
2. ResumeBGMAfterGameOver() called in both methods?
3. GlobalAudioManager still exists (DontDestroyOnLoad)?

### Issue: Button clicks play sound twice
**Cause:** Both IPointerClickHandler and onClick event calling PlayClickSound()
**Solution:** Remove manual onClick sound triggers

---

## 🎯 Best Practices

### ✅ DO:
- Use GlobalAudioManager for persistent BGM
- Use Chapter1AudioManager for chapter-specific SFX
- Add ButtonClickSFX to all interactive buttons
- Keep audio files optimized (compressed, appropriate length)
- Test volume levels across all scenes
- Provide volume control settings to users
- Use fade transitions for professional feel

### ❌ DON'T:
- Don't destroy GlobalAudioManager manually
- Don't play BGM directly without AudioManager
- Don't forget to assign audio clips in Inspector
- Don't use very large audio files (optimize size)
- Don't set volume too high (risk of clipping)
- Don't overlap multiple BGMs simultaneously

---

## 📈 Performance Considerations

### Memory Management:
- AudioClips loaded in memory when assigned
- Use compressed formats (.mp3) for large BGM files
- Use uncompressed (.wav) for short SFX
- Unity loads audio on demand (streaming for large files)

### CPU Usage:
- AudioSource components are lightweight
- Crossfade coroutines use minimal CPU
- Multiple SFX can play simultaneously without issues

### Optimization Tips:
```
BGM Files:
- Compress to .mp3 or .ogg
- Use Unity's "Streaming" load type
- Enable "Load in Background"

SFX Files:
- Keep .wav format for quality
- Use Unity's "Decompress on Load"
- Keep files < 1MB each
```

---

## 🔗 Related Documentation

- [CHARACTER_ANIMATION_SETUP_GUIDE.md](CHARACTER_ANIMATION_SETUP_GUIDE.md)
- [BUTTON_CLICK_SFX_SETUP.md](BUTTON_CLICK_SFX_SETUP.md)
- [GAME_OVER_ANIMATION_FEATURE.md](GAME_OVER_ANIMATION_FEATURE.md)

---

## ✅ Final Setup Checklist

### Pre-Setup:
- [ ] Prepare all audio files (BGM + SFX)
- [ ] Verify audio file formats and specifications
- [ ] Backup project before integration

### GlobalAudioManager:
- [ ] Create GlobalAudioManager GameObject
- [ ] Add 2 AudioSource components
- [ ] Attach GlobalAudioManager script
- [ ] Assign all audio clips
- [ ] Configure volumes
- [ ] Test DontDestroyOnLoad behavior

### Chapter1AudioManager:
- [ ] Create Chapter1AudioManager GameObject in Chapter 1 scene
- [ ] Add 1 AudioSource component
- [ ] Attach Chapter1AudioManager script
- [ ] Assign all SFX clips
- [ ] Enable "Use Global Audio Manager For BGM"
- [ ] Configure SFX volume

### ButtonClickSFX:
- [ ] Add ButtonClickSFX to all buttons (batch or manual)
- [ ] Verify "Use Global Audio Manager" enabled
- [ ] Test button clicks play sounds
- [ ] Verify disabled buttons don't play sounds

### Integration:
- [ ] Assign Chapter1AudioManager reference in CalculationManager
- [ ] Assign Chapter1AudioManager reference in GameOverPanel
- [ ] Add story panel BGM trigger
- [ ] Add main menu BGM trigger
- [ ] Test all audio triggers

### Testing:
- [ ] Test BGM transitions between scenes
- [ ] Test correct answer SFX + animation
- [ ] Test wrong answer SFX + animation
- [ ] Test game over SFX + BGM stop
- [ ] Test BGM resume on back button
- [ ] Test button click SFX on all buttons
- [ ] Test volume controls
- [ ] Test mute/unmute

### Documentation:
- [ ] Document any customizations made
- [ ] Note which audio files used for each clip
- [ ] Create backup of fully configured AudioManagers
- [ ] Share setup guide with team

---

**Setup Complete!** 🎉🎵

Your game now has a professional audio system with:
- ✅ Dynamic BGM based on scene
- ✅ Comprehensive SFX for all interactions
- ✅ Game state audio management
- ✅ Smooth audio transitions
- ✅ User volume controls

**Next Steps:**
1. Import your audio files
2. Assign clips in AudioManager Inspectors
3. Test the complete experience
4. Adjust volumes as needed
5. Enjoy your enhanced game! 🎮
