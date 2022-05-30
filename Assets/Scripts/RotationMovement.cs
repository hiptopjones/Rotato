using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationMovement : MonoBehaviour
{
    public AnimationCurve lerpCurve;
    public float lerpTime;


    private float sourceAngle;
    private float targetAngle;
     private float currentLerpTime;

    // Update is called once per frame
    void Update()
    {
        if (currentLerpTime == lerpTime)
        {
            return;
        }

        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > lerpTime)
        {
            currentLerpTime = lerpTime;
        }

        float percent = currentLerpTime / lerpTime;
        float curvePercent = lerpCurve.Evaluate(percent);

        float lerpAngle = (targetAngle - sourceAngle) * curvePercent;
        float deltaAngle = sourceAngle + (lerpAngle - transform.eulerAngles.z);
        transform.Rotate(transform.forward, deltaAngle);
    }

    public void Rotate(float deltaAngle)
    {
        // Rotation already in progress
        if (currentLerpTime < lerpTime)
        {
            return;
        }

        sourceAngle = transform.eulerAngles.z;
        targetAngle = sourceAngle + deltaAngle;
        currentLerpTime = 0;
    }
}
