using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip clickSound;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
}
