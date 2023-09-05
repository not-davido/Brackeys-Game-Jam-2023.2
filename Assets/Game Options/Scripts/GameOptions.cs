using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOptions : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    static void Init() {
        var gameobject = new GameObject("Game Options");
        gameobject.AddComponent<GameOptions>();
        DontDestroyOnLoad(gameobject);
        Load();
    }

    private void Start()
    {
        Apply();
    }

    static void Load() {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
            foreach (var type in assembly.GetTypes()) {
                if (!type.IsAbstract && type.IsSubclassOf(typeof(GameOption))) {
                    GameOption.Add(type);
                }
            }
        }
    }

    void Apply() {
        foreach (var option in GameOption.All) {
            option.Apply();
        }
    }
}
