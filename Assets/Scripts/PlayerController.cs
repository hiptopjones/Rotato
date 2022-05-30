using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform rabbit;

    [SerializeField]
    private int numRows;

    [SerializeField]
    private int numColumns;

    [SerializeField]
    private float keyRepeatTime;

    private float keyDownTime;

    void Update()
    {
        int deltaX = 0;
        int deltaY = 0;

        keyDownTime += Time.deltaTime;

        HandleKeyPress(KeyCode.UpArrow, 1, ref deltaY);
        HandleKeyPress(KeyCode.DownArrow, -1, ref deltaY);
        HandleKeyPress(KeyCode.LeftArrow, -1, ref deltaX);
        HandleKeyPress(KeyCode.RightArrow, 1, ref deltaX);

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x + deltaX, -numColumns / 2f, numColumns / 2f),
            Mathf.Clamp(transform.position.y + deltaY, -numRows / 2f, numRows / 2f),
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
            }
            else
            {
                if (keyDownTime >= keyRepeatTime)
                {
                    keyDownTime -= keyRepeatTime;
                    delta += increment;
                }
            }
        }
    }
}