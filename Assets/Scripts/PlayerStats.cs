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
    private IntVariable powerUpCountVariable;

    [SerializeField]
    private IntVariable skipCountVariable;

    private void Start()
    {
        moveCountVariable.value = 0;
        rotateCountVariable.value = 0;
        crashCountVariable.value = 0;
        powerUpCountVariable.value = 0;
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
        powerUpCountVariable.value = Mathf.Max(powerUpCountVariable.value - 1, 0);
    }

    public void OnPowerUp()
    {
        powerUpCountVariable.value++;
    }

    public void OnCrash()
    {
        crashCountVariable.value++;
        powerUpCountVariable.value = Mathf.Max(powerUpCountVariable.value - 2, 0);
    }
}
