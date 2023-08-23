using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// Is the game transitioning? Ex: Level or game ending transitions.
    /// </summary>
    public bool GameIsTransitioning { get; private set; }

    /// <summary>
    /// Quick access to the player instance.
    /// </summary>
    KinematicPlayerController2D player;
    Vector3 newPositionAfterDamage;

    private void Awake()
    {
        EventManager.AddListener<GameTransitionEvent>(OnGameTransition);

        player = FindFirstObjectByType<KinematicPlayerController2D>(FindObjectsInactive.Exclude);
    }

    public KinematicPlayerController2D GetPlayer() {
        return player;
    }

    public void SetPlayerActive(bool active) {
        player.gameObject.SetActive(active);
    }

    public void GetPositionAfterDamage(Vector3 position) {
        newPositionAfterDamage = position;
    }

    public void SetPlayerPositionAfterDamage() {
        player.transform.position = newPositionAfterDamage;
    }

    void OnGameTransition(GameTransitionEvent evt) {
        GameIsTransitioning = evt.isTransitioning;
    }
}
