using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowResolutionKeeper : MonoBehaviour
{
    GraphicOption graphicOption;

    [RuntimeInitializeOnLoadMethod]
    static void Init() {
        var gameobject = new GameObject("Window Resolution Keeper");
        gameobject.AddComponent<WindowResolutionKeeper>();
        DontDestroyOnLoad(gameobject);
    }

    // Start is called before the first frame update
    void Start()
    {
        graphicOption = GameOption.Get<GraphicOption>();
    }

    // Update is called once per frame
    void Update()
    {
        if (graphicOption.IsFullscreen) return;

        Resolution cachedResolution = new() {
            width = graphicOption.WindowWidth,
            height = graphicOption.WindowHeight
        };

        Resolution currentResolution = new() {
            width = Screen.width,
            height = Screen.height
        };

        if (!GraphicOption.IsResolutionEqual(cachedResolution, currentResolution)) {
            graphicOption.WindowWidth = Screen.width;
            graphicOption.WindowHeight = Screen.height;
        }
    }
}
