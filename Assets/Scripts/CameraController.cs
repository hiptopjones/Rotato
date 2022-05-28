using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Camera camera;

    [SerializeField]
    private GameObject cameraTarget;

    [SerializeField]
    private GameObject xAxisMarker;

    [SerializeField]
    private GameObject yAxisMarker;

    [SerializeField]
    private GameObject zAxisMarker;

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

        sourcePosition = zAxisMarker.transform.position;
        targetPosition = zAxisMarker.transform.position;

        horiztonalPositions = new List<Vector3>
        {
            xAxisMarker.transform.position,
            zAxisMarker.transform.position,
            -xAxisMarker.transform.position,
        };
        defaultHoriztonalIndex = horiztonalPositions.IndexOf(zAxisMarker.transform.position);

        verticalPositions = new List<Vector3>
        {
            -yAxisMarker.transform.position,
            zAxisMarker.transform.position,
            yAxisMarker.transform.position,
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
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (currentHorizontalIndex >= 0)
                {
                    currentHorizontalIndex = Mathf.Clamp(currentHorizontalIndex + 1, 0, horiztonalPositions.Count - 1);
                    currentVerticalIndex = verticalPositions.IndexOf(horiztonalPositions[currentHorizontalIndex]);
                    SwitchView(horiztonalPositions[currentHorizontalIndex]);
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (currentHorizontalIndex >= 0)
                {
                    currentHorizontalIndex = Mathf.Clamp(currentHorizontalIndex - 1, 0, horiztonalPositions.Count - 1);
                    currentVerticalIndex = verticalPositions.IndexOf(horiztonalPositions[currentHorizontalIndex]);
                    SwitchView(horiztonalPositions[currentHorizontalIndex]);
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (currentVerticalIndex >= 0)
                {
                    currentVerticalIndex = Mathf.Clamp(currentVerticalIndex + 1, 0, verticalPositions.Count - 1);
                    currentHorizontalIndex = horiztonalPositions.IndexOf(verticalPositions[currentVerticalIndex]);
                    SwitchView(verticalPositions[currentVerticalIndex]);
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
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

        camera.transform.position = Vector3.Lerp(sourcePosition, targetPosition, curvePercent);
        camera.transform.LookAt(cameraTarget.transform);
    }


    private void SwitchView(Vector3 newTargetPosition)
    {
        // First we force any previous movement to complete
        camera.transform.position = targetPosition;
        camera.transform.LookAt(cameraTarget.transform);

        // Then we adjust the source and target
        sourcePosition = targetPosition;
        targetPosition = newTargetPosition;

        // Then we update the tweening time reference
        currentLerpTime = 0;
    }
}
