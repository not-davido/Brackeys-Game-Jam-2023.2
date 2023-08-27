using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTreasure : MonoBehaviour
{
    bool collected;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collected) return;

        if (collision.TryGetComponent(out Player _)) {
            collected = true;
            EventManager.Broadcast(Events.PlayerWinEvent);
        }
    }
}
