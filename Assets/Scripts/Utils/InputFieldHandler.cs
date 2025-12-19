using UnityEngine;
using TMPro;

/// <summary>
/// Script untuk handling input field agar bisa submit dengan Enter key
/// Attach script ini ke GameObject yang memiliki CalculationManager
/// </summary>
public class InputFieldHandler : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private CalculationManager calculationManager;

    void Start()
    {
        if (inputField != null)
        {
            // Tambahkan listener untuk onEndEdit (dipanggil saat user tekan Enter)
            inputField.onEndEdit.AddListener(OnInputFieldEndEdit);

            // Fokuskan input field di awal
            inputField.Select();
            inputField.ActivateInputField();
        }
    }

    void OnInputFieldEndEdit(string value)
    {
        // Cek jika user menekan Enter
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            // Panggil fungsi verifikasi jawaban
            if (calculationManager != null)
            {
                calculationManager.VerifyAnswer();
            }
        }
    }

    void Update()
    {
        // Auto-focus input field jika tidak ada yang fokus
        if (inputField != null && !inputField.isFocused)
        {
            // Check if EventSystem exists
            if (UnityEngine.EventSystems.EventSystem.current != null &&
                !UnityEngine.EventSystems.EventSystem.current.alreadySelecting)
            {
                inputField.Select();
            }
        }
    }
}
