using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleFullScreenMode : Toggle
{
    protected override void Initialize() {
        toggle.isOn = GameOption.Get<GraphicOption>().IsFullscreen;
    }

    protected override void UpdateOption(bool value) {
        GameOption.Get<GraphicOption>().IsFullscreen = value;
    }
}
