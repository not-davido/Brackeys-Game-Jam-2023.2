using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class DebugWindow : MonoBehaviour
{
    [Tooltip("Delay between updates of the displayed framerate value")]
    [SerializeField] float PollingTime = 0.5f;

    [SerializeField] TextMeshProUGUI FullscreenResolution;
    [SerializeField] TextMeshProUGUI WindowResolution;
    [SerializeField] TextMeshProUGUI IsFullscreen;
    [SerializeField] TextMeshProUGUI Vsync;
    [SerializeField] TextMeshProUGUI CurrentFrameRate;
    [SerializeField] TextMeshProUGUI TargetFrameRate;
    [SerializeField] TextMeshProUGUI Quality;

    GraphicOption graphicOption;
    float m_AccumulatedDeltaTime = 0f;
    int m_AccumulatedFrameCount = 0;

    private void Start()
    {
        graphicOption = GameOption.Get<GraphicOption>();
    }

    private void Update()
    {
        m_AccumulatedDeltaTime += Time.deltaTime;
        m_AccumulatedFrameCount++;

        if (m_AccumulatedDeltaTime >= PollingTime) {
            int framerate = Mathf.RoundToInt((float)m_AccumulatedFrameCount / m_AccumulatedDeltaTime);
            CurrentFrameRate.text = $"Current Frame Rate: {framerate}";

            m_AccumulatedDeltaTime = 0f;
            m_AccumulatedFrameCount = 0;
        }

        FullscreenResolution.text = $"Fullscreen Resolution: {graphicOption.FullscreenWidth}x{graphicOption.FullscreenHeight}";
        WindowResolution.text = $"Window Resolution: {graphicOption.WindowWidth}x{graphicOption.WindowHeight}";
        IsFullscreen.text = $"Is Fullscreen: {graphicOption.IsFullscreen}";
        Vsync.text = $"Vsync: {graphicOption.Vsync}";
        TargetFrameRate.text = $"Target Frame Rate: {graphicOption.FrameRate}";
        Quality.text = $"Quality: {graphicOption.Quality}";
    }
}
