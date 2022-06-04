using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : MonoBehaviour
{
    [SerializeField]
    private CodedBasicGameEventListener fastForwardListener;

    [SerializeField]
    private CodedBasicGameEventListener gateEnteredListener;

    [SerializeField]
    private Vector3 velocity;

    [SerializeField]
    private float fastForwardMultiplier;

    private Vector3 actualVelocity;

    private void OnEnable()
    {
        fastForwardListener.OnEnable(OnFastForward);
        gateEnteredListener.OnEnable(OnGateEntered);
    }

    private void OnDisable()
    {
        fastForwardListener.OnDisable();
        gateEnteredListener.OnDisable();
    }

    private void Start()
    {
        actualVelocity = velocity;
    }

    private void Update()
    {
        transform.position += actualVelocity * Time.deltaTime;
    }

    private void OnFastForward()
    {
        actualVelocity = fastForwardMultiplier * velocity;
    }

    private void OnGateEntered()
    {
        actualVelocity = velocity;
    }
}
