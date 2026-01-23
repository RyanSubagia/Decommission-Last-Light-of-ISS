using UnityEngine;
using TMPro;

public class DoorConsoleInteraction : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("References")]
    [SerializeField] private DoorInteraction targetDoor;
    [SerializeField] private PlayerKeyInventory playerInventory;

    [Tooltip("If true, player must have the keycard to use this console.")]
    [SerializeField] private bool requireKeycard = true;

    [Header("Missing Keycard Message")]
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private TMP_Text messageTitleText;
    [SerializeField] private TMP_Text messageBodyText;

    [TextArea]
    [SerializeField] private string missingKeyTitle = "Need administrator keycard";

    [TextArea]
    [SerializeField] private string missingKeyBody = "Check head nurse Charlie for more information";

    private bool _isPlayerInRange;
    private bool _isMessageOpen;

    private void Update()
    {
        if (!_isPlayerInRange)
            return;

        if (Input.GetKeyDown(interactKey))
        {
            // If message is already open, close it on next press.
            if (_isMessageOpen)
            {
                HideMissingKeyMessage();
                return;
            }

            if (requireKeycard)
            {
                if (playerInventory == null || !playerInventory.HasKey)
                {
                    ShowMissingKeyMessage();
                    return;
                }
            }

            if (targetDoor != null)
            {
                HideMissingKeyMessage();
                targetDoor.OpenFromConsole();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var indicator = other.GetComponentInParent<PlayerInteractionIndicator>();
        if (indicator != null)
        {
            _isPlayerInRange = true;
            indicator.Show();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var indicator = other.GetComponentInParent<PlayerInteractionIndicator>();
        if (indicator != null)
        {
            _isPlayerInRange = false;
            HideMissingKeyMessage();
            indicator.Hide();
        }
    }

    private void ShowMissingKeyMessage()
    {
        if (messageTitleText != null)
        {
            messageTitleText.text = missingKeyTitle;
        }

        if (messageBodyText != null)
        {
            messageBodyText.text = missingKeyBody;
        }

        if (messagePanel != null)
        {
            messagePanel.SetActive(true);
        }

        _isMessageOpen = true;
    }

    private void HideMissingKeyMessage()
    {
        if (messagePanel != null)
        {
            messagePanel.SetActive(false);
        }

        _isMessageOpen = false;
    }
}
