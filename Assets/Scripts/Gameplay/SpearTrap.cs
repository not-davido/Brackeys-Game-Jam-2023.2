using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearTrap : Trap
{
    [SerializeField] BoxCollider2D[] colliders;
    [SerializeField] float interval = 2;
    [SerializeField] bool isStartingSpearOut;

    [SerializeField] AudioClip spearSfx;

    Animator anim;
    AudioSource audioSource;
    float timer;
    bool spearOut;

    // Start is called before the first frame update
    void Start()
    {
        colliders = GetComponentsInChildren<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();

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

    public void PlaySpearSfx() {
        if (audioSource != null)
            audioSource.PlayOneShot(spearSfx);
    }
}
