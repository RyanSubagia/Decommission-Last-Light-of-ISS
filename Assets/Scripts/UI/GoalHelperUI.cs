using UnityEngine;
using TMPro;

public class GoalHelperUI : MonoBehaviour
{
    public static GoalHelperUI Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private TMP_Text goalText;

    [Header("Settings")]
    [SerializeField] private string defaultGoal = "";
    [SerializeField] private bool hideWhenEmpty = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        ApplyGoal(defaultGoal);
    }

    public void SetGoal(string goal)
    {
        ApplyGoal(goal);
    }

    public void ClearGoal()
    {
        ApplyGoal(string.Empty);
    }

    private void ApplyGoal(string goal)
    {
        if (goalText == null)
            return;

        goalText.text = goal ?? string.Empty;

        if (hideWhenEmpty)
        {
            goalText.enabled = !string.IsNullOrWhiteSpace(goalText.text);
        }
    }
}
