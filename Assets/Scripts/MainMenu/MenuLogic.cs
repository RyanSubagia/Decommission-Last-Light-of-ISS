using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MenuLogic : MonoBehaviour
{
    [SerializeField] private string introSceneName = "Intro";
    [SerializeField] private GameObject howToPanel;
    [SerializeField] private GameObject noteDetailPanel;
    [SerializeField] private GameObject logsPanel;
    [SerializeField] private RectTransform logsListContent;
    [SerializeField] private TextMeshProUGUI detailTitleText;
    [SerializeField] private TextMeshProUGUI detailBodyText;

    private const string LockedListLabel = "????";
    private const string LockedLogTitle = "????";
    private const string LockedLogBody = "This log is locked for the current session.\n\nFind the note in-game to unlock and read it from the main menu.";

    private readonly List<Button> _logButtons = new List<Button>();
    private TMP_FontAsset _listFontAsset;
    private bool _detailOpenedFromLogList;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if ((howToPanel != null && howToPanel.activeSelf)
                || (noteDetailPanel != null && noteDetailPanel.activeSelf)
                || (logsPanel != null && logsPanel.activeSelf))
            {
                CloseHowTo();
            }
        }
    }

    private void Start()
    {
        ResolveUiReferences();
        EnsureListLayoutSupport();
        RefreshLogList();

        if (howToPanel != null)
        {
            howToPanel.SetActive(false);
        }

        if (logsPanel != null)
        {
            logsPanel.SetActive(false);
        }

        if (noteDetailPanel != null)
        {
            noteDetailPanel.SetActive(false);
        }
    }

    public void PlayGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetForNewRun();
        }
        SceneManager.LoadScene(introSceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void PlayClickSfx()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayClickSFX();
        }
    }

    public void OpenHowTo()
    {
        ResolveUiReferences();
        _detailOpenedFromLogList = false;

        if (logsPanel != null)
        {
            logsPanel.SetActive(false);
        }

        if (noteDetailPanel != null)
        {
            noteDetailPanel.SetActive(false);
        }

        if (howToPanel != null)
        {
            howToPanel.SetActive(true);
        }
    }

    public void CloseHowTo()
    {
        if (_detailOpenedFromLogList && noteDetailPanel != null && noteDetailPanel.activeSelf)
        {
            noteDetailPanel.SetActive(false);
            _detailOpenedFromLogList = false;

            OpenFoundLogs();
            return;
        }

        if (howToPanel != null)
        {
            howToPanel.SetActive(false);
        }

        if (noteDetailPanel != null)
        {
            noteDetailPanel.SetActive(false);
        }

        if (logsPanel != null)
        {
            logsPanel.SetActive(false);
        }

        _detailOpenedFromLogList = false;
    }

    public void OpenFoundLogs()
    {
        ResolveUiReferences();
        RefreshLogList();

        if (howToPanel != null)
        {
            howToPanel.SetActive(false);
        }

        if (noteDetailPanel != null)
        {
            noteDetailPanel.SetActive(false);
        }

        _detailOpenedFromLogList = false;
        if (logsPanel != null)
        {
            logsPanel.SetActive(true);
        }
    }

    public void OpenLogs()
    {
        OpenFoundLogs();
    }

    private void OpenLogDetails(int index)
    {
        if (index < 0 || index >= SessionNoteLog.Count || noteDetailPanel == null)
        {
            return;
        }

        if (logsPanel != null)
            logsPanel.SetActive(false);

        if (howToPanel != null)
        {
            howToPanel.SetActive(false);
        }

        noteDetailPanel.SetActive(true);
        _detailOpenedFromLogList = true;
        ApplySelectedLog(SessionNoteLog.Entries[index]);
    }

    private void ApplySelectedLog(SessionNoteLog.Entry entry)
    {
        var title = entry.IsUnlocked ? entry.Title : LockedLogTitle;
        var body = entry.IsUnlocked ? entry.Body : LockedLogBody;
        ApplyPanelText(title, body);
    }

    private void ResolveUiReferences()
    {
        if (howToPanel == null)
        {
            var howToTransform = FindSceneTransformByName("Panel_HowTo");
            if (howToTransform != null)
            {
                howToPanel = howToTransform.gameObject;
            }
        }

        if (noteDetailPanel == null)
        {
            var detailTransform = FindSceneTransformByName("CollectibleNote");
            if (detailTransform != null)
            {
                noteDetailPanel = detailTransform.gameObject;
            }
        }

        if (logsPanel == null)
        {
            var logsTransform = FindSceneTransformByName("Panel_Logs");
            if (logsTransform != null)
            {
                logsPanel = logsTransform.gameObject;
            }
        }

        if (logsListContent == null && logsPanel != null)
        {
            var contentTransform = FindDeepChild(logsPanel.transform, "Content");
            if (contentTransform != null)
            {
                logsListContent = contentTransform as RectTransform;
            }
        }

        if (noteDetailPanel != null && (detailTitleText == null || detailBodyText == null))
        {
            var texts = noteDetailPanel.GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach (var text in texts)
            {
                if (detailTitleText == null && text.name.ToLower().Contains("logid"))
                {
                    detailTitleText = text;
                    continue;
                }

                if (detailBodyText == null && text.name.Contains("Text"))
                {
                    detailBodyText = text;
                }
            }
        }

        if (_listFontAsset == null)
        {
            if (detailBodyText != null)
            {
                _listFontAsset = detailBodyText.font;
            }
            else if (detailTitleText != null)
            {
                _listFontAsset = detailTitleText.font;
            }
        }
    }

    private void ApplyPanelText(string title, string body)
    {
        if (detailTitleText != null)
        {
            detailTitleText.text = title;
        }

        if (detailBodyText != null)
        {
            detailBodyText.text = body;
        }
    }

    private void EnsureListLayoutSupport()
    {
        if (logsListContent == null)
            return;

        var layout = logsListContent.GetComponent<VerticalLayoutGroup>();
        if (layout == null)
        {
            layout = logsListContent.gameObject.AddComponent<VerticalLayoutGroup>();
        }

        layout.spacing = 4f;
        layout.padding = new RectOffset(4, 4, 4, 4);
        layout.childControlHeight = false;
        layout.childControlWidth = true;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = true;

        var fitter = logsListContent.GetComponent<ContentSizeFitter>();
        if (fitter == null)
        {
            fitter = logsListContent.gameObject.AddComponent<ContentSizeFitter>();
        }

        fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }

    private void RefreshLogList()
    {
        if (logsListContent == null)
            return;

        for (var i = 0; i < _logButtons.Count; i++)
        {
            if (_logButtons[i] != null)
            {
                Destroy(_logButtons[i].gameObject);
            }
        }
        _logButtons.Clear();

        EnsureListLayoutSupport();

        for (var index = 0; index < SessionNoteLog.Count; index++)
        {
            var entry = SessionNoteLog.Entries[index];
            var label = entry.IsUnlocked ? entry.Title : LockedListLabel;
            var button = CreateListRowButton(index, label);
            _logButtons.Add(button);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(logsListContent);
    }

    private Button CreateListRowButton(int index, string label)
    {
        var buttonObject = new GameObject($"LogEntry_{index}", typeof(RectTransform), typeof(Image), typeof(Button), typeof(LayoutElement));
        buttonObject.transform.SetParent(logsListContent, false);

        var rect = buttonObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0f, 1f);
        rect.anchorMax = new Vector2(1f, 1f);
        rect.pivot = new Vector2(0.5f, 1f);
        rect.sizeDelta = new Vector2(0f, 28f);

        var layout = buttonObject.GetComponent<LayoutElement>();
        layout.preferredHeight = 28f;
        layout.minHeight = 28f;

        var image = buttonObject.GetComponent<Image>();
        image.color = new Color(0.04f, 0.44f, 0.47f, 0.92f);

        var button = buttonObject.GetComponent<Button>();
        var colors = button.colors;
        colors.normalColor = Color.white;
        colors.highlightedColor = new Color(0.9f, 0.9f, 0.9f, 1f);
        colors.pressedColor = new Color(0.75f, 0.75f, 0.75f, 1f);
        button.colors = colors;
        button.onClick.AddListener(PlayClickSfx);
        button.onClick.AddListener(() => OpenLogDetails(index));

        CreateTextElement("Label", rect, label, 14f, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, TextAlignmentOptions.Left);

        return button;
    }

    private TextMeshProUGUI CreateTextElement(string objectName, Transform parent, string value, float fontSize, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 anchoredPosition, Vector2 sizeDelta, TextAlignmentOptions alignment)
    {
        var textObject = new GameObject(objectName, typeof(RectTransform), typeof(TextMeshProUGUI));
        textObject.transform.SetParent(parent, false);

        var rect = textObject.GetComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.pivot = pivot;
        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = sizeDelta;

        var text = textObject.GetComponent<TextMeshProUGUI>();
        text.text = value;
        text.fontSize = fontSize;
        text.alignment = alignment;
        text.color = Color.white;
        text.enableWordWrapping = false;
        text.margin = new Vector4(10f, 0f, 0f, 0f);

        if (_listFontAsset != null)
        {
            text.font = _listFontAsset;
        }

        return text;
    }

    private static Transform FindDeepChild(Transform parent, string childName)
    {
        if (parent == null)
            return null;

        if (parent.name == childName)
            return parent;

        for (var i = 0; i < parent.childCount; i++)
        {
            var result = FindDeepChild(parent.GetChild(i), childName);
            if (result != null)
                return result;
        }

        return null;
    }

    private static Transform FindSceneTransformByName(string objectName)
    {
        var allTransforms = Resources.FindObjectsOfTypeAll<Transform>();
        foreach (var t in allTransforms)
        {
            if (t == null)
                continue;

            if (!t.gameObject.scene.IsValid())
                continue;

            if (t.name == objectName)
                return t;
        }

        return null;
    }
}
