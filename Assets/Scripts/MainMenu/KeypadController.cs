using UnityEngine;
using TMPro; 

public class KeypadController : MonoBehaviour
{
    [Header("Display")]
    public TMP_Text displayField;

    [SerializeField] private TMP_Text digitCountField;

    [SerializeField] private string placeholderText = "00000";

    [Header("Code Settings")]
    [Tooltip("Correct 6 digit code to unlock.")]
    [SerializeField] private string correctCode = "836294";

    [Header("Panels")] 
    [Tooltip("Root object (panel or canvas) for the keypad UI.")]
    [SerializeField] private GameObject keypadPanel;

    [Header("Door Unlock")]
    [SerializeField] private DoorInteraction targetDoor;

    [Header("Feedback")]
    [SerializeField] private string incorrectMessage = "Password incorrect.";

    private const int MaxLength = 6;

    private void OnEnable()
    {
        SetPlaceholder();
        UpdateDigitCount();
    }

    public void AddCharacter(string character)
    {
        if (displayField == null)
            return;

        if (IsPlaceholderVisible())
        {
            displayField.text = character;
            UpdateDigitCount();
            return;
        }

        if (displayField.text.Length < MaxLength)
        {
            displayField.text = displayField.text + character;
            UpdateDigitCount();
        }
    }

    public void ClearInput()
    {
        if (displayField == null)
            return;

        SetPlaceholder();
        UpdateDigitCount();
    }

    public void DeleteChar()
    {
        if (displayField == null)
            return;

        if (IsPlaceholderVisible())
            return;

        if (displayField.text.Length > 0)
        {
            displayField.text = displayField.text.Substring(0, displayField.text.Length - 1);
            if (displayField.text.Length == 0)
            {
                SetPlaceholder();
            }

            UpdateDigitCount();
        }
    }

    public void ClosePanel()
    {
        if (keypadPanel != null)
        {
            keypadPanel.SetActive(false);
        }

        ClearInput();
    }

    public void SubmitCode()
    {
        if (displayField == null)
            return;

        if (displayField.text.Length != MaxLength)
            return;

        bool isCorrect = displayField.text == correctCode;

        if (isCorrect)
        {
            ClosePanel();

            if (targetDoor != null)
            {
                targetDoor.OpenFromConsole();
            }
        }
        else
        {
            ClosePanel();

            if (HintMessageUI.Instance != null && !string.IsNullOrWhiteSpace(incorrectMessage))
            {
                HintMessageUI.Instance.ShowMessage(incorrectMessage);
            }
        }
    }

    private void UpdateDigitCount()
    {
        if (digitCountField == null)
            return;

        int currentLength = IsPlaceholderVisible() || displayField == null ? 0 : displayField.text.Length;
        digitCountField.text = currentLength + "/" + MaxLength;
    }

    private void SetPlaceholder()
    {
        if (displayField == null)
            return;

        displayField.text = placeholderText;
    }

    private bool IsPlaceholderVisible()
    {
        return displayField != null && displayField.text == placeholderText;
    }
}