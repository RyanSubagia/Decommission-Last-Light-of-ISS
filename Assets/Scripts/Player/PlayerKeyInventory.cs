using UnityEngine;

public class PlayerKeyInventory : MonoBehaviour
{
    [Header("UI Key Indicator")]
    [SerializeField] private GameObject keyIndicator;

    public bool HasKey { get; private set; }

    private void Awake()
    {
        UpdateIndicator();
    }

    public void GiveKey()
    {
        HasKey = true;
        UpdateIndicator();

        if (GoalProgression.Instance != null)
        {
            GoalProgression.Instance.OnKeyFound();
        }
    }

    private void UpdateIndicator()
    {
        if (keyIndicator != null)
        {
            keyIndicator.SetActive(HasKey);
        }
    }
}
