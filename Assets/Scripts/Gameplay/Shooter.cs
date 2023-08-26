using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] Transform projectileSpawn;
    [SerializeField] float projectileForce = 5;
    [SerializeField] float intervals = 2;

    Animator anim;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        timer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > timer + intervals) {
            var p = Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation, projectileSpawn);
            
            if (p.TryGetComponent<Rigidbody2D>(out var rb)) {
                rb.velocity = projectileSpawn.up * projectileForce;
            }

            anim.Play("Idle", 0, 0);

            timer = Time.time;
        }
    }
}
