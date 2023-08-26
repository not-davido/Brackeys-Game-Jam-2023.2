using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapHitSpawnInitialization : MonoBehaviour
{
    [SerializeField] Transform HitSpawn;

    // Start is called before the first frame update
    void Start()
    {
        Trap[] traps = GetComponentsInChildren<Trap>();

        foreach (var trap in traps) {
            trap.positionAfterHit = HitSpawn;
        }
    }

    public void InitializeSpawn(Trap trap) {
        trap.positionAfterHit = HitSpawn;
    }
}
