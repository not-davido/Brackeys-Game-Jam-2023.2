using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    [SerializeField] AudioClip pickupSfx;

    AudioSource audioSource;
    bool collected;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collected) return;

        if (collision.TryGetComponent(out Health playerHealth)) {
            if (playerHealth.Heal(1)) {
                collected = true;
                audioSource.PlayOneShot(pickupSfx);
                GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }
}
