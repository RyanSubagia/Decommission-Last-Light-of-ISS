using UnityEngine;

public class AutoScroll : MonoBehaviour
{
    public float speed = 2.0f;
    private float length;
    private Vector3 startPosition;

    void Start()
    {
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        startPosition = transform.localPosition;
    }

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        if (transform.localPosition.x < startPosition.x - length)
        {
            transform.localPosition += new Vector3(length * 2, 0, 0);
        }
    }
}