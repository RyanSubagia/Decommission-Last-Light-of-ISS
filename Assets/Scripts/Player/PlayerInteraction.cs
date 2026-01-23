using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Interaction Indicator")]
    [SerializeField] private PlayerInteractionIndicator interactionIndicator;

    private Interactable currentInteractable;

    private void Awake()
    {
        if (interactionIndicator == null)
        {
            interactionIndicator = GetComponent<PlayerInteractionIndicator>();
        }
    }

    private void Update()
    {
        if (currentInteractable == null)
            return;

        if (Input.GetKeyDown(interactKey))
        {
            if (currentInteractable.IsPanelOpen)
            {
                currentInteractable.HidePanel();
            }
            else
            {
                currentInteractable.ShowPanel();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var interactable = other.GetComponent<Interactable>();
        if (interactable != null)
        {
            currentInteractable = interactable;

            if (interactionIndicator != null)
            {
                interactionIndicator.Show();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var interactable = other.GetComponent<Interactable>();
        if (interactable != null && interactable == currentInteractable)
        {
             if (currentInteractable.IsPanelOpen)
            {
                currentInteractable.HidePanel();
            }

            currentInteractable = null;

            if (interactionIndicator != null)
            {
                interactionIndicator.Hide();
            }
        }
    }
}

