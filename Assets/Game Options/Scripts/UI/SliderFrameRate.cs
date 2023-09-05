using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderFrameRate : MonoBehaviour
{
    [SerializeField] TMP_Text frameRateText;

    Slider slider;

    private const float min = 10;
    private const float max = 251;

    private void OnEnable() {
        slider = GetComponent<Slider>();
        slider.minValue = min;
        slider.maxValue = max;
        Initialize();
        slider.onValueChanged.AddListener(UpdateValue);
        UpdateValue(slider.value);
    }

    private void OnDisable()
    {
        slider.onValueChanged.RemoveListener(UpdateValue);
    }

    void Initialize() {
        var value = GameOption.Get<GraphicOption>().FrameRate == -1 ? max : GameOption.Get<GraphicOption>().FrameRate;
        slider.SetValueWithoutNotify(value);
    }

    void UpdateValue(float value) {
        var frameRate = value == max ? -1 : value;
        GameOption.Get<GraphicOption>().FrameRate = (int)frameRate;
        frameRateText.text = $"Frame Rate {(frameRate == -1 ? "Unlimited" : (int)frameRate)}";
    }
}
