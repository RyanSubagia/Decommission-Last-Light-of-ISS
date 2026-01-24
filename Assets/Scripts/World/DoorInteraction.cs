using System.Collections;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D blockingCollider;

    [Header("Interaction")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private string openTriggerName = "Open"; 
    [SerializeField] private float openDelay = 1f; 

    [Header("Key Requirement")]
    [SerializeField] private bool requireKey;
    [SerializeField] private PlayerKeyInventory playerInventory;

    [Header("Countdown Requirement")]
    [SerializeField] private bool requireCountdownStarted;

    private bool _isPlayerInRange;
    private bool _isOpen;
    private bool _hasShownIndicator;
    
    private void Reset()
    {
        animator = GetComponent<Animator>();
        blockingCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (_isOpen)
            return;

        if (_isPlayerInRange && Input.GetKeyDown(interactKey))
        {
            if (requireKey)
            {
                if (playerInventory == null || !playerInventory.HasKey)
                {
                    return;
                }
            }

            if (requireCountdownStarted)
            {
                if (GameManager.Instance == null || !GameManager.Instance.IsCountdownRunning)
                {
                    if (HintMessageUI.Instance != null)
                    {
                        HintMessageUI.Instance.ShowMessage("System locked. \nPress decommission protocol button first.");
                    }

                    return;
                }
            }

            StartCoroutine(OpenDoorRoutine());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var indicator = other.GetComponentInParent<PlayerInteractionIndicator>();
        if (indicator != null)
        {
            _isPlayerInRange = true;

            if (!requireKey && !_hasShownIndicator)
            {
                indicator.Show();
                _hasShownIndicator = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var indicator = other.GetComponentInParent<PlayerInteractionIndicator>();
        if (indicator != null)
        {
            _isPlayerInRange = false;
            indicator.Hide();
        }
    }

    private IEnumerator OpenDoorRoutine()
    {
        _isOpen = true;

        if (animator != null && !string.IsNullOrEmpty(openTriggerName))
        {
            animator.SetTrigger(openTriggerName);
        }

        if (openDelay > 0f)
            yield return new WaitForSeconds(openDelay);

        if (blockingCollider != null)
        {
            blockingCollider.enabled = false;
        }
    }

    public void OpenFromConsole()
    {
        if (_isOpen)
            return;

        if (requireCountdownStarted)
        {
            if (GameManager.Instance == null || !GameManager.Instance.IsCountdownRunning)
            {
                if (HintMessageUI.Instance != null)
                {
                    HintMessageUI.Instance.ShowMessage("System locked. Press the red button first.");
                }

                return;
            }
        }

        StartCoroutine(OpenDoorRoutine());
    }
}
