using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Countdown Settings")]
    [SerializeField] private float countdownDurationSeconds = 180f; // 3 minutes
    [SerializeField] private TMP_Text txtxCountdown;

    [Header("Fade Settings")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 2f;

    [Header("Scene Names")] 
    [SerializeField] private string gameOverSceneName = "GameOver";
    [SerializeField] private string escapePodCutsceneSceneName = "Cutscene_Default";
    [SerializeField] private string afterburnerCutsceneSceneName = "Cutscene_Afterburner";

    private float _remainingTime;
    private bool _countdownRunning;
    private bool _hasEnded;

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

        string sceneToLoad = null;

        switch (endType)
        {
            case GameEndType.LoseTimeUp:
                sceneToLoad = gameOverSceneName;
                break;
            case GameEndType.WinEscapePod:
                sceneToLoad = escapePodCutsceneSceneName;
                break;
            case GameEndType.WinAfterburner:
                sceneToLoad = afterburnerCutsceneSceneName;
                break;
        }

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
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
