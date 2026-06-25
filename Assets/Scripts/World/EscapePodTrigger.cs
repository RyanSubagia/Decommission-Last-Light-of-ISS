using UnityEngine;
using TMPro;

public class EscapePodTrigger : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Confirmation UI")]
    [SerializeField] private GameObject confirmationPanel;
    [SerializeField] private TMP_Text confirmationTitleText;
    [SerializeField] private TMP_Text confirmationBodyText;

    [TextArea]
    [SerializeField] private string confirmationTitle = "Leave the station now?";

    [TextArea]
    [SerializeField] private string confirmationBody = "You can still go back and explore before ending.";

    private bool _isPlayerInRange;
    private bool _isConfirmationOpen;
    private PlayerInteractionIndicator _playerIndicator;

    private void Awake()
    {
        HideConfirmation();
    }

    private void Update()
    {
        if (!_isPlayerInRange)
            return;

        if (!_isConfirmationOpen && Input.GetKeyDown(interactKey))
        {
            ShowConfirmation();
            return;
        }

        if (_isConfirmationOpen)
        {
            if (Input.GetKeyDown(KeyCode.Y) || Input.GetKeyDown(KeyCode.Return))
            {
                ConfirmEscape();
            }
            else if (Input.GetKeyDown(KeyCode.N) || Input.GetKeyDown(KeyCode.Escape))
            {
                CancelEscape();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var indicator = other.GetComponentInParent<PlayerInteractionIndicator>();
        if (indicator == null)
            return;

        _isPlayerInRange = true;
        _playerIndicator = indicator;
        _playerIndicator.Show();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var indicator = other.GetComponentInParent<PlayerInteractionIndicator>();
        if (indicator == null)
            return;

        _isPlayerInRange = false;
        _playerIndicator?.Hide();
        _playerIndicator = null;
        HideConfirmation();
    }

    public void ConfirmEscape()
    {
        if (!_isPlayerInRange)
            return;

        HideConfirmation();
        _playerIndicator?.Hide();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.EscapePodReached();
        }
    }

    public void CancelEscape()
    {
        HideConfirmation();
    }

    private void ShowConfirmation()
    {
        if (confirmationTitleText != null)
        {
            confirmationTitleText.text = confirmationTitle;
        }

        if (confirmationBodyText != null)
        {
            confirmationBodyText.text = confirmationBody;
        }

        if (confirmationPanel != null)
        {
            confirmationPanel.SetActive(true);
        }

        _isConfirmationOpen = true;
    }

    private void HideConfirmation()
    {
        if (confirmationPanel != null)
        {
            confirmationPanel.SetActive(false);
        }

        _isConfirmationOpen = false;
    }
}
