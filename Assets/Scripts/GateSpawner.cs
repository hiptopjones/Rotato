using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject parent;

    [SerializeField]
    private GameObject tilePrefab;

    [SerializeField]
    private GameObject gatePrefab;

    private System.Random random = new System.Random();

    public GameObject SpawnGate(int gateIndex, Vector3 gatePosition, int numTilesPerSide)
    {
        GameObject gate = Instantiate(gatePrefab, gatePosition, Quaternion.identity, parent.transform);
        gate.name = $"Gate {gateIndex}";

        BoxCollider gateBoxCollider = gate.GetComponent<BoxCollider>();
        gateBoxCollider.center = new Vector3(0, 0, -1);
        gateBoxCollider.size = new Vector3(numTilesPerSide, numTilesPerSide, 3); // TODO: Move collider center / size values to configuration?

        int numTiles = numTilesPerSide * numTilesPerSide;
        int energyIndex = random.Next(numTiles);
        int energyX = energyIndex % numTilesPerSide;
        int energyY = energyIndex / numTilesPerSide;

        for (int y = 0; y < numTilesPerSide; y++)
        {
            for (int x = 0; x < numTilesPerSide; x++)
            {
                int tileIndex = y * numTilesPerSide + x;

                Vector3 tilePosition = new Vector3(
                    x - Mathf.Floor(numTilesPerSide / 2f),
                    y - Mathf.Floor(numTilesPerSide / 2f),
                    gatePosition.z);
                GameObject obj = Instantiate(tilePrefab, tilePosition, Quaternion.identity, gate.transform);
                obj.name = $"Tile ({x}, {y})";

                Tile tile = obj.GetComponent<Tile>();
                if (tileIndex == energyIndex)
                {
                    tile.SetTileType(TileType.Energy);
                }
                else
                {
                    // Create an empty moat around the energy tile
                    if ((y >= energyY - 1 && y <= energyY + 1) && (x >= energyX - 1 && x <= energyX + 1))
                    {
                        tile.SetTileType(TileType.Empty);
                    }
                    else
                    {
                        tile.SetTileType(TileType.Solid);
                    }
                }
            }
        }

        return gate;
    }
}
