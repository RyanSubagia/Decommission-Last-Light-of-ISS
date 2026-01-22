using UnityEngine;


public class NoteInteractable : Interactable
{
    [Header("Note Content")]
    [TextArea] public string noteTitle;   
    [TextArea] public string noteBody;    

    [Header("Note Panel Controller")]
    [SerializeField] private NotePanelController notePanelController;

    public override void ShowPanel()
    {
        if (notePanelController != null)
        {
            notePanelController.ShowNote(noteTitle, noteBody);
        }

        base.ShowPanel();
    }

    public override void HidePanel()
    {
        if (notePanelController != null)
        {
            notePanelController.Clear();
        }

        base.HidePanel();
    }
}
