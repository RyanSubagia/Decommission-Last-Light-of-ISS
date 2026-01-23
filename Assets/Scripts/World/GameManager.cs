using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Countdown Settings")]
    [SerializeField] private float countdownDurationSeconds = 300f; // 5 minutes
    [SerializeField] private TMP_Text txtxCountdown;

    [Header("Fade Settings")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 2f;

    [Header("Scene Names")] 
    [SerializeField] private string endingSceneName = "Ending";

    private float _remainingTime;
    private bool _countdownRunning;
    private bool _hasEnded;

    public GameEndType LastEndType { get; private set; } = GameEndType.None;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
        }
    }

    private void Update()
    {
        if (!_countdownRunning || _hasEnded)
            return;

        _remainingTime -= Time.deltaTime;

        if (_remainingTime <= 0f)
        {
            _remainingTime = 0f;
            _countdownRunning = false;

            if (!_hasEnded)
            {
                StartCoroutine(HandleGameEnd(GameEndType.LoseTimeUp));
            }
        }

        UpdateCountdownText();
    }

    public void StartCountdown()
    {
        if (_countdownRunning || _hasEnded)
            return;

        _remainingTime = countdownDurationSeconds;
        _countdownRunning = true;
        UpdateCountdownText();
    }

    public void EscapePodReached()
    {
        if (_hasEnded)
            return;

        StartCoroutine(HandleGameEnd(GameEndType.WinEscapePod));
    }

    public void AfterburnerActivated()
    {
        if (_hasEnded)
            return;

        StartCoroutine(HandleGameEnd(GameEndType.WinAfterburner));
    }

    public void TriggerDefaultEnding()
    {
        if (_hasEnded)
            return;

        StartCoroutine(HandleGameEnd(GameEndType.LoseTimeUp));
    }

    private void UpdateCountdownText()
    {
        if (txtxCountdown == null)
            return;

        int totalSeconds = Mathf.CeilToInt(_remainingTime);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        txtxCountdown.text = $"{minutes:00}:{seconds:00}";
    }

    private IEnumerator HandleGameEnd(GameEndType endType)
    {
        _hasEnded = true;
        _countdownRunning = false;

        LastEndType = endType;

        // Fade to black
        if (fadeCanvasGroup != null)
        {
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / fadeDuration);
                fadeCanvasGroup.alpha = t;
                yield return null;
            }
        }

        if (!string.IsNullOrEmpty(endingSceneName))
        {
            SceneManager.LoadScene(endingSceneName);
        }
    }
}

public enum GameEndType
{
    None,
    LoseTimeUp,
    WinEscapePod,
    WinAfterburner
}
