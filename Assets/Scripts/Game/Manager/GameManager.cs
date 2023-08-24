using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// Quick access to the player instance.
    /// </summary>
    Player player;
    Vector3 newPositionAfterDamage;

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

    public void GetPositionAfterDamage(Vector3 position) {
        newPositionAfterDamage = position;
    }

    public void SetPlayerPositionAfterDamage() {
        player.transform.position = newPositionAfterDamage;
    }
}
