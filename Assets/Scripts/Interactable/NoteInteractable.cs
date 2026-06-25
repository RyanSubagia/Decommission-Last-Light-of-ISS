using UnityEngine;


public class NoteInteractable : Interactable
{
    public enum NoteType
    {
        Paper,
        Digital
    }

    public enum GoalNoteType
    {
        None,
        LabResult,
        SecretNote
    }

    [Header("Note Content")]
    [TextArea] public string noteTitle;   
    [TextArea] public string noteBody;    

    [Header("Note Audio")]
    [SerializeField] private NoteType noteType = NoteType.Paper;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip paperNoteClip;
    [SerializeField] private AudioClip digitalNoteClip;
    [SerializeField, Min(0f)] private float sfxDuration = 1f;

    [Header("Note Panel Controller")]
    [SerializeField] private NotePanelController notePanelController;

    [Header("Session Log")]
    [SerializeField] private string sessionLogId;

    [Header("Goal Helper")]
    [SerializeField] private GoalNoteType goalNoteType = GoalNoteType.None;

    private void Awake()
    {
        if (sfxSource == null)
        {
            sfxSource = GetComponent<AudioSource>();
        }
    }

    public override void ShowPanel()
    {
        PlayNoteSfx();

        if (notePanelController != null)
        {
            notePanelController.ShowNote(noteTitle, noteBody);
        }

        SessionNoteLog.Record(GetSessionLogId(), noteTitle, noteBody);

        if (GoalProgression.Instance != null)
        {
            if (goalNoteType == GoalNoteType.LabResult)
            {
                GoalProgression.Instance.OnLabResultFound();
            }
            else if (goalNoteType == GoalNoteType.SecretNote)
            {
                GoalProgression.Instance.OnSecretNoteRead();
            }
        }

        base.ShowPanel();
    }

    public override void HidePanel()
    {
        PlayNoteSfx();

        if (notePanelController != null)
        {
            notePanelController.Clear();
        }

        base.HidePanel();
    }

    private void PlayNoteSfx()
    {
        if (sfxSource == null)
            return;

        var clip = noteType == NoteType.Paper ? paperNoteClip : digitalNoteClip;
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
            if (sfxDuration > 0f)
            {
                StopAllCoroutines();
                StartCoroutine(StopSfxAfterDelay(sfxDuration));
            }
        }
    }

    private System.Collections.IEnumerator StopSfxAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        sfxSource.Stop();
    }

    private string GetSessionLogId()
    {
        if (!string.IsNullOrWhiteSpace(sessionLogId))
            return sessionLogId.Trim();

        if (!string.IsNullOrWhiteSpace(noteTitle))
            return noteTitle.Trim();

        return BuildTransformPath(transform);
    }

    private static string BuildTransformPath(Transform current)
    {
        if (current == null)
            return string.Empty;

        var path = current.name;
        while (current.parent != null)
        {
            current = current.parent;
            path = current.name + "/" + path;
        }

        return path;
    }
}
