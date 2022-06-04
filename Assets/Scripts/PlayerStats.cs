using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField]
    private IntVariable moveCountVariable;

    [SerializeField]
    private IntVariable rotateCountVariable;

    [SerializeField]
    private IntVariable crashCountVariable;

    [SerializeField]
    private IntVariable energyCountVariable;

    [SerializeField]
    private IntVariable skipCountVariable;

    private void Start()
    {
        moveCountVariable.value = 0;
        rotateCountVariable.value = 0;
        crashCountVariable.value = 0;
        energyCountVariable.value = 0;
        skipCountVariable.value = 0;
    }

    public void OnMove()
    {
        moveCountVariable.value++;
    }

    public void OnRotate()
    {
        rotateCountVariable.value++;
    }
    
    public void OnSkip()
    {
        skipCountVariable.value++;
        energyCountVariable.value = Mathf.Max(energyCountVariable.value - 1, 0);
    }

    public void OnEnergy()
    {
        energyCountVariable.value++;
    }

    public void OnCrash()
    {
        crashCountVariable.value++;
        energyCountVariable.value = Mathf.Max(energyCountVariable.value - 2, 0);
    }
}
