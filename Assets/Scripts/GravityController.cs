using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    [SerializeField]
    private AudioClip gravitySound;

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
                // Move the step out of the way
                GameObject step = selectedVoxel.transform.parent.gameObject;
                Rigidbody rigidbody = step.AddComponent<Rigidbody>();
                rigidbody.useGravity = true;
                rigidbody.velocity = 50 * -Vector3.up;

                OnGravity();

                Destroy(step, 0.5f);
            }
        }
    }

    private void OnGravity()
    {
        audioSource.clip = gravitySound;
        audioSource.Play();

        playerState.OnGravity();
    }
}
