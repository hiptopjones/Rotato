using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisController : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve lerpCurve;

    [SerializeField]
    private float lerpTime;

    private float currentLerpTime;
    private float sourceAngle;
    private float targetAngle;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                child.Rotate(child.forward, 90);
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                child.Rotate(child.forward, -90);
            }
        }

        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > lerpTime)
        {
            currentLerpTime = lerpTime;
        }

        float percent = currentLerpTime / lerpTime;
        float curvePercent = lerpCurve.Evaluate(percent);
    }
}
