using UnityEngine;

public class TutorialPromptAnimator : MonoBehaviour
{
    [Header("Sprite Animation (2 Frames)")]
    [SerializeField] private SpriteRenderer targetRenderer;
    [SerializeField] private Sprite frameA;
    [SerializeField] private Sprite frameB;
    [SerializeField] private float frameDuration = 0.35f;

    private Coroutine _animationRoutine;

    private void Awake()
    {
        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<SpriteRenderer>();
        }
    }

    private void OnEnable()
    {
        StartAnimation();
    }

    private void OnDisable()
    {
        StopAnimation();
    }

    private void StartAnimation()
    {
        if (targetRenderer == null || frameA == null || frameB == null)
        {
            return;
        }

        StopAnimation();
        _animationRoutine = StartCoroutine(Animate());
    }

    private void StopAnimation()
    {
        if (_animationRoutine != null)
        {
            StopCoroutine(_animationRoutine);
            _animationRoutine = null;
        }
    }

    private System.Collections.IEnumerator Animate()
    {
        var showA = true;
        targetRenderer.sprite = frameA;

        while (true)
        {
            yield return new WaitForSeconds(frameDuration);

            showA = !showA;
            targetRenderer.sprite = showA ? frameA : frameB;
        }
    }
}
