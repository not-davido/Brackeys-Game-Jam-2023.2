using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Trap
{
    private void Start()
    {
        var initialization = GetComponentInParent<TrapHitSpawnInitialization>();
        initialization.InitializeSpawn(this);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        Destroy(gameObject);
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }
}
