using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocator : MonoBehaviour
{
    public Voxel selectedVoxel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            GameObject target = hit.collider.gameObject;
            selectedVoxel = target.GetComponentInParent<Voxel>();
            selectedVoxel.isSelected = true;
        }
    }
}
