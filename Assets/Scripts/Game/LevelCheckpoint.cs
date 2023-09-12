using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCheckpoint : MonoBehaviour
{
    public bool Reached { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Reached)
            return;

        if (collision.TryGetComponent(out Player _)) {
            FindFirstObjectByType<TrapHitSpawnInitialization>().Spawn(transform);
            Reached = true;

            EventManager.Broadcast(Events.CheckpointReachedEvent);
        }
    }
}
