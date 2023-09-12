using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapHitSpawnInitialization : MonoBehaviour
{
    [SerializeField] Transform HitSpawn;

    // Start is called before the first frame update
    void Start()
    {
        Spawn(HitSpawn);
    }

    public void InitializeSpawn(Trap trap) {
        trap.positionAfterHit = HitSpawn;
    }

    public void Spawn(Transform transform) {
        HitSpawn = transform;

        Trap[] traps = GetComponentsInChildren<Trap>();

        foreach (var trap in traps) {
            trap.positionAfterHit = transform;
        }
    }
}
