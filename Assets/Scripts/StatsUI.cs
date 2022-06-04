using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField]
    private IntVariable moveCountVariable;

    [SerializeField]
    private IntVariable energyCountVariable;

    [Header("TextBoxes")]
    [SerializeField]
    private TextMeshProUGUI moveCountTextBox;

    [SerializeField]
    private TextMeshProUGUI energyCountTextBox;

    [SerializeField]
    private TextMeshProUGUI ratioTextBox;

    // Update is called once per frame
    void Update()
    {
        moveCountTextBox.text = moveCountVariable.value.ToString();
        energyCountTextBox.text = energyCountVariable.value.ToString();

        // Avoid divide-by-zero
        if (energyCountVariable.value > 0)
        {
            ratioTextBox.text = ((float)moveCountVariable.value / energyCountVariable.value).ToString("0.##");
        }
        else
        {
            ratioTextBox.text = "-";
        }
    }
}
