using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [SerializeField]
    private int numMoves;
    [SerializeField]
    private int numRotates;
    [SerializeField]
    private int numPowerUps;
    [SerializeField]
    private int numSkips;
    [SerializeField]
    private int numCrashes;

    public void OnCrash()
    {
        numCrashes++;
    }

    public void OnPowerUp()
    {
        numPowerUps++;
    }

    public void OnSkip()
    {
        numSkips++;
    }

    public void OnMove()
    {
        numMoves++;
    }

    public void OnRotate()
    {
        numRotates++;
    }
}
