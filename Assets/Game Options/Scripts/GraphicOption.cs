using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicOption : GameOption
{
    public bool IsFullscreen {
        get => PlayerPrefs.GetInt("FullscreenOption", 0) == 1;
        set {
            PlayerPrefs.SetInt("FullscreenOption", value ? 1 : 0);
            ApplyFullscreen(value);
        }
    }

    public int FullscreenWidth {
        get => PlayerPrefs.GetInt("FullscreenWidth", Screen.currentResolution.width);
        set => PlayerPrefs.SetInt("FullscreenWidth", value);
    }

    public int FullscreenHeight {
        get => PlayerPrefs.GetInt("FullscreenHeight", Screen.currentResolution.height);
        set => PlayerPrefs.SetInt("FullscreenHeight", value);
    }

    public int WindowWidth {
        get => PlayerPrefs.GetInt("WindowWidth", 960);
        set => PlayerPrefs.SetInt("WindowWidth", value);
    }

    public int WindowHeight {
        get => PlayerPrefs.GetInt("WindowHeight", 540);
        set => PlayerPrefs.SetInt("WindowHeight", value);
    }

    public bool Vsync {
        get => PlayerPrefs.GetInt("VsyncOption", 0) == 1;
        set => PlayerPrefs.SetInt("VsyncOption", value ? 1 : 0);
    }

    public int FrameRate {
        get => PlayerPrefs.GetInt("FrameRateOption", 60);
        set => PlayerPrefs.SetInt("FrameRateOption", value);
    }

    public int Quality {
        get => PlayerPrefs.GetInt("QualityOption", QualitySettings.GetQualityLevel());
        set => PlayerPrefs.SetInt("QualityOption", value);
    }

    void ApplyFullscreen(bool isFullscreen) {
        var fullscreenMode = isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        var width = isFullscreen ? FullscreenWidth : WindowWidth;
        var height = isFullscreen ? FullscreenHeight : WindowHeight;

        Screen.SetResolution(width, height, fullscreenMode);
    }

    public override void Apply() {
        ApplyFullscreen(IsFullscreen);
        QualitySettings.SetQualityLevel(Quality);
        QualitySettings.vSyncCount = Vsync ? 1 : 0;
        Application.targetFrameRate = FrameRate;
    }

    public static bool IsResolutionEqual(Resolution a, Resolution b) {
        return a.width == b.width && a.height == b.height;
    }
}
