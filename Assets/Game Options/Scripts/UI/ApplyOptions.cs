using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApplyOptions : MonoBehaviour
{
    Button button;

    private void OnEnable() {
        button = GetComponent<Button>();
        button.onClick.AddListener(Apply);
    }

    private void OnDisable() {
        button.onClick.RemoveListener(Apply);
    }

    public void Apply() {
        GameOption.Get<GraphicOption>().Apply();
    }
}
