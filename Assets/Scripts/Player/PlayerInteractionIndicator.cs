using UnityEngine;

public class PlayerInteractionIndicator : MonoBehaviour
{
    [Header("Indicator Sprite (Animated)")]
    [SerializeField] private GameObject indicatorObject;
    [SerializeField] private SpriteRenderer indicatorRenderer;
    [SerializeField] private Sprite[] animationFrames;
    [SerializeField] private float frameDuration = 0.2f;
    [Header("Idle Pulse")]
    [SerializeField] private bool enablePulse = true;
    [SerializeField] private float pulseScale = 0.08f;
    [SerializeField] private float pulseSpeed = 2.5f;

    private Coroutine _animationRoutine;
    private int _currentFrameIndex;
    private Vector3 _baseScale;

    private void Awake()
    {
        if (indicatorRenderer == null && indicatorObject != null)
        {
            indicatorRenderer = indicatorObject.GetComponent<SpriteRenderer>();
        }

        if (indicatorObject != null)
        {
            indicatorObject.SetActive(false);
            _baseScale = indicatorObject.transform.localScale;
        }
    }

    private void OnDisable()
    {
        StopAnimation(resetFrame: false);
    }

    public void Show()
    {
        if (indicatorObject != null)
        {
            indicatorObject.SetActive(true);
            indicatorObject.transform.localScale = _baseScale;
            StartAnimation();
        }
    }

    public void Hide()
    {
        if (indicatorObject != null)
        {
            StopAnimation(resetFrame: true);
            indicatorObject.transform.localScale = _baseScale;
            indicatorObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!enablePulse || indicatorObject == null || !indicatorObject.activeSelf)
        {
            return;
        }

        var scaleOffset = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseScale;
        indicatorObject.transform.localScale = _baseScale * scaleOffset;
    }

    private void StartAnimation()
    {
        if (indicatorRenderer == null || animationFrames == null || animationFrames.Length < 2)
        {
            return;
        }

        StopAnimation(resetFrame: false);
        _animationRoutine = StartCoroutine(AnimateIndicator());
    }

    private void StopAnimation(bool resetFrame)
    {
        if (_animationRoutine != null)
        {
            StopCoroutine(_animationRoutine);
            _animationRoutine = null;
        }

        if (resetFrame && indicatorRenderer != null && animationFrames != null && animationFrames.Length > 0)
        {
            _currentFrameIndex = 0;
            indicatorRenderer.sprite = animationFrames[_currentFrameIndex];
        }
    }

    private System.Collections.IEnumerator AnimateIndicator()
    {
        _currentFrameIndex = 0;
        indicatorRenderer.sprite = animationFrames[_currentFrameIndex];

        while (true)
        {
            yield return new WaitForSeconds(frameDuration);

            _currentFrameIndex = (_currentFrameIndex + 1) % animationFrames.Length;
            indicatorRenderer.sprite = animationFrames[_currentFrameIndex];
        }
    }
}
