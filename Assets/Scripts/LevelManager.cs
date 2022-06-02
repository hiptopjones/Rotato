using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameEvent onLevelStarted;

    void Start()
    {
        onLevelStarted.Raise();
    }
}
