using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wormhole : MonoBehaviour
{
    [SerializeField]
    private SimpleGameEvent onWormholeCleared;

    public void OnTriggerExit(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController != null)
        {
            onWormholeCleared.Raise();
        }
    }
}
