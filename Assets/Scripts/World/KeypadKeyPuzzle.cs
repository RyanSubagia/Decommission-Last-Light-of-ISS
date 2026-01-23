using UnityEngine;
using TMPro;

public class KeypadKeyPuzzle : MonoBehaviour
{
    [Header("Display")]
    [SerializeField] private TMP_Text displayField;

    [Header("Code Settings")]
    [Tooltip("Correct 4 digit code to unlock the key.")]
    [SerializeField] private string correctCode = "1234";

    [Header("Panels")]
    [Tooltip("Root object (panel or canvas) for the keypad UI.")]
    [SerializeField] private GameObject keypadPanel;

    [Header("Player Inventory")]
    [SerializeField] private PlayerKeyInventory playerInventory;

    private const int MaxLength = 4;
    private bool _solved;

    private void OnEnable()
    {
        ClearInput();
    }

    public void AddCharacter(string character)
    {
        if (_solved)
            return;

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
        if (_solved)
            return;

        if (displayField == null)
            return;

        if (displayField.text.Length > 0)
        {
            displayField.text = displayField.text.Substring(0, displayField.text.Length - 1);
        }
    }

    public void SubmitCode()
    {
        if (_solved)
            return;

        if (displayField == null)
            return;

        if (displayField.text.Length != MaxLength)
            return;

        bool isCorrect = displayField.text == correctCode;

        if (isCorrect)
        {
            _solved = true;

            if (playerInventory != null)
            {
                playerInventory.GiveKey();
            }

            if (keypadPanel != null)
            {
                keypadPanel.SetActive(false);
            }
        }
        else
        {
            ClearInput();
        }
    }
}
