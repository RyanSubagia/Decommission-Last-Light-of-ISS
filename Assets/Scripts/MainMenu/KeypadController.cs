using UnityEngine;
using TMPro; 

public class KeypadController : MonoBehaviour
{
    [Header("Display")]
    public TMP_Text displayField;

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
        ClearInput();
    }

    public void AddCharacter(string character)
    {
        if (displayField == null)
            return;

        if (displayField.text.Length < MaxLength)
        {
            displayField.text = displayField.text + character;
        }
    }

    public void ClearInput()
    {
        if (displayField == null)
            return;

        displayField.text = string.Empty;
    }

    public void DeleteChar()
    {
        if (displayField == null)
            return;

        if (displayField.text.Length > 0)
        {
            displayField.text = displayField.text.Substring(0, displayField.text.Length - 1);
        }
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
            if (keypadPanel != null)
            {
                keypadPanel.SetActive(false);
            }

            if (targetDoor != null)
            {
                targetDoor.OpenFromConsole();
            }
        }
        else
        {
            if (keypadPanel != null)
            {
                keypadPanel.SetActive(false);
            }

            if (HintMessageUI.Instance != null && !string.IsNullOrWhiteSpace(incorrectMessage))
            {
                HintMessageUI.Instance.ShowMessage(incorrectMessage);
            }

            ClearInput();
        }
    }
}