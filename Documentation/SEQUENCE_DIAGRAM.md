# Sequence Diagram - Trigosolver Game

## 1. Main Menu Flow

### Logo to Main Menu Transition

```mermaid
sequenceDiagram
    actor Player
    participant App as Application
    participant MMM as MainMenuManager
    participant LA as Logo Animator
    participant MMA as MainMenu Animator
    
    Player->>App: Start Game
    App->>MMM: Awake()
    MMM->>MMM: Hide all panels except Logo
    App->>MMM: Start()
    MMM->>LA: AnimateDropIn()
    LA->>LA: Set position above screen
    LA->>LA: Animate drop with bounce
    LA-->>MMM: onComplete callback
    MMM->>MMM: clickAnywhereEnabled = true
    
    Player->>MMM: Click Anywhere / Press Key
    MMM->>MMM: TransitionToMainMenu()
    MMM->>LA: AnimateSinkOut()
    LA->>LA: Animate sink to bottom
    LA-->>MMM: onComplete callback
    MMM->>MMA: AnimateDropIn()
    MMA->>MMA: Animate drop with bounce
    MMA-->>Player: Main Menu Visible
```

### Main Menu to Chapter Selection

```mermaid
sequenceDiagram
    actor Player
    participant MMM as MainMenuManager
    participant MMA as MainMenu Animator
    participant MSA as ModeSelection Animator
    participant MCA as ModeCerita Animator
    
    Player->>MMM: Click "Mulai"
    MMM->>MMM: OnMulaiClicked()
    MMM->>MMA: AnimateSinkOut()
    MMA->>MMA: Animate sink
    MMA-->>MMM: onComplete
    MMM->>MSA: AnimateDropIn()
    MSA-->>Player: Mode Selection Visible
    
    Player->>MMM: Click "Mode Cerita"
    MMM->>MMM: OnModeCeritaClicked()
    MMM->>MSA: AnimateSinkOut()
    MSA-->>MMM: onComplete
    MMM->>MCA: AnimateDropIn()
    MCA-->>Player: Chapter Selection Visible
    
    Player->>MMM: Select Chapter 1
    MMM->>MMM: LoadScene("Chapter1")
```

---

## 2. Gameplay Flow - Start New Question

```mermaid
sequenceDiagram
    actor Player
    participant CM as CalculationManager
    participant TDG as TriangleDataGenerator
    participant UI as UIManagerChapter1
    participant TV as TriangleVisualizer
    participant AM as Chapter1AudioManager
    
    Player->>CM: Scene Loaded
    CM->>CM: Start()
    CM->>TDG: GenerateQuestion()
    TDG->>TDG: GetRandomTriple()
    TDG->>TDG: Random question type (Sin/Cos/Tan)
    TDG->>TDG: Calculate correct answer
    TDG-->>CM: return TriangleData
    
    CM->>CM: soalSekarang++
    CM->>UI: SetupNewQuestion(progress, total, data)
    
    UI->>UI: Update progresText
    UI->>UI: Update pertanyaanText
    UI->>UI: Update labels (depan, samping, miring)
    UI->>TV: DrawTriangle(depan, samping, miring)
    
    TV->>TV: Calculate vertex positions
    TV->>TV: PositionSprite(depanSprite)
    TV->>TV: PositionSprite(sampingSprite)
    TV->>TV: PositionSprite(miringSprite)
    TV->>TV: PositionUILabel(depanLabel)
    TV->>TV: PositionUILabel(sampingLabel)
    TV->>TV: PositionUILabel(miringLabel)
    TV->>TV: Position thetaLabel (world space)
    TV->>TV: ResetColors()
    
    TV-->>UI: Triangle rendered
    UI-->>Player: Question displayed
```

---

## 3. Correct Answer Flow

```mermaid
sequenceDiagram
    actor Player
    participant CM as CalculationManager
    participant UI as UIManagerChapter1
    participant TV as TriangleVisualizer
    participant AM as AudioManager
    
    Player->>UI: Input "0.6"
    Player->>CM: Click "CHECK" button
    CM->>CM: CheckAnswer()
    CM->>CM: ParseUserInput("0.6")
    CM-->>CM: userAnswer = 0.6f
    
    CM->>CM: IsAnswerCorrect(0.6)
    CM->>CM: Math.Abs(0.6 - 0.6) < 0.01?
    CM-->>CM: return true
    
    CM->>CM: HandleCorrectAnswer()
    CM->>CM: score += 10
    CM->>CM: soalBenar++
    
    CM->>UI: ShowFeedback("Benar! +10 poin", true)
    UI->>UI: feedbackText.text = message
    UI->>UI: feedbackPanel.color = green
    UI->>UI: feedbackPanel.SetActive(true)
    
    CM->>TV: ShowFeedback(sideName, true)
    TV->>TV: HighlightSide(sideName, correctColor)
    TV->>TV: sprite.color = green
    
    CM->>AM: PlayCorrectSound()
    AM->>AM: sfxSource.PlayOneShot(correctSFX)
    
    CM->>UI: PlaySparkleEffect()
    UI->>UI: sparkleEffect.Play()
    
    CM->>CM: Wait 1.5 seconds
    CM->>CM: LoadNextQuestion()
    
    alt More questions available
        CM->>TDG: GenerateQuestion()
        Note over CM,TV: Repeat question setup flow
    else All questions completed
        CM->>CM: EndGame(isWin: true)
        CM->>UI: Show "Level Complete" panel
        CM->>AM: PlayCompleteSound()
    end
```

---

## 4. Wrong Answer Flow

```mermaid
sequenceDiagram
    actor Player
    participant CM as CalculationManager
    participant UI as UIManagerChapter1
    participant TV as TriangleVisualizer
    participant AM as AudioManager
    
    Player->>UI: Input "0.8" (wrong)
    Player->>CM: Click "CHECK"
    CM->>CM: CheckAnswer()
    CM->>CM: ParseUserInput("0.8")
    CM-->>CM: userAnswer = 0.8f
    
    CM->>CM: IsAnswerCorrect(0.8)
    CM->>CM: Math.Abs(0.8 - 0.6) = 0.2
    CM->>CM: 0.2 < 0.01? → false
    CM-->>CM: return false
    
    CM->>CM: HandleWrongAnswer()
    CM->>CM: lives--
    CM->>CM: soalSalah++
    
    CM->>UI: ShowFeedback("Salah! Jawaban: 0.6", false)
    UI->>UI: feedbackText.text = message
    UI->>UI: feedbackPanel.color = red
    UI->>UI: feedbackPanel.SetActive(true)
    
    CM->>TV: ShowFeedback(sideName, false)
    TV->>TV: HighlightSide(sideName, wrongColor)
    TV->>TV: sprite.color = red
    
    CM->>AM: PlayWrongSound()
    AM->>AM: sfxSource.PlayOneShot(wrongSFX)
    
    CM->>UI: UpdateLives(lives)
    UI->>UI: livesIcons[lives].SetActive(false)
    
    alt lives > 0
        CM->>CM: Wait 2 seconds
        CM->>CM: LoadNextQuestion()
        Note over CM,TV: Continue to next question
    else lives == 0
        CM->>CM: EndGame(isWin: false)
        CM->>UI: Show "Game Over" panel
        UI-->>Player: Display final score
    end
```

---

## 5. Triangle Visualization Detail

```mermaid
sequenceDiagram
    participant TV as TriangleVisualizer
    participant DS as depanSprite
    participant SS as sampingSprite
    participant MS as miringSprite
    participant DL as depanLabel (UI)
    participant SL as sampingLabel (UI)
    participant ML as miringLabel (UI)
    participant TL as thetaLabel (World)
    participant Cam as Main Camera
    
    TV->>TV: DrawTriangle(3, 4, 5)
    TV->>TV: basePosition = transform.position + centerPosition
    TV->>TV: bottomLeft = basePosition
    TV->>TV: bottomRight = bottomLeft + (4 * 0.5, 0, 0)
    TV->>TV: topLeft = bottomLeft + (0, 3 * 0.5, 0)
    
    Note over TV: Position SAMPING (horizontal)
    TV->>TV: PositionSprite(sampingSprite, bottomLeft, bottomRight, 4)
    TV->>SS: transform.position = midpoint
    TV->>SS: Calculate angle = 0° (horizontal)
    TV->>SS: transform.rotation = Quaternion.Euler(0, 0, -90°)
    TV->>SS: transform.localScale = (lineThickness, scaleY, 1)
    
    TV->>TV: PositionUILabel(sampingLabel, midpoint + offset)
    TV->>Cam: WorldToScreenPoint(worldPos)
    Cam-->>TV: screenPoint
    TV->>SL: rectTransform.position = screenPoint
    TV->>SL: text = "4"
    
    Note over TV: Position DEPAN (vertical)
    TV->>TV: PositionSprite(depanSprite, bottomLeft, topLeft, 3)
    TV->>DS: transform.position = midpoint
    TV->>DS: Calculate angle = 90° (vertical)
    TV->>DS: transform.rotation = Quaternion.Euler(0, 0, 0°)
    TV->>DS: transform.localScale = (lineThickness, scaleY, 1)
    
    TV->>TV: PositionUILabel(depanLabel, midpoint + offset)
    TV->>DL: Update position and text
    
    Note over TV: Position MIRING (diagonal)
    TV->>TV: PositionSprite(miringSprite, topLeft, bottomRight, 5)
    TV->>MS: transform.position = midpoint
    TV->>MS: Calculate angle = ~53° (diagonal)
    TV->>MS: transform.rotation = Quaternion.Euler(0, 0, angle - 90)
    TV->>MS: transform.localScale = (lineThickness, scaleY, 1)
    
    TV->>TV: Calculate perpendicular offset
    TV->>TV: PositionUILabel(miringLabel, midpoint + perpOffset)
    TV->>ML: Update position and text
    
    Note over TV: Position THETA symbol
    TV->>TL: text = "θ"
    TV->>TL: transform.position = bottomLeft + (0.8, 0.8, 0)
    
    TV->>TV: ResetColors()
    TV->>DS: color = normalColor (white)
    TV->>SS: color = normalColor (white)
    TV->>MS: color = normalColor (white)
```

---

## 6. Audio System Flow

```mermaid
sequenceDiagram
    actor Player
    participant CM as CalculationManager
    participant AM as AudioManager
    participant BGM as bgmSource
    participant SFX as sfxSource
    
    Note over AM: Scene Start
    AM->>AM: Start()
    AM->>BGM: clip = backgroundMusic
    AM->>BGM: loop = true
    AM->>BGM: volume = 0.5f
    AM->>BGM: Play()
    
    Note over AM: Correct Answer
    CM->>AM: PlayCorrectSound()
    AM->>SFX: PlayOneShot(correctSFX)
    SFX-->>Player: Play ding sound
    
    Note over AM: Wrong Answer
    CM->>AM: PlayWrongSound()
    AM->>SFX: PlayOneShot(wrongSFX)
    SFX-->>Player: Play buzz sound
    
    Note over AM: Level Complete
    CM->>AM: PlayCompleteSound()
    AM->>SFX: PlayOneShot(completeSFX)
    SFX-->>Player: Play victory fanfare
    
    Note over AM: Settings Changed
    Player->>AM: SetBGMVolume(0.7f)
    AM->>BGM: volume = 0.7f
    AM->>AM: Save to PlayerPrefs
    
    Player->>AM: SetSFXVolume(0.8f)
    AM->>SFX: volume = 0.8f
    AM->>AM: Save to PlayerPrefs
```

---

## 7. Back Navigation Flow

```mermaid
sequenceDiagram
    actor Player
    participant MMM as MainMenuManager
    participant MCA as ModeCerita Animator
    participant MSA as ModeSelection Animator
    participant MMA as MainMenu Animator
    
    Note over Player: User in Chapter Selection
    Player->>MMM: Click "Back"
    MMM->>MMM: OnBackFromModeCerita()
    MMM->>MCA: AnimateSinkOut()
    MCA-->>MMM: onComplete
    MMM->>MSA: AnimateDropIn()
    MSA-->>Player: Mode Selection visible
    
    Note over Player: User in Mode Selection
    Player->>MMM: Click "Back"
    MMM->>MMM: OnBackFromModeSelection()
    MMM->>MSA: AnimateSinkOut()
    MSA-->>MMM: onComplete
    MMM->>MMA: AnimateDropIn()
    MMA-->>Player: Main Menu visible
```

---

## 8. Game Over / Level Complete Flow

```mermaid
sequenceDiagram
    actor Player
    participant CM as CalculationManager
    participant UI as UIManagerChapter1
    participant AM as AudioManager
    participant SM as SceneManager
    
    alt Game Over (lives == 0)
        CM->>CM: EndGame(isWin: false)
        CM->>CM: gameEnded = true
        CM->>UI: Show GameOver Panel
        UI->>UI: gameOverPanel.SetActive(true)
        UI->>UI: finalScoreText = score
        CM->>AM: Stop BGM
        
        Player->>UI: Click "Restart"
        UI->>SM: LoadScene("Chapter1")
        SM-->>Player: Scene reloaded
        
    else Level Complete (all questions done)
        CM->>CM: EndGame(isWin: true)
        CM->>CM: gameEnded = true
        CM->>CM: Calculate accuracy
        CM->>UI: Show LevelComplete Panel
        UI->>UI: completePanel.SetActive(true)
        UI->>UI: Display score, accuracy, time
        CM->>AM: PlayCompleteSound()
        
        alt Check if new highscore
            CM->>CM: score > PlayerPrefs highscore?
            CM->>CM: Save new highscore
            UI->>UI: Show "New Record!" badge
        end
        
        Player->>UI: Click "Next Chapter"
        UI->>SM: LoadScene("Chapter2")
        
        Player->>UI: Click "Main Menu"
        UI->>SM: LoadScene("MainMenu")
    end
```

---

## PlantUML Version - Main Gameplay Flow

```plantuml
@startuml
actor Player
participant "CalculationManager" as CM
participant "TriangleDataGenerator" as TDG
participant "UIManagerChapter1" as UI
participant "TriangleVisualizer" as TV
participant "AudioManager" as AM

== Game Start ==
Player -> CM: Scene Loaded
activate CM
CM -> TDG: GenerateQuestion()
activate TDG
TDG -> TDG: GetRandomTriple()
TDG -> TDG: Random type (Sin/Cos/Tan)
TDG --> CM: TriangleData
deactivate TDG

CM -> UI: SetupNewQuestion(data)
activate UI
UI -> UI: Update UI texts
UI -> TV: DrawTriangle(depan, samping, miring)
activate TV
TV -> TV: Calculate positions
TV -> TV: Position sprites
TV -> TV: Position labels
TV --> UI: Triangle rendered
deactivate TV
UI --> Player: Question displayed
deactivate UI

== Player Answers ==
Player -> UI: Input answer
Player -> CM: Click CHECK

CM -> CM: ParseUserInput()
CM -> CM: IsAnswerCorrect()

alt Answer is Correct
    CM -> CM: HandleCorrectAnswer()
    CM -> UI: ShowFeedback("Benar!", true)
    CM -> TV: ShowFeedback(side, true)
    CM -> AM: PlayCorrectSound()
    CM -> UI: PlaySparkleEffect()
    
    alt More questions
        CM -> TDG: GenerateQuestion()
        note right: Continue to next question
    else All done
        CM -> CM: EndGame(true)
        CM -> UI: Show Level Complete
        CM -> AM: PlayCompleteSound()
    end
    
else Answer is Wrong
    CM -> CM: HandleWrongAnswer()
    CM -> UI: ShowFeedback("Salah!", false)
    CM -> TV: ShowFeedback(side, false)
    CM -> AM: PlayWrongSound()
    CM -> UI: UpdateLives(lives)
    
    alt Lives > 0
        CM -> TDG: GenerateQuestion()
        note right: Continue with reduced lives
    else Lives == 0
        CM -> CM: EndGame(false)
        CM -> UI: Show Game Over
    end
end

deactivate CM
@enduml
```

---

## Timing Diagram - Animation Sequence

```mermaid
gantt
    title Menu Animation Timeline
    dateFormat X
    axisFormat %Ls
    
    section Logo
    Drop Animation (0.8s)     :a1, 0, 800ms
    Bounce Effect            :a2, 600ms, 200ms
    Click Wait               :a3, 800ms, 2000ms
    
    section Transition
    Logo Sink (0.6s)         :b1, 2800ms, 600ms
    
    section Main Menu
    Drop Animation (0.8s)    :c1, 3400ms, 800ms
    Bounce Effect            :c2, 4000ms, 200ms
    User Interaction         :c3, 4200ms, 3000ms
```

---

## State Transition Summary

```
[App Start] 
    → [Logo Animation] 
    → [Click Anywhere] 
    → [Main Menu] 
    → [Mode Selection] 
    → [Chapter Selection] 
    → [Gameplay]
    
[Gameplay Loop]
    → [Generate Question]
    → [Display Triangle]
    → [Wait Input]
    → [Validate Answer]
    → [Feedback]
    → Back to Generate Question (if not done)
    
[Game End]
    → [Level Complete / Game Over]
    → [Show Stats]
    → [Return to Menu / Next Level]
```

---

## Key Interaction Points

1. **Player → MainMenuManager**: Button clicks untuk navigasi menu
2. **CalculationManager → UIManagerChapter1**: Update semua UI elements
3. **UIManagerChapter1 → TriangleVisualizer**: Request render segitiga
4. **TriangleVisualizer → Sprites/Labels**: Direct manipulation visual elements
5. **CalculationManager → AudioManager**: Trigger sound effects
6. **TriangleDataGenerator → TriangleData**: Create question data objects

---

**Generated**: December 18, 2025  
**Diagrams**: 8 detailed sequence flows  
**Format**: Mermaid + PlantUML  
**Coverage**: Menu navigation, gameplay loop, visual rendering, audio system
