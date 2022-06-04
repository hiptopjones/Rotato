using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField]
    private BaseParameterizedGameEvent<bool> onGateCleared;

    [HideInInspector]
    public bool isEnergyCollected;

    public void OnTriggerExit(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController != null)
        {
            onGateCleared.Raise(isEnergyCollected);
        }
    }
}
