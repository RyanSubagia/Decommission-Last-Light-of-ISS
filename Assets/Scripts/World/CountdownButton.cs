using UnityEngine;
using System.Collections; 

public class CountdownButton : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Effects")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip alarmSound;
    private bool _isPlayerInRange;
    private bool _used;

    private void Update()
    {
        if (_used || !_isPlayerInRange)
            return;

        if (Input.GetKeyDown(interactKey))
        {
            _used = true;

            if (audioSource != null && alarmSound != null)
            {
                audioSource.clip = alarmSound;
                audioSource.Play();
                StartCoroutine(StopAudioAfterDelay(1f)); 
            }
            else
            {
                Debug.LogWarning("AudioSource or AlarmSound is missing on CountdownButton.");
            }


            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartCountdown();
            }

            if (GoalProgression.Instance != null)
            {
                GoalProgression.Instance.OnDecommissionStarted();
            }
        }
    }

    private IEnumerator StopAudioAfterDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var indicator = other.GetComponentInParent<PlayerInteractionIndicator>();
        if (indicator != null)
        {
            _isPlayerInRange = true;
            indicator.Show();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var indicator = other.GetComponentInParent<PlayerInteractionIndicator>();
        if (indicator != null)
        {
            _isPlayerInRange = false;
            indicator.Hide();
        }
    }
}