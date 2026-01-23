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
    }

    private void UpdateIndicator()
    {
        if (keyIndicator != null)
        {
            keyIndicator.SetActive(HasKey);
        }
    }
}
