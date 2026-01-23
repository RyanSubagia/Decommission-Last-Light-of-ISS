using UnityEngine;

public class KeycardPickup : MonoBehaviour
{
    [Header("Interaction")] 
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("References")] 
    [SerializeField] private PlayerKeyInventory playerInventory;

    private bool _isPlayerInRange;
    private bool _collected;

    private void Update()
    {
        if (_collected)
            return;

        if (!_isPlayerInRange)
            return;

        if (Input.GetKeyDown(interactKey))
        {
            CollectKeycard();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = true;

            var indicator = other.GetComponentInParent<PlayerInteractionIndicator>();
            if (indicator != null)
            {
                indicator.Show();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = false;

            var indicator = other.GetComponentInParent<PlayerInteractionIndicator>();
            if (indicator != null)
            {
                indicator.Hide();
            }
        }
    }

    private void CollectKeycard()
    {
        _collected = true;

        if (playerInventory != null)
        {
            playerInventory.GiveKey();
        }

        Destroy(gameObject);
    }
}
