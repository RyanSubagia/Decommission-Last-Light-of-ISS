using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using TMPro;

public class EndingVideoController : MonoBehaviour
{
    [Header("Video Player")]
    [SerializeField] private VideoPlayer videoPlayer;

    [Header("Win Clips")]
    [SerializeField] private VideoClip defaultWinClip;
    [SerializeField] private VideoClip trueEndingClip;

    [Header("Lose UI")] 
    [SerializeField] private GameObject losePanel;
    [SerializeField] private TMP_Text loseText;

    [TextArea]
    [SerializeField] private string loseMessage = "YOU FAILED TO EXTRACT";

    [Header("Next Scene After Ending")]
    [SerializeField] private string nextSceneName = "MainMenu";

    [Header("Input")]
    [SerializeField] private KeyCode skipKey = KeyCode.Escape;

    [Header("Lose Ending Flow")]
    [SerializeField, Min(0f)] private float loseTextHoldDuration = 1f;

    [Header("Scene Fade")]
    [SerializeField] private CanvasGroup returnFadeCanvasGroup;
    [SerializeField, Min(0.01f)] private float returnFadeDuration = 0.9f;

    private Coroutine _loseAutoReturnRoutine;
    private Coroutine _loadNextSceneRoutine;
    private bool _isLoadingNextScene;

    private void Awake()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        TryFindReturnFadeCanvasGroup();
        ResetReturnFade();

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
        }
    }

    private void Start()
    {
        SetupEndingPresentation();
    }

    private void OnDestroy()
    {
        if (_loseAutoReturnRoutine != null)
        {
            StopCoroutine(_loseAutoReturnRoutine);
            _loseAutoReturnRoutine = null;
        }

        if (_loadNextSceneRoutine != null)
        {
            StopCoroutine(_loadNextSceneRoutine);
            _loadNextSceneRoutine = null;
        }

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(skipKey) || Input.GetKeyDown(KeyCode.E))
        {
            LoadNextScene();
        }
    }

    private void SetupEndingPresentation()
    {
        var gm = GameManager.Instance;
        var endType = gm != null ? gm.LastEndType : GameEndType.LoseTimeUp;

        _isLoadingNextScene = false;

        if (_loseAutoReturnRoutine != null)
        {
            StopCoroutine(_loseAutoReturnRoutine);
            _loseAutoReturnRoutine = null;
        }

        ResetReturnFade();

        // Default: hide everything
        if (losePanel != null)
        {
            losePanel.SetActive(false);
        }

        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.enabled = false;
        }

        switch (endType)
        {
            case GameEndType.WinEscapePod:
                if (videoPlayer != null && defaultWinClip != null)
                {
                    videoPlayer.enabled = true;
                    videoPlayer.clip = defaultWinClip;
                    videoPlayer.Play();
                }
                break;

            case GameEndType.WinAfterburner:
                if (videoPlayer != null && trueEndingClip != null)
                {
                    videoPlayer.enabled = true;
                    videoPlayer.clip = trueEndingClip;
                    videoPlayer.Play();
                }
                break;

            case GameEndType.LoseTimeUp:
            default:
                _loseAutoReturnRoutine = StartCoroutine(PlayLoseEndingSequence());
                break;
        }
    }

    private System.Collections.IEnumerator PlayLoseEndingSequence()
    {
        if (losePanel != null)
        {
            losePanel.SetActive(true);
        }

        if (loseText != null)
        {
            loseText.text = loseMessage;
        }

        TryFindReturnFadeCanvasGroup();

        bool useSharedLoseFadePanel = UsesSharedLoseFadePanel();

        if (returnFadeCanvasGroup != null)
        {
            returnFadeCanvasGroup.alpha = 1f;
            returnFadeCanvasGroup.blocksRaycasts = true;
            returnFadeCanvasGroup.interactable = true;
        }

        if (useSharedLoseFadePanel)
        {
            SetLoseTextAlpha(0f);
        }

        if (AudioManager.instance != null)
        {
            AudioManager.instance.TransitionToLoseEndingAudio();
        }

        if (useSharedLoseFadePanel)
        {
            yield return FadeLoseTextAlpha(0f, 1f, returnFadeDuration);
        }
        else
        {
            yield return FadeReturnCanvasGroup(1f, 0f, returnFadeDuration);
        }

        yield return new WaitForSecondsRealtime(loseTextHoldDuration);
        LoadNextScene();
    }

    private void OnVideoFinished(VideoPlayer source)
    {
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        if (_isLoadingNextScene)
            return;

        _isLoadingNextScene = true;

        if (_loseAutoReturnRoutine != null)
        {
            StopCoroutine(_loseAutoReturnRoutine);
            _loseAutoReturnRoutine = null;
        }

        _loadNextSceneRoutine = StartCoroutine(LoadNextSceneWithFade());
    }

    private System.Collections.IEnumerator LoadNextSceneWithFade()
    {
        if (string.IsNullOrEmpty(nextSceneName))
        {
            yield break;
        }

        TryFindReturnFadeCanvasGroup();

        bool useSharedLoseFadePanel = UsesSharedLoseFadePanel();

        if (returnFadeCanvasGroup != null)
        {
            returnFadeCanvasGroup.blocksRaycasts = true;
            returnFadeCanvasGroup.interactable = true;

            if (useSharedLoseFadePanel)
            {
                returnFadeCanvasGroup.alpha = 1f;
                yield return FadeLoseTextAlpha(GetLoseTextAlpha(), 0f, returnFadeDuration);
            }
            else
            {
                yield return FadeReturnCanvasGroup(returnFadeCanvasGroup.alpha, 1f, returnFadeDuration);
            }
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetForNewRun();
        }

        SceneManager.LoadScene(nextSceneName);
    }

    private void TryFindReturnFadeCanvasGroup()
    {
        if (returnFadeCanvasGroup != null)
            return;

        var groups = Object.FindObjectsOfType<CanvasGroup>(true);
        foreach (var g in groups)
        {
            if (g.name.ToLower().Contains("fade"))
            {
                returnFadeCanvasGroup = g;
                break;
            }
        }
    }

    private void ResetReturnFade()
    {
        if (returnFadeCanvasGroup == null)
            return;

        returnFadeCanvasGroup.alpha = 0f;
        returnFadeCanvasGroup.blocksRaycasts = false;
        returnFadeCanvasGroup.interactable = false;

        SetLoseTextAlpha(1f);
    }

    private System.Collections.IEnumerator FadeReturnCanvasGroup(float startAlpha, float targetAlpha, float duration)
    {
        if (returnFadeCanvasGroup == null)
            yield break;

        if (duration <= 0f)
        {
            returnFadeCanvasGroup.alpha = targetAlpha;
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            returnFadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            yield return null;
        }

        returnFadeCanvasGroup.alpha = targetAlpha;
    }

    private System.Collections.IEnumerator FadeLoseTextAlpha(float startAlpha, float targetAlpha, float duration)
    {
        if (loseText == null)
            yield break;

        if (duration <= 0f)
        {
            SetLoseTextAlpha(targetAlpha);
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            SetLoseTextAlpha(Mathf.Lerp(startAlpha, targetAlpha, t));
            yield return null;
        }

        SetLoseTextAlpha(targetAlpha);
    }

    private void SetLoseTextAlpha(float alpha)
    {
        if (loseText == null)
            return;

        var color = loseText.color;
        color.a = Mathf.Clamp01(alpha);
        loseText.color = color;
    }

    private float GetLoseTextAlpha()
    {
        if (loseText == null)
            return 1f;

        return loseText.color.a;
    }

    private bool UsesSharedLoseFadePanel()
    {
        return losePanel != null
            && returnFadeCanvasGroup != null
            && returnFadeCanvasGroup.gameObject == losePanel;
    }
}
