using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Solid,
    Energy,
    Empty
}

public class Tile : MonoBehaviour
{
    [SerializeField]
    private SimpleGameEvent onCrashEvent;

    [SerializeField]
    private SimpleGameEvent onEnergyEvent;

    [SerializeField]
    private Material solidMaterial;

    [SerializeField]
    private Material energyMaterial;

    [SerializeField]
    private Material emptyMaterial;

    [SerializeField]
    private Material selectedMaterial;

    [HideInInspector]
    public TileType tileType;

    [HideInInspector]
    public bool isSelected;

    private MeshRenderer meshRenderer;
    private Material defaultMaterial;

    private void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    void Update()
    {
        meshRenderer.material = isSelected ? selectedMaterial : defaultMaterial;

        // Clear this every frame (set by player raycast)
        isSelected = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController != null)
        {
            switch (tileType)
            {
                case TileType.Solid:
                    onCrashEvent.Raise();
                    break;
                case TileType.Energy:
                    Gate gate = transform.parent.GetComponent<Gate>();
                    gate.isEnergyCollected = true;

                    onEnergyEvent.Raise();
                    break;
                case TileType.Empty:
                    // Ignored
                    break;
            }
        }
    }

    public void SetTileType(TileType tileType)
    {
        this.tileType = tileType;

        switch (tileType)
        {
            case TileType.Solid:
                defaultMaterial = solidMaterial;
                break;

            case TileType.Energy:
                defaultMaterial = energyMaterial;
                break;

            case TileType.Empty:
                defaultMaterial = emptyMaterial;
                break;

            default:
                throw new System.Exception($"Unhandled tile type: {tileType}");
        }
    }
}
