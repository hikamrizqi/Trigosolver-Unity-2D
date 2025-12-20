using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// System untuk input jawaban menggunakan button (Duolingo-style)
/// Dipakai untuk 10 soal pertama Chapter 1
/// </summary>
public class ButtonAnswerSystem : MonoBehaviour
{
    [Header("Display")]
    [SerializeField] private TextMeshProUGUI answerDisplayText;
    [SerializeField] private TMP_InputField hiddenInputField; // Link ke jawabanInput di UIManager
    
    [Header("Number Buttons")]
    [SerializeField] private Button btn1;
    [SerializeField] private Button btn2;
    [SerializeField] private Button btn3;
    [SerializeField] private Button btn4;
    [SerializeField] private Button btn5;
    
    [Header("Operator Buttons")]
    [SerializeField] private Button btnSlash;    // /
    [SerializeField] private Button btnDot;      // .
    
    [Header("Control Buttons")]
    [SerializeField] private Button btnDelete;   // DEL - hapus terakhir
    [SerializeField] private Button btnClear;    // CLEAR - hapus semua
    
    [Header("Settings")]
    [SerializeField] private int maxLength = 10; // Max panjang jawaban
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color pressedColor = new Color(0.8f, 0.8f, 0.8f);
    
    private string currentAnswer = "";
    
    private void Start()
    {
        // Setup button listeners
        if (btn1 != null) btn1.onClick.AddListener(() => AddCharacter("1"));
        if (btn2 != null) btn2.onClick.AddListener(() => AddCharacter("2"));
        if (btn3 != null) btn3.onClick.AddListener(() => AddCharacter("3"));
        if (btn4 != null) btn4.onClick.AddListener(() => AddCharacter("4"));
        if (btn5 != null) btn5.onClick.AddListener(() => AddCharacter("5"));
        
        if (btnSlash != null) btnSlash.onClick.AddListener(() => AddCharacter("/"));
        if (btnDot != null) btnDot.onClick.AddListener(() => AddCharacter("."));
        
        if (btnDelete != null) btnDelete.onClick.AddListener(DeleteLastCharacter);
        if (btnClear != null) btnClear.onClick.AddListener(ClearAnswer);
        
        UpdateDisplay();
    }
    
    /// <summary>
    /// Tambahkan karakter ke jawaban
    /// </summary>
    public void AddCharacter(string character)
    {
        // Validasi panjang
        if (currentAnswer.Length >= maxLength)
        {
            Debug.Log($"[ButtonAnswerSystem] Max length reached: {maxLength}");
            return;
        }
        
        // Validasi: tidak boleh operator berturut-turut
        if (currentAnswer.Length > 0)
        {
            char lastChar = currentAnswer[currentAnswer.Length - 1];
            bool lastIsOperator = (lastChar == '/' || lastChar == '.');
            bool newIsOperator = (character == "/" || character == ".");
            
            if (lastIsOperator && newIsOperator)
            {
                Debug.Log($"[ButtonAnswerSystem] Cannot add operator after operator");
                return;
            }
        }
        
        // Tambahkan karakter
        currentAnswer += character;
        UpdateDisplay();
        
        Debug.Log($"[ButtonAnswerSystem] Added '{character}' → Current: '{currentAnswer}'");
    }
    
    /// <summary>
    /// Hapus karakter terakhir (DEL)
    /// </summary>
    public void DeleteLastCharacter()
    {
        if (currentAnswer.Length > 0)
        {
            currentAnswer = currentAnswer.Substring(0, currentAnswer.Length - 1);
            UpdateDisplay();
            Debug.Log($"[ButtonAnswerSystem] Deleted last char → Current: '{currentAnswer}'");
        }
    }
    
    /// <summary>
    /// Hapus semua karakter (CLEAR)
    /// </summary>
    public void ClearAnswer()
    {
        currentAnswer = "";
        UpdateDisplay();
        Debug.Log($"[ButtonAnswerSystem] Cleared answer");
    }
    
    /// <summary>
    /// Update display text dan hidden input field
    /// </summary>
    private void UpdateDisplay()
    {
        // Update display text
        if (answerDisplayText != null)
        {
            answerDisplayText.text = string.IsNullOrEmpty(currentAnswer) ? "___" : currentAnswer;
        }
        
        // Sync ke hidden input field untuk compatibility dengan existing code
        if (hiddenInputField != null)
        {
            hiddenInputField.text = currentAnswer;
        }
    }
    
    /// <summary>
    /// Reset untuk soal baru
    /// </summary>
    public void ResetForNewQuestion()
    {
        currentAnswer = "";
        UpdateDisplay();
    }
    
    /// <summary>
    /// Get current answer (untuk validation)
    /// </summary>
    public string GetCurrentAnswer()
    {
        return currentAnswer;
    }
}
