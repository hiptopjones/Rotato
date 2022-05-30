using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Camera camera;

    [SerializeField]
    private Transform rabbit;

    [SerializeField]
    private GameObject leftMarker;

    [SerializeField]
    private GameObject rightMarker;

    [SerializeField]
    private AnimationCurve lerpCurve;

    [SerializeField]
    private float lerpTime;

    private float currentLerpTime;
    private Vector3 sourcePosition;
    private Vector3 targetPosition;

    private List<Vector3> horiztonalPositions;
    private List<Vector3> verticalPositions;
    private int currentHorizontalIndex;
    private int currentVerticalIndex;

    private int defaultHoriztonalIndex;

    // Start is called before the first frame update
    void Start()
    {
        // TODO: Check that variables are set properly

        sourcePosition = leftMarker.transform.position;
        targetPosition = leftMarker.transform.position;

        horiztonalPositions = new List<Vector3>
        {
            leftMarker.transform.position,
            rightMarker.transform.position
        };
        defaultHoriztonalIndex = horiztonalPositions.IndexOf(leftMarker.transform.position);

        verticalPositions = new List<Vector3>
        {
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            currentHorizontalIndex = defaultHoriztonalIndex;
            currentVerticalIndex = verticalPositions.IndexOf(horiztonalPositions[currentHorizontalIndex]);
            SwitchView(horiztonalPositions[currentHorizontalIndex]);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (currentHorizontalIndex >= 0)
                {
                    currentHorizontalIndex = Mathf.Clamp(currentHorizontalIndex + 1, 0, horiztonalPositions.Count - 1);
                    currentVerticalIndex = verticalPositions.IndexOf(horiztonalPositions[currentHorizontalIndex]);
                    SwitchView(horiztonalPositions[currentHorizontalIndex]);
                }
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                if (currentHorizontalIndex >= 0)
                {
                    currentHorizontalIndex = Mathf.Clamp(currentHorizontalIndex - 1, 0, horiztonalPositions.Count - 1);
                    currentVerticalIndex = verticalPositions.IndexOf(horiztonalPositions[currentHorizontalIndex]);
                    SwitchView(horiztonalPositions[currentHorizontalIndex]);
                }
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                if (currentVerticalIndex >= 0)
                {
                    currentVerticalIndex = Mathf.Clamp(currentVerticalIndex + 1, 0, verticalPositions.Count - 1);
                    currentHorizontalIndex = horiztonalPositions.IndexOf(verticalPositions[currentVerticalIndex]);
                    SwitchView(verticalPositions[currentVerticalIndex]);
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (currentVerticalIndex >= 0)
                {
                    currentVerticalIndex = Mathf.Clamp(currentVerticalIndex - 1, 0, verticalPositions.Count - 1);
                    currentHorizontalIndex = horiztonalPositions.IndexOf(verticalPositions[currentVerticalIndex]);
                    SwitchView(verticalPositions[currentVerticalIndex]);
                }
            }
        }

        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > lerpTime)
        {
            currentLerpTime = lerpTime;
        }

        float percent = currentLerpTime / lerpTime;
        float curvePercent = lerpCurve.Evaluate(percent);

        camera.transform.position = Vector3.Lerp(sourcePosition + rabbit.transform.position, targetPosition + rabbit.transform.position, curvePercent);
        camera.transform.LookAt(rabbit.transform);
    }


    private void SwitchView(Vector3 newTargetPosition)
    {
        // First we force any previous movement to complete
        camera.transform.position = targetPosition + rabbit.transform.position;
        camera.transform.LookAt(rabbit.transform);

        // Then we adjust the source and target
        sourcePosition = targetPosition;
        targetPosition = newTargetPosition;

        // Then we update the tweening time reference
        currentLerpTime = 0;
    }
}
