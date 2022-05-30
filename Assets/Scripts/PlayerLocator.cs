using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocator : MonoBehaviour
{
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
            Voxel voxel = target.GetComponentInParent<Voxel>();
            voxel.isSelected = true;
        }
    }
}
