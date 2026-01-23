using UnityEngine;

public class FinalNoteInteractable : NoteInteractable
{
    [Header("True Ending Button")]
    [SerializeField] private GameObject trueEndingButton;

    private bool _buttonUnlocked;

    public override void HidePanel()
    {
        if (!_buttonUnlocked && trueEndingButton != null)
        {
            trueEndingButton.SetActive(true);
            _buttonUnlocked = true;
        }

        base.HidePanel();
    }
}
