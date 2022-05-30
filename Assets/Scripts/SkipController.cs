using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipController : MonoBehaviour
{
    [SerializeField]
    private AudioClip skipSound;

    private AudioSource audioSource;
    private PlayerLocator playerLocator;
    private PlayerState playerState;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            throw new System.Exception($"Unable to get component of type {nameof(AudioSource)}");
        }

        playerLocator = GetComponent<PlayerLocator>();
        if (playerLocator == null)
        {
            throw new System.Exception($"Unable to get component of type {nameof(PlayerLocator)}");
        }

        playerState = GetComponent<PlayerState>();
        if (playerState == null)
        {
            throw new System.Exception($"Unable to get component of type {nameof(PlayerState)}");
        }
    }

    void Update()
    {
        Voxel selectedVoxel = playerLocator.selectedVoxel;
        if (selectedVoxel != null)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                GameObject step = selectedVoxel.transform.parent.gameObject;

                // Use physics to move the step out of the way in a fun way
                Rigidbody rigidbody = step.AddComponent<Rigidbody>();
                rigidbody.angularVelocity = step.transform.right * 5;
                rigidbody.velocity = 50 * -Vector3.up;

                OnSkip();

                Destroy(step, 0.5f);
            }
        }
    }

    private void OnSkip()
    {
        audioSource.clip = skipSound;
        audioSource.Play();

        playerState.OnSkip();
    }
}
