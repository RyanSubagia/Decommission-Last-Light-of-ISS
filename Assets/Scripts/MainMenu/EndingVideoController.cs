using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class EndingVideoController : MonoBehaviour
{
    [Header("Video Player")]
    [SerializeField] private VideoPlayer videoPlayer;

    [Header("Next Scene After Video")]
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

    private void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
    }

    private void Update()
    {
        if (videoPlayer == null)
            return;

        if (Input.GetKeyDown(skipKey))
        {
            LoadNextScene();
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
