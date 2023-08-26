using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] AudioClip pickupSfx;

    AudioSource audioSource;
    bool collected;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collected) return;

        if (collision.TryGetComponent(out Player _)) {
            collected = true;
            EventManager.Broadcast(Events.CollectiblePickUpEvent);
            GetComponent<SpriteRenderer>().enabled = false;
            audioSource.PlayOneShot(pickupSfx);
        }
    }
}
