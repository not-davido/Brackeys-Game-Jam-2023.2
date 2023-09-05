using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Toggle : MonoBehaviour
{
    protected UnityEngine.UI.Toggle toggle;

    private void OnEnable() {
        toggle = GetComponent<UnityEngine.UI.Toggle>();
        Initialize();
        toggle.onValueChanged.AddListener(UpdateOption);
        UpdateOption(toggle.isOn);
    }

    private void OnDisable() {
        toggle.onValueChanged.RemoveListener(UpdateOption);
    }

    protected abstract void Initialize();
    protected abstract void UpdateOption(bool value);
}
