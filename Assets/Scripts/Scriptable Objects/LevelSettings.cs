using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelSettings : ScriptableObject
{
    public string levelName;
    public int mapSize;
    public int numPowerUps;
    public bool isContinuousMovementEnabled;
    public bool isRotation90Enabled;
    public bool isRotation180Enabled;
    public bool isMirrorEnabled;
    public bool isSkipEnabled;
    public bool isBurstEnabled;
    public bool isLockAndPullEnabled;
}
