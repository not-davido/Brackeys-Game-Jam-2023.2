using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustRainAudio : MonoBehaviour
{
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // Get the first light rain audio source
        audioSource = GetComponent<AudioSource>();

        audioSource.spatialBlend = 1;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = 1;
        audioSource.maxDistance = 35;
    }
}
