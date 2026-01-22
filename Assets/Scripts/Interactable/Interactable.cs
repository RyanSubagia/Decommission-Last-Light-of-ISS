using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("UI Panel to Toggle")]
    [SerializeField] private GameObject interactionPanel;

    public bool IsPanelOpen { get; private set; }

    private void Awake()
    {
        if (interactionPanel != null)
        {
            interactionPanel.SetActive(false);
            IsPanelOpen = false;
        }
    }

    public void ShowPanel()
    {
        if (interactionPanel == null)
            return;

        interactionPanel.SetActive(true);
        IsPanelOpen = true;
    }

    public void HidePanel()
    {
        if (interactionPanel == null)
            return;

        interactionPanel.SetActive(false);
        IsPanelOpen = false;
    }
}
