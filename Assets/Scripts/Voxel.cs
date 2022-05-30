using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel : MonoBehaviour
{
    [SerializeField]
    private Material defaultMaterial;

    [SerializeField]
    private Material selectedMaterial;

    public bool isSolid;
    public bool isTarget;
    public bool isEmpty;

    public bool isSelected;

    private MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (defaultMaterial == null)
        {
            meshRenderer = GetComponentInChildren<MeshRenderer>();
            defaultMaterial = meshRenderer.material;
        }

        meshRenderer.material = isSelected ? selectedMaterial : defaultMaterial;

        // Clear this every frame
        isSelected = false;
    }
}
