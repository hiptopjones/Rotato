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
    private IntVariable powerUpCountVariable;

    [Header("TextBoxes")]
    [SerializeField]
    private TextMeshProUGUI moveCountTextBox;

    [SerializeField]
    private TextMeshProUGUI powerUpCountTextBox;

    [SerializeField]
    private TextMeshProUGUI ratioTextBox;

    // Update is called once per frame
    void Update()
    {
        moveCountTextBox.text = moveCountVariable.value.ToString();
        powerUpCountTextBox.text = powerUpCountVariable.value.ToString();

        // Avoid divide-by-zero
        if (powerUpCountVariable.value > 0)
        {
            ratioTextBox.text = ((float)moveCountVariable.value / powerUpCountVariable.value).ToString("0.##");
        }
        else
        {
            ratioTextBox.text = "-";
        }
    }
}
