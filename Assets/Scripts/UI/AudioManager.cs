using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgmSource;

    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip loseStinger;
    [SerializeField, Min(0.01f)] private float loseTransitionDuration = 0.25f;

    private Coroutine _audioTransitionRoutine;
    private float _bgmDefaultVolume = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            if (bgmSource != null)
            {
                _bgmDefaultVolume = bgmSource.volume;
            }

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance.sfxSource == null)
            {
                instance.sfxSource = sfxSource;
            }

            if (instance.clickSound == null)
            {
                instance.clickSound = clickSound;
            }

            if (instance.bgmSource == null)
            {
                instance.bgmSource = bgmSource;
            }

            if (instance.loseStinger == null)
            {
                instance.loseStinger = loseStinger;
            }

            if (instance.bgmSource != null && instance._bgmDefaultVolume <= 0f)
            {
                instance._bgmDefaultVolume = instance.bgmSource.volume;
            }

            Destroy(gameObject);
        }
    }

    public void PlayClickSFX()
    {
        if (sfxSource == null)
        {
            sfxSource = GetComponent<AudioSource>();
        }

        if (sfxSource == null || clickSound == null)
            return;

        sfxSource.PlayOneShot(clickSound);
    }

    public float TransitionToLoseEndingAudio()
    {
        if (_audioTransitionRoutine != null)
        {
            StopCoroutine(_audioTransitionRoutine);
        }

        float stingerDuration = 0f;

        if (sfxSource == null)
        {
            sfxSource = GetComponent<AudioSource>();
        }

        if (sfxSource != null && loseStinger != null)
        {
            sfxSource.PlayOneShot(loseStinger);
            stingerDuration = loseStinger.length;
        }

        if (bgmSource != null)
        {
            _audioTransitionRoutine = StartCoroutine(FadeOutBgmForLoseRoutine());
        }

        return stingerDuration;
    }

    private IEnumerator FadeOutBgmForLoseRoutine()
    {
        float fadeDuration = Mathf.Clamp(loseTransitionDuration, 0.01f, 0.5f);

        float elapsed = 0f;
        float startVolume = bgmSource.volume;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, t);
            yield return null;
        }

        bgmSource.volume = 0f;
        bgmSource.Stop();
    }
}
