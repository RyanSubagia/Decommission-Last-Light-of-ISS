using UnityEngine;

public class GoalProgression : MonoBehaviour
{
    public static GoalProgression Instance { get; private set; }

    [Header("Goal Text")]
    [TextArea]
    [SerializeField] private string goalStart = "Turn on the decommission protocol";

    [TextArea]
    [SerializeField] private string goalFindLab = "Find the lab research result";

    [TextArea]
    [SerializeField] private string goalFindKey = "Find the missing key";

    [TextArea]
    [SerializeField] private string goalEscape = "Escape the ISS";

    [TextArea]
    [SerializeField] private string goalSeekTruth = "Seek the truth";

    [TextArea]
    [SerializeField] private string goalPreventImpact = "Prevent ISS from entering Earth";

    private bool _decommissionStarted;
    private bool _lockedDoorFound;
    private bool _hasKey;
    private bool _labResultFound;
    private bool _secretDoorOpened;
    private bool _secretNoteRead;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        RefreshGoal();
    }

    public void OnDecommissionStarted()
    {
        _decommissionStarted = true;
        RefreshGoal();
    }

    public void OnLockedDoorFound()
    {
        _lockedDoorFound = true;
        RefreshGoal();
    }

    public void OnKeyFound()
    {
        _hasKey = true;
        RefreshGoal();
    }

    public void OnLabResultFound()
    {
        _labResultFound = true;
        RefreshGoal();
    }

    public void OnSecretDoorOpened()
    {
        _secretDoorOpened = true;
        RefreshGoal();
    }

    public void OnSecretNoteRead()
    {
        _secretNoteRead = true;
        RefreshGoal();
    }

    private void RefreshGoal()
    {
        if (_secretNoteRead)
        {
            SetGoal(goalPreventImpact);
            return;
        }

        if (_secretDoorOpened)
        {
            SetGoal(goalSeekTruth);
            return;
        }

        if (!_decommissionStarted)
        {
            SetGoal(goalStart);
            return;
        }

        if (_labResultFound)
        {
            SetGoal(goalEscape);
            return;
        }

        if (_lockedDoorFound && !_hasKey)
        {
            SetGoal(goalFindKey);
            return;
        }

        SetGoal(goalFindLab);
    }

    private void SetGoal(string goal)
    {
        if (GoalHelperUI.Instance != null)
        {
            GoalHelperUI.Instance.SetGoal(goal);
        }
    }
}
