using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsController : MonoBehaviour
{
    [SerializeField]
    private AudioClip moveSound;

    [SerializeField]
    private AudioClip rotateSound;

    [SerializeField]
    private AudioClip skipSound;

    [SerializeField]
    private AudioClip crashSound;

    [SerializeField]
    private AudioClip powerUpSound;

    [SerializeField]
    private AudioClip invalidSound;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            throw new System.Exception($"Unable to get component of type {nameof(AudioSource)}");
        }
    }

    public void PlayMoveSound()
    {
        audioSource.clip = moveSound;
        audioSource.Play();
    }

    public void PlayRotateSound()
    {
        audioSource.clip = rotateSound;
        audioSource.Play();
    }

    public void PlaySkipSound()
    {
        audioSource.clip = skipSound;
        audioSource.Play();
    }

    public void PlayCrashSound()
    {
        audioSource.clip = crashSound;
        audioSource.Play();
    }

    public void PlayPowerUpSound()
    {
        audioSource.clip = powerUpSound;
        audioSource.Play();
    }

    public void PlayInvalidSound()
    {
        audioSource.clip = invalidSound;
        audioSource.Play();
    }
}
