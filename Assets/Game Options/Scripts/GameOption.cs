using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class GameOption
{
    static readonly Dictionary<Type, GameOption> s_GameOptions = new();

    public static List<GameOption> All => s_GameOptions.Values.ToList();

    public static T Get<T>() where T : GameOption {
        Type t = typeof(T);

        if (!s_GameOptions.ContainsKey(t)) {
            Add(t);
        }

        return (T)s_GameOptions[t];
    }

    public static void Add(Type t) {
        if (!t.IsAbstract && t.IsSubclassOf(typeof(GameOption))) {
            if (!s_GameOptions.ContainsKey(t)) {
                s_GameOptions.Add(t, Create(t));
            }
        }
    }

    public static GameOption Create(Type t) {
        return (GameOption)Activator.CreateInstance(t);
    }

    public abstract void Apply();
}
