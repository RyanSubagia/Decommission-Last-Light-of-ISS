using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundUVScroll : MonoBehaviour
{
    [Tooltip("Kecepatan scroll background.")]
    public float speed = 0.1f;

    private Material _materialInstance;
    private Vector2 _offset;

    private void Awake()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        _materialInstance = Instantiate(spriteRenderer.sharedMaterial);
        spriteRenderer.material = _materialInstance;
    }

    private void Update()
    {
        if (_materialInstance == null)
            return;

        _offset.x += speed * Time.deltaTime;

        if (_offset.x > 1f)
        {
            _offset.x -= 1f;
        }
        else if (_offset.x < -1f)
        {
            _offset.x += 1f;
        }

        _materialInstance.mainTextureOffset = _offset;
    }
}
