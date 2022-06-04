using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityMovement : MonoBehaviour
{
    [SerializeField]
    private Vector3 velocity;

    void Update()
    {
        transform.position += velocity * Time.deltaTime;
    }
}
