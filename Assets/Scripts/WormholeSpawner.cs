using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormholeSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject parent;

    [SerializeField]
    private GameObject wormholePrefab;

    public GameObject SpawnWormhole(int wormholeIndex, Vector3 wormholePosition, int wormholeSize)
    {
        GameObject wormhole = Instantiate(wormholePrefab, wormholePosition, Quaternion.identity, parent.transform);
        wormhole.name = $"Wormhole {wormholeIndex}";

        // Adjust size of wormhole
        wormhole.transform.localScale = new Vector3(wormholeSize, wormholeSize, 1);

        // Adjust size of box collider to match
        BoxCollider boxCollider = wormhole.GetComponent<BoxCollider>();
        boxCollider.size = new Vector3(wormholeSize, wormholeSize, boxCollider.size.z);

        return wormhole;
    }
}
