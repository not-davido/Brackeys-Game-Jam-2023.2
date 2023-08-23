using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseDetector : Singleton<PauseDetector>
{
    public bool IsGamePaused { get; private set; }

    private void Awake()
    {
        EventManager.AddListener<GamePauseEvent>(PauseGame);
    }

    void PauseGame(GamePauseEvent evt) {
        IsGamePaused = evt.paused;
    }

    private void OnDisable()
    {
        EventManager.RemoveListener<GamePauseEvent>(PauseGame);
    }
}