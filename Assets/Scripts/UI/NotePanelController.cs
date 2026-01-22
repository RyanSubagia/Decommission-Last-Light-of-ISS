using UnityEngine;
using TMPro;

public class NotePanelController : MonoBehaviour
{
    [Header("Note Text References")]
    [SerializeField] private TextMeshProUGUI txtIdNote;
    [SerializeField] private TextMeshProUGUI txtNote;

    public void ShowNote(string idText, string noteText)
    {
        if (txtIdNote != null)
        {
            txtIdNote.text = idText;
        }

        if (txtNote != null)
        {
            txtNote.text = noteText;
        }
    }

    public void Clear()
    {
        if (txtIdNote != null)
        {
            txtIdNote.text = string.Empty;
        }

        if (txtNote != null)
        {
            txtNote.text = string.Empty;
        }
    }
}
