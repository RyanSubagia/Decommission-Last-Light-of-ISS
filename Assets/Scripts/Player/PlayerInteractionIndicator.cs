using UnityEngine;

public class PlayerInteractionIndicator : MonoBehaviour
{
    [Header("Indicator Sprite (Animated)")]
    [SerializeField] private GameObject indicatorObject;

    private void Awake()
    {
        if (indicatorObject != null)
        {
            indicatorObject.SetActive(false);
        }
    }

    public void Show()
    {
        if (indicatorObject != null)
        {
            indicatorObject.SetActive(true);
        }
    }

    public void Hide()
    {
        if (indicatorObject != null)
        {
            indicatorObject.SetActive(false);
        }
    }
}
