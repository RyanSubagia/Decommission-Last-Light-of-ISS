using UnityEngine;

public class PlanetGrowTrigger : MonoBehaviour
{
    [SerializeField] private PlanetGrowAnimation planetGrowAnimation;
    [SerializeField] private string playerTag = "Player";
    [Tooltip("Kalau true, animasi hanya diputar sekali saja.")]
    [SerializeField] private bool playOnce = true;

    private bool _hasPlayed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag))
            return;

        if (playOnce && _hasPlayed)
            return;

        _hasPlayed = true;

        if (planetGrowAnimation != null)
        {
            planetGrowAnimation.Play();
        }
    }
}
