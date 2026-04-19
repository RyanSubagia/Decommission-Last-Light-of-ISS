using System.Collections;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D blockingCollider;
    [SerializeField] private AudioSource sfxSource;

    [Header("Interaction")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private string openTriggerName = "Open"; 
    [SerializeField] private float openDelay = 1f; 

    [Header("Door Audio")]
    [SerializeField] private AudioClip openDoorClip;
    [SerializeField, Min(0f)] private float sfxDuration = 1f;

    [Header("Password Requirement")]
    [SerializeField] private bool requirePassword;
    [SerializeField] private GameObject keypadPanel;

    [Header("Key Requirement")]
    [SerializeField] private bool requireKey;
    [SerializeField] private PlayerKeyInventory playerInventory;

    [Header("Countdown Requirement")]
    [SerializeField] private bool requireCountdownStarted;

    [Header("Goal Helper")]
    [SerializeField] private bool isSecretDoor;

    private bool _isPlayerInRange;
    private bool _isOpen;
    private bool _hasShownIndicator;
    
    private void Reset()
    {
        animator = GetComponent<Animator>();
        blockingCollider = GetComponent<Collider2D>();
        sfxSource = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        if (sfxSource == null)
        {
            sfxSource = GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (_isOpen)
            return;

        if (_isPlayerInRange && Input.GetKeyDown(interactKey))
        {
            if (requirePassword)
            {
                if (keypadPanel != null)
                {
                    keypadPanel.SetActive(true);
                }

                return;
            }

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
            _hasShownIndicator = false;
        }
    }

    private IEnumerator OpenDoorRoutine()
    {
        _isOpen = true;

        PlayDoorSfx();

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

        if (isSecretDoor && GoalProgression.Instance != null)
        {
            GoalProgression.Instance.OnSecretDoorOpened();
        }
    }

    private void PlayDoorSfx()
    {
        if (sfxSource == null || openDoorClip == null)
            return;

        sfxSource.PlayOneShot(openDoorClip);
        if (sfxDuration > 0f)
        {
            StartCoroutine(StopSfxAfterDelay(sfxDuration));
        }
    }

    private IEnumerator StopSfxAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        sfxSource.Stop();
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
