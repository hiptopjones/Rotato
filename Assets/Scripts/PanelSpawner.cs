using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform parent;

    [SerializeField]
    private GameObject voxelPrefab;

    [SerializeField]
    private GameObject panelPrefab;

    [SerializeField]
    private Material emptyMaterial;

    [SerializeField]
    private Material solidMaterial;

    [SerializeField]
    private Material powerUpMaterial;

    [SerializeField]
    private LevelSettings levelSettings;

    [SerializeField]
    private GameMode gameMode;

    private System.Random random = new System.Random();
    private int spawnCount;

    public void SpawnPanel()
    {
        float z = spawnCount * gameMode.panelSpacing;

        Vector3 panelPosition = new Vector3(0, 0, z);
        GameObject panel = Instantiate(panelPrefab, panelPosition, Quaternion.identity, parent);
        panel.name = $"Panel {spawnCount}";

        BoxCollider panelBoxCollider = panel.GetComponent<BoxCollider>();
        panelBoxCollider.center = new Vector3(0, 0, -1);
        panelBoxCollider.size = new Vector3(levelSettings.panelSize, levelSettings.panelSize, 3);

        int numVoxels = levelSettings.panelSize * levelSettings.panelSize;
        int powerUpIndex = random.Next(numVoxels);
        int powerUpX = powerUpIndex % levelSettings.panelSize;
        int powerUpY = powerUpIndex / levelSettings.panelSize;

        for (int y = 0; y < levelSettings.panelSize; y++)
        {
            for (int x = 0; x < levelSettings.panelSize; x++)
            {
                int voxelIndex = y * levelSettings.panelSize + x;

                Vector3 voxelPosition = new Vector3(
                    x - Mathf.Floor(levelSettings.panelSize / 2f),
                    y - Mathf.Floor(levelSettings.panelSize / 2f),
                    z);
                GameObject obj = Instantiate(voxelPrefab, voxelPosition, Quaternion.identity, panel.transform);
                obj.name = $"Voxel ({x}, {y})";

                Voxel voxel = obj.GetComponent<Voxel>();
                MeshRenderer meshRenderer = voxel.GetComponentInChildren<MeshRenderer>();

                if (voxelIndex == powerUpIndex)
                {
                    voxel.isPowerUp = true;
                    meshRenderer.material = powerUpMaterial;
                }
                else
                {
                    // Create an empty moat around the power-up
                    if ((y >= powerUpY - 1 && y <= powerUpY + 1) && (x >= powerUpX - 1 && x <= powerUpX + 1))
                    {
                        voxel.isEmpty = true;
                        meshRenderer.material = emptyMaterial;
                    }
                    else
                    {
                        voxel.isSolid = true;
                        meshRenderer.material = solidMaterial;
                    }
                }
            }
        }

        spawnCount++;
    }
}
