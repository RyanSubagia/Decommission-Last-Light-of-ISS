using System.Collections;
using UnityEngine;
using TMPro;

public class HintMessageUI : MonoBehaviour
{
    public static HintMessageUI Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Settings")]
    [SerializeField] private float defaultVisibleDuration = 2f;

    private Coroutine _currentRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }
    }

    public void ShowMessage(string message)
    {
        ShowMessage(message, defaultVisibleDuration);
    }

    public void ShowMessage(string message, float duration)
    {
        if (messageText == null || canvasGroup == null)
            return;

        messageText.text = message;
        canvasGroup.alpha = 1f;

        if (_currentRoutine != null)
        {
            StopCoroutine(_currentRoutine);
        }

        _currentRoutine = StartCoroutine(HideAfterDelay(duration));
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }

        _currentRoutine = null;
    }
}
