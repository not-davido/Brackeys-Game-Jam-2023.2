using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleVsync : Toggle
{
    [SerializeField] Slider targetFrameRateSlider;

    protected override void Initialize() {
        toggle.isOn = GameOption.Get<GraphicOption>().Vsync;
    }

    protected override void UpdateOption(bool value) {
        GameOption.Get<GraphicOption>().Vsync = value;
        UpdateTargetFrameRate(value);
    }

    void UpdateTargetFrameRate(bool value) {
        if (targetFrameRateSlider != null) {
            targetFrameRateSlider.interactable = !value;
            targetFrameRateSlider.targetGraphic.CrossFadeAlpha(value ? 0.1f : 1, targetFrameRateSlider.colors.fadeDuration, true);
        }
    }
}
