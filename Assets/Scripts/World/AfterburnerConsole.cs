using UnityEngine;

public class AfterburnerConsole : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private bool _isPlayerInRange;
    private bool _used;

    private void Update()
    {
        if (_used || !_isPlayerInRange)
            return;

        if (Input.GetKeyDown(interactKey))
        {
            _used = true;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.AfterburnerActivated();
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
            indicator.Hide();
        }
    }
}
