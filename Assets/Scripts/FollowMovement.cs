using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMovement : MonoBehaviour
{
    [SerializeField]
    private Transform follow;

    [SerializeField]
    private Vector3 offset;

    // Using LateUpdate() prevents camera wobble
    void LateUpdate()
    {
        transform.position = follow.position + offset;
        transform.LookAt(follow.transform);
    }
}
