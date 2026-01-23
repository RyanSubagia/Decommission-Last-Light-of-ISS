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

    private bool _isPlayerInRange;
    private bool _isOpen;
    
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

            StartCoroutine(OpenDoorRoutine());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = false;
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
}
