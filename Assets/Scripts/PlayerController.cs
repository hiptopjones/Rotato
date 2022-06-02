using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform rabbit;

    [Header("Settings")]
    [SerializeField]
    private LevelSettings levelSettings;

    [SerializeField]
    private GameSettings gameSettings;

    [Header("Events")]
    [SerializeField]
    private GameEvent onMoveEvent;

    [SerializeField]
    private GameEvent onRotateEvent;

    [SerializeField]
    private GameEvent onCrashEvent;

    [SerializeField]
    private GameEvent onPowerUpEvent;

    [SerializeField]
    private GameEvent onSkipEvent;

    private float keyDownTime;

    private void Start()
    {
        CheckNotNull(rabbit, nameof(rabbit));
        CheckNotNull(levelSettings, nameof(levelSettings));
        CheckNotNull(gameSettings, nameof(gameSettings));
        CheckNotNull(onMoveEvent, nameof(onMoveEvent));
    }

    private void CheckNotNull(object value, string variableName)
    {
        if (value == null)
        {
            throw new System.Exception($"Variable {variableName} is null");
        }
    }

    void Update()
    {
        Voxel selectedVoxel = GetSelectedVoxel();
        if (selectedVoxel != null)
        {
            GameObject selectedStep = selectedVoxel.transform.parent.gameObject;

            if (levelSettings.isSkipEnabled)
            {
                HandleSkip(selectedStep);
            }

            if (levelSettings.isRotation90Enabled)
            {
                HandleRotation(selectedStep);
            }
        }

        HandleMovement();
    }

    private void HandleRotation(GameObject selectedStep)
    {
        // TODO: Add a setting to allow inverting this direction?
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Transform stepTransform = selectedStep.transform;
            RotationMovement rotationMovement = stepTransform.GetComponent<RotationMovement>();
            rotationMovement.Rotate(90);

            onRotateEvent.Raise();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Transform stepTransform = selectedStep.transform;
            RotationMovement rotationMovement = stepTransform.GetComponent<RotationMovement>();
            rotationMovement.Rotate(-90);

            onRotateEvent.Raise();
        }
    }

    void HandleSkip(GameObject selectedStep)
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            // Use physics to move the step out of the way in a fun way
            Rigidbody rigidbody = selectedStep.AddComponent<Rigidbody>();
            rigidbody.angularVelocity = selectedStep.transform.right * 5;
            rigidbody.velocity = 50 * -Vector3.up;

            onSkipEvent.Raise();

            // TODO: Is this the right place to do this?
            Destroy(selectedStep, 0.5f);
        }
    }

    void HandleMovement()
    {
        int deltaX = 0;
        int deltaY = 0;

        keyDownTime += Time.deltaTime;

        HandleKeyPress(KeyCode.UpArrow, 1, ref deltaY);
        HandleKeyPress(KeyCode.DownArrow, -1, ref deltaY);
        HandleKeyPress(KeyCode.LeftArrow, -1, ref deltaX);
        HandleKeyPress(KeyCode.RightArrow, 1, ref deltaX);

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x + deltaX, Mathf.Ceil(-levelSettings.mapSize / 2f), Mathf.Floor(levelSettings.mapSize / 2f)),
            Mathf.Clamp(transform.position.y + deltaY, Mathf.Ceil(-levelSettings.mapSize / 2f), Mathf.Floor(levelSettings.mapSize / 2f)),
            rabbit.position.z);
    }

    private void HandleKeyPress(KeyCode keycode, int increment, ref int delta)
    {
        if (Input.GetKey(keycode))
        {
            if (Input.GetKeyDown(keycode))
            {
                keyDownTime = 0;
                delta += increment;

                onMoveEvent.Raise();
            }
            else
            {
                if (levelSettings.isContinuousMovementEnabled)
                {
                    if (keyDownTime >= gameSettings.keyRepeatTime)
                    {
                        keyDownTime -= gameSettings.keyRepeatTime;
                        delta += increment;

                        onMoveEvent.Raise();
                    }
                }
            }
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
            onCrashEvent.Raise();
        }
        else if (voxel.isTarget)
        {
            onPowerUpEvent.Raise();
        }

        // TODO: Is this the right place to do this?
        Destroy(voxel.transform.parent.gameObject, 0.1f);
    }

    private Voxel GetSelectedVoxel()
    {
        Voxel selectedVoxel = null;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            GameObject target = hit.collider.gameObject;
            selectedVoxel = target.GetComponentInParent<Voxel>();
            if (selectedVoxel != null)
            {
                // TODO: Use an event for this?
                selectedVoxel.isSelected = true;
            }
        }

        return selectedVoxel;
    }
}
