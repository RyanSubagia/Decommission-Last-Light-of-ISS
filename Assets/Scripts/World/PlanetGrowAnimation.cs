using UnityEngine;

public class PlanetGrowAnimation : MonoBehaviour
{
    [Header("Scale Settings")]
    [SerializeField] private Vector3 startScale = new Vector3(0.2f, 0.2f, 1f);
    [SerializeField] private Vector3 endScale = new Vector3(1f, 1f, 1f);
    [SerializeField] private float duration = 3f;
    [SerializeField] private AnimationCurve easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private float _time;
    private bool _isPlaying;

    private void Awake()
    {
        transform.localScale = startScale;
    }

    private void Update()
    {
        if (!_isPlaying)
            return;

        _time += Time.deltaTime;
        float t = Mathf.Clamp01(_time / duration);
        float easedT = easeCurve.Evaluate(t);

        transform.localScale = Vector3.LerpUnclamped(startScale, endScale, easedT);

        if (t >= 1f)
        {
            _isPlaying = false;
        }
    }

    public void Play()
    {
        _time = 0f;
        _isPlaying = true;
        transform.localScale = startScale;
    }
}
