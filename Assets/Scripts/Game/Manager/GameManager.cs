using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool GameIsTransitioningLevel;
    public bool GameIsQuiting;

    /// <summary>
    /// Quick access to the player instance.
    /// </summary>
    Player player;

    private void OnEnable()
    {
        EventManager.AddListener<GameQuitEvent>(OnGameQuit);
        EventManager.AddListener<LevelTransitionEvent>(OnLevelTransition);

    }

    private void OnDisable()
    {
        EventManager.RemoveListener<GameQuitEvent>(OnGameQuit);
        EventManager.RemoveListener<LevelTransitionEvent>(OnLevelTransition);
    }

    private void Awake()
    {
        player = FindFirstObjectByType<Player>(FindObjectsInactive.Exclude);
    }

    public Player GetPlayer() {
        return player;
    }

    public void SetPlayerActive(bool active) {
        player.gameObject.SetActive(active);
    }

    void OnLevelTransition(LevelTransitionEvent evt) {
        if (evt.isTransitioningIn)
            GameIsTransitioningLevel = true;
        else if (evt.isTransitioningOut)
            GameIsTransitioningLevel = false;
    }

    void OnGameQuit(GameQuitEvent evt) {
        GameIsQuiting = true;
        ScreenFade.Instance.FadeIn(1, 0.3f);
    }
}
