using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject rabbit;

    [Header("Settings")]
    [SerializeField]
    private GameSettings gameSettings;

    [Header("Events")]
    [SerializeField]
    private SimpleGameEvent onMoveEvent;

    [SerializeField]
    private SimpleGameEvent onRotateEvent;

    [SerializeField]
    private SimpleGameEvent onSkipEvent;

    [SerializeField]
    private SimpleGameEvent onFastForwardEvent;

    [SerializeField]
    private BaseParameterizedGameEvent<bool> onGateCleared;

    private LevelSettings levelSettings;

    private float keyDownTime;
    private bool isInputDisabled;

    void Update()
    {
        Tile selectedTile = GetSelectedTile();
        if (selectedTile != null)
        {
            GameObject selectedGate = selectedTile.transform.parent.gameObject;

            if (levelSettings.isSkipEnabled)
            {
                HandleSkip(selectedGate);
            }

            if (levelSettings.isRotation90Enabled)
            {
                HandleRotation(selectedGate);
            }

            if (levelSettings.isFastForwardEnabled)
            {
                HandleFastForward(selectedGate);
            }
        }

        HandleMovement();
    }

    private void HandleFastForward(GameObject selectedGate)
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (isInputDisabled)
            {
                // Ignore
            }
            else
            {
                // Disable input during the fast forward
                isInputDisabled = true;

                onFastForwardEvent.Raise();
            }
        }
    }

    private void HandleRotation(GameObject selectedGate)
    {
        // TODO: Should disable the raycast until rotation has stopped to avoid rays piercing this gate and selecting the next gate

        // TODO: Add a setting to allow inverting this direction?
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (isInputDisabled)
            {
                // Ignore
            }
            else
            {
                Transform gateTransform = selectedGate.transform;
                RotationMovement rotationMovement = gateTransform.GetComponent<RotationMovement>();
                rotationMovement.Rotate(90);

                onRotateEvent.Raise();
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (isInputDisabled)
            {
                // Ignore
            }
            else
            {
                Transform gateTransform = selectedGate.transform;
                RotationMovement rotationMovement = gateTransform.GetComponent<RotationMovement>();
                rotationMovement.Rotate(-90);

                onRotateEvent.Raise();
            }
        }
    }

    void HandleSkip(GameObject selectedGate)
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (isInputDisabled)
            {
                // Ignore
            }
            else
            {
                // TODO: Should disable the raycast until this gate is cleared to avoid rays piercing this gate and selecting the next gate
                // TODO: Skip should do a "hard drop" and bring the player forward, so they clear this gate at the same time

                // Use physics to move the gate out of the way in a fun way
                // Add the rigidbody manually because otherwise we get duplicate player collision events (investigate?)
                Rigidbody rigidbody = selectedGate.AddComponent<Rigidbody>();
                rigidbody.angularVelocity = selectedGate.transform.right * 5;
                rigidbody.velocity = 50 * -Vector3.up;

                onSkipEvent.Raise();
                onGateCleared.Raise(false);
            }
        }
    }

    void HandleMovement()
    {
        int deltaX = 0;
        int deltaY = 0;

        keyDownTime += Time.deltaTime;

        // TODO: Should key presses at boundaries still count as movement?
        HandleKeyPress(KeyCode.UpArrow, 1, ref deltaY);
        HandleKeyPress(KeyCode.DownArrow, -1, ref deltaY);
        HandleKeyPress(KeyCode.LeftArrow, -1, ref deltaX);
        HandleKeyPress(KeyCode.RightArrow, 1, ref deltaX);

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x + deltaX, Mathf.Ceil(-levelSettings.numTilesPerSide / 2f), Mathf.Floor(levelSettings.numTilesPerSide / 2f)),
            Mathf.Clamp(transform.position.y + deltaY, Mathf.Ceil(-levelSettings.numTilesPerSide / 2f), Mathf.Floor(levelSettings.numTilesPerSide / 2f)),
            rabbit.transform.position.z);
    }

    private void HandleKeyPress(KeyCode keycode, int increment, ref int delta)
    {
        if (Input.GetKey(keycode))
        {
            if (Input.GetKeyDown(keycode))
            {
                if (isInputDisabled)
                {
                    // Ignore
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
        Gate gate = other.GetComponent<Gate>();
        if (gate != null)
        {
            isInputDisabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Gate gate = other.GetComponent<Gate>();
        if (gate != null)
        {
            isInputDisabled = false;
        }
    }

    private Tile GetSelectedTile()
    {
        Tile selectedTile = null;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, (1 << 10))) // Tile layer is layer 10
        {
            GameObject target = hit.collider.gameObject;
            selectedTile = target.GetComponentInParent<Tile>();
            if (selectedTile != null)
            {
                // TODO: Use an event for this?
                selectedTile.isSelected = true;
            }
        }

        return selectedTile;
    }

    public void SetLevelSettings(LevelSettings levelSettings)
    {
        this.levelSettings = levelSettings;
    }
}
