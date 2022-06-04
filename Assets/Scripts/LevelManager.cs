using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private SimpleGameEvent onLevelCompleted;

    [SerializeField]
    private BooleanCodedParameterizedGameEventListener gateClearedListener;

    [SerializeField]
    private CodedBasicGameEventListener wormholeClearedListener;

    private PlayerController playerController;
    private GateSpawner gateSpawner;
    private WormholeSpawner wormholeSpawner;

    private Queue<GameObject> gates = new Queue<GameObject>();
    private int gateIndex;

    private Queue<GameObject> wormholes = new Queue<GameObject>();
    private int wormholeIndex;

    private Vector3 nextSpawnPosition;

    private LevelSettings levelSettings;
    int numSequentialEnergyCollected;

    private void OnEnable()
    {
        wormholeClearedListener.OnEnable(OnWormholeCleared);
        gateClearedListener.OnEnable(OnGateCleared);
    }

    private void OnDisable()
    {
        wormholeClearedListener.OnDisable();
        gateClearedListener.OnDisable();
    }

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            throw new System.Exception($"Unable to find object of type {nameof(PlayerController)}");
        }

        gateSpawner = FindObjectOfType<GateSpawner>();
        if (gateSpawner == null)
        {
            throw new System.Exception($"Unable to find object of type {nameof(GateSpawner)}");
        }

        wormholeSpawner = FindObjectOfType<WormholeSpawner>();
        if (wormholeSpawner == null)
        {
            throw new System.Exception($"Unable to find object of type {nameof(WormholeSpawner)}");
        }

        nextSpawnPosition = transform.position;
    }

    // Show player
    // Level intro animations
    // Start gate spawner
    // Enable player
    // Play game
    // Wait for N gates cleared in a row
    // Destroy any remaining gates
    // Disable player
    // Level outro animations
    // Fire event for game manager to load next level

    public void PlayLevel(LevelSettings levelSettings)
    {
        numSequentialEnergyCollected = 0;

        this.levelSettings = levelSettings;
        playerController.SetLevelSettings(levelSettings);

        SpawnWormhole();

        for (int i = 0; i < levelSettings.numGatesToSpawn; i++)
        {
            SpawnGate();
        }
    }

    private void OnWormholeCleared()
    {
        // Remove the wormhole just cleared
        GameObject clearedWormhole = wormholes.Dequeue();
        Destroy(clearedWormhole, 0.1f);
    }

    private void OnGateCleared(bool isEnergyCollected)
    {
        // Remove the gate just cleared
        GameObject clearedGate = gates.Dequeue();
        Destroy(clearedGate, 0.1f);

        // Check if the level is complete
        if (isEnergyCollected)
        {
            numSequentialEnergyCollected++;
            if (numSequentialEnergyCollected >= levelSettings.numSequentialEnergyToCollect)
            {
                // TODO: Play any "level completed" animation

                // Clean up remaining gates
                while (gates.Any())
                {
                    GameObject gate = gates.Dequeue();
                    
                    // Adjust next spawn position to earliest gate
                    if (gate.transform.position.z < nextSpawnPosition.z)
                    {
                        nextSpawnPosition = gate.transform.position;
                    }

                    Destroy(gate);
                }

                onLevelCompleted.Raise();

                // Early exit (do not spawn a replacement gate)
                return;
            }
        }
        else
        {
            numSequentialEnergyCollected = 0;
        }

        SpawnGate();
    }

    private void SpawnGate()
    {
        GameObject gate = gateSpawner.SpawnGate(gateIndex++, nextSpawnPosition, levelSettings.numTilesPerSide);
        gates.Enqueue(gate);

        nextSpawnPosition += new Vector3(0, 0, levelSettings.gateSpacing);
    }

    private void SpawnWormhole()
    {
        GameObject wormhole = wormholeSpawner.SpawnWormhole(wormholeIndex++, nextSpawnPosition, levelSettings.numTilesPerSide);
        wormholes.Enqueue(wormhole);

        nextSpawnPosition += new Vector3(0, 0, levelSettings.introWormholeLength);
    }
}