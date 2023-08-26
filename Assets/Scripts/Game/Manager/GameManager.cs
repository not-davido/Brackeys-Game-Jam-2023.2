using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// Quick access to the player instance.
    /// </summary>
    Player player;

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
}
