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

    private void Awake()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

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
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(skipKey))
        {
            LoadNextScene();
        }
    }

    private void SetupEndingPresentation()
    {
        var gm = GameManager.Instance;
        var endType = gm != null ? gm.LastEndType : GameEndType.LoseTimeUp;

        // Default: hide everything
        if (losePanel != null)
        {
            losePanel.SetActive(false);
        }

        if (videoPlayer != null)
        {
            videoPlayer.gameObject.SetActive(false);
        }

        switch (endType)
        {
            case GameEndType.WinEscapePod:
                if (videoPlayer != null && defaultWinClip != null)
                {
                    videoPlayer.gameObject.SetActive(true);
                    videoPlayer.clip = defaultWinClip;
                    videoPlayer.Play();
                }
                break;

            case GameEndType.WinAfterburner:
                if (videoPlayer != null && trueEndingClip != null)
                {
                    videoPlayer.gameObject.SetActive(true);
                    videoPlayer.clip = trueEndingClip;
                    videoPlayer.Play();
                }
                break;

            case GameEndType.LoseTimeUp:
            default:
                if (losePanel != null)
                {
                    losePanel.SetActive(true);
                }

                if (loseText != null)
                {
                    loseText.text = loseMessage;
                }
                break;
        }
    }

    private void OnVideoFinished(VideoPlayer source)
    {
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
