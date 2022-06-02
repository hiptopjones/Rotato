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

    [SerializeField]
    private GameEvent onInvalidEvent;

    [SerializeField]
    private GameEvent onPanelCompletedEvent;

    private float keyDownTime;
    private bool isInputDisabled;

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
            GameObject selectedPanel = selectedVoxel.transform.parent.gameObject;

            if (levelSettings.isSkipEnabled)
            {
                HandleSkip(selectedPanel);
            }

            if (levelSettings.isRotation90Enabled)
            {
                HandleRotation(selectedPanel);
            }
        }

        HandleMovement();
    }

    private void HandleRotation(GameObject selectedPanel)
    {
        // TODO: Add a setting to allow inverting this direction?
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (isInputDisabled)
            {
                onInvalidEvent.Raise();
            }
            else
            {
                Transform panelTransform = selectedPanel.transform;
                RotationMovement rotationMovement = panelTransform.GetComponent<RotationMovement>();
                rotationMovement.Rotate(90);

                onRotateEvent.Raise();
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (isInputDisabled)
            {
                onInvalidEvent.Raise();
            }
            else
            {
                Transform panelTransform = selectedPanel.transform;
                RotationMovement rotationMovement = panelTransform.GetComponent<RotationMovement>();
                rotationMovement.Rotate(-90);

                onRotateEvent.Raise();
            }
        }
    }

    void HandleSkip(GameObject selectedPanel)
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (isInputDisabled)
            {
                onInvalidEvent.Raise();
            }
            else
            {
                // Use physics to move the panel out of the way in a fun way
                Rigidbody rigidbody = selectedPanel.GetComponent<Rigidbody>();
                rigidbody.angularVelocity = selectedPanel.transform.right * 5;
                rigidbody.velocity = 50 * -Vector3.up;

                onSkipEvent.Raise();
                onPanelCompletedEvent.Raise();

                // TODO: Is this the right place to do this?
                Destroy(selectedPanel, 0.5f);
            }
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
            Mathf.Clamp(transform.position.x + deltaX, Mathf.Ceil(-levelSettings.panelSize / 2f), Mathf.Floor(levelSettings.panelSize / 2f)),
            Mathf.Clamp(transform.position.y + deltaY, Mathf.Ceil(-levelSettings.panelSize / 2f), Mathf.Floor(levelSettings.panelSize / 2f)),
            rabbit.position.z);
    }

    private void HandleKeyPress(KeyCode keycode, int increment, ref int delta)
    {
        if (Input.GetKey(keycode))
        {
            if (Input.GetKeyDown(keycode))
            {
                if (isInputDisabled)
                {
                    onInvalidEvent.Raise();
                }
                else
                {
                    keyDownTime = 0;
                    delta += increment;

                    onMoveEvent.Raise();
                }
            }
            else
            {
                if (isInputDisabled)
                {
                    // Ignore
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
    }

    private void OnTriggerEnter(Collider other)
    {
        Voxel voxel = other.GetComponent<Voxel>();
        if (voxel != null)
        {
            HandleVoxelCollision(voxel);
            return;
        }

        Panel panel = other.GetComponent<Panel>();
        if (panel != null)
        {
            HandlePanelCollision(panel);
            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Panel panel = other.GetComponent<Panel>();
        if (panel != null)
        {
            isInputDisabled = false;
            Destroy(panel.gameObject, 0.1f);
        }
    }

    private void HandleVoxelCollision(Voxel voxel)
    {
        if (voxel.isSolid)
        {
            onCrashEvent.Raise();
            onPanelCompletedEvent.Raise();
        }
        else if (voxel.isPowerUp)
        {
            onPowerUpEvent.Raise();
            onPanelCompletedEvent.Raise();
        }
        else if (voxel.isEmpty)
        {
            onPanelCompletedEvent.Raise();
        }
    }

    private void HandlePanelCollision(Panel panel)
    {
        isInputDisabled = true;
    }

    private Voxel GetSelectedVoxel()
    {
        Voxel selectedVoxel = null;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, (1 << 10))) // Voxels is layer 10
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
