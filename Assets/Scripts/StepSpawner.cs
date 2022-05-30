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
    private Material emptyMaterial;

    [SerializeField]
    private Material solidMaterial;

    [SerializeField]
    private Material targetMaterial;

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
        RotationMovement rotationMovement = step.AddComponent<RotationMovement>();
        rotationMovement.lerpTime = 0.15f;
        rotationMovement.lerpCurve = AnimationCurve.Linear(0, 0, 1, 1);

        int numVoxels = numRows * numColumns;
        int targetIndex = random.Next(numVoxels);
        int targetX = targetIndex % numColumns;
        int targetY = targetIndex / numColumns;

        float z = spawnCount * spawnDistance;
        for (int y = 0; y < numRows; y++)
        {
            for (int x = 0; x < numColumns; x++)
            {
                int voxelIndex = y * numColumns + x;

                GameObject obj = Instantiate(voxelPrefab, new Vector3(x - Mathf.Floor(numColumns / 2f), y - Mathf.Floor(numRows / 2f), z), Quaternion.identity, step.transform);
                Voxel voxel = obj.GetComponent<Voxel>();

                if (voxelIndex == targetIndex)
                {
                    voxel.isTarget = true;
                    voxel.GetComponentInChildren<MeshRenderer>().material = targetMaterial;
                }
                else
                {
                    if ((y >= targetY - 1 && y <= targetY + 1) && (x >= targetX - 1 && x <= targetX + 1))
                    {
                        voxel.isEmpty = true;
                        voxel.GetComponentInChildren<MeshRenderer>().material = emptyMaterial;
                    }
                    else
                    {
                        voxel.isSolid = true;
                        voxel.GetComponentInChildren<MeshRenderer>().material = solidMaterial;
                    }
                }
            }
        }

        spawnCount++;
    }
}
