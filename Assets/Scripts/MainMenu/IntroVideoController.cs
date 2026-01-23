using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroVideoController : MonoBehaviour
{
    [Header("Video Player")]
    [SerializeField] private VideoPlayer videoPlayer;

    [Header("Next Scene After Video")]
    [SerializeField] private string nextSceneName = "Game";

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

    private void Start()
    {
        if (videoPlayer != null && !videoPlayer.isPlaying)
        {
            videoPlayer.Play();
        }
    }

    private void Update()
    {
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
