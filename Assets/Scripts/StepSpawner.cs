using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform parent;

    [SerializeField]
    private GameObject voxelPrefab;

    [SerializeField]
    private Material spaceMaterial;

    [SerializeField]
    private Material lineMaterial;

    [SerializeField]
    private Material validMaterial;

    [SerializeField]
    private float spawnTime;

    [SerializeField]
    private float spawnDistance;

    [SerializeField]
    private int numRows;

    [SerializeField]
    private int numColumns;

    private System.Random random = new System.Random();
    private int spawnCount;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CoSpawnStep());
    }

    private IEnumerator CoSpawnStep()
    {
        while (true)
        {
            SpawnStep();
            yield return new WaitForSeconds(spawnTime);
        }
    }

    private void SpawnStep()
    {
        GameObject step = new GameObject($"Step {spawnCount}");
        step.transform.parent = parent;

        int numVoxels = numRows * numColumns;
        int validIndex = random.Next(numVoxels);

        float z = spawnCount * spawnDistance;
        for (int y = 0; y < numRows; y++)
        {
            for (int x = 0; x < numColumns; x++)
            {
                int voxelIndex = y * numColumns + x;

                GameObject voxel = Instantiate(voxelPrefab, new Vector3(x - Mathf.Floor(numColumns / 2f), y - Mathf.Floor(numRows / 2f), z), Quaternion.identity, step.transform);
                if (voxelIndex == validIndex)
                {
                    voxel.GetComponentInChildren<MeshRenderer>().material = validMaterial;
                }
                else if (voxelIndex % 2 == 0)
                {
                    voxel.GetComponentInChildren<MeshRenderer>().material = spaceMaterial;
                }
                else
                {
                    voxel.GetComponentInChildren<MeshRenderer>().material = lineMaterial;
                }
            }
        }

        Destroy(step, 20);
        spawnCount++;
    }
}
