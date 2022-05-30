using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    [SerializeField]
    private AudioClip crashSound;

    [SerializeField]
    private AudioClip powerUpSound;
    
    private AudioSource audioSource;
    private PlayerState playerState;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            throw new System.Exception($"Unable to get component of type {nameof(AudioSource)}");
        }

        playerState = GetComponent<PlayerState>();
        if (playerState == null)
        {
            throw new System.Exception($"Unable to get component of type {nameof(PlayerState)}");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Voxel voxel = other.GetComponent<Voxel>();
        if (voxel == null)
        {
            return;
        }

        if (voxel.isSolid)
        {
            OnCrash();
        }
        else if (voxel.isTarget)
        {
            OnPowerUp();
        }

        Destroy(voxel.transform.parent.gameObject, 0.1f);
    }

    private void OnCrash()
    {
        audioSource.clip = crashSound;
        audioSource.Play();

        playerState.OnCrash();
    }

    private void OnPowerUp()
    {
        audioSource.clip = powerUpSound;
        audioSource.Play();

        playerState.OnPowerUp();
    }
}
