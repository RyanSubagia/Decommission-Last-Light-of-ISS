using UnityEngine;

public class EscapePodTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var indicator = other.GetComponentInParent<PlayerInteractionIndicator>();
        if (indicator == null)
            return;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.EscapePodReached();
        }
    }
}
