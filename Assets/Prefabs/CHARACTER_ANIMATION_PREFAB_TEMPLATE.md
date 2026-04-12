# Prefab Configuration Template

## CharacterAnimationSystem GameObject Structure

```json
{
  "GameObject": "CharacterAnimationSystem",
  "Components": [
    {
      "Type": "RectTransform",
      "anchorMin": [0.5, 0],
      "anchorMax": [0.5, 0],
      "pivot": [0.5, 0],
      "anchoredPosition": [0, 0],
      "sizeDelta": [1920, 1080]
    },
    {
      "Type": "CharacterAnimationController",
      "characterImage": "Reference to CharacterImage",
      "characterTransform": "Reference to CharacterImage RectTransform",
      "correctAnimationSprites": [
        "sprite_correct_1",
        "sprite_correct_2",
        "sprite_correct_3",
        "sprite_correct_4",
        "sprite_correct_5"
      ],
      "wrongAnimationSprites": [
        "sprite_wrong_1",
        "sprite_wrong_2",
        "sprite_wrong_3",
        "sprite_wrong_4",
        "sprite_wrong_5"
      ],
      "spriteAnimationSpeed": 0.15,
      "bubbleChatPanel": "Reference to BubbleChatPanel",
      "bubbleChatText": "Reference to ChatText",
      "moveUpDuration": 0.8,
      "moveDownDuration": 0.8,
      "displayDuration": 2.5,
      "hiddenPosition": [0, -800],
      "centerPosition": [0, 0],
      "correctMessages": [
        "Hebat! Jawabanmu benar!",
        "Luar biasa! Kamu pintar!",
        "Sempurna! Pertahankan!",
        "Bagus sekali! Terus seperti itu!",
        "Mantap! Kamu memahaminya!"
      ],
      "wrongMessages": [
        "Oops! Coba periksa lagi.",
        "Hmm, belum tepat. Semangat!",
        "Jangan menyerah! Coba lagi.",
        "Hampir! Periksa perhitunganmu.",
        "Yuk, fokus dan coba lagi!"
      ]
    }
  ],
  "Children": [
    {
      "GameObject": "CharacterImage",
      "Components": [
        {
          "Type": "RectTransform",
          "anchorMin": [0.5, 0],
          "anchorMax": [0.5, 0],
          "pivot": [0.5, 0],
          "anchoredPosition": [0, -800],
          "sizeDelta": [250, 250]
        },
        {
          "Type": "Image",
          "sprite": "None (will be set by script)",
          "color": [1, 1, 1, 1],
          "raycastTarget": false
        },
        {
          "Type": "CanvasGroup",
          "alpha": 1,
          "interactable": false,
          "blocksRaycasts": false
        }
      ],
      "Children": [
        {
          "GameObject": "BubbleChatPanel",
          "Components": [
            {
              "Type": "RectTransform",
              "anchorMin": [0.5, 1],
              "anchorMax": [0.5, 1],
              "pivot": [0.5, 0],
              "anchoredPosition": [0, 50],
              "sizeDelta": [350, 120]
            },
            {
              "Type": "Image",
              "sprite": "UI_Bubble or Rounded Rectangle",
              "color": [1, 1, 1, 0.9],
              "imageType": "Sliced",
              "raycastTarget": false
            },
            {
              "Type": "CanvasGroup",
              "alpha": 1,
              "interactable": false,
              "blocksRaycasts": false
            }
          ],
          "Children": [
            {
              "GameObject": "ChatText",
              "Components": [
                {
                  "Type": "RectTransform",
                  "anchorMin": [0, 0],
                  "anchorMax": [1, 1],
                  "pivot": [0.5, 0.5],
                  "anchoredPosition": [0, 0],
                  "offsetMin": [10, 10],
                  "offsetMax": [-10, -10]
                },
                {
                  "Type": "TextMeshProUGUI",
                  "text": "",
                  "font": "Thaleah or Pixel Font",
                  "fontSize": 22,
                  "fontStyle": "Normal",
                  "alignment": "Center",
                  "color": [0, 0, 0, 1],
                  "enableWordWrapping": true,
                  "overflowMode": "Truncate",
                  "enableAutoSizing": false
                }
              ]
            }
          ]
        }
      ]
    }
  ]
}
```

## Unity Scene Hierarchy View

```
Canvas (Chapter1)
├── [Existing UI Elements]
│   ├── Header
│   ├── Triangle
│   ├── Answer Tiles
│   └── Feedback
│
└── CharacterAnimationSystem
    └── CharacterImage (Image)
        └── BubbleChatPanel (Image)
            └── ChatText (TextMeshProUGUI)
```

## Component Order on CharacterAnimationSystem

1. **RectTransform** (built-in)
2. **CharacterAnimationController** (custom script)

## Component Order on CharacterImage

1. **RectTransform** (built-in)
2. **Image** (UI component)
3. **CanvasGroup** (optional, for fade effects)

## Component Order on BubbleChatPanel

1. **RectTransform** (built-in)
2. **Image** (UI component)
3. **CanvasGroup** (optional, for fade effects)

## Inspector Values Quick Reference

### CharacterImage (RectTransform)
```yaml
Anchor Preset: Bottom Center
Pos X: 0
Pos Y: -800  # Hidden below screen
Pos Z: 0
Width: 250
Height: 250
```

### CharacterImage (Image)
```yaml
Source Image: None (dynamic)
Color: White (255, 255, 255, 255)
Material: None
Raycast Target: ❌ Unchecked
```

### BubbleChatPanel (RectTransform)
```yaml
Anchor Preset: Top Center (of parent)
Pos X: 0
Pos Y: 50  # Slightly above character
Pos Z: 0
Width: 350
Height: 120
```

### BubbleChatPanel (Image)
```yaml
Source Image: UI_Bubble or Rounded Rect
Color: White (255, 255, 255, 230)
Image Type: Sliced (for 9-slice scaling)
Raycast Target: ❌ Unchecked
```

### ChatText (TextMeshProUGUI)
```yaml
Text: [Empty - filled by script]
Font Asset: Thaleah or your pixel font
Font Size: 22
Alignment: Center (horizontal & vertical)
Color: Black (0, 0, 0, 255)
Word Wrapping: ✅ Enabled
Overflow: Truncate
Auto Size: ❌ Disabled
Extra Padding: 10px all sides
```

## CharacterAnimationController Script Inspector

### Character Setup
```yaml
Character Image: [CharacterImage (Image)]
Character Transform: [CharacterImage (RectTransform)]
```

### Animation Sprites
```yaml
Correct Animation Sprites:
  Size: 5
  Element 0: [sprite_correct_1]
  Element 1: [sprite_correct_2]
  Element 2: [sprite_correct_3]
  Element 3: [sprite_correct_4]
  Element 4: [sprite_correct_5]

Wrong Animation Sprites:
  Size: 5
  Element 0: [sprite_wrong_1]
  Element 1: [sprite_wrong_2]
  Element 2: [sprite_wrong_3]
  Element 3: [sprite_wrong_4]
  Element 4: [sprite_wrong_5]

Sprite Animation Speed: 0.15
```

### Bubble Chat
```yaml
Bubble Chat Panel: [BubbleChatPanel (GameObject)]
Bubble Chat Text: [ChatText (TextMeshProUGUI)]
```

### Animation Settings
```yaml
Move Up Duration: 0.8
Move Down Duration: 0.8
Display Duration: 2.5
Hidden Position: X:0, Y:-800
Center Position: X:0, Y:0
```

### Random Messages
```yaml
Correct Messages:
  Size: 5
  Element 0: "Hebat! Jawabanmu benar!"
  Element 1: "Luar biasa! Kamu pintar!"
  Element 2: "Sempurna! Pertahankan!"
  Element 3: "Bagus sekali! Terus seperti itu!"
  Element 4: "Mantap! Kamu memahaminya!"

Wrong Messages:
  Size: 5
  Element 0: "Oops! Coba periksa lagi."
  Element 1: "Hmm, belum tepat. Semangat!"
  Element 2: "Jangan menyerah! Coba lagi."
  Element 3: "Hampir! Periksa perhitunganmu."
  Element 4: "Yuk, fokus dan coba lagi!"
```

## Sorting Order & Canvas Hierarchy

```
Canvas (Sort Order: 0)
├── Background (Sort Order: 0)
├── Triangle Visualizer (Sort Order: 1)
├── Answer Tiles (Sort Order: 2)
├── UI Elements (Sort Order: 3)
└── CharacterAnimationSystem (Sort Order: 10) ← Top layer
    └── CharacterImage
        └── BubbleChatPanel (Sort Order: 11) ← Above character
```

**Important:** CharacterAnimationSystem should be at the END of hierarchy to render on top!

## Material & Shader Settings

### For CharacterImage:
```yaml
Material: None (use default UI/Default)
Shader: UI/Default
```

### For BubbleChatPanel:
```yaml
Material: None (use default UI/Default)
Shader: UI/Default
```

### For ChatText:
```yaml
Material: TextMeshPro/Distance Field
Shader: TextMeshPro/Distance Field
```

## Animation Curve Alternatives (If no DOTween)

If DOTween is not available, you can use AnimationCurve:

```csharp
// In CharacterAnimationController.cs
// Replace DOTween with:

[Header("Animation Curves (Fallback)")]
[SerializeField] private AnimationCurve moveUpCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
[SerializeField] private AnimationCurve moveDownCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

// Usage in coroutine:
float t = 0;
while (t < moveUpDuration)
{
    t += Time.deltaTime;
    float normalizedTime = t / moveUpDuration;
    float curveValue = moveUpCurve.Evaluate(normalizedTime);
    Vector2 pos = Vector2.Lerp(hiddenPosition, centerPosition, curveValue);
    characterTransform.anchoredPosition = pos;
    yield return null;
}
```

## Prefab Save Location

Recommended path:
```
Assets/Prefabs/UI/Chapter1/CharacterAnimationSystem.prefab
```

## Notes

1. **Raycast Target** should be DISABLED on all Image components to prevent blocking other UI interactions
2. **CanvasGroup** is optional but useful for fade effects
3. **Sorting Order** ensure character renders on top of other UI
4. **Anchor & Pivot** correctly set for smooth animation from bottom
5. **Font Asset** must be TMP font, not legacy Unity font

## Export as Prefab Steps

1. Setup complete GameObject dengan semua settings
2. Drag `CharacterAnimationSystem` dari Hierarchy ke Project folder
3. Save as `CharacterAnimationSystem.prefab`
4. Untuk scene lain, drag prefab dari Project ke Canvas
5. Assign references di CalculationManager

---

**Template Version:** 1.0  
**Last Updated:** 2026-01-28  
**Compatible With:** Unity 2021.3+
