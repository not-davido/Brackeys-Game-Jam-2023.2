using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SensitivityAdjustmentUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI valueText;
    [SerializeField] Button backButton;
    [SerializeField] Button forwardButton;

    int sensitivityValue = 5;

    private void OnEnable()
    {
        backButton.onClick.AddListener(Decrement);
        forwardButton.onClick.AddListener(Increment);
        valueText.text = sensitivityValue.ToString();
    }

    void Increment() {
        sensitivityValue++;
        valueText.text = sensitivityValue.ToString();
    }

    void Decrement() {
        sensitivityValue--;
        valueText.text = sensitivityValue.ToString();
    }

    private void OnDisable() {
        backButton.onClick.RemoveListener(Decrement);
        forwardButton.onClick.RemoveListener(Increment);
    }
}
