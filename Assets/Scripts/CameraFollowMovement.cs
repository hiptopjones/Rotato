using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject follow;

    [SerializeField]
    private Vector3 offset;

    // Using LateUpdate() prevents camera wobble
    void LateUpdate()
    {
        transform.position = follow.transform.position + offset;
        transform.LookAt(follow.transform);
    }
}
