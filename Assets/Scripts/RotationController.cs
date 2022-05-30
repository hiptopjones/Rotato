using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    private PlayerLocator playerLocator;
    private PlayerState playerState;

    private void Start()
    {
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
            // TODO: Add a setting to allow inverting this
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Transform stepTransform = selectedVoxel.transform.parent;
                RotationMovement rotationMovement = stepTransform.GetComponent<RotationMovement>();
                rotationMovement.Rotate(90);

                playerState.OnRotate();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                Transform stepTransform = selectedVoxel.transform.parent;
                RotationMovement rotationMovement = stepTransform.GetComponent<RotationMovement>();
                rotationMovement.Rotate(-90);

                playerState.OnRotate();
            }
        }
    }
}
