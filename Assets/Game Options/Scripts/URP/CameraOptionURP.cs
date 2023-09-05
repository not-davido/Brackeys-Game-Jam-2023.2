using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Rendering.Universal;

// Uncomment if using URP

//public class CameraOptionURP : GameOption
//{
//    static readonly List<UniversalAdditionalCameraData> s_Cameras = new();

//    public AntialiasingMode AntiAliasing {
//        get => (AntialiasingMode)PlayerPrefs.GetInt("CameraOptionURP", (int)AntialiasingMode.FastApproximateAntialiasing);
//        set => PlayerPrefs.SetInt("CameraOptionURP", (int)value);
//    }

//    public static void AddCamera(UniversalAdditionalCameraData camera) {
//        if (!s_Cameras.Contains(camera))
//            s_Cameras.Add(camera);
//    }

//    public static void RemoveCamera(UniversalAdditionalCameraData camera) {
//        if (s_Cameras.Contains(camera))
//            s_Cameras.Remove(camera);
//    }

//    public override void Apply() {
//        foreach (var camera in s_Cameras) {
//            camera.antialiasing = AntiAliasing;
//        }
//    }
//}
