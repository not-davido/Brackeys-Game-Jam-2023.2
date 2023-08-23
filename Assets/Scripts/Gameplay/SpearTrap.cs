using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearTrap : MonoBehaviour
{
    [SerializeField] BoxCollider2D[] colliders;
    [SerializeField] float interval = 2;
    [SerializeField] bool isStartingSpearOut;

    [Tooltip("If player is hit, will they be taken back in a different positon?")]
    [SerializeField] bool hitResetsPosition = true;
    [SerializeField] Transform positionAfterHit;

    Animator anim;
    float timer;
    bool spearOut;

    // Start is called before the first frame update
    void Start()
    {
        colliders = GetComponentsInChildren<BoxCollider2D>();

        foreach (var c in colliders) {
            c.isTrigger = true;
        }

        anim = GetComponent<Animator>();
        anim.SetBool("isStartingSpearOut", isStartingSpearOut);
        spearOut = isStartingSpearOut;
        anim.SetBool("spearOut", spearOut);
        timer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > timer + interval) {
            spearOut = !spearOut;
            anim.SetBool("spearOut", spearOut);
            timer = Time.time;
        }
    }

    public void SetCollider(int index) {
        int i = 0;

        foreach (var collider in colliders) {
            if (i == index) {
                collider.enabled = true;
            } else {
                collider.enabled = false;
            }

            i++;
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.collider.TryGetComponent(out Health health)) {
    //        health.TakeDamage(1, collision);

    //        if (hitResetsPosition) {
    //            GameManager.Instance.GetPositionAfterDamage(positionAfterHit.position);
    //        }
    //    }
    //}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Health playerHealth)) {
            if (playerHealth.TakeDamage(1, null)) {
                GameManager.Instance.GetPositionAfterDamage(positionAfterHit.position);
            }
        }
    }
}
