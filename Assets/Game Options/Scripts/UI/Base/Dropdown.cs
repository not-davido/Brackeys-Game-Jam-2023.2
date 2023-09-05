using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Dropdown : MonoBehaviour 
{
    protected TMP_Dropdown dropdown;

    private void OnEnable() {
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.options.Clear();
        InitializeOptions();
        dropdown.onValueChanged.AddListener(UpdateOptions);
        UpdateOptions(dropdown.value);
    }

    private void OnDisable()
    {
        dropdown.onValueChanged.RemoveListener(UpdateOptions);
    }

    protected abstract void InitializeOptions();
    protected abstract void UpdateOptions(int value);
}
