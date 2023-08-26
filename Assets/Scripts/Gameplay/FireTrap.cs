using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : Trap
{
    [SerializeField] bool StartOn = true;

    [SerializeField] float inAndOutInterval = 3;

    [SerializeField] GameObject fire;

    Animator anim;
    BoxCollider2D box2d;
    float fireTimer;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        box2d = GetComponent<BoxCollider2D>();

        fire.SetActive(StartOn);

        fireTimer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > fireTimer + inAndOutInterval) {
            fire.SetActive(!fire.activeSelf);
            box2d.enabled = fire.activeSelf;

            if (fire.activeSelf) {
                anim.Play("Fire", 0, 0);
            }

            fireTimer = Time.time;
        }
    }
}
